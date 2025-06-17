using Gestore_Abbonamenti.ViewModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Gestore_Abbonamenti.View
{
    /// <summary>
    /// Logica di interazione per Istituti.xaml
    /// </summary>
    public partial class IstitutiView : Window
    {
        public string? CodiceSelezionato { get; private set; }
        public IstitutiView()
        {
            InitializeComponent();
            DataContext = new ViewModel.IstitutiViewModel();

            Loaded += (s, e) =>
            {
                if (DataContext is IstitutiViewModel vm)
                {
                    vm.CloseRequested += (sender, result) =>
                    {
                        DialogResult = result;
                        Close();
                    };
                }
            };

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
}
