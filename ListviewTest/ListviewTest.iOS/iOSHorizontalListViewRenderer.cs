using ListviewTest;
using ListviewTest.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(HorizontalListView), typeof(iOSHorizontalListViewRenderer))]
namespace ListviewTest.iOS
{
    using Xamarin.Forms.Platform.iOS;

    public class iOSHorizontalListViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged( VisualElementChangedEventArgs e )
        {
            base.OnElementChanged( e );

            var element = e.NewElement as HorizontalListView;
            element?.Render();
        }
    }
}
