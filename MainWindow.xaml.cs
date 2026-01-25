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

    private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
    {
        SwitchTheme(true);
    }

    private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
    {
        SwitchTheme(false);
    }

    private void SwitchTheme(bool isDark)
    {
        var newThemeSource = new Uri(isDark ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml", UriKind.Relative);
        
        // Load the new dictionary
        var newTheme = new ResourceDictionary { Source = newThemeSource };

        // Find and replace the existing theme dictionary
        if (Application.Current.Resources.MergedDictionaries.Count > 0)
        {
            Application.Current.Resources.MergedDictionaries[0] = newTheme;
        }
    }
}