using Gestore_Abbonamenti.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Gestore_Abbonamenti.View;

public partial class ScoolSettingView : Page
{
    public ScoolSettingView()
    {
        InitializeComponent();
        DataContext = new ScoolSettingViewModel();
    }

    private void EnableSearch_Click(object sender, RoutedEventArgs e)
    {
        if (SearchText.Width == 0)
        {
            // Impostiamo la visibilità prima dell'animazione
            SearchText.Visibility = Visibility.Visible;

            // Mostra il TextBox prima di animarlo
            SearchText.Opacity = 1;  // Assicurati che sia visibile
            SearchText.Focus();

            // Crea l'animazione di espansione
            DoubleAnimation expandAnimation = new DoubleAnimation
            {
                From = 0,      // Larghezza iniziale
                To = 200,      // Larghezza finale
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // Esegui l'animazione
            SearchText.BeginAnimation(WidthProperty, expandAnimation);
        }
        else
        {
            // Crea l'animazione di contrazione
            DoubleAnimation collapseAnimation = new DoubleAnimation
            {
                From = 200,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Al termine dell'animazione, nascondi il TextBox
            collapseAnimation.Completed += (s, a) =>
            {
                SearchText.Visibility = Visibility.Collapsed;
                SearchText.Opacity = 0;
            };

            // Esegui l'animazione di contrazione
            SearchText.BeginAnimation(WidthProperty, collapseAnimation);
        }
    }
}
