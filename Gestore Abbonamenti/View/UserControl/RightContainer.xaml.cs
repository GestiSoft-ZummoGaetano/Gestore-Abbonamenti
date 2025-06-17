using GestoreAbbonamenti.Model;
using System.Windows;
using userControl = System.Windows.Controls.UserControl;

namespace Gestore_Abbonamenti.View.UserControl
{
    /// <summary>
    /// Logica di interazione per RightContainer.xaml
    /// </summary>
    public partial class RightContainer : userControl
    {
        public RightContainer()
        {
            InitializeComponent();
        }

        #region Property
        public static readonly DependencyProperty GenitoreProperty =
    DependencyProperty.Register("Genitore", typeof(ViewRiepilogo), typeof(RightContainer), new PropertyMetadata(null));

public ViewRiepilogo Genitore
{
    get { return (ViewRiepilogo)GetValue(GenitoreProperty); }
    set { SetValue(GenitoreProperty, value); }
}


        #endregion
    }
}
