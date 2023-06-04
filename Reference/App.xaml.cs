using System;
using VADisabilityCalculator;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VADisabilityCalculator
{
    public partial class App : Application
    {
        public App()
        {
            
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjI5NTgzMkAzMjMxMmUzMTJlMzMzNU8wTENuZjQvRHlJUEVrM3IzQVllTHJNTkRKU2NQK1orM1BDT1A2VVdHUmc9");                
            
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
