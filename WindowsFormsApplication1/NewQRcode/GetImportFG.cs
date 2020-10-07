using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsFormsApplication1.ClassMysql;
using WindowsFormsApplication1.ClassObject;

namespace WindowsFormsApplication1.NewQRcode
{
    class GetImportFG
    {
        public static Import_FinishGood_WareHouse ConvertQR2DataTable(string txtInput,string warehouseName)
        {
            Import_FinishGood_WareHouse Temp = new Import_FinishGood_WareHouse();
            string[] arraydata = Regex.Replace( txtInput," ","").TrimStart('s').TrimEnd('e').Split(';');
            if (IdentifyQR.IsCorrectFormat(txtInput.Trim()) == true)
                {
                  Temp.TransactionID = Regex.Replace(txtInput, " ", "");
                  Temp.UserID = Class.valiballecommon.GetStorage().UserName;
                  Temp.STT = "0001";// arraydata[1];
                  Temp.ProductOrder = /*arraydata[2]*/ "Not found";
                  Temp.Product = arraydata[2];
                  Temp.Quantity =Convert.ToUInt32( arraydata[4]);
                  Temp.LotNo = arraydata[7];
                  Temp.Warehouse = warehouseName;
                  Temp.dateImport = DateTime.Now;
                  return Temp;
                }
            return null ;
        }
    }
}
