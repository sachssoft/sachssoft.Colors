namespace sachssoft.Drawing.Colors.Samples
{
    partial class ColorTransformerForm
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
            TransformerSelector = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            AmountSlider = new TrackBar();
            FactorSlider = new TrackBar();
            ImageViewer = new PictureBox();
            AmountValue = new Label();
            FactorValue = new Label();
            ((System.ComponentModel.ISupportInitialize)AmountSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FactorSlider).BeginInit();
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
            label2.Text = "Transformer:";
            // 
            // TransformerSelector
            // 
            TransformerSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            TransformerSelector.FormattingEnabled = true;
            TransformerSelector.Location = new Point(168, 62);
            TransformerSelector.Name = "TransformerSelector";
            TransformerSelector.Size = new Size(250, 33);
            TransformerSelector.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(454, 14);
            label3.Name = "label3";
            label3.Size = new Size(150, 25);
            label3.TabIndex = 4;
            label3.Text = "Amount:";
            // 
            // label4
            // 
            label4.Location = new Point(454, 70);
            label4.Name = "label4";
            label4.Size = new Size(150, 25);
            label4.TabIndex = 5;
            label4.Text = "Factor:";
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
            // FactorSlider
            // 
            FactorSlider.Enabled = false;
            FactorSlider.LargeChange = 10;
            FactorSlider.Location = new Point(610, 62);
            FactorSlider.Maximum = 100;
            FactorSlider.Name = "FactorSlider";
            FactorSlider.Size = new Size(400, 69);
            FactorSlider.TabIndex = 7;
            FactorSlider.TickFrequency = 10;
            // 
            // ImageViewer
            // 
            ImageViewer.Location = new Point(12, 133);
            ImageViewer.Name = "ImageViewer";
            ImageViewer.Size = new Size(1587, 700);
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
            // FactorValue
            // 
            FactorValue.Location = new Point(1016, 70);
            FactorValue.Name = "FactorValue";
            FactorValue.Size = new Size(150, 25);
            FactorValue.TabIndex = 10;
            FactorValue.Text = "0";
            // 
            // ColorTransformerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1611, 845);
            Controls.Add(FactorValue);
            Controls.Add(AmountValue);
            Controls.Add(ImageViewer);
            Controls.Add(FactorSlider);
            Controls.Add(AmountSlider);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(TransformerSelector);
            Controls.Add(label2);
            Controls.Add(ImageSelector);
            Controls.Add(label1);
            Name = "ColorTransformerForm";
            Text = "Color Transformer";
            ((System.ComponentModel.ISupportInitialize)AmountSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)FactorSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)ImageViewer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox ImageSelector;
        private Label label2;
        private ComboBox TransformerSelector;
        private Label label3;
        private Label label4;
        private TrackBar AmountSlider;
        private TrackBar FactorSlider;
        private PictureBox ImageViewer;
        private Label AmountValue;
        private Label FactorValue;
    }
}
