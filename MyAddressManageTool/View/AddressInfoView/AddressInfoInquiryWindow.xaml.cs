using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Core.ViewModel.Implement;
using MyAddressManageTool.Model;
using MyAddressManageTool.MyApi;
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

namespace MyAddressManageTool.View.AddressInfoView
{
    /// <summary>
    /// AddressInfoInquiryWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AddressInfoInquiryWindow : Window
    {
        // 画面ID
        private const string VIEW_ID = "V0002";

        private AddressInfoSearchCondition condition;

        public AddressInfoInquiryWindow()
        {
            InitializeComponent();
            // コンボボックス設定
            NyLetterCreateFlagComboBox.ItemsSource = TypeManager.GetTypeDictByIdWithBlank("NY_LETTER_CREATE");
            //検索条件インスタンス化
            condition = new();
            // バインディング設定
            DataContext = condition;
            AddressInfoInquiryData.DataContext = condition;
            // 履歴カラム非表示
            HistoryColumn.Visibility = Visibility.Hidden;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // 検索時間設定
            condition.SearchDateTime = DateTime.Now.ToString();

            // 履歴カラム表示制御
            if (condition.IsHistoryView)
            {
                HistoryColumn.Visibility = Visibility.Visible;
            }
            else
            {
                HistoryColumn.Visibility = Visibility.Hidden;
            }

            // Validationチェック
            // Validationチェック
            MyValidation validation = new();
            validation.ExecuteValidate(VIEW_ID, condition);
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
            AddressInfoInquiry model = new(transaction);
            bool isCommited = false;

            try
            {
                // 事前チェック
                model.ApplicationArgCheck(condition);
                // データ取得
                IList<Inquiry4AddressInformation> datas = model.Search(condition);
                // 検索結果件数反映
                condition.DataCount = datas.Count.ToString();
                // 検索結果反映
                condition.Results = datas;
                //  コミット
                transaction.Commit();
                isCommited = true;
            }
            catch (MyApplicationException ex)
            {
                IList<string> error = new List<string>();
                error.Add(ex.Message);
                ErrorImformationList.ItemsSource = error;
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

        private void GoRegisterPageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddressInfoInquiryData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
