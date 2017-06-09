namespace BVKGraph
{
    partial class LoadForm
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
            this.pbLoad = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).BeginInit();
            this.SuspendLayout();
            // 
            // pbLoad
            // 
            this.pbLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLoad.Image = global::BVKGraph.Properties.Resources.load;
            this.pbLoad.Location = new System.Drawing.Point(0, 0);
            this.pbLoad.Name = "pbLoad";
            this.pbLoad.Size = new System.Drawing.Size(100, 100);
            this.pbLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLoad.TabIndex = 0;
            this.pbLoad.TabStop = false;
            this.pbLoad.UseWaitCursor = true;
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.Controls.Add(this.pbLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.UseWaitCursor = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbLoad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLoad;
    }
}