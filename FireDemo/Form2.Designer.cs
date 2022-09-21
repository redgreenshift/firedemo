
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
            this.buttonInMeeting = new System.Windows.Forms.Button();
            this.buttonFrustratedStatus = new System.Windows.Forms.Button();
            this.buttonLunchStatus = new System.Windows.Forms.Button();
            this.buttonSauron = new System.Windows.Forms.Button();
            this.buttonDemoFHL = new System.Windows.Forms.Button();
            this.buttonHistoryOfFire = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.buttonSauronV2 = new System.Windows.Forms.Button();
            this.buttonSauronV1 = new System.Windows.Forms.Button();
            this.buttonFastRender = new System.Windows.Forms.Button();
            this.buttonAdvanced = new System.Windows.Forms.Button();
            this.buttonBatmanSingleThread = new System.Windows.Forms.Button();
            this.buttonRainBORG = new System.Windows.Forms.Button();
            this.buttonRainbowFire = new System.Windows.Forms.Button();
            this.groupBoxExperiment = new System.Windows.Forms.GroupBox();
            this.buttonFluxCapacitor = new System.Windows.Forms.Button();
            this.buttonRainbowFireDark = new System.Windows.Forms.Button();
            this.buttonSauronV3 = new System.Windows.Forms.Button();
            this.buttonRainbowBatman = new System.Windows.Forms.Button();
            this.buttonBlueTBD = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxExperiment.SuspendLayout();
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
            this.buttonAwayStatus.Location = new System.Drawing.Point(6, 117);
            this.buttonAwayStatus.Name = "buttonAwayStatus";
            this.buttonAwayStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonAwayStatus.TabIndex = 3;
            this.buttonAwayStatus.Text = "Away";
            this.buttonAwayStatus.UseVisualStyleBackColor = false;
            this.buttonAwayStatus.Click += new System.EventHandler(this.buttonAwayStatus_Click);
            // 
            // buttonOofStatus
            // 
            this.buttonOofStatus.BackColor = System.Drawing.Color.Black;
            this.buttonOofStatus.ForeColor = System.Drawing.Color.MediumOrchid;
            this.buttonOofStatus.Location = new System.Drawing.Point(182, 213);
            this.buttonOofStatus.Name = "buttonOofStatus";
            this.buttonOofStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonOofStatus.TabIndex = 7;
            this.buttonOofStatus.Text = "Stuck";
            this.buttonOofStatus.UseVisualStyleBackColor = false;
            this.buttonOofStatus.Click += new System.EventHandler(this.buttonOofStatus_Click);
            // 
            // buttonBusyStatus
            // 
            this.buttonBusyStatus.BackColor = System.Drawing.Color.Red;
            this.buttonBusyStatus.Location = new System.Drawing.Point(6, 213);
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
            this.buttonAvailableStatus.Location = new System.Drawing.Point(6, 21);
            this.buttonAvailableStatus.Name = "buttonAvailableStatus";
            this.buttonAvailableStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonAvailableStatus.TabIndex = 2;
            this.buttonAvailableStatus.Text = "Available";
            this.buttonAvailableStatus.UseVisualStyleBackColor = false;
            this.buttonAvailableStatus.Click += new System.EventHandler(this.buttonAvailableStatus_Click);
            // 
            // buttonDndStatus
            // 
            this.buttonDndStatus.BackColor = System.Drawing.Color.Crimson;
            this.buttonDndStatus.Location = new System.Drawing.Point(6, 405);
            this.buttonDndStatus.Name = "buttonDndStatus";
            this.buttonDndStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonDndStatus.TabIndex = 5;
            this.buttonDndStatus.Text = "Do Not Disturb";
            this.buttonDndStatus.UseVisualStyleBackColor = false;
            this.buttonDndStatus.Click += new System.EventHandler(this.buttonDndStatus_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonBlueTBD);
            this.groupBox1.Controls.Add(this.buttonInMeeting);
            this.groupBox1.Controls.Add(this.buttonFrustratedStatus);
            this.groupBox1.Controls.Add(this.buttonLunchStatus);
            this.groupBox1.Controls.Add(this.buttonSauron);
            this.groupBox1.Controls.Add(this.buttonAwayStatus);
            this.groupBox1.Controls.Add(this.buttonDndStatus);
            this.groupBox1.Controls.Add(this.buttonOofStatus);
            this.groupBox1.Controls.Add(this.buttonAvailableStatus);
            this.groupBox1.Controls.Add(this.buttonBusyStatus);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(26, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(538, 601);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pick your status";
            // 
            // buttonInMeeting
            // 
            this.buttonInMeeting.BackColor = System.Drawing.Color.OrangeRed;
            this.buttonInMeeting.Location = new System.Drawing.Point(6, 309);
            this.buttonInMeeting.Name = "buttonInMeeting";
            this.buttonInMeeting.Size = new System.Drawing.Size(172, 90);
            this.buttonInMeeting.TabIndex = 13;
            this.buttonInMeeting.Text = "In a Meeting";
            this.buttonInMeeting.UseVisualStyleBackColor = false;
            this.buttonInMeeting.Click += new System.EventHandler(this.buttonInMeeting_Click);
            // 
            // buttonFrustratedStatus
            // 
            this.buttonFrustratedStatus.BackColor = System.Drawing.Color.DarkGreen;
            this.buttonFrustratedStatus.ForeColor = System.Drawing.Color.Yellow;
            this.buttonFrustratedStatus.Location = new System.Drawing.Point(182, 405);
            this.buttonFrustratedStatus.Name = "buttonFrustratedStatus";
            this.buttonFrustratedStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonFrustratedStatus.TabIndex = 12;
            this.buttonFrustratedStatus.Text = "Frustrated / FireDrill";
            this.buttonFrustratedStatus.UseVisualStyleBackColor = false;
            this.buttonFrustratedStatus.Click += new System.EventHandler(this.buttonFrustratedStatus_Click);
            // 
            // buttonLunchStatus
            // 
            this.buttonLunchStatus.BackColor = System.Drawing.Color.Indigo;
            this.buttonLunchStatus.ForeColor = System.Drawing.Color.Lime;
            this.buttonLunchStatus.Location = new System.Drawing.Point(182, 117);
            this.buttonLunchStatus.Name = "buttonLunchStatus";
            this.buttonLunchStatus.Size = new System.Drawing.Size(172, 90);
            this.buttonLunchStatus.TabIndex = 11;
            this.buttonLunchStatus.Text = "Lunch";
            this.buttonLunchStatus.UseVisualStyleBackColor = false;
            this.buttonLunchStatus.Click += new System.EventHandler(this.buttonLunchStatus_Click);
            // 
            // buttonSauron
            // 
            this.buttonSauron.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.buttonSauron.Location = new System.Drawing.Point(182, 21);
            this.buttonSauron.Name = "buttonSauron";
            this.buttonSauron.Size = new System.Drawing.Size(172, 90);
            this.buttonSauron.TabIndex = 6;
            this.buttonSauron.Text = "Talkative";
            this.buttonSauron.UseVisualStyleBackColor = false;
            this.buttonSauron.Click += new System.EventHandler(this.buttonSauron_Click);
            // 
            // buttonDemoFHL
            // 
            this.buttonDemoFHL.Location = new System.Drawing.Point(360, 117);
            this.buttonDemoFHL.Name = "buttonDemoFHL";
            this.buttonDemoFHL.Size = new System.Drawing.Size(172, 90);
            this.buttonDemoFHL.TabIndex = 8;
            this.buttonDemoFHL.Text = "FHL Demo";
            this.buttonDemoFHL.UseVisualStyleBackColor = true;
            this.buttonDemoFHL.Click += new System.EventHandler(this.buttonDemoFHL_Click);
            // 
            // buttonHistoryOfFire
            // 
            this.buttonHistoryOfFire.Location = new System.Drawing.Point(6, 501);
            this.buttonHistoryOfFire.Name = "buttonHistoryOfFire";
            this.buttonHistoryOfFire.Size = new System.Drawing.Size(172, 90);
            this.buttonHistoryOfFire.TabIndex = 9;
            this.buttonHistoryOfFire.Text = "About / History";
            this.buttonHistoryOfFire.UseVisualStyleBackColor = true;
            this.buttonHistoryOfFire.Click += new System.EventHandler(this.buttonHistoryOfFire_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.Location = new System.Drawing.Point(360, 501);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(172, 90);
            this.buttonQuit.TabIndex = 10;
            this.buttonQuit.Text = "EXIT";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // buttonSauronV2
            // 
            this.buttonSauronV2.Location = new System.Drawing.Point(183, 213);
            this.buttonSauronV2.Name = "buttonSauronV2";
            this.buttonSauronV2.Size = new System.Drawing.Size(172, 90);
            this.buttonSauronV2.TabIndex = 27;
            this.buttonSauronV2.Text = "Sauron v3.5";
            this.buttonSauronV2.UseVisualStyleBackColor = true;
            this.buttonSauronV2.Click += new System.EventHandler(this.buttonSauronV2_Click);
            // 
            // buttonSauronV1
            // 
            this.buttonSauronV1.Location = new System.Drawing.Point(183, 117);
            this.buttonSauronV1.Name = "buttonSauronV1";
            this.buttonSauronV1.Size = new System.Drawing.Size(172, 90);
            this.buttonSauronV1.TabIndex = 26;
            this.buttonSauronV1.Text = "Sauron v2.1";
            this.buttonSauronV1.UseVisualStyleBackColor = true;
            this.buttonSauronV1.Click += new System.EventHandler(this.buttonSauronV1_Click);
            // 
            // buttonFastRender
            // 
            this.buttonFastRender.Location = new System.Drawing.Point(183, 21);
            this.buttonFastRender.Name = "buttonFastRender";
            this.buttonFastRender.Size = new System.Drawing.Size(172, 90);
            this.buttonFastRender.TabIndex = 25;
            this.buttonFastRender.Text = "Fast Render Experiment";
            this.buttonFastRender.UseVisualStyleBackColor = true;
            this.buttonFastRender.Click += new System.EventHandler(this.buttonFastRender_Click);
            // 
            // buttonAdvanced
            // 
            this.buttonAdvanced.Location = new System.Drawing.Point(6, 21);
            this.buttonAdvanced.Name = "buttonAdvanced";
            this.buttonAdvanced.Size = new System.Drawing.Size(172, 90);
            this.buttonAdvanced.TabIndex = 20;
            this.buttonAdvanced.Text = "Original Experiment";
            this.buttonAdvanced.UseVisualStyleBackColor = true;
            this.buttonAdvanced.Click += new System.EventHandler(this.buttonAdvanced_Click);
            // 
            // buttonBatmanSingleThread
            // 
            this.buttonBatmanSingleThread.Location = new System.Drawing.Point(360, 21);
            this.buttonBatmanSingleThread.Name = "buttonBatmanSingleThread";
            this.buttonBatmanSingleThread.Size = new System.Drawing.Size(172, 90);
            this.buttonBatmanSingleThread.TabIndex = 30;
            this.buttonBatmanSingleThread.Text = "Batman Single Thread Demo";
            this.buttonBatmanSingleThread.UseVisualStyleBackColor = true;
            this.buttonBatmanSingleThread.Click += new System.EventHandler(this.buttonBatmanSingleThread_Click);
            // 
            // buttonRainBORG
            // 
            this.buttonRainBORG.Location = new System.Drawing.Point(6, 117);
            this.buttonRainBORG.Name = "buttonRainBORG";
            this.buttonRainBORG.Size = new System.Drawing.Size(172, 90);
            this.buttonRainBORG.TabIndex = 21;
            this.buttonRainBORG.Text = "RainBORG";
            this.buttonRainBORG.UseVisualStyleBackColor = true;
            this.buttonRainBORG.Click += new System.EventHandler(this.buttonRainBORG_Click);
            // 
            // buttonRainbowFire
            // 
            this.buttonRainbowFire.Location = new System.Drawing.Point(6, 213);
            this.buttonRainbowFire.Name = "buttonRainbowFire";
            this.buttonRainbowFire.Size = new System.Drawing.Size(172, 90);
            this.buttonRainbowFire.TabIndex = 22;
            this.buttonRainbowFire.Text = "Rainbow Fire";
            this.buttonRainbowFire.UseVisualStyleBackColor = true;
            this.buttonRainbowFire.Click += new System.EventHandler(this.buttonRainbowFire_Click);
            // 
            // groupBoxExperiment
            // 
            this.groupBoxExperiment.Controls.Add(this.buttonFluxCapacitor);
            this.groupBoxExperiment.Controls.Add(this.buttonRainbowFireDark);
            this.groupBoxExperiment.Controls.Add(this.buttonSauronV3);
            this.groupBoxExperiment.Controls.Add(this.buttonDemoFHL);
            this.groupBoxExperiment.Controls.Add(this.buttonQuit);
            this.groupBoxExperiment.Controls.Add(this.buttonHistoryOfFire);
            this.groupBoxExperiment.Controls.Add(this.buttonRainbowBatman);
            this.groupBoxExperiment.Controls.Add(this.buttonSauronV2);
            this.groupBoxExperiment.Controls.Add(this.buttonRainbowFire);
            this.groupBoxExperiment.Controls.Add(this.buttonFastRender);
            this.groupBoxExperiment.Controls.Add(this.buttonSauronV1);
            this.groupBoxExperiment.Controls.Add(this.buttonAdvanced);
            this.groupBoxExperiment.Controls.Add(this.buttonBatmanSingleThread);
            this.groupBoxExperiment.Controls.Add(this.buttonRainBORG);
            this.groupBoxExperiment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxExperiment.Location = new System.Drawing.Point(582, 31);
            this.groupBoxExperiment.Name = "groupBoxExperiment";
            this.groupBoxExperiment.Size = new System.Drawing.Size(538, 601);
            this.groupBoxExperiment.TabIndex = 8;
            this.groupBoxExperiment.TabStop = false;
            this.groupBoxExperiment.Text = "Demos and Experiments";
            // 
            // buttonFluxCapacitor
            // 
            this.buttonFluxCapacitor.Location = new System.Drawing.Point(183, 405);
            this.buttonFluxCapacitor.Name = "buttonFluxCapacitor";
            this.buttonFluxCapacitor.Size = new System.Drawing.Size(172, 90);
            this.buttonFluxCapacitor.TabIndex = 32;
            this.buttonFluxCapacitor.Text = "Flux";
            this.buttonFluxCapacitor.UseVisualStyleBackColor = true;
            this.buttonFluxCapacitor.Click += new System.EventHandler(this.buttonFluxCapacitor_Click);
            // 
            // buttonRainbowFireDark
            // 
            this.buttonRainbowFireDark.Location = new System.Drawing.Point(6, 405);
            this.buttonRainbowFireDark.Name = "buttonRainbowFireDark";
            this.buttonRainbowFireDark.Size = new System.Drawing.Size(172, 90);
            this.buttonRainbowFireDark.TabIndex = 31;
            this.buttonRainbowFireDark.Text = "Rainbow Fire Dark";
            this.buttonRainbowFireDark.UseVisualStyleBackColor = true;
            this.buttonRainbowFireDark.Click += new System.EventHandler(this.buttonRainbowFireDark_Click);
            // 
            // buttonSauronV3
            // 
            this.buttonSauronV3.Location = new System.Drawing.Point(183, 309);
            this.buttonSauronV3.Name = "buttonSauronV3";
            this.buttonSauronV3.Size = new System.Drawing.Size(172, 90);
            this.buttonSauronV3.TabIndex = 28;
            this.buttonSauronV3.Text = "Sauron v3.6";
            this.buttonSauronV3.UseVisualStyleBackColor = true;
            this.buttonSauronV3.Click += new System.EventHandler(this.buttonSauronV3_Click);
            // 
            // buttonRainbowBatman
            // 
            this.buttonRainbowBatman.Location = new System.Drawing.Point(6, 309);
            this.buttonRainbowBatman.Name = "buttonRainbowBatman";
            this.buttonRainbowBatman.Size = new System.Drawing.Size(172, 90);
            this.buttonRainbowBatman.TabIndex = 23;
            this.buttonRainbowBatman.Text = "RainBAT";
            this.buttonRainbowBatman.UseVisualStyleBackColor = true;
            this.buttonRainbowBatman.Click += new System.EventHandler(this.buttonRainbowBatman_Click);
            // 
            // buttonBlueTBD
            // 
            this.buttonBlueTBD.BackColor = System.Drawing.Color.DodgerBlue;
            this.buttonBlueTBD.Location = new System.Drawing.Point(185, 310);
            this.buttonBlueTBD.Name = "buttonBlueTBD";
            this.buttonBlueTBD.Size = new System.Drawing.Size(169, 89);
            this.buttonBlueTBD.TabIndex = 14;
            this.buttonBlueTBD.Text = "Blue: TBD";
            this.buttonBlueTBD.UseVisualStyleBackColor = false;
            this.buttonBlueTBD.Click += new System.EventHandler(this.buttonBlueTBD_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1262, 703);
            this.Controls.Add(this.groupBoxExperiment);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.buttonDemo);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBoxExperiment.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonBatmanSingleThread;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.Button buttonAdvanced;
        private System.Windows.Forms.Button buttonFastRender;
        private System.Windows.Forms.Button buttonSauronV1;
        private System.Windows.Forms.Button buttonSauronV2;
        private System.Windows.Forms.Button buttonRainbowFire;
        private System.Windows.Forms.GroupBox groupBoxExperiment;
        private System.Windows.Forms.Button buttonRainbowBatman;
        private System.Windows.Forms.Button buttonHistoryOfFire;
        private System.Windows.Forms.Button buttonSauronV3;
        private System.Windows.Forms.Button buttonSauron;
        private System.Windows.Forms.Button buttonDemoFHL;
        private System.Windows.Forms.Button buttonLunchStatus;
        private System.Windows.Forms.Button buttonFrustratedStatus;
        private System.Windows.Forms.Button buttonInMeeting;
        private System.Windows.Forms.Button buttonRainbowFireDark;
        private System.Windows.Forms.Button buttonFluxCapacitor;
        private System.Windows.Forms.Button buttonBlueTBD;
    }
}