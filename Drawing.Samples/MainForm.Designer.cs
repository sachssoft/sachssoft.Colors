namespace sachssoft.Drawing.Colors.Samples
{
    partial class MainForm
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
            TransformerButton = new Button();
            BlenderButton = new Button();
            SuspendLayout();
            // 
            // TransformerButton
            // 
            TransformerButton.Location = new Point(12, 12);
            TransformerButton.Name = "TransformerButton";
            TransformerButton.Size = new Size(275, 71);
            TransformerButton.TabIndex = 0;
            TransformerButton.Text = "Transformer";
            TransformerButton.UseVisualStyleBackColor = true;
            // 
            // BlenderButton
            // 
            BlenderButton.Location = new Point(12, 89);
            BlenderButton.Name = "BlenderButton";
            BlenderButton.Size = new Size(275, 71);
            BlenderButton.TabIndex = 1;
            BlenderButton.Text = "Blender";
            BlenderButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BlenderButton);
            Controls.Add(TransformerButton);
            Name = "MainForm";
            Text = "MainForm";
            ResumeLayout(false);
        }

        #endregion

        private Button TransformerButton;
        private Button BlenderButton;
    }
}