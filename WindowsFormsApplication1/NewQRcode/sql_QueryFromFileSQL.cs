using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.ClassObject;

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
        public static sql_CheckCondition.QueryResult InsertHaveStageManagementAndLotManagement(DataRow ERPPQC,int i,string TF002, bool IsCheckQuantity_Weight)
        {
            try
            {
                conn.Open();
                string fullText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+ @"\NewQRcode\FileQuerySQL\StageManagement_LotManagement.sql");
                string script = null;
                script = fullText.Replace("@PO_VALUE", ERPPQC["ProductOrder"].ToString().Trim().Replace("-",""))
                                            .Replace("@LOT_VALUE", ERPPQC["LotNo"].ToString().Trim())
                                            .Replace("@USER_VALUE", Class.valiballecommon.GetStorage().UserName)
                                            .Replace("@WAREHOUSE_VALUE", ERPPQC["Warehouse"].ToString().Trim())
                                            .Replace("@LOCATION_VALUE", ERPPQC["Location"].ToString())
                                            .Replace("@QUANTITY_VALUE", ERPPQC["Quantity"].ToString())
                                            .Replace("@TF001_VALUE", Class.valiballecommon.GetStorage().DocNo)
                                            .Replace("@TF002_VALUE", TF002)
                                            .Replace("@STT_VALUE", (i + 1).ToString("0000"))
                                            .Replace("@Confirm_VALUE", ReturnYN(IsCheckQuantity_Weight));
                    using (var command = new SqlCommand(script, conn))
                    {
                        command.ExecuteNonQuery();
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
        public static sql_CheckCondition.QueryResult InsertNoStageManagementAndLotManagement(DataRow ERPPQC, int i, string TF002, bool IsCheckQuantity_Weight)
        {
            try
            {
                conn.Open();
                string fullText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\NewQRcode\FileQuerySQL\not_stageManagement_LotManagement.sql");
                string script = null;
                script = fullText.Replace("@PO_VALUE", ERPPQC["ProductOrder"].ToString().Trim().Replace("-", ""))
                                            .Replace("@LOT_VALUE", ERPPQC["LotNo"].ToString().Trim())
                                            .Replace("@USER_VALUE", Class.valiballecommon.GetStorage().UserName)
                                            .Replace("@WAREHOUSE_VALUE", ERPPQC["Warehouse"].ToString().Trim())
                                            .Replace("@LOCATION_VALUE", ERPPQC["Location"].ToString())
                                            .Replace("@QUANTITY_VALUE", ERPPQC["Quantity"].ToString())
                                            .Replace("@TF001_VALUE", Class.valiballecommon.GetStorage().DocNo)
                                            .Replace("@TF002_VALUE", TF002)
                                            .Replace("@STT_VALUE", (i + 1).ToString("0000"))
                                            .Replace("@Confirm_VALUE", ReturnYN(IsCheckQuantity_Weight));
                using (var command = new SqlCommand(script, conn))
                {
                    command.ExecuteNonQuery();
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
        public static sql_CheckCondition.QueryResult InsertNoStageManagementAndNoLotManagement(DataRow ERPPQC, int i, string TF002, bool IsCheckQuantity_Weight)
        {
            try
            {
                conn.Open();
                string fullText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\NewQRcode\FileQuerySQL\not_stageManagement_NoLotManagement.sql");
                string script = null;
                script = fullText.Replace("@PO_VALUE", ERPPQC["ProductOrder"].ToString().Trim().Replace("-", ""))
                                            .Replace("@LOT_VALUE", ERPPQC["LotNo"].ToString().Trim())
                                            .Replace("@USER_VALUE", Class.valiballecommon.GetStorage().UserName)
                                            .Replace("@WAREHOUSE_VALUE", ERPPQC["Warehouse"].ToString().Trim())
                                            .Replace("@LOCATION_VALUE", ERPPQC["Location"].ToString())
                                            .Replace("@QUANTITY_VALUE", ERPPQC["Quantity"].ToString())
                                            .Replace("@TF001_VALUE", Class.valiballecommon.GetStorage().DocNo)
                                            .Replace("@TF002_VALUE", TF002)
                                            .Replace("@STT_VALUE", (i + 1).ToString("0000"))
                                            .Replace("@Confirm_VALUE", ReturnYN(IsCheckQuantity_Weight));
                using (var command = new SqlCommand(script, conn))
                {
                    command.ExecuteNonQuery();
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
        public static sql_CheckCondition.QueryResult InsertHaveStageManagementAndNoLotManagement(DataRow ERPPQC, int i, string TF002, bool IsCheckQuantity_Weight)
        {
            try
            {
                conn.Open();
                string fullText = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\NewQRcode\FileQuerySQL\StageManagement_NotLotManagement.sql");
                string script = null;
                script = fullText.Replace("@PO_VALUE", ERPPQC["ProductOrder"].ToString().Trim().Replace("-", ""))
                                            .Replace("@LOT_VALUE", ERPPQC["LotNo"].ToString().Trim())
                                            .Replace("@USER_VALUE", Class.valiballecommon.GetStorage().UserName)
                                            .Replace("@WAREHOUSE_VALUE", ERPPQC["Warehouse"].ToString().Trim())
                                            .Replace("@LOCATION_VALUE", ERPPQC["Location"].ToString())
                                            .Replace("@QUANTITY_VALUE", ERPPQC["Quantity"].ToString())
                                            .Replace("@TF001_VALUE", Class.valiballecommon.GetStorage().DocNo)
                                            .Replace("@TF002_VALUE", TF002)
                                            .Replace("@STT_VALUE", (i + 1).ToString("0000"))
                                            .Replace("@Confirm_VALUE", ReturnYN(IsCheckQuantity_Weight));
                using (var command = new SqlCommand(script, conn))
                {
                    command.ExecuteNonQuery();
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
