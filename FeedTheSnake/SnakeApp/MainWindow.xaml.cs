using FeedTheSnake;
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
using static FeedTheSnake.SnakeGame;

namespace SnakeApp
{
    //TODO Make help menu , work with milisecondns or 0.1
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Binds game Score to txtBlock
            Binding b = new Binding
            {
                ElementName = "feedTheSnake",
                Path = new PropertyPath("Score"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged

            };
            DataContext = feedTheSnake;
            txtScore.SetBinding(TextBlock.TextProperty, b);

            //Binds game ExpiretionTime to SlrExpiretionTime
            Binding s = new Binding
            {
                ElementName = "feedTheSnake",
                Path = new PropertyPath("ExpiretionTime"),                
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged

            };
            DataContext = feedTheSnake;
            SlrExpiretionTime.SetBinding(Slider.ValueProperty, s);

            //Binds game State to txtState
            Binding st = new Binding
            {
                ElementName = "feedTheSnake",
                Path = new PropertyPath("State"),
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged

            };
            DataContext = feedTheSnake;
            txtState.SetBinding(TextBlock.TextProperty, st);

            feedTheSnake.NewGame();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            feedTheSnake.NewGame();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            feedTheSnake.NewGame();
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {            
            var checkedButton = (RadioButton)e.Source;
            feedTheSnake.Level = checkedButton.Name switch
            {
                "radZero" => GameLevel.ZERO,
                "radFirst" => GameLevel.FIRST,
                "radSecond" => GameLevel.SECOND,
                "radThird" => GameLevel.THIRD,
                _ => GameLevel.ZERO
            };
           
            FeedTheSnake.SnakeGame a = new FeedTheSnake.SnakeGame();
            feedTheSnake.NewGame();
            
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            helpPopup.IsOpen = true;


            this.IsEnabled = false;


        }

        private void HelpWindow_PopupClosed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
        }
    }
}