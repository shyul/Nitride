namespace Nitride.EE
{
    partial class SpectrumControlForm
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
            this.ComboBoxSampleLength = new System.Windows.Forms.ComboBox();
            this.BtnTrigContinous = new System.Windows.Forms.Button();
            this.BtnAdcDataTrigStop = new System.Windows.Forms.Button();
            this.label112 = new System.Windows.Forms.Label();
            this.CheckBoxEnableTimeDomain = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ComboBoxSweepMode = new System.Windows.Forms.ComboBox();
            this.CheckBoxPause = new System.Windows.Forms.CheckBox();
            this.BtnApplyConfiguration = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.TextBoxSampleRate = new System.Windows.Forms.TextBox();
            this.TextBoxDecmRate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxBandwidth = new System.Windows.Forms.TextBox();
            this.BtnTrigSingle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComboBoxSampleLength
            // 
            this.ComboBoxSampleLength.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxSampleLength.FormattingEnabled = true;
            this.ComboBoxSampleLength.Location = new System.Drawing.Point(131, 39);
            this.ComboBoxSampleLength.Name = "ComboBoxSampleLength";
            this.ComboBoxSampleLength.Size = new System.Drawing.Size(107, 21);
            this.ComboBoxSampleLength.TabIndex = 119;
            this.ComboBoxSampleLength.Text = "8192";
            // 
            // BtnTrigContinous
            // 
            this.BtnTrigContinous.BackColor = System.Drawing.Color.GreenYellow;
            this.BtnTrigContinous.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTrigContinous.ForeColor = System.Drawing.Color.Green;
            this.BtnTrigContinous.Location = new System.Drawing.Point(12, 224);
            this.BtnTrigContinous.Name = "BtnTrigContinous";
            this.BtnTrigContinous.Size = new System.Drawing.Size(85, 26);
            this.BtnTrigContinous.TabIndex = 114;
            this.BtnTrigContinous.Text = "Continous";
            this.BtnTrigContinous.UseVisualStyleBackColor = false;
            this.BtnTrigContinous.Click += new System.EventHandler(this.BtnTrigContinous_Click);
            // 
            // BtnAdcDataTrigStop
            // 
            this.BtnAdcDataTrigStop.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdcDataTrigStop.ForeColor = System.Drawing.Color.Red;
            this.BtnAdcDataTrigStop.Location = new System.Drawing.Point(244, 224);
            this.BtnAdcDataTrigStop.Name = "BtnAdcDataTrigStop";
            this.BtnAdcDataTrigStop.Size = new System.Drawing.Size(77, 26);
            this.BtnAdcDataTrigStop.TabIndex = 115;
            this.BtnAdcDataTrigStop.Text = "Stop";
            this.BtnAdcDataTrigStop.UseVisualStyleBackColor = true;
            this.BtnAdcDataTrigStop.Click += new System.EventHandler(this.BtnAdcDataTrigStop_Click);
            // 
            // label112
            // 
            this.label112.Location = new System.Drawing.Point(47, 42);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(78, 13);
            this.label112.TabIndex = 116;
            this.label112.Text = "Sample Length";
            this.label112.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CheckBoxEnableTimeDomain
            // 
            this.CheckBoxEnableTimeDomain.AutoSize = true;
            this.CheckBoxEnableTimeDomain.Checked = true;
            this.CheckBoxEnableTimeDomain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxEnableTimeDomain.Location = new System.Drawing.Point(114, 147);
            this.CheckBoxEnableTimeDomain.Name = "CheckBoxEnableTimeDomain";
            this.CheckBoxEnableTimeDomain.Size = new System.Drawing.Size(124, 17);
            this.CheckBoxEnableTimeDomain.TabIndex = 233;
            this.CheckBoxEnableTimeDomain.Text = "Enable Time Domain";
            this.CheckBoxEnableTimeDomain.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(55, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 235;
            this.label10.Text = "Sweep Mode";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ComboBoxSweepMode
            // 
            this.ComboBoxSweepMode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxSweepMode.FormattingEnabled = true;
            this.ComboBoxSweepMode.Location = new System.Drawing.Point(131, 12);
            this.ComboBoxSweepMode.Name = "ComboBoxSweepMode";
            this.ComboBoxSweepMode.Size = new System.Drawing.Size(107, 21);
            this.ComboBoxSweepMode.TabIndex = 234;
            this.ComboBoxSweepMode.Text = "FFT";
            // 
            // CheckBoxPause
            // 
            this.CheckBoxPause.AutoSize = true;
            this.CheckBoxPause.Checked = true;
            this.CheckBoxPause.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxPause.Location = new System.Drawing.Point(103, 230);
            this.CheckBoxPause.Name = "CheckBoxPause";
            this.CheckBoxPause.Size = new System.Drawing.Size(56, 17);
            this.CheckBoxPause.TabIndex = 244;
            this.CheckBoxPause.Text = "Pause";
            this.CheckBoxPause.UseVisualStyleBackColor = true;
            this.CheckBoxPause.CheckedChanged += new System.EventHandler(this.CheckBoxPause_CheckedChanged);
            // 
            // BtnApplyConfiguration
            // 
            this.BtnApplyConfiguration.Location = new System.Drawing.Point(119, 170);
            this.BtnApplyConfiguration.Name = "BtnApplyConfiguration";
            this.BtnApplyConfiguration.Size = new System.Drawing.Size(119, 26);
            this.BtnApplyConfiguration.TabIndex = 243;
            this.BtnApplyConfiguration.Text = "Apply Configuration";
            this.BtnApplyConfiguration.UseVisualStyleBackColor = true;
            this.BtnApplyConfiguration.Click += new System.EventHandler(this.BtnApplyConfiguration_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(57, 95);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 246;
            this.label14.Text = "Sample Rate";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxSampleRate
            // 
            this.TextBoxSampleRate.Location = new System.Drawing.Point(131, 92);
            this.TextBoxSampleRate.Name = "TextBoxSampleRate";
            this.TextBoxSampleRate.ReadOnly = true;
            this.TextBoxSampleRate.Size = new System.Drawing.Size(107, 20);
            this.TextBoxSampleRate.TabIndex = 245;
            // 
            // TextBoxDecmRate
            // 
            this.TextBoxDecmRate.Location = new System.Drawing.Point(131, 66);
            this.TextBoxDecmRate.Name = "TextBoxDecmRate";
            this.TextBoxDecmRate.Size = new System.Drawing.Size(107, 20);
            this.TextBoxDecmRate.TabIndex = 247;
            this.TextBoxDecmRate.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 248;
            this.label1.Text = "Decimation Rate";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 250;
            this.label2.Text = "Bandwidth";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBoxBandwidth
            // 
            this.TextBoxBandwidth.Location = new System.Drawing.Point(131, 118);
            this.TextBoxBandwidth.Name = "TextBoxBandwidth";
            this.TextBoxBandwidth.ReadOnly = true;
            this.TextBoxBandwidth.Size = new System.Drawing.Size(107, 20);
            this.TextBoxBandwidth.TabIndex = 249;
            // 
            // BtnTrigSingle
            // 
            this.BtnTrigSingle.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BtnTrigSingle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTrigSingle.ForeColor = System.Drawing.Color.Teal;
            this.BtnTrigSingle.Location = new System.Drawing.Point(165, 224);
            this.BtnTrigSingle.Name = "BtnTrigSingle";
            this.BtnTrigSingle.Size = new System.Drawing.Size(73, 26);
            this.BtnTrigSingle.TabIndex = 251;
            this.BtnTrigSingle.Text = "Single";
            this.BtnTrigSingle.UseVisualStyleBackColor = false;
            this.BtnTrigSingle.Click += new System.EventHandler(this.BtnTrigSingle_Click);
            // 
            // SpectrumControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 260);
            this.Controls.Add(this.BtnTrigSingle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxBandwidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxDecmRate);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.TextBoxSampleRate);
            this.Controls.Add(this.CheckBoxPause);
            this.Controls.Add(this.BtnApplyConfiguration);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ComboBoxSweepMode);
            this.Controls.Add(this.CheckBoxEnableTimeDomain);
            this.Controls.Add(this.ComboBoxSampleLength);
            this.Controls.Add(this.BtnTrigContinous);
            this.Controls.Add(this.BtnAdcDataTrigStop);
            this.Controls.Add(this.label112);
            this.Name = "SpectrumControlForm";
            this.Text = "Spectrum Control";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBoxSampleLength;
        private System.Windows.Forms.Button BtnTrigContinous;
        private System.Windows.Forms.Button BtnAdcDataTrigStop;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.CheckBox CheckBoxEnableTimeDomain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ComboBoxSweepMode;
        private System.Windows.Forms.CheckBox CheckBoxPause;
        private System.Windows.Forms.Button BtnApplyConfiguration;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox TextBoxSampleRate;
        private System.Windows.Forms.TextBox TextBoxDecmRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxBandwidth;
        private System.Windows.Forms.Button BtnTrigSingle;
    }
}