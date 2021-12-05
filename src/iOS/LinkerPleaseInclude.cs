using System.Collections.Specialized;
using System.Windows.Input;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace PrankChat.Mobile.iOS
{
    [Preserve(AllMembers = true)]
    public class LinkerPleaseInclude
    {
        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                                      uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
        }

        public void Include(UIBarButtonItem barButton)
        {
            barButton.Clicked += (s, e) => barButton.Title = barButton.Title + string.Empty;
            barButton.Enabled = false;
        }

        public void Include(UITextField textField)
        {
            textField.Text = textField.Text + string.Empty;
            textField.Placeholder = textField.Placeholder + string.Empty;
            textField.EditingChanged += (sender, args) => { textField.Text = string.Empty; };
            textField.ValueChanged += (sender, args) => { textField.Text = string.Empty; };
            textField.EditingDidEnd += (sender, args) => { textField.Text = string.Empty; };
        }

        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + string.Empty;
            textView.TextStorage.DidProcessEditing += (sender, e) => textView.Text = string.Empty;
            textView.Changed += (sender, args) => { textView.Text = string.Empty; };
        }

        public void Include(UILabel label)
        {
            label.Text = label.Text + string.Empty;
            label.AttributedText = new NSAttributedString(label.AttributedText.ToString() + string.Empty);
        }

        public void Include(UIImageView imageView)
        {
            imageView.Image = new UIImage(imageView.Image.CGImage);
        }

        public void Include(UIDatePicker date)
        {
            date.Date = date.Date.AddSeconds(1);
            date.ValueChanged += (sender, args) => { date.Date = NSDate.DistantFuture; };
        }

        public void Include(UISlider slider)
        {
            slider.Value = slider.Value + 1;
            slider.ValueChanged += (sender, args) => { slider.Value = 1; };
        }

        public void Include(UIProgressView progress)
        {
            progress.Progress = progress.Progress + 1;
        }

        public void Include(UISwitch sw)
        {
            sw.On = !sw.On;
            sw.ValueChanged += (sender, args) => { sw.On = false; };
        }

        public void Include(MvxViewController vc)
        {
            vc.Title = vc.Title + string.Empty;
        }

        public void Include(UIStepper s)
        {
            s.Value = s.Value + 1;
            s.ValueChanged += (sender, args) => { s.Value = 0; };
        }

        public void Include(UIPageControl s)
        {
            s.Pages = s.Pages + 1;
            s.ValueChanged += (sender, args) => { s.Pages = 0; };
        }

        public void Include(UISearchBar bar)
        {
            bar.Text = string.Empty;
            bar.Placeholder = bar.Placeholder + string.Empty;
            bar.TextChanged += (s, e) => { };
            bar.ShouldBeginEditing = (searchBar) => false;
        }

        public void Include(UIViewController controller)
        {
            controller.NavigationItem.LeftBarButtonItem = new UIBarButtonItem();
        }

        public void Include(Microsoft.Extensions.Logging.Abstractions.NullLogger c)
        {
            c.BeginScope<UILexiconEntry>(null);
        }

        public void Include(Microsoft.Extensions.Logging.ILogger l)
        {
            l.Log<UITextView>(Microsoft.Extensions.Logging.LogLevel.Critical, new Microsoft.Extensions.Logging.EventId(), new UITextView(), new System.Exception(), (u, ex) => ex.Message);
        }

        public void Include(INotifyCollectionChanged changed)
        {
            changed.CollectionChanged += (s, e) => { var test = string.Format("{0}{1}{2}{3}{4}", e.Action, e.NewItems, e.NewStartingIndex, e.OldItems, e.OldStartingIndex); };
        }

        public void Include(ICommand command)
        {
            command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
        }
    }
}