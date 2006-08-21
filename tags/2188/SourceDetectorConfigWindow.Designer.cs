namespace MeGUI
{
    partial class SourceDetectorConfigWindow
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
            this.analysisPercentLabel = new System.Windows.Forms.Label();
            this.analysisPercent = new System.Windows.Forms.NumericUpDown();
            this.minAnalyseSectionsLabel = new System.Windows.Forms.Label();
            this.minAnalyseSections = new System.Windows.Forms.NumericUpDown();
            this.hybridThresholdLabel = new System.Windows.Forms.Label();
            this.hybridThreshold = new System.Windows.Forms.NumericUpDown();
            this.hybridFOThresholdLabel = new System.Windows.Forms.Label();
            this.hybridFOThreshold = new System.Windows.Forms.NumericUpDown();
            this.portionThresholdLabel = new System.Windows.Forms.Label();
            this.portionThreshold = new System.Windows.Forms.NumericUpDown();
            this.maximumPortionsLabel = new System.Windows.Forms.Label();
            this.maximumPortions = new System.Windows.Forms.NumericUpDown();
            this.portionsAllowed = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.priority = new System.Windows.Forms.ComboBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.analysisPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAnalyseSections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hybridThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hybridFOThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portionThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumPortions)).BeginInit();
            this.SuspendLayout();
            // 
            // analysisPercentLabel
            // 
            this.analysisPercentLabel.AutoSize = true;
            this.analysisPercentLabel.Location = new System.Drawing.Point(12, 9);
            this.analysisPercentLabel.Name = "analysisPercentLabel";
            this.analysisPercentLabel.Size = new System.Drawing.Size(88, 13);
            this.analysisPercentLabel.TabIndex = 0;
            this.analysisPercentLabel.Text = "Analysis Percent:";
            // 
            // analysisPercent
            // 
            this.analysisPercent.Location = new System.Drawing.Point(213, 7);
            this.analysisPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.analysisPercent.Name = "analysisPercent";
            this.analysisPercent.Size = new System.Drawing.Size(120, 20);
            this.analysisPercent.TabIndex = 1;
            this.analysisPercent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // minAnalyseSectionsLabel
            // 
            this.minAnalyseSectionsLabel.AutoSize = true;
            this.minAnalyseSectionsLabel.Location = new System.Drawing.Point(12, 35);
            this.minAnalyseSectionsLabel.Name = "minAnalyseSectionsLabel";
            this.minAnalyseSectionsLabel.Size = new System.Drawing.Size(133, 13);
            this.minAnalyseSectionsLabel.TabIndex = 0;
            this.minAnalyseSectionsLabel.Text = "Minimum analysis sections:";
            // 
            // minAnalyseSections
            // 
            this.minAnalyseSections.Location = new System.Drawing.Point(213, 33);
            this.minAnalyseSections.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minAnalyseSections.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minAnalyseSections.Name = "minAnalyseSections";
            this.minAnalyseSections.Size = new System.Drawing.Size(120, 20);
            this.minAnalyseSections.TabIndex = 1;
            this.minAnalyseSections.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            // 
            // hybridThresholdLabel
            // 
            this.hybridThresholdLabel.AutoSize = true;
            this.hybridThresholdLabel.Location = new System.Drawing.Point(12, 61);
            this.hybridThresholdLabel.Name = "hybridThresholdLabel";
            this.hybridThresholdLabel.Size = new System.Drawing.Size(107, 13);
            this.hybridThresholdLabel.TabIndex = 0;
            this.hybridThresholdLabel.Text = "Hybrid Threshold (%):";
            // 
            // hybridThreshold
            // 
            this.hybridThreshold.Location = new System.Drawing.Point(213, 59);
            this.hybridThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hybridThreshold.Name = "hybridThreshold";
            this.hybridThreshold.Size = new System.Drawing.Size(120, 20);
            this.hybridThreshold.TabIndex = 1;
            this.hybridThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // hybridFOThresholdLabel
            // 
            this.hybridFOThresholdLabel.AutoSize = true;
            this.hybridFOThresholdLabel.Location = new System.Drawing.Point(12, 87);
            this.hybridFOThresholdLabel.Name = "hybridFOThresholdLabel";
            this.hybridFOThresholdLabel.Size = new System.Drawing.Size(161, 13);
            this.hybridFOThresholdLabel.TabIndex = 0;
            this.hybridFOThresholdLabel.Text = "Hybrid Field Order Threshold (%):";
            // 
            // hybridFOThreshold
            // 
            this.hybridFOThreshold.Location = new System.Drawing.Point(213, 85);
            this.hybridFOThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.hybridFOThreshold.Name = "hybridFOThreshold";
            this.hybridFOThreshold.Size = new System.Drawing.Size(120, 20);
            this.hybridFOThreshold.TabIndex = 1;
            this.hybridFOThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // portionThresholdLabel
            // 
            this.portionThresholdLabel.AutoSize = true;
            this.portionThresholdLabel.Location = new System.Drawing.Point(12, 170);
            this.portionThresholdLabel.Name = "portionThresholdLabel";
            this.portionThresholdLabel.Size = new System.Drawing.Size(93, 13);
            this.portionThresholdLabel.TabIndex = 0;
            this.portionThresholdLabel.Text = "Portion Threshold:";
            // 
            // portionThreshold
            // 
            this.portionThreshold.DecimalPlaces = 1;
            this.portionThreshold.Enabled = false;
            this.portionThreshold.Location = new System.Drawing.Point(213, 168);
            this.portionThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portionThreshold.Name = "portionThreshold";
            this.portionThreshold.Size = new System.Drawing.Size(120, 20);
            this.portionThreshold.TabIndex = 1;
            this.portionThreshold.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // maximumPortionsLabel
            // 
            this.maximumPortionsLabel.AutoSize = true;
            this.maximumPortionsLabel.Location = new System.Drawing.Point(12, 196);
            this.maximumPortionsLabel.Name = "maximumPortionsLabel";
            this.maximumPortionsLabel.Size = new System.Drawing.Size(147, 13);
            this.maximumPortionsLabel.TabIndex = 0;
            this.maximumPortionsLabel.Text = "Maximum Number of Portions:";
            // 
            // maximumPortions
            // 
            this.maximumPortions.Enabled = false;
            this.maximumPortions.Location = new System.Drawing.Point(213, 194);
            this.maximumPortions.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maximumPortions.Name = "maximumPortions";
            this.maximumPortions.Size = new System.Drawing.Size(120, 20);
            this.maximumPortions.TabIndex = 1;
            this.maximumPortions.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // portionsAllowed
            // 
            this.portionsAllowed.AutoSize = true;
            this.portionsAllowed.Location = new System.Drawing.Point(15, 150);
            this.portionsAllowed.Name = "portionsAllowed";
            this.portionsAllowed.Size = new System.Drawing.Size(104, 17);
            this.portionsAllowed.TabIndex = 2;
            this.portionsAllowed.Text = "Portions Allowed";
            this.portionsAllowed.UseVisualStyleBackColor = true;
            this.portionsAllowed.CheckedChanged += new System.EventHandler(this.portionsAllowed_CheckedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(264, 229);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(69, 24);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(189, 229);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(69, 24);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // priority
            // 
            this.priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priority.Items.AddRange(new object[] {
            "Lowest",
            "Below Normal",
            "Normal",
            "Above Normal",
            "Highest"});
            this.priority.Location = new System.Drawing.Point(213, 111);
            this.priority.Name = "priority";
            this.priority.Size = new System.Drawing.Size(120, 21);
            this.priority.TabIndex = 15;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(12, 111);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(88, 23);
            this.priorityLabel.TabIndex = 14;
            this.priorityLabel.Text = "Process Priority";
            this.priorityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SourceDetectorConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 265);
            this.Controls.Add(this.priority);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.portionsAllowed);
            this.Controls.Add(this.maximumPortions);
            this.Controls.Add(this.maximumPortionsLabel);
            this.Controls.Add(this.portionThreshold);
            this.Controls.Add(this.portionThresholdLabel);
            this.Controls.Add(this.hybridFOThreshold);
            this.Controls.Add(this.hybridFOThresholdLabel);
            this.Controls.Add(this.hybridThreshold);
            this.Controls.Add(this.hybridThresholdLabel);
            this.Controls.Add(this.minAnalyseSections);
            this.Controls.Add(this.minAnalyseSectionsLabel);
            this.Controls.Add(this.analysisPercent);
            this.Controls.Add(this.analysisPercentLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SourceDetectorConfigWindow";
            this.Text = "Source Detector Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.analysisPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAnalyseSections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hybridThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hybridFOThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portionThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maximumPortions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label analysisPercentLabel;
        private System.Windows.Forms.NumericUpDown analysisPercent;
        private System.Windows.Forms.Label minAnalyseSectionsLabel;
        private System.Windows.Forms.NumericUpDown minAnalyseSections;
        private System.Windows.Forms.Label hybridThresholdLabel;
        private System.Windows.Forms.NumericUpDown hybridThreshold;
        private System.Windows.Forms.Label hybridFOThresholdLabel;
        private System.Windows.Forms.NumericUpDown hybridFOThreshold;
        private System.Windows.Forms.Label portionThresholdLabel;
        private System.Windows.Forms.NumericUpDown portionThreshold;
        private System.Windows.Forms.Label maximumPortionsLabel;
        private System.Windows.Forms.NumericUpDown maximumPortions;
        private System.Windows.Forms.CheckBox portionsAllowed;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox priority;
        private System.Windows.Forms.Label priorityLabel;
    }
}