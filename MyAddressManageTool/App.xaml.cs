using MyAddressManageTool.TableManager;
using MyAddressManageTool.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using MyAddressManageTool.MyApi;

namespace MyAddressManageTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex? mutex;

        /// <summary>
        /// アプリケーションの開始時ロジック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 多重起動防止ロジック
            // mutex作成
            mutex = new(false, "MutexGhostTTMyAdressManagerToolApplication");

            // mutexの所有権を要求
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("このアプリケーションは多重起動できません。",
                    "Error Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                // mutexの解放
                if (null != mutex)
                {
                    mutex.Close();
                    mutex = null;
                }

                Shutdown();
            }
        }

        /// <summary>
        /// アプリケーションの想定外例外ハンドリング
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // エラー情報の出力
            string title = e.Exception.GetType().Name;
            string message = e.Exception.Message + Environment.NewLine + e.Exception.StackTrace;
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

            // mutex解放
            if (null != mutex)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }

            // エラーハンドルフラグ更新
            e.Handled = true;
            Shutdown();
        }

        /// <summary>
        /// アプリケーションの終了時ロジック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // mutexの解放
            if (null != mutex)
            {
                mutex.ReleaseMutex();
                mutex.Close();
                mutex = null;
            }
        }
    }
}
