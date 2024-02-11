using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelpItem
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : UserControl
    {
        public event EventHandler? PopupClosed;
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var parentPopup = this.Parent as Popup;
            if (parentPopup != null)
            {
                parentPopup.IsOpen = false;
            }

            OnPopupClosed();
        }

        private void OnPopupClosed()
        {
            PopupClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
