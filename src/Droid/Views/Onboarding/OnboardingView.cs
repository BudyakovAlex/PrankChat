using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content.Resources;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Onboarding;
using PrankChat.Mobile.Core.ViewModels.Onboarding.Items;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Onboarding;
using PrankChat.Mobile.Droid.Listeners;
using PrankChat.Mobile.Droid.Views.Base;
using PrankChat.Mobile.Droid.Utils.Helpers;
using System;
using System.Collections.Generic;

namespace PrankChat.Mobile.Droid.Views.Onboarding
{
    [MvxActivityPresentation]
    [Activity(
        LaunchMode = LaunchMode.SingleTop,
        Theme = "@style/Theme.PrankChat.OnBoarding",
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class OnboardingView : BaseView<OnboardingViewModel>
    {
        private readonly List<IndicatorView> _indicatorViews = new List<IndicatorView>();

        private MvxRecyclerView _recyclerView;
        private LinearLayout _indicatorLinearLayout;
        private MaterialButton _actionButton;
        private LinearLayoutManager _layoutManager;

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                CountChanged(_count);
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;

                if (_selectedIndex != _layoutManager.FindFirstCompletelyVisibleItemPosition())
                {
                    _recyclerView.SmoothScrollToPosition(_selectedIndex);
                }

                for (var i = 0; i < _indicatorViews.Count; i++)
                {
                    var backgroundId = GetIndicatorBackgroundId(i, _selectedIndex);
                    _indicatorViews[i].Indicator.SetBackgroundResource(backgroundId);
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            var colorArgb = ResourcesCompat.GetColor(Resources, Resource.Color.deep_purple, null);
            var color = new Color(colorArgb);
            Window.SetStatusBarColor(color);

            OnCreate(bundle, Resource.Layout.activity_onboarding);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.recycler_view);
            _indicatorLinearLayout = FindViewById<LinearLayout>(Resource.Id.indicator_linear_layout);
            _actionButton = FindViewById<MaterialButton>(Resource.Id.action_button);

            _layoutManager = new LinearLayoutManager(this, RecyclerView.Horizontal, false);
            _recyclerView.SetLayoutManager(_layoutManager);
            _recyclerView.Adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OnboardingItemViewModel, OnboardingItemViewHolder>(Resource.Layout.cell_onboarding);
            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_recyclerView);

            _recyclerView.AddOnScrollListener(new RecyclerViewScrollListener(OnScrollChanged));
        }

        private void OnScrollChanged(RecyclerView recyclerView, int dx, int dy)
        {
            if (ViewModel is null)
            {
                return;
            }

            ViewModel.SelectedIndex = dx < 0
                ? _layoutManager.FindFirstVisibleItemPosition()
                : _layoutManager.FindLastVisibleItemPosition();
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<OnboardingView, OnboardingViewModel>();

            bindingSet.Bind(this).For(v => v.Count).To(vm => vm.ItemsCount);
            bindingSet.Bind(this).For(v => v.SelectedIndex).To(vm => vm.SelectedIndex);
            bindingSet.Bind(_recyclerView).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_actionButton).For(v => v.Text).To(vm => vm.ActionTitle);
            bindingSet.Bind(_actionButton).For(v => v.BindClick()).To(vm => vm.ActionCommand);
        }

        private void CountChanged(int count)
        {
            _indicatorLinearLayout.RemoveAllViews();
            _indicatorViews.Clear();

            for (int i = 0; i < count; i++)
            {
                var indicatorView = new IndicatorView(this);
                _indicatorViews.Add(indicatorView);
                _indicatorLinearLayout.AddView(indicatorView);
            }
        }

        private int GetIndicatorBackgroundId(int index, int currentIndex)
        {
            if (index > currentIndex)
            {
                return Android.Resource.Color.Transparent;
            }

            return index == currentIndex
                ? Resource.Drawable.bg_competition_orange
                : Resource.Color.competition_new_border;
        }

        private class IndicatorView : FrameLayout
        {
            public IndicatorView(Context context)
                : base(context)
            {
                Initialize(context);
            }

            protected IndicatorView(IntPtr javaReference, JniHandleOwnership transfer)
                : base(javaReference, transfer)
            {
            }

            public View Indicator { get; private set; }

            private void Initialize(Context context)
            {
                var size = DisplayUtils.DpToPx(20);
                var margin = DisplayUtils.DpToPx(7);

                LayoutParameters = new MarginLayoutParams(size, size)
                {
                    MarginStart = margin,
                    MarginEnd = margin,
                    
                };
                SetBackgroundResource(Resource.Drawable.bg_onboarding_indicator);

                InitializeIndicator(context);
            }

            private void InitializeIndicator(Context context)
            {
                var size = DisplayUtils.DpToPx(16);

                Indicator = new View(context);
                Indicator.LayoutParameters = new LayoutParams(size, size, GravityFlags.Center);
                Indicator.SetBackgroundColor(Color.Transparent);
                Indicator.SetRoundedCorners(size / 2f);

                AddView(Indicator);
            }
        }
    }
}
