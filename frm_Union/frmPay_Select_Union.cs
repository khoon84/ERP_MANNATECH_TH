﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MLM_Program
{
    public partial class frmPay_Select_Union : Form
    {
        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
              
        cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;
        //public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;

        public frmPay_Select_Union()
        {
            InitializeComponent();
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

            combo_Pay.Items.Add("");
            combo_Pay.Items.Add("일일정산");
            combo_Pay.Items.Add("주간정산");
            combo_Pay.Items.Add("월정산");
            combo_Pay.Items.Add("센터정산");
           
            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtMbid2.Mask = cls_app_static_var.Member_Number_Fromat;


            mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtOutDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtOutDate2.Mask = cls_app_static_var.Date_Number_Fromat;


            txt_P_1.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_P_2.BackColor = cls_app_static_var.txt_Enable_Color;
            //grB_Search.Height = mtxtMbid.Top + mtxtMbid.Height + 3;
            
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

            cfm.button_flat_change(butt_Check_02);
            cfm.button_flat_change(butt_Check_01);
            cfm.button_flat_change(butt_Save); 
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
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

                            //cls_form_Meth cfm = new cls_form_Meth();
                            // cfm.form_Group_Panel_Enable_True(this);

                            //SendKeys.Send("{TAB}");
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
            }

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

            if (mtxtOutDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtOutDate.Text, mtxtOutDate, "Date") == false)
                {
                    mtxtOutDate.Focus();
                    return false;
                }

            }

            if (mtxtOutDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtOutDate2.Text, mtxtOutDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus();
                    return false;
                }
            }



            return true;
        }


        private void Make_Base_Query(ref string Tsql, string Sub_sql )
        {

            //string[] g_HeaderText = {"선택" ,"회원_번호", "성명"  , "마감일자"  , "지급일자"   
            //                     ,  "주민번호"   , "은행명"  , "발생액"     , "차액"    , "실지급액" 
            //                     , "구분1//"    , "구분2//" , "신고일자" , "수당명칭"   , "은행코드"   
            //                     , "계좌번호" , "예금주"    , "교육일자"      , ""    , ""                                                                
            //                        };
            //Tsql = Tsql + ", LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2) ";            

            cls_form_Meth cm = new cls_form_Meth();
            
            Tsql = " Select  '' ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", (C.mbid + '-' + Convert(Varchar,C.mbid2)) AS M_ID ";
            else
                Tsql = Tsql + ", C.mbid2 AS M_ID";

            Tsql = Tsql + " ,C.M_Name ";
            Tsql = Tsql + " ,ToEndDate ";
            Tsql = Tsql + " ,PayDate ";

            
            Tsql = Tsql + ", C.Cpno  ";

            Tsql = Tsql + " ,C.BankName ";
            Tsql = Tsql + " ,SumAllAllowance ";
            Tsql = Tsql + " ,0 ";
            Tsql = Tsql + " ,TruePayment ";
            Tsql = Tsql + " ,PayTF ";
            Tsql = Tsql + " ,'' ";

            Tsql = Tsql + " ,RepDate ";
            Tsql = Tsql + " ,'수당' , C.BankCode, C.BankAcc , C.BankOwner  ";
            Tsql = Tsql + " ,tbl_Memberinfo.ED_DAte ";
            Tsql = Tsql + " ,'','' ";
            Tsql = Tsql + " From ";

            Tsql = Tsql + "  ( ";
            if (combo_Pay.SelectedIndex == 1 || combo_Pay.Text.Trim() == "")
            {
                Tsql = Tsql + " Select 1 PayTF , ";
                Tsql = Tsql + " Mbid , Mbid2 , M_Name , Cpno , Isnull(Bankname,'')  BankName, BankCode , BankAcc , BankOwner, ";
                Tsql = Tsql + " PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " ,Isnull(( Select Max(RepDate) From T_REALMLM_Pay_Send_TF  (nolock) A Where A.ToEndDate = ToEndDate And A.Mbid=Mbid And A.Mbid2 = Mbid2 And A.PayTF= 1 ),'')  RepDate ";
                Tsql = Tsql + " From tbl_ClosePay_01_Mod (nolock)  ";
                Tsql = Tsql + " LEFT JOIN  tbl_Bank (nolock)  ON tbl_ClosePay_01_Mod.BankCode = tbl_Bank.Ncode ";

                Tsql = Tsql + Sub_sql;

                if (combo_Pay.Text.Trim() == "")
                    Tsql = Tsql + " UNION ALL";
            }

            if (combo_Pay.SelectedIndex == 2 || combo_Pay.Text.Trim() == "")
            {
                Tsql = Tsql + " Select 2 PayTF,";
                Tsql = Tsql + " Mbid , Mbid2 , M_Name, Cpno , Isnull(Bankname,'') BankName, BankCode , BankAcc , BankOwner, ";
                Tsql = Tsql + " PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " ,Isnull(( Select Max(RepDate) From T_REALMLM_Pay_Send_TF  (nolock) A Where A.ToEndDate = ToEndDate And A.Mbid=Mbid And A.Mbid2 = Mbid2 And A.PayTF= 2 ),'')  RepDate ";
                Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
                Tsql = Tsql + " LEFT JOIN  tbl_Bank (nolock)  ON tbl_ClosePay_04_Mod.BankCode = tbl_Bank.Ncode ";

                Tsql = Tsql + Sub_sql;

                if (combo_Pay.Text.Trim() == "")
                    Tsql = Tsql + " UNION ALL";
            }

            if (combo_Pay.SelectedIndex == 3 || combo_Pay.Text.Trim() == "")
            {
                Tsql = Tsql + " Select 4 PayTF, ";
                Tsql = Tsql + " Mbid , Mbid2, M_Name, Cpno , Isnull(Bankname,'') BankName, BankCode , BankAcc , BankOwner, ";
                Tsql = Tsql + " PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " ,Isnull(( Select Max(RepDate) From T_REALMLM_Pay_Send_TF  (nolock) A Where A.ToEndDate = ToEndDate And A.Mbid=Mbid And A.Mbid2 = Mbid2 And A.PayTF= 4 ),'')  RepDate ";
                Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
                Tsql = Tsql + " LEFT JOIN  tbl_Bank (nolock)  ON tbl_ClosePay_04_Mod.BankCode = tbl_Bank.Ncode ";

                Tsql = Tsql + Sub_sql;

                if (combo_Pay.Text.Trim() == "")
                    Tsql = Tsql + " UNION ALL";
            }

            if (combo_Pay.SelectedIndex == 4 || combo_Pay.Text.Trim() == "")
            {
                Tsql = Tsql + " Select 10 PayTF,";
                Tsql = Tsql + " Mbid , Mbid2, M_Name, Cpno , Isnull(Bankname,'') BankName, BankCode , BankAcc , BankOwner, ";
                Tsql = Tsql + " PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
                Tsql = Tsql + " ,Isnull(( Select Max(RepDate) From T_REALMLM_Pay_Send_TF  (nolock) A Where A.ToEndDate = ToEndDate And A.Mbid=Mbid And A.Mbid2 = Mbid2 And A.PayTF= 10 ),'')  RepDate ";
                Tsql = Tsql + " From tbl_ClosePay_10_Mod (nolock)  ";
                Tsql = Tsql + " LEFT JOIN  tbl_Bank (nolock)  ON tbl_ClosePay_10_Mod.BankCode = tbl_Bank.Ncode ";

                Tsql = Tsql + Sub_sql;
            }
            
            Tsql = Tsql + " )AS C  ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo (nolock) ON tbl_Memberinfo.Mbid = C.Mbid And tbl_Memberinfo.Mbid2 = C.Mbid2 ";

            Tsql = Tsql + " Where tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            if (opt_Ed_2.Checked == true)
                Tsql = Tsql + " And  tbl_Memberinfo.Ed_Date <> ''";

            if (opt_Ed_3.Checked == true)
                Tsql = Tsql + " And  tbl_Memberinfo.Ed_Date = ''";

            if (opt_C_01.Checked == true)
            {
                Tsql = Tsql + " And (Convert(varchar,C.PayTF ) +'-' + C.ToEndDate + '-' + C.Mbid + '-' + Convert(varchar,C.Mbid2)) NOT IN";
                Tsql = Tsql + " (Select (Convert(varchar,PayTF ) +'-' + ToEndDate + '-' + Mbid + '-' + Convert(varchar,Mbid2)) From T_REALMLM_Pay_Send_TF ) ";
            }

            if (opt_C_02.Checked == true)
            {
                Tsql = Tsql + " And (Convert(varchar,C.PayTF ) +'-' + C.ToEndDate + '-' + C.Mbid + '-' + Convert(varchar,C.Mbid2)) IN";
                Tsql = Tsql + " (Select (Convert(varchar,PayTF ) +'-' + ToEndDate + '-' + Mbid + '-' + Convert(varchar,Mbid2)) From T_REALMLM_Pay_Send_TF ) ";
            }


            Tsql = Tsql + " Order By C.ToEndDate Asc,C.mbid,C.mbid2,C.PayDate  ";
                        
        }



        private void Make_Base_Query_(ref string Tsql)
        {
            //미승인 내역이면서 정상 판매 내역만 불러온다. 반품이나 교환같은것들은 안불러온다.
            //string strSql = " Where tbl_SalesDetail_TF.SellTF = 1   ";



            string strSql = " Where SumAllAllowance >0 ";

            //우선은 한번 신고한 내역 다시 신고 할수 있도록 처리 햇음.
            //strSql = strSql + " And tbl_SalesitemDetail.OrderNumber +'-' + Convert(Varchar, tbl_SalesitemDetail.Salesitemindex ) Not IN  ";
            //strSql = strSql + " (Select OrderNumber +'-' + Convert(Varchar, Salesitemindex )  From T_REALMLM_Stock_Send_TF )";
   

            string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 = " + Mbid2;
                }


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
                        strSql = strSql + " And Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And Mbid2 <= " + Mbid2;
                }
            }


            //회원명으로 검색
            if (txtName.Text.Trim() != "")
                strSql = strSql + " And M_Name Like '%" + txtName.Text.Trim() + "%'";

            //가입일자로 검색 -1
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And ToEndDate = '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate1.Text.Replace("-", "").Trim() != "") && (mtxtSellDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And ToEndDate >= '" + mtxtSellDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And ToEndDate <= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            }


            //가입일자로 검색 -1
            if ((mtxtOutDate.Text.Replace("-", "").Trim() != "") && (mtxtOutDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And PayDate = '" + mtxtOutDate.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtOutDate.Text.Replace("-", "").Trim() != "") && (mtxtOutDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And PayDate >= '" + mtxtOutDate.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And PayDate <= '" + mtxtOutDate2.Text.Replace("-", "").Trim() + "'";
            }
            
            Tsql =  strSql;            
        }




        private void Base_Grid_Set()
        {
            string Tsql = ""; string Sub_sql = "";

            Make_Base_Query_(ref Sub_sql);

            Make_Base_Query(ref Tsql, Sub_sql);

            

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            double Sum_11 = 0; double Sum_12 = 0; double Sum_13 = 0;
            //double Sum_13 = 0; //double Sum_14 = 0; double Sum_15 = 0;
            //double Sum_16 = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Sum_11 = fi_cnt;
                Sum_12 = Sum_12 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());
                Sum_13 = Sum_13 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                //Sum_14 = Sum_14 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][13].ToString());                
            }

            if (gr_dic_text.Count > 0)
            {
                txt_P_1.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_11);
                txt_P_2.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                txt_P_3.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_13);
                //txt_P_4.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_12);
                //txt_P_5.Text = string.Format(cls_app_static_var.str_Currency_Type, Sum_14);
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

            cgb.grid_col_Count = 20;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 4;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;



            string[] g_HeaderText = {"선택" ,"회원_번호", "성명"  , "마감일자"  , "지급일자"   
                                 ,  "주민번호"   , "은행명"  , "발생액"     , "차액"    , "실지급액" 
                                 , "구분1//"    , "구분2//" , "신고일자" , "수당명칭"   , "은행코드"   
                                 , "계좌번호" , "예금주"    , "교육일자"      , ""    , ""                                                                
                                    };

            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 90, 110, 120, 90  
                             , 90 ,90, 120, 80, 110
                             , 0  ,0 , 120, 80, 80
                             , 80  ,100 , 110, 0 , 0                                                                                       
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                                 
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight 
                               ,DataGridViewContentAlignment.MiddleRight //10

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleLeft  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //20

                      
                              };
            cgb.grid_col_alignment = g_Alignment;


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
 
                                ,encrypter.Decrypt( ds.Tables[base_db_name].Rows[fi_cnt][5].ToString() ,"Cpno_Union")
                                ,ds.Tables[base_db_name].Rows[fi_cnt][6]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][7]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][8]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][9]

                                ,ds.Tables[base_db_name].Rows[fi_cnt][10]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][11]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][12]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][13]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][14]

                                ,encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][15].ToString () ) 
                                ,ds.Tables[base_db_name].Rows[fi_cnt][16]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][17]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][18]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][19]
                 
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


        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            //int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                //Sw_Tab = 1;
            }

            //if (tb.Name == "txtCenter")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
            //}

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter2_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);
            //}

            //if (tb.Name == "txtSellCode")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtSellCode_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtSellCode_Code);
            //}


            //if (tb.Name == "txt_ItemName2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txt_ItemName_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);
            //}
        }


        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {            
            //if (tb.Name == "txtCenter")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code,"");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code);
                
            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtCenter2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter2_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter2_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txt_ItemName2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txt_ItemName_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txt_ItemName_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}
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

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtCenter4")
                cgb_Pop.Next_Focus_Control = butt_Select;

            if (tb.Name == "txtIO")
            {
                cgb_Pop.Next_Focus_Control = butt_Select;
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode, "", " And  (Ncode ='004' OR Ncode = '005'  OR Ncode = '006' ) ");
            }
            else
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
            //Tsql = Tsql + " And  (Ncode ='004' OR Ncode = '005' ) ";


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
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2" ))
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);
            }
            else
            {
                if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
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
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
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

            if ((tb.Name == "txtCenter") || (tb.Name == "txtCenter2"))
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
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
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%'";
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





        private void Base_Select_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Check_01")
            {
                for (int i = 0; i < dGridView_Base.RowCount; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }                
            }


            else if (bt.Name == "butt_Check_02")
            {
                for (int i = 0; i < dGridView_Base.RowCount; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }

            }

            else if (bt.Name == "butt_Save")
            {
                Boolean chage_Center_tf = false;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                progress.Visible = true; progress.Value = 0;
                chage_Center_tf = Chang_CenterCode();  //실질적인 센터 변경이 이루어지는 메소드
                progress.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                if (chage_Center_tf == true)
                {
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                    cgb.d_Grid_view_Header_Reset();
                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                    cls_form_Meth ct = new cls_form_Meth();
                    ct.from_control_clear(this, mtxtMbid);

                    opt_Line_1.Checked = true; opt_Leave_1.Checked = true;
                    chk_Total.Checked = false; 
                }
             }
        }

        private Boolean Chang_CenterCode()
        {
            //Msg_Useing_Not_Data
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Check_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
            if (Check_TextBox_Save_Error() == false) return false;
            
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();
             
            string StrSql = "";
            cls_form_Meth cm = new cls_form_Meth();
            try
            {
                //string T_Mbid = "";                 
                string ToEndDate = ""; string PayDate = ""; double RealPay = 0;
                int PayTF = 0; string Mbid = ""; int Mbid2 = 0; double PayTotal = 0; int P_Index_2 = 0;
                string MEMBER_NUM = "";
                string Mem_Name = ""; string Mem_Cpno = ""; string Mem_BankName = "";
                cls_Search_DB csd = new cls_Search_DB();
                
                progress.Maximum = dGridView_Base.Rows.Count + 1;
                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        MEMBER_NUM = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                        ToEndDate = dGridView_Base.Rows[i].Cells[3].Value.ToString();
                        PayDate = dGridView_Base.Rows[i].Cells[4].Value.ToString();
                        PayTotal = double.Parse(dGridView_Base.Rows[i].Cells[7].Value.ToString());
                        RealPay = double.Parse(dGridView_Base.Rows[i].Cells[9].Value.ToString());
                        PayTF = int.Parse(dGridView_Base.Rows[i].Cells[10].Value.ToString());
                        P_Index_2 = i;

                        Mem_BankName = dGridView_Base.Rows[i].Cells[6].Value.ToString();
                        Mem_Cpno = dGridView_Base.Rows[i].Cells[5].Value.ToString();
                        Mem_Name = dGridView_Base.Rows[i].Cells[2].Value.ToString();


                        csd.Member_Nmumber_Split(MEMBER_NUM, ref Mbid, ref Mbid2);

                        StrSql = "Insert into  T_REALMLM_Pay_Send_TF (" ;
                        StrSql = StrSql + " PayTF ,  FromEndDate , ToEndDate , PayDate, RepDate ,  Mbid , Mbid2 , PayTotal , RealPay," ;
                        StrSql = StrSql + " MEMBER_NUM, P_Index_1, P_Index_2 ";
                        StrSql = StrSql + " ) Values (" ;
                        StrSql = StrSql + PayTF + ",'','" + ToEndDate + "','" + PayDate + "', Replace(LEFT(Convert(Varchar(25),GetDate(),21),10),'-','') " ;
                        StrSql = StrSql + ",'" + Mbid + "'," + Mbid2 + "," + PayTotal + "," + RealPay;
                        StrSql = StrSql + ",'" + MEMBER_NUM + "',0," + P_Index_2 ;
                        StrSql = StrSql + " )" ;
            
                        Temp_Connect.Insert_Data(StrSql, "T_REALMLM_Pay_Send_TF", Conn, tran, this.Name.ToString(), this.Text);
                        //수당내역을 조합에 보냇다는 로그 파일을 만든다. 추후에 다시 안보내기 위해서인지 아님.. 몇번 보냇는지 남기기 위함인지.
                        

                        StrSql = "EXEC p_mlmunion_Pay '" + cls_app_static_var.T_Company_Code + "'," ;
                        StrSql = StrSql + "'" + PayTF + "'," ;
                        StrSql = StrSql + P_Index_2 + "," ;
                        StrSql = StrSql + "'" + MEMBER_NUM + "',";
                        StrSql = StrSql + "'" + Mem_Cpno + "'," ;
                        StrSql = StrSql + "'" + Mem_Name + "'," ;
                        StrSql = StrSql + "'" + Mem_BankName + "'," ;
                        StrSql = StrSql + "'" + ToEndDate + "'," ;
                        StrSql = StrSql + "'" + PayDate + "'," ;
                        StrSql = StrSql + PayTotal + "," ;
                        StrSql = StrSql + (PayTotal - RealPay) + "," ;
                        StrSql = StrSql + RealPay ;
                        
                        Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text);                                                                                              
                    }

                    progress.Value = progress.Value + 1;
                }
                tran.Commit();


                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_App"));
                return true;
            }


            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_App_Err"));
                return false;
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }
        }


        private Boolean Check_TextBox_Save_Error()
        {
            int chk_cnt = 0; string Min_SellDate = cls_User.gid_date_time ;
            //string S_SellDate = "";
            for (int i = 0; i < dGridView_Base.Rows.Count ; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;
                    //S_SellDate = "";
                    //if (dGridView_Base.Rows[i].Cells[7].Value.ToString() != "") 
                    //{
                    //    S_SellDate = dGridView_Base.Rows[i].Cells[7].Value.ToString().Replace ("-","") ;

                    //    if (int.Parse (Min_SellDate) > int.Parse (S_SellDate))
                    //        Min_SellDate = S_SellDate ; 
                    //}
                    
                }
            }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.

            if (chk_cnt == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select") + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                return false;
            } //end if 체크를 해서 선택한 내역이 없을 경우 메시지 뛰우고나간다.


            //S_SellDate = S_SellDate.Substring (0,4) +'-' + S_SellDate.Substring (4,2) + '-' + S_SellDate.Substring (6,2) ;
            //string  S_SellDate2  =cls_User.gid_date_time.Substring (0,4) +'-' + cls_User.gid_date_time.Substring (4,2) + '-' + cls_User.gid_date_time.Substring (6,2) ;

            //cls_Date_G date_G = new cls_Date_G();
            //double dif = date_G.DateDiff("d", DateTime.Parse(S_SellDate), DateTime.Parse(S_SellDate2));
                        
            //if (dif > 5)
            //{
            //    while ( DateTime.Parse(S_SellDate) <= DateTime.Parse(S_SellDate2))
            //    {
            //        int r_d = date_G.Check_Date_HolyDay_TF(DateTime.Parse(S_SellDate));
            //        dif = dif + r_d;

            //        DateTime TodayDate = new DateTime();
            //        TodayDate = DateTime.Parse(S_SellDate);
            //        S_SellDate = TodayDate.AddDays(1).ToString("yyyy-MM-dd");
            //    }
            //}

            //if (dif > 5)
            //{
            //    string t_Msg = "";                
            //    t_Msg = "현재일 기준으로 5영업일이 지난 판매일자의 내역이 존재합니다." + "\n" +
            //        "5영일이 지난 내역에 대해서는 특판측에서 날짜를 열어 놓아야 합니다." + "\n" + 
            //        "현 선택 하신 내역에 대한 신고를 하시겠습니까?";

            //    if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(t_Msg), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
            //}


            // //주문일자를 넣었는지 먼저 체크한다. 안넣었으며 말고
            //if (txtSellDate.Text.Trim() != "")
            //{
            //    int Ret = 0;
            //    cls_Check_Input_Error c_er = new cls_Check_Input_Error();
            //    Ret = c_er.Input_Date_Err_Check(txtSellDate);

            //    if (Ret == -1)
            //    {
            //        txtSellDate.Focus(); return false;
            //    }
            //}

            //if (txtSellDate.Text.Trim ()  != "")
            //{
            //    //마감정산이 이루어진 판매 날짜인지 체크한다.
            //    cls_Search_DB csd= new cls_Search_DB ();    
            //    if (csd.Close_Check_SellDate("tbl_CloseTotal_01", txtSellDate.Text.Trim()) == false)
            //    {
            //        txtSellDate.Focus(); return false;
            //    }
            //}
            //else
            //{
            //    //마감정산이 이루어진 판매 날짜인지 체크한다. 선택된 내역들 중에서 날짜 중에서.
            //    cls_Search_DB csd= new cls_Search_DB ();    
            //    if (csd.Close_Check_SellDate("tbl_CloseTotal_01", Min_SellDate.Trim()) == false)
            //    {
            //        butt_Save.Focus(); return false;
            //    }
            //}

            return true;
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
                txt_P_1.Text = ""; txt_P_2.Text = "";

                combo_Pay.SelectedIndex = -1;
                
                //radioB_S.Checked = true; radioB_R.Checked = true;
                 opt_sell_1.Checked = true;                 opt_C_01.Checked = true; 
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                txt_P_1.Text = ""; txt_P_2.Text = "";
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;                
                Base_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }
           
            else if (bt.Name == "butt_Excel")
            {
                //frmBase_Excel e_f = new frmBase_Excel();
                //e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                //e_f.ShowDialog();
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
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }

       

        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            //if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            //{
            //    string Send_Nubmer = ""; string Send_Name = "";
            //    Send_Nubmer = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
            //    Send_Name = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
            //    Send_Mem_Number(Send_Nubmer, Send_Name);   //부모한테 이벤트 발생 신호한다.
            //}            
        }


        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;

            if (e.RowIndex < 0) return;

            if (dg.CurrentCell.ColumnIndex != 0) return;

            if (dg.CurrentRow.Cells[0].Value.ToString() == "V")
            {
                dg.CurrentCell.Value = "";
            }

            else
            {
                dg.CurrentCell.Value = "V";
            }


        }



        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
           // SendKeys.Send("{TAB}");
        }













    

 
 
        private void PopUp(object sender, EventArgs e)
        {
            MenuItem miClicked = null;
 
            if (sender is MenuItem)
                miClicked = (MenuItem)sender;
            else
                return;

            cls_form_Meth cm = new cls_form_Meth();
            string menu_Caption = cm._chang_base_caption_search("선택_내역_체크"); ;        

            string item = miClicked.Text;

            if (item == menu_Caption)
            {

                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Selected == true )                    
                        dGridView_Base.Rows[i].Cells[0].Value = "V";
                    
                }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.

            }
        }

        private void dGridView_Base_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pp = new Point ();
                pp.X = e.X;
                pp.Y = e.Y;

                DataGridView dg = (DataGridView)sender;
                dg.ContextMenu.Show(dg, pp);
            }
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
            ct.Search_Date_TextBox_Put(mtxtOutDate, mtxtOutDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }


        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            EventArgs ee = null;
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            if (chk_Total.Checked == true)
            {
                Base_Select_Button_Click(butt_Check_01, ee);
            }
            else
                Base_Select_Button_Click(butt_Check_02, ee);

            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells ;
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Selected == true)
                    dGridView_Base.Rows[i].Cells[0].Value = "V";

            }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.


            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }





























































    }
}
