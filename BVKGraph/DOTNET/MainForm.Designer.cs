namespace DOTNET
{
    partial class WorkForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkForm));
            this.connect_Button = new System.Windows.Forms.Button();
            this.port_Combobox = new System.Windows.Forms.ComboBox();
            this.port_Label = new System.Windows.Forms.Label();
            this.baudrate_Combobox = new System.Windows.Forms.ComboBox();
            this.baudrate_label = new System.Windows.Forms.Label();
            this.parity_Combobox = new System.Windows.Forms.ComboBox();
            this.parity_Label = new System.Windows.Forms.Label();
            this.stopbits_Combobox = new System.Windows.Forms.ComboBox();
            this.stopbits_Label = new System.Windows.Forms.Label();
            this.databits_Combobox = new System.Windows.Forms.ComboBox();
            this.databits_Label = new System.Windows.Forms.Label();
            this.disconnect_Button = new System.Windows.Forms.Button();
            this.startbvk_button = new System.Windows.Forms.Button();
            this.stopbvk_button = new System.Windows.Forms.Button();
            this.starttest_button = new System.Windows.Forms.Button();
            this.limitation_button = new System.Windows.Forms.Button();
            this.derivation_button = new System.Windows.Forms.Button();
            this.error_TextBox = new System.Windows.Forms.TextBox();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.cleangraph_button = new System.Windows.Forms.Button();
            this.dataResiveCounterTest_label = new System.Windows.Forms.Label();
            this.timerCount = new System.Windows.Forms.Timer(this.components);
            this.dataResiveProgressBar = new System.Windows.Forms.ProgressBar();
            this.normalmode_button = new System.Windows.Forms.Button();
            this.engineeringmode_button = new System.Windows.Forms.Button();
            this.kdo_button = new System.Windows.Forms.Button();
            this.yac1_button = new System.Windows.Forms.Button();
            this.yac2_button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.load_button = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.packages_counter_label = new System.Windows.Forms.Label();
            this.error_counter_label = new System.Windows.Forms.Label();
            this.clean_error_linkLabel = new System.Windows.Forms.LinkLabel();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.graph_groupBox = new System.Windows.Forms.GroupBox();
            this.btnResetGraph = new System.Windows.Forms.Button();
            this.control_groupBox = new System.Windows.Forms.GroupBox();
            this.btnServiceMode = new System.Windows.Forms.Button();
            this.Sync_checkBox = new System.Windows.Forms.CheckBox();
            this.com_tabControl = new System.Windows.Forms.TabControl();
            this.com1_tabPage = new System.Windows.Forms.TabPage();
            this.lbBVK = new System.Windows.Forms.Label();
            this.com2_tabPage = new System.Windows.Forms.TabPage();
            this.lbInfoSync = new System.Windows.Forms.Label();
            this.disconnect2_button = new System.Windows.Forms.Button();
            this.connect2_button = new System.Windows.Forms.Button();
            this.port2_Combobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.baudrate2_Combobox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.parity2_Combobox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.stopbits2_Combobox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.databits2_Combobox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.control_tabPage = new System.Windows.Forms.TabPage();
            this.measure_tabPage = new System.Windows.Forms.TabPage();
            this.cbAdmission = new System.Windows.Forms.CheckBox();
            this.gbScale = new System.Windows.Forms.GroupBox();
            this.btnScale9 = new System.Windows.Forms.Button();
            this.btnScale17 = new System.Windows.Forms.Button();
            this.btnScale35 = new System.Windows.Forms.Button();
            this.gbLimitation2 = new System.Windows.Forms.GroupBox();
            this.btnChangeFrame = new System.Windows.Forms.Button();
            this.btnLimitation2 = new System.Windows.Forms.Button();
            this.gbAmplitude = new System.Windows.Forms.GroupBox();
            this.btnShowAmpl = new System.Windows.Forms.Button();
            this.labXInfo = new System.Windows.Forms.Label();
            this.labYInfo = new System.Windows.Forms.Label();
            this.tbYCounted = new System.Windows.Forms.TextBox();
            this.tbTimeCounted = new System.Windows.Forms.TextBox();
            this.bntResetTimePoints = new System.Windows.Forms.Button();
            this.rbtnMeasureGraph3 = new System.Windows.Forms.RadioButton();
            this.rbtnMeasureGraph2 = new System.Windows.Forms.RadioButton();
            this.rbtnMeasureGraph1 = new System.Windows.Forms.RadioButton();
            this.checkBoxMeasuring = new System.Windows.Forms.CheckBox();
            this.comstatus2_label = new System.Windows.Forms.Label();
            this.comstatus_label = new System.Windows.Forms.Label();
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.syncstatus_label = new System.Windows.Forms.Label();
            this.timeGraphName_label = new System.Windows.Forms.Label();
            this.graph_groupBox.SuspendLayout();
            this.control_groupBox.SuspendLayout();
            this.com_tabControl.SuspendLayout();
            this.com1_tabPage.SuspendLayout();
            this.com2_tabPage.SuspendLayout();
            this.control_tabPage.SuspendLayout();
            this.measure_tabPage.SuspendLayout();
            this.gbScale.SuspendLayout();
            this.gbLimitation2.SuspendLayout();
            this.gbAmplitude.SuspendLayout();
            this.SuspendLayout();
            // 
            // connect_Button
            // 
            this.connect_Button.Location = new System.Drawing.Point(6, 27);
            this.connect_Button.Name = "connect_Button";
            this.connect_Button.Size = new System.Drawing.Size(66, 37);
            this.connect_Button.TabIndex = 0;
            this.connect_Button.Text = "Connect";
            this.connect_Button.UseVisualStyleBackColor = true;
            this.connect_Button.Click += new System.EventHandler(this.connect_Button_Click);
            // 
            // port_Combobox
            // 
            this.port_Combobox.DisplayMember = "1";
            this.port_Combobox.FormattingEnabled = true;
            this.port_Combobox.Location = new System.Drawing.Point(78, 40);
            this.port_Combobox.Name = "port_Combobox";
            this.port_Combobox.Size = new System.Drawing.Size(73, 21);
            this.port_Combobox.TabIndex = 1;
            // 
            // port_Label
            // 
            this.port_Label.AutoSize = true;
            this.port_Label.Location = new System.Drawing.Point(75, 24);
            this.port_Label.Name = "port_Label";
            this.port_Label.Size = new System.Drawing.Size(26, 13);
            this.port_Label.TabIndex = 2;
            this.port_Label.Text = "Port";
            // 
            // baudrate_Combobox
            // 
            this.baudrate_Combobox.FormattingEnabled = true;
            this.baudrate_Combobox.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.baudrate_Combobox.Location = new System.Drawing.Point(160, 40);
            this.baudrate_Combobox.Name = "baudrate_Combobox";
            this.baudrate_Combobox.Size = new System.Drawing.Size(73, 21);
            this.baudrate_Combobox.TabIndex = 3;
            // 
            // baudrate_label
            // 
            this.baudrate_label.AutoSize = true;
            this.baudrate_label.Location = new System.Drawing.Point(157, 24);
            this.baudrate_label.Name = "baudrate_label";
            this.baudrate_label.Size = new System.Drawing.Size(30, 13);
            this.baudrate_label.TabIndex = 4;
            this.baudrate_label.Text = "Rate";
            // 
            // parity_Combobox
            // 
            this.parity_Combobox.FormattingEnabled = true;
            this.parity_Combobox.Location = new System.Drawing.Point(244, 40);
            this.parity_Combobox.Name = "parity_Combobox";
            this.parity_Combobox.Size = new System.Drawing.Size(73, 21);
            this.parity_Combobox.TabIndex = 5;
            // 
            // parity_Label
            // 
            this.parity_Label.AutoSize = true;
            this.parity_Label.Location = new System.Drawing.Point(241, 24);
            this.parity_Label.Name = "parity_Label";
            this.parity_Label.Size = new System.Drawing.Size(33, 13);
            this.parity_Label.TabIndex = 6;
            this.parity_Label.Text = "Parity";
            // 
            // stopbits_Combobox
            // 
            this.stopbits_Combobox.FormattingEnabled = true;
            this.stopbits_Combobox.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.stopbits_Combobox.Location = new System.Drawing.Point(325, 40);
            this.stopbits_Combobox.Name = "stopbits_Combobox";
            this.stopbits_Combobox.Size = new System.Drawing.Size(73, 21);
            this.stopbits_Combobox.TabIndex = 7;
            // 
            // stopbits_Label
            // 
            this.stopbits_Label.AutoSize = true;
            this.stopbits_Label.Location = new System.Drawing.Point(322, 24);
            this.stopbits_Label.Name = "stopbits_Label";
            this.stopbits_Label.Size = new System.Drawing.Size(48, 13);
            this.stopbits_Label.TabIndex = 8;
            this.stopbits_Label.Text = "Stop bits";
            // 
            // databits_Combobox
            // 
            this.databits_Combobox.FormattingEnabled = true;
            this.databits_Combobox.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.databits_Combobox.Location = new System.Drawing.Point(406, 40);
            this.databits_Combobox.Name = "databits_Combobox";
            this.databits_Combobox.Size = new System.Drawing.Size(73, 21);
            this.databits_Combobox.TabIndex = 9;
            // 
            // databits_Label
            // 
            this.databits_Label.AutoSize = true;
            this.databits_Label.Location = new System.Drawing.Point(403, 24);
            this.databits_Label.Name = "databits_Label";
            this.databits_Label.Size = new System.Drawing.Size(49, 13);
            this.databits_Label.TabIndex = 10;
            this.databits_Label.Text = "Data bits";
            // 
            // disconnect_Button
            // 
            this.disconnect_Button.Location = new System.Drawing.Point(485, 27);
            this.disconnect_Button.Name = "disconnect_Button";
            this.disconnect_Button.Size = new System.Drawing.Size(76, 37);
            this.disconnect_Button.TabIndex = 13;
            this.disconnect_Button.Text = "Disconnect";
            this.disconnect_Button.UseVisualStyleBackColor = true;
            this.disconnect_Button.Click += new System.EventHandler(this.disconnect_Button_Click);
            // 
            // startbvk_button
            // 
            this.startbvk_button.Location = new System.Drawing.Point(465, 17);
            this.startbvk_button.Name = "startbvk_button";
            this.startbvk_button.Size = new System.Drawing.Size(80, 23);
            this.startbvk_button.TabIndex = 15;
            this.startbvk_button.Text = "Запись ХР";
            this.startbvk_button.UseVisualStyleBackColor = true;
            this.startbvk_button.Click += new System.EventHandler(this.startbvk_button_Click);
            // 
            // stopbvk_button
            // 
            this.stopbvk_button.BackColor = System.Drawing.Color.LightCoral;
            this.stopbvk_button.Location = new System.Drawing.Point(6, 17);
            this.stopbvk_button.Name = "stopbvk_button";
            this.stopbvk_button.Size = new System.Drawing.Size(50, 52);
            this.stopbvk_button.TabIndex = 16;
            this.stopbvk_button.Text = "Стоп БВК";
            this.stopbvk_button.UseVisualStyleBackColor = false;
            this.stopbvk_button.Click += new System.EventHandler(this.stopbvk_button_Click);
            // 
            // starttest_button
            // 
            this.starttest_button.Location = new System.Drawing.Point(143, 46);
            this.starttest_button.Name = "starttest_button";
            this.starttest_button.Size = new System.Drawing.Size(75, 23);
            this.starttest_button.TabIndex = 17;
            this.starttest_button.Text = "Тест БВК";
            this.starttest_button.UseVisualStyleBackColor = true;
            this.starttest_button.Click += new System.EventHandler(this.starttest_button_Click);
            // 
            // limitation_button
            // 
            this.limitation_button.BackColor = System.Drawing.Color.Khaki;
            this.limitation_button.Location = new System.Drawing.Point(384, 17);
            this.limitation_button.Name = "limitation_button";
            this.limitation_button.Size = new System.Drawing.Size(75, 23);
            this.limitation_button.TabIndex = 19;
            this.limitation_button.Text = "Уров. 0,5";
            this.limitation_button.UseVisualStyleBackColor = false;
            this.limitation_button.Click += new System.EventHandler(this.limitation_button_Click);
            // 
            // derivation_button
            // 
            this.derivation_button.Location = new System.Drawing.Point(465, 46);
            this.derivation_button.Name = "derivation_button";
            this.derivation_button.Size = new System.Drawing.Size(80, 23);
            this.derivation_button.TabIndex = 20;
            this.derivation_button.Text = "Запись ДРВ";
            this.derivation_button.UseVisualStyleBackColor = true;
            this.derivation_button.Click += new System.EventHandler(this.derivation_button_Click);
            // 
            // error_TextBox
            // 
            this.error_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.error_TextBox.Location = new System.Drawing.Point(7, 654);
            this.error_TextBox.Multiline = true;
            this.error_TextBox.Name = "error_TextBox";
            this.error_TextBox.ReadOnly = true;
            this.error_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.error_TextBox.Size = new System.Drawing.Size(830, 36);
            this.error_TextBox.TabIndex = 22;
            // 
            // zedGraph
            // 
            this.zedGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zedGraph.Location = new System.Drawing.Point(7, 115);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(830, 492);
            this.zedGraph.TabIndex = 25;
            this.zedGraph.Load += new System.EventHandler(this.zedGraph_Load);
            this.zedGraph.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseDoubleClick);
            // 
            // cleangraph_button
            // 
            this.cleangraph_button.Location = new System.Drawing.Point(94, 17);
            this.cleangraph_button.Name = "cleangraph_button";
            this.cleangraph_button.Size = new System.Drawing.Size(75, 23);
            this.cleangraph_button.TabIndex = 26;
            this.cleangraph_button.Text = "Очистить";
            this.cleangraph_button.UseVisualStyleBackColor = true;
            this.cleangraph_button.Click += new System.EventHandler(this.cleangraph_button_Click);
            // 
            // dataResiveCounterTest_label
            // 
            this.dataResiveCounterTest_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataResiveCounterTest_label.AutoSize = true;
            this.dataResiveCounterTest_label.Location = new System.Drawing.Point(309, 693);
            this.dataResiveCounterTest_label.Name = "dataResiveCounterTest_label";
            this.dataResiveCounterTest_label.Size = new System.Drawing.Size(105, 13);
            this.dataResiveCounterTest_label.TabIndex = 30;
            this.dataResiveCounterTest_label.Text = "Packages per sec: 0";
            // 
            // timerCount
            // 
            this.timerCount.Enabled = true;
            this.timerCount.Interval = 1000;
            this.timerCount.Tick += new System.EventHandler(this.timerCount_Tick);
            // 
            // dataResiveProgressBar
            // 
            this.dataResiveProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataResiveProgressBar.Location = new System.Drawing.Point(7, 625);
            this.dataResiveProgressBar.Name = "dataResiveProgressBar";
            this.dataResiveProgressBar.Size = new System.Drawing.Size(830, 23);
            this.dataResiveProgressBar.TabIndex = 32;
            this.dataResiveProgressBar.Visible = false;
            // 
            // normalmode_button
            // 
            this.normalmode_button.BackColor = System.Drawing.Color.YellowGreen;
            this.normalmode_button.Location = new System.Drawing.Point(62, 17);
            this.normalmode_button.Name = "normalmode_button";
            this.normalmode_button.Size = new System.Drawing.Size(75, 23);
            this.normalmode_button.TabIndex = 33;
            this.normalmode_button.Text = "Координ.";
            this.normalmode_button.UseVisualStyleBackColor = false;
            this.normalmode_button.Click += new System.EventHandler(this.normalmode_button_Click);
            // 
            // engineeringmode_button
            // 
            this.engineeringmode_button.Location = new System.Drawing.Point(223, 17);
            this.engineeringmode_button.Name = "engineeringmode_button";
            this.engineeringmode_button.Size = new System.Drawing.Size(75, 52);
            this.engineeringmode_button.TabIndex = 34;
            this.engineeringmode_button.Text = "Инженер.";
            this.engineeringmode_button.UseVisualStyleBackColor = true;
            this.engineeringmode_button.Click += new System.EventHandler(this.engineeringmode_button_Click);
            // 
            // kdo_button
            // 
            this.kdo_button.Location = new System.Drawing.Point(62, 46);
            this.kdo_button.Name = "kdo_button";
            this.kdo_button.Size = new System.Drawing.Size(75, 23);
            this.kdo_button.TabIndex = 35;
            this.kdo_button.Text = "Поправки";
            this.kdo_button.UseVisualStyleBackColor = true;
            this.kdo_button.Click += new System.EventHandler(this.kdo_button_Click);
            // 
            // yac1_button
            // 
            this.yac1_button.BackColor = System.Drawing.Color.PaleTurquoise;
            this.yac1_button.Location = new System.Drawing.Point(304, 17);
            this.yac1_button.Name = "yac1_button";
            this.yac1_button.Size = new System.Drawing.Size(75, 23);
            this.yac1_button.TabIndex = 36;
            this.yac1_button.Text = "КОД1";
            this.yac1_button.UseVisualStyleBackColor = false;
            this.yac1_button.Click += new System.EventHandler(this.yac1_button_Click);
            // 
            // yac2_button
            // 
            this.yac2_button.Location = new System.Drawing.Point(304, 46);
            this.yac2_button.Name = "yac2_button";
            this.yac2_button.Size = new System.Drawing.Size(75, 23);
            this.yac2_button.TabIndex = 37;
            this.yac2_button.Text = "КОД2";
            this.yac2_button.UseVisualStyleBackColor = true;
            this.yac2_button.Click += new System.EventHandler(this.yac2_button_Click);
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(94, 46);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(75, 23);
            this.save_button.TabIndex = 38;
            this.save_button.Text = "Сохранить";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // load_button
            // 
            this.load_button.Location = new System.Drawing.Point(13, 46);
            this.load_button.Name = "load_button";
            this.load_button.Size = new System.Drawing.Size(75, 23);
            this.load_button.TabIndex = 39;
            this.load_button.Text = "Открыть";
            this.load_button.UseVisualStyleBackColor = true;
            this.load_button.Click += new System.EventHandler(this.load_button_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // packages_counter_label
            // 
            this.packages_counter_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.packages_counter_label.AutoSize = true;
            this.packages_counter_label.Location = new System.Drawing.Point(435, 693);
            this.packages_counter_label.Name = "packages_counter_label";
            this.packages_counter_label.Size = new System.Drawing.Size(82, 13);
            this.packages_counter_label.TabIndex = 40;
            this.packages_counter_label.Text = "Bytes resived: 0";
            // 
            // error_counter_label
            // 
            this.error_counter_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.error_counter_label.AutoSize = true;
            this.error_counter_label.Location = new System.Drawing.Point(559, 693);
            this.error_counter_label.Name = "error_counter_label";
            this.error_counter_label.Size = new System.Drawing.Size(46, 13);
            this.error_counter_label.TabIndex = 41;
            this.error_counter_label.Text = "Errors: 0";
            // 
            // clean_error_linkLabel
            // 
            this.clean_error_linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clean_error_linkLabel.AutoSize = true;
            this.clean_error_linkLabel.Location = new System.Drawing.Point(788, 693);
            this.clean_error_linkLabel.Name = "clean_error_linkLabel";
            this.clean_error_linkLabel.Size = new System.Drawing.Size(49, 13);
            this.clean_error_linkLabel.TabIndex = 42;
            this.clean_error_linkLabel.TabStop = true;
            this.clean_error_linkLabel.Text = "Clean list";
            this.clean_error_linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clean_error_linkLabel_LinkClicked);
            // 
            // graph_groupBox
            // 
            this.graph_groupBox.Controls.Add(this.btnResetGraph);
            this.graph_groupBox.Controls.Add(this.save_button);
            this.graph_groupBox.Controls.Add(this.load_button);
            this.graph_groupBox.Controls.Add(this.cleangraph_button);
            this.graph_groupBox.Location = new System.Drawing.Point(568, 3);
            this.graph_groupBox.Name = "graph_groupBox";
            this.graph_groupBox.Size = new System.Drawing.Size(178, 81);
            this.graph_groupBox.TabIndex = 43;
            this.graph_groupBox.TabStop = false;
            this.graph_groupBox.Text = "Графики";
            // 
            // btnResetGraph
            // 
            this.btnResetGraph.Location = new System.Drawing.Point(13, 17);
            this.btnResetGraph.Name = "btnResetGraph";
            this.btnResetGraph.Size = new System.Drawing.Size(75, 23);
            this.btnResetGraph.TabIndex = 40;
            this.btnResetGraph.Text = "Обновить";
            this.btnResetGraph.UseVisualStyleBackColor = true;
            this.btnResetGraph.Click += new System.EventHandler(this.btnResetGraph_Click);
            // 
            // control_groupBox
            // 
            this.control_groupBox.Controls.Add(this.btnServiceMode);
            this.control_groupBox.Controls.Add(this.Sync_checkBox);
            this.control_groupBox.Controls.Add(this.stopbvk_button);
            this.control_groupBox.Controls.Add(this.startbvk_button);
            this.control_groupBox.Controls.Add(this.derivation_button);
            this.control_groupBox.Controls.Add(this.starttest_button);
            this.control_groupBox.Controls.Add(this.limitation_button);
            this.control_groupBox.Controls.Add(this.yac2_button);
            this.control_groupBox.Controls.Add(this.yac1_button);
            this.control_groupBox.Controls.Add(this.kdo_button);
            this.control_groupBox.Controls.Add(this.engineeringmode_button);
            this.control_groupBox.Controls.Add(this.normalmode_button);
            this.control_groupBox.Location = new System.Drawing.Point(3, 3);
            this.control_groupBox.Name = "control_groupBox";
            this.control_groupBox.Size = new System.Drawing.Size(559, 81);
            this.control_groupBox.TabIndex = 44;
            this.control_groupBox.TabStop = false;
            this.control_groupBox.Text = "Управление";
            // 
            // btnServiceMode
            // 
            this.btnServiceMode.Location = new System.Drawing.Point(143, 17);
            this.btnServiceMode.Name = "btnServiceMode";
            this.btnServiceMode.Size = new System.Drawing.Size(75, 23);
            this.btnServiceMode.TabIndex = 51;
            this.btnServiceMode.Text = "Служебные";
            this.btnServiceMode.UseVisualStyleBackColor = true;
            this.btnServiceMode.Click += new System.EventHandler(this.btnServiceMode_Click);
            // 
            // Sync_checkBox
            // 
            this.Sync_checkBox.Appearance = System.Windows.Forms.Appearance.Button;
            this.Sync_checkBox.Location = new System.Drawing.Point(384, 46);
            this.Sync_checkBox.Name = "Sync_checkBox";
            this.Sync_checkBox.Size = new System.Drawing.Size(75, 23);
            this.Sync_checkBox.TabIndex = 50;
            this.Sync_checkBox.Text = "Вкл. синхр.";
            this.Sync_checkBox.UseVisualStyleBackColor = true;
            this.Sync_checkBox.CheckedChanged += new System.EventHandler(this.Sync_checkBox_CheckedChanged);
            // 
            // com_tabControl
            // 
            this.com_tabControl.Controls.Add(this.com1_tabPage);
            this.com_tabControl.Controls.Add(this.com2_tabPage);
            this.com_tabControl.Controls.Add(this.control_tabPage);
            this.com_tabControl.Controls.Add(this.measure_tabPage);
            this.com_tabControl.Location = new System.Drawing.Point(36, -1);
            this.com_tabControl.Name = "com_tabControl";
            this.com_tabControl.SelectedIndex = 0;
            this.com_tabControl.Size = new System.Drawing.Size(760, 114);
            this.com_tabControl.TabIndex = 46;
            // 
            // com1_tabPage
            // 
            this.com1_tabPage.BackColor = System.Drawing.SystemColors.Control;
            this.com1_tabPage.Controls.Add(this.lbBVK);
            this.com1_tabPage.Controls.Add(this.disconnect_Button);
            this.com1_tabPage.Controls.Add(this.connect_Button);
            this.com1_tabPage.Controls.Add(this.port_Combobox);
            this.com1_tabPage.Controls.Add(this.port_Label);
            this.com1_tabPage.Controls.Add(this.baudrate_Combobox);
            this.com1_tabPage.Controls.Add(this.baudrate_label);
            this.com1_tabPage.Controls.Add(this.parity_Combobox);
            this.com1_tabPage.Controls.Add(this.parity_Label);
            this.com1_tabPage.Controls.Add(this.stopbits_Combobox);
            this.com1_tabPage.Controls.Add(this.stopbits_Label);
            this.com1_tabPage.Controls.Add(this.databits_Combobox);
            this.com1_tabPage.Controls.Add(this.databits_Label);
            this.com1_tabPage.Location = new System.Drawing.Point(4, 22);
            this.com1_tabPage.Name = "com1_tabPage";
            this.com1_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.com1_tabPage.Size = new System.Drawing.Size(752, 88);
            this.com1_tabPage.TabIndex = 0;
            this.com1_tabPage.Text = "Порт 1";
            // 
            // lbBVK
            // 
            this.lbBVK.AutoSize = true;
            this.lbBVK.Location = new System.Drawing.Point(6, 3);
            this.lbBVK.Name = "lbBVK";
            this.lbBVK.Size = new System.Drawing.Size(74, 13);
            this.lbBVK.TabIndex = 14;
            this.lbBVK.Text = "Обмен с БВК";
            // 
            // com2_tabPage
            // 
            this.com2_tabPage.BackColor = System.Drawing.Color.Gainsboro;
            this.com2_tabPage.Controls.Add(this.lbInfoSync);
            this.com2_tabPage.Controls.Add(this.disconnect2_button);
            this.com2_tabPage.Controls.Add(this.connect2_button);
            this.com2_tabPage.Controls.Add(this.port2_Combobox);
            this.com2_tabPage.Controls.Add(this.label1);
            this.com2_tabPage.Controls.Add(this.baudrate2_Combobox);
            this.com2_tabPage.Controls.Add(this.label2);
            this.com2_tabPage.Controls.Add(this.parity2_Combobox);
            this.com2_tabPage.Controls.Add(this.label3);
            this.com2_tabPage.Controls.Add(this.stopbits2_Combobox);
            this.com2_tabPage.Controls.Add(this.label4);
            this.com2_tabPage.Controls.Add(this.databits2_Combobox);
            this.com2_tabPage.Controls.Add(this.label5);
            this.com2_tabPage.Location = new System.Drawing.Point(4, 22);
            this.com2_tabPage.Name = "com2_tabPage";
            this.com2_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.com2_tabPage.Size = new System.Drawing.Size(752, 88);
            this.com2_tabPage.TabIndex = 1;
            this.com2_tabPage.Text = "Порт 2";
            // 
            // lbInfoSync
            // 
            this.lbInfoSync.AutoSize = true;
            this.lbInfoSync.Location = new System.Drawing.Point(6, 3);
            this.lbInfoSync.Name = "lbInfoSync";
            this.lbInfoSync.Size = new System.Drawing.Size(77, 13);
            this.lbInfoSync.TabIndex = 26;
            this.lbInfoSync.Text = "Сигнал СХОД";
            // 
            // disconnect2_button
            // 
            this.disconnect2_button.Location = new System.Drawing.Point(485, 28);
            this.disconnect2_button.Name = "disconnect2_button";
            this.disconnect2_button.Size = new System.Drawing.Size(76, 37);
            this.disconnect2_button.TabIndex = 25;
            this.disconnect2_button.Text = "Disconnect";
            this.disconnect2_button.UseVisualStyleBackColor = true;
            this.disconnect2_button.Click += new System.EventHandler(this.disconnect2_button_Click);
            // 
            // connect2_button
            // 
            this.connect2_button.Location = new System.Drawing.Point(6, 28);
            this.connect2_button.Name = "connect2_button";
            this.connect2_button.Size = new System.Drawing.Size(66, 37);
            this.connect2_button.TabIndex = 14;
            this.connect2_button.Text = "Connect";
            this.connect2_button.UseVisualStyleBackColor = true;
            this.connect2_button.Click += new System.EventHandler(this.connect2_button_Click);
            // 
            // port2_Combobox
            // 
            this.port2_Combobox.DisplayMember = "1";
            this.port2_Combobox.FormattingEnabled = true;
            this.port2_Combobox.Location = new System.Drawing.Point(78, 41);
            this.port2_Combobox.Name = "port2_Combobox";
            this.port2_Combobox.Size = new System.Drawing.Size(73, 21);
            this.port2_Combobox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Port";
            // 
            // baudrate2_Combobox
            // 
            this.baudrate2_Combobox.FormattingEnabled = true;
            this.baudrate2_Combobox.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.baudrate2_Combobox.Location = new System.Drawing.Point(160, 41);
            this.baudrate2_Combobox.Name = "baudrate2_Combobox";
            this.baudrate2_Combobox.Size = new System.Drawing.Size(73, 21);
            this.baudrate2_Combobox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Rate";
            // 
            // parity2_Combobox
            // 
            this.parity2_Combobox.FormattingEnabled = true;
            this.parity2_Combobox.Location = new System.Drawing.Point(244, 41);
            this.parity2_Combobox.Name = "parity2_Combobox";
            this.parity2_Combobox.Size = new System.Drawing.Size(73, 21);
            this.parity2_Combobox.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Parity";
            // 
            // stopbits2_Combobox
            // 
            this.stopbits2_Combobox.FormattingEnabled = true;
            this.stopbits2_Combobox.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.stopbits2_Combobox.Location = new System.Drawing.Point(325, 41);
            this.stopbits2_Combobox.Name = "stopbits2_Combobox";
            this.stopbits2_Combobox.Size = new System.Drawing.Size(73, 21);
            this.stopbits2_Combobox.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(322, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Stop bits";
            // 
            // databits2_Combobox
            // 
            this.databits2_Combobox.FormattingEnabled = true;
            this.databits2_Combobox.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.databits2_Combobox.Location = new System.Drawing.Point(406, 41);
            this.databits2_Combobox.Name = "databits2_Combobox";
            this.databits2_Combobox.Size = new System.Drawing.Size(73, 21);
            this.databits2_Combobox.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(403, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Data bits";
            // 
            // control_tabPage
            // 
            this.control_tabPage.BackColor = System.Drawing.Color.AliceBlue;
            this.control_tabPage.Controls.Add(this.control_groupBox);
            this.control_tabPage.Controls.Add(this.graph_groupBox);
            this.control_tabPage.Location = new System.Drawing.Point(4, 22);
            this.control_tabPage.Name = "control_tabPage";
            this.control_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.control_tabPage.Size = new System.Drawing.Size(752, 88);
            this.control_tabPage.TabIndex = 2;
            this.control_tabPage.Text = "Управление";
            // 
            // measure_tabPage
            // 
            this.measure_tabPage.Controls.Add(this.cbAdmission);
            this.measure_tabPage.Controls.Add(this.gbScale);
            this.measure_tabPage.Controls.Add(this.gbLimitation2);
            this.measure_tabPage.Controls.Add(this.gbAmplitude);
            this.measure_tabPage.Controls.Add(this.labXInfo);
            this.measure_tabPage.Controls.Add(this.labYInfo);
            this.measure_tabPage.Controls.Add(this.tbYCounted);
            this.measure_tabPage.Controls.Add(this.tbTimeCounted);
            this.measure_tabPage.Controls.Add(this.bntResetTimePoints);
            this.measure_tabPage.Controls.Add(this.rbtnMeasureGraph3);
            this.measure_tabPage.Controls.Add(this.rbtnMeasureGraph2);
            this.measure_tabPage.Controls.Add(this.rbtnMeasureGraph1);
            this.measure_tabPage.Controls.Add(this.checkBoxMeasuring);
            this.measure_tabPage.Location = new System.Drawing.Point(4, 22);
            this.measure_tabPage.Name = "measure_tabPage";
            this.measure_tabPage.Size = new System.Drawing.Size(752, 88);
            this.measure_tabPage.TabIndex = 3;
            this.measure_tabPage.Text = "Измерения";
            this.measure_tabPage.UseVisualStyleBackColor = true;
            // 
            // cbAdmission
            // 
            this.cbAdmission.AutoSize = true;
            this.cbAdmission.Location = new System.Drawing.Point(300, 18);
            this.cbAdmission.Name = "cbAdmission";
            this.cbAdmission.Size = new System.Drawing.Size(90, 17);
            this.cbAdmission.TabIndex = 52;
            this.cbAdmission.Text = "Допуск ДРВ";
            this.cbAdmission.UseVisualStyleBackColor = true;
            // 
            // gbScale
            // 
            this.gbScale.Controls.Add(this.btnScale9);
            this.gbScale.Controls.Add(this.btnScale17);
            this.gbScale.Controls.Add(this.btnScale35);
            this.gbScale.Location = new System.Drawing.Point(604, 7);
            this.gbScale.Name = "gbScale";
            this.gbScale.Size = new System.Drawing.Size(129, 78);
            this.gbScale.TabIndex = 51;
            this.gbScale.TabStop = false;
            this.gbScale.Text = "Масштаб";
            // 
            // btnScale9
            // 
            this.btnScale9.Location = new System.Drawing.Point(6, 19);
            this.btnScale9.Name = "btnScale9";
            this.btnScale9.Size = new System.Drawing.Size(57, 23);
            this.btnScale9.TabIndex = 12;
            this.btnScale9.Text = "ΔK = 9";
            this.btnScale9.UseVisualStyleBackColor = true;
            this.btnScale9.Click += new System.EventHandler(this.btnScale9_Click);
            // 
            // btnScale17
            // 
            this.btnScale17.Location = new System.Drawing.Point(66, 19);
            this.btnScale17.Name = "btnScale17";
            this.btnScale17.Size = new System.Drawing.Size(57, 23);
            this.btnScale17.TabIndex = 10;
            this.btnScale17.Text = "ΔK = 17";
            this.btnScale17.UseVisualStyleBackColor = true;
            this.btnScale17.Click += new System.EventHandler(this.btnScale17_Click);
            // 
            // btnScale35
            // 
            this.btnScale35.Location = new System.Drawing.Point(66, 46);
            this.btnScale35.Name = "btnScale35";
            this.btnScale35.Size = new System.Drawing.Size(57, 23);
            this.btnScale35.TabIndex = 11;
            this.btnScale35.Text = "ΔK = 35";
            this.btnScale35.UseVisualStyleBackColor = true;
            this.btnScale35.Click += new System.EventHandler(this.btnScale35_Click);
            // 
            // gbLimitation2
            // 
            this.gbLimitation2.Controls.Add(this.btnChangeFrame);
            this.gbLimitation2.Controls.Add(this.btnLimitation2);
            this.gbLimitation2.Location = new System.Drawing.Point(418, 7);
            this.gbLimitation2.Name = "gbLimitation2";
            this.gbLimitation2.Size = new System.Drawing.Size(87, 78);
            this.gbLimitation2.TabIndex = 13;
            this.gbLimitation2.TabStop = false;
            this.gbLimitation2.Text = "Limitation 2";
            this.gbLimitation2.Visible = false;
            // 
            // btnChangeFrame
            // 
            this.btnChangeFrame.Location = new System.Drawing.Point(6, 46);
            this.btnChangeFrame.Name = "btnChangeFrame";
            this.btnChangeFrame.Size = new System.Drawing.Size(75, 23);
            this.btnChangeFrame.TabIndex = 13;
            this.btnChangeFrame.Text = "Disable";
            this.btnChangeFrame.UseVisualStyleBackColor = true;
            this.btnChangeFrame.Click += new System.EventHandler(this.btnChangeFrame_Click);
            // 
            // btnLimitation2
            // 
            this.btnLimitation2.Location = new System.Drawing.Point(6, 19);
            this.btnLimitation2.Name = "btnLimitation2";
            this.btnLimitation2.Size = new System.Drawing.Size(75, 23);
            this.btnLimitation2.TabIndex = 12;
            this.btnLimitation2.Text = "Disable";
            this.btnLimitation2.UseVisualStyleBackColor = true;
            this.btnLimitation2.Click += new System.EventHandler(this.btnLimitation2_Click);
            // 
            // gbAmplitude
            // 
            this.gbAmplitude.Controls.Add(this.btnShowAmpl);
            this.gbAmplitude.Location = new System.Drawing.Point(511, 7);
            this.gbAmplitude.Name = "gbAmplitude";
            this.gbAmplitude.Size = new System.Drawing.Size(87, 78);
            this.gbAmplitude.TabIndex = 9;
            this.gbAmplitude.TabStop = false;
            this.gbAmplitude.Text = "Мощность";
            // 
            // btnShowAmpl
            // 
            this.btnShowAmpl.Location = new System.Drawing.Point(6, 30);
            this.btnShowAmpl.Name = "btnShowAmpl";
            this.btnShowAmpl.Size = new System.Drawing.Size(75, 23);
            this.btnShowAmpl.TabIndex = 10;
            this.btnShowAmpl.Text = "Показать";
            this.btnShowAmpl.UseVisualStyleBackColor = true;
            this.btnShowAmpl.Click += new System.EventHandler(this.btnShowAmpl_Click);
            // 
            // labXInfo
            // 
            this.labXInfo.AutoSize = true;
            this.labXInfo.Location = new System.Drawing.Point(11, 17);
            this.labXInfo.Name = "labXInfo";
            this.labXInfo.Size = new System.Drawing.Size(38, 13);
            this.labXInfo.TabIndex = 8;
            this.labXInfo.Text = "ΔK, гр";
            // 
            // labYInfo
            // 
            this.labYInfo.AutoSize = true;
            this.labYInfo.Location = new System.Drawing.Point(11, 40);
            this.labYInfo.Name = "labYInfo";
            this.labYInfo.Size = new System.Drawing.Size(33, 13);
            this.labYInfo.TabIndex = 7;
            this.labYInfo.Text = "ΔТ, с";
            // 
            // tbYCounted
            // 
            this.tbYCounted.Enabled = false;
            this.tbYCounted.Location = new System.Drawing.Point(55, 10);
            this.tbYCounted.Name = "tbYCounted";
            this.tbYCounted.Size = new System.Drawing.Size(100, 20);
            this.tbYCounted.TabIndex = 6;
            // 
            // tbTimeCounted
            // 
            this.tbTimeCounted.Enabled = false;
            this.tbTimeCounted.Location = new System.Drawing.Point(55, 36);
            this.tbTimeCounted.Name = "tbTimeCounted";
            this.tbTimeCounted.Size = new System.Drawing.Size(100, 20);
            this.tbTimeCounted.TabIndex = 5;
            // 
            // bntResetTimePoints
            // 
            this.bntResetTimePoints.Location = new System.Drawing.Point(55, 60);
            this.bntResetTimePoints.Name = "bntResetTimePoints";
            this.bntResetTimePoints.Size = new System.Drawing.Size(65, 20);
            this.bntResetTimePoints.TabIndex = 4;
            this.bntResetTimePoints.Text = "Очистить";
            this.bntResetTimePoints.UseVisualStyleBackColor = true;
            this.bntResetTimePoints.Click += new System.EventHandler(this.bntResetTimePoints_Click);
            // 
            // rbtnMeasureGraph3
            // 
            this.rbtnMeasureGraph3.AutoSize = true;
            this.rbtnMeasureGraph3.Location = new System.Drawing.Point(174, 63);
            this.rbtnMeasureGraph3.Name = "rbtnMeasureGraph3";
            this.rbtnMeasureGraph3.Size = new System.Drawing.Size(110, 17);
            this.rbtnMeasureGraph3.TabIndex = 3;
            this.rbtnMeasureGraph3.Text = "Вертикаль. Крен";
            this.rbtnMeasureGraph3.UseVisualStyleBackColor = true;
            // 
            // rbtnMeasureGraph2
            // 
            this.rbtnMeasureGraph2.AutoSize = true;
            this.rbtnMeasureGraph2.Location = new System.Drawing.Point(174, 40);
            this.rbtnMeasureGraph2.Name = "rbtnMeasureGraph2";
            this.rbtnMeasureGraph2.Size = new System.Drawing.Size(129, 17);
            this.rbtnMeasureGraph2.TabIndex = 2;
            this.rbtnMeasureGraph2.Text = "Горизонт. Задержка";
            this.rbtnMeasureGraph2.UseVisualStyleBackColor = true;
            // 
            // rbtnMeasureGraph1
            // 
            this.rbtnMeasureGraph1.AutoSize = true;
            this.rbtnMeasureGraph1.Checked = true;
            this.rbtnMeasureGraph1.Location = new System.Drawing.Point(174, 17);
            this.rbtnMeasureGraph1.Name = "rbtnMeasureGraph1";
            this.rbtnMeasureGraph1.Size = new System.Drawing.Size(101, 17);
            this.rbtnMeasureGraph1.TabIndex = 1;
            this.rbtnMeasureGraph1.TabStop = true;
            this.rbtnMeasureGraph1.Text = "Освещённость";
            this.rbtnMeasureGraph1.UseVisualStyleBackColor = true;
            // 
            // checkBoxMeasuring
            // 
            this.checkBoxMeasuring.AutoSize = true;
            this.checkBoxMeasuring.Checked = true;
            this.checkBoxMeasuring.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMeasuring.Location = new System.Drawing.Point(55, 62);
            this.checkBoxMeasuring.Name = "checkBoxMeasuring";
            this.checkBoxMeasuring.Size = new System.Drawing.Size(120, 17);
            this.checkBoxMeasuring.TabIndex = 0;
            this.checkBoxMeasuring.Text = "Режим измерения";
            this.checkBoxMeasuring.UseVisualStyleBackColor = true;
            this.checkBoxMeasuring.Visible = false;
            this.checkBoxMeasuring.CheckedChanged += new System.EventHandler(this.checkBoxMeasuring_CheckedChanged);
            // 
            // comstatus2_label
            // 
            this.comstatus2_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comstatus2_label.AutoSize = true;
            this.comstatus2_label.Location = new System.Drawing.Point(156, 693);
            this.comstatus2_label.Name = "comstatus2_label";
            this.comstatus2_label.Size = new System.Drawing.Size(140, 13);
            this.comstatus2_label.TabIndex = 48;
            this.comstatus2_label.Text = "COM2 status: Disconnected";
            // 
            // comstatus_label
            // 
            this.comstatus_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comstatus_label.AutoSize = true;
            this.comstatus_label.Location = new System.Drawing.Point(16, 693);
            this.comstatus_label.Name = "comstatus_label";
            this.comstatus_label.Size = new System.Drawing.Size(134, 13);
            this.comstatus_label.TabIndex = 47;
            this.comstatus_label.Text = "COM status: Disconnected";
            // 
            // syncstatus_label
            // 
            this.syncstatus_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.syncstatus_label.AutoSize = true;
            this.syncstatus_label.Location = new System.Drawing.Point(641, 693);
            this.syncstatus_label.Name = "syncstatus_label";
            this.syncstatus_label.Size = new System.Drawing.Size(51, 13);
            this.syncstatus_label.TabIndex = 49;
            this.syncstatus_label.Text = "Sync: No";
            // 
            // timeGraphName_label
            // 
            this.timeGraphName_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.timeGraphName_label.AutoSize = true;
            this.timeGraphName_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timeGraphName_label.Location = new System.Drawing.Point(407, 610);
            this.timeGraphName_label.Name = "timeGraphName_label";
            this.timeGraphName_label.Size = new System.Drawing.Size(60, 13);
            this.timeGraphName_label.TabIndex = 50;
            this.timeGraphName_label.Text = "Время, с";
            // 
            // WorkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 712);
            this.Controls.Add(this.timeGraphName_label);
            this.Controls.Add(this.syncstatus_label);
            this.Controls.Add(this.comstatus2_label);
            this.Controls.Add(this.comstatus_label);
            this.Controls.Add(this.com_tabControl);
            this.Controls.Add(this.clean_error_linkLabel);
            this.Controls.Add(this.error_counter_label);
            this.Controls.Add(this.packages_counter_label);
            this.Controls.Add(this.dataResiveProgressBar);
            this.Controls.Add(this.dataResiveCounterTest_label);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.error_TextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "WorkForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BVK";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WorkForm_KeyDown);
            this.graph_groupBox.ResumeLayout(false);
            this.control_groupBox.ResumeLayout(false);
            this.com_tabControl.ResumeLayout(false);
            this.com1_tabPage.ResumeLayout(false);
            this.com1_tabPage.PerformLayout();
            this.com2_tabPage.ResumeLayout(false);
            this.com2_tabPage.PerformLayout();
            this.control_tabPage.ResumeLayout(false);
            this.measure_tabPage.ResumeLayout(false);
            this.measure_tabPage.PerformLayout();
            this.gbScale.ResumeLayout(false);
            this.gbLimitation2.ResumeLayout(false);
            this.gbAmplitude.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_Button;
        private System.Windows.Forms.ComboBox port_Combobox;
        private System.Windows.Forms.Label port_Label;
        private System.Windows.Forms.ComboBox baudrate_Combobox;
        private System.Windows.Forms.Label baudrate_label;
        private System.Windows.Forms.ComboBox parity_Combobox;
        private System.Windows.Forms.Label parity_Label;
        private System.Windows.Forms.ComboBox stopbits_Combobox;
        private System.Windows.Forms.Label stopbits_Label;
        private System.Windows.Forms.ComboBox databits_Combobox;
        private System.Windows.Forms.Label databits_Label;
        private System.Windows.Forms.Button disconnect_Button;
        private System.Windows.Forms.Button startbvk_button;
        private System.Windows.Forms.Button stopbvk_button;
        private System.Windows.Forms.Button starttest_button;
        private System.Windows.Forms.Button limitation_button;
        private System.Windows.Forms.Button derivation_button;
        private System.Windows.Forms.TextBox error_TextBox;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.Button cleangraph_button;
        private System.Windows.Forms.Label dataResiveCounterTest_label;
        private System.Windows.Forms.Timer timerCount;
        private System.Windows.Forms.ProgressBar dataResiveProgressBar;
        private System.Windows.Forms.Button normalmode_button;
        private System.Windows.Forms.Button engineeringmode_button;
        private System.Windows.Forms.Button kdo_button;
        private System.Windows.Forms.Button yac1_button;
        private System.Windows.Forms.Button yac2_button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button load_button;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label packages_counter_label;
        private System.Windows.Forms.Label error_counter_label;
        private System.Windows.Forms.LinkLabel clean_error_linkLabel;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.GroupBox graph_groupBox;
        private System.Windows.Forms.GroupBox control_groupBox;
        private System.Windows.Forms.TabControl com_tabControl;
        private System.Windows.Forms.TabPage com1_tabPage;
        private System.Windows.Forms.TabPage com2_tabPage;
        private System.Windows.Forms.Label comstatus2_label;
        private System.Windows.Forms.Label comstatus_label;
        private System.Windows.Forms.Button disconnect2_button;
        private System.Windows.Forms.Button connect2_button;
        private System.Windows.Forms.ComboBox port2_Combobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox baudrate2_Combobox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox parity2_Combobox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox stopbits2_Combobox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox databits2_Combobox;
        private System.Windows.Forms.Label label5;
        private System.IO.Ports.SerialPort serialPort2;
        private System.Windows.Forms.Label syncstatus_label;
        private System.Windows.Forms.CheckBox Sync_checkBox;
        private System.Windows.Forms.TabPage control_tabPage;
        private System.Windows.Forms.Label timeGraphName_label;
        private System.Windows.Forms.TabPage measure_tabPage;
        private System.Windows.Forms.RadioButton rbtnMeasureGraph3;
        private System.Windows.Forms.RadioButton rbtnMeasureGraph2;
        private System.Windows.Forms.RadioButton rbtnMeasureGraph1;
        private System.Windows.Forms.CheckBox checkBoxMeasuring;
        private System.Windows.Forms.Button bntResetTimePoints;
        private System.Windows.Forms.TextBox tbTimeCounted;
        private System.Windows.Forms.Label labXInfo;
        private System.Windows.Forms.Label labYInfo;
        private System.Windows.Forms.TextBox tbYCounted;
        private System.Windows.Forms.GroupBox gbAmplitude;
        private System.Windows.Forms.Button btnShowAmpl;
        private System.Windows.Forms.Button btnResetGraph;
        private System.Windows.Forms.Button btnScale35;
        private System.Windows.Forms.Button btnScale17;
        private System.Windows.Forms.Button btnLimitation2;
        private System.Windows.Forms.GroupBox gbScale;
        private System.Windows.Forms.GroupBox gbLimitation2;
        private System.Windows.Forms.Button btnChangeFrame;
        private System.Windows.Forms.Label lbBVK;
        private System.Windows.Forms.Label lbInfoSync;
        private System.Windows.Forms.Button btnScale9;
        private System.Windows.Forms.Button btnServiceMode;
        private System.Windows.Forms.CheckBox cbAdmission;
    }
}

