using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using ZedGraph;
// !!!!ReSharper disable All

////////////////////////////
// Автор: Ланденок Владимир
// Редакция от 02.06.2017
///////////////////////////

namespace BVKGraph
{
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    [SuppressMessage("ReSharper", "CoVariantArrayConversion")]
    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
    [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
    public partial class WorkForm : Form
    {
        #region Variables

        private GraphPane _pane1 = new GraphPane();
        private GraphPane _pane2 = new GraphPane();
        private GraphPane _pane3 = new GraphPane();

        MasterPane _masterPane = new MasterPane();

        private Stopwatch _swatch = new Stopwatch();

        // Объявление массивов для работы с данными
        private byte[] _command = new byte[3];
        private ushort[] _dataGraph;

        // Статические массивы (для обработки принятых данных)
        private ushort[] _dataGraph2;
        private ushort[] _dataGraph3;
        private ushort[] _dataGraph4 = new ushort[600000];
        private ushort[] _dataGraph5 = new ushort[600000];

        //Нахождение экстремумов мощности
        private ushort[] _croppedArray;
        private ushort[] _croppedArray2;

        private double[] _dataGraph6Limit;
        private double[] _dataGraph7Limit;
        private double[] _dataGraph8Averaged;
        private double[] _dataGraph9Averaged;
        private double[] _dataGraph10Correction;
        private double[] _dataGraph11Correction;

        private ulong[] _timePoint = new ulong[2100000];
        private ulong[] _timePoint4 = new ulong[2100000];
        private ulong[] _timePoint5 = new ulong[2100000];

        // Усреднение 8 кадра
        private double[] _dataGraph10ServiceLimitation = new double[7500];
        private ulong[] _timePoint10ServiceLimitation = new ulong[210000];
        private double[] _dataGraph11ServiceLimitation = new double[7500];
        private ulong[] _timePoint11ServiceLimitation = new ulong[210000];

        private byte[] _responce = new byte[2100000];
        private byte[] _responceTemp = new byte[15000];
        // Объвление констант для  битовых масок
        private const uint MaskFirstByteConst = 0x0F;
        private const uint MaskThirdByteConst = 0x7F;
        private const uint MaskSecondByteConst = 0x7F;
        private const uint MaskResponceConst = 0x70;

        // Размеры шрифтов
        private const int LabelsXfontSize = 25;
        private const int LabelsYfontSize = 20;
        private const int TitleXFontSize = 25;
        private const int TitleYFontSize = 20;
        private const int LegendFontSize = 15;
        private const int MainTitleFontSize = 30;

        //Диапазоны осей
        private const double XScaleMin = 0;
        private const double XScaleMax = 30000000;
        private const double YScaleMin9 = -9;
        private const double YScaleMax9 = 9;
        private const double YScaleMin17 = -17;
        private const double YScaleMax17 = 17;
        private const double YScaleMin35 = -35;
        private const double YScaleMax35 = 35;

        // Поиск первого бита
        private const uint MaskFirstByteLogicConst = 0x80;
        //Значение угол\азимут
        private const short BadPointConst = 0x7FFF;

        //Объявление вспомогательных переменных
        private int _timerCounter;
        private int _readCounter;
        private int _possitionCounter;
        private int _dataGraphCounter4, _dataGraphCounter5 ;
        ulong _timebuffer;
        private byte _timer30Sec = 1;
        private int _errorCounter;

        //Флаги
        private bool _scanningstatus;
        private bool _syncstatus;
        private bool _limitation = true;
        private bool _engeneering;
        private bool _scaleXAxis16Or35 = true;
        private bool _service;
        private bool _correction;
        private bool _limitAverage = true;
        private bool _filter = true;

        //Флаг точек
        private bool _measPoint1Flag = true;
        private bool _measPoint2Flag = true;
        private double _measPointX1;
        private double _measPointX2;
        private double _measPointY1;
        private double _measPointY2;
        CurveItem _curveToDelete1;
        CurveItem _curveToDelete2;

        //Отображение амплитуды
        private bool _amplShow;

        #endregion

        public WorkForm()
        {
            InitializeComponent();
            error_TextBox.Text = " [" + DateTime.Now + "] " + "Инициализация завершена. Текущая версия программы: " +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
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
            zedGraph.PointValueEvent += zedGraph_PointValueEvent;
            // !!! Подпишемся на событие, которое будет возникать перед тем, 
            // как будет показано контекстное меню.
            zedGraph.ContextMenuBuilder += zedGraph_ContextMenuBuilder;
        }

        #region Events

        /// <summary>
        /// Обработчик события, который вызывается, перед показом контекстного меню
        /// </summary>
        /// <param name="sender">Компонент ZedGraph</param>
        /// <param name="menuStrip">Контекстное меню, которое будет показано</param>
        /// <param name="mousePt">Координаты курсора мыши</param>
        /// <param name="objState">Состояние контекстного меню. Описывает объект, на который кликнули.</param>
        public void zedGraph_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            // Переименуем (переведем на русский язык) некоторые пункты контекстного меню
            menuStrip.Items[0].Text = "Копировать графики";
            menuStrip.Items[1].Text = "Сохранить как картинку…";
            menuStrip.Items[2].Text = "Параметры печати…";
            menuStrip.Items[3].Text = "Печать…";
            //menuStrip.Items[4].Text = "Показывать значения в точках…";
            menuStrip.Items[5].Text = "Уменьшить (Ctrl + Z)";
            menuStrip.Items[6].Text = "Отменить все увеличение";
            menuStrip.Items[7].Text = "Установить масштаб по умолчанию…";

            // Некоторые пункты удалим
            //Удаляем пункт меню параметры страницы
            //menuStrip.Items.RemoveAt(2);
            //Удалаяем пункт меню показать значения в точках
            menuStrip.Items.RemoveAt(4);
        }
        //Событие всплывающей подсказки
        private string zedGraph_PointValueEvent(ZedGraphControl sender,GraphPane pane,CurveItem curve,int iPt)
        {
            // Получим точку, около которой находимся
            PointPair point = curve[iPt];
            // Сформируем строку F1 было
            string result = $"K: {point.Y:F2}\nt: {point.X / 1000000}";
            Thread.Sleep(100);
            return result;
        }
        // Ctrl + F12 Показать кнопку лимитация 2, по дефолту включена
        // Ctrl + Z Un-Zoom hot key
        private void WorkForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch  (e.KeyCode)
            {
                //Отображение скрытого меню отладки
                case Keys.F12:
                    if (e.Control && e.KeyCode == Keys.F12)
                    {
                        gbLimitation2.Visible = !gbLimitation2.Visible;
                    }     
                    break;
                //Горячие клавиши, отменить увеличение на шаг
                case Keys.Z:
                    if (e.Control && e.KeyCode == Keys.Z)
                    {
                        zedGraph.ZoomOut(zedGraph.GraphPane);
                    }
                    break;
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
                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + " the hardware detected a framing error";
                            break;
                        case SerialError.Overrun:

                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + " a character-buffer overrun has occurred. The next character is lost";
                            break;
                        case SerialError.RXOver:
                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + " an input buffer overflow has occured."
                            + " There is either no room in the input buffer, or a character was received after the End-Of-File (EOF) character.\n";
                            break;
                        case SerialError.RXParity:
                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + " the hardware detected a parity error.\n";
                            break;
                        case SerialError.TXFull:
                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + "the application tried to transmit a character,"
                                + " but the output buffer was full.";
                            break;
                        default:
                            error_TextBox.Text = " [" + DateTime.Now + "] " + "On port " + serialPort.PortName + " an unknown error occurred.\n";
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
                    _readCounter = serialPort.Read(_responce, _possitionCounter, 1000);
                    _swatch.Stop();
                    _possitionCounter += _readCounter;
                    _timerCounter += _readCounter;
                }
                else
                {
                    serialPort.Read(_responceTemp, 0, 2000);
                    serialPort.DiscardInBuffer();
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        //Настройка параметров Combobox
        private void combobox_prop()
        {
            try
            {
                cbLimitationLevel.SelectedIndex = 2;
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
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
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
                serialPort.ReadBufferSize = 300000;
                serialPort.Open();
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + " Connected to COM1";
                button_prop();
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
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
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + " Disconnected from COM1";
                button_prop();
                if (serialPort.IsOpen)
                {
                    _scanningstatus = false;
                    dataResiveProgressBar.Value = 1;
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
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
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + " Connected to COM2";
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }

        private void disconnect2_button_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort2.Close();
                button2_prop();
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + " Disconnected from COM2";
                _syncstatus = false;
                Sync_checkBox.Text = "   Sync Off  ";
                syncstatus_label.Text = "Sync: No";
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
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
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
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
                _swatch.Start();
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
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        //Запуск режима работы "Хризантема"
        private void startbvk_button_Click(object sender, EventArgs e)
        {
            DataReset();

            if (_syncstatus == false)
            {
                byte commandprop = 0x1A;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                _scanningstatus = true;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }
            else
            {
                byte commandprop = 0x1A;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
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
                _command[0] = 0xAF;
                _command[1] = commandprop;
                _command[2] = Convert.ToByte(0xAF ^ commandprop);
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        // Остановка БВК
        private void stopbvk_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x1C;
            DataSend(commandprop);
            serialPort.Write(_command, 0, 3);
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
            starttest_button.UseVisualStyleBackColor = true;
        }
        // Отключить\Включить режим ограничения по уровню 0,5
        private void limitation_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x17;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
            }

            _limitation = !_limitation;

            if (limitation_button.UseVisualStyleBackColor)
                limitation_button.BackColor = Color.Khaki;
            else limitation_button.UseVisualStyleBackColor = true;
        }
        //Запуск режима Деривации (БКВ должен быть остановлен)
        private void derivation_button_Click(object sender, EventArgs e)
        {
            DataReset();

            if (_syncstatus == false)
            {
                byte commandprop = 0x1B;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
                dataResiveProgressBar.Visible = true;
                derivation_button.Enabled = false;
                _scanningstatus = true;
                starttest_button.Enabled = false;
                startbvk_button.Enabled = false;
                serialPort.DiscardInBuffer();
            }
            else
            {
                byte commandprop = 0x1B;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
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
            serialPort.Write(_command, 0, 3);
            starttest_button.BackColor = Color.Khaki;
        }
        //Нормальный режим
        private void normalmode_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x11;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
            }

            _correction = false;
            _service = false;
            _limitation = true;
            normalmode_button.BackColor = Color.YellowGreen;
            btnServiceMode.UseVisualStyleBackColor = true;
            kdo_button.UseVisualStyleBackColor = true;
            starttest_button.UseVisualStyleBackColor = true;
            limitation_button.BackColor = Color.Khaki;
        }
        // Инженерный режим
        private void engineeringmode_button_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x12;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
            }
                _engeneering = !_engeneering;

            if (engineeringmode_button.UseVisualStyleBackColor)
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
                serialPort.Write(_command, 0, 3);
            }

