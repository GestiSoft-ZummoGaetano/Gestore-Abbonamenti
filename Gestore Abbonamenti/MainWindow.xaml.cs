using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View;
using Gestore_Abbonamenti.View.ShowDialog;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model.cache;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Gestore_Abbonamenti
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private static bool IsAuthenticated = false;
        private static string message;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {           
            await VerificaCredenziali();

            if (!IsAuthenticated)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, $"{message}\nAccesso negato!", ShowDialogImage.ERROR);
                Application.Current.Shutdown();  // Chiude l'app se l'autenticazione fallisce
                return;
            }

            frame.Navigate(new ListaGenitoriView());
        }

        private async Task VerificaCredenziali()
        {
            try
            {
                var splash = new SplashScreen("Autenticazione in corso...");
                splash.Show();

                var NomeUtente = LogicFactory.Instance.StartUp.GetUser();
                var url = "https://www.villachifeciscopello.com/_functions/verificaCredenziali";
                var json = $"{{" +
                    $"\"utente\": \"{NomeUtente}\", " +
                    $"\"idHardware\": \"{GestiCache.IdHardware}\", " +
                    $"\"idHardDisk\": \"{GestiCache.IdHardDisk}\", " +
                    $"\"idCpu\": \"{GestiCache.IdCpu}\"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);

                IsAuthenticated = jsonResponse["success"]?.ToObject<bool>() ?? false;
                message = jsonResponse["message"]?.ToString() ?? "";

                splash.Close();
            }
            catch (Exception ex)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "Errore di connessione: " + ex.Message, ShowDialogImage.ERROR);
                Application.Current.Shutdown();
            }
        }

        private void ListaAnagrafica()
        {
            frame.NavigationService.RemoveBackEntry();
            frame.Content = null;
            frame.Navigate(new ListaGenitoriView());
        }

        private void ScoolSettingView()
        {
            frame.NavigationService.RemoveBackEntry();
            frame.Content = null;
            frame.Navigate(new ScoolSettingView());
        }

        private void Riepilog(ToggleButton clickedButton)
        {
            if (clickedButton != null)
            {
                bool semstre;
                string tagValue = clickedButton.Tag?.ToString();

                semstre = tagValue == "RIEPILOGO PRIMO SEMESTRE";

                frame.NavigationService.RemoveBackEntry();
                frame.Content = null;
                frame.Navigate(new VisualizzaView(semstre, tagValue));
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton clickedButton)
            {
                var parent = VisualTreeHelper.GetParent(clickedButton);
                while (parent != null && !(parent is Panel))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (parent is Panel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is ToggleButton button && button != clickedButton)
                        {
                            button.IsChecked = false;
                        }
                    }
                }
                switch (clickedButton.Tag.ToString())
                {
                    case "LISTA ANAGRAFICA":
                        ListaAnagrafica();
                        break;

                    case "RIEPILOGO PRIMO SEMESTRE":
                        Riepilog(clickedButton);
                        break;

                    case "RIEPILOGO SECONDO SEMESTRE":
                        Riepilog(clickedButton);
                        break;

                    case "SCUOLE":
                        ScoolSettingView();
                        break;
                }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.pdf");

            if (File.Exists(pdfPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Il file PDF non è stato trovato.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
