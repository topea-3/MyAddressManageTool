using MyAddressManageTool.TableManager;
using MyAddressManageTool.TableManager.Entity;
using MyAddressManageTool.TableManager.Manager;
using MyAddressManageTool.TableManager.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyAddressManageTool.Test
{
    internal class TestClass
    {
        public static void testExec()
        {
            TransactionManager.UpdateAccessDbPath(@"C:\tool\開発中\住所録管理_ver.3.00\住所録管理.accdb");

            TransactionManager manager = new TransactionManager();

            try
            {
                manager.StartTransaction();
                TableManagerControll<NumberControllTableEntity> tableManager = new(
                    new NumberControlTableMapper(), manager.Transaction);

                NumberControllTableEntity entity = new NumberControllTableEntity();
                entity.Id = "ADDRESS_ID";

                NumberControllTableEntity outEntity = tableManager.FindByPrimarykey(entity);

                StringBuilder @string = new StringBuilder();
                @string.Append(outEntity.Id);
                @string.Append("," + outEntity.NumberName);
                @string.Append("," + outEntity.CurrentNumber);
                @string.Append("," + outEntity.MaxNumber);
                @string.Append("," + outEntity.PreFix);

                MessageBox.Show(@string.ToString());

            }
            catch (Exception ex)
            {
                manager.Rollback();
                throw ex;
            }
            finally
            {
                manager.EndTransaction();
            }
        }
    }
}