            _correction = true;
            _service = false;
            _limitation = false;
            normalmode_button.UseVisualStyleBackColor = true;
            btnServiceMode.UseVisualStyleBackColor = true;
            starttest_button.UseVisualStyleBackColor = true;
            kdo_button.BackColor = Color.YellowGreen;
            limitation_button.BackColor = Color.Khaki;
        }
        // Выбор УАС-1
        private void yac1_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x15;
            DataSend(commandprop);
            serialPort.Write(_command, 0, 3);

            yac1_button.BackColor = Color.PaleTurquoise;
            yac2_button.UseVisualStyleBackColor = true;
        }
        // Выбор УАС-2
        private void yac2_button_Click(object sender, EventArgs e)
        {
            byte commandprop = 0x16;
            DataSend(commandprop);
            serialPort.Write(_command, 0, 3);

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
                binaryWriter.Write(_responce, 0, _possitionCounter);
                binaryWriter.Close();
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + "File saved Data.bin";
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
                binaryReader.Read(_responce, 0, (int)binaryReader.BaseStream.Length);
                _possitionCounter = (int)binaryReader.BaseStream.Length;
                binaryReader.Close();
                error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + "File open " + openFileDialog.SafeFileName;
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
                    serialPort.Write(_command, 0, 3);
                    _syncstatus = true;
                    Sync_checkBox.Text = "Откл.синхр.";
                    syncstatus_label.Text = "Sync: Yes";
                    Sync_checkBox.BackColor = Color.SkyBlue;
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        //Кнопка показать\отключить  график амплитуда
        private void btnShowAmpl_Click(object sender, EventArgs e)
        {
            if (_amplShow)
            {
                btnShowAmpl.Text = "Показать";
                _amplShow = false;
                rbtnMeasureGraph1.Enabled = false;
                rbtnMeasureGraph2.Select();
            }
            else
            {
                btnShowAmpl.Text = "Скрыть";
                _amplShow = true;
                rbtnMeasureGraph1.Enabled = true;
            }
            _masterPane.PaneList.Clear();
            Thread thrd1 = new Thread(GraphDraw);
            thrd1.IsBackground = true;
            thrd1.Start();
        }
        //Кнопка перезагрузки графиков
        private void btnResetGraph_Click(object sender, EventArgs e)
        {
            // Если есть что удалять
            if (_pane1.CurveList.Count > 0 && _pane2.CurveList.Count > 0 && _pane3.CurveList.Count > 0)
            {
                // Удалим все кривые
                _pane1.CurveList.Clear();
                _pane2.CurveList.Clear();
                _pane3.CurveList.Clear();
                // Обновим график
                zedGraph.Invalidate();
            }

            Thread thrd = new Thread(GraphDraw);
            thrd.Start();
        }
        //Масштаб графика +-9
        private void btnScale9_Click(object sender, EventArgs e)
        {
            _pane2.YAxis.Scale.Min = YScaleMin9;
            _pane3.YAxis.Scale.Min = YScaleMin9;

            _pane2.YAxis.Scale.Max = YScaleMax9;
            _pane3.YAxis.Scale.Max = YScaleMax9;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        //Изменение масштаба YAxis -35 + 35
        private void btnScale35_Click(object sender, EventArgs e)
        {
            _scaleXAxis16Or35 = false;
            _pane2.YAxis.Scale.Min = YScaleMin35;
            _pane3.YAxis.Scale.Min = YScaleMin35;

            _pane2.YAxis.Scale.Max = YScaleMax35;
            _pane3.YAxis.Scale.Max = YScaleMax35;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        //Изменение масштаба YAxis -17 + 17
        private void btnScale17_Click(object sender, EventArgs e)
        {
            _scaleXAxis16Or35 = true;
            _pane2.YAxis.Scale.Min = YScaleMin17;
            _pane3.YAxis.Scale.Min = YScaleMin17;

            _pane2.YAxis.Scale.Max = YScaleMax17;
            _pane3.YAxis.Scale.Max = YScaleMax17;

            zedGraph.Invalidate();
            zedGraph.AxisChange();
        }
        // Режим отображения служебных данных
        private void btnServiceMode_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte commandprop = 0x13;
                DataSend(commandprop);
                serialPort.Write(_command, 0, 3);
            }

            _service = true;
            _correction = false;
            _limitation = false;

            normalmode_button.UseVisualStyleBackColor = true;
            kdo_button.UseVisualStyleBackColor = true;
            btnServiceMode.BackColor = Color.YellowGreen;
            limitation_button.UseVisualStyleBackColor = true;
        }

        //!!! ТЕСТ Включение / отключенин двойнного усреднения
        private void btnLimitAverage_Click(object sender, EventArgs e)
        {
            if (_limitAverage)
            {
                _limitAverage = false;
                btnLimitAverage.Text = "Enable";
            }
            else
            {
                _limitAverage = true;
                btnLimitAverage.Text = "Disable";
            }
        }
        //!!! ТЕСТ Включение / отключенин двойнного усреднения
        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (_filter)
            {
                _filter = false;
                btnFilter.Text = "Filter On";
            }
            else
            {
                _filter = true;
                btnFilter.Text = "Filter Off";
            }
        }

        #endregion

        #region Graphs

        // ! Формирование графика
        private void GraphDraw()
        {
            try
            {
                _masterPane = zedGraph.MasterPane;

                // !!! Свойства IsSynchronizeXAxes и IsSynchronizeYAxes указывают, что
                // оси на графиках должны перемещаться и масштабироваться одновременно.
                zedGraph.IsSynchronizeXAxes = true;
                // !!! Показывать значение точек по умолчанию включено
                zedGraph.IsShowPointValues = true;

                // По умолчанию в MasterPane содержится один экземпляр класса GraphPane 
                // (который можно получить из свойства zedGraph.GraphPane)
                // Очистим этот список, так как потом мы будем создавать графики вручную
                _masterPane.PaneList.Clear();
                try
                {
                    // Заполнение графика данными, 
                    // поэтому вынесем заполнение точек в отдельный метод DrawSingleGraph()
                    Graph1(_pane1);
                    Graph2(_pane2);
                    Graph3(_pane3);

                }
                catch (NullReferenceException)
                {
                    Invoke(new MethodInvoker(delegate 
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + "Nothing to draw.";                     
                    }
                    )); //MessageBox.Show("Nothing to draw.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                // Добавим новый график в MasterPane
                if (_amplShow)
                _masterPane.Add(_pane1);
                _masterPane.Add(_pane2);
                _masterPane.Add(_pane3);
                

                // Будем размещать добавленные графики в MasterPane
                using (Graphics g = CreateGraphics())
                {
                    // Графики будут размещены в один столбец друг под другом
                    _masterPane.SetLayout(g, PaneLayout.SingleColumn);
                }

                // Настройка свойств графиков
                graph_prop_final(_pane1, _pane2, _pane3);

                if (_scaleXAxis16Or35)
                    btnScale17_Click(null, null);
                else
                    btnScale35_Click(null ,null);
                

                // Вызываем метод AxisChange (), чтобы обновить данные об осях. 
                //zedGraph.AxisChange();

                // Обновляем график
                zedGraph.Invalidate();

            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
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
                    if (_dataGraph[i] != BadPointConst)
                        list1.Add(_timePoint[i], (Convert.ToDouble(_dataGraph[i])) - 1100);
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

            if (cbShowExtrems.Checked)
            {
                // Создадим список точек=> График номер 1
                PointPairList list2 = new PointPairList();
                {
                    for (int i = 0; i < _croppedArray.Length; i++)
                    {
                        if (_croppedArray[i] != 0)
                        {
                            list2.Add(_timePoint[i], (Convert.ToDouble(_croppedArray[i])) - 1100); // / 10);
                        }
                    }
                }
                // Создадим кривые ЭКСТРЕМУМОВ 
                LineItem curve2 = pane1.AddCurve("", list2, Color.Red, SymbolType.Circle);
                // У кривой линия будет невидимой
                curve2.Line.IsVisible = true;
                // Цвет заполнения отметок (ромбиков) - голубой
                curve2.Symbol.Fill.Color = Color.Red;
                // Тип заполнения - сплошная заливка
                curve2.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve2.Symbol.Size = 15;
                // Линия невидимая
                curve2.Line.IsVisible = false;
                // Точки лимитации
                PointPairList list3 = new PointPairList();
                {
                    for (int i = 0; i < _croppedArray2.Length; i++)
                    {
                        if (_croppedArray2[i] != 0)
                        {
                            list3.Add(_timePoint[i], (Convert.ToDouble(_croppedArray2[i])) - 1100); // / 10);
                        }
                    }
                }
                // Точки лимитации
                LineItem curve3 = pane1.AddCurve("", list3, Color.Green, SymbolType.Circle);
                // У кривой линия будет невидимой
                curve3.Line.IsVisible = true;
                // Цвет заполнения отметок (ромбиков) - голубой
                curve3.Symbol.Fill.Color = Color.Green;
                // Тип заполнения - сплошная заливка
                curve3.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve3.Symbol.Size = 15;
                // Линия невидимая
                curve3.Line.IsVisible = false;
            }
        }
        // График 2
        private void Graph2(GraphPane pane2)
        {

            // Создадим список точек=> График номер 2
            PointPairList list2 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph2.Length; i++)
                {
                    if (_dataGraph2[i] != (ushort)BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list2.Add(_timePoint[i], _dataGraph2[i] - 32);
                    }
                }
            }
            //Лимитация в режиме служебный(8 кадр)
            // !!! Считает не все точки, а только точки с интервалом менее 80 мкс
            PointPairList list4 = new PointPairList();
            {
                // Служебный кадр
                //Без расчета среднего значения

                for (int i = 0; i < _dataGraphCounter4; i++)
                {
                    if (_dataGraph4[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list4.Add(_timePoint4[i], _dataGraph4[i] - 32);
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
            // Выделенные точки лимитации
            PointPairList list10 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph6Limit.Length; i++)
                {
                    if ((ushort)_dataGraph6Limit[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list10.Add(_timePoint[i], _dataGraph6Limit[i] - 32);
                    }
                }
            }
            // Создадим кривые 
            if (_engeneering)
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

                LineItem curve10 = pane2.AddCurve("", list10, Color.Red, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve10.Symbol.Fill.Color = Color.Red;
                // Тип заполнения - сплошная заливка
                curve10.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve10.Symbol.Size = 15;
                // Линия невидимая
                curve10.Line.IsVisible = false;
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
            PointPairList list12 = new PointPairList();
            {
                double sum = 0;
                bool average = false;
                for (int i = 0; i < _dataGraph8Averaged.Length; i++)
                {
                    if ((ushort)_dataGraph8Averaged[i] != BadPointConst)
                    {
                        if (_limitAverage)
                        {
                            if (average)
                            {
                                // Отнимаем значение 33 для удобства отображения
                                list12.Add(_timePoint[i], ((_dataGraph8Averaged[i] - 32) + sum) / 2);
                                average = !average;
                            }
                            else
                            {
                                sum = _dataGraph8Averaged[i] - 32;
                                average = !average;
                            }               
                        }
                        else
                        {             
                                list12.Add(_timePoint[i], (_dataGraph8Averaged[i] - 32));
                        }
                    }
                }
            }
            if (_limitation)
            {
                LineItem curve12 = pane2.AddCurve("", list12, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve12.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                curve12.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve12.Symbol.Size = 10;
                // Линия невидимая
                curve12.Line.IsVisible = true;
            }
            PointPairList list14 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph10Correction.Length; i+=3)
                {
                    if ((ushort)_dataGraph10Correction[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list14.Add(_timePoint[i], _dataGraph10Correction[i] - 32);
                    }
                }
            }
            if (_correction)
            {
                LineItem curve14 = pane2.AddCurve("", list14, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve14.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                curve14.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve14.Symbol.Size = 10;
                // Линия невидимая
                curve14.Line.IsVisible = true;
            }
        }
        //График 3
        private void Graph3(GraphPane pane3)
        {

            // Создадим список точек=> График номер 3
            PointPairList list3 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph3.Length; i++)
                {
                    if (_dataGraph3[i] != (ushort) BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list3.Add(_timePoint[i], _dataGraph3[i] - 32);
                    }
                }
            }
            // 8 кадр список точек
            PointPairList list5 = new PointPairList();
            {
                //Служебный кадр
                //Без расчета среднего значения

                for (int i = 0; i < _dataGraphCounter5; i++)
                {
                    if (_dataGraph5[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list5.Add(_timePoint5[i], _dataGraph5[i] - 32);
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
            //Служебный режим
            if (_service)
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
            // Выделенные точки лимитации
            PointPairList list11 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph7Limit.Length; i++)
                {
                    if ((ushort)_dataGraph7Limit[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list11.Add(_timePoint[i], _dataGraph7Limit[i] - 32);
                    }
                }
            }
            PointPairList list13 = new PointPairList();
            {
                double sum = 0;
                bool average = false;
                for (int i = 0; i < _dataGraph9Averaged.Length; i++)
                {
                    if ((ushort)_dataGraph9Averaged[i] != BadPointConst)
                    {
                        if (_limitAverage)
                        {
                            if (average)
                            {
                                // Отнимаем значение 33 для удобства отображения
                                list13.Add(_timePoint[i], ((_dataGraph9Averaged[i] - 32) + sum) / 2);
                                average = !average;
                            }
                            else
                            {
                                sum = _dataGraph9Averaged[i] - 32;
                                average = !average;
                            }
                        }
                        else
                        {
                                list13.Add(_timePoint[i], (_dataGraph9Averaged[i] - 32));
                        }
                    }
                }
            }
            //Графики инженерные
            if (_engeneering)
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

                LineItem curve11 = pane3.AddCurve("", list11, Color.Red, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve11.Symbol.Fill.Color = Color.Red;
                // Тип заполнения - сплошная заливка
                curve11.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve11.Symbol.Size = 15;
                // Линия невидимая
                curve11.Line.IsVisible = false;
                // Лимитация
            }
            //График лимитации
            if (_limitation)
            {
                LineItem curve13 = pane3.AddCurve("", list13, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve13.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                curve13.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve13.Symbol.Size = 10;
                // Линия невидимая
                curve13.Line.IsVisible = true;
            }
            PointPairList list15 = new PointPairList();
            {
                for (int i = 0; i < _dataGraph11Correction.Length; i+=3)
                {
                    if ((ushort)_dataGraph11Correction[i] != BadPointConst)
                    {
                        // Отнимаем значение 33 для удобства отображения
                        list15.Add(_timePoint[i], _dataGraph11Correction[i] - 32);
                    }
                }
            }
            if (_correction)
            {
                LineItem curve15 = pane3.AddCurve("", list15, Color.DeepPink, SymbolType.Diamond);
                // Цвет заполнения отметок (ромбиков) - голубой
                curve15.Symbol.Fill.Color = Color.DeepPink;
                // Тип заполнения - сплошная заливка
                curve15.Symbol.Fill.Type = FillType.Solid;
                // Размер ромбиков
                curve15.Symbol.Size = 10;
                // Линия невидимая
                curve15.Line.IsVisible = true;
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
            MasterPane masterPane = zedGraph.MasterPane;
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
            pane1.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane1.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane1.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane1.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane1.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane1.Title.FontSpec.Size = MainTitleFontSize;
            pane1.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 2
            // Установим размеры шрифтов для меток вдоль осей
            pane2.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane2.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane2.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane2.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane2.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane2.Title.FontSpec.Size = MainTitleFontSize;
            pane2.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 3
            // Установим размеры шрифтов для меток вдоль осей
            pane3.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane3.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane3.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane3.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane3.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane3.Title.FontSpec.Size = MainTitleFontSize;
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
            pane3.YAxis.Title.Text = "Вертикаль \r\n Крен";

            //pane1.YAxis.Title.Text = "Значение Амплитуды";
            //pane2.YAxis.Title.Text = "Значение Азимута\r\n";
            //pane3.YAxis.Title.Text = "Значение Угла\r\n";

            // Параметры шрифтов для графика 1
            // Установим размеры шрифтов для меток вдоль осей
            pane1.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane1.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane1.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane1.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane1.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane1.Title.FontSpec.Size = MainTitleFontSize;
            pane1.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 2
            // Установим размеры шрифтов для меток вдоль осей
            pane2.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane2.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane2.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane2.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane2.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane2.Title.FontSpec.Size = MainTitleFontSize;
            pane2.Title.FontSpec.IsUnderline = true;
            // Параметры шрифтов для графика 3
            // Установим размеры шрифтов для меток вдоль осей
            pane3.XAxis.Scale.FontSpec.Size = LabelsXfontSize;
            pane3.YAxis.Scale.FontSpec.Size = LabelsYfontSize;

            // Установим размеры шрифтов для подписей по осям
            pane3.XAxis.Title.FontSpec.Size = TitleXFontSize;
            pane3.YAxis.Title.FontSpec.Size = TitleYFontSize;

            // Установим размеры шрифта для легенды
            pane3.Legend.FontSpec.Size = LegendFontSize;

            // Установим размеры шрифта для общего заголовка
            pane3.Title.FontSpec.Size = MainTitleFontSize;
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

                Invoke(new MethodInvoker(delegate 
                    {
                        dataResiveProgressBar.Visible = false;
                    }
                ));

                TimeProcessing();
                //+1 делается ввиду специфику округления в прграммирование для избежания выхода за пределы массива
                _dataGraph = new ushort[_possitionCounter / 4 + 1];
                for (int i = 0; i < _dataGraph.Length; i++)
                {
                    _dataGraph[i] = (ushort) BadPointConst;
                }
                _dataGraph2 = new ushort[_dataGraph.Length];
                // Заполнения массива 2 значениями BadPointConst
                for (int i = 0; i < _dataGraph2.Length; i++)
                {
                    _dataGraph2[i] = (ushort)BadPointConst;
                }
                // Заполнения массива 3 значениями BadPointConst
                _dataGraph3 = new ushort[_dataGraph.Length];
                for (int i = 0; i < _dataGraph3.Length; i++)
                {
                    _dataGraph3[i] = (ushort)BadPointConst;
                }

                if (_possitionCounter != 0)
                {
                    // Формирование графиков
                    int k = 0;
                    int z = 0;
                    int u = 0;
                    int c = 0;
                    byte errorPackagesCounter = 0;
                    for (int i = 0; i < _possitionCounter; i += 4)
                    {
                        //Проверка стартового бита
                        uint startByteCheck1 = _responce[i] & MaskFirstByteLogicConst;
                        uint startByteCheck2 = _responce[i + 1] & MaskFirstByteLogicConst;
                        uint startByteCheck3 = _responce[i + 2] & MaskFirstByteLogicConst;
                        uint startByteCheck4 = _responce[i + 3] & MaskFirstByteLogicConst;

                        if (startByteCheck1 != 0 && startByteCheck2 == 0 && startByteCheck3 == 0 && startByteCheck4 == 0)
                        {
                            _dataGraph[k] =
                                Convert.ToUInt16(((_responce[i] & MaskFirstByteConst) << 7) |
                                                 (_responce[i + 2] & MaskThirdByteConst));
                            k++;

                            switch (_responce[i] & MaskResponceConst)
                            {
                                // 0x0
                                case 0x00:
                                    if (Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst) < 65)
                                        _dataGraph2[k] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    break;
                                // 0x1
                                case 0x10:
                                    if (Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst) < 65)
                                        _dataGraph3[k] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    break;
                                //0x2
                                case 0x20:
                                    if (Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst) < 65)
                                        _dataGraph2[k] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    break;
                                //0x3
                                case 0x30:
                                    if (Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst) < 65)
                                        _dataGraph3[k] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    break;
                                // 0x4   (Служебный)
                                case 0x40:
                                    _dataGraph4[u] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    _timePoint4[u] = _timePoint[z];
                                    _dataGraphCounter4++;
                                    u++;
                                    break;
                                // 0x5 (Крен)
                                case 0x50:
                                    _dataGraph5[c] = Convert.ToUInt16(_responce[i + 1] & MaskSecondByteConst);
                                    _timePoint5[c] = _timePoint[z];
                                    _dataGraphCounter5++;
                                    c++;
                                    break;
                            }
                            z++;
                        }
                        else
                        {
                            if (errorPackagesCounter == 4)
                            {
                                _dataGraph[k] = 0;
                                k++;
                                errorPackagesCounter = 0;
                                _errorCounter++;
                            }
                            errorPackagesCounter++;
                            i -= 3;
                            error_counter_label.Text = "Errors:" + _errorCounter;
                        }
                    }
                    // Исправление ошибок графика амплитуда
                    // диапазон коррекции 0 to 1030
                    for (int i = 1; i < _dataGraph.Length; i++)
                    {
                        if (_dataGraph[i] <= 1030)
                        {
                            _dataGraph[i] = _dataGraph[i - 1];
                        }
                    }

                    // Обработка данных
                    // Для режимов
                    // Лимитация, Поправка, Служебный
                    DataTreatment();
                    Thread graphThread = new Thread(GraphDraw);
                    graphThread.Name = "Graph draw thread (open file)";
                    graphThread.IsBackground = true;
                    graphThread.Start();
                    _timer30Sec = 1;
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                {
                    error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message + ex.StackTrace;
                }
                ));
            }
        }
        // Формировавание данных для лимитации
        private void DataTreatment()
        {
            try
            {
                // Выбираем шапки экстремумов амплитуды
                //Отсечка маленьких экстремумов с амплитудой меньше 1105
                ushort amplMaxValue = ushort.MinValue;
                _croppedArray = new ushort[_dataGraph.Length];
                for (int i = 26; i < _dataGraph.Length - 26; i++)
                {
                    if (_dataGraph[i] > amplMaxValue
                        && _dataGraph[i + 25] < _dataGraph[i]
                        && _dataGraph[i + 24] < _dataGraph[i]
                        && _dataGraph[i + 23] < _dataGraph[i]
                        && _dataGraph[i + 22] < _dataGraph[i]
                        && _dataGraph[i + 21] < _dataGraph[i]
                        && _dataGraph[i + 20] < _dataGraph[i]
                        && _dataGraph[i + 19] < _dataGraph[i]
                        && _dataGraph[i + 18] < _dataGraph[i]
                        && _dataGraph[i + 17] < _dataGraph[i]
                        && _dataGraph[i + 16] < _dataGraph[i]
                        && _dataGraph[i + 15] < _dataGraph[i]
                        && _dataGraph[i + 14] < _dataGraph[i]
                        && _dataGraph[i + 13] < _dataGraph[i]
                        && _dataGraph[i + 12] < _dataGraph[i]
                        && _dataGraph[i + 11] < _dataGraph[i]
                        && _dataGraph[i + 10] < _dataGraph[i]
                        && _dataGraph[i + 9] < _dataGraph[i]
                        && _dataGraph[i + 8] < _dataGraph[i]
                        && _dataGraph[i + 7] < _dataGraph[i]
                        && _dataGraph[i + 6] < _dataGraph[i]
                        && _dataGraph[i + 5] < _dataGraph[i]
                        && _dataGraph[i + 4] < _dataGraph[i]
                        && _dataGraph[i + 3] < _dataGraph[i]
                        && _dataGraph[i + 2] < _dataGraph[i]
                        && _dataGraph[i + 1] < _dataGraph[i]
                        && _dataGraph[i - 1] <= _dataGraph[i]
                        && _dataGraph[i - 2] <= _dataGraph[i]
                        && _dataGraph[i - 3] <= _dataGraph[i]
                        && _dataGraph[i - 4] <= _dataGraph[i]
                        && _dataGraph[i - 5] <= _dataGraph[i]
                        && _dataGraph[i - 6] <= _dataGraph[i]
                        && _dataGraph[i - 7] <= _dataGraph[i]
                        && _dataGraph[i - 8] <= _dataGraph[i]
                        && _dataGraph[i - 9] <= _dataGraph[i]
                        && _dataGraph[i - 10] <= _dataGraph[i]
                        && _dataGraph[i - 11] <= _dataGraph[i]
                        && _dataGraph[i - 12] <= _dataGraph[i]
                        && _dataGraph[i - 13] <= _dataGraph[i]
                        && _dataGraph[i - 14] <= _dataGraph[i]
                        && _dataGraph[i - 15] <= _dataGraph[i]
                        && _dataGraph[i - 16] <= _dataGraph[i]
                        && _dataGraph[i - 17] <= _dataGraph[i]
                        && _dataGraph[i - 18] <= _dataGraph[i]
                        && _dataGraph[i - 19] <= _dataGraph[i]
                        && _dataGraph[i - 20] <= _dataGraph[i]
                        && _dataGraph[i - 21] <= _dataGraph[i]
                        && _dataGraph[i - 22] <= _dataGraph[i]
                        && _dataGraph[i - 23] <= _dataGraph[i]
                        && _dataGraph[i - 24] <= _dataGraph[i]
                        && _dataGraph[i - 25] <= _dataGraph[i])
                    {
                        if (_dataGraph[i] > 1105)
                        {
                            _croppedArray[i] = _dataGraph[i];
                            amplMaxValue = ushort.MinValue;
                        }
                    }
                }
                //Выделение диапазонов графика согласно мощности
                //Фильтрация в пределах 16-ти точек
                double sumPower = 0;
                int counterPower = 0;
                int startPossition = 0;
                for (int i = 0; i < _croppedArray.Length; i++)
                {
                    if (_croppedArray[i] != 0)
                    {
                        if (counterPower != 16)
                        {
                            sumPower += _croppedArray[i] - 1100;
                            counterPower++;
                        }
                        else
                        {
                            //Задает уровень учитываеммой мощности
                            sumPower = sumPower / counterPower * 0.2;
                            for (int j = startPossition; j < i; j++)
                            {
                                if (sumPower > _croppedArray[j] - 1100 && _croppedArray[j] != 0)
                                {
                                    _croppedArray[j] = 0;
                                }
                            }
                            sumPower = 0;
                            counterPower = 0;
                            startPossition = i;
                        }
                    }
                }
                //Выделение памяти под массив лимитации
                _croppedArray2 = new ushort[_croppedArray.Length];
                //Выделение диапазонов графика согласно мощности
                //УРОВЕНЬ Лимитации задается здесь
                // 1.2 = 80%, 2 = 50%
                for (int i = 0; i < _croppedArray.Length - 30; i++)
                {
                    if (_croppedArray[i] != 0)
                    {
                        for (int j = i; j < i + 30; j++)
                        {
                            if ((_dataGraph[j] - 1100) >= ((_croppedArray[i] - 1100) / 2.0)) //Double.Parse(cbLimitationLevel.Text)))
                            {
                                _croppedArray2[j] = _dataGraph[j];
                            }
                        }
                        for (int j = i - 30; j < i; j++)
                        {
                            if ((_dataGraph[j] - 1100) >= ((_croppedArray[i] - 1100) / 2.0)) //Double.Parse(cbLimitationLevel.Text)))
                            {
                                _croppedArray2[j] = _dataGraph[j];
                            }
                        }
                    }
                }
                _dataGraph6Limit = new double[_dataGraph.Length];
                //Заполняем массив лимитации 1 BadPointConst
                for (int i = 0; i < _dataGraph6Limit.Length; i++)
                {
                    _dataGraph6Limit[i] = (ushort) BadPointConst;
                }
                _dataGraph7Limit = new double[_dataGraph.Length];
                //Заполняем массив лимитации 2 BadPointConst
                for (int i = 0; i < _dataGraph7Limit.Length; i++)
                {
                    _dataGraph7Limit[i] = (ushort) BadPointConst;
                }
                //Формирование выборки точек лимитации на графиках 2 и 3
                for (int i = 0; i < _croppedArray2.Length; i++)
                {
                    if (_croppedArray2[i] != 0)
                    {
                        if (_dataGraph2[i] != 0)
                        {
                            _dataGraph6Limit[i] = _dataGraph2[i];
                        }

                        if (_dataGraph3[i] != 0)
                        {
                            _dataGraph7Limit[i] = _dataGraph3[i];
                        }
                    }
                }
                //Заполнения массива усредненной лимитации графика 1
                _dataGraph8Averaged = new double[_dataGraph6Limit.Length];
                for (int i = 0; i < _dataGraph8Averaged.Length; i++)
                {
                    _dataGraph8Averaged[i] = BadPointConst;
                }
                //Заполнения массива усредненной лимитации графика 2
                _dataGraph9Averaged = new double[_dataGraph7Limit.Length];
                for (int i = 0; i < _dataGraph9Averaged.Length; i++)
                {
                    _dataGraph9Averaged[i] = BadPointConst;
                }
                //Усреднение лимитации 1
                double dataSumAver1 = 0;
                int dataSumAver1Counter = 0;
                for (int i = 0; i < _dataGraph6Limit.Length; i++)
                {
                    if ((ushort)_dataGraph6Limit[i] != BadPointConst)
                    {
                        dataSumAver1 += _dataGraph6Limit[i];
                        dataSumAver1Counter++;
                    }
                    else
                    {
                        if (dataSumAver1Counter != 0)
                        {
                            _dataGraph8Averaged[i] = dataSumAver1 / dataSumAver1Counter;
                            dataSumAver1 = 0;
                            dataSumAver1Counter = 0;
                        }
                    }
                }
                //Усреднение лимитации 2
                double dataSumAver2 = 0;
                int dataSumAver2Counter = 0;
                for (int i = 0; i < _dataGraph7Limit.Length; i++)
                {
                    if ((ushort)_dataGraph7Limit[i] != BadPointConst)
                    {
                        dataSumAver2 += _dataGraph7Limit[i];
                        dataSumAver2Counter++;
                    }
                    else
                    {
                        if (dataSumAver2Counter != 0)
                        {
                            _dataGraph9Averaged[i] = dataSumAver2 / dataSumAver2Counter;
                            dataSumAver2 = 0;
                            dataSumAver2Counter = 0;
                        }
                    }
                }
                // Возможность отключения Фильтра
                // Значение времени 10_000
                if (_filter)
                {
                    //Фильтрация ошибочных точек График 1
                    int counter1 = 0;
                    for (int i = 0; i < _dataGraph8Averaged.Length; i++)
                    {
                        if ((ushort)_dataGraph8Averaged[i] != BadPointConst)
                        {
                            if ((_timePoint[i] - _timePoint[counter1]) >= 15_000 || (_timePoint[i] - _timePoint[counter1]) <= 5_000)
                            {
                                _dataGraph8Averaged[counter1] = BadPointConst;
                            }
                            counter1 = i;
                        }
                    }

                    //Фильтрация ошибочных точек График 2
                    int counter2 = 0;
                    for (int i = 0; i < _dataGraph9Averaged.Length; i++)
                    {
                        if ((ushort)_dataGraph9Averaged[i] != BadPointConst)
                        {
                            if ((_timePoint[i] - _timePoint[counter2]) >= 15_000 || (_timePoint[i] - _timePoint[counter1]) <= 5_000)
                            {
                                _dataGraph9Averaged[counter2] = BadPointConst;
                            }
                            counter2 = i;
                        }
                    }
                }
                //Формирование графиков коррекции график 1
                _dataGraph10Correction = new double[_dataGraph2.Length];
                for (int i = 0; i < _dataGraph10Correction.Length; i++)
                {
                    _dataGraph10Correction[i] = BadPointConst;
                }
                //Формирование графиков коррекции график 2
                _dataGraph11Correction = new double[_dataGraph3.Length];
                for (int i = 0; i < _dataGraph11Correction.Length; i++)
                {
                    _dataGraph11Correction[i] = BadPointConst;
                }
                // Устреднение коррекции график 1
                //Формирование массивов для режима поправка
                // Усреднение коррекции График 1
                for (int i = 0; i < _dataGraph2.Length - 3; i += 3)
                {
                    if (_dataGraph2[i] == _dataGraph2[i + 1] && _dataGraph2[i] == _dataGraph2[i + 2])
                    {
                        _dataGraph10Correction[i] = _dataGraph2[i];
                    }
                }
                // Усреднение коррекции График 2
                for (int i = 0; i < _dataGraph3.Length - 3; i += 3)
                {
                    if (_dataGraph3[i] == _dataGraph3[i + 1] && _dataGraph3[i] == _dataGraph3[i + 2])
                    {
                        _dataGraph11Correction[i] = _dataGraph3[i];
                    }
                }
                // Служебный
                // Отсечение шумов Служебного графика 1
                for (int i = 0; i < _dataGraphCounter4; i += 3)
                {
                    if (_dataGraph4[i] == _dataGraph4[i + 1] && _dataGraph4[i] == _dataGraph4[i + 2])
                    {
                        _dataGraph4[i + 1] = (ushort)BadPointConst;
                        _dataGraph4[i + 2] = (ushort)BadPointConst;
                    }
                    else
                    {
                        _dataGraph4[i] = (ushort)BadPointConst;
                        _dataGraph4[i + 1] = (ushort)BadPointConst;
                        _dataGraph4[i + 2] = (ushort)BadPointConst;
                    }
                }
                // Отсечение шумов Служебного графика 2
                for (int i = 0; i < _dataGraphCounter5; i += 3)
                {
                    if (_dataGraph5[i] == _dataGraph5[i + 1] && _dataGraph5[i] == _dataGraph5[i + 2])
                    {
                        _dataGraph5[i + 1] = (ushort)BadPointConst;
                        _dataGraph5[i + 2] = (ushort)BadPointConst;
                    }
                    else
                    {
                        _dataGraph5[i] = (ushort)BadPointConst;
                        _dataGraph5[i + 1] = (ushort)BadPointConst;
                        _dataGraph5[i + 2] = (ushort)BadPointConst;
                    }
                }
                //Служебный
                // Усреднение график 1
                int sumService = 0;
                int sumCounter = 0;
                for (int i = 0; i < _dataGraphCounter4; i++)
                {
                    if (_dataGraph4[i] != _dataGraph4[i + 1] ||
                        _dataGraph4[i] != _dataGraph4[i + 2] ||
                        _dataGraph4[i] != _dataGraph4[i + 3] ||
                        _dataGraph4[i] != _dataGraph4[i + 4] ||
                        _dataGraph4[i] != _dataGraph4[i + 5] ||
                        _dataGraph4[i] != _dataGraph4[i + 6] ||
                        _dataGraph4[i] != _dataGraph4[i + 7] ||
                        _dataGraph4[i] != _dataGraph4[i + 8] ||
                        _dataGraph4[i] != _dataGraph4[i + 9] ||
                        _dataGraph4[i] != _dataGraph4[i + 10])
                    {

                        if (sumCounter < 10)
                        {
                            if (_dataGraph4[i] != BadPointConst)
                            {
                                sumService += _dataGraph4[i];
                                sumCounter++;
                            }
                        }
                        else
                        {
                            sumService /= sumCounter;

                            int counter = 0;
                            int j = i;
                            while (counter < 10)
                            {
                                if (_dataGraph4[j] != BadPointConst)
                                {
                                    _dataGraph4[j] = (ushort)sumService;
                                    counter++;
                                }
                                j--;
                            }
                            sumService = 0;
                            sumCounter = 0;
                        }
                    }
                }
                // Усреднение график 2
                int sumService2 = 0;
                int sumCounter2 = 0;
                for (int i = 0; i < _dataGraphCounter5; i++)
                {
                    if (_dataGraph5[i] != _dataGraph5[i + 1] ||
                        _dataGraph5[i] != _dataGraph5[i + 2] ||
                        _dataGraph5[i] != _dataGraph5[i + 3] ||
                        _dataGraph5[i] != _dataGraph5[i + 4] ||
                        _dataGraph5[i] != _dataGraph5[i + 5] ||
                        _dataGraph5[i] != _dataGraph5[i + 6] ||
                        _dataGraph5[i] != _dataGraph5[i + 7] ||
                        _dataGraph5[i] != _dataGraph5[i + 8] ||
                        _dataGraph5[i] != _dataGraph5[i + 9] ||
                        _dataGraph5[i] != _dataGraph5[i + 10])
                    {
                        if (sumCounter2 < 10)
                        {
                            if (_dataGraph5[i] != BadPointConst)
                            {
                                sumService2 += _dataGraph5[i];
                                sumCounter2++;
                            }
                        }
                        else
                        {
                            sumService2 /= sumCounter2;

                            int counter2 = 0;
                            int k = i;
                            while (counter2 < 10)
                            {
                                if (_dataGraph5[k] != BadPointConst)
                                {
                                    _dataGraph5[k] = (ushort)sumService2;
                                    counter2++;
                                }
                                k--;
                            }
                            sumService2 = 0;
                            sumCounter2 = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        //Формирование времени
        private void TimeProcessing()
        {
            try
            {
                byte errorPackagesCounter = 0;
                int j = 0;
                if (_swatch.ElapsedMilliseconds > 0)
                {
                    _timebuffer = Convert.ToUInt64(_swatch.ElapsedMilliseconds * 1000);
                    _swatch.Reset();
                }
                for (int i = 0; i < _possitionCounter; i += 4)
                {
                    uint startByteCheck1 = _responce[i] & MaskFirstByteLogicConst;
                    uint startByteCheck2 = _responce[i + 1] & MaskFirstByteLogicConst;
                    uint startByteCheck3 = _responce[i + 2] & MaskFirstByteLogicConst;
                    uint startByteCheck4 = _responce[i + 3] & MaskFirstByteLogicConst;
                    {
                        if (startByteCheck1 != 0 && startByteCheck2 == 0 && startByteCheck3 == 0 && startByteCheck4 == 0)
                        {
                            _timePoint[j] = _timebuffer;
                            _timebuffer += Convert.ToUInt64(_responce[i + 3]);
                            j++;
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                error_counter_label.Text = "Errors:" + _errorCounter;
                                if (errorPackagesCounter == 4)
                                {
                                    errorPackagesCounter = 0;
                                    _errorCounter++;
                                }
                                errorPackagesCounter++;
                                // ReSharper disable once AccessToModifiedClosure
                                i -= 3;
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        // ! В случае исключительной ситуации, полный сброс переменных
        private void DataReset()
        {
            try
            {
                // Очистка всех существующих массивов в случае сбоя программы
                Array.Clear(_command, 0, _command.Length);

                if (_dataGraph != null)
                    Array.Clear(_dataGraph, 0, _dataGraph.Length);
                if (_dataGraph2 != null)
                    Array.Clear(_dataGraph2, 0, _dataGraph2.Length);
                if (_dataGraph3 != null)
                    Array.Clear(_dataGraph3, 0, _dataGraph3.Length);

                Array.Clear(_dataGraph4, 0, _dataGraph4.Length);
                Array.Clear(_dataGraph5, 0, _dataGraph5.Length);

                if (_dataGraph6Limit != null)
                    Array.Clear(_dataGraph6Limit, 0, _dataGraph6Limit.Length);
                if (_dataGraph7Limit != null)
                    Array.Clear(_dataGraph7Limit, 0, _dataGraph7Limit.Length);
                if (_dataGraph8Averaged != null)
                    Array.Clear(_dataGraph8Averaged, 0, _dataGraph8Averaged.Length);
                if (_dataGraph9Averaged != null)
                    Array.Clear(_dataGraph9Averaged, 0, _dataGraph9Averaged.Length);
                if (_dataGraph10Correction != null)
                    Array.Clear(_dataGraph10Correction, 0, _dataGraph10Correction.Length);
                if (_dataGraph11Correction != null)
                    Array.Clear(_dataGraph11Correction, 0, _dataGraph11Correction.Length);
                if (_croppedArray != null)
                    Array.Clear(_croppedArray, 0, _croppedArray.Length);
                if (_croppedArray2 != null)
                    Array.Clear(_croppedArray2, 0, _croppedArray2.Length);

                Array.Clear(_dataGraph10ServiceLimitation, 0, _dataGraph10ServiceLimitation.Length);
                Array.Clear(_dataGraph11ServiceLimitation, 0, _dataGraph11ServiceLimitation.Length);
                Array.Clear(_responce, 0, _responce.Length);
                Array.Clear(_timePoint, 0, _timePoint.Length);
                Array.Clear(_timePoint4, 0, _timePoint4.Length);
                Array.Clear(_timePoint5, 0, _timePoint5.Length);
                Array.Clear(_timePoint10ServiceLimitation, 0, _timePoint10ServiceLimitation.Length);
                Array.Clear(_timePoint11ServiceLimitation, 0, _timePoint11ServiceLimitation.Length);

                // Очистка вспомогательных переменных
                _scanningstatus = false;
                dataResiveProgressBar.Value = 1;
                dataResiveProgressBar.Visible = false;
                _timerCounter = 0;
                _possitionCounter = 0;
                _dataGraphCounter4 = 0;
                _dataGraphCounter5 = 0;
                _timebuffer = 0;
                _timer30Sec = 1;

                // Если есть что удалять
                if (_pane1.CurveList.Count > 0)
                {
                    // Удалим все кривые
                    _pane1.CurveList.Clear();
                    // Синхронизация графиков по масштабам оси Х и Y
                    _pane1.XAxis.Scale.Min = XScaleMin;
                    _pane1.XAxis.Scale.Max = XScaleMax;
                }
                if (_pane2.CurveList.Count > 0 && _pane3.CurveList.Count > 0)
                {
                    _pane2.CurveList.Clear();
                    _pane3.CurveList.Clear();
                    _pane2.XAxis.Scale.Min = XScaleMin;
                    _pane3.XAxis.Scale.Min = XScaleMin;
                    _pane2.XAxis.Scale.Max = XScaleMax;
                    _pane3.XAxis.Scale.Max = XScaleMax;
                }

                // Обновим график
                zedGraph.AxisChange();
                zedGraph.Invalidate();

                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                }
            }
            catch (Exception ex)
            {
                Invoke(new MethodInvoker(delegate
                    {
                        error_TextBox.Text += "\r\n [" + DateTime.Now + "]" + ex.Message;
                    }
                ));
            }
        }
        //! Таймер
        private void timerCount_Tick(object sender, EventArgs e)
        {
            dataResiveCounterTest_label.Text = "Packages per sec: " + (_timerCounter);
            packages_counter_label.Text = "Bytes resived: " + _possitionCounter;
            _timerCounter = 0;
            if (_scanningstatus)
            {
                dataResiveProgressBar.PerformStep();
                _timer30Sec++;
            }
            if (_timer30Sec == 30)
            {
                _swatch.Stop();
                Thread dataThread = new Thread(DataProcessing) {IsBackground = true};
                dataThread.Start();
            }
        }
        #endregion

        #region Measure (Time) 
        //Выделение точки для измерений
        private void zedGraph_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkBoxMeasuring.Checked)
            {
                if (rbtnMeasureGraph1.Checked)
                {
                    DistinguishPointGraph1(e);
                }
                else
                    if (rbtnMeasureGraph2.Checked)
                    {
                        DistinguishPointGraph2(e);
                    }
                    else
                        if (rbtnMeasureGraph3.Checked)
                        {
                            DistinguishPointGraph3(e);
                        }
            }
                zedGraph.Invalidate();
         }
        //Отрисовка выделенной точки на графике 1 для измерений
        private void DistinguishPointGraph1(MouseEventArgs e)
        {
            if (_measPoint1Flag || _measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = _pane1.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    // ReSharper disable once ObjectCreationAsStatement
                    new PointPairList {curve[index]};


                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = _pane1.AddCurve("",
                        new [] { curve[index].X },
                        new [] { curve[index].Y },
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

                    if (_measPoint1Flag)
                    {
                        _curveToDelete1 = curvePount;
                        _measPointX1 = curve[index].X;
                        _measPointY1 = curve[index].Y;
                        _measPoint1Flag = false;
                    }
                    else
                    {
                        _curveToDelete2 = curvePount;
                        _measPointX2 = curve[index].X;
                        _measPointY2 = curve[index].Y;
                        _measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((_measPointX2 - _measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((_measPointY2 - _measPointY1)).ToString("F2");
                    }

                }
            }
        }
        //Отрисовка выделенной точки на графике 2 для измерений
        private void DistinguishPointGraph2(MouseEventArgs e)
        {
            if (_measPoint1Flag || _measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = _pane2.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    PointPairList point = new PointPairList();

                    point.Add(curve[index]);

                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = _pane2.AddCurve("",
                        new [] { curve[index].X },
                        new [] { curve[index].Y },
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

                    if (_measPoint1Flag)
                    {
                        _curveToDelete1 = curvePount;
                        _measPointX1 = curve[index].X;
                        _measPointY1 = curve[index].Y;
                        _measPoint1Flag = false;
                    }
                    else
                    {
                        _curveToDelete2 = curvePount;
                        _measPointX2 = curve[index].X;
                        _measPointY2 = curve[index].Y;
                        _measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((_measPointX2 - _measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((_measPointY2 - _measPointY1)).ToString("F2");
                    }

                }
            }
        }
        //Отрисовка выделенной точки на графике 3 для измерений
        private void DistinguishPointGraph3(MouseEventArgs e)
        {
            if (_measPoint1Flag || _measPoint2Flag)
            {
                // Сюда будет сохранена кривая, рядом с которой был произведен клик
                CurveItem curve;

                // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
                int index;

                // Максимальное расстояние от точки клика до кривой в пикселях, 
                // при котором еще считается, что клик попал в окрестность кривой.
                GraphPane.Default.NearestTol = 10;

                bool result = _pane3.FindNearestPoint(e.Location, out curve, out index);

                if (result)
                {
                    // Максимально расстояние от точки клика до кривой не превысило NearestTol

                    // Добавим точку на график, вблизи которой произошел клик
                    PointPairList point = new PointPairList();

                    point.Add(curve[index]);

                    // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                    LineItem curvePount = _pane3.AddCurve("",
                        new [] { curve[index].X },
                        new [] { curve[index].Y },
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

                    if (_measPoint1Flag)
                    {
                        _curveToDelete1 = curvePount;
                        _measPointX1 = curve[index].X;
                        _measPointY1 = curve[index].Y;
                        _measPoint1Flag = false;
                    }
                    else
                    {
                        _curveToDelete2 = curvePount;
                        _measPointX2 = curve[index].X;
                        _measPointY2 = curve[index].Y;
                        _measPoint2Flag = false;
                        tbTimeCounted.Text = Math.Abs((_measPointX2 - _measPointX1) / 1000000).ToString("F6");
                        tbYCounted.Text = Math.Abs((_measPointY2 - _measPointY1)).ToString("F2");
                    }

                }
            }   
        }
        //Кнопка сдаления точек измерений на графике
        private void bntResetTimePoints_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtnMeasureGraph1.Checked)
                {
                    _pane1.CurveList.Remove(_curveToDelete1);
                    _pane1.CurveList.Remove(_curveToDelete2);
                }

                if (rbtnMeasureGraph2.Checked)
                {
                    _pane2.CurveList.Remove(_curveToDelete1);
                    _pane2.CurveList.Remove(_curveToDelete2);
                }

                if (rbtnMeasureGraph3.Checked)
                {
                    _pane3.CurveList.Remove(_curveToDelete1);
                    _pane3.CurveList.Remove(_curveToDelete2);
                }

            }
            finally
            {
                _measPointX1 = 0;
                _measPointX2 = 0;
                _measPoint1Flag = true;
                _measPoint2Flag = true;
                zedGraph.Invalidate();
                tbTimeCounted.Text = ""; 
                tbYCounted.Text = "";
            }
        }
        //Разрешить измерение времени
        private void checkBoxMeasuring_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMeasuring.Checked)
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