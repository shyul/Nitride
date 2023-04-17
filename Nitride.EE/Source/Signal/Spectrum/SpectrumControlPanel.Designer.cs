namespace Nitride.EE
{
    partial class SpectrumControlPanel
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
            label3 = new System.Windows.Forms.Label();
            TextBoxSampleTime = new System.Windows.Forms.TextBox();
            BtnTrigSingle = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            TextBoxBandwidth = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            TextBoxDecmRate = new System.Windows.Forms.TextBox();
            label14 = new System.Windows.Forms.Label();
            TextBoxSampleRate = new System.Windows.Forms.TextBox();
            CheckBoxPause = new System.Windows.Forms.CheckBox();
            BtnApplyConfiguration = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            ComboBoxSweepMode = new System.Windows.Forms.ComboBox();
            CheckBoxEnableTimeDomain = new System.Windows.Forms.CheckBox();
            ComboBoxSampleLength = new System.Windows.Forms.ComboBox();
            BtnTrigContinous = new System.Windows.Forms.Button();
            BtnAdcDataTrigStop = new System.Windows.Forms.Button();
            label112 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(65, 172);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(75, 15);
            label3.TabIndex = 271;
            label3.Text = "Sample Time";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxSampleTime
            // 
            TextBoxSampleTime.Location = new System.Drawing.Point(152, 168);
            TextBoxSampleTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxSampleTime.Name = "TextBoxSampleTime";
            TextBoxSampleTime.ReadOnly = true;
            TextBoxSampleTime.Size = new System.Drawing.Size(124, 23);
            TextBoxSampleTime.TabIndex = 270;
            // 
            // BtnTrigSingle
            // 
            BtnTrigSingle.BackColor = System.Drawing.Color.PaleTurquoise;
            BtnTrigSingle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            BtnTrigSingle.ForeColor = System.Drawing.Color.Teal;
            BtnTrigSingle.Location = new System.Drawing.Point(191, 289);
            BtnTrigSingle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnTrigSingle.Name = "BtnTrigSingle";
            BtnTrigSingle.Size = new System.Drawing.Size(85, 30);
            BtnTrigSingle.TabIndex = 269;
            BtnTrigSingle.Text = "Single";
            BtnTrigSingle.UseVisualStyleBackColor = false;
            BtnTrigSingle.Click += BtnTrigSingle_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(78, 142);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(64, 15);
            label2.TabIndex = 268;
            label2.Text = "Bandwidth";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxBandwidth
            // 
            TextBoxBandwidth.Location = new System.Drawing.Point(152, 138);
            TextBoxBandwidth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxBandwidth.Name = "TextBoxBandwidth";
            TextBoxBandwidth.ReadOnly = true;
            TextBoxBandwidth.Size = new System.Drawing.Size(124, 23);
            TextBoxBandwidth.TabIndex = 267;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(45, 82);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(94, 15);
            label1.TabIndex = 266;
            label1.Text = "Decimation Rate";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxDecmRate
            // 
            TextBoxDecmRate.Location = new System.Drawing.Point(152, 78);
            TextBoxDecmRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxDecmRate.Name = "TextBoxDecmRate";
            TextBoxDecmRate.Size = new System.Drawing.Size(124, 23);
            TextBoxDecmRate.TabIndex = 265;
            TextBoxDecmRate.Text = "1";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(65, 112);
            label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(72, 15);
            label14.TabIndex = 264;
            label14.Text = "Sample Rate";
            label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxSampleRate
            // 
            TextBoxSampleRate.Location = new System.Drawing.Point(152, 108);
            TextBoxSampleRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBoxSampleRate.Name = "TextBoxSampleRate";
            TextBoxSampleRate.ReadOnly = true;
            TextBoxSampleRate.Size = new System.Drawing.Size(124, 23);
            TextBoxSampleRate.TabIndex = 263;
            // 
            // CheckBoxPause
            // 
            CheckBoxPause.AutoSize = true;
            CheckBoxPause.Location = new System.Drawing.Point(119, 296);
            CheckBoxPause.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxPause.Name = "CheckBoxPause";
            CheckBoxPause.Size = new System.Drawing.Size(57, 19);
            CheckBoxPause.TabIndex = 262;
            CheckBoxPause.Text = "Pause";
            CheckBoxPause.UseVisualStyleBackColor = true;
            CheckBoxPause.CheckedChanged += CheckBoxPause_CheckedChanged;
            // 
            // BtnApplyConfiguration
            // 
            BtnApplyConfiguration.Location = new System.Drawing.Point(137, 241);
            BtnApplyConfiguration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnApplyConfiguration.Name = "BtnApplyConfiguration";
            BtnApplyConfiguration.Size = new System.Drawing.Size(139, 30);
            BtnApplyConfiguration.TabIndex = 261;
            BtnApplyConfiguration.Text = "Apply Configuration";
            BtnApplyConfiguration.UseVisualStyleBackColor = true;
            BtnApplyConfiguration.Click += BtnApplyConfiguration_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(63, 19);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(75, 15);
            label10.TabIndex = 260;
            label10.Text = "Sweep Mode";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxSweepMode
            // 
            ComboBoxSweepMode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ComboBoxSweepMode.FormattingEnabled = true;
            ComboBoxSweepMode.Location = new System.Drawing.Point(152, 16);
            ComboBoxSweepMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBoxSweepMode.Name = "ComboBoxSweepMode";
            ComboBoxSweepMode.Size = new System.Drawing.Size(124, 21);
            ComboBoxSweepMode.TabIndex = 259;
            ComboBoxSweepMode.Text = "FFT";
            // 
            // CheckBoxEnableTimeDomain
            // 
            CheckBoxEnableTimeDomain.AutoSize = true;
            CheckBoxEnableTimeDomain.Checked = true;
            CheckBoxEnableTimeDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBoxEnableTimeDomain.Location = new System.Drawing.Point(137, 216);
            CheckBoxEnableTimeDomain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBoxEnableTimeDomain.Name = "CheckBoxEnableTimeDomain";
            CheckBoxEnableTimeDomain.Size = new System.Drawing.Size(135, 19);
            CheckBoxEnableTimeDomain.TabIndex = 258;
            CheckBoxEnableTimeDomain.Text = "Enable Time Domain";
            CheckBoxEnableTimeDomain.UseVisualStyleBackColor = true;
            // 
            // ComboBoxSampleLength
            // 
            ComboBoxSampleLength.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ComboBoxSampleLength.FormattingEnabled = true;
            ComboBoxSampleLength.Location = new System.Drawing.Point(152, 47);
            ComboBoxSampleLength.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBoxSampleLength.Name = "ComboBoxSampleLength";
            ComboBoxSampleLength.Size = new System.Drawing.Size(124, 21);
            ComboBoxSampleLength.TabIndex = 257;
            ComboBoxSampleLength.Text = "8192";
            // 
            // BtnTrigContinous
            // 
            BtnTrigContinous.BackColor = System.Drawing.Color.GreenYellow;
            BtnTrigContinous.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            BtnTrigContinous.ForeColor = System.Drawing.Color.Green;
            BtnTrigContinous.Location = new System.Drawing.Point(13, 289);
            BtnTrigContinous.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnTrigContinous.Name = "BtnTrigContinous";
            BtnTrigContinous.Size = new System.Drawing.Size(99, 30);
            BtnTrigContinous.TabIndex = 254;
            BtnTrigContinous.Text = "Continous";
            BtnTrigContinous.UseVisualStyleBackColor = false;
            BtnTrigContinous.Click += BtnTrigContinous_Click;
            // 
            // BtnAdcDataTrigStop
            // 
            BtnAdcDataTrigStop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            BtnAdcDataTrigStop.ForeColor = System.Drawing.Color.Red;
            BtnAdcDataTrigStop.Location = new System.Drawing.Point(284, 289);
            BtnAdcDataTrigStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BtnAdcDataTrigStop.Name = "BtnAdcDataTrigStop";
            BtnAdcDataTrigStop.Size = new System.Drawing.Size(90, 30);
            BtnAdcDataTrigStop.TabIndex = 255;
            BtnAdcDataTrigStop.Text = "Stop";
            BtnAdcDataTrigStop.UseVisualStyleBackColor = true;
            BtnAdcDataTrigStop.Click += BtnAdcDataTrigStop_Click;
            // 
            // label112
            // 
            label112.Location = new System.Drawing.Point(54, 50);
            label112.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label112.Name = "label112";
            label112.Size = new System.Drawing.Size(91, 15);
            label112.TabIndex = 256;
            label112.Text = "Sample Length";
            label112.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SpectrumControlPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label3);
            Controls.Add(TextBoxSampleTime);
            Controls.Add(BtnTrigSingle);
            Controls.Add(label2);
            Controls.Add(TextBoxBandwidth);
            Controls.Add(label1);
            Controls.Add(TextBoxDecmRate);
            Controls.Add(label14);
            Controls.Add(TextBoxSampleRate);
            Controls.Add(CheckBoxPause);
            Controls.Add(BtnApplyConfiguration);
            Controls.Add(label10);
            Controls.Add(ComboBoxSweepMode);
            Controls.Add(CheckBoxEnableTimeDomain);
            Controls.Add(ComboBoxSampleLength);
            Controls.Add(BtnTrigContinous);
            Controls.Add(BtnAdcDataTrigStop);
            Controls.Add(label112);
            Name = "SpectrumControlPanel";
            Size = new System.Drawing.Size(390, 334);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TextBoxSampleTime;
        private System.Windows.Forms.Button BtnTrigSingle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxBandwidth;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox TextBoxDecmRate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox TextBoxSampleRate;
        private System.Windows.Forms.CheckBox CheckBoxPause;
        private System.Windows.Forms.Button BtnApplyConfiguration;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.ComboBox ComboBoxSweepMode;
        private System.Windows.Forms.CheckBox CheckBoxEnableTimeDomain;
        public System.Windows.Forms.ComboBox ComboBoxSampleLength;
        private System.Windows.Forms.Button BtnTrigContinous;
        private System.Windows.Forms.Button BtnAdcDataTrigStop;
        private System.Windows.Forms.Label label112;
    }
}
