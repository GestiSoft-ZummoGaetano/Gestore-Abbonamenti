using System.Windows;

namespace Gestore_Abbonamenti;

/// <summary>
/// Logica di interazione per SplashScreen.xaml
/// </summary>
public partial class SplashScreen : Window
{
    public SplashScreen(string title)
    {
        InitializeComponent();
        splashTitle.Text = title;
    }
}
