namespace ListviewTest
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class HorizontalListView : ScrollView
    {
        public DataTemplateSelector ItemTemplateSelector
        {
            get => (DataTemplateSelector) GetValue( ItemTemplateSelectorProperty );
            set => SetValue( ItemTemplateSelectorProperty, value );
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue( ItemsSourceProperty );
            set => SetValue( ItemsSourceProperty, value );
        }

//        public DataTemplate ItemTemplate
//        {
//            get => (DataTemplate) GetValue( ItemTemplateProperty );
//            set => SetValue( ItemTemplateProperty, value );
//        }

        public ICommand SelectedCommand
        {
            get => (ICommand) GetValue( SelectedCommandProperty );
            set => SetValue( SelectedCommandProperty, value );
        }

        public object SelectedCommandParameter
        {
            get => GetValue( SelectedCommandParameterProperty );
            set => SetValue( SelectedCommandParameterProperty, value );
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create( "ItemsSource", typeof( IEnumerable ), typeof( HorizontalListView ), default( IEnumerable ),
                                     BindingMode.Default, null, HandleBindingPropertyChangedDelegate );

//        public static readonly BindableProperty ItemTemplateProperty =
//            BindableProperty.Create( "ItemTemplate", typeof( DataTemplate ), typeof( HorizontalListView ), default( DataTemplate ) );

        public static readonly BindableProperty ItemTemplateSelectorProperty =
            BindableProperty.Create( "ItemTemplateSelector", typeof( DataTemplateSelector ), typeof( HorizontalListView ), default( DataTemplateSelector ) );

        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create( "SelectedCommand", typeof( ICommand ), typeof( HorizontalListView ), null );

        public static readonly BindableProperty SelectedCommandParameterProperty =
            BindableProperty.Create( "SelectedCommandParameter", typeof( object ), typeof( HorizontalListView ), null );

        public event EventHandler<ItemTappedEventArgs> ItemSelected;

        private static void HandleBindingPropertyChangedDelegate( BindableObject bindable, object oldValue, object newValue )
        {
            var isOldObservable = oldValue?.GetType().GetTypeInfo().ImplementedInterfaces.Any( i => i == typeof( INotifyCollectionChanged ) );
            var isNewObservable = newValue?.GetType().GetTypeInfo().ImplementedInterfaces.Any( i => i == typeof( INotifyCollectionChanged ) );

            var tl = (HorizontalListView) bindable;
            if ( isOldObservable.GetValueOrDefault( false ) )
            {
                ( (INotifyCollectionChanged) oldValue ).CollectionChanged -= tl.HandleCollectionChanged;
            }

            if ( isNewObservable.GetValueOrDefault( false ) )
            {
                ( (INotifyCollectionChanged) newValue ).CollectionChanged += tl.HandleCollectionChanged;
            }

            if ( oldValue != newValue )
            {
                tl.Render();
            }
        }

        private void HandleCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            Render();
        }

        public void Render()
        {
            if ( ItemTemplateSelector == null || ItemsSource == null )
            {
                Content = null;
                return;
            }

            var layout = new StackLayout
            {
                Orientation = Orientation == ScrollOrientation.Vertical ? StackOrientation.Vertical : StackOrientation.Horizontal
            };

            foreach ( var item in ItemsSource )
            {
                var command = SelectedCommand ?? new Command( obj =>
                                                              {
                                                                  var args = new ItemTappedEventArgs( ItemsSource, item );
                                                                  ItemSelected?.Invoke( this, args );
                                                              } );
                var commandParameter = SelectedCommandParameter ?? item;


                try
                {
                    var view = ItemTemplateSelector.CreateContent() as View;
                    view.BindingContext = item;
                    view.GestureRecognizers.Add( new TapGestureRecognizer
                    {
                        Command = command,
                        CommandParameter = commandParameter,
                        NumberOfTapsRequired = 1
                    } );

                    layout.Children.Add( view );
                }
                catch ( Exception e )
                { 
                    throw;
                }
            }

            Content = layout;
        }
    }
}