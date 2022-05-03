using MyAddressManageTool.MyApi;
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
