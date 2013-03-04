using System.Windows;

namespace NotifyPropertyChanged
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new NameViewModel_PostSharp();
//            DataContext = new NameViewModel_NotifyPropertyWeaver();
//            DataContext = new NameViewModel();
        }
    }
}
