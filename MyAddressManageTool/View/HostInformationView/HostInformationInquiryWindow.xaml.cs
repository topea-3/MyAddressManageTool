using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Model;
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

namespace MyAddressManageTool.View.HostInformationView
{
    /// <summary>
    /// HostInformationInquiryWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HostInformationInquiryWindow : Window
    {
        private HostInfoSearchCondition condition;

        public HostInformationInquiryWindow()
        {
            InitializeComponent();

            // 検索条件インスタンス化
            condition = new();
            // バインディング設定
            DataContext = condition;
            HostInformationInquiryData.DataContext = condition;
            // 履歴カラム非表示
            HistoryColumn.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 検索実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // Validationチェック なし

            // トランザクション制御開始
            TransactionManager transaction = new();
            transaction.StartTransaction();
            HostInformationInquiry model = new(transaction);
            bool isCommited = false;

            try
            {
                // 事前チェック
                model.ApplicationArgCheck(condition);
                // データ取得
                IList<Inquiry4HostInformation> datas = model.Search(condition);
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

        /// <summary>
        /// 登録画面起動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoRegisterPageButton_Click(object sender, RoutedEventArgs e)
        {
            HostInformation hostInformation = new HostInformation();
            HostInformationWindow window = new HostInformationWindow(null, HostInformationWindow.ModeType.Register);
            window.Show();
        }

        /// <summary>
        /// データセレクトイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HostInformationInquiryData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Inquiry4HostInformation? selectedItem = HostInformationInquiryData.SelectedItem as Inquiry4HostInformation;

            if (selectedItem == null)
            {
                // 選択アイテムがなしの場合処理終了
                return;
            }

            // 連携データセットアップ
            HostInformation argHostInformation = new();
            argHostInformation.HostId = selectedItem.HostId;
            argHostInformation.SeqNo = selectedItem.SeqNo;

            // ホスト情報画面を参照モードで起動
            HostInformationWindow window = new HostInformationWindow(argHostInformation, HostInformationWindow.ModeType.Show);
            window.Show();
        }
    }
}
