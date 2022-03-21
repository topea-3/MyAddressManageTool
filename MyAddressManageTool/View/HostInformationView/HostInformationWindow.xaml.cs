using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.Core.ViewModel.Implement;
using MyAddressManageTool.Model;
using MyAddressManageTool.MyApi;
using MyAddressManageTool.TableManager;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MyAddressManageTool.View.HostInformationView
{
    /// <summary>
    /// HostInformationWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class HostInformationWindow : Window
    {
        /// <summary>
        /// モードタイプEnum
        /// </summary>
        public enum ModeType
        {
            Show,
            Register,
            Change,
            Delete
        }

        // 画面ID
        private const string VIEW_ID = "V0001";
        // タイプ値
        private const string DELETE_FLAG_NORMAL = "0";
        private const string DELETE_FLAG_DELETE = "1";
        // モードタイプ
        private ModeType modeType;

        // ホスト情報
        private HostInformation hostInformation;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="argHostInformation">引継ホスト情報</param>
        /// <param name="argModeType">モードタイプ</param>
        public HostInformationWindow(HostInformation? argHostInformation, ModeType argModeType)
        {
            // 引継情報の保持
            modeType = argModeType;
            if(null == argHostInformation)
            {
                hostInformation = new HostInformation();
            }
            else
            {
                hostInformation = argHostInformation;
            }

            // 画面初期化
            InitializeComponent();
            DataContext = hostInformation;

            // 画面プロパティ調整
            switch (modeType)
            {
                case ModeType.Register:
                    RegisterModeRadioButton.IsChecked = true;
                    SetupRegisterMode();
                    break;
                case ModeType.Delete:
                    DeleteModeRadioButton.IsChecked = true;
                    SetupDeleteMode();
                    break;
                case ModeType.Change:
                    ChangeModeRadioButton.IsChecked = true;
                    SetupChangeMode();
                    break;
                case ModeType.Show:
                    ShowModeRadioButton.IsChecked = true;
                    SetupShowMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // 確認メッセージ表示
            MessageBoxResult result = MessageBox.Show("入力値をクリアしますか？", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Cancel)
            {
                // キャンセルした場合何もしない
                return;
            }

            // 入力項目のクリア
            hostInformation.HostName = null;
            hostInformation.FamilyName = null;
            hostInformation.Name = null;
            hostInformation.SubName1 = null;
            hostInformation.SubName2 = null;
            hostInformation.SubName3 = null;
            hostInformation.SubName4 = null;
            hostInformation.SubName5 = null;
            hostInformation.AddressNumber1 = null;
            hostInformation.AddressNumber2 = null;
            hostInformation.Address1 = null;
            hostInformation.Address2 = null;
            hostInformation.Remarks = null;

            ErrorImformationList.ItemsSource = null;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            switch (modeType)
            {
                case ModeType.Register:
                    RegisterExec();
                    break;
                case ModeType.Delete:
                    DeleteExecute();
                    break;
                case ModeType.Change:
                    ChangeExecute();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        private void ShowModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            modeType = ModeType.Show;
            SetupShowMode();
        }

        private void RegisterModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // 確認メッセージ表示
            if (ModeType.Register != modeType)
            {
                MessageBoxResult result = MessageBox.Show("情報が失われます。よろしいですか？", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Cancel)
                {
                    // キャンセルの場合、復元
                    RegisterModeRadioButton.IsChecked = false;
                    switch (modeType)
                    {
                        case ModeType.Delete:
                            DeleteModeRadioButton.IsChecked = true;
                            break;
                        case ModeType.Change:
                            ChangeModeRadioButton.IsChecked = true;
                            break;
                        case ModeType.Show:
                            ShowModeRadioButton.IsChecked = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("mode");
                    }
                    return;
                }
            }
            // モード設定
            modeType = ModeType.Register;

            // 画面情報クリア
            hostInformation.HostId = null;
            hostInformation.SeqNo = 1;
            hostInformation.HostName = null;
            hostInformation.DeleteFlag = DELETE_FLAG_NORMAL;
            hostInformation.CreateDateTime = null;
            hostInformation.UpdateDateTime = null;

            // モードセットアップ
            SetupRegisterMode();
        }

        private void ChangeModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            modeType = ModeType.Change;
            SetupChangeMode();
        }

        private void DeleteModeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            modeType = ModeType.Delete;
            SetupDeleteMode();
        }

        /// <summary>
        /// 入力項目読み取り専用化
        /// </summary>
        private void ChangeReadOnlyAllInputItem()
        {
            HostNameTextBox.IsReadOnly = true;
            FamilyNameTextBox.IsReadOnly = true;
            NameTextBox.IsReadOnly = true;
            SubName1TextBox.IsReadOnly = true;
            SubName2TextBox.IsReadOnly = true; 
            SubName3TextBox.IsReadOnly= true;
            SubName4TextBox.IsReadOnly = true;
            SubName5TextBox.IsReadOnly = true;
            AddressNumber1TextBox.IsReadOnly = true;
            AddressNumber2TextBox.IsReadOnly = true;
            Address1TextBox.IsReadOnly = true;
            Address2TextBox.IsReadOnly = true;
            RemarksTextBox.IsReadOnly = true;
        }

        /// <summary>
        /// 入力項目読み取り専用解除
        /// </summary>
        private void ActivateAllInputItem()
        {
            HostNameTextBox.IsReadOnly = false;
            FamilyNameTextBox.IsReadOnly = false;
            NameTextBox.IsReadOnly = false;
            SubName1TextBox.IsReadOnly = false;
            SubName2TextBox.IsReadOnly = false;
            SubName3TextBox.IsReadOnly = false;
            SubName4TextBox.IsReadOnly = false;
            SubName5TextBox.IsReadOnly = false;
            AddressNumber1TextBox.IsReadOnly = false;
            AddressNumber2TextBox.IsReadOnly = false;
            Address1TextBox.IsReadOnly = false;
            Address2TextBox.IsReadOnly = false;
            RemarksTextBox.IsReadOnly = false;
        }

        /// <summary>
        /// 登録モードセットアップ
        /// </summary>
        private void SetupRegisterMode()
        {
            // タイトル変更
            TiltleLabel.Content = "登録";

            // ラジオボタン非活性化
            ShowModeRadioButton.IsEnabled = false;
            ChangeModeRadioButton.IsEnabled = false;
            DeleteModeRadioButton.IsEnabled = false;

            // 入力項目読み取り専用解除
            ActivateAllInputItem();

            // アクションボタン設定
            ActionButton.Visibility = Visibility.Visible;
            ActionButton.Content = "登録";

            // クリアボタン設定
            ClearButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 参照モードセットアップ
        /// </summary>
        private void SetupShowMode()
        {
            // タイトル変更
            TiltleLabel.Content = "参照";

            // 入力項目読取専用化
            ChangeReadOnlyAllInputItem();

            // アクションボタン設定
            ActionButton.Visibility = Visibility.Hidden;

            // クリアボタン設定
            ClearButton.Visibility = Visibility.Hidden;

            // データ取得
            // トランザクション制御開始
            TransactionManager transaction = new();
            transaction.StartTransaction();
            HostInformationUnitInquiry model = new(transaction);
            bool isCommited = false;

            try
            {
                // 取得処理
                HostInformation currentHostInformation = model.GetHostInformation(hostInformation.HostId, hostInformation.SeqNo);
                transaction.Commit();
                isCommited = true;

                // 画面表示更新
                ErrorImformationList.ItemsSource = null;
                PropertiesCopyUtil.CopyProperties(currentHostInformation, ref hostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
                
                if (DELETE_FLAG_DELETE == hostInformation.DeleteFlag)
                {
                    // ラジオボタン非活性化
                    ChangeModeRadioButton.IsEnabled = false;
                    DeleteModeRadioButton.IsEnabled = false;
                }
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
        /// 変更モードセットアップ
        /// </summary>
        private void SetupChangeMode()
        {
            // タイトル変更
            TiltleLabel.Content = "変更";

            // 入力項目読み取り専用解除
            ActivateAllInputItem();

            // アクションボタン設定
            ActionButton.Visibility = Visibility.Visible;
            ActionButton.Content = "変更";

            // クリアボタン設定
            ClearButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 削除モードセットアップ
        /// </summary>
        private void SetupDeleteMode()
        {
            // タイトル変更
            TiltleLabel.Content = "削除";

            // 入力項目読取専用化
            ChangeReadOnlyAllInputItem();

            // アクションボタン設定
            ActionButton.Visibility = Visibility.Visible;
            ActionButton.Content = "削除";

            // クリアボタン設定
            ClearButton.Visibility = Visibility.Hidden;

            // データ取得
            // トランザクション制御開始
            TransactionManager transaction = new();
            transaction.StartTransaction();
            HostInformationUnitInquiry model = new(transaction);
            bool isCommited = false;

            try
            {
                // 取得処理
                HostInformation currentHostInformation = model.GetHostInformation(hostInformation.HostId, hostInformation.SeqNo);
                transaction.Commit();
                isCommited = true;

                // 画面表示更新
                ErrorImformationList.ItemsSource = null;
                PropertiesCopyUtil.CopyProperties(currentHostInformation, ref hostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);

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
        /// 登録処理
        /// </summary>
        private void RegisterExec()
        {
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

                // 画面情報全クリア
                hostInformation.HostId = null;
                hostInformation.SeqNo = 1;
                hostInformation.HostName = null;
                hostInformation.CreateDateTime = null;
                hostInformation.UpdateDateTime = null;
                hostInformation.HostName = null;
                hostInformation.FamilyName = null;
                hostInformation.Name = null;
                hostInformation.SubName1 = null;
                hostInformation.SubName2 = null;
                hostInformation.SubName3 = null;
                hostInformation.SubName4 = null;
                hostInformation.SubName5 = null;
                hostInformation.AddressNumber1 = null;
                hostInformation.AddressNumber2 = null;
                hostInformation.Address1 = null;
                hostInformation.Address2 = null;
                hostInformation.Remarks = null;
                ErrorImformationList.ItemsSource = null;
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

        // 変更処理
        private void ChangeExecute()
        {
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
            HostInformationChange model = new(transaction);
            bool isCommited = false;

            try
            {
                HostInformation returnHostInformation = model.ExcecuteChange(hostInformation);
                transaction.Commit();
                isCommited = true;

                // 完了メッセージ表示
                _ = MessageBox.Show("変更完了しました。", "Information", MessageBoxButton.OK, MessageBoxImage.None);

                // 画面情報の更新
                ErrorImformationList.ItemsSource = null;
                PropertiesCopyUtil.CopyProperties(returnHostInformation, ref hostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
                ShowModeRadioButton.IsChecked = true;
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
        // 変更処理
        private void DeleteExecute()
        {
            MessageBoxResult result = MessageBox.Show("削除します。よろしいですか？", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }

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
            HostInformationDelete model = new(transaction);
            bool isCommited = false;

            try
            {
                HostInformation returnHostInformation = model.ExecuteDelete(hostInformation);
                transaction.Commit();
                isCommited = true;

                // 完了メッセージ表示
                _ = MessageBox.Show("削除完了しました。", "Information", MessageBoxButton.OK, MessageBoxImage.None);

                // 画面情報の更新
                ErrorImformationList.ItemsSource = null;
                PropertiesCopyUtil.CopyProperties(returnHostInformation, ref hostInformation, PropertiesCopyUtil.CopyType.NullEmptyOverride);
                ShowModeRadioButton.IsChecked = true;
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


    }
}
