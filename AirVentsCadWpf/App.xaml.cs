using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace AirVentsCadWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}
//namespace AirVentsCadWpf
//{
//    /// <summary>
//    /// Interaction logic for App.xaml
//    /// </summary>
//    public partial class App
//    {
//        // logger.Debug("Hello World!");

//    //#region Nlog

//    //    private ILogger _logger;// = new NLogLogger();

       

//    //    protected void OnStartup(StartupEventArgs e)
//    //    {
//    //       // Boot();

//    //        ConfigureLogger();
            
//    //        _logger.Info("Application starting.");

//    //        // Logging
//    //        Current.DispatcherUnhandledException += (sender, args) => _logger.Error(args.Exception);

//    //        //var window = new MainWindow().WithModel(ServiceLocator.GetInstance<MainWindowViewModel>());
//    //        //window.Show();
//    //    }

//    //    protected override void OnExit(ExitEventArgs e)
//    //    {
//    //        _logger.Info("Application exiting.");
//    //        base.OnExit(e);
//    //    }

    

//    //    public interface ILogger
//    //    {
//    //        void Info(string message);
//    //        void Warn(string message);
//    //        void Debug(string message);
//    //        void Error(string message);
//    //        void Error(Exception x);
//    //        void Fatal(string message);
//    //        void Fatal(Exception x);
//    //    }

//    //    public class NLogLogger : ILogger
//    //    {
//    //        private readonly Logger _logger;

//    //        public NLogLogger() { _logger = LogManager.GetCurrentClassLogger(); }

//    //        public void Info(string message) { _logger.Info(message); }

//    //        public void Warn(string message) { _logger.Warn(message); }

//    //        public void Debug(string message) { _logger.Debug(message); }

//    //        public void Error(string message) { _logger.Error(message); }

//    //        public void Error(Exception x) { _logger.Error(this.BuildExceptionMessage(x)); }

//    //        public void Fatal(string message) { _logger.Fatal(message); }

//    //        public void Fatal(Exception x) { _logger.Fatal(this.BuildExceptionMessage(x)); }
//    //    }

//    //    public static class LoggerExt
//    //    {
//    //        public static string BuildExceptionMessage(MainWindow.ILogger logger, Exception x)
//    //        {
//    //            Exception logException = x;
//    //            if (x.InnerException != null)
//    //                logException = x.InnerException;

//    //            string strErrorMsg = Environment.NewLine + "Message :" + logException.Message;

//    //            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

//    //            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

//    //            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
//    //            return strErrorMsg;
//    //        }
//    //    }

//    //    public static string GetUserAppDataPath()
//    //    {
//    //        var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
//    //            "YourCompany\\YourApp");
//    //        return logPath;
//    //    }

//    //    public static void ConfigureLogger()
//    //    {
//    //        // Step 1. Create configuration object
//    //        LoggingConfiguration config = new LoggingConfiguration();

//    //        FileTarget fileTarget = new FileTarget();
//    //        config.AddTarget("file", fileTarget);

//    //        // Step 3. Set target properties

//    //        var logPath = CommonUtils.GetUserAppDataPath();

//    //        fileTarget.FileName = Path.Combine(logPath, "MyApp.log");
//    //        fileTarget.ArchiveFileName = Path.Combine(logPath, "MyApp.{#####}.txt");
//    //        fileTarget.ArchiveAboveSize = 10240; // 10kb
//    //        fileTarget.ArchiveNumbering = ArchiveNumberingMode.Sequence;
//    //        fileTarget.ConcurrentWrites = true;
//    //        fileTarget.KeepFileOpen = false;

//    //        fileTarget.Layout = "${longdate} | ${level} | ${message}";

//    //        LoggingRule rule2 = new LoggingRule("*", LogLevel.Info, fileTarget);
//    //        config.LoggingRules.Add(rule2);

//    //        // Step 5. Activate the configuration
//    //        LogManager.Configuration = config;
//    //    }

//    //    #endregion
//    }
//}
