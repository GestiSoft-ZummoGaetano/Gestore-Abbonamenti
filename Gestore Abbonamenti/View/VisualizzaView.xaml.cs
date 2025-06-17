using Gestore_Abbonamenti.View.UserControl;
using Gestore_Abbonamenti.ViewModel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Gestore_Abbonamenti.View;

/// <summary>
/// Logica di interazione per VisualizzaView.xaml
/// </summary>
public partial class VisualizzaView : Page
{
    public VisualizzaView(bool semestre, string tagValue)
    {
        InitializeComponent();
        DataContext = new RiepilogoViewModel(semestre, tagValue);
    }
}
