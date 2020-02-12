﻿using System;
using Prism.Behaviors;
using Prism.Common;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;

namespace Prism.Plugin.Popups
{
#if __IOS__
    [Foundation.Preserve(AllMembers = true)]
#elif __ANDROID__
    [Android.Runtime.Preserve(AllMembers = true)]
#endif
    public class PopupPageBehaviorFactory : PageBehaviorFactory
    {
        IPopupNavigation _popupNavigation { get; }
        IApplicationProvider _applicationProvider { get; }

        public PopupPageBehaviorFactory(IPopupNavigation popupNavigation, IApplicationProvider applicationProvider)
        {
            _popupNavigation = popupNavigation;
            _applicationProvider = applicationProvider;
        }

        public override void ApplyPageBehaviors(Xamarin.Forms.Page page)
        {
            base.ApplyPageBehaviors(page);
            if (page is PopupPage popupPage)
            {
                popupPage.Behaviors.Add(new BackgroundPopupDismissalBehavior(_popupNavigation, _applicationProvider));
            }
        }
    }
}
