
namespace FireDemo
{
    partial class Form2
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
            this.buttonDemo = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonAwayStatus = new System.Windows.Forms.Button();
            this.buttonOofStatus = new System.Windows.Forms.Button();
            this.buttonBusyStatus = new System.Windows.Forms.Button();
            this.buttonAvailableStatus = new System.Windows.Forms.Button();
            this.buttonDndStatus = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonBatmanMultiThread = new System.Windows.Forms.Button();
            this.buttonRainBORG = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDemo
            // 
            this.buttonDemo.Location = new System.Drawing.Point(151, 2);
            this.buttonDemo.Name = "buttonDemo";
            this.buttonDemo.Size = new System.Drawing.Size(75, 23);
            this.buttonDemo.TabIndex = 0;
            this.buttonDemo.Text = "Stert!";
            this.buttonDemo.UseVisualStyleBackColor = true;
            this.buttonDemo.Click += new System.EventHandler(this.buttonDemo_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(355, 2);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(75, 23);
            this.buttonChange.TabIndex = 1;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // buttonAwayStatus
            // 
            this.buttonAwayStatus.BackColor = System.Drawing.Color.Orange;
            this.buttonAwayStatus.Location = new System.Drawing.Point(6, 21);
            this.buttonAwayStatus.Name = "buttonAwayStatus";
            this.buttonAwayStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonAwayStatus.TabIndex = 2;
            this.buttonAwayStatus.Text = "Away";
            this.buttonAwayStatus.UseVisualStyleBackColor = false;
            this.buttonAwayStatus.Click += new System.EventHandler(this.buttonAwayStatus_Click);
            // 
            // buttonOofStatus
            // 
            this.buttonOofStatus.BackColor = System.Drawing.Color.DarkViolet;
            this.buttonOofStatus.Location = new System.Drawing.Point(7, 117);
            this.buttonOofStatus.Name = "buttonOofStatus";
            this.buttonOofStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonOofStatus.TabIndex = 3;
            this.buttonOofStatus.Text = "OOF";
            this.buttonOofStatus.UseVisualStyleBackColor = false;
            this.buttonOofStatus.Click += new System.EventHandler(this.buttonOofStatus_Click);
            // 
            // buttonBusyStatus
            // 
            this.buttonBusyStatus.BackColor = System.Drawing.Color.Red;
            this.buttonBusyStatus.Location = new System.Drawing.Point(184, 21);
            this.buttonBusyStatus.Name = "buttonBusyStatus";
            this.buttonBusyStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonBusyStatus.TabIndex = 4;
            this.buttonBusyStatus.Text = "Busy";
            this.buttonBusyStatus.UseVisualStyleBackColor = false;
            this.buttonBusyStatus.Click += new System.EventHandler(this.buttonBusyStatus_Click);
            // 
            // buttonAvailableStatus
            // 
            this.buttonAvailableStatus.BackColor = System.Drawing.Color.ForestGreen;
            this.buttonAvailableStatus.Location = new System.Drawing.Point(7, 213);
            this.buttonAvailableStatus.Name = "buttonAvailableStatus";
            this.buttonAvailableStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonAvailableStatus.TabIndex = 5;
            this.buttonAvailableStatus.Text = "Available";
            this.buttonAvailableStatus.UseVisualStyleBackColor = false;
            this.buttonAvailableStatus.Click += new System.EventHandler(this.buttonAvailableStatus_Click);
            // 
            // buttonDndStatus
            // 
            this.buttonDndStatus.BackColor = System.Drawing.Color.Crimson;
            this.buttonDndStatus.Location = new System.Drawing.Point(362, 21);
            this.buttonDndStatus.Name = "buttonDndStatus";
            this.buttonDndStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonDndStatus.TabIndex = 6;
            this.buttonDndStatus.Text = "Do Not Disturb";
            this.buttonDndStatus.UseVisualStyleBackColor = false;
            this.buttonDndStatus.Click += new System.EventHandler(this.buttonDndStatus_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonBatmanMultiThread);
            this.groupBox1.Controls.Add(this.buttonRainBORG);
            this.groupBox1.Controls.Add(this.buttonAwayStatus);
            this.groupBox1.Controls.Add(this.buttonDndStatus);
            this.groupBox1.Controls.Add(this.buttonOofStatus);
            this.groupBox1.Controls.Add(this.buttonAvailableStatus);
            this.groupBox1.Controls.Add(this.buttonBusyStatus);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(595, 504);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pick your status";
            // 
            // buttonBatmanMultiThread
            // 
            this.buttonBatmanMultiThread.Location = new System.Drawing.Point(185, 117);
            this.buttonBatmanMultiThread.Name = "buttonBatmanMultiThread";
            this.buttonBatmanMultiThread.Size = new System.Drawing.Size(137, 41);
            this.buttonBatmanMultiThread.TabIndex = 8;
            this.buttonBatmanMultiThread.Text = "Batman MT";
            this.buttonBatmanMultiThread.UseVisualStyleBackColor = true;
            this.buttonBatmanMultiThread.Click += new System.EventHandler(this.buttonBatmanMultiThread_Click);
            // 
            // buttonRainBORG
            // 
            this.buttonRainBORG.Location = new System.Drawing.Point(185, 164);
            this.buttonRainBORG.Name = "buttonRainBORG";
            this.buttonRainBORG.Size = new System.Drawing.Size(137, 41);
            this.buttonRainBORG.TabIndex = 7;
            this.buttonRainBORG.Text = "RainBORG";
            this.buttonRainBORG.UseVisualStyleBackColor = true;
            this.buttonRainBORG.Click += new System.EventHandler(this.buttonRainBORG_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1262, 703);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.buttonDemo);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDemo;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonAwayStatus;
        private System.Windows.Forms.Button buttonOofStatus;
        private System.Windows.Forms.Button buttonBusyStatus;
        private System.Windows.Forms.Button buttonAvailableStatus;
        private System.Windows.Forms.Button buttonDndStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonRainBORG;
        private System.Windows.Forms.Button buttonBatmanMultiThread;
    }
}