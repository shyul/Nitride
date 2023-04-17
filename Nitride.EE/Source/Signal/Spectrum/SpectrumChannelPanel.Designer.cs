namespace Nitride.EE
{
    partial class SpectrumChannelPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label18 = new System.Windows.Forms.Label();
            TextBoxPersistDepth = new System.Windows.Forms.TextBox();
            label17 = new System.Windows.Forms.Label();
            TextBoxHistoDepth = new System.Windows.Forms.TextBox();
            label16 = new System.Windows.Forms.Label();
            TextBoxPersistBufferHeight = new System.Windows.Forms.TextBox();
            CheckBoxPause = new System.Windows.Forms.CheckBox();
            BtnApplyConfiguration = new System.Windows.Forms.Button();
            label15 = new System.Windows.Forms.Label();
            TextBoxFftGain = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            TextBoxDspGain = new System.Windows.Forms.TextBox();
            label13 = new System.Windows.Forms.Label();
            TextBoxTracePoints = new System.Windows.Forms.TextBox();
            CheckBoxEnablePersist = new System.Windows.Forms.CheckBox();
            CheckBoxEnableHisto = new System.Windows.Forms.CheckBox();
            CheckBoxEnable = new System.Windows.Forms.CheckBox();
            label12 = new System.Windows.Forms.Label();
            TextBoxSweepType = new System.Windows.Forms.TextBox();
            label11 = new System.Windows.Forms.Label();
            ComboBoxDetectorType = new System.Windows.Forms.ComboBox();
            label10 = new System.Windows.Forms.Label();
            ComboBoxFftWindow = new System.Windows.Forms.ComboBox();
            label9 = new System.Windows.Forms.Label();
            TextBoxTickStep = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            TextBoxRange = new System.Windows.Forms.TextBox();
            label8 = new System.Windows.Forms.Label();
            TextBoxReferenceLevel = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            TextBoxVBW = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            TextBoxRBW = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            TextBoxStopFreq = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            TextBoxStartFreq = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            TextBoxFreqSpan = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            TextBoxCenterFreq = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(247, 156);
            label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(76, 15);
            label18.TabIndex = 289;
            label18.Text = "Persist Depth";
            label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxPersistDepth
            // 
            TextBoxPersistDepth.Location = new System.Drawing.Point(336, 153);
            TextBoxPersistDepth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxPersistDepth.Name = "TextBoxPersistDepth";
            TextBoxPersistDepth.Size = new System.Drawing.Size(93, 23);
            TextBoxPersistDepth.TabIndex = 288;
            TextBoxPersistDepth.Text = "32";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(255, 126);
            label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(70, 15);
            label17.TabIndex = 287;
            label17.Text = "Histo Depth";
            label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxHistoDepth
            // 
            TextBoxHistoDepth.Location = new System.Drawing.Point(336, 123);
            TextBoxHistoDepth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxHistoDepth.Name = "TextBoxHistoDepth";
            TextBoxHistoDepth.Size = new System.Drawing.Size(93, 23);
            TextBoxHistoDepth.TabIndex = 286;
            TextBoxHistoDepth.Text = "128";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(247, 96);
            label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(76, 15);
            label16.TabIndex = 285;
            label16.Text = "Persist Y Axis";
            label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxPersistBufferHeight
            // 
            TextBoxPersistBufferHeight.Location = new System.Drawing.Point(336, 93);
            TextBoxPersistBufferHeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxPersistBufferHeight.Name = "TextBoxPersistBufferHeight";
            TextBoxPersistBufferHeight.Size = new System.Drawing.Size(93, 23);
            TextBoxPersistBufferHeight.TabIndex = 284;
            TextBoxPersistBufferHeight.Text = "800";
            // 
            // CheckBoxPause
            // 
            CheckBoxPause.AutoSize = true;
            CheckBoxPause.Location = new System.Drawing.Point(206, 247);
            CheckBoxPause.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxPause.Name = "CheckBoxPause";
            CheckBoxPause.Size = new System.Drawing.Size(57, 19);
            CheckBoxPause.TabIndex = 283;
            CheckBoxPause.Text = "Pause";
            CheckBoxPause.UseVisualStyleBackColor = true;
            CheckBoxPause.CheckedChanged += CheckBoxPause_CheckedChanged;
            // 
            // BtnApplyConfiguration
            // 
            BtnApplyConfiguration.Location = new System.Drawing.Point(11, 240);
            BtnApplyConfiguration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnApplyConfiguration.Name = "BtnApplyConfiguration";
            BtnApplyConfiguration.Size = new System.Drawing.Size(161, 30);
            BtnApplyConfiguration.TabIndex = 282;
            BtnApplyConfiguration.Text = "Apply Configuration";
            BtnApplyConfiguration.UseVisualStyleBackColor = true;
            BtnApplyConfiguration.Click += BtnApplyConfiguration_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(468, 204);
            label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(52, 15);
            label15.TabIndex = 281;
            label15.Text = "FFT Gain";
            label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxFftGain
            // 
            TextBoxFftGain.Location = new System.Drawing.Point(535, 200);
            TextBoxFftGain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxFftGain.Name = "TextBoxFftGain";
            TextBoxFftGain.ReadOnly = true;
            TextBoxFftGain.Size = new System.Drawing.Size(108, 23);
            TextBoxFftGain.TabIndex = 280;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(465, 175);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(55, 15);
            label14.TabIndex = 279;
            label14.Text = "DSP Gain";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxDspGain
            // 
            TextBoxDspGain.Location = new System.Drawing.Point(535, 170);
            TextBoxDspGain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxDspGain.Name = "TextBoxDspGain";
            TextBoxDspGain.ReadOnly = true;
            TextBoxDspGain.Size = new System.Drawing.Size(108, 23);
            TextBoxDspGain.TabIndex = 278;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(449, 96);
            label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(70, 15);
            label13.TabIndex = 277;
            label13.Text = "Trace Points";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxTracePoints
            // 
            TextBoxTracePoints.Location = new System.Drawing.Point(535, 93);
            TextBoxTracePoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxTracePoints.Name = "TextBoxTracePoints";
            TextBoxTracePoints.Size = new System.Drawing.Size(108, 23);
            TextBoxTracePoints.TabIndex = 276;
            // 
            // CheckBoxEnablePersist
            // 
            CheckBoxEnablePersist.AutoSize = true;
            CheckBoxEnablePersist.Checked = true;
            CheckBoxEnablePersist.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBoxEnablePersist.Location = new System.Drawing.Point(322, 251);
            CheckBoxEnablePersist.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxEnablePersist.Name = "CheckBoxEnablePersist";
            CheckBoxEnablePersist.Size = new System.Drawing.Size(98, 19);
            CheckBoxEnablePersist.TabIndex = 275;
            CheckBoxEnablePersist.Text = "Enable Persist";
            CheckBoxEnablePersist.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnableHisto
            // 
            CheckBoxEnableHisto.AutoSize = true;
            CheckBoxEnableHisto.Checked = true;
            CheckBoxEnableHisto.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBoxEnableHisto.Location = new System.Drawing.Point(322, 221);
            CheckBoxEnableHisto.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxEnableHisto.Name = "CheckBoxEnableHisto";
            CheckBoxEnableHisto.Size = new System.Drawing.Size(120, 19);
            CheckBoxEnableHisto.TabIndex = 274;
            CheckBoxEnableHisto.Text = "Enable Histogram";
            CheckBoxEnableHisto.UseVisualStyleBackColor = true;
            // 
            // CheckBoxEnable
            // 
            CheckBoxEnable.AutoSize = true;
            CheckBoxEnable.Checked = true;
            CheckBoxEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBoxEnable.Location = new System.Drawing.Point(322, 191);
            CheckBoxEnable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxEnable.Name = "CheckBoxEnable";
            CheckBoxEnable.Size = new System.Drawing.Size(61, 19);
            CheckBoxEnable.TabIndex = 273;
            CheckBoxEnable.Text = "Enable";
            CheckBoxEnable.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(446, 6);
            label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(75, 15);
            label12.TabIndex = 272;
            label12.Text = "Sweep Mode";
            label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxSweepType
            // 
            TextBoxSweepType.Location = new System.Drawing.Point(535, 3);
            TextBoxSweepType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxSweepType.Name = "TextBoxSweepType";
            TextBoxSweepType.ReadOnly = true;
            TextBoxSweepType.Size = new System.Drawing.Size(108, 23);
            TextBoxSweepType.TabIndex = 271;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(440, 66);
            label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(79, 15);
            label11.TabIndex = 270;
            label11.Text = "Detector Type";
            label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxDetectorType
            // 
            ComboBoxDetectorType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ComboBoxDetectorType.FormattingEnabled = true;
            ComboBoxDetectorType.Location = new System.Drawing.Point(535, 62);
            ComboBoxDetectorType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBoxDetectorType.Name = "ComboBoxDetectorType";
            ComboBoxDetectorType.Size = new System.Drawing.Size(108, 21);
            ComboBoxDetectorType.TabIndex = 269;
            ComboBoxDetectorType.Text = "Peak";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(442, 36);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(78, 15);
            label10.TabIndex = 268;
            label10.Text = "Window Type";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxFftWindow
            // 
            ComboBoxFftWindow.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ComboBoxFftWindow.FormattingEnabled = true;
            ComboBoxFftWindow.Items.AddRange(new object[] { "Rectangle", "Triangle", "Hanning", "Hamming", "Nuttall", "Blackman", "BlackmanNuttall", "BlackmanHarris", "FlatTop" });
            ComboBoxFftWindow.Location = new System.Drawing.Point(535, 32);
            ComboBoxFftWindow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBoxFftWindow.Name = "ComboBoxFftWindow";
            ComboBoxFftWindow.Size = new System.Drawing.Size(108, 21);
            ComboBoxFftWindow.TabIndex = 267;
            ComboBoxFftWindow.Text = "Flat Top";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(264, 66);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(59, 15);
            label9.TabIndex = 266;
            label9.Text = "Y Axis Div";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxTickStep
            // 
            TextBoxTickStep.Location = new System.Drawing.Point(336, 63);
            TextBoxTickStep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxTickStep.Name = "TextBoxTickStep";
            TextBoxTickStep.Size = new System.Drawing.Size(93, 23);
            TextBoxTickStep.TabIndex = 265;
            TextBoxTickStep.Text = "10";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(283, 36);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(40, 15);
            label7.TabIndex = 264;
            label7.Text = "Range";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxRange
            // 
            TextBoxRange.Location = new System.Drawing.Point(336, 33);
            TextBoxRange.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxRange.Name = "TextBoxRange";
            TextBoxRange.Size = new System.Drawing.Size(93, 23);
            TextBoxRange.TabIndex = 263;
            TextBoxRange.Text = "130";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(228, 6);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(89, 15);
            label8.TabIndex = 262;
            label8.Text = "Reference Level";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxReferenceLevel
            // 
            TextBoxReferenceLevel.Location = new System.Drawing.Point(336, 3);
            TextBoxReferenceLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxReferenceLevel.Name = "TextBoxReferenceLevel";
            TextBoxReferenceLevel.Size = new System.Drawing.Size(93, 23);
            TextBoxReferenceLevel.TabIndex = 261;
            TextBoxReferenceLevel.Text = "0";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(78, 156);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(32, 15);
            label5.TabIndex = 260;
            label5.Text = "VBW";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxVBW
            // 
            TextBoxVBW.Location = new System.Drawing.Point(124, 153);
            TextBoxVBW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxVBW.Name = "TextBoxVBW";
            TextBoxVBW.ReadOnly = true;
            TextBoxVBW.Size = new System.Drawing.Size(93, 23);
            TextBoxVBW.TabIndex = 259;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(78, 126);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(32, 15);
            label6.TabIndex = 258;
            label6.Text = "RBW";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxRBW
            // 
            TextBoxRBW.Location = new System.Drawing.Point(124, 123);
            TextBoxRBW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxRBW.Name = "TextBoxRBW";
            TextBoxRBW.ReadOnly = true;
            TextBoxRBW.Size = new System.Drawing.Size(93, 23);
            TextBoxRBW.TabIndex = 257;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(21, 96);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(89, 15);
            label4.TabIndex = 256;
            label4.Text = "Stop Frequency";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxStopFreq
            // 
            TextBoxStopFreq.Location = new System.Drawing.Point(124, 93);
            TextBoxStopFreq.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxStopFreq.Name = "TextBoxStopFreq";
            TextBoxStopFreq.ReadOnly = true;
            TextBoxStopFreq.Size = new System.Drawing.Size(93, 23);
            TextBoxStopFreq.TabIndex = 255;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(21, 66);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(89, 15);
            label3.TabIndex = 254;
            label3.Text = "Start Frequency";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxStartFreq
            // 
            TextBoxStartFreq.Location = new System.Drawing.Point(124, 63);
            TextBoxStartFreq.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxStartFreq.Name = "TextBoxStartFreq";
            TextBoxStartFreq.ReadOnly = true;
            TextBoxStartFreq.Size = new System.Drawing.Size(93, 23);
            TextBoxStartFreq.TabIndex = 253;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(18, 36);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(91, 15);
            label2.TabIndex = 252;
            label2.Text = "Frequency Span";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxFreqSpan
            // 
            TextBoxFreqSpan.Location = new System.Drawing.Point(124, 33);
            TextBoxFreqSpan.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxFreqSpan.Name = "TextBoxFreqSpan";
            TextBoxFreqSpan.Size = new System.Drawing.Size(93, 23);
            TextBoxFreqSpan.TabIndex = 251;
            TextBoxFreqSpan.Text = "100000000";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(11, 6);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(100, 15);
            label1.TabIndex = 250;
            label1.Text = "Center Frequency";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxCenterFreq
            // 
            TextBoxCenterFreq.Location = new System.Drawing.Point(124, 3);
            TextBoxCenterFreq.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxCenterFreq.Name = "TextBoxCenterFreq";
            TextBoxCenterFreq.Size = new System.Drawing.Size(93, 23);
            TextBoxCenterFreq.TabIndex = 249;
            TextBoxCenterFreq.Text = "187500000";
            // 
            // SpectrumChannelPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label18);
            Controls.Add(TextBoxPersistDepth);
            Controls.Add(label17);
            Controls.Add(TextBoxHistoDepth);
            Controls.Add(label16);
            Controls.Add(TextBoxPersistBufferHeight);
            Controls.Add(CheckBoxPause);
            Controls.Add(BtnApplyConfiguration);
            Controls.Add(label15);
            Controls.Add(TextBoxFftGain);
            Controls.Add(label14);
            Controls.Add(TextBoxDspGain);
            Controls.Add(label13);
            Controls.Add(TextBoxTracePoints);
            Controls.Add(CheckBoxEnablePersist);
            Controls.Add(CheckBoxEnableHisto);
            Controls.Add(CheckBoxEnable);
            Controls.Add(label12);
            Controls.Add(TextBoxSweepType);
            Controls.Add(label11);
            Controls.Add(ComboBoxDetectorType);
            Controls.Add(label10);
            Controls.Add(ComboBoxFftWindow);
            Controls.Add(label9);
            Controls.Add(TextBoxTickStep);
            Controls.Add(label7);
            Controls.Add(TextBoxRange);
            Controls.Add(label8);
            Controls.Add(TextBoxReferenceLevel);
            Controls.Add(label5);
            Controls.Add(TextBoxVBW);
            Controls.Add(label6);
            Controls.Add(TextBoxRBW);
            Controls.Add(label4);
            Controls.Add(TextBoxStopFreq);
            Controls.Add(label3);
            Controls.Add(TextBoxStartFreq);
            Controls.Add(label2);
            Controls.Add(TextBoxFreqSpan);
            Controls.Add(label1);
            Controls.Add(TextBoxCenterFreq);
            Name = "SpectrumChannelPanel";
            Size = new System.Drawing.Size(648, 277);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox TextBoxPersistDepth;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox TextBoxHistoDepth;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox TextBoxPersistBufferHeight;
        private System.Windows.Forms.CheckBox CheckBoxPause;
        private System.Windows.Forms.Button BtnApplyConfiguration;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox TextBoxFftGain;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox TextBoxDspGain;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextBoxTracePoints;
        private System.Windows.Forms.CheckBox CheckBoxEnablePersist;
        private System.Windows.Forms.CheckBox CheckBoxEnableHisto;
        private System.Windows.Forms.CheckBox CheckBoxEnable;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextBoxSweepType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ComboBoxDetectorType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ComboBoxFftWindow;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TextBoxTickStep;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TextBoxRange;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxReferenceLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxVBW;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBoxRBW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TextBoxStopFreq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxStartFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxFreqSpan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxCenterFreq;
    }
}
