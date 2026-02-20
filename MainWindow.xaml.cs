using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DekelApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).WindowState = WindowState.Minimized;
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        var win = Window.GetWindow(this);
        win.WindowState = win.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }
}