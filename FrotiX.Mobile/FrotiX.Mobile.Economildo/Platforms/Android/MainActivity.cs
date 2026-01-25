using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit; // Importa o namespace necessário para o WebView

namespace FrotiX.Economildo
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                                ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
            //    SystemUiFlags.HideNavigation |
            //    SystemUiFlags.Fullscreen |
            //    SystemUiFlags.ImmersiveSticky
            //);

#if DEBUG
            // Habilita a depuração remota do WebView quando em modo DEBUG
            Android.Webkit.WebView.SetWebContentsDebuggingEnabled(true);
#endif
        }
    }
}