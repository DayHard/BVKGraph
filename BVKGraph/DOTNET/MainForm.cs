using System;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
// ReSharper disable All

////////////////////////////
// Автор: Ланденок Владимир
// Редакция от 11.04.2017
///////////////////////////

namespace DOTNET
{
    public partial class WorkForm : Form
    {
        #region Variables

        GraphPane pane1 = new GraphPane();
        GraphPane pane2 = new GraphPane();
        GraphPane pane3 = new GraphPane();

        MasterPane masterPane = new MasterPane();

        Stopwatch swatch = new Stopwatch();

        // Объявление массивов для работы с данными
        private byte[] command = new byte[3];
        private ushort[] _dataGraph;

        // Статические массивы (для обработки принятых данных)
        private ushort[] _dataGraph2 = new ushort[600000];
        private ushort[] _dataGraph3 = new ushort[600000];
        private ushort[] _dataGraph4 = new ushort[75000];
        private ushort[] _dataGraph5 = new ushort[75000];
        private double[] _dataGraph6Limit = new double[75000];
        private double[] _dataGraph7Limit = new double[75000];
        private double[] _dataGraph8Correction = new double[75000];
        private double[] _dataGraph9Correction = new double[75000];
        private ulong[] timePoint = new ulong[2100000];
        private ulong[] timePoint2 = new ulong[2100000];
        private ulong[] timePoint3 = new ulong[2100000];
        private ulong[] timePoint4 = new ulong[200000];
        private ulong[] timePoint5 = new ulong[200000];
        private ulong[] timePoint6Limit = new ulong[200000];
        private ulong[] timePoint7Limit = new ulong[200000];
        private ulong[] timePoint8Correction = new ulong[75000];
        private ulong[] timePoint9Correction = new ulong[75000];

        // Усреднение 8 кадра
        private double[] _dataGraph10ServiceLimitation = new double[7500];
        private ulong[] timePoint10ServiceLimitation = new ulong[210000];
        private int _dataGraphCounter10ServiceLimitation = 0;
        private double[] _dataGraph11ServiceLimitation = new double[7500];
        private ulong[] timePoint11ServiceLimitation = new ulong[210000];
        private int _dataGraphCounter11ServiceLimitation = 0;

        private byte[] responce = new byte[2100000];
        private byte[] responce_temp = new byte[15000];
        // Объвление констант для  битовых масок
        private const uint MaskFirstByteConst = 0x0F;
        private const uint MaskThirdByteConst = 0x7F;
        private const uint MaskSecondByteConst = 0x7F;
        private const uint MaskResponceConst = 0x70;

        // Размеры шрифтов
        private const int labelsXfontSize = 25;
        private const int labelsYfontSize = 20;
        private const int titleXFontSize = 25;
        private const int titleYFontSize = 20;
        private const int legendFontSize = 15;
        private const int mainTitleFontSize = 30;

        //Диапазоны осей
        private const double XScaleMin = 0;
        private const double XScaleMax = 30000000;
        private const double YScaleMin9 = -9;
        private const double YScaleMax9 = 9;
        private const double YScaleMin17 = -17;
        private const double YScaleMax17 = 17;
        private const double YScaleMin35 = -35;
        private const double YScaleMax35 = 35;

        //Допуск графиков
        private const double admission1 = 4.07D;
        private const double admission2 = 8.15D;
        private const double timeAdmission1 = 0;
        private const double timeAdmission2 = 1;
        private const double timeAdmission3 = 2;
        private const double timeAdmission4 = 35_000_000;

        // Поиск первого бита
        private const uint MaskFirstByteLogicConst = 0x80;
        //Значение угол\азимут
        private const short BadPointConst = 0x7FFF;

        //Объявление вспомогательных переменных
        private int _timer_counter;
        private int read_counter;
        private int possition_counter;
        private int _dataGraphCounter2 = 0, _dataGraphCounter3 = 0, _dataGraphCounter4 = 0,
                    _dataGraphCounter5 = 0, _dataGraphCounter6Limit = 0, _dataGraphCounter7Limit = 0,
                    _dataGraphCounter8Correction = 0, _dataGraphCounter9Correction = 0;
        ulong timebuffer;
        private byte timer30sec = 1;
        private int error_counter = 0;

        //Флаги
        private bool _scanningstatus;
        private bool _syncstatus;
        private bool _limitation = true;
        private bool _engeneering = false;
        private bool _scaleXAxis16OR35 = true;
        private bool _limitation2 = true;
        private bool _sysframe= true;
        private bool _correction = false;
        private bool _service = false;

        //Флаг точек
        private bool measPoint1Flag = true;
        private bool measPoint2Flag = true;
        private double measPointX1 = 0;
        private double measPointX2 = 0;
        private double measPointY1 = 0;
        private double measPointY2 = 0;
        CurveItem curveToDelete1;
        CurveItem curveToDelete2;

        //Отображение амплитуды
        private bool _amplShow = false;

        //Таймаут потока
        private const int ThreadTimeOut = 3000;

        #endregion

        public WorkForm()
        {
            InitializeComponent();
            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "Initialization done!";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            combobox_prop();
            combobox2_prop();
            button_prop();
            button2_prop();
            graph_prop();
            serialPort.DataReceived += SerialPortDataReceived;
            serialPort.ErrorReceived += SerialPortErrorReceived;
            serialPort2.DataReceived += serialPort2_DataReceived;
            // Будем обрабатывать событие PointValueEvent, чтобы изменить формат представления координат
            zedGraph.PointValueEvent += new ZedGraphControl.PointValueHandler(zedGraph_PointValueEvent);
        }

