namespace DOTNET
{
    partial class LogoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerLogo = new System.Windows.Forms.Timer(this.components);
            this.timerOpacity = new System.Windows.Forms.Timer(this.components);
            this.pbLoad = new System.Windows.Forms.PictureBox();
            this.labVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // timerLogo
            // 
            this.timerLogo.Enabled = true;
            this.timerLogo.Interval = 1000;
            this.timerLogo.Tick += new System.EventHandler(this.timerLogo_Tick);
            // 
            // timerOpacity
            // 
            this.timerOpacity.Enabled = true;
            this.timerOpacity.Interval = 20;
            this.timerOpacity.Tick += new System.EventHandler(this.timerOpacity_Tick);
            // 
            // pbLoad
            // 
            this.pbLoad.BackColor = System.Drawing.SystemColors.Window;
            this.pbLoad.Image = global::BVKGraph.Properties.Resources.load;
            this.pbLoad.InitialImage = global::BVKGraph.Properties.Resources.load;
            this.pbLoad.Location = new System.Drawing.Point(131, 104);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(100, 50);
            this.pbLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLoad.TabIndex = 0;
            this.pbLoad.TabStop = false;
            this.pbLoad.UseWaitCursor = true;
            // 
            // labVersion
            // 
            this.labVersion.AutoSize = true;
            this.labVersion.BackColor = System.Drawing.SystemColors.Window;
            this.labVersion.Location = new System.Drawing.Point(237, 127);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(28, 13);
            this.labVersion.TabIndex = 1;
            this.labVersion.Text = "ver :";
            this.labVersion.UseWaitCursor = true;
            // 
            // LogoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::BVKGraph.Properties.Resources.Logo5Peleng;
            this.ClientSize = new System.Drawing.Size(352, 149);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.pbLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LogoForm";
            this.Opacity = 0.2D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogoForm";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.LogoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerLogo;
        private System.Windows.Forms.Timer timerOpacity;
        private System.Windows.Forms.PictureBox pbLoad;
        private System.Windows.Forms.Label labVersion;
    }
}