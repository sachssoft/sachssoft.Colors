using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.IO;
using System;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Runtime.InteropServices;
using Avalonia.Media;
using System.Net.Http.Headers;

namespace sachssoft.Avalonia.Colors.Samples.Views;

public partial class TransformersView : UserControl
{
    private IColorTransformer? _transformator;
    private WriteableBitmap _source;
    private WriteableBitmap _destination;

    public TransformersView()
    {
        InitializeComponent();

        ImageSelector.SelectionChanged += ImageSelector_SelectionChanged;
        TransformerSelector.SelectionChanged += TransformerSelector_SelectionChanged;
        LoadAssets();
        LoadTransformers();

        AmountSlider.ValueChanged += AmountSlider_ValueChanged;
    }

    private void TransformerSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var type = ((ComboBoxItem)TransformerSelector.SelectedItem!).DataContext as Type;

        if (type != null)
        {
            _transformator = Activator.CreateInstance(type) as IColorTransformer;
            UpdateTransformation();
        }
    }

    private void AmountSlider_ValueChanged(object? sender, global::Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
    {
        if (_transformator != null)
        {
            UpdateTransformation();
        }
    }

    private void UpdateTransformation()
    {
        _destination = AdjustFramebufferPixels(_source, (float)AmountSlider.Value);
        ExampleImage.Source = _destination;
    }

    private void ImageSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var item = (ComboBoxItem)e.AddedItems[0]!;
        var file_path = (string)item.DataContext!;

        if (File.Exists(file_path))
        {
            // Lade das Bitmap
            _source = WriteableBitmap.Decode(File.Open(file_path, FileMode.Open));
            //_destination = WriteableBitmap.Decode(File.Open(file_path, FileMode.Open));
            ExampleImage.Source = _source;
        }
        else
        {
            ExampleImage.Source = null;
            Console.WriteLine($"Datei nicht gefunden: {file_path}");
        }
    }


    private void LoadAssets()
    {
        var images_dir = Path.Combine(AppContext.BaseDirectory, "Assets", "Images");
        if (Directory.Exists(images_dir))
        {
            var image_files = Directory.GetFiles(images_dir, "*.*", SearchOption.AllDirectories)
                                       .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".webp"))
                                       .ToList();

            foreach (var file in image_files)
            {
                Console.WriteLine($"Bild gefunden: {file}");
                ImageSelector.Items.Add(new ComboBoxItem()
                {
                    Content = Path.GetFileName(file),
                    DataContext = file
                });
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
            TransformerSelector.Items.Add(new ComboBoxItem()
            {
                Content = SplitPascalCase(transformer.Name),
                DataContext = transformer
            });
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

    private WriteableBitmap ConvertBitmapToWriteableBitmap(Bitmap bitmap)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Speichere das Bitmap im MemoryStream im PNG-Format
            bitmap.Save(memoryStream);

            // Erstelle ein WriteableBitmap aus dem MemoryStream
            memoryStream.Seek(0, SeekOrigin.Begin); // Zurück zum Anfang des Streams
            var writeableBitmap = WriteableBitmap.Decode(memoryStream);

            return writeableBitmap;
        }
    }

    public WriteableBitmap AdjustFramebufferPixels(WriteableBitmap source_bitmap, float amount)
    {
        var width = source_bitmap.PixelSize.Width;
        var height = source_bitmap.PixelSize.Height;
        var dpi = source_bitmap.Dpi;

        var new_bitmap = new WriteableBitmap(new PixelSize(width, height), dpi, PixelFormat.Bgra8888, AlphaFormat.Premul);

        using (var source_fb = source_bitmap.Lock())
        using (var target_fb = new_bitmap.Lock())
        {
            unsafe
            {
                byte* src = (byte*)source_fb.Address;
                byte* dst = (byte*)target_fb.Address;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * source_fb.RowBytes + x * 4;

                        byte blue = src[index];
                        byte green = src[index + 1];
                        byte red = src[index + 2];
                        byte alpha = src[index + 3];

                        var output = _transformator!.Transform(new Color(alpha, red, green, blue), amount);

                        //byte new_red = (byte)Math.Clamp(red + red * factor, 0, 255);
                        //byte new_green = (byte)Math.Clamp(green + green * factor, 0, 255);
                        //byte new_blue = (byte)Math.Clamp(blue + blue * factor, 0, 255);

                        //dst[index] = new_blue;
                        //dst[index + 1] = new_green;
                        //dst[index + 2] = new_red;
                        //dst[index + 3] = alpha;

                        dst[index] = output.B;
                        dst[index + 1] = output.G;
                        dst[index + 2] = output.R;
                        dst[index + 3] = output.A;
                    }
                }
            }
        }

        return new_bitmap;
    }



}