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
using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Core.Message;
using MyAddressManageTool.Core.ViewModel.Implement;
using MyAddressManageTool.TableManager;
using MyAddressManageTool.Model;

namespace MyAddressManageTool.View.HostInformationView
{
    /// <summary>
    /// HostInformationRegisterPage.xaml の相互作用ロジック
    /// 画面ID：V0001
    /// </summary>
    public partial class HostInformationRegisterPage : Page
    {
        private const string VIEW_ID = "V0001";

        public HostInformationRegisterPage()
        {
            InitializeComponent();
            DataContext = new HostInformation();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // 画面情報取得
            HostInformation hostInformation = (HostInformation)DataContext;

            // Validationチェック
            MyValidation validation = new();
            validation.ExecuteValidate(VIEW_ID, hostInformation);
            IList<string> validationErros = validation.GetResults();

            if (validationErros.Count > 0)
            {
                ErrorImformationList.ItemsSource = validationErros;
                UpdateLayout();
                return;
            }

            // トランザクション制御開始
            TransactionManager transaction = new();
            transaction.StartTransaction();
            HostInformationRegister register = new(transaction);
            bool isCommited = false;

            try
            {
                // 登録事前チェック
                register.ApplicationArgCheck(hostInformation);

                // 登録処理
                register.Register(hostInformation);
                transaction.Commit();
                isCommited = true;

                // 完了メッセージ表示
                _ = MessageBox.Show("登録完了しました。", "Information", MessageBoxButton.OK, MessageBoxImage.None);

                // 画面項目初期化
                NavigationService.Refresh();
            }
            catch (MyApplicationException ex)
            {
                IList<string> error = new List<string>();
                error.Add(ex.Message);
                ErrorImformationList.ItemsSource=error;
            }
            finally
            {
                if (!isCommited)
                {
                    transaction.Rollback();
                }
                transaction.EndTransaction();
            }
        }
    }
}
