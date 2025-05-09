using System.Drawing.Imaging;

namespace sachssoft.Drawing.Colors.Samples
{

    public partial class ColorTransformerForm : Form
    {
        private List<string> _file_paths = new();
        private List<Type> _transformers = new();
        private IColorTransformer _transformer;
        private Bitmap _source;
        private Bitmap _target;

        public ColorTransformerForm()
        {
            InitializeComponent();
            LoadImages();
            LoadTransformers();
            ImageSelector.SelectedIndexChanged += ImageSelector_SelectedIndexChanged;
            TransformerSelector.SelectedIndexChanged += TransformerSelector_SelectedIndexChanged;
            AmountSlider.ValueChanged += AmountSlider_ValueChanged;
            FactorSlider.ValueChanged += FactorSlider_ValueChanged;
            ImageSelector.SelectedIndex = 0;
            TransformerSelector.SelectedIndex = 0;
        }

        private void FactorSlider_ValueChanged(object? sender, EventArgs e)
        {
            FactorValue.Text = Math.Round(FactorSlider.Value / 100.0, 2).ToString();
            UpdateTransformation();
        }

        private void TransformerSelector_SelectedIndexChanged(object? sender, EventArgs e)
        {

            if (TransformerSelector.SelectedIndex >= 0)
            {
                var type = _transformers[TransformerSelector.SelectedIndex];
                _transformer = Activator.CreateInstance(type) as IColorTransformer;

                if (_transformer is IFactorColorTransformer factored)
                {
                    FactorSlider.Minimum = (int)(factored.FactorMinimum * 100f);
                    FactorSlider.Maximum = (int)(factored.FactorMaximum * 100f);
                }

                FactorSlider.Enabled = _transformer is IFactorColorTransformer;

                UpdateTransformation();
            }
        }

        private void AmountSlider_ValueChanged(object? sender, EventArgs e)
        {
            AmountValue.Text = Math.Round(AmountSlider.Value / 100.0, 2).ToString();
            UpdateTransformation();
        }

        private void UpdateTransformation()
        {
            if (_source != null && _transformer != null)
            {
                Adjust(new ColorRange(AmountSlider.Value / 100f), FactorSlider.Value / 100f);
            }
        }

        private void ImageSelector_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (ImageSelector.SelectedIndex >= 0)
            {
                _source = new Bitmap(_file_paths[ImageSelector.SelectedIndex]);
                ImageViewer.Image = _source;
                UpdateTransformation();
            }
        }

        private void LoadImages()
        {
            var images_dir = Path.Combine(AppContext.BaseDirectory, "Images");
            if (Directory.Exists(images_dir))
            {
                var image_files = Directory.GetFiles(images_dir, "*.*", SearchOption.AllDirectories)
                                           .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".webp"))
                                           .ToList();

                foreach (var file in image_files)
                {
                    Console.WriteLine($"Bild gefunden: {file}");
                    ImageSelector.Items.Add(Path.GetFileName(file));
                    _file_paths.Add(file);
                }
            }
        }

        private void LoadTransformers()
        {
            var transformers = typeof(IColorTransformer).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IColorTransformer)) && t.IsClass && !t.IsAbstract);

            foreach (var transformer in transformers)
            {
                TransformerSelector.Items.Add(SplitPascalCase(transformer.Name));
                _transformers.Add(transformer);
            }
        }

        private static string SplitPascalCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // "Transformer" entfernen
            input = input.Replace("Transformer", "")
                         .Replace("Color", "");

            // Leerzeichen zwischen Großbuchstaben einfügen
            var formatted = System.Text.RegularExpressions.Regex
                .Replace(input, "(?<!^)([A-Z])", " $1")
                .Trim();

            return formatted;
        }

        private unsafe void Adjust(ColorRange amount, float factor)
        {
            _target?.Dispose(); // Vorheriges Zielbild freigeben
            _target = new Bitmap(_source.Width, _source.Height, PixelFormat.Format32bppArgb);

            Rectangle rect = new Rectangle(0, 0, _source.Width, _source.Height);

            BitmapData src_data = null;
            BitmapData tgt_data = null;

            try
            {
                src_data = _source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                tgt_data = _target.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                int height = _source.Height;
                int width = _source.Width;

                for (int y = 0; y < height; y++)
                {
                    uint* src_row = (uint*)((byte*)src_data.Scan0 + y * src_data.Stride);
                    uint* tgt_row = (uint*)((byte*)tgt_data.Scan0 + y * tgt_data.Stride);

                    for (int x = 0; x < width; x++)
                    {
                        uint pixel = src_row[x];

                        byte a = (byte)(pixel >> 24);
                        byte r = (byte)(pixel >> 16);
                        byte g = (byte)(pixel >> 8);
                        byte b = (byte)(pixel);

                        Color output;

                        if (_transformer is IFactorColorTransformer factored)
                        {
                            output = factored.Transform(Color.FromArgb(a, r, g, b), amount, factor);
                        }
                        else
                        {
                            output = _transformer.Transform(Color.FromArgb(a, r, g, b), amount);
                        }

                        tgt_row[x] = (uint)(output.A << 24 | output.R << 16 | output.G << 8 | output.B);
                    }
                }
            }
            finally
            {
                if (src_data != null) _source.UnlockBits(src_data);
                if (tgt_data != null) _target.UnlockBits(tgt_data);
            }

            ImageViewer.Image = _target;
        }

    }
}
