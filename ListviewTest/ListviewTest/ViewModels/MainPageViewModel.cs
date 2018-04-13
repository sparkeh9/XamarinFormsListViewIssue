namespace ListviewTest.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Prism.Navigation;

    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<string> itemsSource;

        public ObservableCollection<string> ItemsSource
        {
            get => itemsSource;
            set => SetProperty( ref itemsSource, value );
        }

        public MainPageViewModel( INavigationService navigationService ) : base( navigationService )
        {
            Title = "Main Page";

            ItemsSource = new ObservableCollection<string>(new List<string>
            {
                "1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1",
                "1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1",
                "1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1",
                "1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1","1"
            });
        }
    }
}
