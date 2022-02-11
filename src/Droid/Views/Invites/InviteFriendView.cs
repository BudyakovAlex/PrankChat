using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Core.Content;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Invites;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Invites
{
    [MvxActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        WindowSoftInputMode = SoftInput.AdjustResize)]
    public sealed class InviteFriendView : BaseView<InviteFriendViewModel>, TextView.IOnEditorActionListener
    {
        private TextView _descriptionTextView;
        private TextInputLayout _emailTextInputLayout;
        private TextInputEditText _emailTextInputEditText;
        private TextView _errorTextView;
        private Button _sendButton;
        private FrameLayout _loadingOverlay;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.InviteFriend;

        public bool HasError
        {
            set
            {
                UpdateEmailTextInputLayoutBoxStrokeColor(value);
                UpdateEmailTextInputEditTextColor(value);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            OnCreate(savedInstanceState, Resource.Layout.activity_invite_friend);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _descriptionTextView = FindViewById<TextView>(Resource.Id.description_text_view);
            _emailTextInputLayout = FindViewById<TextInputLayout>(Resource.Id.email_text_input_layout);
            _errorTextView = FindViewById<TextView>(Resource.Id.error_text_view);
            _loadingOverlay = FindViewById<FrameLayout>(Resource.Id.loading_overlay);    

            _emailTextInputEditText = FindViewById<TextInputEditText>(Resource.Id.email_text_input_edit_text);
            _emailTextInputEditText.SetOnEditorActionListener(this);

            _sendButton = FindViewById<Button>(Resource.Id.send_button);
            _sendButton.Text = Core.Localization.Resources.Send;

            var emailTextInputLayout = FindViewById<TextInputLayout>(Resource.Id.email_text_input_layout);
            emailTextInputLayout.Hint = Core.Localization.Resources.EnterFriendsEmail;
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(this).For(nameof(HasError)).To(vm => vm.HasError);
            bindingSet.Bind(_descriptionTextView).For(v => v.BindAttributedText()).To(vm => vm.Description);
            bindingSet.Bind(_emailTextInputEditText).For(v => v.Text).To(vm => vm.Email);
            bindingSet.Bind(_errorTextView).For(v => v.Text).To(vm => vm.ErrorMessage);
            bindingSet.Bind(_sendButton).For(v => v.BindClick()).To(vm => vm.SendCommand);
            bindingSet.Bind(_loadingOverlay).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }

        private void UpdateEmailTextInputLayoutBoxStrokeColor(bool hasError)
        {
            var colorId = hasError
                ? Resource.Color.light_error
                : Resource.Color.border;

            var color = ContextCompat.GetColor(this, colorId);
            var colorStateList = CreateColorStateList(color);
            _emailTextInputLayout.SetBoxStrokeColorStateList(colorStateList);

            static ColorStateList CreateColorStateList(int color) => new ColorStateList(
                new int[][]
                {
                    new int[] { Android.Resource.Attribute.StateFocused },
                },
                new int[]
                {
                    color,
                    color
                });
        }

        private void UpdateEmailTextInputEditTextColor(bool hasError)
        {
            var colorId = hasError
                ? Resource.Color.light_error
                : Resource.Color.applicationBlack;

            var colorStateList = ContextCompat.GetColorStateList(this, colorId);
            _emailTextInputEditText.SetTextColor(colorStateList);
        }

        bool TextView.IOnEditorActionListener.OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Done)
            {
                ViewModel.SendCommand.Execute();
            }

            return false;
        }
    }
}
