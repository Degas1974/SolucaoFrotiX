using System.Configuration;
using System.Data;
using System.Windows;
using Syncfusion.Licensing;

namespace DeepSeekIDE.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public App()
    {
        // Registrar licença Syncfusion v32
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfeHRSRGhcVUVyWkpWYEg=");
    }
}
