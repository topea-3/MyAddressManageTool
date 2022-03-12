using MyAddressManageTool.TableManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyAddressManageTool.View
{
    /// <summary>
    /// DbConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DbConfigWindow : Window
    {
        public DbConfigWindow()
        {
            InitializeComponent();
        }

        private void OpenFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "MicrosoftAccess (.accdb)|*.accdb";
            dialog.Title = "Open File dialog";
            dialog.DefaultExt = ".accdb";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                AccessFullPathTextBox.Text = dialog.FileName;
            }
        }

        private void ShutdownApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult=false;
        }

        private void DbSettingUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionManager.UpdateAccessDbPath(AccessFullPathTextBox.Text);
            if (TransactionManager.IsAccessDbPathEffective())
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("指定したファイルが有効ではありません","無効のファイル",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
