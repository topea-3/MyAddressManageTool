using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager
{
    internal class TransactionManager
    {
        // Access拡張子
        private const string ACCESS_EXTENTION = ".accdb";

        // 内部保持接続文字列
        private string connectionString = "";

        // トランザクションプロパティ
        private OleDbTransaction? transaction = null;
        public OleDbTransaction Transaction => transaction ?? throw new ArgumentNullException("transaction");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TransactionManager()
        {
            if (IsAccessDbPathEffective())
            {
                // Accessのファイルパスが有効な場合接続文字列を設定する
                MakeupConnectionString();
            }
        }

        /// <summary>
        /// Access DB ファイルパスの設定更新
        /// </summary>
        /// <param name="path"></param>
        public static void UpdateAccessDbPath(string path)
        {
            ApplicationSetting.Default.AccessDbPath = path;
            ApplicationSetting.Default.Save();
        }

        /// <summary>
        /// 設定されているAccessファイルパスの有効判定
        /// </summary>
        /// <returns>有効：True</returns>
        public static bool IsAccessDbPathEffective()
        {
            string path = ApplicationSetting.Default.AccessDbPath;

            // 未設定チェック
            if (path.Trim().Length == 0)
            {
                return false;
            }

            // ファイル存在チェック
            if (File.Exists(path))
            {
                // 拡張子チェック
                return ACCESS_EXTENTION.Equals(Path.GetExtension(path).ToLower());
            }

            return false;
        }

        /// <summary>
        /// トランザクションの開始
        /// </summary>
        public void StartTransaction()
        {
            OleDbConnection connection = new(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();

        }

        /// <summary>
        /// トランザクションのコミット
        /// </summary>
        public void Commit()
        {
            transaction?.Commit();
        }

        /// <summary>
        /// トランザクションのロールバック
        /// </summary>
        public void Rollback()
        {
            transaction?.Rollback();
        }

        /// <summary>
        /// トランザクションの終了とリソースのクローズ
        /// </summary>
        public void EndTransaction()
        {
            transaction?.Connection?.Close();
            transaction?.Connection?.Dispose();
            transaction?.Dispose();

        }

        /// <summary>
        /// トランザクションの終了とリソースのクローズ
        /// </summary>
        /// <param name="dataReaders"></param>
        public void EndTransaction(OleDbDataReader[] dataReaders)
        {
            foreach (var dataReader in dataReaders)
            {
                dataReader?.Close();
                dataReader?.Dispose();
            }
            EndTransaction();
        }

        /// <summary>
        /// トランザクションの終了とリソースのクローズ
        /// </summary>
        /// <param name="dataAdapters"></param>
        public void EndTransaction(OleDbDataAdapter[] dataAdapters)
        {
            foreach(var dataAdapter in dataAdapters)
            {
                dataAdapter?.Dispose();
            }
            EndTransaction();
        }

        /// <summary>
        /// トランザクションの終了とリソースのクローズ
        /// </summary>
        /// <param name="dataAdapters"></param>
        /// <param name="dataReaders"></param>
        public void EndTransaction(OleDbDataAdapter[] dataAdapters, OleDbDataReader[] dataReaders)
        {
            foreach (var dataAdapter in dataAdapters)
            {
                dataAdapter?.Dispose();
            }
            EndTransaction(dataReaders);
        }

        /// <summary>
        /// 接続文字列設定
        /// </summary>
        /// <exception cref="FileNotFoundException">有効なAccessDBのパスが設定されていない</exception>
        private void MakeupConnectionString()
        {
            string accessDbPath = ApplicationSetting.Default.AccessDbPath;

            if (!IsAccessDbPathEffective())
            {
                // 有効なAccessDBのパスが設定されていない
                throw new FileNotFoundException(accessDbPath);
            }

            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = "Microsoft.ACE.OLEDB.12.0";
            builder.DataSource = accessDbPath;

            this.connectionString = builder.ConnectionString;
        }
    }
}
