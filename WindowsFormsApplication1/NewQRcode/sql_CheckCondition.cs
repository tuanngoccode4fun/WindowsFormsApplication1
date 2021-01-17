using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.NewQRcode
{
    class sql_CheckCondition
    {
        public enum QueryResult {OK, NG, Exception };
        static SqlConnection conn = DBUtils.GetTLVN2DBConnection(); //get from user database
        static List<string> ListSpec = new List<string>() { "AD-", "AT-", "ED-", "ET-", "CD-", "CT-" };
        /// <summary>
        /// INVMB - MB010 : có giá tri => có công đoạn ( trừ mã MB001 bắt dau bằng AD-, AT-, ED-, ET-, CD-, CT-)
        /// </summary>
        /// <param name="MB001_product"></param>
        /// <returns></returns>
        public static QueryResult Is_stageManagement(string MB001_product)
        {
            try
            {
                string temp = MB001_product.Trim().Substring(0, 3);
                if (ListSpec.Contains(temp))
                {
                    return QueryResult.NG;
                }
                conn.Open();
                string m_query_INVMB = @"select distinct MB010 from INVMB where MB001 = '" + MB001_product.Trim()+"'"; // 
                using (SqlCommand command = new SqlCommand(m_query_INVMB, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Kiểm tra có kết quả trả về
                    if (reader.HasRows)
                    {
                        return QueryResult.OK;
                    }
                    else
                    {
                        return QueryResult.NG;
                    }
                }

            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "Is_stageManagement", ex.Message);
                return QueryResult.Exception;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        ///  INVMB -MB022 : gia tri là "Y" hoac "T" => quan ly so lot
        /// </summary>
        /// <param name="MB001_product"></param>
        /// <returns></returns>
        public static QueryResult Is_lotManagement(string MB001_product)
        {
            try
            {
                conn.Open();
                string m_query_INVMB = @"select distinct MB022 from INVMB where MB001 = '" + MB001_product.Trim() + "'"; // 
                using (DataTable myTable = new DataTable())
                using (SqlCommand command = new SqlCommand(m_query_INVMB, conn))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Kiểm tra có kết quả trả về
                    if (reader.HasRows)
                    {
                        myTable.Load(reader);
                        if (myTable.Rows[0]["MB022"].ToString() == "Y" || myTable.Rows[0]["MB022"].ToString() == "T")
                        {
                            return QueryResult.OK;
                        }
                        return QueryResult.NG;
                    }
                    else
                    {
                        return QueryResult.NG;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "Is_lotManagement", ex.Message);
                return QueryResult.Exception;
            }
            finally
            {
                conn.Close();
            }
        }
      
    }
}
