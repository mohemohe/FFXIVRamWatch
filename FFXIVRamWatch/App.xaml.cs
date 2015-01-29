using FFXIVRamWatch.Models;
using FFXIVRamWatch.Views;
using Livet;
using System;
using System.Windows;

namespace FFXIVRamWatch
{
    /// <summary>
    ///     App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Config.Initialize();

            var mainWindow = new MainWindow();
            mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            mainWindow.Top = Config.WindowPos.Y;
            mainWindow.Left = Config.WindowPos.X;
            mainWindow.Topmost = true;
            mainWindow.ShowActivated = true;

            mainWindow.Show();
        }

        //集約エラーハンドラ
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TODO:ロギング処理など
            MessageBox.Show(
                "不明なエラーが発生しました。アプリケーションを終了します。",
                "エラー",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Environment.Exit(1);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Config.WriteSettings();
        }
    }
}