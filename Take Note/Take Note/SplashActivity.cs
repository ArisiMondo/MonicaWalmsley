namespace Take_Note
{
    using System.Threading;

    using Android.App;
    using Android.OS;

    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle); //-----------------Declares as actions on opening of app
            Thread.Sleep(1000); //--------------------Pauses thread to show splash screen
            StartActivity(typeof(MainActivity)); //---Opens the main app
        }
    }
}