        #region Events
        //Событие всплывающей подсказки
        string zedGraph_PointValueEvent(ZedGraphControl sender,GraphPane pane,CurveItem curve,int iPt)
        {
            // Получим точку, около которой находимся
            PointPair point = curve[iPt];

            // Сформируем строку F1 было
            string result = string.Format("X: {1}\nY: {0:F2}", point.Y, ((double)point.X) / 1000000);

            System.Threading.Thread.Sleep(100);
            return result;
        }
        // Ctrl + Shift + F12 Показать кнопку лимитация 2, по дефолту включена
        private void WorkForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F12)
            {
                gbLimitation2.Visible = !gbLimitation2.Visible;
            }
        }
        #endregion

        #region SerialPort

        // Событие получение ошибки
        void SerialPortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            SerialError sr = e.EventType;
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {

                    switch (sr)
                    {
                        case SerialError.Frame:
                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + " the hardware detected a framing error";
                            break;
                        case SerialError.Overrun:

                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + " a character-buffer overrun has occurred. The next character is lost";
                            break;
                        case SerialError.RXOver:
                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + " an input buffer overflow has occured."
                            + " There is either no room in the input buffer, or a character was received after the End-Of-File (EOF) character.\n";
                            break;
                        case SerialError.RXParity:
                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + " the hardware detected a parity error.\n";
                            break;
                        case SerialError.TXFull:
                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + "the application tried to transmit a character,"
                                + " but the output buffer was full.";
                            break;
                        default:
                            error_TextBox.Text = " [" + System.DateTime.Now + "] " + "On port " + serialPort.PortName + " an unknown error occurred.\n";
                            break;
                    }
                });
            }

        }
        // Событие получение данных
        void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_scanningstatus && serialPort.IsOpen)
                {
                    read_counter = serialPort.Read(responce, possition_counter, 1000);
                    swatch.Stop();
                    possition_counter += read_counter;
                    _timer_counter += read_counter;
                }
                else
                {
                    serialPort.Read(responce_temp, 0, 2000);
                    serialPort.DiscardInBuffer();
                }
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }
        //Настройка параметров Combobox
        private void combobox_prop()
        {
            try
            {
                baudrate_Combobox.SelectedIndex = 6;
                stopbits_Combobox.SelectedIndex = 0;
                databits_Combobox.SelectedIndex = 3;
                Array parities = Enum.GetValues(typeof(Parity));
                foreach (Parity p in parities) parity_Combobox.Items.Add(p);
                parity_Combobox.SelectedIndex = 2;
                port_Combobox.Items.AddRange(SerialPort.GetPortNames());
                if (port_Combobox.Items.Count != 0)
                    port_Combobox.SelectedIndex = port_Combobox.SelectionStart;
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }

        }
        //Настройка  и подключение к COM
        private void connect_Button_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort.PortName = port_Combobox.Text;
                serialPort.BaudRate = int.Parse(baudrate_Combobox.Text);
                serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), parity_Combobox.Text);
                serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopbits_Combobox.Text);
                serialPort.DataBits = int.Parse(databits_Combobox.Text);
                serialPort.ReceivedBytesThreshold = 1000;
                //!!!!!
                serialPort.ReadBufferSize = 300000;
                serialPort.Open();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + " Connected to COM1";
                button_prop();
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }

        }
        //Отключение от COM
        private void disconnect_Button_Click(object sender, EventArgs e)
        {
            try
            {
                _scanningstatus = false;
                dataResiveProgressBar.Visible = false;
                Thread.Sleep(10);
                serialPort.Close();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + " Disconnected from COM1";
                button_prop();
                if (serialPort.IsOpen)
                {
                    _scanningstatus = false;
                    dataResiveProgressBar.Value = 1;
                }
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }

        #endregion

        #region SerialPort2
        private void connect2_button_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.PortName = port2_Combobox.Text;
                serialPort2.BaudRate = int.Parse(baudrate2_Combobox.Text);
                serialPort2.Parity = (Parity)Enum.Parse(typeof(Parity), parity2_Combobox.Text);
                serialPort2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopbits2_Combobox.Text);
                serialPort2.DataBits = int.Parse(databits2_Combobox.Text);
                serialPort2.ReceivedBytesThreshold = 1;
                serialPort2.Open();
                button2_prop();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + " Connected to COM2";
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }

        private void disconnect2_button_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.Close();
                button2_prop();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + " Disconnected from COM2";
                _syncstatus = false;
                Sync_checkBox.Text = "   Sync Off  ";
                syncstatus_label.Text = "Sync: No";
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }

        private void combobox2_prop()
        {
            try
            {
                baudrate2_Combobox.SelectedIndex = 3;
                stopbits2_Combobox.SelectedIndex = 0;
                databits2_Combobox.SelectedIndex = 3;
                Array parities = Enum.GetValues(typeof(Parity));
                foreach (Parity p in parities) parity2_Combobox.Items.Add(p);
                parity2_Combobox.SelectedIndex = 0;
                port2_Combobox.Items.AddRange(SerialPort.GetPortNames());
                if (port2_Combobox.Items.Count != 0)
                    port2_Combobox.SelectedIndex = port_Combobox.SelectionStart;
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }

        }

        private void button2_prop()
        {
            if (serialPort2.IsOpen)
            {
                connect2_button.Enabled = false;
                disconnect2_button.Enabled = true;
                port2_Combobox.Enabled = false;
                baudrate2_Combobox.Enabled = false;
                parity2_Combobox.Enabled = false;
                stopbits2_Combobox.Enabled = false;
                databits2_Combobox.Enabled = false;
                comstatus2_label.Text = "COM2 status: Connected";
                Sync_checkBox.Enabled = true;
            }
            else
            {
                connect2_button.Enabled = true;
                disconnect2_button.Enabled = false;
                port2_Combobox.Enabled = true;
                baudrate2_Combobox.Enabled = true;
                parity2_Combobox.Enabled = true;
                stopbits2_Combobox.Enabled = true;
                databits2_Combobox.Enabled = true;
                comstatus2_label.Text = "COM2 status: Disconnected";
                Sync_checkBox.Enabled = false;
            }
        }
        // Обработчик событий
        void serialPort2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_syncstatus && serialPort.IsOpen)
            {
                Invoke((MethodInvoker)delegate
                {
                    dataResiveProgressBar.Visible = true;
                    derivation_button.Enabled = false;
                    starttest_button.Enabled = false;
                    startbvk_button.Enabled = false;
                });
                swatch.Start();
                _scanningstatus = true;
                serialPort2.DiscardInBuffer();
            }
        }

        #endregion

        #region Buttons

        // Настройка свойств кнопок
        private void button_prop()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    connect_Button.Enabled = false;
                    disconnect_Button.Enabled = true;
                    port_Combobox.Enabled = false;
                    baudrate_Combobox.Enabled = false;
                    parity_Combobox.Enabled = false;
                    stopbits_Combobox.Enabled = false;
                    databits_Combobox.Enabled = false;
                    comstatus_label.Text = "COM status: Connected";
                    startbvk_button.Enabled = true;
                    stopbvk_button.Enabled = true;
                    starttest_button.Enabled = true;
                    //limitation_button.Enabled = true;
                    derivation_button.Enabled = true;
                    //normalmode_button.Enabled = true;
                    //engineeringmode_button.Enabled = true;
                    //kdo_button.Enabled = true;
                    yac1_button.Enabled = true;
                    yac2_button.Enabled = true;
                }
                else
                {
                    connect_Button.Enabled = true;
                    disconnect_Button.Enabled = false;
                    port_Combobox.Enabled = true;
                    baudrate_Combobox.Enabled = true;
                    parity_Combobox.Enabled = true;
                    stopbits_Combobox.Enabled = true;
                    databits_Combobox.Enabled = true;
                    comstatus_label.Text = "COM status: Disconnected";
                    startbvk_button.Enabled = false;
                    stopbvk_button.Enabled = false;
                    starttest_button.Enabled = false;
                    //limitation_button.Enabled = false;
                    derivation_button.Enabled = false;
                    //normalmode_button.Enabled = false;
                    //engineeringmode_button.Enabled = false;
                    //kdo_button.Enabled = false;
                    yac1_button.Enabled = false;
                    yac2_button.Enabled = false;
                }
                rbtnMeasureGraph1.Enabled = false;
                rbtnMeasureGraph2.Select();
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }
        //Запуск режима работы "Хризантема"
        private void startbvk_button_Click(object sender, EventArgs e)
        {
            if (_syncstatus == false)
            {
                DataReset();
                byte commandprop = 0x1A;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                _scanningstatus = true;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }
            else
            {
                DataReset();
                byte commandprop = 0x1A;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }

            // Настройки цветов кнопок
            startbvk_button.BackColor = Color.LightCoral;
            stopbvk_button.UseVisualStyleBackColor = true;
            derivation_button.UseVisualStyleBackColor = true;
            //starttest_button.UseVisualStyleBackColor = true;
        }
        // Формирование пакета данных
        private void DataSend(byte commandprop)
        {
            try
            {
                command[0] = 0xAF;
                command[1] = commandprop;
                command[2] = Convert.ToByte(0xAF ^ commandprop);
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }
        // Остановка БВК
        private void stopbvk_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x1C;
            DataSend(commandprop);
            serialPort.Write(command, 0, 3);
            dataResiveProgressBar.Visible = false;
            derivation_button.Enabled = true;
            starttest_button.Enabled = true;
            startbvk_button.Enabled = true;
            dataResiveProgressBar.Value = 1;
            error_counter_label.Text = "Errors: 0";
            if (_scanningstatus)
                DataProcessing();
            _scanningstatus = false;

            // Настройки цветов кнопок
            stopbvk_button.BackColor = Color.LightCoral;
            startbvk_button.UseVisualStyleBackColor = true;
            derivation_button.UseVisualStyleBackColor = true;
            //starttest_button.UseVisualStyleBackColor = true;
        }
        // Отключить\Включить режим ограничения по уровню 0,5
        private void limitation_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x17;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
            }

            _limitation = !_limitation;
            _correction = false;

            if (limitation_button.UseVisualStyleBackColor == true)
                limitation_button.BackColor = Color.Khaki;
            else limitation_button.UseVisualStyleBackColor = true;
        }
        //Запуск режима Деривации (БКВ должен быть остановлен)
        private void derivation_button_Click(object sender, EventArgs e)
        {
            if (_syncstatus == false)
            {
                DataReset();
                byte commandprop = 0x1B;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                _scanningstatus = true;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }
            else
            {
                DataReset();
                byte commandprop = 0x1B;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }

            // Настройки цветов кнопок
            derivation_button.BackColor = Color.LightCoral;
            stopbvk_button.UseVisualStyleBackColor = true;
            startbvk_button.UseVisualStyleBackColor = true;
            //starttest_button.UseVisualStyleBackColor = true;
        }
        // Запуск тестового режима
        private void starttest_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x14;
            DataSend(commandprop);
            serialPort.Write(command, 0, 3);
            starttest_button.BackColor = Color.Khaki;
        }
        //Нормальный режим
        private void normalmode_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x11;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
            }

            _service = false;
            _correction = false;
            // Нужна для перехода в режим коррекции
            _limitation2 = true;

            normalmode_button.BackColor = Color.YellowGreen;
            btnServiceMode.UseVisualStyleBackColor = true;
            kdo_button.UseVisualStyleBackColor = true;
            starttest_button.UseVisualStyleBackColor = true;
            _limitation = true;
            limitation_button.BackColor = Color.Khaki;
        }
        // Инженерный режим
        private void engineeringmode_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x12;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
            }
                _engeneering = !_engeneering;

            if (engineeringmode_button.UseVisualStyleBackColor == true)
                engineeringmode_button.BackColor = Color.Khaki;
            else engineeringmode_button.UseVisualStyleBackColor = true;
        }
        // Режим КДО
        private void kdo_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x13;
                DataSend(commandprop);
                serialPort.Write(command, 0, 3);
            }

            _limitation2 = false;
            _service = false;
            _correction = true;
            normalmode_button.UseVisualStyleBackColor = true;
            btnServiceMode.UseVisualStyleBackColor = true;
            starttest_button.UseVisualStyleBackColor = true;
            kdo_button.BackColor = Color.YellowGreen;
            _limitation = true;
            limitation_button.BackColor = Color.Khaki;
        }
        // Выбор УАС-1
        private void yac1_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x15;
            DataSend(commandprop);
            serialPort.Write(command, 0, 3);

            yac1_button.BackColor = Color.PaleTurquoise;
            yac2_button.UseVisualStyleBackColor = true;
        }
        // Выбор УАС-2
        private void yac2_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x16;
            DataSend(commandprop);
            serialPort.Write(command, 0, 3);

            yac1_button.UseVisualStyleBackColor = true;
            yac2_button.BackColor = Color.PaleTurquoise;
        }
        // Сохранения данных для графиков
        private void save_button_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Все документы (*.bin) | *.bin";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter binaryWriter = new BinaryWriter(File.Open(saveFileDialog.FileName, FileMode.OpenOrCreate));
                binaryWriter.Write(responce, 0, possition_counter);
                binaryWriter.Close();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + "File saved Data.bin";
            }
        }
        // Загрузка данных для графиков
        private void load_button_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Все документы (*.bin) | *.bin";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                DataReset();
                BinaryReader binaryReader = new BinaryReader(File.Open(openFileDialog.FileName, FileMode.OpenOrCreate));
                binaryReader.Read(responce, 0, (int)binaryReader.BaseStream.Length);
                possition_counter = (int)binaryReader.BaseStream.Length;
                binaryReader.Close();
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + "File open " + openFileDialog.SafeFileName;
                DataProcessing();
            }
        }
        // Очистка Textbox от ошибок
        private void clean_error_linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            error_TextBox.Clear();
        }
        // Запуск по сигналу с COM2
        private void Sync_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_syncstatus)
                {
                    _syncstatus = false;
                    Sync_checkBox.Text = " Вкл. синхр. ";
                    syncstatus_label.Text = "Sync: No";
                    Sync_checkBox.UseVisualStyleBackColor = true;
                }
                else
                {
                    byte commandprop = 0x1B;
                    DataSend(commandprop);
                    serialPort.Write(command, 0, 3);
                    _syncstatus = true;
                    Sync_checkBox.Text = "Откл.синхр.";
                    syncstatus_label.Text = "Sync: Yes";
                    Sync_checkBox.BackColor = Color.SkyBlue;
                }
            }
            catch (Exception ex)
            {
                error_TextBox.Text = "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }
        }
        //Кнопка показать\отключить  график амплитуда
        private void btnShowAmpl_Click(object sender, EventArgs e)
        {
            if (_amplShow)
            {
                btnShowAmpl.Text = "Показать";
                _amplShow = false;
                masterPane.PaneList.Clear();
                Thread thrd1 = new Thread(GraphDraw);
                thrd1.Start();
                thrd1.Join(ThreadTimeOut);
                rbtnMeasureGraph1.Enabled = false;
                rbtnMeasureGraph2.Select();
            }
            else
            {
                btnShowAmpl.Text = "Скрыть";
                _amplShow = true;
                masterPane.PaneList.Clear();
                Thread thrd2 = new Thread(GraphDraw);
                thrd2.Start();
                thrd2.Join(ThreadTimeOut);
                rbtnMeasureGraph1.Enabled = true;
            }
        }
        //Кнопка перезагрузки графиков
        private void btnResetGraph_Click(object sender, EventArgs e)
        {
            // Если есть что удалять
            if (pane1.CurveList.Count > 0 && pane2.CurveList.Count > 0 && pane3.CurveList.Count > 0)
            {
                // Удалим все кривые
                pane1.CurveList.Clear();
                pane2.CurveList.Clear();
                pane3.CurveList.Clear();
                // Обновим график
                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }

            Thread thrd = new Thread(GraphDraw);
            thrd.Start();
            thrd.Join(ThreadTimeOut);
        }
        //Масштаб графика +-9
        private void btnScale9_Click(object sender, EventArgs e)
        {
            pane2.YAxis.Scale.Min = YScaleMin9;
            pane3.YAxis.Scale.Min = YScaleMin9;

            pane2.YAxis.Scale.Max = YScaleMax9;
            pane3.YAxis.Scale.Max = YScaleMax9;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        //Изменение масштаба YAxis -35 + 35
        private void btnScale35_Click(object sender, EventArgs e)
        {
            _scaleXAxis16OR35 = false;
            pane2.YAxis.Scale.Min = YScaleMin35;
            pane3.YAxis.Scale.Min = YScaleMin35;

            pane2.YAxis.Scale.Max = YScaleMax35;
            pane3.YAxis.Scale.Max = YScaleMax35;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        //Изменение масштаба YAxis -17 + 17
        private void btnScale17_Click(object sender, EventArgs e)
        {
            _scaleXAxis16OR35 = true;
            pane2.YAxis.Scale.Min = YScaleMin17;
            pane3.YAxis.Scale.Min = YScaleMin17;

            pane2.YAxis.Scale.Max = YScaleMax17;
            pane3.YAxis.Scale.Max = YScaleMax17;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        // Лимитация 2
        private void btnLimitation2_Click(object sender, EventArgs e)
        {
            if (_limitation2)
            {
                _limitation2 = false;
                btnLimitation2.Text = "Enable";
            }
            else
            {
                _limitation2 = true;
                btnLimitation2.Text = "Disable";
            }

        }
        //Отключение показа системного кадра
        private void btnChangeFrame_Click(object sender, EventArgs e)
        {
            if (_sysframe)
            {
                _sysframe = false;
                btnChangeFrame.Text = "Enable";
            }
            else
            {
                _sysframe = true;
                btnChangeFrame.Text = "Disable";
            }
        }
        // Режим отображения служебных данных
        private void btnServiceMode_Click(object sender, EventArgs e)
        {
            _service = true;

            normalmode_button.UseVisualStyleBackColor = true;
            kdo_button.UseVisualStyleBackColor = true;
            btnServiceMode.BackColor = Color.YellowGreen;

            _limitation = false;
            limitation_button.UseVisualStyleBackColor = true;
        }

        #endregion

        #region Graphs

        // ! Формирование графика
        private void GraphDraw()
        {
            try
            {
                masterPane = zedGraph.MasterPane;

                // !!! Свойства IsSynchronizeXAxes и IsSynchronizeYAxes указывают, что
                // оси на графиках должны перемещаться и масштабироваться одновременно.
                zedGraph.IsSynchronizeXAxes = true;
                // !!! Показывать значение точек по умолчанию включено
                zedGraph.IsShowPointValues = true;

                // По умолчанию в MasterPane содержится один экземпляр класса GraphPane 
                // (который можно получить из свойства zedGraph.GraphPane)
                // Очистим этот список, так как потом мы будем создавать графики вручную
                masterPane.PaneList.Clear();
                try
                {
                    // Заполнение графика данными, 
                    // поэтому вынесем заполнение точек в отдельный метод DrawSingleGraph()
                    Graph1(pane1);
                    Graph2(pane2);
                    Graph3(pane3);

                }
                catch 
                {
                    MessageBox.Show("Nothing to draw.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                // Добавим новый график в MasterPane
                if (_amplShow)
                masterPane.Add(pane1);
                masterPane.Add(pane2);
                masterPane.Add(pane3);
                

                // Будем размещать добавленные графики в MasterPane
                using (Graphics g = CreateGraphics())
                {
                    // Графики будут размещены в один столбец друг под другом
                    masterPane.SetLayout(g, PaneLayout.SingleColumn);
                }

                // Настройка свойств графиков
                graph_prop_final(pane1, pane2, pane3);

                if (_scaleXAxis16OR35)
                    btnScale17_Click(null, null);
                else
                    btnScale35_Click(null ,null);
                

                // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
                zedGraph.AxisChange();

                // Обновляем график
                zedGraph.Invalidate();

            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                {
                        error_TextBox.Text = "\r\n [" + System.DateTime.Now + "]" + ex.Message;

                }
                ));
            }

        }
        // График 1
        private void Graph1(GraphPane pane1)
        {

            // Создадим список точек=> График номер 1
            PointPairList list1 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph.Length - 1; i++)
                {
                    if(_dataGraph[i] > 1000)
                    list1.Add(timePoint[i], (Convert.ToDouble(_dataGraph[i])) - 1100);// / 10);
                }
            }

            ////
            // Создадим кривые 
            LineItem curve1 = pane1.AddCurve("", list1, Color.Blue, SymbolType.Circle);
            // У кривой линия будет невидимой
            curve1.Line.IsVisible = true;
            // Цвет заполнения отметок (ромбиков) - голубой
            curve1.Symbol.Fill.Color = Color.Blue;
            // Тип заполнения - сплошная заливка
            curve1.Symbol.Fill.Type = FillType.Solid;
            // Размер ромбиков
            curve1.Symbol.Size = 7;
            // Линия невидимая
            curve1.Line.IsVisible = true;
            // !!!
            // Указываем, что расположение легенды мы будет задавать 
            // в виде координат левого верхнего угла
            pane1.Legend.Position = LegendPos.InsideBotRight;

            // Координаты будут отсчитываться в системе координат окна графика
            pane1.Legend.Location.CoordinateFrame = CoordType.ChartFraction;

            // Задаем выравнивание, относительно которого мы будем задавать координаты
            // В данном случае мы будем располагать легенду справа внизу
            pane1.Legend.Location.AlignH = AlignH.Right;
            pane1.Legend.Location.AlignV = AlignV.Bottom;

            // Задаем координаты легенды 
            // Вычитаем 0.02f, чтобы был небольшой зазор между осями и легендой
            pane1.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 1.0f - 0.02f);

            pane1.Legend.FontSpec.Size = 7;

        }
        // График 2
        private void Graph2(GraphPane pane2)
        {

            // Создадим список точек=> График номер 2
            PointPairList list2 = new PointPairList();
            {
                for (int i = 0; i < _dataGraphCounter2; i++)
                {
                    // Отнимаем значение 33 для удобства отображения
                    list2.Add(timePoint2[i], _dataGraph2[i] - 32);
                }
            }
            //Лимитация в режиме служебный(8 кадр)
            // !!! Считает не все точки, а только точки с интервалом менее 80 мкс
            PointPairList list4 = new PointPairList();
            {
                //// Служебный кадр
                ////Без расчета среднего значения

                //for (int i = 0; i < _dataGraphCounter4; i++)
                //{
                //    // Отнимаем значение 33 для удобства отображения
                //    list4.Add(timePoint4[i], _dataGraph4[i] - 32);
                //}

                double dataTemp = 0;
                int counter = 0;
                for (int i = 0; i < _dataGraphCounter4; i++)
                {
                    if ((timePoint4[i + 1] - timePoint4[i]) <= 80)
                    {
                        dataTemp += _dataGraph4[i];
                        counter++;
                    }
                    else
                    {
                        if (counter != 0)
                        {
                            var temp = (dataTemp / counter);
                            _dataGraph11ServiceLimitation[_dataGraphCounter11ServiceLimitation] = (dataTemp / counter);
                        }
                        else
                        {
                            _dataGraph11ServiceLimitation[_dataGraphCounter11ServiceLimitation] = BadPointConst;
                        }
                        timePoint11ServiceLimitation[_dataGraphCounter11ServiceLimitation] = timePoint4[i];
                        _dataGraphCounter11ServiceLimitation++;

                        counter = 0;
                        dataTemp = 0;
                    }
                }

                for (int i = 0; i < _dataGraphCounter11ServiceLimitation; i++)
                {
                    // Отнимаем значение 33 для удобства отображения
                    if (_dataGraph11ServiceLimitation[i] != BadPointConst)
                    {
                        list4.Add(timePoint11ServiceLimitation[i], _dataGraph11ServiceLimitation[i] - 32);
                    }
                }
            }
            PointPairList list6 = new PointPairList();
            {
                // Лимитация по 0.5 график 1
                if (_limitation2)
                {
                    for (int i = 0; i < _dataGraphCounter6Limit - 1; i += 2)
                    {
                        if (_dataGraph6Limit[i] != BadPointConst && timePoint6Limit[i] != 0
                            && _dataGraph6Limit[i + 1] != BadPointConst && timePoint6Limit[i + 1] != 0)
                        {
                            // Отнимаем значение 33 для удобства отображения
                            list6.Add(timePoint6Limit[i], _dataGraph6Limit[i] - 32);
                        }
                        else i--;
                    }
                }
                else
                {
                    // Считает 3 точки подрят
                    if (_correction)
                    {
                        for (int i = 0; i < _dataGraphCounter8Correction; i++)
                        {
                                list6.Add(timePoint8Correction[i], _dataGraph8Correction[i] - 32);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _dataGraphCounter6Limit - 1; i++)
                        {
                            // Отнимаем значение 33 для удобства отображения
                            list6.Add(timePoint6Limit[i], _dataGraph6Limit[i] - 32);
                        }
                    }
                }
            }
            //Лист точек для допуска
            PointPairList list8 = new PointPairList();
            {

                list8.Add(35_000_000, 8.15D);
                list8.Add(2_000_000, 8.15D);
                list8.Add(1_000_000, 4.07D);
                list8.Add(0, 4.07D);
                list8.Add(0, -4.07D);
                list8.Add(1_000_000, -4.07D);
                list8.Add(2_000_000, -8.15D);
                list8.Add(35_000_000, -8.15D);
            }
            // Создадим кривые 
            if (_engeneering == true)
            {
                LineItem curve2 = pane2.AddCurve("", list2, Color.Green, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve2.Symbol.Fill.Color = Color.Green;
                // Тип заполнения - сплошная заливка
                curve2.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve2.Symbol.Size = 7;
                // Линия невидимая
                curve2.Line.IsVisible = false;
            }
            //Служебный режим где виден только служебный кадр
            if (_service)
            {
                LineItem curve4 = pane2.AddCurve("", list4, Color.BlueViolet, SymbolType.Diamond);

                // Цвет заполнения отметок (ромбиков) - голубой
                curve4.Symbol.Fill.Color = Color.BlueViolet;
                // Тип заполнения - сплошная заливка
                curve4.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve4.Symbol.Size = 7;
                // Линия невидимая
                curve4.Line.IsVisible = false;
            }
            // Допуск
            if (cbAdmission.Checked)
            {
                LineItem curve8 = pane2.AddCurve("", list8, Color.Red, SymbolType.Diamond);
                // Размер ромбиков
                curve8.Symbol.Size = 1;
                // Линия невидимая
                curve8.Line.IsVisible = true;
            }

            // Лимитация
            if (_limitation)
            {
                LineItem curve6 = pane2.AddCurve("", list6, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve6.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                curve6.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve6.Symbol.Size = 7;
                // Линия невидимая
                curve6.Line.IsVisible = true;
            }
        }
        //График 3
        private void Graph3(GraphPane pane3)
        {

            // Создадим список точек=> График номер 3
            PointPairList list3 = new PointPairList();
            {
                for (int i = 0; i < _dataGraphCounter3; i++)
                {
                    // Отнимаем значение 33 для удобства отображения
                    list3.Add(timePoint3[i], _dataGraph3[i] - 32);
                }
            }
            // 8 кадр список точек
            PointPairList list5 = new PointPairList();
            {
                // Служебный кадр
                //Без расчета среднего значения

                //for (int i = 0; i < _dataGraphCounter5; i++)
                //{
                //    // Отнимаем значение 33 для удобства отображения
                //    list5.Add(timePoint5[i], _dataGraph5[i] - 32);
                //}

                double dataTemp = 0;
                int counter = 0;
                for (int i = 0; i < _dataGraphCounter5; i++)
                {
                    if ((timePoint5[i + 1] - timePoint5[i]) <= 80)
                    {
                        dataTemp += _dataGraph5[i];
                        counter++;
                    }
                    else
                    {
                        if (counter !=0)
                        {
                            var temp = (dataTemp / counter);
                            _dataGraph10ServiceLimitation[_dataGraphCounter10ServiceLimitation] = (dataTemp / counter);
                        }
                        else
                        {
                            _dataGraph10ServiceLimitation[_dataGraphCounter10ServiceLimitation] = BadPointConst;
                        }
                        timePoint10ServiceLimitation[_dataGraphCounter10ServiceLimitation] = timePoint5[i];
                        _dataGraphCounter10ServiceLimitation++;

                        counter = 0;
                        dataTemp = 0;
                    }
                }

                for (int i = 0; i < _dataGraphCounter10ServiceLimitation; i++)
                {                  
                    // Отнимаем значение 33 для удобства отображения
                    if (_dataGraph10ServiceLimitation[i] != BadPointConst)
                    {
                        list5.Add(timePoint10ServiceLimitation[i], _dataGraph10ServiceLimitation[i] - 32);                        
                    }
                }
            }
            //Лист точек для допуска
            PointPairList list9 = new PointPairList();
            {
                list9.Add(35_000_000, 8.15D);
                list9.Add(2_000_000, 8.15D);
                list9.Add(1_000_000, 4.07D);
                list9.Add(0, 4.07D);
                list9.Add(0, -4.07D);
                list9.Add(1_000_000, -4.07D);
                list9.Add(2_000_000, -8.15D);
                list9.Add(35_000_000, -8.15D);
            }
            if (_engeneering == true)
            {
                // Создадим кривые 
                LineItem curve3 = pane3.AddCurve("", list3, Color.Green, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve3.Symbol.Fill.Color = Color.Green;
                // Тип заполнения - сплошная заливка
                curve3.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve3.Symbol.Size = 7;
                // Линия невидимая
                curve3.Line.IsVisible = false;
            }
            //Служебный режим)
            if (_service == true)
            {
                // Создадим кривые 
                LineItem curve5 = pane3.AddCurve("", list5, Color.BlueViolet, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve5.Symbol.Fill.Color = Color.BlueViolet;
                // Тип заполнения - сплошная заливка
                curve5.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve5.Symbol.Size = 7;
                // Линия невидимая
                curve5.Line.IsVisible = false;
            }
            // Допуск
            if (cbAdmission.Checked)
            {
                LineItem curve9 = pane3.AddCurve("", list9, Color.Red, SymbolType.Diamond);
                // Размер ромбиков
                curve9.Symbol.Size = 1;
                // Линия невидимая
                curve9.Line.IsVisible = true;
            }

            PointPairList list7 = new PointPairList();
            {
                // Лимитация по 0.5 график 1
                if (_limitation2)
                {

                    for (int i = 0; i < _dataGraphCounter7Limit - 1; i += 2)
                    {
                        if (_dataGraph7Limit[i] != BadPointConst && timePoint7Limit[i] != 0
                            && _dataGraph7Limit[i + 1] != BadPointConst && timePoint7Limit[i + 1] != 0)
                        {
                            // Отнимаем значение 33 для удобства отображения
                            list7.Add(timePoint7Limit[i], _dataGraph7Limit[i] - 32);
                        }
                        else i--;
                    }
                }
                else
                {
                    if (_correction)
                    {
                        for (int i = 0; i < _dataGraphCounter9Correction; i++)
                        {
                            list7.Add(timePoint9Correction[i], _dataGraph9Correction[i] - 32);
                        }
                    }
                    else
                    {
                         for (int i = 0; i < _dataGraphCounter7Limit - 1; i++)
                        {
                            // Отнимаем значение 33 для удобства отображения
                            list7.Add(timePoint7Limit[i], _dataGraph7Limit[i] - 32);
                        }
                    }
                }
            }
            if (_limitation)
            {
                LineItem curve7 = pane3.AddCurve("", list7, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                //curve3.Symbol.Fill.Color = Color.Red;
                //curve5.Symbol.Fill.Color = Color.Brown;
                curve7.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                //curve3.Symbol.Fill.Type = FillType.Solid;
                //curve5.Symbol.Fill.Type = FillType.Solid;
                curve7.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                //curve3.Symbol.Size = 7;
                //curve5.Symbol.Size = 7;
                curve7.Symbol.Size = 7;
                // Линия невидимая
                //curve3.Line.IsVisible = false;
                //curve5.Line.IsVisible = false;
                curve7.Line.IsVisible = true;
            }
        }
        // Очистка графика
        private void cleangraph_button_Click(object sender, EventArgs e)
        {
            DataReset();
        }
        // Настройка параметров ZedGraph при инициализации
        public void graph_prop()
        {
            ZedGraph.MasterPane masterPane = zedGraph.MasterPane;
            zedGraph.IsSynchronizeXAxes = true;
            //zedGraph.IsSynchronizeYAxes = true;
            masterPane.PaneList.Clear();
            GraphPane pane1 = new GraphPane();
            GraphPane pane2 = new GraphPane();
            GraphPane pane3 = new GraphPane();

            if (_amplShow)
                masterPane.Add(pane1);
            masterPane.Add(pane2);
            masterPane.Add(pane3);

            pane1.Title.FontSpec.FontColor = Color.Blue;
            pane2.Title.FontSpec.FontColor = Color.Green;
            pane3.Title.FontSpec.FontColor = Color.Green;

            pane1.YAxis.Title.Text = " \r\n Мощность";
            pane2.YAxis.Title.Text = "Горизонт \r\n Код. Задержка";
            pane3.YAxis.Title.Text = "Вертикаль \r\n Крен";
            // Параметры шрифтов для графика 1
            // Установим размеры шрифтов для меток вдоль осей
            pane1.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane1.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane1.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane1.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane1.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane1.Title.FontSpec.Size = mainTitleFontSize;
            pane1.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 2
            // Установим размеры шрифтов для меток вдоль осей
            pane2.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane2.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane2.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane2.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane2.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane2.Title.FontSpec.Size = mainTitleFontSize;
            pane2.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 3
            // Установим размеры шрифтов для меток вдоль осей
            pane3.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane3.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane3.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane3.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane3.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane3.Title.FontSpec.Size = mainTitleFontSize;
            pane3.Title.FontSpec.IsUnderline = true;


            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.SingleColumn);
            }
            //Настройка прогресс бара
            dataResiveProgressBar.Minimum = 1;
            dataResiveProgressBar.Maximum = 30;
            dataResiveProgressBar.Step = 1;

            //Сетка график 1 крупная
            pane1.XAxis.MajorGrid.IsVisible = true;
            pane1.XAxis.MajorGrid.DashOn = 10;
            pane1.XAxis.MajorGrid.DashOff = 5;
            pane1.YAxis.MajorGrid.IsVisible = true;
            pane1.YAxis.MajorGrid.DashOn = 10;
            pane1.YAxis.MajorGrid.DashOff = 5;
            // Сетка график 2 крупная
            pane2.XAxis.MajorGrid.IsVisible = true;
            pane2.XAxis.MajorGrid.DashOn = 10;
            pane2.XAxis.MajorGrid.DashOff = 5;
            pane2.YAxis.MajorGrid.IsVisible = true;
            pane2.YAxis.MajorGrid.DashOn = 10;
            pane2.YAxis.MajorGrid.DashOff = 5;
            // Сетка график 3 крупная
            pane3.XAxis.MajorGrid.IsVisible = true;
            pane3.XAxis.MajorGrid.DashOn = 10;
            pane3.XAxis.MajorGrid.DashOff = 5;
            pane3.YAxis.MajorGrid.IsVisible = true;
            pane3.YAxis.MajorGrid.DashOn = 10;
            pane3.YAxis.MajorGrid.DashOff = 5;

            //Сетка график 1 мелкая
            pane1.XAxis.MinorGrid.IsVisible = true;
            pane1.XAxis.MinorGrid.DashOn = 1;
            pane1.XAxis.MinorGrid.DashOff = 2;
            pane1.YAxis.MinorGrid.IsVisible = true;
            pane1.YAxis.MinorGrid.DashOn = 1;
            pane1.YAxis.MinorGrid.DashOff = 2;
            // Сетка график 2 мелкая
            pane2.XAxis.MinorGrid.IsVisible = true;
            pane2.XAxis.MinorGrid.DashOn = 1;
            pane2.XAxis.MinorGrid.DashOff = 2;
            pane2.YAxis.MinorGrid.IsVisible = true;
            pane2.YAxis.MinorGrid.DashOn = 1;
            pane2.YAxis.MinorGrid.DashOff = 2;
            // Сетка график 3 мелкая
            pane3.XAxis.MinorGrid.IsVisible = true;
            pane3.XAxis.MinorGrid.DashOn = 1;
            pane3.XAxis.MinorGrid.DashOff = 2;
            pane3.YAxis.MinorGrid.IsVisible = true;
            pane3.YAxis.MinorGrid.DashOn = 1;
            pane3.YAxis.MinorGrid.DashOff = 2;

            // Синхронизация графиков по масштабам оси Х и Y
            pane1.XAxis.Scale.Min = XScaleMin;
            pane2.XAxis.Scale.Min = XScaleMin;
            pane3.XAxis.Scale.Min = XScaleMin;

            pane1.XAxis.Scale.Max = XScaleMax;
            pane2.XAxis.Scale.Max = XScaleMax;
            pane3.XAxis.Scale.Max = XScaleMax;

            pane2.YAxis.Scale.Min = YScaleMin17;
            pane3.YAxis.Scale.Min = YScaleMin17;

            pane2.YAxis.Scale.Max = YScaleMax17;
            pane3.YAxis.Scale.Max = YScaleMax17;

            //// Подпись по оси X невидимая
            //pane1.XAxis.Title.FontSpec.FontColor = Color.White;
            //pane2.XAxis.Title.FontSpec.FontColor = Color.White;
            //pane3.XAxis.Title.FontSpec.FontColor = Color.White;
            //pane1.XAxis.Title.FontSpec.is

            // Уберем отображение степени в подписи оси X
            pane1.XAxis.Title.IsOmitMag = true;
            pane2.XAxis.Title.IsOmitMag = true;
            pane3.XAxis.Title.IsOmitMag = true;

            //pane1.XAxis.Scale.Mag = 6;
            //pane2.XAxis.Scale.Mag = 6;
            //pane3.XAxis.Scale.Mag = 6;

            zedGraph.Invalidate();
            zedGraph.AxisChange();

        }
        // Настройка параметров ZedGraph после отрисовке графиков
        private void graph_prop_final(GraphPane pane1, GraphPane pane2, GraphPane pane3)
        {
            // Подпись по оси X невидимая
            pane1.XAxis.Title.FontSpec.FontColor = Color.White;
            pane2.XAxis.Title.FontSpec.FontColor = Color.White;
            pane3.XAxis.Title.FontSpec.FontColor = Color.White;

            pane1.Title.FontSpec.FontColor = Color.Blue;
            pane2.Title.FontSpec.FontColor = Color.Green;
            pane3.Title.FontSpec.FontColor = Color.Green;
            pane1.YAxis.Title.Text = " \r\n Мощность";
            pane2.YAxis.Title.Text = "Горизонт \r\n Код. Задержка";
            pane3.YAxis.Title.Text = "Вертикаль \r\n Крен"; ;

            //pane1.YAxis.Title.Text = "Значение Амплитуды";
            //pane2.YAxis.Title.Text = "Значение Азимута\r\n";
            //pane3.YAxis.Title.Text = "Значение Угла\r\n";

            // Параметры шрифтов для графика 1
            // Установим размеры шрифтов для меток вдоль осей
            pane1.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane1.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane1.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane1.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane1.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane1.Title.FontSpec.Size = mainTitleFontSize;
            pane1.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 2
            // Установим размеры шрифтов для меток вдоль осей
            pane2.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane2.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane2.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane2.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane2.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane2.Title.FontSpec.Size = mainTitleFontSize;
            pane2.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 3
            // Установим размеры шрифтов для меток вдоль осей
            pane3.XAxis.Scale.FontSpec.Size = labelsXfontSize;
            pane3.YAxis.Scale.FontSpec.Size = labelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane3.XAxis.Title.FontSpec.Size = titleXFontSize;
            pane3.YAxis.Title.FontSpec.Size = titleYFontSize;

            // Установим размеры шрифта для легенды
            pane3.Legend.FontSpec.Size = legendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane3.Title.FontSpec.Size = mainTitleFontSize;
            pane3.Title.FontSpec.IsUnderline = true;

            ////
            //Сетка график 1 крупная
            pane1.XAxis.MajorGrid.IsVisible = true;
            pane1.XAxis.MajorGrid.DashOn = 10;
            pane1.XAxis.MajorGrid.DashOff = 5;
            pane1.YAxis.MajorGrid.IsVisible = true;
            pane1.YAxis.MajorGrid.DashOn = 10;
            pane1.YAxis.MajorGrid.DashOff = 5;
            // Сетка график 2 крупная
            pane2.XAxis.MajorGrid.IsVisible = true;
            pane2.XAxis.MajorGrid.DashOn = 10;
            pane2.XAxis.MajorGrid.DashOff = 5;
            pane2.YAxis.MajorGrid.IsVisible = true;
            pane2.YAxis.MajorGrid.DashOn = 10;
            pane2.YAxis.MajorGrid.DashOff = 5;
            // Сетка график 3 крупная
            pane3.XAxis.MajorGrid.IsVisible = true;
            pane3.XAxis.MajorGrid.DashOn = 10;
            pane3.XAxis.MajorGrid.DashOff = 5;
            pane3.YAxis.MajorGrid.IsVisible = true;
            pane3.YAxis.MajorGrid.DashOn = 10;
            pane3.YAxis.MajorGrid.DashOff = 5;

            //Сетка график 1 мелкая
            pane1.XAxis.MinorGrid.IsVisible = true;
            pane1.XAxis.MinorGrid.DashOn = 1;
            pane1.XAxis.MinorGrid.DashOff = 2;
            pane1.YAxis.MinorGrid.IsVisible = true;
            pane1.YAxis.MinorGrid.DashOn = 1;
            pane1.YAxis.MinorGrid.DashOff = 2;
            // Сетка график 2 мелкая
            pane2.XAxis.MinorGrid.IsVisible = true;
            pane2.XAxis.MinorGrid.DashOn = 1;
            pane2.XAxis.MinorGrid.DashOff = 2;
            pane2.YAxis.MinorGrid.IsVisible = true;
            pane2.YAxis.MinorGrid.DashOn = 1;
            pane2.YAxis.MinorGrid.DashOff = 2;
            // Сетка график 3 мелкая
            pane3.XAxis.MinorGrid.IsVisible = true;
            pane3.XAxis.MinorGrid.DashOn = 1;
            pane3.XAxis.MinorGrid.DashOff = 2;
            pane3.YAxis.MinorGrid.IsVisible = true;
            pane3.YAxis.MinorGrid.DashOn = 1;
            pane3.YAxis.MinorGrid.DashOff = 2;


        }

        #endregion

        #region Data
        // Метод обработки данных
        private void DataProcessing()
        {
            try
            {
                _scanningstatus = false;
                dataResiveProgressBar.Visible = false;
                TimeProcessing();
                //+1 делается ввиду специфику округления в прграммирование для избежания выхода за пределы массива
                _dataGraph = new ushort[possition_counter / 4 + 1];

                if (possition_counter != 0)
                {
                    // Формирование графиков
                    int j = 0;
                    int n = 0;
                    int k = 0;
                    int z = 0;
                    int u = 0;
                    int c = 0;
                    byte error_packages_counter = 0;
                    for (int i = 0; i < possition_counter; i += 4)
                    {
                        //Проверка стартового бита
                        uint startByteCheck1 = responce[i] & MaskFirstByteLogicConst;
                        uint startByteCheck2 = responce[i + 1] & MaskFirstByteLogicConst;
                        uint startByteCheck3 = responce[i + 2] & MaskFirstByteLogicConst;
                        uint startByteCheck4 = responce[i + 3] & MaskFirstByteLogicConst;

                        if (startByteCheck1 != 0 && startByteCheck2 == 0 && startByteCheck3 == 0 && startByteCheck4 == 0)
                        {
                            _dataGraph[k] =
                                Convert.ToUInt16(((responce[i] & MaskFirstByteConst) << 7) |
                                                 (responce[i + 2] & MaskThirdByteConst));
                            k++;

                            switch (responce[i] & MaskResponceConst)
                            {
                                // 0x0
                                case 0x00:
                                    _dataGraph2[j] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint2[j] = timePoint[z];
                                    _dataGraphCounter2++;
                                    j++;
                                    break;
                                // 0x1
                                case 0x10:
                                    _dataGraph3[n] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint3[n] = timePoint[z];
                                    _dataGraphCounter3++;
                                    n++;
                                    break;
                                //0x2
                                case 0x20:
                                    _dataGraph2[j] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint2[j] = timePoint[z];
                                    _dataGraphCounter2++;
                                    j++;
                                    break;
                                //0x3
                                case 0x30:
                                    _dataGraph3[n] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint3[n] = timePoint[z];
                                    _dataGraphCounter3++;
                                    n++;
                                    break;
                                // 0x4   (Служебный)
                                case 0x40:
                                    _dataGraph4[u] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint4[u] = timePoint[z];
                                    _dataGraphCounter4++;
                                    u++;
                                    break;
                                // 0x5 (Крен)
                                case 0x50:
                                    _dataGraph5[c] = Convert.ToUInt16(responce[i + 1] & MaskSecondByteConst);
                                    timePoint5[c] = timePoint[z];
                                    _dataGraphCounter5++;
                                    c++;
                                    break;
                            }
                            z++;
                        }
                        else
                        {
                            if (error_packages_counter == 4)
                            {
                                _dataGraph[k] = 0;
                                k++;
                                error_packages_counter = 0;
                                error_counter++;
                            }
                            error_packages_counter++;
                            i -= 3;
                            error_counter_label.Text = "Errors:" + error_counter;
                        }
                    }
                    if (_limitation)
                    {
                        DataLimitation();
                    }
                    Thread graphThread = new Thread(GraphDraw);
                    graphThread.Name = "Graph draw thread (open file)";
                    graphThread.IsBackground = true;
                    graphThread.Start();
                    timer30sec = 1;
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                {
                    error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message + ex.StackTrace;
                }
                ));
            }
        }
        // Формировавание данных для лимитации
        private void DataLimitation()
        {
            try
            {
                //Лимит по 0.5 график 1
                ushort pointSum = 0;
                ushort pointCounter = 0;
                for (int i = 0; i < _dataGraphCounter2; i++)
                {
                    if (_dataGraph2[i] == _dataGraph2[i + 1] && _dataGraph2[i] == _dataGraph2[i + 2] && (timePoint2[i + 4] - timePoint2[i + 3])<= 5000)
                    {
                        if (Math.Abs(_dataGraph2[i + 2] - _dataGraph2[i + 3]) > 2)
                        {
                            pointCounter = 0;
                            pointSum = 0;
                        }
                    }
                    else
                    {
                        if ((timePoint2[i + 4] - timePoint2[i + 3]) <= 80 &&
                            _dataGraph2[i + 3] != _dataGraph2[i + 4])
                        {
                            pointSum += _dataGraph2[i + 3];
                            pointCounter++;

                        }
                        else
                        {
                            if ((timePoint2[i + 4] - timePoint2[i + 3]) <= 80 &&
                                _dataGraph2[i + 3] != _dataGraph2[i + 5])
                            {
                                pointSum += _dataGraph2[i + 3];
                                pointCounter++;
                            }
                            else
                            {
                                if (pointCounter != 0)
                                {
                                    _dataGraph6Limit[_dataGraphCounter6Limit] = (double) pointSum/
                                                                                (double) pointCounter;
                                    timePoint6Limit[_dataGraphCounter6Limit] = timePoint2[i + 3];
                                    _dataGraphCounter6Limit++;
                                    pointCounter = 0;
                                    pointSum = 0;
                                }
                            }
                        }
                    }
                }

                //Лимит по 0.5 график 2
                ushort pointSum2 = 0;
                ushort pointCounter2 = 0;
                for (int i = 0; i < _dataGraphCounter3; i++)
                {
                    if (_dataGraph3[i] == _dataGraph3[i + 1] && _dataGraph3[i] == _dataGraph3[i + 2])
                    {
                        if (Math.Abs(_dataGraph3[i + 2] - _dataGraph3[i + 3]) > 2)
                        {
                            pointCounter2 = 0;
                            pointSum2 = 0;
                        }
                    }
                    else
                    {
                        if ((timePoint3[i + 4] - timePoint3[i + 3]) <= 80 && _dataGraph3[i + 3] != _dataGraph3[i + 4])
                        {
                            pointSum2 += _dataGraph3[i + 3];
                            pointCounter2++;

                        }
                        else
                        {
                            if ((timePoint3[i + 4] - timePoint3[i + 3]) <= 80 &&
                                _dataGraph3[i + 3] != _dataGraph3[i + 5])
                            {
                                pointSum2 += _dataGraph3[i + 3];
                                pointCounter2++;
                            }
                            else
                            {
                                if (pointCounter2 != 0)
                                {
                                    _dataGraph7Limit[_dataGraphCounter7Limit] = (double) pointSum2/
                                                                                (double) pointCounter2;
                                    timePoint7Limit[_dataGraphCounter7Limit] = timePoint3[i + 3];
                                    _dataGraphCounter7Limit++;
                                    pointCounter2 = 0;
                                    pointSum2 = 0;
                                }
                            }
                        }
                    }
                }

                if (_limitation2)
                {
                    if (_sysframe)
                    {
                        // Проходим массив, если время между точками больше 20мкс
                        //зануляем время, аплитуде присваиваем значение 0xBB8(3000)
                        for (int i = 0; i < _dataGraphCounter6Limit; i++)
                        {
                            if ((timePoint6Limit[i + 1] - timePoint6Limit[i]) >= 20000)
                            {
                                timePoint6Limit[i] = 0;
                                _dataGraph6Limit[i] = BadPointConst;
                            }
                        }
                        // Проходим массив, если время между точками больше 20мкс
                        //зануляем время, аплитуде присваиваем значение 0xBB8(3000)
                        for (int i = 0; i < _dataGraphCounter7Limit; i++)
                        {
                            if ((timePoint7Limit[i + 1] - timePoint7Limit[i]) >= 20000)
                            {
                                timePoint7Limit[i] = 0;
                                _dataGraph7Limit[i] = BadPointConst;
                            }
                        }
                    }

                    // Фильтрация ошибок в вычисления
                    //График 1
                    for (int i = 0; i < _dataGraphCounter6Limit; i++)
                    {
                        if ((timePoint6Limit[i + 1] - timePoint6Limit[i]) <= 3000)
                        {
                            _dataGraph6Limit[i + 1] = (ushort)BadPointConst;
                        }
                    }

                    // Фильтрация ошибок в вычисления
                    //График 2
                    for (int i = 0; i < _dataGraphCounter7Limit; i++)
                    {
                        if ((timePoint7Limit[i + 1] - timePoint7Limit[i]) <= 3000)
                        {
                            _dataGraph7Limit[i + 1] = (ushort)BadPointConst;
                        }
                    }


                    //Перерасчет усредненной лимитации для графика 1
                    for (int i = 0; i < _dataGraphCounter6Limit; i++)
                    {
                        if (_dataGraph6Limit[i] != BadPointConst)
                        {
                            if (_dataGraph6Limit[i] != BadPointConst && timePoint6Limit[i] != 0 
                                && _dataGraph6Limit[i + 1] != BadPointConst && timePoint6Limit[i + 1] != 0)
                            {
                                _dataGraph6Limit[i] = (_dataGraph6Limit[i] + _dataGraph6Limit[i + 1]) / 2;
                            }
                        }
                    }
                    //Перерасчет усредненной лимитации для графика 2
                    for (int i = 0; i < _dataGraphCounter7Limit; i++)
                    {
                        if (_dataGraph7Limit[i] != BadPointConst)
                        {
                            if (_dataGraph7Limit[i] != BadPointConst && timePoint7Limit[i] != 0
                                && _dataGraph7Limit[i + 1] != BadPointConst && timePoint7Limit[i + 1] != 0)
                            {
                                _dataGraph7Limit[i] = (_dataGraph7Limit[i] + _dataGraph7Limit[i + 1]) / 2;
                            }
                        }
                    }
                }

                //Формирование массивов для режима поправка
                // График 1
                for (int i = 0; i < _dataGraphCounter2; i+=3)
                {
                    if (_dataGraph2[i] == _dataGraph2[i + 1] && _dataGraph2[i] == _dataGraph2[i + 2])
                    {
                        _dataGraph8Correction[_dataGraphCounter8Correction] = _dataGraph2[i];
                        timePoint8Correction[_dataGraphCounter8Correction] = timePoint2[i];
                        _dataGraphCounter8Correction++;
                    }               
                }
                //Формирование массивов для режима поправка
                //График 2
                for (int i = 0; i < _dataGraphCounter3; i += 3)
                {
                    if (_dataGraph3[i] == _dataGraph3[i + 1] && _dataGraph3[i] == _dataGraph3[i + 2])
                    {
                        _dataGraph9Correction[_dataGraphCounter9Correction] = _dataGraph3[i];
                        timePoint9Correction[_dataGraphCounter9Correction] = timePoint3[i];
                        _dataGraphCounter9Correction++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

        }
        //Формирование времени
        private void TimeProcessing()
        {
            try
            {
                byte error_packages_counter = 0;
                int j = 0;
                if (swatch.ElapsedMilliseconds > 0)
                {
                    timebuffer = Convert.ToUInt64(swatch.ElapsedMilliseconds * 1000); ;
                    swatch.Reset();
                }
                for (int i = 0; i < possition_counter; i += 4)
                {
                    uint startByteCheck1 = responce[i] & MaskFirstByteLogicConst;
                    uint startByteCheck2 = responce[i + 1] & MaskFirstByteLogicConst;
                    uint startByteCheck3 = responce[i + 2] & MaskFirstByteLogicConst;
                    uint startByteCheck4 = responce[i + 3] & MaskFirstByteLogicConst;
                    {
                        if (startByteCheck1 != 0 && startByteCheck2 == 0 && startByteCheck3 == 0 && startByteCheck4 == 0)
                        {
                            timePoint[j] = timebuffer;
                            timebuffer += Convert.ToUInt64(responce[i + 3]);
                            j++;
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                error_counter_label.Text = "Errors:" + error_counter;
                                if (error_packages_counter == 4)
                                {
                                    error_packages_counter = 0;
                                    error_counter++;
                                }
                                error_packages_counter++;
                                i -= 3;
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message;
            }

        }
        // ! В случае исключительной ситуации, полный сброс переменных
        private void DataReset()
        {
            try
            {
                // Очистка всех существующих массивов в случае сбоя программы
                Array.Clear(command, 0, command.Length);
                if (_dataGraph != null)
                    Array.Clear(_dataGraph, 0, _dataGraph.Length);
                Array.Clear(_dataGraph2, 0, _dataGraph2.Length);
                Array.Clear(_dataGraph3, 0, _dataGraph3.Length);
                Array.Clear(_dataGraph4, 0, _dataGraph4.Length);
                Array.Clear(_dataGraph5, 0, _dataGraph5.Length);
                Array.Clear(_dataGraph6Limit, 0, _dataGraph6Limit.Length);
                Array.Clear(_dataGraph7Limit, 0, _dataGraph7Limit.Length);
                Array.Clear(_dataGraph8Correction, 0, _dataGraph8Correction.Length);
                Array.Clear(_dataGraph9Correction, 0, _dataGraph9Correction.Length);
                Array.Clear(_dataGraph10ServiceLimitation, 0, _dataGraph10ServiceLimitation.Length);
                Array.Clear(_dataGraph11ServiceLimitation, 0 , _dataGraph11ServiceLimitation.Length);
                Array.Clear(responce, 0, responce.Length);
                Array.Clear(timePoint, 0, timePoint.Length);
                Array.Clear(timePoint2, 0, timePoint2.Length);
                Array.Clear(timePoint3, 0, timePoint3.Length);
                Array.Clear(timePoint4, 0, timePoint4.Length);
                Array.Clear(timePoint5, 0, timePoint5.Length);
                Array.Clear(timePoint6Limit, 0, timePoint6Limit.Length);
                Array.Clear(timePoint7Limit, 0, timePoint7Limit.Length);
                Array.Clear(timePoint8Correction, 0, timePoint8Correction.Length);
                Array.Clear(timePoint9Correction, 0, timePoint9Correction.Length);
                Array.Clear(timePoint10ServiceLimitation, 0, timePoint10ServiceLimitation.Length);
                Array.Clear(timePoint11ServiceLimitation, 0 , timePoint11ServiceLimitation.Length);
                // Очистка вспомогательных переменных
                _scanningstatus = false;
                dataResiveProgressBar.Value = 1;
                dataResiveProgressBar.Visible = false;
                _timer_counter = 0;
                possition_counter = 0;
                _dataGraphCounter2 = 0;
                _dataGraphCounter3 = 0;
                _dataGraphCounter4 = 0;
                _dataGraphCounter5 = 0;

                _dataGraphCounter6Limit = 0;
                _dataGraphCounter7Limit = 0;
                _dataGraphCounter8Correction = 0;
                _dataGraphCounter9Correction = 0;
                _dataGraphCounter10ServiceLimitation = 0;
                _dataGraphCounter11ServiceLimitation = 0;
                timebuffer = 0;
                timer30sec = 1;

                // Если есть что удалять
                if (pane1.CurveList.Count > 0 && pane2.CurveList.Count > 0 && pane3.CurveList.Count > 0)
                {
                    // Удалим все кривые
                    pane1.CurveList.Clear();
                    pane2.CurveList.Clear();
                    pane3.CurveList.Clear();
                    // Синхронизация графиков по масштабам оси Х и Y
                    pane1.XAxis.Scale.Min = XScaleMin;
                    pane2.XAxis.Scale.Min = XScaleMin;
                    pane3.XAxis.Scale.Min = XScaleMin;

                    pane1.XAxis.Scale.Max = XScaleMax;
                    pane2.XAxis.Scale.Max = XScaleMax;
                    pane3.XAxis.Scale.Max = XScaleMax;
                    // Обновим график
                    zedGraph.AxisChange();
                    zedGraph.Invalidate();
                }
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                }
            }
            catch (Exception ex)
            {
                error_TextBox.Text += "\r\n [" + System.DateTime.Now + "]" + ex.Message + ex.StackTrace;
            }
        }

        private void zedGraph_Load(object sender, EventArgs e)
        {

        }

        //! Таймер
        private void timerCount_Tick(object sender, EventArgs e)
        {
            dataResiveCounterTest_label.Text = "Packages per sec: " + (_timer_counter).ToString();
            packages_counter_label.Text = "Bytes resived: " + possition_counter.ToString();
            _timer_counter = 0;
            if (_scanningstatus)
            {
                dataResiveProgressBar.PerformStep();
                timer30sec++;
            }
            if (timer30sec == 30)
            {
                swatch.Stop();
                Thread dataThread = new Thread(DataProcessing);
                dataThread.IsBackground = true;
                dataThread.Start();
            }
        }

        #endregion

        #region Time Counter
        //Выделение точки для измерений
        private void zedGraph_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkBoxMeasuring.Checked == true)
            {
                if (rbtnMeasureGraph1.Checked == true)
                {
                    DistinguishPointGraph1(e);
                }
                else
                    if (rbtnMeasureGraph2.Checked == true)
                    {
                        DistinguishPointGraph2(e);
                    }
                    else
                        if (rbtnMeasureGraph3.Checked == true)
                        {
                            DistinguishPointGraph3(e);
                        }
            }
                zedGraph.Invalidate();
         }
        //Отрисовка выделенной точки на графике 1 для измерений
        private void DistinguishPointGraph1(MouseEventArgs e)
        {
            if (measPoint1Flag || measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = pane1.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    PointPairList point = new PointPairList();

                    point.Add(curve[index]);

                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = pane1.AddCurve("",
                        new double[] { curve[index].X },
                        new double[] { curve[index].Y },
                        Color.Blue,
                        SymbolType.Circle);

                    // 
                    curvePount.Line.IsVisible = false;

                    // Цвет заполнения круга - колубой
                    curvePount.Symbol.Fill.Color = Color.Black;

                    // Тип заполнения - сплошная заливка
                    curvePount.Symbol.Fill.Type = FillType.Solid;

                    // Размер круга
                    curvePount.Symbol.Size = 20;

                    if (measPoint1Flag)
                    {
                        curveToDelete1 = curvePount;
                        measPointX1 = curve[index].X;
                        measPointY1 = curve[index].Y;
                        measPoint1Flag = false;
                    }
                    else
                    {
                        curveToDelete2 = curvePount;
                        measPointX2 = curve[index].X;
                        measPointY2 = curve[index].Y;
                        measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((measPointX2 - measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((measPointY2 - measPointY1)).ToString("F2");
                    }

                }
            }
        }
        //Отрисовка выделенной точки на графике 2 для измерений
        private void DistinguishPointGraph2(MouseEventArgs e)
        {
            if (measPoint1Flag || measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = pane2.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    PointPairList point = new PointPairList();

                    point.Add(curve[index]);

                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = pane2.AddCurve("",
                        new double[] { curve[index].X },
                        new double[] { curve[index].Y },
                        Color.Blue,
                        SymbolType.Circle);

                    // 
                    curvePount.Line.IsVisible = false;

                    // Цвет заполнения круга - колубой
                    curvePount.Symbol.Fill.Color = Color.Black;

                    // Тип заполнения - сплошная заливка
                    curvePount.Symbol.Fill.Type = FillType.Solid;

                    // Размер круга
                    curvePount.Symbol.Size = 20;

                    if (measPoint1Flag)
                    {
                        curveToDelete1 = curvePount;
                        measPointX1 = curve[index].X;
                        measPointY1 = curve[index].Y;
                        measPoint1Flag = false;
                    }
                    else
                    {
                        curveToDelete2 = curvePount;
                        measPointX2 = curve[index].X;
                        measPointY2 = curve[index].Y;
                        measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((measPointX2 - measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((measPointY2 - measPointY1)).ToString("F2");
                    }

                }
            }
        }
        //Отрисовка выделенной точки на графике 3 для измерений
        private void DistinguishPointGraph3(MouseEventArgs e)
        {
            if (measPoint1Flag || measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = pane3.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    PointPairList point = new PointPairList();

                    point.Add(curve[index]);

                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = pane3.AddCurve("",
                        new double[] { curve[index].X },
                        new double[] { curve[index].Y },
                        Color.Blue,
                        SymbolType.Circle);

                    // 
                    curvePount.Line.IsVisible = false;

                    // Цвет заполнения круга - колубой
                    curvePount.Symbol.Fill.Color = Color.Black;

                    // Тип заполнения - сплошная заливка
                    curvePount.Symbol.Fill.Type = FillType.Solid;

                    // Размер круга
                    curvePount.Symbol.Size = 20;

                    if (measPoint1Flag)
                    {
                        curveToDelete1 = curvePount;
                        measPointX1 = curve[index].X;
                        measPointY1 = curve[index].Y;
                        measPoint1Flag = false;
                    }
                    else
                    {
                        curveToDelete2 = curvePount;
                        measPointX2 = curve[index].X;
                        measPointY2 = curve[index].Y;
                        measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((measPointX2 - measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((measPointY2 - measPointY1)).ToString("F2");
                    }

                }
            }   
        }
        //Кнопка сдаления точек измерений на графике
        private void bntResetTimePoints_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtnMeasureGraph1.Checked == true)
                {
                    pane1.CurveList.Remove(curveToDelete1);
                    pane1.CurveList.Remove(curveToDelete2);
                }

                if (rbtnMeasureGraph2.Checked == true)
                {
                    pane2.CurveList.Remove(curveToDelete1);
                    pane2.CurveList.Remove(curveToDelete2);
                }

                if (rbtnMeasureGraph3.Checked == true)
                {
                    pane3.CurveList.Remove(curveToDelete1);
                    pane3.CurveList.Remove(curveToDelete2);
                }

            }
            catch
            {
            }
            finally
            {
                measPointX1 = 0;
                measPointX2 = 0;
                measPoint1Flag = true;
                measPoint2Flag = true;
                zedGraph.Invalidate();
                tbTimeCounted.Text = ""; 
                tbYCounted.Text = "";
            }
        }
        //Разрешить измерение времени
        private void checkBoxMeasuring_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMeasuring.Checked == true)
            {
                if(_amplShow)
                rbtnMeasureGraph1.Enabled = true;
                rbtnMeasureGraph2.Enabled = true;
                rbtnMeasureGraph3.Enabled = true;
            }
            else 
            {
                if(_amplShow)
                rbtnMeasureGraph1.Enabled = false;
                rbtnMeasureGraph2.Enabled = false;
                rbtnMeasureGraph3.Enabled = false;
            }
        }
        #endregion

    }
}