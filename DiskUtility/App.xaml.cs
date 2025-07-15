using DiskUtility.Services.Root;
using DiskUtility.WindowsAPI.ComTypes;
using System;
using System.Diagnostics.Tracing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;

namespace DiskUtility
{
    /// <summary>
    /// 磁盘工具应用程序
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private bool isDisposed;
        private WindowsXamlManager windowXamlManager;

        public App()
        {
            windowXamlManager = WindowsXamlManager.InitializeForCurrentThread();
            (Window.Current as object as IXamlSourceTransparency).SetIsBackgroundTransparent(true);
            InitializeComponent();
            LoadComponent(this, new Uri("ms-appx:///App.xaml"), ComponentResourceLocation.Application);
            UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// 处理应用程序未知异常处理
        /// </summary>
        private void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs args)
        {
            LogService.WriteLog(EventLevel.Warning, nameof(DiskUtility), nameof(App), nameof(OnUnhandledException), 1, args.Exception);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~App()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
                if (disposing)
                {
                    windowXamlManager.Dispose();
                    windowXamlManager = null;
                    LogService.CloseLog();
                    System.Windows.Forms.Application.Exit();
                }
            }
        }
    }
}
