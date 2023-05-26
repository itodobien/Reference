using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using VADisabilityCalculator.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(CustomLabelRenderer))]
namespace VADisabilityCalculator.iOS
{
    public class CustomLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var attributedString = new Foundation.NSAttributedString(Control.Text);
                Control.AttributedText = attributedString;
                Control.UserInteractionEnabled = true;
            }
        }
    }
}
