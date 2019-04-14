﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Globalization;
using TMDbExample.Forms.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TMDbExample.Forms
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new UpcomingMoviesPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            base.OnResume();
            var defaultCulture = new CultureInfo("en-US");
            SetCultureInfo(defaultCulture);
        }

        private void SetCultureInfo(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
