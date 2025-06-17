using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View.ShowDialog;
using Gestore_Abbonamenti.ViewModel;
using GestoreAbbonamenti.Common.Enum;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gestore_Abbonamenti.View
{
    /// <summary>
    /// Logica di interazione per AddOrUpdateArchivioView.xaml
    /// </summary>
    public partial class AddOrUpdateArchivioView : Window
    {
        private bool isPopupOpen = false;
        public AddOrUpdateArchivioView(int? anno = null, long id = 0)
        {       
            InitializeComponent();
            DataContext = new AddOrUpdateArchivioViewModel(anno, id);
        }

        private void ComboboxBehavior(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (sender is ComboBox comboBox && comboBox.IsEditable)
                {
                    var textBox = (TextBox)comboBox.Template.FindName("PART_EditableTextBox", comboBox);
                    if (textBox == null)
                        return;

                    if (e.Key == Key.Enter)
                    {
                        comboBox.IsDropDownOpen = false;
                        return;
                    }

                    var text = textBox.Text;

                    if (string.IsNullOrEmpty(text))
                        return;

                    // Apri la lista a discesa solo se il testo ha 3 o più caratteri
                    if (text.Length >= 1)
                        comboBox.IsDropDownOpen = true;
                    else
                        comboBox.IsDropDownOpen = false;

                    // Evita la selezione automatica del testo
                    textBox.SelectionStart = text.Length;
                    textBox.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                // Puoi loggare o gestire l'eccezione se necessario
            }
        }

        private void SearchList_KeyUp(object sender, KeyEventArgs e)
        {
            ComboboxBehavior(sender, e);
        }
    }
}
