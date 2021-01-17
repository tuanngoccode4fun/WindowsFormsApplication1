using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1.WMS.Model;
using System.Data;
using WindowsFormsApplication1.Database;
using System.Windows.Forms;
using WindowsFormsApplication1.NewQRcode.UI_mesage;

namespace WindowsFormsApplication1.WMS.Controller
{
  public  class UpdateData2DBForFinishedGoods
    {
        public bool UpdateDataDBForFinishedGoods(FinishedGoodsItems fgItems, out string ERPDoc, out string SFTDoc)
		{
			try
			{
				GetdataSFTToDataTable getdataSFTToDataTable = new GetdataSFTToDataTable();
				DataTable dtLotMODETAL = getdataSFTToDataTable.GetDataTableLOTMODETAIL(fgItems.productCode);
				ERPDataUpdate eRPDataUpdate = new ERPDataUpdate();
				string TB002 = eRPDataUpdate.getTB002(Class.valiballecommon.GetStorage().DocNo);
				SFTDataUpdate sFTDataUpdate = new SFTDataUpdate();
				string TransNo = sFTDataUpdate.getTransnoOfSFT(Class.valiballecommon.GetStorage().DocNo);
				
				Database.ADMMFUpdate aDMMF = new ADMMFUpdate();
				DataTable dtADMMF = aDMMF.GetDtADMFFByUser(Class.valiballecommon.GetStorage().UserName);
				var Update2SFT = sFTDataUpdate.SFTdataUpdate(fgItems, dtLotMODETAL, TB002, TransNo);
				if (Update2SFT == false)
				{
					SystemLog.Output(SystemLog.MSG_TYPE.War, "sFTDataUpdate.SFTdataUpdate(fgItems, TB002, TransNo)", "false");
				}
				else
				{
					SystemLog.Output(SystemLog.MSG_TYPE.War, Class.valiballecommon.GetStorage().DocNo+"-" + TransNo + " is created !", "");
				}

				var Update2ERP = eRPDataUpdate.UploadtoERPDBForFinishedGoods(fgItems,dtADMMF, dtLotMODETAL, TB002, TransNo);
				if (Update2ERP == false)
				{
					SystemLog.Output(SystemLog.MSG_TYPE.War, "eRPDataUpdate.UploadtoERPDBForFinishedGoods(fgItems, TB002, TransNo)", "false");
				}
				else
				{
					SystemLog.Output(SystemLog.MSG_TYPE.War, Class.valiballecommon.GetStorage().DocNo+"-" + TB002 + " is created !", "");
				}
				Database.Model.INVItems iNVItems = new Database.Model.INVItems();
				iNVItems.Product = fgItems.product;
				iNVItems.ProductCode = fgItems.productCode;
				iNVItems.Lot = fgItems.lot;
				iNVItems.Create_Date = fgItems.ImportDate;
				iNVItems.TypeDoccument = Class.valiballecommon.GetStorage().DocNo;
				iNVItems.DoccumentNo = TB002;
				iNVItems.STTDoc = "0001";
				iNVItems.Warehouse = fgItems.Warehouse;
				iNVItems.TypeInportExport = "1";
				iNVItems.TypeChange = "1";
				iNVItems.Quantity = fgItems.TotalQty;
				iNVItems.PackageQty = 0;
				iNVItems.Note = iNVItems.ProductCode;
				iNVItems.Location = fgItems.location;
				iNVItems.ImportDate = fgItems.ImportDate;
				iNVItems.MainLocation = fgItems.location;

			
				Database.INVMFUpdate iNVMFUpdate = new INVMFUpdate();
				var UpdateINVMF = iNVMFUpdate.InsertINVMF(iNVItems, dtADMMF);
				Database.INVMEUpdate iNVMEUpdate = new INVMEUpdate();
				var UpdateINVME = iNVMEUpdate.InsertINVME(iNVItems, dtADMMF);

				INVLAUpdate iNVLAUpdate = new INVLAUpdate();
				var UpdateINVLA = iNVLAUpdate.InsertINVLA(iNVItems, dtADMMF);
				INVLFUpdate iNVLFUpdate = new INVLFUpdate();
				var UpdateINVLF = iNVLFUpdate.InsertINVLF(iNVItems, dtADMMF);
				INVMCUpdate iNVMCUpdate = new INVMCUpdate();
				var UpdateINVMC = iNVMCUpdate.UpdateOrInsertINVMC(iNVItems, dtADMMF);
				INVMMUpdate iNVMMUpdate = new INVMMUpdate();
				var UpdateINVMM = iNVMMUpdate.UpdateOrInsertINVMM(iNVItems, dtADMMF);
				ERPDoc = TB002;
				SFTDoc = TransNo;


			}
			catch (Exception ex)
			{

				SystemLog.Output(SystemLog.MSG_TYPE.Err, "UpdateData2DBForFinishedGoods(FinishedGoodsItems fgItems)", ex.Message);
				ERPDoc = "";
				SFTDoc = "";
				return false;
			}
            return true;
        }


