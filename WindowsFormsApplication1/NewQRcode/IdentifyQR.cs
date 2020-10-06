using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowsFormsApplication1.ClassMysql;
using WindowsFormsApplication1.ClassObject;

namespace WindowsFormsApplication1.NewQRcode
{
    
    class IdentifyQR
    {
      static int countItem =10; //typeof(Class_ExportFG_WareHouse).GetProperties().Count();// HAVE CHANGE IF REAL DATA

      static public  bool IsCorrectFormat(string txtInput)
        {
            int countCurrent = System.Text.RegularExpressions.Regex.Replace(txtInput, " ", "").Split(';').Count();
            if (txtInput.Trim().StartsWith("s")!=true)
            {
                MessageBox.Show("QR code not start with \"s\" ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (txtInput.Trim().EndsWith("e") != true)
            {
                MessageBox.Show("QR code not end with \"e\" ", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (countCurrent != countItem)
            {
                MessageBox.Show(string.Format("QR input not enough item spec = {0}, current {1}",countItem, countCurrent), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;    
            }
            return true;
        }
        static public bool IsDuplicate(List<Import_FinishGood_WareHouse> listInput, Import_FinishGood_WareHouse itemInput)
        {
                   return ( listInput.Any(x => x.LotNo == itemInput.LotNo)
                         && listInput.Any(x => x.Product == itemInput.Product)
                         && listInput.Any(x => x.ProductOrder == itemInput.ProductOrder)
                         && listInput.Any(x => x.Quantity == itemInput.Quantity)
                         && listInput.Any(x => x.STT == itemInput.STT)
                         && listInput.Any(x => x.SubQR == itemInput.SubQR)
                         && listInput.Any(x => x.TL101 == itemInput.TL101)
                         && listInput.Any(x => x.TL102 == itemInput.TL102)
                         && listInput.Any(x => x.TL103 == itemInput.TL103)
                         && listInput.Any(x => x.TL104 == itemInput.TL104)
                         && listInput.Any(x => x.TL111 == itemInput.TL111)
                         && listInput.Any(x => x.TL112 == itemInput.TL112)
                         && listInput.Any(x => x.TL113 == itemInput.TL113)
                         && listInput.Any(x => x.TL114 == itemInput.TL114)
                         && listInput.Any(x => x.TransactionID == itemInput.TransactionID)
                         && listInput.Any(x => x.UserID == itemInput.UserID)
                         && listInput.Any(x => x.Warehouse == itemInput.Warehouse)// have to check

                   );}
        static public bool IsWrongWareHouse(List<Import_FinishGood_WareHouse> listInput, Import_FinishGood_WareHouse itemInput)
        {
            if (listInput.Count == 0)
            {
                return false;
            }
            return (listInput.Any(x => x.Warehouse != itemInput.Warehouse));
        }


    }
}
