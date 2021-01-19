using System;
using System.Collections.Generic;
using System.Data;
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
        static string ReturnYN(bool value)
        {
            if (value) return "Y";
            else return "N";
        }
        /// <summary>
        /// Full insert 
        /// </summary>
        /// <param name="ERPPQC"></param>
        /// <param name="TB002"></param>
        /// <param name="IsCheckQuantity_Weight"></param>
        /// <returns></returns>
        public static sql_CheckCondition.QueryResult InsertHaveStageManagementAndLotManagement(DataTable ERPPQC, string TB002, bool IsCheckQuantity_Weight)
        {
            try
            {
                conn.Open();
                string fullText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+ @"\NewQRcode\FileQuerySQL\Insert import warehouse full confirm or not confirm.sql");
                string script = null;
                for (int i = 0; i < ERPPQC.Rows.Count; i++)
                {
                    script = fullText.Replace("@PO_VALUE", ERPPQC.Rows[i]["ProductOrder"].ToString().Trim().Replace("-",""))
                                            .Replace("@LOT_VALUE", ERPPQC.Rows[i]["LotNo"].ToString().Trim())
                                            .Replace("@USER_VALUE", Class.valiballecommon.GetStorage().UserName)
                                            .Replace("@WAREHOUSE_VALUE", ERPPQC.Rows[i]["Warehouse"].ToString().Trim())
                                            .Replace("@LOCATION_VALUE", ERPPQC.Rows[i]["Location"].ToString())
                                            .Replace("@QUANTITY_VALUE", ERPPQC.Rows[i]["Quantity"].ToString())
                                            .Replace("@TF001_VALUE", Class.valiballecommon.GetStorage().DocNo)
                                            .Replace("@STT_VALUE", (i + 1).ToString("0000"))
                                            .Replace("@Confirm_VALUE", ReturnYN(IsCheckQuantity_Weight));
                    using (var command = new SqlCommand(script, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                return sql_CheckCondition.QueryResult.OK;
            }
            catch (Exception ex)
            {
                SystemLog.Output(SystemLog.MSG_TYPE.Err, "InsertHaveStageManagementAndLotManagement", ex.Message);
                return sql_CheckCondition.QueryResult.Exception;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
