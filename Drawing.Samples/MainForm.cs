namespace sachssoft.Drawing.Colors.Samples
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TransformerButton.Click += (s, e) =>
            {
                var dlg = new ColorTransformerForm();
                dlg.ShowDialog();
            };
            BlenderButton.Click += (s, e) =>
            {
                var dlg = new ColorBlenderForm();
                dlg.ShowDialog();
            };
        }
    }
}
