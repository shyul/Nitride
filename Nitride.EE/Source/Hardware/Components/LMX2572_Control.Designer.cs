namespace Nitride.EE
{
    partial class LMX2572_Control
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LMX2572_Control));
            this.LabelVcoLock = new System.Windows.Forms.Label();
            this.LabelChDivider = new System.Windows.Forms.Label();
            this.ComboBoxChDivider = new System.Windows.Forms.ComboBox();
            this.LabelRfoutAMux = new System.Windows.Forms.Label();
            this.LabelVcoSelect = new System.Windows.Forms.Label();
            this.ComboBoxRfoutAMux = new System.Windows.Forms.ComboBox();
            this.ComboBoxVcoSelect = new System.Windows.Forms.ComboBox();
            this.LabelVcoFreq = new System.Windows.Forms.Label();
            this.TextBoxVcoFreq = new System.Windows.Forms.TextBox();
            this.TextBoxPfdFreq = new System.Windows.Forms.TextBox();
            this.TextBoxPreR = new System.Windows.Forms.TextBox();
            this.CheckBoxRefDoublerEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxRMulti = new System.Windows.Forms.TextBox();
            this.CheckBoxMashResetN = new System.Windows.Forms.CheckBox();
            this.CheckBoxMashSeedEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxMashResetCounter = new System.Windows.Forms.TextBox();
            this.Label_Mash_Reset_Counter = new System.Windows.Forms.Label();
            this.TextBoxPfdDlySel = new System.Windows.Forms.TextBox();
            this.TextBoxRfoutAPower = new System.Windows.Forms.TextBox();
            this.Label_PFD_DLY_SEL = new System.Windows.Forms.Label();
            this.CheckBoxRfoutAEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxMashSeed = new System.Windows.Forms.TextBox();
            this.Label_MashSeed = new System.Windows.Forms.Label();
            this.Label_Mash_Order = new System.Windows.Forms.Label();
            this.ComboBoxMashOrder = new System.Windows.Forms.ComboBox();
            this.BtnCalVco = new System.Windows.Forms.Button();
            this.TextBoxFDen = new System.Windows.Forms.TextBox();
            this.TextBoxFNum = new System.Windows.Forms.TextBox();
            this.TextBoxNDiv = new System.Windows.Forms.TextBox();
            this.TextBoxRDiv = new System.Windows.Forms.TextBox();
            this.CheckBoxManualVcoSelect = new System.Windows.Forms.CheckBox();
            this.TextBoxReferenceFreq = new System.Windows.Forms.TextBox();
            this.TextBoxRfoutAFreq = new System.Windows.Forms.TextBox();
            this.ComboBoxChargePumpCurrent = new System.Windows.Forms.ComboBox();
            this.GroupBoxCalibration = new System.Windows.Forms.GroupBox();
            this.CheckBox_QUICK_RECAL_EN = new System.Windows.Forms.CheckBox();
            this.Label_CAL_CLK_DIV = new System.Windows.Forms.Label();
            this.TextBox_CAL_CLK_DIV = new System.Windows.Forms.TextBox();
            this.Label_FCAL_HPFD_ADJ = new System.Windows.Forms.Label();
            this.TextBox_FCAL_HPFD_ADJ = new System.Windows.Forms.TextBox();
            this.Label_FCAL_LPFD_ADJ = new System.Windows.Forms.Label();
            this.TextBox_FCAL_LPFD_ADJ = new System.Windows.Forms.TextBox();
            this.GroupBoxRfoutA = new System.Windows.Forms.GroupBox();
            this.LabelRfoutAPower = new System.Windows.Forms.Label();
            this.LabelRfoutAFreq = new System.Windows.Forms.Label();
            this.GroupBoxRfoutB = new System.Windows.Forms.GroupBox();
            this.LabelRfoutBPower = new System.Windows.Forms.Label();
            this.LabelRfoutBFreq = new System.Windows.Forms.Label();
            this.ComboBoxRfoutBMux = new System.Windows.Forms.ComboBox();
            this.CheckBoxRfoutBEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxRfoutBPower = new System.Windows.Forms.TextBox();
            this.LabelRfoutBMux = new System.Windows.Forms.Label();
            this.TextBoxRfoutBFreq = new System.Windows.Forms.TextBox();
            this.CheckBoxSyncEnable = new System.Windows.Forms.CheckBox();
            this.GroupBoxCalibration.SuspendLayout();
            this.GroupBoxRfoutA.SuspendLayout();
            this.GroupBoxRfoutB.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelVcoLock
            // 
            this.LabelVcoLock.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoLock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelVcoLock.ForeColor = System.Drawing.Color.Red;
            this.LabelVcoLock.Location = new System.Drawing.Point(847, 82);
            this.LabelVcoLock.Name = "LabelVcoLock";
            this.LabelVcoLock.Size = new System.Drawing.Size(57, 13);
            this.LabelVcoLock.TabIndex = 178;
            this.LabelVcoLock.Text = "Unlock";
            this.LabelVcoLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelChDivider
            // 
            this.LabelChDivider.Location = new System.Drawing.Point(1005, 159);
            this.LabelChDivider.Name = "LabelChDivider";
            this.LabelChDivider.Size = new System.Drawing.Size(67, 13);
            this.LabelChDivider.TabIndex = 220;
            this.LabelChDivider.Text = "Ch Divider";
            this.LabelChDivider.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboBoxChDivider
            // 
            this.ComboBoxChDivider.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxChDivider.FormattingEnabled = true;
            this.ComboBoxChDivider.Items.AddRange(new object[] {
            "2",
            "4",
            "8",
            "16",
            "32",
            "64",
            "128",
            "256"});
            this.ComboBoxChDivider.Location = new System.Drawing.Point(1005, 201);
            this.ComboBoxChDivider.Name = "ComboBoxChDivider";
            this.ComboBoxChDivider.Size = new System.Drawing.Size(67, 21);
            this.ComboBoxChDivider.TabIndex = 219;
            this.ComboBoxChDivider.Text = "2";
            // 
            // LabelRfoutAMux
            // 
            this.LabelRfoutAMux.Location = new System.Drawing.Point(81, 14);
            this.LabelRfoutAMux.Name = "LabelRfoutAMux";
            this.LabelRfoutAMux.Size = new System.Drawing.Size(62, 13);
            this.LabelRfoutAMux.TabIndex = 201;
            this.LabelRfoutAMux.Text = "Path";
            this.LabelRfoutAMux.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelVcoSelect
            // 
            this.LabelVcoSelect.AutoSize = true;
            this.LabelVcoSelect.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoSelect.Location = new System.Drawing.Point(813, 185);
            this.LabelVcoSelect.Name = "LabelVcoSelect";
            this.LabelVcoSelect.Size = new System.Drawing.Size(37, 13);
            this.LabelVcoSelect.TabIndex = 206;
            this.LabelVcoSelect.Text = "Select";
            this.LabelVcoSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboBoxRfoutAMux
            // 
            this.ComboBoxRfoutAMux.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxRfoutAMux.FormattingEnabled = true;
            this.ComboBoxRfoutAMux.Items.AddRange(new object[] {
            "Divider",
            "VCO"});
            this.ComboBoxRfoutAMux.Location = new System.Drawing.Point(81, 31);
            this.ComboBoxRfoutAMux.Name = "ComboBoxRfoutAMux";
            this.ComboBoxRfoutAMux.Size = new System.Drawing.Size(62, 21);
            this.ComboBoxRfoutAMux.TabIndex = 197;
            this.ComboBoxRfoutAMux.Text = "Divider";
            // 
            // ComboBoxVcoSelect
            // 
            this.ComboBoxVcoSelect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxVcoSelect.FormattingEnabled = true;
            this.ComboBoxVcoSelect.Items.AddRange(new object[] {
            "Auto",
            "VCO1",
            "VCO2",
            "VCO3",
            "VCO4",
            "VCO5",
            "VCO6"});
            this.ComboBoxVcoSelect.Location = new System.Drawing.Point(856, 181);
            this.ComboBoxVcoSelect.Name = "ComboBoxVcoSelect";
            this.ComboBoxVcoSelect.Size = new System.Drawing.Size(81, 21);
            this.ComboBoxVcoSelect.TabIndex = 203;
            this.ComboBoxVcoSelect.Text = "Auto";
            // 
            // LabelVcoFreq
            // 
            this.LabelVcoFreq.AutoSize = true;
            this.LabelVcoFreq.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoFreq.Location = new System.Drawing.Point(847, 104);
            this.LabelVcoFreq.Name = "LabelVcoFreq";
            this.LabelVcoFreq.Size = new System.Drawing.Size(60, 13);
            this.LabelVcoFreq.TabIndex = 217;
            this.LabelVcoFreq.Text = "Frequency";
            this.LabelVcoFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBoxVcoFreq
            // 
            this.TextBoxVcoFreq.Location = new System.Drawing.Point(816, 123);
            this.TextBoxVcoFreq.Name = "TextBoxVcoFreq";
            this.TextBoxVcoFreq.ReadOnly = true;
            this.TextBoxVcoFreq.Size = new System.Drawing.Size(121, 22);
            this.TextBoxVcoFreq.TabIndex = 215;
            // 
            // TextBoxPfdFreq
            // 
            this.TextBoxPfdFreq.Location = new System.Drawing.Point(499, 51);
            this.TextBoxPfdFreq.Name = "TextBoxPfdFreq";
            this.TextBoxPfdFreq.ReadOnly = true;
            this.TextBoxPfdFreq.Size = new System.Drawing.Size(81, 22);
            this.TextBoxPfdFreq.TabIndex = 6;
            // 
            // TextBoxPreR
            // 
            this.TextBoxPreR.Location = new System.Drawing.Point(247, 51);
            this.TextBoxPreR.Name = "TextBoxPreR";
            this.TextBoxPreR.Size = new System.Drawing.Size(45, 22);
            this.TextBoxPreR.TabIndex = 3;
            this.TextBoxPreR.Text = "1";
            // 
            // CheckBoxRefDoublerEnable
            // 
            this.CheckBoxRefDoublerEnable.AutoSize = true;
            this.CheckBoxRefDoublerEnable.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxRefDoublerEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxRefDoublerEnable.Location = new System.Drawing.Point(149, 53);
            this.CheckBoxRefDoublerEnable.Name = "CheckBoxRefDoublerEnable";
            this.CheckBoxRefDoublerEnable.Size = new System.Drawing.Size(61, 17);
            this.CheckBoxRefDoublerEnable.TabIndex = 2;
            this.CheckBoxRefDoublerEnable.Text = "Enable";
            this.CheckBoxRefDoublerEnable.UseVisualStyleBackColor = false;
            // 
            // TextBoxRMulti
            // 
            this.TextBoxRMulti.Location = new System.Drawing.Point(330, 51);
            this.TextBoxRMulti.Name = "TextBoxRMulti";
            this.TextBoxRMulti.Size = new System.Drawing.Size(47, 22);
            this.TextBoxRMulti.TabIndex = 4;
            this.TextBoxRMulti.Text = "1";
            // 
            // CheckBoxMashResetN
            // 
            this.CheckBoxMashResetN.AutoSize = true;
            this.CheckBoxMashResetN.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxMashResetN.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckBoxMashResetN.Location = new System.Drawing.Point(618, 265);
            this.CheckBoxMashResetN.Name = "CheckBoxMashResetN";
            this.CheckBoxMashResetN.Size = new System.Drawing.Size(39, 31);
            this.CheckBoxMashResetN.TabIndex = 209;
            this.CheckBoxMashResetN.Text = "Reset";
            this.CheckBoxMashResetN.UseVisualStyleBackColor = false;
            // 
            // CheckBoxMashSeedEnable
            // 
            this.CheckBoxMashSeedEnable.AutoSize = true;
            this.CheckBoxMashSeedEnable.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxMashSeedEnable.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckBoxMashSeedEnable.Location = new System.Drawing.Point(576, 265);
            this.CheckBoxMashSeedEnable.Name = "CheckBoxMashSeedEnable";
            this.CheckBoxMashSeedEnable.Size = new System.Drawing.Size(36, 31);
            this.CheckBoxMashSeedEnable.TabIndex = 208;
            this.CheckBoxMashSeedEnable.Text = "Seed";
            this.CheckBoxMashSeedEnable.UseVisualStyleBackColor = false;
            // 
            // TextBoxMashResetCounter
            // 
            this.TextBoxMashResetCounter.Location = new System.Drawing.Point(638, 332);
            this.TextBoxMashResetCounter.Name = "TextBoxMashResetCounter";
            this.TextBoxMashResetCounter.Size = new System.Drawing.Size(103, 22);
            this.TextBoxMashResetCounter.TabIndex = 205;
            // 
            // Label_Mash_Reset_Counter
            // 
            this.Label_Mash_Reset_Counter.AutoSize = true;
            this.Label_Mash_Reset_Counter.BackColor = System.Drawing.Color.Transparent;
            this.Label_Mash_Reset_Counter.Location = new System.Drawing.Point(552, 336);
            this.Label_Mash_Reset_Counter.Name = "Label_Mash_Reset_Counter";
            this.Label_Mash_Reset_Counter.Size = new System.Drawing.Size(80, 13);
            this.Label_Mash_Reset_Counter.TabIndex = 207;
            this.Label_Mash_Reset_Counter.Text = "Reset Counter";
            this.Label_Mash_Reset_Counter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxPfdDlySel
            // 
            this.TextBoxPfdDlySel.Location = new System.Drawing.Point(638, 360);
            this.TextBoxPfdDlySel.Name = "TextBoxPfdDlySel";
            this.TextBoxPfdDlySel.ReadOnly = true;
            this.TextBoxPfdDlySel.Size = new System.Drawing.Size(103, 22);
            this.TextBoxPfdDlySel.TabIndex = 202;
            this.TextBoxPfdDlySel.Text = "1";
            // 
            // TextBoxRfoutAPower
            // 
            this.TextBoxRfoutAPower.Location = new System.Drawing.Point(73, 60);
            this.TextBoxRfoutAPower.Name = "TextBoxRfoutAPower";
            this.TextBoxRfoutAPower.Size = new System.Drawing.Size(70, 22);
            this.TextBoxRfoutAPower.TabIndex = 192;
            this.TextBoxRfoutAPower.Text = "0";
            // 
            // Label_PFD_DLY_SEL
            // 
            this.Label_PFD_DLY_SEL.AutoSize = true;
            this.Label_PFD_DLY_SEL.BackColor = System.Drawing.Color.Transparent;
            this.Label_PFD_DLY_SEL.Location = new System.Drawing.Point(561, 365);
            this.Label_PFD_DLY_SEL.Name = "Label_PFD_DLY_SEL";
            this.Label_PFD_DLY_SEL.Size = new System.Drawing.Size(71, 13);
            this.Label_PFD_DLY_SEL.TabIndex = 204;
            this.Label_PFD_DLY_SEL.Text = "PFD_DLY_SEL";
            this.Label_PFD_DLY_SEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBoxRfoutAEnable
            // 
            this.CheckBoxRfoutAEnable.AutoSize = true;
            this.CheckBoxRfoutAEnable.Location = new System.Drawing.Point(14, 33);
            this.CheckBoxRfoutAEnable.Name = "CheckBoxRfoutAEnable";
            this.CheckBoxRfoutAEnable.Size = new System.Drawing.Size(61, 17);
            this.CheckBoxRfoutAEnable.TabIndex = 191;
            this.CheckBoxRfoutAEnable.Text = "Enable";
            this.CheckBoxRfoutAEnable.UseVisualStyleBackColor = true;
            // 
            // TextBoxMashSeed
            // 
            this.TextBoxMashSeed.Location = new System.Drawing.Point(638, 304);
            this.TextBoxMashSeed.Name = "TextBoxMashSeed";
            this.TextBoxMashSeed.Size = new System.Drawing.Size(103, 22);
            this.TextBoxMashSeed.TabIndex = 198;
            // 
            // Label_MashSeed
            // 
            this.Label_MashSeed.AutoSize = true;
            this.Label_MashSeed.BackColor = System.Drawing.Color.Transparent;
            this.Label_MashSeed.Location = new System.Drawing.Point(569, 309);
            this.Label_MashSeed.Name = "Label_MashSeed";
            this.Label_MashSeed.Size = new System.Drawing.Size(63, 13);
            this.Label_MashSeed.TabIndex = 200;
            this.Label_MashSeed.Text = "Mash Seed";
            this.Label_MashSeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_Mash_Order
            // 
            this.Label_Mash_Order.AutoSize = true;
            this.Label_Mash_Order.BackColor = System.Drawing.Color.Transparent;
            this.Label_Mash_Order.Location = new System.Drawing.Point(564, 238);
            this.Label_Mash_Order.Name = "Label_Mash_Order";
            this.Label_Mash_Order.Size = new System.Drawing.Size(68, 13);
            this.Label_Mash_Order.TabIndex = 190;
            this.Label_Mash_Order.Text = "Mash Order";
            this.Label_Mash_Order.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboBoxMashOrder
            // 
            this.ComboBoxMashOrder.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxMashOrder.FormattingEnabled = true;
            this.ComboBoxMashOrder.Items.AddRange(new object[] {
            "INT",
            "1",
            "2",
            "3",
            "4"});
            this.ComboBoxMashOrder.Location = new System.Drawing.Point(638, 235);
            this.ComboBoxMashOrder.Name = "ComboBoxMashOrder";
            this.ComboBoxMashOrder.Size = new System.Drawing.Size(103, 21);
            this.ComboBoxMashOrder.TabIndex = 189;
            // 
            // BtnCalVco
            // 
            this.BtnCalVco.Location = new System.Drawing.Point(816, 218);
            this.BtnCalVco.Name = "BtnCalVco";
            this.BtnCalVco.Size = new System.Drawing.Size(121, 22);
            this.BtnCalVco.TabIndex = 188;
            this.BtnCalVco.Text = "Cal VCO";
            this.BtnCalVco.UseVisualStyleBackColor = true;
            // 
            // TextBoxFDen
            // 
            this.TextBoxFDen.Location = new System.Drawing.Point(663, 151);
            this.TextBoxFDen.Name = "TextBoxFDen";
            this.TextBoxFDen.Size = new System.Drawing.Size(78, 22);
            this.TextBoxFDen.TabIndex = 186;
            // 
            // TextBoxFNum
            // 
            this.TextBoxFNum.Location = new System.Drawing.Point(663, 114);
            this.TextBoxFNum.Name = "TextBoxFNum";
            this.TextBoxFNum.Size = new System.Drawing.Size(78, 22);
            this.TextBoxFNum.TabIndex = 184;
            // 
            // TextBoxNDiv
            // 
            this.TextBoxNDiv.Location = new System.Drawing.Point(567, 134);
            this.TextBoxNDiv.Name = "TextBoxNDiv";
            this.TextBoxNDiv.Size = new System.Drawing.Size(74, 22);
            this.TextBoxNDiv.TabIndex = 182;
            // 
            // TextBoxRDiv
            // 
            this.TextBoxRDiv.Location = new System.Drawing.Point(414, 51);
            this.TextBoxRDiv.Name = "TextBoxRDiv";
            this.TextBoxRDiv.Size = new System.Drawing.Size(48, 22);
            this.TextBoxRDiv.TabIndex = 5;
            this.TextBoxRDiv.Text = "1";
            // 
            // CheckBoxManualVcoSelect
            // 
            this.CheckBoxManualVcoSelect.AutoSize = true;
            this.CheckBoxManualVcoSelect.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxManualVcoSelect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBoxManualVcoSelect.Location = new System.Drawing.Point(829, 158);
            this.CheckBoxManualVcoSelect.Name = "CheckBoxManualVcoSelect";
            this.CheckBoxManualVcoSelect.Size = new System.Drawing.Size(98, 17);
            this.CheckBoxManualVcoSelect.TabIndex = 224;
            this.CheckBoxManualVcoSelect.Text = "Manual Select";
            this.CheckBoxManualVcoSelect.UseVisualStyleBackColor = false;
            // 
            // TextBoxReferenceFreq
            // 
            this.TextBoxReferenceFreq.Location = new System.Drawing.Point(30, 51);
            this.TextBoxReferenceFreq.Name = "TextBoxReferenceFreq";
            this.TextBoxReferenceFreq.ReadOnly = true;
            this.TextBoxReferenceFreq.Size = new System.Drawing.Size(82, 22);
            this.TextBoxReferenceFreq.TabIndex = 1;
            // 
            // TextBoxRfoutAFreq
            // 
            this.TextBoxRfoutAFreq.Location = new System.Drawing.Point(43, 88);
            this.TextBoxRfoutAFreq.Name = "TextBoxRfoutAFreq";
            this.TextBoxRfoutAFreq.ReadOnly = true;
            this.TextBoxRfoutAFreq.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutAFreq.TabIndex = 226;
            // 
            // ComboBoxChargePumpCurrent
            // 
            this.ComboBoxChargePumpCurrent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxChargePumpCurrent.FormattingEnabled = true;
            this.ComboBoxChargePumpCurrent.Items.AddRange(new object[] {
            "Tri-State",
            "0.625 mA",
            "1.25 mA",
            "1.875 mA",
            "2.5 mA",
            "3.125 mA",
            "3.75 mA",
            "4.375 mA",
            "5 mA",
            "5.625 mA",
            "6.25 mA",
            "6.875 mA"});
            this.ComboBoxChargePumpCurrent.Location = new System.Drawing.Point(619, 52);
            this.ComboBoxChargePumpCurrent.Name = "ComboBoxChargePumpCurrent";
            this.ComboBoxChargePumpCurrent.Size = new System.Drawing.Size(70, 21);
            this.ComboBoxChargePumpCurrent.TabIndex = 7;
            this.ComboBoxChargePumpCurrent.Text = "6.875 mA";
            // 
            // GroupBoxCalibration
            // 
            this.GroupBoxCalibration.Controls.Add(this.CheckBox_QUICK_RECAL_EN);
            this.GroupBoxCalibration.Controls.Add(this.Label_CAL_CLK_DIV);
            this.GroupBoxCalibration.Controls.Add(this.TextBox_CAL_CLK_DIV);
            this.GroupBoxCalibration.Controls.Add(this.Label_FCAL_HPFD_ADJ);
            this.GroupBoxCalibration.Controls.Add(this.TextBox_FCAL_HPFD_ADJ);
            this.GroupBoxCalibration.Controls.Add(this.Label_FCAL_LPFD_ADJ);
            this.GroupBoxCalibration.Controls.Add(this.TextBox_FCAL_LPFD_ADJ);
            this.GroupBoxCalibration.Location = new System.Drawing.Point(292, 114);
            this.GroupBoxCalibration.Name = "GroupBoxCalibration";
            this.GroupBoxCalibration.Size = new System.Drawing.Size(217, 138);
            this.GroupBoxCalibration.TabIndex = 230;
            this.GroupBoxCalibration.TabStop = false;
            this.GroupBoxCalibration.Text = "Calibration";
            // 
            // CheckBox_QUICK_RECAL_EN
            // 
            this.CheckBox_QUICK_RECAL_EN.AutoSize = true;
            this.CheckBox_QUICK_RECAL_EN.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_QUICK_RECAL_EN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_QUICK_RECAL_EN.Location = new System.Drawing.Point(50, 114);
            this.CheckBox_QUICK_RECAL_EN.Name = "CheckBox_QUICK_RECAL_EN";
            this.CheckBox_QUICK_RECAL_EN.Size = new System.Drawing.Size(114, 17);
            this.CheckBox_QUICK_RECAL_EN.TabIndex = 240;
            this.CheckBox_QUICK_RECAL_EN.Text = "QUICK_RECAL_EN";
            this.CheckBox_QUICK_RECAL_EN.UseVisualStyleBackColor = false;
            // 
            // Label_CAL_CLK_DIV
            // 
            this.Label_CAL_CLK_DIV.AutoSize = true;
            this.Label_CAL_CLK_DIV.BackColor = System.Drawing.Color.Transparent;
            this.Label_CAL_CLK_DIV.Location = new System.Drawing.Point(25, 80);
            this.Label_CAL_CLK_DIV.Name = "Label_CAL_CLK_DIV";
            this.Label_CAL_CLK_DIV.Size = new System.Drawing.Size(72, 13);
            this.Label_CAL_CLK_DIV.TabIndex = 239;
            this.Label_CAL_CLK_DIV.Text = "CAL_CLK_DIV";
            this.Label_CAL_CLK_DIV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_CAL_CLK_DIV
            // 
            this.TextBox_CAL_CLK_DIV.Location = new System.Drawing.Point(103, 77);
            this.TextBox_CAL_CLK_DIV.Name = "TextBox_CAL_CLK_DIV";
            this.TextBox_CAL_CLK_DIV.ReadOnly = true;
            this.TextBox_CAL_CLK_DIV.Size = new System.Drawing.Size(102, 22);
            this.TextBox_CAL_CLK_DIV.TabIndex = 238;
            // 
            // Label_FCAL_HPFD_ADJ
            // 
            this.Label_FCAL_HPFD_ADJ.AutoSize = true;
            this.Label_FCAL_HPFD_ADJ.BackColor = System.Drawing.Color.Transparent;
            this.Label_FCAL_HPFD_ADJ.Location = new System.Drawing.Point(8, 52);
            this.Label_FCAL_HPFD_ADJ.Name = "Label_FCAL_HPFD_ADJ";
            this.Label_FCAL_HPFD_ADJ.Size = new System.Drawing.Size(89, 13);
            this.Label_FCAL_HPFD_ADJ.TabIndex = 237;
            this.Label_FCAL_HPFD_ADJ.Text = "FCAL_HPFD_ADJ";
            this.Label_FCAL_HPFD_ADJ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_FCAL_HPFD_ADJ
            // 
            this.TextBox_FCAL_HPFD_ADJ.Location = new System.Drawing.Point(103, 49);
            this.TextBox_FCAL_HPFD_ADJ.Name = "TextBox_FCAL_HPFD_ADJ";
            this.TextBox_FCAL_HPFD_ADJ.ReadOnly = true;
            this.TextBox_FCAL_HPFD_ADJ.Size = new System.Drawing.Size(102, 22);
            this.TextBox_FCAL_HPFD_ADJ.TabIndex = 236;
            // 
            // Label_FCAL_LPFD_ADJ
            // 
            this.Label_FCAL_LPFD_ADJ.AutoSize = true;
            this.Label_FCAL_LPFD_ADJ.BackColor = System.Drawing.Color.Transparent;
            this.Label_FCAL_LPFD_ADJ.Location = new System.Drawing.Point(11, 24);
            this.Label_FCAL_LPFD_ADJ.Name = "Label_FCAL_LPFD_ADJ";
            this.Label_FCAL_LPFD_ADJ.Size = new System.Drawing.Size(86, 13);
            this.Label_FCAL_LPFD_ADJ.TabIndex = 235;
            this.Label_FCAL_LPFD_ADJ.Text = "FCAL_LPFD_ADJ";
            this.Label_FCAL_LPFD_ADJ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_FCAL_LPFD_ADJ
            // 
            this.TextBox_FCAL_LPFD_ADJ.Location = new System.Drawing.Point(103, 21);
            this.TextBox_FCAL_LPFD_ADJ.Name = "TextBox_FCAL_LPFD_ADJ";
            this.TextBox_FCAL_LPFD_ADJ.ReadOnly = true;
            this.TextBox_FCAL_LPFD_ADJ.Size = new System.Drawing.Size(102, 22);
            this.TextBox_FCAL_LPFD_ADJ.TabIndex = 234;
            // 
            // GroupBoxRfoutA
            // 
            this.GroupBoxRfoutA.Controls.Add(this.LabelRfoutAPower);
            this.GroupBoxRfoutA.Controls.Add(this.LabelRfoutAFreq);
            this.GroupBoxRfoutA.Controls.Add(this.ComboBoxRfoutAMux);
            this.GroupBoxRfoutA.Controls.Add(this.CheckBoxRfoutAEnable);
            this.GroupBoxRfoutA.Controls.Add(this.TextBoxRfoutAPower);
            this.GroupBoxRfoutA.Controls.Add(this.LabelRfoutAMux);
            this.GroupBoxRfoutA.Controls.Add(this.TextBoxRfoutAFreq);
            this.GroupBoxRfoutA.Location = new System.Drawing.Point(1112, 10);
            this.GroupBoxRfoutA.Name = "GroupBoxRfoutA";
            this.GroupBoxRfoutA.Size = new System.Drawing.Size(151, 117);
            this.GroupBoxRfoutA.TabIndex = 231;
            this.GroupBoxRfoutA.TabStop = false;
            this.GroupBoxRfoutA.Text = "RFOUT A";
            // 
            // LabelRfoutAPower
            // 
            this.LabelRfoutAPower.AutoSize = true;
            this.LabelRfoutAPower.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutAPower.Location = new System.Drawing.Point(28, 63);
            this.LabelRfoutAPower.Name = "LabelRfoutAPower";
            this.LabelRfoutAPower.Size = new System.Drawing.Size(39, 13);
            this.LabelRfoutAPower.TabIndex = 228;
            this.LabelRfoutAPower.Text = "Power";
            this.LabelRfoutAPower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelRfoutAFreq
            // 
            this.LabelRfoutAFreq.AutoSize = true;
            this.LabelRfoutAFreq.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutAFreq.Location = new System.Drawing.Point(7, 91);
            this.LabelRfoutAFreq.Name = "LabelRfoutAFreq";
            this.LabelRfoutAFreq.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutAFreq.TabIndex = 227;
            this.LabelRfoutAFreq.Text = "Freq";
            this.LabelRfoutAFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBoxRfoutB
            // 
            this.GroupBoxRfoutB.Controls.Add(this.LabelRfoutBPower);
            this.GroupBoxRfoutB.Controls.Add(this.LabelRfoutBFreq);
            this.GroupBoxRfoutB.Controls.Add(this.ComboBoxRfoutBMux);
            this.GroupBoxRfoutB.Controls.Add(this.CheckBoxRfoutBEnable);
            this.GroupBoxRfoutB.Controls.Add(this.TextBoxRfoutBPower);
            this.GroupBoxRfoutB.Controls.Add(this.LabelRfoutBMux);
            this.GroupBoxRfoutB.Controls.Add(this.TextBoxRfoutBFreq);
            this.GroupBoxRfoutB.Location = new System.Drawing.Point(1112, 293);
            this.GroupBoxRfoutB.Name = "GroupBoxRfoutB";
            this.GroupBoxRfoutB.Size = new System.Drawing.Size(151, 117);
            this.GroupBoxRfoutB.TabIndex = 232;
            this.GroupBoxRfoutB.TabStop = false;
            this.GroupBoxRfoutB.Text = "RFOUT B";
            // 
            // LabelRfoutBPower
            // 
            this.LabelRfoutBPower.AutoSize = true;
            this.LabelRfoutBPower.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutBPower.Location = new System.Drawing.Point(28, 63);
            this.LabelRfoutBPower.Name = "LabelRfoutBPower";
            this.LabelRfoutBPower.Size = new System.Drawing.Size(39, 13);
            this.LabelRfoutBPower.TabIndex = 228;
            this.LabelRfoutBPower.Text = "Power";
            this.LabelRfoutBPower.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelRfoutBFreq
            // 
            this.LabelRfoutBFreq.AutoSize = true;
            this.LabelRfoutBFreq.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutBFreq.Location = new System.Drawing.Point(7, 91);
            this.LabelRfoutBFreq.Name = "LabelRfoutBFreq";
            this.LabelRfoutBFreq.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutBFreq.TabIndex = 227;
            this.LabelRfoutBFreq.Text = "Freq";
            this.LabelRfoutBFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboBoxRfoutBMux
            // 
            this.ComboBoxRfoutBMux.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBoxRfoutBMux.FormattingEnabled = true;
            this.ComboBoxRfoutBMux.Items.AddRange(new object[] {
            "Divider",
            "VCO",
            "SysRef"});
            this.ComboBoxRfoutBMux.Location = new System.Drawing.Point(81, 31);
            this.ComboBoxRfoutBMux.Name = "ComboBoxRfoutBMux";
            this.ComboBoxRfoutBMux.Size = new System.Drawing.Size(62, 21);
            this.ComboBoxRfoutBMux.TabIndex = 197;
            this.ComboBoxRfoutBMux.Text = "Divider";
            // 
            // CheckBoxRfoutBEnable
            // 
            this.CheckBoxRfoutBEnable.AutoSize = true;
            this.CheckBoxRfoutBEnable.Location = new System.Drawing.Point(14, 33);
            this.CheckBoxRfoutBEnable.Name = "CheckBoxRfoutBEnable";
            this.CheckBoxRfoutBEnable.Size = new System.Drawing.Size(61, 17);
            this.CheckBoxRfoutBEnable.TabIndex = 191;
            this.CheckBoxRfoutBEnable.Text = "Enable";
            this.CheckBoxRfoutBEnable.UseVisualStyleBackColor = true;
            // 
            // TextBoxRfoutBPower
            // 
            this.TextBoxRfoutBPower.Location = new System.Drawing.Point(73, 60);
            this.TextBoxRfoutBPower.Name = "TextBoxRfoutBPower";
            this.TextBoxRfoutBPower.Size = new System.Drawing.Size(70, 22);
            this.TextBoxRfoutBPower.TabIndex = 192;
            this.TextBoxRfoutBPower.Text = "0";
            // 
            // LabelRfoutBMux
            // 
            this.LabelRfoutBMux.Location = new System.Drawing.Point(81, 14);
            this.LabelRfoutBMux.Name = "LabelRfoutBMux";
            this.LabelRfoutBMux.Size = new System.Drawing.Size(62, 13);
            this.LabelRfoutBMux.TabIndex = 201;
            this.LabelRfoutBMux.Text = "Path";
            this.LabelRfoutBMux.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxRfoutBFreq
            // 
            this.TextBoxRfoutBFreq.Location = new System.Drawing.Point(43, 88);
            this.TextBoxRfoutBFreq.Name = "TextBoxRfoutBFreq";
            this.TextBoxRfoutBFreq.ReadOnly = true;
            this.TextBoxRfoutBFreq.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutBFreq.TabIndex = 226;
            // 
            // CheckBoxSyncEnable
            // 
            this.CheckBoxSyncEnable.AutoSize = true;
            this.CheckBoxSyncEnable.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxSyncEnable.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.CheckBoxSyncEnable.Location = new System.Drawing.Point(663, 265);
            this.CheckBoxSyncEnable.Name = "CheckBoxSyncEnable";
            this.CheckBoxSyncEnable.Size = new System.Drawing.Size(67, 31);
            this.CheckBoxSyncEnable.TabIndex = 233;
            this.CheckBoxSyncEnable.Text = "Phase Sync";
            this.CheckBoxSyncEnable.UseVisualStyleBackColor = false;
            // 
            // LMX2572_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.CheckBoxSyncEnable);
            this.Controls.Add(this.GroupBoxRfoutB);
            this.Controls.Add(this.GroupBoxRfoutA);
            this.Controls.Add(this.GroupBoxCalibration);
            this.Controls.Add(this.ComboBoxChargePumpCurrent);
            this.Controls.Add(this.TextBoxReferenceFreq);
            this.Controls.Add(this.CheckBoxManualVcoSelect);
            this.Controls.Add(this.LabelVcoLock);
            this.Controls.Add(this.LabelChDivider);
            this.Controls.Add(this.ComboBoxChDivider);
            this.Controls.Add(this.LabelVcoSelect);
            this.Controls.Add(this.ComboBoxVcoSelect);
            this.Controls.Add(this.LabelVcoFreq);
            this.Controls.Add(this.TextBoxVcoFreq);
            this.Controls.Add(this.TextBoxPfdFreq);
            this.Controls.Add(this.TextBoxPreR);
            this.Controls.Add(this.CheckBoxRefDoublerEnable);
            this.Controls.Add(this.TextBoxRMulti);
            this.Controls.Add(this.CheckBoxMashResetN);
            this.Controls.Add(this.CheckBoxMashSeedEnable);
            this.Controls.Add(this.TextBoxMashResetCounter);
            this.Controls.Add(this.Label_Mash_Reset_Counter);
            this.Controls.Add(this.TextBoxPfdDlySel);
            this.Controls.Add(this.Label_PFD_DLY_SEL);
            this.Controls.Add(this.TextBoxMashSeed);
            this.Controls.Add(this.Label_MashSeed);
            this.Controls.Add(this.Label_Mash_Order);
            this.Controls.Add(this.ComboBoxMashOrder);
            this.Controls.Add(this.BtnCalVco);
            this.Controls.Add(this.TextBoxFDen);
            this.Controls.Add(this.TextBoxFNum);
            this.Controls.Add(this.TextBoxNDiv);
            this.Controls.Add(this.TextBoxRDiv);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "LMX2572_Control";
            this.Size = new System.Drawing.Size(1272, 419);
            this.GroupBoxCalibration.ResumeLayout(false);
            this.GroupBoxCalibration.PerformLayout();
            this.GroupBoxRfoutA.ResumeLayout(false);
            this.GroupBoxRfoutA.PerformLayout();
            this.GroupBoxRfoutB.ResumeLayout(false);
            this.GroupBoxRfoutB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxReferenceFreq;
        private System.Windows.Forms.CheckBox CheckBoxRefDoublerEnable;
        private System.Windows.Forms.TextBox TextBoxPreR;
        private System.Windows.Forms.TextBox TextBoxRMulti;
        private System.Windows.Forms.TextBox TextBoxPfdFreq;
        private System.Windows.Forms.ComboBox ComboBoxChargePumpCurrent;

        private System.Windows.Forms.TextBox TextBoxFDen;
        private System.Windows.Forms.TextBox TextBoxFNum;
        private System.Windows.Forms.TextBox TextBoxNDiv;
        private System.Windows.Forms.TextBox TextBoxRDiv;

        private System.Windows.Forms.Label LabelVcoLock;
        private System.Windows.Forms.Label LabelVcoFreq;
        private System.Windows.Forms.TextBox TextBoxVcoFreq;
        private System.Windows.Forms.CheckBox CheckBoxManualVcoSelect;
        private System.Windows.Forms.Label LabelVcoSelect;
        private System.Windows.Forms.ComboBox ComboBoxVcoSelect;
        private System.Windows.Forms.Button BtnCalVco;

        private System.Windows.Forms.CheckBox CheckBoxMashResetN;
        private System.Windows.Forms.CheckBox CheckBoxMashSeedEnable;
        private System.Windows.Forms.CheckBox CheckBoxSyncEnable;
        private System.Windows.Forms.Label Label_Mash_Reset_Counter;
        private System.Windows.Forms.TextBox TextBoxMashResetCounter;
        private System.Windows.Forms.TextBox TextBoxPfdDlySel;
        private System.Windows.Forms.Label Label_PFD_DLY_SEL;
        private System.Windows.Forms.Label Label_MashSeed;
        private System.Windows.Forms.TextBox TextBoxMashSeed;
        private System.Windows.Forms.Label Label_Mash_Order;
        private System.Windows.Forms.ComboBox ComboBoxMashOrder;

        private System.Windows.Forms.Label LabelChDivider;
        private System.Windows.Forms.ComboBox ComboBoxChDivider;

        private System.Windows.Forms.GroupBox GroupBoxRfoutA;
        private System.Windows.Forms.Label LabelRfoutAMux;
        private System.Windows.Forms.ComboBox ComboBoxRfoutAMux;
        private System.Windows.Forms.Label LabelRfoutAPower;
        private System.Windows.Forms.TextBox TextBoxRfoutAPower;
        private System.Windows.Forms.Label LabelRfoutAFreq;
        private System.Windows.Forms.TextBox TextBoxRfoutAFreq;
        private System.Windows.Forms.CheckBox CheckBoxRfoutAEnable;

        private System.Windows.Forms.GroupBox GroupBoxRfoutB;
        private System.Windows.Forms.Label LabelRfoutBMux;
        private System.Windows.Forms.ComboBox ComboBoxRfoutBMux;
        private System.Windows.Forms.Label LabelRfoutBPower;
        private System.Windows.Forms.TextBox TextBoxRfoutBPower;
        private System.Windows.Forms.Label LabelRfoutBFreq;
        private System.Windows.Forms.TextBox TextBoxRfoutBFreq;
        private System.Windows.Forms.CheckBox CheckBoxRfoutBEnable;

        private System.Windows.Forms.GroupBox GroupBoxCalibration;
        private System.Windows.Forms.Label Label_CAL_CLK_DIV;
        private System.Windows.Forms.TextBox TextBox_CAL_CLK_DIV;
        private System.Windows.Forms.Label Label_FCAL_HPFD_ADJ;
        private System.Windows.Forms.TextBox TextBox_FCAL_HPFD_ADJ;
        private System.Windows.Forms.Label Label_FCAL_LPFD_ADJ;
        private System.Windows.Forms.TextBox TextBox_FCAL_LPFD_ADJ;
        private System.Windows.Forms.CheckBox CheckBox_QUICK_RECAL_EN;
    }
}
