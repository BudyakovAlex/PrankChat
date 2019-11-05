using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.iOS.Presentation.Views
{
    public partial class MainView : BaseView
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            //set.Bind(TextField).To(vm => vm.Text);
            //set.Bind(Button).To(vm => vm.ResetTextCommand);
            set.Apply();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

