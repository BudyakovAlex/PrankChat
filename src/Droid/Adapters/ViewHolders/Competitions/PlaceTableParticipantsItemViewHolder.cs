using Android.Views;
using Google.Android.Material.TextField;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Extensions;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions
{
    public class PlaceTableParticipantsItemViewHolder : CardViewHolder
    {
        private TextInputLayout _textInputLayout;
        private TextInputEditText _textInputEditText;

        public PlaceTableParticipantsItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _textInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.text_input_layout);
            _textInputEditText = view.FindViewById<TextInputEditText>(Resource.Id.text_input_edit_text);

            _textInputEditText.SetTextChangeListener(sequence => _textInputEditText.MoveCursorBeforeSymbol(Core.Localization.Resources.Percent, sequence));
        }

        public override void BindData()
        {
            using var bindingSet = this.CreateBindingSet<PlaceTableParticipantsItemViewHolder, PlaceTableParticipantsItemViewModel>();

            bindingSet.Bind(_textInputLayout).For(v => v.Hint).To(vm => vm.Title);
            bindingSet.Bind(_textInputEditText).For(v => v.Text).To(vm => vm.Percent)
                .WithConversion<PercentConverter>();
        }
    }
}