		public bool UpdateDataDBForFinishedGoods(DataTable dtERPPQC,out string ERPDoc)
		{
			try
			{
				ERPDoc = "";
				//Class.valiballecommon.GetStorage().DocNo
				ERPDataUpdate eRPDataUpdate = new ERPDataUpdate();
				string TB002 = eRPDataUpdate.getTB002(Class.valiballecommon.GetStorage().DocNo);//fix
				//string TB002 = eRPDataUpdate.getTF002(Class.valiballecommon.GetStorage().DocNo);//fix disable 22/12/2020

				ConvertDataTable convertDataTable = new ConvertDataTable();
				ConvertDataERP convertDataERP = new ConvertDataERP();
                DataTable dtSFCTC = convertDataERP.GetDataTableSFCTC(dtERPPQC, TB002, "Y");
				DataTable dtSFCTB = convertDataERP.GetDataTableSFCTB(dtSFCTC, dtERPPQC, "", "Y");// NO SFT
				DataTable dtMOCTG = convertDataERP.GetDataTableMOCTG(dtERPPQC, TB002);
                DataTable dtMOCTF = convertDataERP.GetDataTableMOCTF(dtMOCTG, TB002);
                if (dtSFCTC.Rows.Count > 0 && dtMOCTG.Rows.Count > 0 && dtMOCTF.Rows.Count > 0)
                {
                    Database.SFC.SFCTC sFCTC = new Database.SFC.SFCTC();
                    var InsertSFCTC = sFCTC.InsertData(dtSFCTC);
                    if (InsertSFCTC == false)
                    {
                        MessageBox.Show("Insert SFCTC fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
					Database.SFC.SFCTB sFCTB = new Database.SFC.SFCTB();
					var InsertSFCTB = sFCTB.InsertData(dtSFCTB);
					if (InsertSFCTB == false)
					{
						MessageBox.Show("Insert SFCTB fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
					Database.SFC.SFCTA sFCTA = new Database.SFC.SFCTA();
                    var UpdateSFCTA = sFCTA.UpdateSFCTAForFinishedGoods(dtERPPQC);// UPDATE SO LUONG
                    if (UpdateSFCTA == false)
                    {
                        MessageBox.Show("Insert SFCTA fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    Database.MOC.MOCTG mOCTG = new Database.MOC.MOCTG();
                    var insertMoctg = mOCTG.InsertData(dtMOCTG);
                    if (insertMoctg == false)
                    {
						ClassMessageBoxUI.Show("Insert MOCTG fail ", false);
                        return false;
                    }

                    Database.MOC.MOCTF mOCTF = new Database.MOC.MOCTF();
                    var insertMOCTF = mOCTF.InsertData(dtMOCTF);
                    if (insertMOCTF == false)
                    {
						ClassMessageBoxUI.Show("Insert MOCTF fail ", false);
                        return false;
                    }
                    Database.MOC.MOCTA mOCTA = new Database.MOC.MOCTA();
                    var updateMOCTA = mOCTA.UpdateMOCTAForFinishedGoods(dtERPPQC);
                    if (updateMOCTA == false)
                    {
						ClassMessageBoxUI.Show("update MOCTA fail ",false);
                        return false;
                    }
                    UpdateWarehouseForFinishedGoods updateWarehouseForFinishedGoods = new UpdateWarehouseForFinishedGoods();
					var UpdateWarehouse = updateWarehouseForFinishedGoods.UpdateWarehouse(dtERPPQC, TB002);
					if (UpdateWarehouse == false)
					{
						ClassMessageBoxUI.Show("update stock warehouse fail ", false);
					}
				}
				ERPDoc = TB002;
			}
			catch (Exception ex)
			{
				SystemLog.Output(SystemLog.MSG_TYPE.Err, "UpdateData2DBForFinishedGoods(FinishedGoodsItems fgItems)", ex.Message);
				ERPDoc = "";
				return false;
			}
			return true;
		}
		public bool UpdateDataDBForAllDeparment(DataTable dtERPPQC, out string ERPDoc)
		{
			ERPDoc = null;
			try
			{

			}
			catch (Exception ex)
			{
				SystemLog.Output(SystemLog.MSG_TYPE.Err,"Exception message", "UpdateDataDBForAllDeparment :" + ex.Message);
			}
			return false;
		}
		public bool UpdateDataDBForFinishedGoodsNotConfirm(DataTable dtERPPQC, string Location, out string ERPDoc, out string SFTDoc)
		{
			try
			{
				ERPDoc = "";
				SFTDoc = "";

				ERPDataUpdate eRPDataUpdate = new ERPDataUpdate();
				string TB002 = eRPDataUpdate.getTB002(Class.valiballecommon.GetStorage().DocNo);
				SFTDataUpdate sFTDataUpdate = new SFTDataUpdate();
				string TransNo = sFTDataUpdate.getTransnoOfSFT(Class.valiballecommon.GetStorage().DocNo);
				Database.SFT.SFT_TRANSORDER_LINE sFT_TRANSORDER_LINE = new Database.SFT.SFT_TRANSORDER_LINE();
				ConvertDataTable convertDataTable = new ConvertDataTable();
				DataTable dtTRANSORDER_LINE = convertDataTable.ERPPQCtoSFTTRANSORDERLINE(dtERPPQC, TransNo, TB002, Location);
				DataTable dtTRANSORDER = convertDataTable.GetDataSFTTRANSORDER(dtERPPQC, dtTRANSORDER_LINE);
				DataTable dtWSRUN = convertDataTable.GetDataTableSFT_WS_RUN(dtERPPQC, dtTRANSORDER_LINE);
				ConvertDataERP convertDataERP = new ConvertDataERP();
				DataTable dtSFCTC = convertDataERP.GetDataTableSFCTC(dtERPPQC, TB002,"N");////
				DataTable dtSFCTB = convertDataERP.GetDataTableSFCTB(dtSFCTC, dtERPPQC, TransNo,"N");///
			
				if (dtTRANSORDER_LINE.Rows.Count > 0 && dtTRANSORDER.Rows.Count > 0 && dtWSRUN.Rows.Count > 0
					&& dtSFCTC.Rows.Count > 0 && dtSFCTB.Rows.Count > 0 )
				{

					var Result = sFT_TRANSORDER_LINE.InsertData(dtTRANSORDER_LINE);
					if (Result == false)
					{
						MessageBox.Show("Insert TransOrder_Line fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}

					Database.SFT.SFT_TRANSORDER sFT_TRANSORDER = new Database.SFT.SFT_TRANSORDER();
					var resultTransOrder = sFT_TRANSORDER.InsertData(dtTRANSORDER);
					if (resultTransOrder == false)
					{
						MessageBox.Show("Insert TransOrder fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}

					int[] sequence = new int[dtERPPQC.Rows.Count];
					Database.SFT.SFT_WS_RUN sFT_WS_RUN = new Database.SFT.SFT_WS_RUN();
					var resultWs_run = sFT_WS_RUN.InsertData(dtWSRUN, out sequence);
					if (resultWs_run == false)
					{
						MessageBox.Show("Insert SFT_WS_RUN fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
					Database.SFT.MODETAIL mODETAIL = new Database.SFT.MODETAIL();
					var resultUpdate = mODETAIL.UpdateMODETAIL(dtTRANSORDER_LINE);//MOC027 ko biet la gi ?
					if (resultUpdate == false)
					{
						MessageBox.Show("update Modetail fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
					Database.SFT.SFT_LOT sFT_LOT = new Database.SFT.SFT_LOT();
					var InsertOrUpdate = sFT_LOT.InsertUpdateLot(dtTRANSORDER_LINE);
					if (InsertOrUpdate == false)
					{
						MessageBox.Show("Insert Lot fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}

					Database.SFC.SFCTC sFCTC = new Database.SFC.SFCTC();///

					var InsertSFCTC = sFCTC.InsertData(dtSFCTC);/////
					if (InsertSFCTC == false)
					{
						MessageBox.Show("Insert SFCTC fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}

					Database.SFC.SFCTB sFCTB = new Database.SFC.SFCTB();/////
					var InsertSFCTB = sFCTB.InsertData(dtSFCTB);
					if (InsertSFCTB == false)
					{
						MessageBox.Show("Insert SFCTB fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
					Database.SFC.SFCTA sFCTA = new Database.SFC.SFCTA();/////
					var UpdateSFCTA = sFCTA.UpdateSFCTAForFinishedGoodsNotConfirm(dtERPPQC);
					if (UpdateSFCTA == false)
					{
						MessageBox.Show("Insert SFCTA fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}

					Database.ERPSOFT.ERPOutPQCQR eRPOutPQCQR = new Database.ERPSOFT.ERPOutPQCQR();////function kho
					var updateOutPQC = eRPOutPQCQR.UpdateImportWarehouse(dtERPPQC, Class.valiballecommon.GetStorage().DocNo+"-"+TB002);
					if (updateOutPQC == false)
					{
						MessageBox.Show("Insert import status fail ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
				}
				ERPDoc = TB002;
				SFTDoc = TransNo;


			}
			catch (Exception ex)
			{

				SystemLog.Output(SystemLog.MSG_TYPE.Err, "UpdateData2DBForFinishedGoods(FinishedGoodsItems fgItems)", ex.Message);
				ERPDoc = "";
				SFTDoc = "";
				return false;
			}
			return true;
		}
	}
}
