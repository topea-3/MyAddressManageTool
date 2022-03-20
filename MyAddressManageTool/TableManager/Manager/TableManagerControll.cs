using MyAddressManageTool.Core.ExceptionManage;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Mapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyAddressManageTool.TableManager.Manager
{
    /// <summary>
    /// テーブルマネージャー
    /// </summary>
    /// <typeparam name="T">エンティティ型</typeparam>
    internal class TableManagerControll<T> where T : class
    {
        // スペース
        private const string SPACE = " ";
        // カンマ
        private const string SEPERATER = ",";
        // SQLのパラメータ文字
        private const string SQL_PARAMETER_MARKER = " ? ";

        // トランザクション
        private readonly OleDbTransaction transaction;
        // マッパー
        private readonly IMapper mapper;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbTransaction"></param>
        public TableManagerControll(IMapper mapper_, OleDbTransaction dbTransaction)
        {
            this.mapper = mapper_;
            transaction = dbTransaction;
        }

        /// <summary>
        /// INSERTメソッド
        /// </summary>
        /// <param name="argEntity"></param>
        public void Insert(T argEntity)
        {
            IList<object> parameterList = new List<object>();

            StringBuilder sql = new($"insert into {mapper.GetTableName()} values (");
            _ = sql.Append(CreateAllInsertValueSql(argEntity, ref parameterList));
            _ = sql.Append(')');

            // command生成
            OleDbCommand cmd = new(sql.ToString(), transaction.Connection, transaction);

            // パラメータ設定
            foreach (object parameter in parameterList)
            {
                OleDbParameter oleDbParameter = new OleDbParameter();
                oleDbParameter.Value = parameter;
                if (typeof(DateTime) == parameter.GetType())
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
                }
                _ = cmd.Parameters.Add(oleDbParameter);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// UPDATAEメソッド
        /// </summary>
        /// <param name="argEntity"></param>
        public void Update(T argEntity)
        {
            IList<object> parameterList = new List<object>();

            StringBuilder sql = new($"update {mapper.GetTableName()} set ");
            _ = sql.Append(CreateAllSetValueSql(argEntity, ref parameterList));
            _ = sql.Append(CreatePkWhere(argEntity, ref parameterList));

            // command生成
            OleDbCommand cmd = new(sql.ToString(), transaction.Connection, transaction);

            // パラメータ設定
            foreach (object parameter in parameterList)
            {
                OleDbParameter oleDbParameter = new OleDbParameter();
                oleDbParameter.Value = parameter;
                if (typeof(DateTime) == parameter.GetType())
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
                }
                _ = cmd.Parameters.Add(oleDbParameter);
            }

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 主キー検索
        /// </summary>
        /// <param name="argEntity">引数</param>
        /// <returns></returns>
        public T FindByPrimarykey(T argEntity)
        {
            IList<object> parameterList = new List<object> ();

            string whereSql = CreatePkWhere(argEntity, ref parameterList);

            ICollection<T> queryResultList = Select(whereSql.ToString(), parameterList);

            return queryResultList.First();
        }

        /// <summary>
        /// セレクトテーブル
        /// </summary>
        /// <param name="whereSql"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public ICollection<T> Select(string whereSql, IList<object> parameterList)
        {
            // SQL生成
            StringBuilder sqlBuilder = new(CreateSelectSql());
            sqlBuilder.Append(whereSql);

            // command生成
            OleDbCommand cmd = new(sqlBuilder.ToString(), transaction.Connection, transaction);
            
            // パラメータ設定
            foreach(object parameter in parameterList)
            {
                OleDbParameter oleDbParameter = new OleDbParameter();
                oleDbParameter.Value = parameter;
                if (typeof(DateTime) == parameter.GetType())
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
                }
                _ = cmd.Parameters.Add(oleDbParameter);
            }

            // クエリ実行
            OleDbDataReader reader = cmd.ExecuteReader();

            return ConvertEntity(reader);
        }

        /// <summary>
        /// 件数取得
        /// </summary>
        /// <param name="whereSql">条件句</param>
        /// <param name="parameterList">パラメータ</param>
        /// <returns></returns>
        public int Count(string whereSql, IList<object> parameterList)
        {
            StringBuilder sql = new($"select count(*) as cnt from {mapper.GetTableName()} ");
            sql.Append(whereSql);
            OleDbCommand cmd = new(sql.ToString(), transaction.Connection,transaction);
            // パラメータ設定
            foreach (object parameter in parameterList)
            {
                OleDbParameter oleDbParameter = new OleDbParameter();
                oleDbParameter.Value = parameter;
                if (typeof(DateTime) == parameter.GetType())
                {
                    oleDbParameter.OleDbType = OleDbType.Date;
                }
                _ = cmd.Parameters.Add(oleDbParameter);
            }

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        /// <summary>
        /// セレクト部分の生成
        /// </summary>
        /// <returns></returns>
        private string CreateSelectSql()
        {
            // Mapperから項目情報を取得
            IDictionary<string, IMapper.ItemProperties> itemInfoDict = this.mapper.GetItemInfoDict();

            StringBuilder builder = new("select");

            bool firstFlag = true;

            foreach (var itemPropety in itemInfoDict.Values)
            {
                if (firstFlag)
                {
                    _ = builder.Append(SPACE).Append(itemPropety.ColumnName);
                    firstFlag = false;
                }
                else
                {
                    _ = builder.Append(SPACE).Append(SEPERATER).Append(itemPropety.ColumnName);
                }
            }

            _ = builder.Append(" from").Append(SPACE).Append(mapper.GetTableName()).Append(SPACE);
            
            return builder.ToString();
        }

        /// <summary>
        /// PK条件句生成
        /// </summary>
        /// <param name="argEntity"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        /// <exception cref="MissingFieldException"></exception>
        /// <exception cref="NotFoundException"></exception>
        private string CreatePkWhere(T argEntity, ref IList<object> parameterList)
        {
            // Entityリフレクション
            Type entityType = argEntity.GetType();

            // Where生成
            StringBuilder whereSql = new();
            bool firstFlag = true;
            foreach (KeyValuePair<string, IMapper.ItemProperties> map in mapper.GetItemInfoDict())
            {
                if (map.Value.UniqKeyFlag)
                {
                    if (firstFlag)
                    {
                        _ = whereSql.Append(" where").Append(SPACE).Append(map.Value.ColumnName).Append(" = ?");
                        firstFlag = false;
                    }
                    else
                    {
                        _ = whereSql.Append(SPACE).Append(" AND ").Append(map.Value.ColumnName).Append(" = ?");
                    }

                    PropertyInfo properttInfo = entityType.GetProperty(name: map.Key) ?? throw new MissingFieldException(map.Key);
                    parameterList.Add(properttInfo.GetValue(argEntity) ?? throw new NotFoundException(map.Key));
                }
            }

            return whereSql.ToString();
        }

        /// <summary>
        /// update table名 set [部品A] の[部品A]部分の生成
        /// </summary>
        /// <param name="argEntity"></param>
        /// <param name="setValueParameters"></param>
        /// <returns></returns>
        /// <exception cref="MissingFieldException"></exception>
        /// <exception cref="NotFoundException"></exception>
        private string CreateAllSetValueSql(T argEntity, ref IList<object> setValueParameters)
        {
            // Entityリフレクション
            Type entityType = argEntity.GetType();

            bool firstFlag = true;
            StringBuilder sqlBuilder = new();

            foreach (KeyValuePair<string, IMapper.ItemProperties> item in mapper.GetItemInfoDict())
            {
                if (firstFlag)
                {
                    _ = sqlBuilder.Append(SPACE + item.Value.ColumnName + " = ? ");
                    firstFlag = false;
                }
                else
                {
                    _ = sqlBuilder.Append(SPACE + SEPERATER + item.Value.ColumnName + " = ? ");
                }

                PropertyInfo properttInfo = entityType.GetProperty(name: item.Key) ?? throw new MissingFieldException(item.Key);
                setValueParameters.Add(properttInfo.GetValue(argEntity) ?? "");
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// insert into table名 values ( [部品] ) の部品部分生成
        /// </summary>
        /// <param name="argEntity"></param>
        /// <param name="setValueParameters"></param>
        /// <returns></returns>
        /// <exception cref="MissingFieldException"></exception>
        /// <exception cref="NotFoundException"></exception>
        private string CreateAllInsertValueSql(T argEntity, ref IList<object> setValueParameters)
        {
            // Entityリフレクション
            Type entityType = argEntity.GetType();

            bool firstFlag = true;
            StringBuilder sqlBuilder = new();

            foreach (KeyValuePair<string, IMapper.ItemProperties> item in mapper.GetItemInfoDict())
            {
                if (firstFlag)
                {
                    _ = sqlBuilder.Append(SQL_PARAMETER_MARKER);
                    firstFlag = false;
                }
                else
                {
                    _ = sqlBuilder.Append(SEPERATER + SQL_PARAMETER_MARKER);
                }

                PropertyInfo properttInfo = entityType.GetProperty(name: item.Key) ?? throw new MissingFieldException(item.Key);
                setValueParameters.Add(properttInfo.GetValue(argEntity) ?? "");
            }

            return sqlBuilder.ToString();
        }


        /// <summary>
        /// OleDbDataからEntityへの詰め替え
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        private IList<T> ConvertEntity(OleDbDataReader reader)
        {
            // マッパー情報取得
            IDictionary<string, IMapper.ItemProperties> mapperInfo = mapper.GetItemInfoDict();

            IList<T> list = new List<T>();

            while (reader.Read())
            {
                // エンティティのインスタンス化
                T entity = (T)(Activator.CreateInstance(typeof(T)) ?? throw new NotFoundException("Fail get instace."));
                Type entityType = entity.GetType();
                foreach (PropertyInfo property in entityType.GetProperties())
                {
                    // Entityの項目名
                    string propertyName = property.Name;
                    // Entityの項目型
                    Type propertyType = property.PropertyType;
                    // テーブル項目名
                    string columnName = mapperInfo[propertyName].ColumnName;
                    // テーブル値
                    object columnValue = reader.GetValue(reader.GetOrdinal(columnName));
                    // Entityへの値の設定
                    property.SetValue(entity, Convert.ChangeType(columnValue, propertyType));
                }
                list.Add(entity);
            }

            return list;
        }
    }
}
