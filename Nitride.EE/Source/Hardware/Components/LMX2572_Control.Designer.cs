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
            this.TextBoxPfdDlySel = new System.Windows.Forms.TextBox();
            this.TextBoxRfoutAPower = new System.Windows.Forms.TextBox();
            this.Label_PFD_DLY_SEL = new System.Windows.Forms.Label();
            this.CheckBoxRfoutAEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxMashSeed = new System.Windows.Forms.TextBox();
            this.Label_MashSeed = new System.Windows.Forms.Label();
            this.Label_Mash_Order = new System.Windows.Forms.Label();
            this.ComboBoxMashOrder = new System.Windows.Forms.ComboBox();
            this.TextBoxFDen = new System.Windows.Forms.TextBox();
            this.TextBoxFNum = new System.Windows.Forms.TextBox();
            this.TextBoxNDiv = new System.Windows.Forms.TextBox();
            this.TextBoxRDiv = new System.Windows.Forms.TextBox();
            this.CheckBoxManualVcoSelect = new System.Windows.Forms.CheckBox();
            this.TextBoxReferenceFreq = new System.Windows.Forms.TextBox();
            this.TextBoxRfoutAFreq = new System.Windows.Forms.TextBox();
            this.ComboBoxChargePumpCurrent = new System.Windows.Forms.ComboBox();
            this.Label_CAL_CLK_DIV = new System.Windows.Forms.Label();
            this.TextBox_CAL_CLK_DIV = new System.Windows.Forms.TextBox();
            this.LabelRfoutAPower = new System.Windows.Forms.Label();
            this.LabelRfoutAFreq = new System.Windows.Forms.Label();
            this.LabelRfoutBPower = new System.Windows.Forms.Label();
            this.LabelRfoutBFreq = new System.Windows.Forms.Label();
            this.ComboBoxRfoutBMux = new System.Windows.Forms.ComboBox();
            this.CheckBoxRfoutBEnable = new System.Windows.Forms.CheckBox();
            this.TextBoxRfoutBPower = new System.Windows.Forms.TextBox();
            this.LabelRfoutBMux = new System.Windows.Forms.Label();
            this.TextBoxRfoutBFreq = new System.Windows.Forms.TextBox();
            this.BtnLoadTICS = new System.Windows.Forms.Button();
            this.BtnUpdateUI = new System.Windows.Forms.Button();
            this.BtnCommit = new System.Windows.Forms.Button();
            this.LabelPhaseShift = new System.Windows.Forms.Label();
            this.TextBoxPhaseShift = new System.Windows.Forms.TextBox();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.LabelRfoutAPhaseShift = new System.Windows.Forms.Label();
            this.TextBoxRfoutAPhaseShift = new System.Windows.Forms.TextBox();
            this.LabelRfoutBPhaseShift = new System.Windows.Forms.Label();
            this.TextBoxRfoutBPhaseShift = new System.Windows.Forms.TextBox();
            this.GroupBoxVcoCalibration = new System.Windows.Forms.GroupBox();
            this.CheckBox_VCO_CAPCTRL_FORCE = new System.Windows.Forms.CheckBox();
            this.Label_StartAmplitude = new System.Windows.Forms.Label();
            this.TextBox_VcoStartAmplitude = new System.Windows.Forms.TextBox();
            this.BtnCalVco = new System.Windows.Forms.Button();
            this.TextBox_FCAL_LPFD_ADJ = new System.Windows.Forms.TextBox();
            this.Label_FCAL_LPFD_ADJ = new System.Windows.Forms.Label();
            this.CheckBox_FCAL_EN = new System.Windows.Forms.CheckBox();
            this.TextBox_VCO_CAPCTRL = new System.Windows.Forms.TextBox();
            this.TextBox_FCAL_HPFD_ADJ = new System.Windows.Forms.TextBox();
            this.Label_FCAL_HPFD_ADJ = new System.Windows.Forms.Label();
            this.CheckBox_QUICK_RECAL_EN = new System.Windows.Forms.CheckBox();
            this.GroupBoxPhaseSync = new System.Windows.Forms.GroupBox();
            this.CheckBoxPsyncPin = new System.Windows.Forms.CheckBox();
            this.LabelMashResetDelay = new System.Windows.Forms.Label();
            this.TextBoxMashResetDelay = new System.Windows.Forms.TextBox();
            this.TextBoxMashResetCounter = new System.Windows.Forms.TextBox();
            this.CheckBoxSyncEnable = new System.Windows.Forms.CheckBox();
            this.Label_Mash_Reset_Counter = new System.Windows.Forms.Label();
            this.GroupBoxVcoCalibration.SuspendLayout();
            this.GroupBoxPhaseSync.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelVcoLock
            // 
            this.LabelVcoLock.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoLock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LabelVcoLock.ForeColor = System.Drawing.Color.Red;
            this.LabelVcoLock.Location = new System.Drawing.Point(850, 84);
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
            this.ComboBoxChDivider.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.LabelRfoutAMux.AutoSize = true;
            this.LabelRfoutAMux.Location = new System.Drawing.Point(1128, 81);
            this.LabelRfoutAMux.Name = "LabelRfoutAMux";
            this.LabelRfoutAMux.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutAMux.TabIndex = 201;
            this.LabelRfoutAMux.Text = "Path";
            this.LabelRfoutAMux.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LabelVcoSelect
            // 
            this.LabelVcoSelect.AutoSize = true;
            this.LabelVcoSelect.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoSelect.Location = new System.Drawing.Point(813, 179);
            this.LabelVcoSelect.Name = "LabelVcoSelect";
            this.LabelVcoSelect.Size = new System.Drawing.Size(37, 13);
            this.LabelVcoSelect.TabIndex = 206;
            this.LabelVcoSelect.Text = "Select";
            this.LabelVcoSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboBoxRfoutAMux
            // 
            this.ComboBoxRfoutAMux.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComboBoxRfoutAMux.FormattingEnabled = true;
            this.ComboBoxRfoutAMux.Items.AddRange(new object[] {
            "Divider",
            "VCO"});
            this.ComboBoxRfoutAMux.Location = new System.Drawing.Point(1112, 97);
            this.ComboBoxRfoutAMux.Name = "ComboBoxRfoutAMux";
            this.ComboBoxRfoutAMux.Size = new System.Drawing.Size(62, 21);
            this.ComboBoxRfoutAMux.TabIndex = 197;
            this.ComboBoxRfoutAMux.Text = "Divider";
            // 
            // ComboBoxVcoSelect
            // 
            this.ComboBoxVcoSelect.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComboBoxVcoSelect.FormattingEnabled = true;
            this.ComboBoxVcoSelect.Items.AddRange(new object[] {
            "VCO1",
            "VCO2",
            "VCO3",
            "VCO4",
            "VCO5",
            "VCO6"});
            this.ComboBoxVcoSelect.Location = new System.Drawing.Point(856, 176);
            this.ComboBoxVcoSelect.Name = "ComboBoxVcoSelect";
            this.ComboBoxVcoSelect.Size = new System.Drawing.Size(81, 21);
            this.ComboBoxVcoSelect.TabIndex = 203;
            this.ComboBoxVcoSelect.Text = "VCO1";
            // 
            // LabelVcoFreq
            // 
            this.LabelVcoFreq.AutoSize = true;
            this.LabelVcoFreq.BackColor = System.Drawing.Color.Transparent;
            this.LabelVcoFreq.Location = new System.Drawing.Point(847, 105);
            this.LabelVcoFreq.Name = "LabelVcoFreq";
            this.LabelVcoFreq.Size = new System.Drawing.Size(60, 13);
            this.LabelVcoFreq.TabIndex = 217;
            this.LabelVcoFreq.Text = "Frequency";
            this.LabelVcoFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBoxVcoFreq
            // 
            this.TextBoxVcoFreq.Location = new System.Drawing.Point(816, 125);
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
            this.CheckBoxMashResetN.Location = new System.Drawing.Point(619, 289);
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
            this.CheckBoxMashSeedEnable.Location = new System.Drawing.Point(552, 289);
            this.CheckBoxMashSeedEnable.Name = "CheckBoxMashSeedEnable";
            this.CheckBoxMashSeedEnable.Size = new System.Drawing.Size(36, 31);
            this.CheckBoxMashSeedEnable.TabIndex = 208;
            this.CheckBoxMashSeedEnable.Text = "Seed";
            this.CheckBoxMashSeedEnable.UseVisualStyleBackColor = false;
            // 
            // TextBoxPfdDlySel
            // 
            this.TextBoxPfdDlySel.Location = new System.Drawing.Point(590, 258);
            this.TextBoxPfdDlySel.Name = "TextBoxPfdDlySel";
            this.TextBoxPfdDlySel.ReadOnly = true;
            this.TextBoxPfdDlySel.Size = new System.Drawing.Size(103, 22);
            this.TextBoxPfdDlySel.TabIndex = 202;
            this.TextBoxPfdDlySel.Text = "1";
            // 
            // TextBoxRfoutAPower
            // 
            this.TextBoxRfoutAPower.Location = new System.Drawing.Point(1184, 96);
            this.TextBoxRfoutAPower.Name = "TextBoxRfoutAPower";
            this.TextBoxRfoutAPower.Size = new System.Drawing.Size(70, 22);
            this.TextBoxRfoutAPower.TabIndex = 192;
            this.TextBoxRfoutAPower.Text = "0";
            // 
            // Label_PFD_DLY_SEL
            // 
            this.Label_PFD_DLY_SEL.AutoSize = true;
            this.Label_PFD_DLY_SEL.BackColor = System.Drawing.Color.Transparent;
            this.Label_PFD_DLY_SEL.Location = new System.Drawing.Point(513, 261);
            this.Label_PFD_DLY_SEL.Name = "Label_PFD_DLY_SEL";
            this.Label_PFD_DLY_SEL.Size = new System.Drawing.Size(71, 13);
            this.Label_PFD_DLY_SEL.TabIndex = 204;
            this.Label_PFD_DLY_SEL.Text = "PFD_DLY_SEL";
            this.Label_PFD_DLY_SEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBoxRfoutAEnable
            // 
            this.CheckBoxRfoutAEnable.AutoSize = true;
            this.CheckBoxRfoutAEnable.Location = new System.Drawing.Point(1193, 185);
            this.CheckBoxRfoutAEnable.Name = "CheckBoxRfoutAEnable";
            this.CheckBoxRfoutAEnable.Size = new System.Drawing.Size(61, 17);
            this.CheckBoxRfoutAEnable.TabIndex = 191;
            this.CheckBoxRfoutAEnable.Text = "Enable";
            this.CheckBoxRfoutAEnable.UseVisualStyleBackColor = true;
            // 
            // TextBoxMashSeed
            // 
            this.TextBoxMashSeed.Location = new System.Drawing.Point(590, 334);
            this.TextBoxMashSeed.Name = "TextBoxMashSeed";
            this.TextBoxMashSeed.Size = new System.Drawing.Size(103, 22);
            this.TextBoxMashSeed.TabIndex = 198;
            // 
            // Label_MashSeed
            // 
            this.Label_MashSeed.AutoSize = true;
            this.Label_MashSeed.BackColor = System.Drawing.Color.Transparent;
            this.Label_MashSeed.Location = new System.Drawing.Point(521, 339);
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
            this.Label_Mash_Order.Location = new System.Drawing.Point(516, 234);
            this.Label_Mash_Order.Name = "Label_Mash_Order";
            this.Label_Mash_Order.Size = new System.Drawing.Size(68, 13);
            this.Label_Mash_Order.TabIndex = 190;
            this.Label_Mash_Order.Text = "Mash Order";
            this.Label_Mash_Order.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboBoxMashOrder
            // 
            this.ComboBoxMashOrder.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComboBoxMashOrder.FormattingEnabled = true;
            this.ComboBoxMashOrder.Items.AddRange(new object[] {
            "INT",
            "1",
            "2",
            "3",
            "4"});
            this.ComboBoxMashOrder.Location = new System.Drawing.Point(590, 231);
            this.ComboBoxMashOrder.Name = "ComboBoxMashOrder";
            this.ComboBoxMashOrder.Size = new System.Drawing.Size(103, 21);
            this.ComboBoxMashOrder.TabIndex = 189;
            // 
            // TextBoxFDen
            // 
            this.TextBoxFDen.Location = new System.Drawing.Point(615, 151);
            this.TextBoxFDen.Name = "TextBoxFDen";
            this.TextBoxFDen.Size = new System.Drawing.Size(78, 22);
            this.TextBoxFDen.TabIndex = 186;
            // 
            // TextBoxFNum
            // 
            this.TextBoxFNum.Location = new System.Drawing.Point(615, 114);
            this.TextBoxFNum.Name = "TextBoxFNum";
            this.TextBoxFNum.Size = new System.Drawing.Size(78, 22);
            this.TextBoxFNum.TabIndex = 184;
            // 
            // TextBoxNDiv
            // 
            this.TextBoxNDiv.Location = new System.Drawing.Point(518, 134);
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
            this.CheckBoxManualVcoSelect.Location = new System.Drawing.Point(832, 156);
            this.CheckBoxManualVcoSelect.Name = "CheckBoxManualVcoSelect";
            this.CheckBoxManualVcoSelect.Size = new System.Drawing.Size(98, 17);
            this.CheckBoxManualVcoSelect.TabIndex = 224;
            this.CheckBoxManualVcoSelect.Text = "Manual Select";
            this.CheckBoxManualVcoSelect.UseVisualStyleBackColor = false;
            this.CheckBoxManualVcoSelect.CheckedChanged += new System.EventHandler(this.CheckBoxManualVcoSelect_CheckedChanged);
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
            this.TextBoxRfoutAFreq.Location = new System.Drawing.Point(1155, 23);
            this.TextBoxRfoutAFreq.Name = "TextBoxRfoutAFreq";
            this.TextBoxRfoutAFreq.ReadOnly = true;
            this.TextBoxRfoutAFreq.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutAFreq.TabIndex = 226;
            // 
            // ComboBoxChargePumpCurrent
            // 
            this.ComboBoxChargePumpCurrent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            // Label_CAL_CLK_DIV
            // 
            this.Label_CAL_CLK_DIV.AutoSize = true;
            this.Label_CAL_CLK_DIV.BackColor = System.Drawing.Color.Transparent;
            this.Label_CAL_CLK_DIV.Location = new System.Drawing.Point(991, 362);
            this.Label_CAL_CLK_DIV.Name = "Label_CAL_CLK_DIV";
            this.Label_CAL_CLK_DIV.Size = new System.Drawing.Size(72, 13);
            this.Label_CAL_CLK_DIV.TabIndex = 239;
            this.Label_CAL_CLK_DIV.Text = "CAL_CLK_DIV";
            this.Label_CAL_CLK_DIV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_CAL_CLK_DIV
            // 
            this.TextBox_CAL_CLK_DIV.Location = new System.Drawing.Point(982, 379);
            this.TextBox_CAL_CLK_DIV.Name = "TextBox_CAL_CLK_DIV";
            this.TextBox_CAL_CLK_DIV.ReadOnly = true;
            this.TextBox_CAL_CLK_DIV.Size = new System.Drawing.Size(90, 22);
            this.TextBox_CAL_CLK_DIV.TabIndex = 238;
            // 
            // LabelRfoutAPower
            // 
            this.LabelRfoutAPower.AutoSize = true;
            this.LabelRfoutAPower.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutAPower.Location = new System.Drawing.Point(1197, 80);
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
            this.LabelRfoutAFreq.Location = new System.Drawing.Point(1119, 26);
            this.LabelRfoutAFreq.Name = "LabelRfoutAFreq";
            this.LabelRfoutAFreq.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutAFreq.TabIndex = 227;
            this.LabelRfoutAFreq.Text = "Freq";
            this.LabelRfoutAFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelRfoutBPower
            // 
            this.LabelRfoutBPower.AutoSize = true;
            this.LabelRfoutBPower.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutBPower.Location = new System.Drawing.Point(1197, 292);
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
            this.LabelRfoutBFreq.Location = new System.Drawing.Point(1119, 342);
            this.LabelRfoutBFreq.Name = "LabelRfoutBFreq";
            this.LabelRfoutBFreq.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutBFreq.TabIndex = 227;
            this.LabelRfoutBFreq.Text = "Freq";
            this.LabelRfoutBFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboBoxRfoutBMux
            // 
            this.ComboBoxRfoutBMux.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComboBoxRfoutBMux.FormattingEnabled = true;
            this.ComboBoxRfoutBMux.Items.AddRange(new object[] {
            "Divider",
            "VCO",
            "SysRef"});
            this.ComboBoxRfoutBMux.Location = new System.Drawing.Point(1112, 308);
            this.ComboBoxRfoutBMux.Name = "ComboBoxRfoutBMux";
            this.ComboBoxRfoutBMux.Size = new System.Drawing.Size(62, 21);
            this.ComboBoxRfoutBMux.TabIndex = 197;
            this.ComboBoxRfoutBMux.Text = "Divider";
            // 
            // CheckBoxRfoutBEnable
            // 
            this.CheckBoxRfoutBEnable.AutoSize = true;
            this.CheckBoxRfoutBEnable.Location = new System.Drawing.Point(1193, 220);
            this.CheckBoxRfoutBEnable.Name = "CheckBoxRfoutBEnable";
            this.CheckBoxRfoutBEnable.Size = new System.Drawing.Size(61, 17);
            this.CheckBoxRfoutBEnable.TabIndex = 191;
            this.CheckBoxRfoutBEnable.Text = "Enable";
            this.CheckBoxRfoutBEnable.UseVisualStyleBackColor = true;
            // 
            // TextBoxRfoutBPower
            // 
            this.TextBoxRfoutBPower.Location = new System.Drawing.Point(1184, 308);
            this.TextBoxRfoutBPower.Name = "TextBoxRfoutBPower";
            this.TextBoxRfoutBPower.Size = new System.Drawing.Size(70, 22);
            this.TextBoxRfoutBPower.TabIndex = 192;
            this.TextBoxRfoutBPower.Text = "0";
            // 
            // LabelRfoutBMux
            // 
            this.LabelRfoutBMux.AutoSize = true;
            this.LabelRfoutBMux.Location = new System.Drawing.Point(1128, 292);
            this.LabelRfoutBMux.Name = "LabelRfoutBMux";
            this.LabelRfoutBMux.Size = new System.Drawing.Size(30, 13);
            this.LabelRfoutBMux.TabIndex = 201;
            this.LabelRfoutBMux.Text = "Path";
            this.LabelRfoutBMux.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxRfoutBFreq
            // 
            this.TextBoxRfoutBFreq.Location = new System.Drawing.Point(1155, 339);
            this.TextBoxRfoutBFreq.Name = "TextBoxRfoutBFreq";
            this.TextBoxRfoutBFreq.ReadOnly = true;
            this.TextBoxRfoutBFreq.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutBFreq.TabIndex = 226;
            // 
            // BtnLoadTICS
            // 
            this.BtnLoadTICS.Location = new System.Drawing.Point(19, 323);
            this.BtnLoadTICS.Name = "BtnLoadTICS";
            this.BtnLoadTICS.Size = new System.Drawing.Size(93, 23);
            this.BtnLoadTICS.TabIndex = 253;
            this.BtnLoadTICS.Text = "Load TICS";
            this.BtnLoadTICS.UseVisualStyleBackColor = true;
            this.BtnLoadTICS.Click += new System.EventHandler(this.BtnLoadTICS_Click);
            // 
            // BtnUpdateUI
            // 
            this.BtnUpdateUI.Location = new System.Drawing.Point(19, 381);
            this.BtnUpdateUI.Name = "BtnUpdateUI";
            this.BtnUpdateUI.Size = new System.Drawing.Size(93, 23);
            this.BtnUpdateUI.TabIndex = 252;
            this.BtnUpdateUI.Text = "Update UI";
            this.BtnUpdateUI.UseVisualStyleBackColor = true;
            this.BtnUpdateUI.Click += new System.EventHandler(this.BtnUpdateUI_Click);
            // 
            // BtnCommit
            // 
            this.BtnCommit.Location = new System.Drawing.Point(19, 352);
            this.BtnCommit.Name = "BtnCommit";
            this.BtnCommit.Size = new System.Drawing.Size(93, 23);
            this.BtnCommit.TabIndex = 251;
            this.BtnCommit.Text = "Commit";
            this.BtnCommit.UseVisualStyleBackColor = true;
            this.BtnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // LabelPhaseShift
            // 
            this.LabelPhaseShift.AutoSize = true;
            this.LabelPhaseShift.BackColor = System.Drawing.Color.Transparent;
            this.LabelPhaseShift.Location = new System.Drawing.Point(520, 365);
            this.LabelPhaseShift.Name = "LabelPhaseShift";
            this.LabelPhaseShift.Size = new System.Drawing.Size(64, 13);
            this.LabelPhaseShift.TabIndex = 256;
            this.LabelPhaseShift.Text = "Phase Shift";
            this.LabelPhaseShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxPhaseShift
            // 
            this.TextBoxPhaseShift.Location = new System.Drawing.Point(590, 362);
            this.TextBoxPhaseShift.Name = "TextBoxPhaseShift";
            this.TextBoxPhaseShift.ReadOnly = true;
            this.TextBoxPhaseShift.Size = new System.Drawing.Size(103, 22);
            this.TextBoxPhaseShift.TabIndex = 255;
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "OpenFileDialog";
            // 
            // LabelRfoutAPhaseShift
            // 
            this.LabelRfoutAPhaseShift.AutoSize = true;
            this.LabelRfoutAPhaseShift.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutAPhaseShift.Location = new System.Drawing.Point(1112, 54);
            this.LabelRfoutAPhaseShift.Name = "LabelRfoutAPhaseShift";
            this.LabelRfoutAPhaseShift.Size = new System.Drawing.Size(37, 13);
            this.LabelRfoutAPhaseShift.TabIndex = 261;
            this.LabelRfoutAPhaseShift.Text = "Phase";
            this.LabelRfoutAPhaseShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxRfoutAPhaseShift
            // 
            this.TextBoxRfoutAPhaseShift.Location = new System.Drawing.Point(1155, 51);
            this.TextBoxRfoutAPhaseShift.Name = "TextBoxRfoutAPhaseShift";
            this.TextBoxRfoutAPhaseShift.ReadOnly = true;
            this.TextBoxRfoutAPhaseShift.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutAPhaseShift.TabIndex = 260;
            // 
            // LabelRfoutBPhaseShift
            // 
            this.LabelRfoutBPhaseShift.AutoSize = true;
            this.LabelRfoutBPhaseShift.BackColor = System.Drawing.Color.Transparent;
            this.LabelRfoutBPhaseShift.Location = new System.Drawing.Point(1112, 370);
            this.LabelRfoutBPhaseShift.Name = "LabelRfoutBPhaseShift";
            this.LabelRfoutBPhaseShift.Size = new System.Drawing.Size(37, 13);
            this.LabelRfoutBPhaseShift.TabIndex = 259;
            this.LabelRfoutBPhaseShift.Text = "Phase";
            this.LabelRfoutBPhaseShift.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxRfoutBPhaseShift
            // 
            this.TextBoxRfoutBPhaseShift.Location = new System.Drawing.Point(1155, 367);
            this.TextBoxRfoutBPhaseShift.Name = "TextBoxRfoutBPhaseShift";
            this.TextBoxRfoutBPhaseShift.ReadOnly = true;
            this.TextBoxRfoutBPhaseShift.Size = new System.Drawing.Size(100, 22);
            this.TextBoxRfoutBPhaseShift.TabIndex = 258;
            // 
            // GroupBoxVcoCalibration
            // 
            this.GroupBoxVcoCalibration.Controls.Add(this.CheckBox_VCO_CAPCTRL_FORCE);
            this.GroupBoxVcoCalibration.Controls.Add(this.Label_StartAmplitude);
            this.GroupBoxVcoCalibration.Controls.Add(this.TextBox_VcoStartAmplitude);
            this.GroupBoxVcoCalibration.Controls.Add(this.BtnCalVco);
            this.GroupBoxVcoCalibration.Controls.Add(this.TextBox_FCAL_LPFD_ADJ);
            this.GroupBoxVcoCalibration.Controls.Add(this.Label_FCAL_LPFD_ADJ);
            this.GroupBoxVcoCalibration.Controls.Add(this.CheckBox_FCAL_EN);
            this.GroupBoxVcoCalibration.Controls.Add(this.TextBox_VCO_CAPCTRL);
            this.GroupBoxVcoCalibration.Controls.Add(this.TextBox_FCAL_HPFD_ADJ);
            this.GroupBoxVcoCalibration.Controls.Add(this.Label_FCAL_HPFD_ADJ);
            this.GroupBoxVcoCalibration.Controls.Add(this.CheckBox_QUICK_RECAL_EN);
            this.GroupBoxVcoCalibration.Location = new System.Drawing.Point(724, 213);
            this.GroupBoxVcoCalibration.Name = "GroupBoxVcoCalibration";
            this.GroupBoxVcoCalibration.Size = new System.Drawing.Size(240, 197);
            this.GroupBoxVcoCalibration.TabIndex = 257;
            this.GroupBoxVcoCalibration.TabStop = false;
            this.GroupBoxVcoCalibration.Text = "VCO Calibration";
            // 
            // CheckBox_VCO_CAPCTRL_FORCE
            // 
            this.CheckBox_VCO_CAPCTRL_FORCE.AutoSize = true;
            this.CheckBox_VCO_CAPCTRL_FORCE.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_VCO_CAPCTRL_FORCE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_VCO_CAPCTRL_FORCE.Location = new System.Drawing.Point(6, 135);
            this.CheckBox_VCO_CAPCTRL_FORCE.Name = "CheckBox_VCO_CAPCTRL_FORCE";
            this.CheckBox_VCO_CAPCTRL_FORCE.Size = new System.Drawing.Size(139, 17);
            this.CheckBox_VCO_CAPCTRL_FORCE.TabIndex = 248;
            this.CheckBox_VCO_CAPCTRL_FORCE.Text = "VCO_CAPCTRL_FORCE";
            this.CheckBox_VCO_CAPCTRL_FORCE.UseVisualStyleBackColor = false;
            this.CheckBox_VCO_CAPCTRL_FORCE.CheckedChanged += new System.EventHandler(this.CheckBox_VCO_CAPCTRL_FORCE_CheckedChanged);
            // 
            // Label_StartAmplitude
            // 
            this.Label_StartAmplitude.AutoSize = true;
            this.Label_StartAmplitude.BackColor = System.Drawing.Color.Transparent;
            this.Label_StartAmplitude.Enabled = false;
            this.Label_StartAmplitude.Location = new System.Drawing.Point(4, 107);
            this.Label_StartAmplitude.Name = "Label_StartAmplitude";
            this.Label_StartAmplitude.Size = new System.Drawing.Size(87, 13);
            this.Label_StartAmplitude.TabIndex = 245;
            this.Label_StartAmplitude.Text = "Start Amplitude";
            this.Label_StartAmplitude.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_VcoStartAmplitude
            // 
            this.TextBox_VcoStartAmplitude.Enabled = false;
            this.TextBox_VcoStartAmplitude.Location = new System.Drawing.Point(97, 104);
            this.TextBox_VcoStartAmplitude.Name = "TextBox_VcoStartAmplitude";
            this.TextBox_VcoStartAmplitude.Size = new System.Drawing.Size(137, 22);
            this.TextBox_VcoStartAmplitude.TabIndex = 244;
            // 
            // BtnCalVco
            // 
            this.BtnCalVco.Location = new System.Drawing.Point(26, 166);
            this.BtnCalVco.Name = "BtnCalVco";
            this.BtnCalVco.Size = new System.Drawing.Size(192, 22);
            this.BtnCalVco.TabIndex = 188;
            this.BtnCalVco.Text = "Calibrate VCO";
            this.BtnCalVco.UseVisualStyleBackColor = true;
            this.BtnCalVco.Click += new System.EventHandler(this.BtnCalVco_Click);
            // 
            // TextBox_FCAL_LPFD_ADJ
            // 
            this.TextBox_FCAL_LPFD_ADJ.Location = new System.Drawing.Point(98, 48);
            this.TextBox_FCAL_LPFD_ADJ.Name = "TextBox_FCAL_LPFD_ADJ";
            this.TextBox_FCAL_LPFD_ADJ.ReadOnly = true;
            this.TextBox_FCAL_LPFD_ADJ.Size = new System.Drawing.Size(136, 22);
            this.TextBox_FCAL_LPFD_ADJ.TabIndex = 234;
            // 
            // Label_FCAL_LPFD_ADJ
            // 
            this.Label_FCAL_LPFD_ADJ.AutoSize = true;
            this.Label_FCAL_LPFD_ADJ.BackColor = System.Drawing.Color.Transparent;
            this.Label_FCAL_LPFD_ADJ.Location = new System.Drawing.Point(6, 51);
            this.Label_FCAL_LPFD_ADJ.Name = "Label_FCAL_LPFD_ADJ";
            this.Label_FCAL_LPFD_ADJ.Size = new System.Drawing.Size(86, 13);
            this.Label_FCAL_LPFD_ADJ.TabIndex = 235;
            this.Label_FCAL_LPFD_ADJ.Text = "FCAL_LPFD_ADJ";
            this.Label_FCAL_LPFD_ADJ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBox_FCAL_EN
            // 
            this.CheckBox_FCAL_EN.AutoSize = true;
            this.CheckBox_FCAL_EN.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_FCAL_EN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_FCAL_EN.Location = new System.Drawing.Point(24, 25);
            this.CheckBox_FCAL_EN.Name = "CheckBox_FCAL_EN";
            this.CheckBox_FCAL_EN.Size = new System.Drawing.Size(70, 17);
            this.CheckBox_FCAL_EN.TabIndex = 243;
            this.CheckBox_FCAL_EN.Text = "FCAL_EN";
            this.CheckBox_FCAL_EN.UseVisualStyleBackColor = false;
            // 
            // TextBox_VCO_CAPCTRL
            // 
            this.TextBox_VCO_CAPCTRL.Location = new System.Drawing.Point(151, 132);
            this.TextBox_VCO_CAPCTRL.Name = "TextBox_VCO_CAPCTRL";
            this.TextBox_VCO_CAPCTRL.ReadOnly = true;
            this.TextBox_VCO_CAPCTRL.Size = new System.Drawing.Size(83, 22);
            this.TextBox_VCO_CAPCTRL.TabIndex = 241;
            // 
            // TextBox_FCAL_HPFD_ADJ
            // 
            this.TextBox_FCAL_HPFD_ADJ.Location = new System.Drawing.Point(98, 76);
            this.TextBox_FCAL_HPFD_ADJ.Name = "TextBox_FCAL_HPFD_ADJ";
            this.TextBox_FCAL_HPFD_ADJ.ReadOnly = true;
            this.TextBox_FCAL_HPFD_ADJ.Size = new System.Drawing.Size(136, 22);
            this.TextBox_FCAL_HPFD_ADJ.TabIndex = 236;
            // 
            // Label_FCAL_HPFD_ADJ
            // 
            this.Label_FCAL_HPFD_ADJ.AutoSize = true;
            this.Label_FCAL_HPFD_ADJ.BackColor = System.Drawing.Color.Transparent;
            this.Label_FCAL_HPFD_ADJ.Location = new System.Drawing.Point(3, 79);
            this.Label_FCAL_HPFD_ADJ.Name = "Label_FCAL_HPFD_ADJ";
            this.Label_FCAL_HPFD_ADJ.Size = new System.Drawing.Size(89, 13);
            this.Label_FCAL_HPFD_ADJ.TabIndex = 237;
            this.Label_FCAL_HPFD_ADJ.Text = "FCAL_HPFD_ADJ";
            this.Label_FCAL_HPFD_ADJ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBox_QUICK_RECAL_EN
            // 
            this.CheckBox_QUICK_RECAL_EN.AutoSize = true;
            this.CheckBox_QUICK_RECAL_EN.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_QUICK_RECAL_EN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CheckBox_QUICK_RECAL_EN.Location = new System.Drawing.Point(100, 25);
            this.CheckBox_QUICK_RECAL_EN.Name = "CheckBox_QUICK_RECAL_EN";
            this.CheckBox_QUICK_RECAL_EN.Size = new System.Drawing.Size(114, 17);
            this.CheckBox_QUICK_RECAL_EN.TabIndex = 240;
            this.CheckBox_QUICK_RECAL_EN.Text = "QUICK_RECAL_EN";
            this.CheckBox_QUICK_RECAL_EN.UseVisualStyleBackColor = false;
            // 
            // GroupBoxPhaseSync
            // 
            this.GroupBoxPhaseSync.Controls.Add(this.CheckBoxPsyncPin);
            this.GroupBoxPhaseSync.Controls.Add(this.LabelMashResetDelay);
            this.GroupBoxPhaseSync.Controls.Add(this.TextBoxMashResetDelay);
            this.GroupBoxPhaseSync.Controls.Add(this.TextBoxMashResetCounter);
            this.GroupBoxPhaseSync.Controls.Add(this.CheckBoxSyncEnable);
            this.GroupBoxPhaseSync.Controls.Add(this.Label_Mash_Reset_Counter);
            this.GroupBoxPhaseSync.Location = new System.Drawing.Point(225, 310);
            this.GroupBoxPhaseSync.Name = "GroupBoxPhaseSync";
            this.GroupBoxPhaseSync.Size = new System.Drawing.Size(250, 100);
            this.GroupBoxPhaseSync.TabIndex = 262;
            this.GroupBoxPhaseSync.TabStop = false;
            this.GroupBoxPhaseSync.Text = "Phase Sync";
            // 
            // CheckBoxPsyncPin
            // 
            this.CheckBoxPsyncPin.AutoSize = true;
            this.CheckBoxPsyncPin.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxPsyncPin.Location = new System.Drawing.Point(141, 21);
            this.CheckBoxPsyncPin.Name = "CheckBoxPsyncPin";
            this.CheckBoxPsyncPin.Size = new System.Drawing.Size(80, 17);
            this.CheckBoxPsyncPin.TabIndex = 247;
            this.CheckBoxPsyncPin.Text = "Sync Input";
            this.CheckBoxPsyncPin.UseVisualStyleBackColor = false;
            // 
            // LabelMashResetDelay
            // 
            this.LabelMashResetDelay.AutoSize = true;
            this.LabelMashResetDelay.BackColor = System.Drawing.Color.Transparent;
            this.LabelMashResetDelay.Location = new System.Drawing.Point(60, 75);
            this.LabelMashResetDelay.Name = "LabelMashResetDelay";
            this.LabelMashResetDelay.Size = new System.Drawing.Size(66, 13);
            this.LabelMashResetDelay.TabIndex = 246;
            this.LabelMashResetDelay.Text = "Reset Delay";
            this.LabelMashResetDelay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoxMashResetDelay
            // 
            this.TextBoxMashResetDelay.Location = new System.Drawing.Point(132, 72);
            this.TextBoxMashResetDelay.Name = "TextBoxMashResetDelay";
            this.TextBoxMashResetDelay.ReadOnly = true;
            this.TextBoxMashResetDelay.Size = new System.Drawing.Size(103, 22);
            this.TextBoxMashResetDelay.TabIndex = 245;
            // 
            // TextBoxMashResetCounter
            // 
            this.TextBoxMashResetCounter.Location = new System.Drawing.Point(132, 44);
            this.TextBoxMashResetCounter.Name = "TextBoxMashResetCounter";
            this.TextBoxMashResetCounter.Size = new System.Drawing.Size(103, 22);
            this.TextBoxMashResetCounter.TabIndex = 205;
            // 
            // CheckBoxSyncEnable
            // 
            this.CheckBoxSyncEnable.AutoSize = true;
            this.CheckBoxSyncEnable.BackColor = System.Drawing.Color.Transparent;
            this.CheckBoxSyncEnable.Location = new System.Drawing.Point(29, 21);
            this.CheckBoxSyncEnable.Name = "CheckBoxSyncEnable";
            this.CheckBoxSyncEnable.Size = new System.Drawing.Size(82, 17);
            this.CheckBoxSyncEnable.TabIndex = 233;
            this.CheckBoxSyncEnable.Text = "Phase Sync";
            this.CheckBoxSyncEnable.UseVisualStyleBackColor = false;
            // 
            // Label_Mash_Reset_Counter
            // 
            this.Label_Mash_Reset_Counter.AutoSize = true;
            this.Label_Mash_Reset_Counter.BackColor = System.Drawing.Color.Transparent;
            this.Label_Mash_Reset_Counter.Location = new System.Drawing.Point(46, 47);
            this.Label_Mash_Reset_Counter.Name = "Label_Mash_Reset_Counter";
            this.Label_Mash_Reset_Counter.Size = new System.Drawing.Size(80, 13);
            this.Label_Mash_Reset_Counter.TabIndex = 207;
            this.Label_Mash_Reset_Counter.Text = "Reset Counter";
            this.Label_Mash_Reset_Counter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LMX2572_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Nitride.EE.Properties.Resources.LMX2572_BD;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.GroupBoxPhaseSync);
            this.Controls.Add(this.Label_CAL_CLK_DIV);
            this.Controls.Add(this.LabelRfoutAPhaseShift);
            this.Controls.Add(this.TextBox_CAL_CLK_DIV);
            this.Controls.Add(this.TextBoxRfoutAPhaseShift);
            this.Controls.Add(this.LabelRfoutBPhaseShift);
            this.Controls.Add(this.TextBoxRfoutBPhaseShift);
            this.Controls.Add(this.GroupBoxVcoCalibration);
            this.Controls.Add(this.LabelRfoutAFreq);
            this.Controls.Add(this.LabelRfoutAPower);
            this.Controls.Add(this.TextBoxRfoutAFreq);
            this.Controls.Add(this.LabelPhaseShift);
            this.Controls.Add(this.TextBoxPhaseShift);
            this.Controls.Add(this.TextBoxRfoutAPower);
            this.Controls.Add(this.ComboBoxRfoutAMux);
            this.Controls.Add(this.LabelRfoutAMux);
            this.Controls.Add(this.LabelRfoutBFreq);
            this.Controls.Add(this.LabelRfoutBPower);
            this.Controls.Add(this.TextBoxRfoutBFreq);
            this.Controls.Add(this.BtnLoadTICS);
            this.Controls.Add(this.BtnUpdateUI);
            this.Controls.Add(this.TextBoxRfoutBPower);
            this.Controls.Add(this.ComboBoxRfoutBMux);
            this.Controls.Add(this.BtnCommit);
            this.Controls.Add(this.LabelRfoutBMux);
            this.Controls.Add(this.CheckBoxRfoutAEnable);
            this.Controls.Add(this.CheckBoxRfoutBEnable);
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
            this.Controls.Add(this.TextBoxPfdDlySel);
            this.Controls.Add(this.Label_PFD_DLY_SEL);
            this.Controls.Add(this.TextBoxMashSeed);
            this.Controls.Add(this.Label_MashSeed);
            this.Controls.Add(this.Label_Mash_Order);
            this.Controls.Add(this.ComboBoxMashOrder);
            this.Controls.Add(this.TextBoxFDen);
            this.Controls.Add(this.TextBoxFNum);
            this.Controls.Add(this.TextBoxNDiv);
            this.Controls.Add(this.TextBoxRDiv);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name = "LMX2572_Control";
            this.Size = new System.Drawing.Size(1272, 419);
            this.GroupBoxVcoCalibration.ResumeLayout(false);
            this.GroupBoxVcoCalibration.PerformLayout();
            this.GroupBoxPhaseSync.ResumeLayout(false);
            this.GroupBoxPhaseSync.PerformLayout();
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

        private System.Windows.Forms.CheckBox CheckBoxMashResetN;
        private System.Windows.Forms.CheckBox CheckBoxMashSeedEnable;
        private System.Windows.Forms.TextBox TextBoxPfdDlySel;
        private System.Windows.Forms.Label Label_PFD_DLY_SEL;
        private System.Windows.Forms.Label Label_MashSeed;
        private System.Windows.Forms.TextBox TextBoxMashSeed;
        private System.Windows.Forms.Label Label_Mash_Order;
        private System.Windows.Forms.ComboBox ComboBoxMashOrder;

        private System.Windows.Forms.Label LabelChDivider;
        private System.Windows.Forms.ComboBox ComboBoxChDivider;
        private System.Windows.Forms.Label LabelRfoutAMux;
        private System.Windows.Forms.ComboBox ComboBoxRfoutAMux;
        private System.Windows.Forms.Label LabelRfoutAPower;
        private System.Windows.Forms.TextBox TextBoxRfoutAPower;
        private System.Windows.Forms.Label LabelRfoutAFreq;
        private System.Windows.Forms.TextBox TextBoxRfoutAFreq;
        private System.Windows.Forms.CheckBox CheckBoxRfoutAEnable;
        private System.Windows.Forms.Label LabelRfoutBMux;
        private System.Windows.Forms.ComboBox ComboBoxRfoutBMux;
        private System.Windows.Forms.Label LabelRfoutBPower;
        private System.Windows.Forms.TextBox TextBoxRfoutBPower;
        private System.Windows.Forms.Label LabelRfoutBFreq;
        private System.Windows.Forms.TextBox TextBoxRfoutBFreq;
        private System.Windows.Forms.CheckBox CheckBoxRfoutBEnable;
        private System.Windows.Forms.Label Label_CAL_CLK_DIV;
        private System.Windows.Forms.TextBox TextBox_CAL_CLK_DIV;
        private System.Windows.Forms.Button BtnLoadTICS;
        private System.Windows.Forms.Button BtnUpdateUI;
        private System.Windows.Forms.Button BtnCommit;
        private System.Windows.Forms.Label LabelPhaseShift;
        private System.Windows.Forms.TextBox TextBoxPhaseShift;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Label LabelRfoutAPhaseShift;
        private System.Windows.Forms.TextBox TextBoxRfoutAPhaseShift;
        private System.Windows.Forms.Label LabelRfoutBPhaseShift;
        private System.Windows.Forms.TextBox TextBoxRfoutBPhaseShift;
        private System.Windows.Forms.GroupBox GroupBoxVcoCalibration;
        private System.Windows.Forms.CheckBox CheckBox_VCO_CAPCTRL_FORCE;
        private System.Windows.Forms.Label Label_StartAmplitude;
        private System.Windows.Forms.TextBox TextBox_VcoStartAmplitude;
        private System.Windows.Forms.Button BtnCalVco;
        private System.Windows.Forms.TextBox TextBox_FCAL_LPFD_ADJ;
        private System.Windows.Forms.Label Label_FCAL_LPFD_ADJ;
        private System.Windows.Forms.CheckBox CheckBox_FCAL_EN;
        private System.Windows.Forms.TextBox TextBox_VCO_CAPCTRL;
        private System.Windows.Forms.TextBox TextBox_FCAL_HPFD_ADJ;
        private System.Windows.Forms.Label Label_FCAL_HPFD_ADJ;
        private System.Windows.Forms.CheckBox CheckBox_QUICK_RECAL_EN;
        private System.Windows.Forms.GroupBox GroupBoxPhaseSync;
        private System.Windows.Forms.CheckBox CheckBoxPsyncPin;
        private System.Windows.Forms.Label LabelMashResetDelay;
        private System.Windows.Forms.TextBox TextBoxMashResetDelay;
        private System.Windows.Forms.TextBox TextBoxMashResetCounter;
        private System.Windows.Forms.CheckBox CheckBoxSyncEnable;
        private System.Windows.Forms.Label Label_Mash_Reset_Counter;
    }
}
