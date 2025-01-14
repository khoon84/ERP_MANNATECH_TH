﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;

namespace MLM_Program
{
    public partial class frmSell_Select_Mem_Item : Form
    {



        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        cls_Grid_Base cgb = new cls_Grid_Base();
        
        private const string base_db_name = "tbl_SalesItemDetail";
        private int Data_Set_Form_TF;

        public delegate void SendNumberDele(string Send_Number, string Send_Name, string Send_OrderNumber);
        public event SendNumberDele Send_Mem_Number;

        private Series series_Item = new Series();


        public frmSell_Select_Mem_Item()
        {
            InitializeComponent();

            DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance
            | BindingFlags.SetProperty, null, dGridView_Base, new object[] { true });
        }

      


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            //mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;
            //grB_Search.Height = mtxtMbid.Top + mtxtMbid.Height + 3;    

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);
            cpbf.Put_Rec_Code_ComboBox(combo_Rec, combo_Rec_Code);



            Reset_Chart_Total();

            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate1.Text = DateTime.Now.ToString("yyyy-MM-dd");

            mtxtSellDate11.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate22.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMakDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2_2.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_3.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_4.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_5.BackColor = cls_app_static_var.txt_Enable_Color;
            



            mtxtMbid.Focus();
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Excel.Left = butt_Select.Left + butt_Select.Width + 2;
            butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Select);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);  
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                // mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Select, ee1);  //butt_Search
                //EventArgs ee1 = null; Select_Button_Click(butt_Select, ee1);

                //Set_Form_Date(mtxtMbid.Text);
                Data_Set_Form_TF = 0;
            }
        }

        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (!this.Controls.ContainsKey("Popup_gr"))
                        this.Close();
                    else
                    {
                        DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];

                        if (T_Gd.Name == "Popup_gr")
                        {
                            if (T_Gd.Tag != null)
                            {
                                if (!this.Controls.ContainsKey(T_Gd.Tag.ToString()))
                                {
                                    cls_form_Meth cfm = new cls_form_Meth();
                                    Control T_cl = cfm.from_Search_Control(this, T_Gd.Tag.ToString());
                                    if (T_cl != null)
                                        T_cl.Focus();

                                }
                            }

                            T_Gd.Visible = false;
                            T_Gd.Dispose();

                            // cls_form_Meth cfm = new cls_form_Meth();
                            // cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

           
            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
            }
        }


        private Boolean Check_TextBox_Error()
        {
           
            cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            /*
            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")               
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid);

                if (Ret == -1)
                {                    
                    mtxtMbid.Focus();     return false;
                }   
            }


            if (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtMbid2);

                if (Ret == -1)
                {
                    mtxtMbid2.Focus(); return false;
                }   
            }*/


            if (mtxtSellDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate1.Text, mtxtSellDate1, "Date") == false)
                {
                    mtxtSellDate1.Focus();
                    return false;
                }

            }

            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }
            }

            if (mtxtMakDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate1.Text, mtxtMakDate1, "Date") == false)
                {
                    mtxtMakDate1.Focus();
                    return false;
                }
            }

            if (mtxtMakDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakDate2.Text, mtxtMakDate2, "Date") == false)
                {
                    mtxtMakDate2.Focus();
                    return false;
                }
            }


                   

            return true;
        }


        private void Make_Base_Query(ref string Tsql)
        {

            //string[] g_HeaderText = {"주문번호"  , "주문_일자"   ,  "회원_번호"   , "성명"   , "_주민번호"       
            //                        , "상품코드"    , "상품명"   , "개별단가"    , "개별PV" ,  "주문_수량"
            //                       ,"출고_수량" , "총상품액"    , "총상품PV" , "주문_종류"   , "구분" 
            //                      , "배송구분" ,  "우편번호","배송지"   , "수령인명", "연락처"
            //                      , "등록_센타명" ,  "주문_센타명"    , ""  ,"",""                                 
            //                        };


            Tsql = "Select  ";
            Tsql = Tsql + "  tbl_SalesDetail.OrderNumber  ";
            Tsql = Tsql + " ,LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2) as SellDate  ";
            Tsql = Tsql + " ,LEFT(tbl_SalesDetail.SellDate_2,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate_2,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate_2,2)  as SellDate_2 ";
            Tsql = Tsql + ", LEFT(StockOut.Out_Date,4) +'-' + LEFT(RIGHT(StockOut.Out_Date,4),2) + '-' + RIGHT(StockOut.Out_Date,2)  as Out_Date";
            Tsql = Tsql + ", tbl_SalesDetail.mbid ";
            Tsql = Tsql + " ,tbl_SalesDetail.M_Name ";            
            Tsql = Tsql + ", tbl_Memberinfo.Cpno";

            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCode "; 
            Tsql = Tsql + " , tbl_Goods.Name Item_Name ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemPrice ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemPV ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCV ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemCount ";

            Tsql = Tsql + " , tbl_SalesItemDetail.Send_itemCount1 ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemTotalPrice ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemTotalPV ";
            Tsql = Tsql + " , tbl_SalesItemDetail.ItemTotalCV ";
            Tsql = Tsql + " , tbl_SellType.SellTypeName SellCodeName  ";
            cls_form_Meth cm = new cls_form_Meth();
            Tsql = Tsql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When SellState = 'N_3' Then '" + cm._chang_base_caption_search("교환_정상") + "'";
            Tsql = Tsql + "  When SellState = 'R_1' Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When SellState = 'R_3' Then '" + cm._chang_base_caption_search("교환_반품") + "'";
            Tsql = Tsql + "  When SellState = 'C_1' Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END  SellStateName ";


            Tsql = Tsql + " ,Case When Receive_Method = '1' Then '" + cm._chang_base_caption_search("직접수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '2' Then '" + cm._chang_base_caption_search("배송") + "'";
            Tsql = Tsql + "  When Receive_Method = '3' Then '" + cm._chang_base_caption_search("센타수령") + "'";
            Tsql = Tsql + "  When Receive_Method = '4' Then '" + cm._chang_base_caption_search("본사직접수령") + "'";
            Tsql = Tsql + " ELSE '' "; 
            Tsql = Tsql + " END  Receive_Method_Name ";

            Tsql = Tsql + " ,Get_ZipCode ";
            Tsql = Tsql + " ,Get_Address1 ";
            Tsql = Tsql + " ,Get_Address2 ";
            Tsql = Tsql + " ,Get_Name1 ";
            Tsql = Tsql + " ,Get_Tel1 ";
            Tsql = Tsql + " ,Get_Tel2 ";
            

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " ,Isnull(S_Bus.Name,'') as S_B_Name";


            Tsql = Tsql + " ,tbl_SalesItemDetail.Salesitemindex  ";
            Tsql = Tsql + " ,tbl_SalesDetail.Recordid  ";
            Tsql = Tsql + " ,tbl_Sales_Rece.Receive_Method  ";

            Tsql = Tsql + " ,Case When Ga_Order >= 1 Then '" + cm._chang_base_caption_search("미승인") + "'";
            Tsql = Tsql + "  When Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            Tsql = Tsql + " ELSE '' END ";

            
             
            Tsql = Tsql + " From tbl_SalesItemDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_Sales_Rece (nolock)  ON tbl_SalesItemDetail.OrderNumber = tbl_Sales_Rece.OrderNumber And tbl_SalesItemDetail.Salesitemindex = tbl_Sales_Rece.Salesitemindex "; 
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = tbl_SalesDetail.Mbid And tbl_Memberinfo.Mbid2 = tbl_SalesDetail.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code  ";
            Tsql = Tsql + " LEFT JOIN tbl_Business S_Bus (nolock) ON tbl_SalesDetail.BusCode = S_Bus.NCode And tbl_SalesDetail.Na_code = S_Bus.Na_code  ";            
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";
            Tsql = Tsql + " LEFT JOIN(select  OrderNumber, max(Out_Date) as out_date from tbl_StockOutput (nolock) group by OrderNumber) StockOut on tbl_SalesDetail.OrderNumber = StockOut.OrderNumber";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";
            Tsql = Tsql + " LEFT Join tbl_SellType ON tbl_SalesDetail.SellCode = tbl_SellType.SellCode ";
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where tbl_SalesDetail.Ga_Order = 0  And tbl_SalesDetail.SellCode = ''  ";
            
                        string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "") 
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "") 
                )
            {
                //cls_Search_DB csb = new cls_Search_DB();
                //if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                //{
                Mbid = mtxtMbid.Text;

                if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid ='" + Mbid + "'";

                //    if (Mbid2 >= 0)
                //        strSql = strSql + " And tbl_SalesDetail.Mbid2 = " + Mbid2;
                //}


            }

            //회원번호2로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And tbl_SalesDetail.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And tbl_SalesDetail.Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.M_Name Like '%" + txtName.Text.Trim() + "%'";






            //주문일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_SalesDetail.SellDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //주문일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_SalesDetail.SellDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_SalesDetail.SellDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }

            //정산일자로 검색 -1
            if ((mtxtSellDate11.Text.Replace("-", "").Trim() != "") && (mtxtSellDate11.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.SellDate_2 ,10),'-','') = '" + mtxtSellDate11.Text.Replace("-", "").Trim() + "'";

            //정산일자로 검색 -2
            if ((mtxtSellDate11.Text.Replace("-", "").Trim() != "") && (mtxtSellDate22.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.SellDate_2 ,10),'-','') >= '" + mtxtSellDate11.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( tbl_SalesDetail.SellDate_2 ,10),'-','') <= '" + mtxtSellDate22.Text.Replace("-", "").Trim() + "'";
            }


            if (txt_ItemName_Code2.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesitemDetail.ItemCode = '" + txt_ItemName_Code2.Text.Trim() + "'";

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.BusinessCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (txtCenter2_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.BusCode = '" + txtCenter2_Code.Text.Trim() + "'";

            //if (txtR_Id_Code.Text.Trim() != "")
            //    strSql = strSql + " And tbl_SalesDetail.recordid = '" + txtR_Id_Code.Text.Trim() + "'";


            if (txtSellCode_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.SellCode = '" + txtSellCode_Code.Text.Trim() + "'";

           
            

            if (txtOrderNumber.Text.Trim() != "")
                strSql = strSql + " And tbl_SalesDetail.OrderNumber = '" + txtOrderNumber.Text.Trim() + "'";



            if (combo_Rec_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method = " + combo_Rec_Code.Text.Trim(); 



            if (radioB_Rec_2.Checked == true)
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method IS Not NULL ";

            if (radioB_Rec_3.Checked == true)
                strSql = strSql + " And tbl_Sales_Rece.Receive_Method IS  NULL ";


            if (radioB_SellTF2.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.Ga_Order = 0 ";

            if (radioB_SellTF3.Checked == true)
                strSql = strSql + " And tbl_SalesDetail.Ga_Order > 0 ";



            if (opt_sell_2.Checked == true)
            {
                strSql = strSql + " And (tbl_SalesitemDetail.SellState = 'N_1' OR tbl_SalesitemDetail.SellState = 'N_3' ) ";
                strSql = strSql + " And tbl_SalesDetail.ReturnTF <> 5  ";
            }
            if (opt_sell_3.Checked == true)
                strSql = strSql + " And (tbl_SalesitemDetail.SellState = 'R_1' OR tbl_SalesitemDetail.SellState = 'R_3' ) ";

            //if (opt_sell_4.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 3 ";

            //if (opt_sell_5.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.ReturnTF = 4 ";
            
            //if (opt_Ed_2.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney = 0 ";

            //if (opt_Ed_3.Checked == true)
            //    strSql = strSql + " And tbl_SalesDetail.UnaccMoney <> 0 ";



            ////strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //strSql = strSql + " And tbl_SalesDetail.BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            //strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by tbl_SalesDetail.SellDate DESC, tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + ",tbl_SalesDetail.Mbid, tbl_SalesDetail.Mbid2  ";
        }




        private void Base_Grid_Set()
        {   
            string Tsql = "";            
            Make_Base_Query(ref Tsql);

            Make_Base_Query_(ref Tsql);

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                  
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text ) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_10 = 0; double Sum_11 = 0; double Sum_12 = 0;
            double Sum_13 = 0; double Sum_14 = 0; //double Sum_15 = 0;
            //double Sum_16 = 0;
            double Sell_Cnt_1 = 0; double Sell_Cnt_2 = 0;
            double Out_Cnt_1 = 0; double Out_Cnt_2 = 0;

            double Out_Put_1 = 0 ,  Out_Put_2 = 0 , Out_Put_3 = 0 ,  Out_Put_4 = 0;
            int OrdCnt = 0;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            Dictionary<string, double> SelType_1 = new Dictionary<string, double>();
            Dictionary<string, double> Item_Cnt = new Dictionary<string, double>();
            Dictionary<string, string> OrderNum = new Dictionary<string, string>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                string T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["OrderNumber"].ToString();
              

                Sum_10 = Sum_10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());  //판매수량
                Sum_11 = Sum_11 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Send_itemCount1"].ToString());  //출고수량
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPrice"].ToString());  //금액
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPV"].ToString());  //PV       
                Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalCV"].ToString());  //CV       


                if (OrderNum.ContainsKey(T_ver) != true)
                {
                    OrdCnt++;
                    OrderNum[T_ver] = T_ver;
                }



                T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["SellCodeName"].ToString();
                if (SelType_1.ContainsKey(T_ver) == true)
                {
                    SelType_1[T_ver] = SelType_1[T_ver] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());  //금액                    
                }
                else
                {
                    SelType_1[T_ver] = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());
                }

                T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["Recordid"].ToString();
                if (T_ver.Contains("WEB") != true)
                {
                    Sell_Cnt_1 = Sell_Cnt_1 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());  //금액                    
                }
                else
                {
                    Sell_Cnt_2 = Sell_Cnt_2 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][11].ToString());  //금액                    
                }

                Out_Cnt_1 = Out_Cnt_1  + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Send_itemCount1"].ToString()); //출고수량
                Out_Cnt_2 = Out_Cnt_2 + (double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString()) - double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["Send_itemCount1"].ToString())); //출고수량


                T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["Receive_Method_Name"].ToString();
                if (T_ver != "")
                {
                    T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["Receive_Method"].ToString();
                    if (T_ver == "1")
                        Out_Put_1++;
                    if (T_ver == "2")
                        Out_Put_2++;
                    if (T_ver == "3")
                        Out_Put_3++;
                }
                else               
                    Out_Put_4++; 
                



                T_ver = ds.Tables[base_db_name].Rows[fi_cnt]["Item_Name"].ToString();

                if (T_ver != "")
                {
                    if (Item_Cnt.ContainsKey(T_ver) == true)
                        Item_Cnt[T_ver] = Item_Cnt[T_ver] + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());  //금액                    
                    else
                        Item_Cnt[T_ver] = double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());  //금액                    
                }

            }

            //Reset_Chart_Total(Out_Cnt_1, Out_Cnt_2, 0);
            //Reset_Chart_Total(ref SelType_1);
            //Reset_Chart_Total(Sell_Cnt_1, Sell_Cnt_2);
            //Reset_Chart_Total(Out_Put_1, Out_Put_2, Out_Put_3, Out_Put_4);

            //foreach (string tkey in Item_Cnt.Keys)
            //{
            //    Push_data(series_Item, tkey, Item_Cnt[tkey]);
            //}


            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
                txt_P_2_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_10);
                txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, OrdCnt);
                //txt_P_6.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_15);
                //txt_P_7.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_16);        
            }
            
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();

            //dGridView_Base.Rows[0].Frozen = true;
            //dGridView_Base.la
        }



        private void dGridView_Base_Header_Reset()
        {
            
            cgb.grid_col_Count = 31;            
            cgb.basegrid = dGridView_Base;            
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 5;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;


            string[] g_HeaderText = {"주문번호"     , "주문_일자"   , "정산_일자", "출고 일자", "회원_번호"   
                                    , "성명"   , "_주민번호"   , "상품코드"    , "상품명"   , "개별단가"   
                                    , "개별PV"  , "개별CV"      ,  "주문_수량"  ,"출고_수량" , "총상품액"    
                                    , "총상품PV" , "총상품CV"    , "주문_종류"   , "구분"     , "배송구분"    
                                    , "우편번호"   ,  "배송지"     , "수령인명"    , "연락처1"  , "연락처2"    
                                    , "등록_센타명"   ,  "주문_센타명"     , ""  ,""  ,""   
                                    ,"승인여부"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 120, 90, 90, 0, 90
                             , 90 , 0   , 90  , 130 ,  80
                             , 0 , 0  , 80  , 0  ,  80 
                             , 0 , 0  , 0  , 80  ,  90 
                             , 70  , 100 , 110 , 100 ,  90 
                             , 110, 100 ,   0 ,   0 ,   0
                             ,  0                          
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                    
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true
                                     ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter//5
                            ,DataGridViewContentAlignment.MiddleCenter   //6
 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight //10

                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight   
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight  
                               ,DataGridViewContentAlignment.MiddleRight //15
 
                               ,DataGridViewContentAlignment.MiddleRight    
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter                            
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleLeft  //20

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft //25

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft //31
                      
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[16 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            
            cgb.grid_cell_format = gr_dic_cell_format;            
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][4].ToString () ,"Cpno")
 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][5]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][15]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][19].ToString ()                                
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][20].ToString ())
                                , ds.Tables[base_db_name].Rows[fi_cnt][21]  + encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][22].ToString () )

                                ,encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][23].ToString () )
                                ,ds.Tables[base_db_name].Rows[fi_cnt][24]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][25]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][26]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][27]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][28]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][29]
                 
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }


               
        

        private void txtData_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            if (sender is TextBox)
            {
                T_R.Text_Focus_All_Sel((TextBox)sender);
                TextBox tb = null;
                tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = cls_app_static_var.txt_Focus_Color;
            }

            if (sender is MaskedTextBox)
            {
                T_R.Text_Focus_All_Sel((MaskedTextBox)sender);
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = cls_app_static_var.txt_Focus_Color;
            }

            if (this.Controls.ContainsKey("Popup_gr"))
            {
                DataGridView T_Gd = (DataGridView)this.Controls["Popup_gr"];
                T_Gd.Visible = false;
                T_Gd.Dispose();
            }
        }

        private void txtData_Base_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = (TextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }

        }



        private void MtxtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            //회원번호 관련칸은 소문자를 다 대문자로 만들어 준다.
            if (e.KeyChar >= 97 && e.KeyChar <= 122)
            {
                string str = e.KeyChar.ToString().ToUpper();
                char[] ch = str.ToCharArray();
                e.KeyChar = ch[0];
            }

            if (e.KeyChar == 13)
            {        
                SendKeys.Send("{TAB}");
            }
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox tb = (MaskedTextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);            
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);

            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //숫자만 입력 가능하다.
                if (T_R.Text_KeyChar_Check(e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }


            else if ((tb.Tag != null) && (tb.Tag.ToString() == "ncode")) //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

        }



        private void MtxtData_Temp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    Data_Set_Form_TF = 1;
                    int SW = 0;
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        SW = 1;
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    Data_Set_Form_TF = 0;
                }
                else
                    SendKeys.Send("{TAB}");


            }
        }




        private bool Sn_Number_(string Sn, MaskedTextBox mtb, string sort_TF, int t_Sort2 = 0)
        {
            if (Sn != "")
            {

                bool check_b = false;
                cls_Sn_Check csn_C = new cls_Sn_Check();

                //sort_TF = "biz";  //사업자번호체크
                //sort_TF = "Tel";  //전화번호체크
                //sort_TF = "Zip";  //우편번호체크

                if (sort_TF == "Date")
                {
                    cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                    if (c_er.Input_Date_Err_Check__01(mtb) == false)
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                        mtb.Focus(); return false;
                    }
                }


                check_b = csn_C.Number_NotInput_Check(mtb.Text, sort_TF);

                if (check_b == false)
                {
                    if (sort_TF == "biz")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_BuNum")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                    if (sort_TF == "Tel")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Tel")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                    if (sort_TF == "Zip")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_AddCode")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                    if (sort_TF == "Date")
                    {
                        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Date")
                           + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    }

                    mtb.Focus(); return false;
                }
            }

            return true;
        }



        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false)  return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtCenter_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtCenter2_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0; 
            }


            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1; 
                if (tb.Text.Trim() == "")
                    txt_ItemName_Code2.Text = "";
                Data_Set_Form_TF = 0; 
            }
        }

        

        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter_Code,"");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtBank")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtCenter2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter2_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtCenter2_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtCenter2_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtSellCode_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemName2")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemName_Code2);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txt_ItemName_Code2, "");
                //else
                //    Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }
        }

        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (tb.Name == "txtCenter")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Select;

            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
        }



        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code, string strSql)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb ;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);
           
                if (tb.Name == "txtSellCode")
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;

                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowMemeberCenter";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtR_Id")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);
                }

                if (tb.Name == "txtBank")
                {
                    string Tsql;
                    Tsql = "Select Ncode ,BankName    ";
                    Tsql = Tsql + " From tbl_Bank (nolock) ";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " where  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

                if (tb.Name == "txtCenter2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }                                

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }

                if (tb.Name == "txt_ItemName2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name    ";
                    Tsql = Tsql + " From tbl_Goods (nolock) ";                    
                    //Tsql = Tsql + " Where GoodUse = 0 ";
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);
                }

                


            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql="";
            
            if (tb.Name == "txtCenter")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txtBank")
            {
                Tsql = "Select Ncode , BankName   ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%')";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
            }


            if (tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }
          

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";
            }

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();
            }

            if ((ReCnt > 1) || (ReCnt == 0)) Db_Grid_Popup(tb, tb1_Code, Tsql);
        }







        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(this, mtxtMbid);

                Reset_Chart_Total();

                combo_Se.SelectedIndex = -1;
                combo_Rec.SelectedIndex = -1;

                opt_Ed_1.Checked = true;  opt_sell_1.Checked = true;
                radioB_Rec_1.Checked = true;
                //radioB_S.Checked = true;
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                Reset_Chart_Total();

                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                txt_P_1.Text = ""; txt_P_2.Text = ""; txt_P_2_2.Text = ""; txt_P_3.Text = "";
                txt_P_4.Text ="";// txt_P_5.Text ="" ;txt_P_6.Text ="";
               // txt_P_7.Text ="";

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                combo_Rec_Code.SelectedIndex = combo_Rec.SelectedIndex;
                chart_Center.Series.Clear();
                Save_Nom_Line_Chart();   

                Base_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
           
            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name  == "butt_Exp")
            {
                if (bt.Text == "...")
                {
                    grB_Search.Height = button_base.Top + button_base.Height + 3;
                    bt.Text = ".";
                }
                else
                {
                    grB_Search.Height = butt_Exp.Top + butt_Exp.Height + 3;
                    bt.Text = "...";
                }
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Sell_Item_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            //SendKeys.Send("{TAB}");
        }


        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate1, mtxtSellDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }


        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(txtMakDate1, txtMakDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }










        private void Reset_Chart_Total()
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();

            double[] yValues = { 0, 0 };
            string[] xValues = { cm._chang_base_caption_search("출고"), cm._chang_base_caption_search("미출고") };
            chart_Mem.Series["Series1"].Points.DataBindXY(xValues, yValues);

            chart_Mem.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_Mem.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Mem.Legends[0].Enabled = true;

            string Tsql = "Select SellCode , SellTypeName ";
            Tsql = Tsql + " From tbl_SellType ";
            Tsql = Tsql + " Order BY SellCode  ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_SellType", ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt != 0)
            {
                double[] yValues_2 = new double[ReCnt];
                string[] xValues_2 = new string[ReCnt]; // { cm._chang_base_caption_search(""), cm._chang_base_caption_search("탈퇴") }; 

                for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
                {
                    yValues_2[fi_cnt] = 0;
                    xValues_2[fi_cnt] = ds.Tables["tbl_SellType"].Rows[fi_cnt]["SellTypeName"].ToString();
                }

                chart_Leave.Series["Series1"].Points.DataBindXY(xValues_2, yValues_2);

                chart_Leave.Series["Series1"].ChartType = SeriesChartType.Pie;
                chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                chart_Leave.Legends[0].Enabled = true;
            }


            double[] yValues_4 = { 0, 0 , 0 , 0 };
            string[] xValues_4 = { cm._chang_base_caption_search("직접수령"), cm._chang_base_caption_search("배송"), cm._chang_base_caption_search("센타수령"), cm._chang_base_caption_search("미입력") };
            chart_Rec.Series["Series1"].Points.DataBindXY(xValues_4, yValues_4);
            chart_Rec.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_Rec.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Rec.Legends[0].Enabled = true;


            double[] yValues_3 = { 0, 0 };
            string[] xValues_3 = { cm._chang_base_caption_search("일반"), cm._chang_base_caption_search("WEB") };
            chart_edu.Series["Series1"].Points.DataBindXY(xValues_3, yValues_3);
            chart_edu.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;

            chart_Center.Series.Clear();
            series_Item.Points.Clear();
        }



        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2, double SellCnt_3)
        {
            //chart_Mem.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Mem.Series.Clear();
            chart_Mem.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("출고"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("출고");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("미출고"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("미출고");
            series_Save.Points.Add(dp2);

            
        }

        private void Reset_Chart_Total(ref Dictionary<string, double> SelType_1)
        {

            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Leave.Series.Clear();
            chart_Leave.Series.Add(series_Save);
            int forCnt = 0;
            foreach (string tkey in SelType_1.Keys)
            {
                DataPoint dp = new DataPoint();
                series_Save.ChartType = SeriesChartType.Pie;
                dp.SetValueXY(tkey, SelType_1[tkey]);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SelType_1[tkey]);
                dp.LabelForeColor = Color.Black;
                dp.LegendText = tkey;
                series_Save.Points.Add(dp);
                forCnt++;
            }

            chart_Leave.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Leave.Legends[0].Enabled = true;
        }


         //Tsql = Tsql + " ,Case When Receive_Method = '1' Then '" + cm._chang_base_caption_search("직접수령") + "'";
         //   Tsql = Tsql + "  When Receive_Method = '2' Then '" + cm._chang_base_caption_search("배송") + "'";
         //   Tsql = Tsql + "  When Receive_Method = '3' Then '" + cm._chang_base_caption_search("센타수령") + "'";

        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2)
        {
            //chart_edu.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_edu.Series.Clear();
            chart_edu.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("일반"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("일반");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("WEB"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("WEB");
            series_Save.Points.Add(dp2);


            chart_edu.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_edu.Legends[0].Enabled = true;
        }

        private void Reset_Chart_Total(double SellCnt_1, double SellCnt_2, double SellCnt_3, double SellCnt_4)
        {
            //chart_rec.Series.Clear();
            cls_form_Meth cm = new cls_form_Meth();
            Series series_Save = new Series();

            chart_Rec.Series.Clear();
            chart_Rec.Series.Add(series_Save);

            DataPoint dp = new DataPoint();
            series_Save.ChartType = SeriesChartType.Pie;
            dp.SetValueXY(cm._chang_base_caption_search("직접수령"), SellCnt_1);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_1);
            dp.LabelForeColor = Color.Black;
            dp.LegendText = cm._chang_base_caption_search("직접수령");
            series_Save.Points.Add(dp);

            DataPoint dp2 = new DataPoint();

            dp2.SetValueXY(cm._chang_base_caption_search("배송"), SellCnt_2);
            dp2.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_2);
            dp2.LabelForeColor = Color.Black;
            dp2.LegendText = cm._chang_base_caption_search("배송");
            series_Save.Points.Add(dp2);


            DataPoint dp3 = new DataPoint();

            dp3.SetValueXY(cm._chang_base_caption_search("센타수령"), SellCnt_3);
            dp3.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_3);
            dp3.LabelForeColor = Color.Black;
            dp3.LegendText = cm._chang_base_caption_search("센타수령");
            series_Save.Points.Add(dp3);

            DataPoint dp4 = new DataPoint();

            dp4.SetValueXY(cm._chang_base_caption_search("미입력"), SellCnt_4);
            dp4.Label = string.Format(cls_app_static_var.str_Currency_Type, SellCnt_4);
            dp4.LabelForeColor = Color.Black;
            dp4.LegendText = cm._chang_base_caption_search("미입력");
            series_Save.Points.Add(dp4);


            chart_Rec.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Rec.Legends[0].Enabled = true;
        }



        private void Push_data(Series series, string p, double p_3)
        {
            if (p != "")
            {
                DataPoint dp = new DataPoint();

                if (p.Replace(" ", "").Length >= 5)
                    dp.SetValueXY(p.Replace(" ", "").Substring(0, 5), p_3);
                else
                    dp.SetValueXY(p.Replace(" ", ""), p_3);

                dp.Font = new System.Drawing.Font("맑은고딕", 9);
                dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3);
                series.Points.Add(dp);
            }
        }



        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();

            chart_Center.Series.Clear();
            series_Item.Points.Clear();

            series_Item["DrawingStyle"] = "Emboss";
            series_Item["PointWidth"] = "0.4";
            series_Item.Name = cm._chang_base_caption_search("판매수량");

            series_Item.ChartType = SeriesChartType.Column;

            chart_Center.Series.Add(series_Item);
            chart_Center.ChartAreas[0].AxisX.Interval = 1;
            chart_Center.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Center.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 8;
            //chart_Center.ChartAreas[0].AxisY.Interval = 5000000;

            chart_Center.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart_Center.Legends[0].Enabled = true;

        }

        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                Parent_but_Exp_Base_Width = panel4.Width;
                but_Exp_Base_Left = but_Exp.Left;

                panel4.Width = but_Exp.Width;
                but_Exp.Left = 0;
                but_Exp.Text = ">>";
            }
            else
            {
                panel4.Width = Parent_but_Exp_Base_Width;
                but_Exp.Left = but_Exp_Base_Left;
                but_Exp.Text = "<<";


            }
        }










        




































    }
}
