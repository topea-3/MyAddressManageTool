using MyAddressManageTool.TableManager;
using MyAddressManageTool.Test;
using MyAddressManageTool.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyAddressManageTool.View.HostInformationView;

namespace MyAddressManageTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DB設定チェック
            if (!TransactionManager.IsAccessDbPathEffective())
            {
                // 有効な設定がない場合、設定ダイアログ表示
                DbConfigWindow dbConfigWindow = new DbConfigWindow();
                bool? daialogResult = dbConfigWindow.ShowDialog();
                
                if (!daialogResult ?? throw new ApplicationException("想定外のNULL参照が発生しました。"))
                {
                    Application.Current.Shutdown();
                }
            }

        }

        private void ExecuteTest(object sender, RoutedEventArgs e)
        {
            // TestClass.testExec();
            ContentsWindow contentsWindow = new("HostInformationView/HostInformationInquiryPage.xaml");
            contentsWindow.Show();
        }
    }
}
