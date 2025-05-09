namespace sachssoft.Drawing.Colors.Samples
{
    partial class ColorBlenderForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            ImageSelector = new ComboBox();
            label2 = new Label();
            BlenderSelector = new ComboBox();
            label3 = new Label();
            AmountSlider = new TrackBar();
            ImageViewer = new PictureBox();
            AmountValue = new Label();
            BlendModeColor = new RadioButton();
            BlendModeImage = new RadioButton();
            BlendImageSelector = new ComboBox();
            ColorButton = new Button();
            ((System.ComponentModel.ISupportInitialize)AmountSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ImageViewer).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(12, 14);
            label1.Name = "label1";
            label1.Size = new Size(150, 25);
            label1.TabIndex = 0;
            label1.Text = "Image:";
            // 
            // ImageSelector
            // 
            ImageSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            ImageSelector.FormattingEnabled = true;
            ImageSelector.Location = new Point(168, 6);
            ImageSelector.Name = "ImageSelector";
            ImageSelector.Size = new Size(250, 33);
            ImageSelector.TabIndex = 1;
            // 
            // label2
            // 
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(150, 25);
            label2.TabIndex = 2;
            label2.Text = "Blender:";
            // 
            // BlenderSelector
            // 
            BlenderSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            BlenderSelector.FormattingEnabled = true;
            BlenderSelector.Location = new Point(168, 62);
            BlenderSelector.Name = "BlenderSelector";
            BlenderSelector.Size = new Size(250, 33);
            BlenderSelector.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(454, 14);
            label3.Name = "label3";
            label3.Size = new Size(150, 25);
            label3.TabIndex = 4;
            label3.Text = "Amount:";
            // 
            // AmountSlider
            // 
            AmountSlider.LargeChange = 10;
            AmountSlider.Location = new Point(610, 6);
            AmountSlider.Maximum = 100;
            AmountSlider.Name = "AmountSlider";
            AmountSlider.Size = new Size(400, 69);
            AmountSlider.TabIndex = 6;
            AmountSlider.TickFrequency = 10;
            // 
            // ImageViewer
            // 
            ImageViewer.Location = new Point(12, 199);
            ImageViewer.Name = "ImageViewer";
            ImageViewer.Size = new Size(1587, 634);
            ImageViewer.TabIndex = 8;
            ImageViewer.TabStop = false;
            // 
            // AmountValue
            // 
            AmountValue.Location = new Point(1016, 14);
            AmountValue.Name = "AmountValue";
            AmountValue.Size = new Size(150, 25);
            AmountValue.TabIndex = 9;
            AmountValue.Text = "0";
            // 
            // BlendModeColor
            // 
            BlendModeColor.AutoSize = true;
            BlendModeColor.Checked = true;
            BlendModeColor.Location = new Point(454, 68);
            BlendModeColor.Name = "BlendModeColor";
            BlendModeColor.Size = new Size(84, 29);
            BlendModeColor.TabIndex = 10;
            BlendModeColor.TabStop = true;
            BlendModeColor.Text = "Color:";
            BlendModeColor.UseVisualStyleBackColor = true;
            // 
            // BlendModeImage
            // 
            BlendModeImage.AutoSize = true;
            BlendModeImage.Location = new Point(454, 138);
            BlendModeImage.Name = "BlendModeImage";
            BlendModeImage.Size = new Size(91, 29);
            BlendModeImage.TabIndex = 11;
            BlendModeImage.Text = "Image:";
            BlendModeImage.UseVisualStyleBackColor = true;
            // 
            // BlendImageSelector
            // 
            BlendImageSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            BlendImageSelector.FormattingEnabled = true;
            BlendImageSelector.Location = new Point(610, 138);
            BlendImageSelector.Name = "BlendImageSelector";
            BlendImageSelector.Size = new Size(250, 33);
            BlendImageSelector.TabIndex = 12;
            // 
            // ColorButton
            // 
            ColorButton.BackColor = Color.FromArgb(255, 128, 128);
            ColorButton.Location = new Point(610, 68);
            ColorButton.Name = "ColorButton";
            ColorButton.Size = new Size(50, 50);
            ColorButton.TabIndex = 13;
            ColorButton.UseVisualStyleBackColor = false;
            // 
            // ColorBlenderForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1611, 845);
            Controls.Add(ColorButton);
            Controls.Add(BlendImageSelector);
            Controls.Add(BlendModeImage);
            Controls.Add(BlendModeColor);
            Controls.Add(AmountValue);
            Controls.Add(ImageViewer);
            Controls.Add(AmountSlider);
            Controls.Add(label3);
            Controls.Add(BlenderSelector);
            Controls.Add(label2);
            Controls.Add(ImageSelector);
            Controls.Add(label1);
            Name = "ColorBlenderForm";
            Text = "Color Blender";
            ((System.ComponentModel.ISupportInitialize)AmountSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)ImageViewer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox ImageSelector;
        private Label label2;
        private ComboBox BlenderSelector;
        private Label label3;
        private TrackBar AmountSlider;
        private PictureBox ImageViewer;
        private Label AmountValue;
        private RadioButton BlendModeColor;
        private RadioButton BlendModeImage;
        private ComboBox BlendImageSelector;
        private Button ColorButton;
    }
}
