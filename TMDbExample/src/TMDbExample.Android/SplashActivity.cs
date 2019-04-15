using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace TMDbExample.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        private void Startup()
        {
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}