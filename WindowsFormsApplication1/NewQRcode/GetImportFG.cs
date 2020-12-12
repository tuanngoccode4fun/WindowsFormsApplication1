using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.ClassMysql;
using WindowsFormsApplication1.ClassObject;

namespace WindowsFormsApplication1.NewQRcode
{
    class GetImportFG
    {
        public static Import_FinishGood_WareHouse ConvertQR2DataTable(string txtInput,string warehouseName, DataGridView dtinput)
        {
            Import_FinishGood_WareHouse Temp = new Import_FinishGood_WareHouse();
            string[] arraydata = Regex.Replace( txtInput," ","").TrimStart('s').TrimEnd('e').Split(';');
            if (IdentifyQR.IsCorrectFormat(txtInput.Trim()) == true)
                {
                  Temp.TransactionID = Regex.Replace(txtInput, " ", "");
                  Temp.UserID = Class.valiballecommon.GetStorage().UserName;
                  Temp.STT = (dtinput.Rows.Count+1).ToString("D4");
                  Temp.ProductOrder = ReturnProductOrder(arraydata[1]);//"B511-20100019";//"B511-20100154";
                  Temp.Product = arraydata[2];
                  Temp.Quantity =Convert.ToUInt32( arraydata[4]);
                  Temp.LotNo = arraydata[7];
                  Temp.Warehouse = warehouseName;
                  Temp.dateImport = DateTime.Now;
                  return Temp;
                }
            return null ;
        }
    static string ReturnProductOrder(string text)
        {
            try
            {
                if (text.ToCharArray().Count() > 4)
                {
                    return (text.Substring(0, 4) + "-" + text.Substring(4));
                }
            }
            catch (Exception ex)
            {
                Log.Logfile.Output(Log.StatusLog.Error,"ReturnProductOrder: " + ex.Message);
            }
            return null;
        }
    }
}
