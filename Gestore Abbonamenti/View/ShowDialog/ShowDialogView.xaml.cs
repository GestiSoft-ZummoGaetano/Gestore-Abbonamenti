using GestiSoGestoreAbbonamentift.Common.Enum;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.ViewModel;
using System.Windows;

namespace Gestore_Abbonamenti.View.ShowDialog
{
    /// <summary>
    /// Logica di interazione per ShowDialog.xaml
    /// </summary>
    public partial class ShowDialogView : Window
    {
        private readonly ShowDialogViewModel _viewModel;
        public ShowDialogResult SelectedButton { get; private set; }
        public ShowDialogView()
        {
            InitializeComponent();
            _viewModel = new ShowDialogViewModel();
            DataContext = _viewModel;
        }

        public void SetContext(ShowDialogResult title, string textMessage, ShowDialogImage image, ShowDialogButton button = ShowDialogButton.OK)
        {
            _viewModel.SetContext(title, textMessage, image, button);
        }

        public static ShowDialogResult ShowDialogPage(ShowDialogResult title, string textMessage, ShowDialogImage image, ShowDialogButton button = ShowDialogButton.OK)
        {
            var dialog = new ShowDialogView();
            dialog.SetContext(title, textMessage, image, button);
            dialog.ShowDialog();
            return dialog.SelectedButton;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            SelectedButton = ShowDialogResult.YES;
            Close();
        }
        private void No_Click(object sender, RoutedEventArgs e)
        {
            SelectedButton = ShowDialogResult.NO;
            Close();
        }
    }
}

