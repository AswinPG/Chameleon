﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Chameleon.Core;
using Chameleon.Core.ViewModels;
using MediaManager;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Navigation;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Intent = global::Android.Content.Intent;

namespace Chameleon.Android
{
    [Activity(
        Label = "@string/app_name",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme.Launcher",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend, Intent.ActionSendMultiple, Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable, Intent.CategoryAppMusic }, DataMimeTypes = new[] { "video/*", "audio/*" }, Label = "@string/app_name")]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, Core.App, FormsApp>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Forms.SetFlags("CollectionView_Experimental");
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            TabLayoutResource = Android.Resource.Layout.Tabbar;
            ToolbarResource = Android.Resource.Layout.Toolbar;
            SetTheme(Android.Resource.Style.MainTheme);

            base.OnCreate(savedInstanceState);

            CrossMediaManager.Current.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            HandleIntent();
        }

        private async void HandleIntent()
        {
            if (await CrossMediaManager.Android.PlayFromIntent(Intent))
            {
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<PlayerViewModel>();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
