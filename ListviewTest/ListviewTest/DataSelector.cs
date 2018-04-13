namespace ListviewTest
{
    using Xamarin.Forms;

    public class DataSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate( object item, BindableObject container )
        {
            return new DataTemplate( () => new DataTemplateView() );
        }
    }
}
