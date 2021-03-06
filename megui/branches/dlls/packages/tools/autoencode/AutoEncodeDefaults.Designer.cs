namespace MeGUI
{
    partial class AutoEncodeDefaults
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
            this.AutomaticEncodingGroup = new System.Windows.Forms.GroupBox();
            this.noTargetRadio = new System.Windows.Forms.RadioButton();
            this.sizeSelection = new System.Windows.Forms.ComboBox();
            this.StorageMediumLabel = new System.Windows.Forms.Label();
            this.averageBitrateRadio = new System.Windows.Forms.RadioButton();
            this.FileSizeRadio = new System.Windows.Forms.RadioButton();
            this.projectedBitrateKBits = new System.Windows.Forms.TextBox();
            this.AverageBitrateLabel = new System.Windows.Forms.Label();
            this.muxedSizeMBs = new System.Windows.Forms.TextBox();
            this.FileSizeLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.container = new System.Windows.Forms.ComboBox();
            this.splitOutput = new System.Windows.Forms.CheckBox();
            this.containerLabel = new System.Windows.Forms.Label();
            this.SplitSizeLabel = new System.Windows.Forms.Label();
            this.splitSize = new System.Windows.Forms.TextBox();
            this.addSubsNChapters = new System.Windows.Forms.CheckBox();
            this.AutomaticEncodingGroup.SuspendLayout();
            this.OutputGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AutomaticEncodingGroup
            // 
            this.AutomaticEncodingGroup.Controls.Add(this.noTargetRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.sizeSelection);
            this.AutomaticEncodingGroup.Controls.Add(this.StorageMediumLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.averageBitrateRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeRadio);
            this.AutomaticEncodingGroup.Controls.Add(this.projectedBitrateKBits);
            this.AutomaticEncodingGroup.Controls.Add(this.AverageBitrateLabel);
            this.AutomaticEncodingGroup.Controls.Add(this.muxedSizeMBs);
            this.AutomaticEncodingGroup.Controls.Add(this.FileSizeLabel);
            this.AutomaticEncodingGroup.Location = new System.Drawing.Point(3, 80);
            this.AutomaticEncodingGroup.Name = "AutomaticEncodingGroup";
            this.AutomaticEncodingGroup.Size = new System.Drawing.Size(424, 104);
            this.AutomaticEncodingGroup.TabIndex = 18;
            this.AutomaticEncodingGroup.TabStop = false;
            this.AutomaticEncodingGroup.Text = "Size and Bitrate";
            // 
            // noTargetRadio
            // 
            this.noTargetRadio.Location = new System.Drawing.Point(16, 72);
            this.noTargetRadio.Name = "noTargetRadio";
            this.noTargetRadio.Size = new System.Drawing.Size(208, 18);
            this.noTargetRadio.TabIndex = 22;
            this.noTargetRadio.TabStop = true;
            this.noTargetRadio.Text = "No Target Size (use profile settings)";
            this.noTargetRadio.UseVisualStyleBackColor = true;
            this.noTargetRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // sizeSelection
            // 
            this.sizeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeSelection.Location = new System.Drawing.Point(308, 16);
            this.sizeSelection.Name = "sizeSelection";
            this.sizeSelection.Size = new System.Drawing.Size(64, 21);
            this.sizeSelection.TabIndex = 0;
            this.sizeSelection.SelectedIndexChanged += new System.EventHandler(this.sizeSelection_SelectedIndexChanged);
            // 
            // StorageMediumLabel
            // 
            this.StorageMediumLabel.Location = new System.Drawing.Point(222, 20);
            this.StorageMediumLabel.Name = "StorageMediumLabel";
            this.StorageMediumLabel.Size = new System.Drawing.Size(90, 18);
            this.StorageMediumLabel.TabIndex = 17;
            this.StorageMediumLabel.Text = "Storage Medium";
            // 
            // averageBitrateRadio
            // 
            this.averageBitrateRadio.Location = new System.Drawing.Point(16, 47);
            this.averageBitrateRadio.Name = "averageBitrateRadio";
            this.averageBitrateRadio.Size = new System.Drawing.Size(100, 18);
            this.averageBitrateRadio.TabIndex = 16;
            this.averageBitrateRadio.Text = "Average Bitrate";
            this.averageBitrateRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // FileSizeRadio
            // 
            this.FileSizeRadio.Checked = true;
            this.FileSizeRadio.Location = new System.Drawing.Point(16, 20);
            this.FileSizeRadio.Name = "FileSizeRadio";
            this.FileSizeRadio.Size = new System.Drawing.Size(100, 18);
            this.FileSizeRadio.TabIndex = 15;
            this.FileSizeRadio.TabStop = true;
            this.FileSizeRadio.Text = "File Size";
            this.FileSizeRadio.CheckedChanged += new System.EventHandler(this.FileSizeRadio_CheckedChanged);
            // 
            // projectedBitrateKBits
            // 
            this.projectedBitrateKBits.Enabled = false;
            this.projectedBitrateKBits.Location = new System.Drawing.Point(118, 44);
            this.projectedBitrateKBits.Name = "projectedBitrateKBits";
            this.projectedBitrateKBits.Size = new System.Drawing.Size(64, 21);
            this.projectedBitrateKBits.TabIndex = 9;
            this.projectedBitrateKBits.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // AverageBitrateLabel
            // 
            this.AverageBitrateLabel.Location = new System.Drawing.Point(187, 47);
            this.AverageBitrateLabel.Name = "AverageBitrateLabel";
            this.AverageBitrateLabel.Size = new System.Drawing.Size(37, 23);
            this.AverageBitrateLabel.TabIndex = 10;
            this.AverageBitrateLabel.Text = "kbit/s";
            // 
            // muxedSizeMBs
            // 
            this.muxedSizeMBs.Location = new System.Drawing.Point(118, 16);
            this.muxedSizeMBs.Name = "muxedSizeMBs";
            this.muxedSizeMBs.Size = new System.Drawing.Size(64, 21);
            this.muxedSizeMBs.TabIndex = 1;
            this.muxedSizeMBs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // FileSizeLabel
            // 
            this.FileSizeLabel.Location = new System.Drawing.Point(187, 20);
            this.FileSizeLabel.Name = "FileSizeLabel";
            this.FileSizeLabel.Size = new System.Drawing.Size(24, 13);
            this.FileSizeLabel.TabIndex = 4;
            this.FileSizeLabel.Text = "MB";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(379, 188);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(48, 23);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(311, 188);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(48, 23);
            this.saveButton.TabIndex = 19;
            this.saveButton.Text = "Save";
            // 
            // OutputGroupBox
            // 
            this.OutputGroupBox.Controls.Add(this.container);
            this.OutputGroupBox.Controls.Add(this.splitOutput);
            this.OutputGroupBox.Controls.Add(this.containerLabel);
            this.OutputGroupBox.Controls.Add(this.SplitSizeLabel);
            this.OutputGroupBox.Controls.Add(this.splitSize);
            this.OutputGroupBox.Location = new System.Drawing.Point(3, 12);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(424, 62);
            this.OutputGroupBox.TabIndex = 21;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output Options";
            // 
            // container
            // 
            this.container.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.container.FormattingEnabled = true;
            this.container.Location = new System.Drawing.Point(118, 20);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(64, 21);
            this.container.TabIndex = 25;
            // 
            // splitOutput
            // 
            this.splitOutput.Location = new System.Drawing.Point(192, 21);
            this.splitOutput.Name = "splitOutput";
            this.splitOutput.Size = new System.Drawing.Size(97, 18);
            this.splitOutput.TabIndex = 21;
            this.splitOutput.Text = "Split Output";
            this.splitOutput.CheckedChanged += new System.EventHandler(this.splitOutput_CheckedChanged);
            // 
            // containerLabel
            // 
            this.containerLabel.AutoSize = true;
            this.containerLabel.Location = new System.Drawing.Point(6, 23);
            this.containerLabel.Name = "containerLabel";
            this.containerLabel.Size = new System.Drawing.Size(54, 13);
            this.containerLabel.TabIndex = 24;
            this.containerLabel.Text = "Container";
            // 
            // SplitSizeLabel
            // 
            this.SplitSizeLabel.Location = new System.Drawing.Point(381, 23);
            this.SplitSizeLabel.Name = "SplitSizeLabel";
            this.SplitSizeLabel.Size = new System.Drawing.Size(32, 16);
            this.SplitSizeLabel.TabIndex = 14;
            this.SplitSizeLabel.Text = "MB";
            // 
            // splitSize
            // 
            this.splitSize.Enabled = false;
            this.splitSize.Location = new System.Drawing.Point(308, 20);
            this.splitSize.Name = "splitSize";
            this.splitSize.Size = new System.Drawing.Size(64, 21);
            this.splitSize.TabIndex = 13;
            this.splitSize.Text = "0";
            this.splitSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textField_KeyPress);
            // 
            // addSubsNChapters
            // 
            this.addSubsNChapters.Location = new System.Drawing.Point(12, 187);
            this.addSubsNChapters.Name = "addSubsNChapters";
            this.addSubsNChapters.Size = new System.Drawing.Size(256, 24);
            this.addSubsNChapters.TabIndex = 22;
            this.addSubsNChapters.Text = "Add additional content (audio, subs, chapters)";
            // 
            // AutoEncodeDefaults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(433, 214);
            this.Controls.Add(this.addSubsNChapters);
            this.Controls.Add(this.OutputGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.AutomaticEncodingGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoEncodeDefaults";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Auto Encode Defaults";
            this.AutomaticEncodingGroup.ResumeLayout(false);
            this.AutomaticEncodingGroup.PerformLayout();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox AutomaticEncodingGroup;
        private System.Windows.Forms.RadioButton noTargetRadio;
        private System.Windows.Forms.ComboBox sizeSelection;
        private System.Windows.Forms.Label StorageMediumLabel;
        private System.Windows.Forms.RadioButton averageBitrateRadio;
        private System.Windows.Forms.RadioButton FileSizeRadio;
        private System.Windows.Forms.TextBox projectedBitrateKBits;
        private System.Windows.Forms.Label AverageBitrateLabel;
        private System.Windows.Forms.TextBox muxedSizeMBs;
        private System.Windows.Forms.Label FileSizeLabel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox OutputGroupBox;
        private System.Windows.Forms.ComboBox container;
        private System.Windows.Forms.CheckBox splitOutput;
        private System.Windows.Forms.Label containerLabel;
        private System.Windows.Forms.Label SplitSizeLabel;
        private System.Windows.Forms.TextBox splitSize;
        private System.Windows.Forms.CheckBox addSubsNChapters;
    }
}