using Avalonia;
using Avalonia.Controls;

namespace sachssoft.Avalonia.Colors.Samples
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewSelector.SelectionChanged += ViewSelector_SelectionChanged;
        }

        private void ViewSelector_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ContentViewer.Content = e.AddedItems.Count > 0 ? ((IDataContextProvider?)e.AddedItems[0])?.DataContext : null;
        }
    }
}