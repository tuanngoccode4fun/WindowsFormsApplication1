using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.NewQRcode
{
    class sql_QueryFromFileSQL
    {
         static SqlConnection conn = DBUtils.GetTLVN2DBConnection(); //get from user database
        public static sql_CheckCondition.QueryResult InsertHaveStageManagement()
        {
            try
            {
                conn.Open();
                string script = File.ReadAllText(@"E:\Project Docs\MX462-PD\MX756_ModMappings1.sql");
                using (var command = new SqlCommand(script, conn))
                {
                    command.ExecuteNonQuery();
                }
                return sql_CheckCondition.QueryResult.OK;
            }
            catch (Exception ex)
            {
                return sql_CheckCondition.QueryResult.Exception;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
