﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;
using System.Reflection;

namespace MLM_Program
{
    public partial class frmMember_Select_Not_Sell : Form
    {

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);



          cls_Grid_Base cgb = new cls_Grid_Base();
        private const string base_db_name = "tbl_SalesDetail";
        private int Data_Set_Form_TF;

        Series series_Center = new Series();
        Series series_User = new Series();

        //public delegate void SendNumberDele(string Send_Number, string Send_Name);
        //public event SendNumberDele Send_Mem_Number;



        public frmMember_Select_Not_Sell()
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


            Save_Nom_Line_Chart();


            string[] data_P = { cm._chang_base_caption_search("1")
                               ,cm._chang_base_caption_search("2")
                               ,cm._chang_base_caption_search("3")                               
                               ,cm._chang_base_caption_search("4")                               
                               ,cm._chang_base_caption_search("5")                               
                               ,cm._chang_base_caption_search("6")                               
                               ,cm._chang_base_caption_search("7")                               
                               ,cm._chang_base_caption_search("8")                               
                               ,cm._chang_base_caption_search("9")                               
                               ,cm._chang_base_caption_search("10")                               
                               ,cm._chang_base_caption_search("11")                               
                               ,cm._chang_base_caption_search("12")                               
                              };

            // 각 콤보박스에 데이타를 초기화
            combo_Sort.Items.AddRange(data_P);
            combo_Sort.SelectedIndex = -1;


            mtxtCDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtLeaveDate.Mask = cls_app_static_var.Date_Number_Fromat;
        }




        private void frm_Base_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();
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

            cfm.button_flat_change(butt_Check_01);
            cfm.button_flat_change(butt_Check_02);
            cfm.button_flat_change(butt_Save);  
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
                            //cfm.form_Group_Panel_Enable_True(this);
                        }
                    }
                }// end if

            }

            ////그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            //if (sender is DataGridView)
            //{
                

            //    if (e.KeyValue == 13)
            //    {
            //        EventArgs ee =null;
            //        dGridView_Base_DoubleClick(sender, ee);
            //        e.Handled = true;
            //    } // end if
            //}

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

            //if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")               
            //{
            //    int Ret = 0;
            //    Ret = c_er._Member_Nmumber_Split(mtxtMbid);

            //    if (Ret == -1)
            //    {                    
            //        mtxtMbid.Focus();     return false;
            //    }   
            //}


            //if (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er._Member_Nmumber_Split(mtxtMbid2);

            //    if (Ret == -1)
            //    {
            //        mtxtMbid2.Focus(); return false;
            //    }   
            //}


            if (mtxtCDate1.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Requisite_Data")
                       + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_SearchBase_Date")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtCDate1.Focus(); return false;
            }

            if (combo_Sort.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Requisite_Data")
                       + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_SearchBase_Date2")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtCDate1.Focus(); return false;
            }
            

            if (mtxtCDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtCDate1.Text, mtxtCDate1, "Date") == false)
                {
                    mtxtCDate1.Focus();
                    return false;
                }

            }                     

            return true;
        }


        private void Make_Base_Query(ref string Tsql)
        {

            //string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "가입일"   , "후원인"        
            //                    , "후원인명"   , "추천인"    , "추천인명"  , "마지막판매일"
            //                    ,"기준일이후판매횟수" , "센타"  ,"" ,"" ,""
            //                        };

            string sdate = "";
            DateTime TodayDate = new DateTime();
            string t_string = mtxtCDate1.Text.Replace("-", "").Trim().Substring(0, 4) + "-" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(4, 2) + "-" + mtxtCDate1.Text.Replace("-", "").Trim().Substring(6, 2);
            TodayDate = DateTime.Parse(t_string);
         

            sdate = TodayDate.AddMonths(-int.Parse(combo_Sort.Text .ToString ())).ToString("yyyy/MM/dd hh:mm");
            sdate = sdate.Substring(0, 10).Replace("-", "");


            Tsql = "Select  '',";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " mbid + '-' + Convert(Varchar,mbid2) ";
            else
                Tsql = Tsql + " mbid2 ";

            Tsql = Tsql + " ,M_Name ";
            
           Tsql = Tsql + ",  Cpno  ";

            Tsql = Tsql + " , Isnull(regtime,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,Saveid + '-' + Convert(Varchar,Saveid2) ";
            else
                Tsql = Tsql + " ,Saveid2 ";

            Tsql = Tsql + " , Isnull(B_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,Nominid + '-' + Convert(Varchar,Nominid2) ";
            else
                Tsql = Tsql + " ,Nominid2 ";

            Tsql = Tsql + " , Isnull(C_Name,'') ";
            Tsql = Tsql + " ,M_sellDate ";
            Tsql = Tsql + " ,Sell_01_Cnt ";
            Tsql = Tsql + " ,BusName ";
            Tsql = Tsql + " ,LeaveDate ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            
            Tsql = Tsql + " FROM ufn_Member_Search_Leave('" + sdate + "') ";          
        }




        private void Make_Base_Query_(ref string Tsql)
        {
            string strSql = " Where Mbid2 >= 0 ";
            //// strSql = strSql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

            //string Mbid = ""; int Mbid2 = 0;
            ////회원번호1로 검색
            //if (
            //    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "") 
            //    &&
            //    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "") 
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        strSql = strSql + " And tbl_Memberinfo.Mbid = '" + Mbid + "'";
            //        strSql = strSql + " And tbl_Memberinfo.Mbid2 = " + Mbid2;
            //    }
            //}

            ////회원번호2로 검색
            //if (
            //    (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    &&
            //    (mtxtMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_Memberinfo.Mbid >='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_Memberinfo.Mbid2 >= " + Mbid2;
            //    }

            //    if (csb.Member_Nmumber_Split(mtxtMbid2.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_Memberinfo.Mbid <='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_Memberinfo.Mbid2 <= " + Mbid2;
            //    }
            //}


            ////회원명으로 검색
            //if (txtName.Text.Trim() != "")
            //    strSql = strSql + " And tbl_Memberinfo.M_Name Like '%" + txtName.Text.Trim() + "%'";





            ////변경일자로 검색 -1
            //if ((txtCDate1.Text.Trim() != "") && (txtCDate2.Text.Trim() == ""))
            //    strSql = strSql + " And Replace(Left(A.ModRecordtime,10),'-','')  = '" + txtCDate1.Text.Trim() + "'";

            ////변경일자로 검색 -2
            //if ((txtCDate1.Text.Trim() != "") && (txtCDate2.Text.Trim() != ""))
            //{
            //    strSql = strSql + " And Replace(Left(A.ModRecordtime,10),'-','') >= '" + txtCDate1.Text.Trim() + "'";
            //    strSql = strSql + " And Replace(Left(A.ModRecordtime,10),'-','')   <= '" + txtCDate2.Text.Trim() + "'";
            //}

                       

            //센타코드로으로 검색
            if (txtCenter_Code.Text.Trim() != "")
                strSql = strSql + " And BusCode = '" + txtCenter_Code.Text.Trim() + "'";

            if (opt_Leave_2.Checked == true)
                strSql = strSql + " And LeaveDate = ''";

            if (opt_Leave_3.Checked == true)
                strSql = strSql + " And LeaveDate <> ''";

            strSql = strSql + " And BusCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";

            Tsql = Tsql + strSql ;
            Tsql = Tsql + " Order by Mbid, Mbid2 ASC   ";
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
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name , this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            string Cen_Code = "";
            Dictionary<string, int> dic_Center = new Dictionary<string, int>();            

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                Cen_Code = ds.Tables[base_db_name].Rows[fi_cnt]["BusName"].ToString();                

                if (dic_Center.ContainsKey(Cen_Code) == false)
                    dic_Center[Cen_Code] = 1;
                else
                    dic_Center[Cen_Code]++;
                
            }

            if (gr_dic_text.Count > 0)
            {
                foreach (string t_key in dic_Center.Keys)
                {
                    Push_data(series_Center, t_key, double.Parse(dic_Center[t_key].ToString()));
                }               
            }


            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();     
        }



        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 15;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 3;            
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = {"선택" , "회원_번호"  , "성명"   , "주민번호"  , "가입일"   
                                     , "후원인"   , "후원인명"   , "추천인"    , "추천인명"   , "마지막판매일"
                                 ,"기준일이후판매횟수" , "센타"   ,"탈퇴일" ,"" , ""
                                    };


            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 85, 90, 130, 100, 100  
                             ,cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.save_uging_Pr_Flag , cls_app_static_var.nom_uging_Pr_Flag , cls_app_static_var.nom_uging_Pr_Flag,100
                             ,100, 90 , 80 , 0 , 0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,  true
                                    ,true , true,  true,  true ,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter             
                               ,DataGridViewContentAlignment.MiddleCenter             
                                ,DataGridViewContentAlignment.MiddleCenter  //5

                                ,DataGridViewContentAlignment.MiddleCenter                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter             
                               ,DataGridViewContentAlignment.MiddleCenter             
                                ,DataGridViewContentAlignment.MiddleCenter  //5
                              };

            cgb.grid_col_alignment = g_Alignment;
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {


            string str_3 = "";

            cls_form_Meth cm = new cls_form_Meth();
            str_3 = cm._chang_base_caption_search(ds.Tables[base_db_name].Rows[fi_cnt][3].ToString());

            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,encrypter.Decrypt (ds.Tables[base_db_name].Rows[fi_cnt][3].ToString (),"Cpno")
                                ,ds.Tables[base_db_name].Rows[fi_cnt][4]
                                 
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
                    string Sn = mtb.Text.Replace("-", "").Replace("_", "").Trim();
                    if (mtb.Name == "mtxtBiz1")
                    {
                        if (Sn_Number_(Sn, mtb, "biz") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtTel2")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    if (mtb.Name == "mtxtZip1")
                    {
                        if (Sn_Number_(Sn, mtb, "Tel") == true)
                            SendKeys.Send("{TAB}");
                    }

                    string R4_name = mtb.Name.Substring(mtb.Name.Length - 4, 4);
                    if (R4_name == "Date" || R4_name == "ate3" || R4_name == "ate1" || R4_name == "ate2" || R4_name == "ate4")
                    {
                        if (Sn_Number_(Sn, mtb, "Date") == true)
                            SendKeys.Send("{TAB}");
                    }

                   

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
                    string[] date_a = mtb.Text.Split('-');

                    if (date_a.Length >= 3 && date_a[0].Trim() != "" && date_a[1].Trim() != "" && date_a[2].Trim() != "")
                    {
                        string Date_YYYY = "0000" + int.Parse(date_a[0]).ToString();

                        date_a[0] = Date_YYYY.Substring(Date_YYYY.Length - 4, 4);

                        if (int.Parse(date_a[1]) < 10)
                            date_a[1] = "0" + int.Parse(date_a[1]).ToString();

                        if (int.Parse(date_a[2]) < 10)
                            date_a[2] = "0" + int.Parse(date_a[2]).ToString();

                        mtb.Text = date_a[0] + '-' + date_a[1] + '-' + date_a[2];

                        cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                        if (mtb.Text.Replace("-","").Trim() != "")
                        {
                            int Ret = 0;
                            Ret = c_er.Input_Date_Err_Check(mtb);

                            if (Ret == -1)
                            {
                                mtb.Focus(); return false;
                            }
                        }

                    }
                    else
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

            if ((sender is TextBox) == false) return;

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

            //if (tb.Name == "txtBank")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtBank_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtBank_Code);
            //}

            //if (tb.Name == "txtR_Id")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code);
            //}            
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

            //if (tb.Name == "txtChange")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtChange_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtChange_Code);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            //if (tb.Name == "txtBank")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtBank_Code, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtBank_Code);

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
            cgb_Pop.Change_Header_Text_TF = true;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtChange")
                    cgb_Pop.db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, strSql);
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y'";
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " And  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
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
                    if (cls_User.gid_CountryCode != "") Tsql = Tsql + " Where  Na_Code = '" + cls_User.gid_CountryCode + "'"; 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);
                }

                if (tb.Name == "txtChange")
                {
                    string Tsql;
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex ;

                    cgb_Pop.db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);
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
                Tsql = Tsql + " Where (Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And   Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
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

            if (tb.Name == "txtChange")
            {                
                Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                Tsql = Tsql + " Where " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;                
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
                ct.from_control_clear(this, mtxtCDate1);

                Save_Nom_Line_Chart();
                opt_Leave_1.Checked = true;
                combo_Sort.SelectedIndex = -1;
                
            }
            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
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
                    bt.Text =".";
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
            Excel_Export_File_Name = this.Text; // "Member_Select_Change";
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
            ct.Search_Date_TextBox_Put(mtxtCDate1, mtxtCDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }





        private void Push_data(Series series, string p, double p_3)
        {                       
            DataPoint dp = new DataPoint();

            if (p.Replace(" ", "").Length >= 4)
                dp.SetValueXY(p.Replace(" ", "").Substring(0, 4), p_3);
            else
                dp.SetValueXY(p, p_3);

            dp.Font = new System.Drawing.Font("맑은고딕", 9);
            dp.Label = string.Format(cls_app_static_var.str_Currency_Type, p_3); // p_3.ToString();                  
            series.Points.Add(dp);        

        }



        //Push_data(series_Item, nodeKey.ToString() + "Line", Save_Cnt[nodeKey]);
        private void Save_Nom_Line_Chart()
        {
            cls_form_Meth cm = new cls_form_Meth();


            //---------------------------------------------------
            chart_Center.Series.Clear();
            series_Center.Points.Clear();

            series_Center.Points.Clear();
            series_Center["DrawingStyle"] = "Emboss";
            series_Center["PointWidth"] = "0.5";
            series_Center.Name = cm._chang_base_caption_search("인원수");
            series_Center.ChartType = SeriesChartType.Column;

            //series_Center.Legend = "Legend1";
            chart_Center.Series.Add(series_Center);
            //---------------------------------------------------

            
            

            //---------------------------------------------------
            chart_Center.ChartAreas[0].AxisX.Interval = 1;
            chart_Center.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Center.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 7;
            chart_Center.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //---------------------------------------------------




        }

        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

            if (chk_Total.Checked == true)
            {
                for (int i = 0; i < dGridView_Base.RowCount; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }
            }
            else
            {
                for (int i = 0; i < dGridView_Base.RowCount; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }
            }

            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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

        private void butt_Save_Click(object sender, EventArgs e)
        {
            Boolean chage_Leave_tf = false;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            progress.Visible = true; progress.Value = 0;
            chage_Leave_tf = Chang_CenterCode();  //실질적인 센터 변경이 이루어지는 메소드
            progress.Visible = false;
            this.Cursor = System.Windows.Forms.Cursors.Default;

            if (chage_Leave_tf == true)
            {
                chk_Total.Checked = false;
                mtxtLeaveDate.Text = "";
                Base_Button_Click(butt_Select, e);
                ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                //cgb.d_Grid_view_Header_Reset();
                ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                
            }
        }


        private Boolean Chang_CenterCode()
        {
            //Msg_Useing_Not_Data
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;
            if (Check_TextBox_Save_Error() == false) return false;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();
            string StrSql = "";

            try
            {
                string T_Mbid = ""; string AfterDetail = ""; string BeforeDetail = "";
                string Mbid = ""; int Mbid2 = 0;
                cls_Search_DB csd = new cls_Search_DB();
                progress.Maximum = dGridView_Base.Rows.Count + 1;

                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        T_Mbid = dGridView_Base.Rows[i].Cells[1].Value.ToString();
                        Mbid = ""; Mbid2 = 0;
                        csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2); //회원번호를 분할 받아온다.

                        StrSql = "Update tbl_Memberinfo Set ";
                        StrSql = StrSql + " LeaveDate = '" + mtxtLeaveDate.Text.Replace ("-","").Trim() + "'";
                        StrSql = StrSql + " Where mbid = '" + Mbid + "'";
                        StrSql = StrSql + " And mbid2 = " + Mbid2.ToString();

                        Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name.ToString(), this.Text);


                        BeforeDetail = dGridView_Base.Rows[i].Cells[12].Value.ToString();  //전 센타코드
                        AfterDetail = mtxtLeaveDate.Text.Replace("-", "");   // 후센타코드

                        StrSql = "insert into tbl_Memberinfo_Mod ";
                        StrSql = StrSql + " (";
                        StrSql = StrSql + " mbid, mbid2 ";
                        StrSql = StrSql + ", ChangeDetail ";
                        StrSql = StrSql + ", BeforeDetail ";
                        StrSql = StrSql + ", AfterDetail ";
                        StrSql = StrSql + ", ModRecordid ";
                        StrSql = StrSql + ", ModRecordtime ";
                        StrSql = StrSql + " ) Values (";
                        StrSql = StrSql + "'" + Mbid + "'";
                        StrSql = StrSql + "," + Mbid2.ToString();
                        StrSql = StrSql + ", 'LeaveDate'";
                        StrSql = StrSql + ", '" + BeforeDetail + "'";
                        StrSql = StrSql + ", '" + AfterDetail + "'";
                        StrSql = StrSql + ",'" + cls_User.gid + "'";
                        StrSql = StrSql + ", Convert(Varchar(25),GetDate(),21) ";
                        StrSql = StrSql + ")";

                        Temp_Connect.Insert_Data(StrSql, "tbl_Memberinfo", Conn, tran, this.Name.ToString(), this.Text);
                    }

                    progress.Value = progress.Value + 1;
                }

                tran.Commit();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                return true;
            }


            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
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
            int chk_cnt = 0;

            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    chk_cnt++;
                }
            }//  end for 그리드 상에서 엑셀 전환을 선택한 V 한 내역을 파악한다.

            if (chk_cnt == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Select") + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Select"));
                return false;
            } //end if 체크를 해서 선택한 내역이 없을 경우 메시지 뛰우고나간다.


            if (mtxtLeaveDate.Text.Replace ("-","").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Requisite_Data")
                       + " - " + cls_app_static_var.app_msg_rm.GetString("Msg_Leave_Date")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtLeaveDate.Focus(); return false;
            }

            //cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            ////if (mtxtLeaveDate.Text.Trim() != "")
            ////{
            ////    int Ret = 0;
            ////    Ret = c_er.Input_Date_Err_Check(txtLeaveDate);

            ////    if (Ret == -1)
            ////    {
            ////        txtLeaveDate.Focus(); return false;
            ////    }
            ////}

            if (mtxtLeaveDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtLeaveDate.Text, mtxtLeaveDate, "Date") == false)
                {
                    mtxtLeaveDate.Focus();
                    return false;
                }

            }

           
            

            return true;
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

        private void butt_Check_01_Click(object sender, EventArgs e)
        {
            int R_Cnt = dGridView_Base.RowCount;
            for (int i = 0; i < R_Cnt; i++)
            {
                dGridView_Base.Rows[i].Cells[0].Value = "V";
            }
        }

        private void butt_Check_02_Click(object sender, EventArgs e)
        {
            int R_Cnt = dGridView_Base.RowCount;
            for (int i = 0; i < R_Cnt; i++)
            {
                dGridView_Base.Rows[i].Cells[0].Value = "";
            }
        }
    }
}
