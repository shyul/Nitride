namespace Nitride.EE
{
    partial class SpectrumChannelForm
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
            this.TextBoxCenterFreq = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxFreqSpan = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextBoxStartFreq = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxStopFreq = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxVBW = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TextBoxRBW = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TextBoxRange = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TextBoxReferenceLevel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TextBoxTickStep = new System.Windows.Forms.TextBox();
            this.ComboBoxFftWindow = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ComboBoxDetectorType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TextBoxSweepType = new System.Windows.Forms.TextBox();
            this.CheckBoxEnable = new System.Windows.Forms.CheckBox();
            this.CheckBoxEnableHisto = new System.Windows.Forms.CheckBox();
            this.CheckBoxEnablePersist = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TextBoxTracePoints = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.TextBoxDspGain = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.TextBoxFftGain = new System.Windows.Forms.TextBox();
            this.BtnApplyConfiguration = new System.Windows.Forms.Button();
            this.CheckBoxPause = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // TextBoxCenterFreq
            // 
            this.TextBoxCenterFreq.Location = new System.Drawing.Point(113, 12);
            this.TextBoxCenterFreq.Name = "TextBoxCenterFreq";
            this.TextBoxCenterFreq.Size = new System.Drawing.Size(80, 20);
            this.TextBoxCenterFreq.TabIndex = 208;
            this.TextBoxCenterFreq.Text = "187500000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 209;
            this.label1.Text = "Center Frequency";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 211;
            this.label2.Text = "Frequency Span";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxFreqSpan
            // 
            this.TextBoxFreqSpan.Location = new System.Drawing.Point(113, 38);
            this.TextBoxFreqSpan.Name = "TextBoxFreqSpan";
            this.TextBoxFreqSpan.Size = new System.Drawing.Size(80, 20);
            this.TextBoxFreqSpan.TabIndex = 210;
            this.TextBoxFreqSpan.Text = "100000000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 213;
            this.label3.Text = "Start Frequency";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxStartFreq
            // 
            this.TextBoxStartFreq.Location = new System.Drawing.Point(113, 64);
            this.TextBoxStartFreq.Name = "TextBoxStartFreq";
            this.TextBoxStartFreq.ReadOnly = true;
            this.TextBoxStartFreq.Size = new System.Drawing.Size(80, 20);
            this.TextBoxStartFreq.TabIndex = 212;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 215;
            this.label4.Text = "Stop Frequency";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxStopFreq
            // 
            this.TextBoxStopFreq.Location = new System.Drawing.Point(113, 90);
            this.TextBoxStopFreq.Name = "TextBoxStopFreq";
            this.TextBoxStopFreq.ReadOnly = true;
            this.TextBoxStopFreq.Size = new System.Drawing.Size(80, 20);
            this.TextBoxStopFreq.TabIndex = 214;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(74, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 219;
            this.label5.Text = "VBW";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxVBW
            // 
            this.TextBoxVBW.Location = new System.Drawing.Point(113, 142);
            this.TextBoxVBW.Name = "TextBoxVBW";
            this.TextBoxVBW.ReadOnly = true;
            this.TextBoxVBW.Size = new System.Drawing.Size(80, 20);
            this.TextBoxVBW.TabIndex = 218;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(74, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 217;
            this.label6.Text = "RBW";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxRBW
            // 
            this.TextBoxRBW.Location = new System.Drawing.Point(113, 116);
            this.TextBoxRBW.Name = "TextBoxRBW";
            this.TextBoxRBW.ReadOnly = true;
            this.TextBoxRBW.Size = new System.Drawing.Size(80, 20);
            this.TextBoxRBW.TabIndex = 216;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(258, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 223;
            this.label7.Text = "Range";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxRange
            // 
            this.TextBoxRange.Location = new System.Drawing.Point(303, 38);
            this.TextBoxRange.Name = "TextBoxRange";
            this.TextBoxRange.Size = new System.Drawing.Size(80, 20);
            this.TextBoxRange.TabIndex = 222;
            this.TextBoxRange.Text = "130";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(211, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 221;
            this.label8.Text = "Reference Level";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxReferenceLevel
            // 
            this.TextBoxReferenceLevel.Location = new System.Drawing.Point(303, 12);
            this.TextBoxReferenceLevel.Name = "TextBoxReferenceLevel";
            this.TextBoxReferenceLevel.Size = new System.Drawing.Size(80, 20);
            this.TextBoxReferenceLevel.TabIndex = 220;
            this.TextBoxReferenceLevel.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(242, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 225;
            this.label9.Text = "Y Axis Div";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxTickStep
            // 
            this.TextBoxTickStep.Location = new System.Drawing.Point(303, 64);
            this.TextBoxTickStep.Name = "TextBoxTickStep";
            this.TextBoxTickStep.Size = new System.Drawing.Size(80, 20);
            this.TextBoxTickStep.TabIndex = 224;
            this.TextBoxTickStep.Text = "10";
            // 
            // ComboBoxFftWindow
            // 
            this.ComboBoxFftWindow.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxFftWindow.FormattingEnabled = true;
            this.ComboBoxFftWindow.Items.AddRange(new object[] {
            "Rectangle",
            "Triangle",
            "Hanning",
            "Hamming",
            "Nuttall",
            "Blackman",
            "BlackmanNuttall",
            "BlackmanHarris",
            "FlatTop"});
            this.ComboBoxFftWindow.Location = new System.Drawing.Point(484, 37);
            this.ComboBoxFftWindow.Name = "ComboBoxFftWindow";
            this.ComboBoxFftWindow.Size = new System.Drawing.Size(93, 21);
            this.ComboBoxFftWindow.TabIndex = 226;
            this.ComboBoxFftWindow.Text = "Flat Top";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(405, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 227;
            this.label10.Text = "Window Type";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(403, 67);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 229;
            this.label11.Text = "Detector Type";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxDetectorType
            // 
            this.ComboBoxDetectorType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxDetectorType.FormattingEnabled = true;
            this.ComboBoxDetectorType.Location = new System.Drawing.Point(484, 63);
            this.ComboBoxDetectorType.Name = "ComboBoxDetectorType";
            this.ComboBoxDetectorType.Size = new System.Drawing.Size(93, 21);
            this.ComboBoxDetectorType.TabIndex = 228;
            this.ComboBoxDetectorType.Text = "Peak";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(408, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 231;
            this.label12.Text = "Sweep Mode";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxSweepType
            // 
            this.TextBoxSweepType.Location = new System.Drawing.Point(484, 12);
            this.TextBoxSweepType.Name = "TextBoxSweepType";
            this.TextBoxSweepType.ReadOnly = true;
            this.TextBoxSweepType.Size = new System.Drawing.Size(93, 20);
            this.TextBoxSweepType.TabIndex = 230;
            // 
            // CheckBoxEnable
            // 
            this.CheckBoxEnable.AutoSize = true;
            this.CheckBoxEnable.Checked = true;
            this.CheckBoxEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEnable.Location = new System.Drawing.Point(261, 115);
            this.CheckBoxEnable.Name = "CheckBoxEnable";
            this.CheckBoxEnable.Size = new System.Drawing.Size(59, 17);
            this.CheckBoxEnable.TabIndex = 232;
            this.CheckBoxEnable.Text = "Enable";
            this.CheckBoxEnable.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnableHisto
            // 
            this.CheckBoxEnableHisto.AutoSize = true;
            this.CheckBoxEnableHisto.Checked = true;
            this.CheckBoxEnableHisto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEnableHisto.Location = new System.Drawing.Point(261, 141);
            this.CheckBoxEnableHisto.Name = "CheckBoxEnableHisto";
            this.CheckBoxEnableHisto.Size = new System.Drawing.Size(109, 17);
            this.CheckBoxEnableHisto.TabIndex = 233;
            this.CheckBoxEnableHisto.Text = "Enable Histogram";
            this.CheckBoxEnableHisto.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnablePersist
            // 
            this.CheckBoxEnablePersist.AutoSize = true;
            this.CheckBoxEnablePersist.Checked = true;
            this.CheckBoxEnablePersist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEnablePersist.Location = new System.Drawing.Point(261, 167);
            this.CheckBoxEnablePersist.Name = "CheckBoxEnablePersist";
            this.CheckBoxEnablePersist.Size = new System.Drawing.Size(93, 17);
            this.CheckBoxEnablePersist.TabIndex = 234;
            this.CheckBoxEnablePersist.Text = "Enable Persist";
            this.CheckBoxEnablePersist.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(411, 93);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 236;
            this.label13.Text = "Trace Points";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxTracePoints
            // 
            this.TextBoxTracePoints.Location = new System.Drawing.Point(484, 90);
            this.TextBoxTracePoints.Name = "TextBoxTracePoints";
            this.TextBoxTracePoints.Size = new System.Drawing.Size(93, 20);
            this.TextBoxTracePoints.TabIndex = 235;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(424, 149);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 238;
            this.label14.Text = "DSP Gain";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxDspGain
            // 
            this.TextBoxDspGain.Location = new System.Drawing.Point(484, 145);
            this.TextBoxDspGain.Name = "TextBoxDspGain";
            this.TextBoxDspGain.ReadOnly = true;
            this.TextBoxDspGain.Size = new System.Drawing.Size(93, 20);
            this.TextBoxDspGain.TabIndex = 237;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(427, 174);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(51, 13);
            this.label15.TabIndex = 240;
            this.label15.Text = "FFT Gain";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxFftGain
            // 
            this.TextBoxFftGain.Location = new System.Drawing.Point(484, 171);
            this.TextBoxFftGain.Name = "TextBoxFftGain";
            this.TextBoxFftGain.ReadOnly = true;
            this.TextBoxFftGain.Size = new System.Drawing.Size(93, 20);
            this.TextBoxFftGain.TabIndex = 239;
            // 
            // BtnApplyConfiguration
            // 
            this.BtnApplyConfiguration.Location = new System.Drawing.Point(12, 232);
            this.BtnApplyConfiguration.Name = "BtnApplyConfiguration";
            this.BtnApplyConfiguration.Size = new System.Drawing.Size(138, 26);
            this.BtnApplyConfiguration.TabIndex = 241;
            this.BtnApplyConfiguration.Text = "Apply Configuration";
            this.BtnApplyConfiguration.UseVisualStyleBackColor = true;
            this.BtnApplyConfiguration.Click += new System.EventHandler(this.BtnApplyConfiguration_Click);
            // 
            // CheckBoxPause
            // 
            this.CheckBoxPause.AutoSize = true;
            this.CheckBoxPause.Checked = true;
            this.CheckBoxPause.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxPause.Location = new System.Drawing.Point(179, 238);
            this.CheckBoxPause.Name = "CheckBoxPause";
            this.CheckBoxPause.Size = new System.Drawing.Size(56, 17);
            this.CheckBoxPause.TabIndex = 242;
            this.CheckBoxPause.Text = "Pause";
            this.CheckBoxPause.UseVisualStyleBackColor = true;
            this.CheckBoxPause.CheckedChanged += new System.EventHandler(this.CheckBoxPause_CheckedChanged);
            // 
            // SpectrumChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 270);
            this.Controls.Add(this.CheckBoxPause);
            this.Controls.Add(this.BtnApplyConfiguration);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.TextBoxFftGain);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.TextBoxDspGain);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.TextBoxTracePoints);
            this.Controls.Add(this.CheckBoxEnablePersist);
            this.Controls.Add(this.CheckBoxEnableHisto);
            this.Controls.Add(this.CheckBoxEnable);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.TextBoxSweepType);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ComboBoxDetectorType);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ComboBoxFftWindow);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.TextBoxTickStep);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TextBoxRange);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TextBoxReferenceLevel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TextBoxVBW);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TextBoxRBW);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TextBoxStopFreq);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextBoxStartFreq);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxFreqSpan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxCenterFreq);
            this.Name = "SpectrumChannelForm";
            this.Text = "Spectrum Channel Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TextBoxCenterFreq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxFreqSpan;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxStartFreq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxStopFreq;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxVBW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBoxRBW;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextBoxRange;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxReferenceLevel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TextBoxTickStep;
        private System.Windows.Forms.ComboBox ComboBoxFftWindow;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ComboBoxDetectorType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextBoxSweepType;
        private System.Windows.Forms.CheckBox CheckBoxEnable;
        private System.Windows.Forms.CheckBox CheckBoxEnableHisto;
        private System.Windows.Forms.CheckBox CheckBoxEnablePersist;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextBoxTracePoints;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox TextBoxDspGain;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox TextBoxFftGain;
        private System.Windows.Forms.Button BtnApplyConfiguration;
        private System.Windows.Forms.CheckBox CheckBoxPause;
    }
}