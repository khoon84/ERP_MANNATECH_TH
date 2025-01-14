﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Collections;
using System.Xml ;

namespace MLM_Program
{
    public partial class frmMember_TreeView : Form
    {

       
        public delegate void Take_NumberDele(ref string Send_Number, ref string Send_Name);
        public event Take_NumberDele Take_Mem_Number;

         cls_Grid_Base cgb = new cls_Grid_Base();
         cls_Grid_Base cg_Up_S = new cls_Grid_Base();

        private const string base_db_name = "tbl_Memberinfo";
        private int Data_Set_Form_TF;

        Dictionary<string, TreeNode> dic_TreeEx = new Dictionary<string, TreeNode>();
        Dictionary<string, TreeNode> dic_TreeEx_2 = new Dictionary<string, TreeNode>();

        Series series_Save = new Series();
        Series series_Nom = new Series();


        private Dictionary<int, string> TreeDic_Cnt = new Dictionary<int, string>();
        private Dictionary<string, cls_Mem_TreeView> TreeDic = new Dictionary<string, cls_Mem_TreeView>();
        private Dictionary<int, cls_Tree_Line> LineDic = new Dictionary<int, cls_Tree_Line>();

        private Dictionary<int, string> TreeDic_Nom_Cnt = new Dictionary<int, string>();
        private Dictionary<string, cls_Mem_TreeView> TreeDic_Nom = new Dictionary<string, cls_Mem_TreeView>();
        private Dictionary<int, cls_Tree_Line> LineDic_Nom = new Dictionary<int, cls_Tree_Line>();
        private int IntervalHeight = 5;
        private int IntervalWidth = 15;
        private int LastLvl = 0;

        private int Print_W_Cur_PagCnt = 0;
        private int Print_H_Cur_PagCnt = 0;

        private int W_Print_PagCnt = 0;
        private int H_Print_PagCnt = 0;

        private int Print_W_NOM_Cur_PagCnt = 0;
        private int Print_H_NOM_Cur_PagCnt = 0;

        private int W_NOM_Print_PagCnt = 0;
        private int H_NOM_Print_PagCnt = 0;


        private int PB_Print_W_Cur_PagCnt = 0;
        private int PB_Print_H_Cur_PagCnt = 0;

        private int PB_W_Print_PagCnt = 0;
        private int PB_H_Print_PagCnt = 0;

        private delegate void TreeviewDel();



         public frmMember_TreeView()
        {
            InitializeComponent();
            BaseDoc.DefaultPageSettings.Landscape = true;
            prPrview.Document.DefaultPageSettings.Landscape = true;
        }





         private void frmBase_From_Load(object sender, EventArgs e)
         {
           
            Data_Set_Form_TF = 0;
             
             cls_form_Meth cm = new cls_form_Meth();
             cm.from_control_text_base_chang(this);

             cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
             cpbf.Put_SellCode_ComboBox(combo_Se, combo_Se_Code);
             
             mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;

             mtxtSellDate1.Mask = cls_app_static_var.Date_Number_Fromat;
             mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;

             txtDownCnt.BackColor = cls_app_static_var.txt_Enable_Color;

             if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
             {                 
                 tb_Sort_TF.Visible = false;                 
                 opt_C_3.Checked = true;
                 opt_C_2.Checked = false;
             }

             if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
             {              
                 tb_Sort_TF.Visible = false;
                 opt_C_2.Checked = true;
                 opt_C_3.Checked = false;
             }


         }


         private void frm_Base_Activated(object sender, EventArgs e)
         {
            //19-03-11 깜빡임제거 this.Refresh();

             string Send_Number = ""; string Send_Name = "";
             Take_Mem_Number(ref Send_Number, ref Send_Name);

             if (Send_Number != "")
             {
                 mtxtMbid.Text = Send_Number;
                 txtName.Text = Send_Name;
                 EventArgs ee = null;
                 Base_Button_Click(butt_Select, ee);
             }
         }


         private void frmBase_Resize(object sender, EventArgs e)
         {
             //butt_Exit.Left = this.Width - butt_Exit.Width - 20;

             //butt_Clear.Left = 3;
             //butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
             //butt_Save_Image.Left = butt_Select.Left + butt_Select.Width + 2;
             //butt_Nom_Image.Left = butt_Save_Image.Left + butt_Save_Image.Width + 2;
             ////this.Refresh();
             
             //int base_w = this.Width / 6;
             //butt_Clear.Width = base_w;
             //butt_Select.Width = base_w;
             //butt_Exp.Width = base_w;
             //butt_Save_Print.Width = base_w;
             //butt_Save_Image.Width = base_w;
             //butt_Exit.Width = base_w;

             //butt_Clear.Left = 0;
             //butt_Select.Left = butt_Clear.Left + butt_Clear.Width;
             //butt_Exp.Left = butt_Select.Left + butt_Select.Width;
             //butt_Save_Print.Left = butt_Exp.Left + butt_Exp.Width;
             //butt_Save_Image.Left = butt_Save_Print.Left + butt_Save_Print.Width;
             //butt_Exit.Left = butt_Save_Image.Left + butt_Save_Image.Width;


             butt_Clear.Left = 0;
             butt_Select.Left = butt_Clear.Left + butt_Clear.Width + 2;
             butt_Exp.Left = butt_Select.Left + butt_Select.Width + 2;
             butt_Save_Print.Left = butt_Exp.Left + butt_Exp.Width + 2;
             butt_Save_Image.Left = butt_Save_Print.Left + butt_Save_Print.Width + 2;
             butt_Exit.Left = this.Width - butt_Exit.Width - 17;


             cls_form_Meth cfm = new cls_form_Meth();
             cfm.button_flat_change(butt_Clear);
             cfm.button_flat_change(butt_Select);
             cfm.button_flat_change(butt_Exp);
             cfm.button_flat_change(butt_Save_Print);
             cfm.button_flat_change(butt_Save_Image);
             cfm.button_flat_change(butt_Exit);  
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

                             //  cls_form_Meth cfm = new cls_form_Meth();
                             // cfm.form_Group_Panel_Enable_True(this);
                         }
                     }
                 }// end if

             }

             //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
             if (sender is DataGridView)
             {
                 if (e.KeyValue == 46)
                 {
                     e.Handled = true;
                 } // end if

                 if (e.KeyValue == 13)
                 {
                     EventArgs ee = null;
                     dGridView_Base_DoubleClick(sender, ee);
                     e.Handled = true;
                 } // end if
             }
             
             Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 113)
                T_bt = butt_Select;     //조회  F1
            //if (e.KeyValue == 115)
                //T_bt = butt_Delete;   // 삭제  F4
            //if (e.KeyValue == 119)
                //T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //엑셀  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);
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
                         if (mtb.Text.Replace("-", "").Trim() != "")
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
                if (mtxtMbid.Text.Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        txtName.Text = Search_Name;
                    }
                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2);

                        //cls_app_static_var.Search_Member_Number_Mbid = Mbid;
                        //cls_app_static_var.Search_Member_Number_Mbid2 = Mbid2;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        e_f.ShowDialog();

                        //txtMemberName.Text = cls_app_static_var.Search_Member_Name_Return;
                        //mtxtMbid.Text = cls_app_static_var.Search_Member_Number_Return;     
                    }
                }

                SendKeys.Send("{TAB}");
            }
        }

        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number;
            txtName.Text = Send_Name;
        }

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (mtxtMbid.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                txtName.Text = "";
            }
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



        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
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
            else if (tb.Tag.ToString() == "1")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if (tb.Tag.ToString() == "name")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }
            else if (tb.Tag.ToString() == "ncode") //코드관련해서 코드를치면 관련 내역이 나오도록 하기 위함.
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

            TextBox tb = (TextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }


        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }

        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName")
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {
                    //cls_app_static_var.Search_Member_Name = txt_tag;
                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                    e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    e_f.ShowDialog();                 
                }
                SendKeys.Send("{TAB}");
            }

        }


        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }
        

        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtSellCode_Code);
                ////if (tb.Text.ToString() == "")
                ////    Db_Grid_Popup(tb, txtSellCode_Code, "");
                ////else
                ////    Ncod_Text_Set_Data(tb, txtSellCode_Code);

                ////SendKeys.Send("{TAB}");
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


            cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, cls_User.gid_CountryCode);
        }


        private void Db_Grid_Popup(TextBox tb, TextBox tb1_Code, string strSql)
        {
            //cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            //DataGridView Popup_gr = new DataGridView();
            //Popup_gr.Name = "Popup_gr";
            //this.Controls.Add(Popup_gr);
            //cgb_Pop.basegrid = Popup_gr;
            //cgb_Pop.Base_fr = this;
            //cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            //cgb_Pop.Base_tb_2 = tb ;    //2번은 명임

            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;


            if (strSql != "")
            {
                if (tb.Name == "txtCenter")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txtBank")
                    cgb_Pop.db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", strSql);

                if (tb.Name == "txtSellCode")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", strSql);
                    cgb_Pop.Next_Focus_Control = txtSellCode;
                }
            }
            else
            {
                if (tb.Name == "txtCenter")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  U_TF = 0 "; //사용센타만 보이게 한다 
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

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                    cgb_Pop.Next_Focus_Control = txtSellCode;
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

            if (tb.Name == "txtSellCode")
            {
                Tsql = "Select SellCode ,SellTypeName    ";
                Tsql = Tsql + " From tbl_SellType (nolock) ";
                Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
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
                trv_Member.Nodes.Clear();
                trv_Member_Nom.Nodes.Clear();
                dic_TreeEx.Clear();
                dic_TreeEx_2.Clear();

                cls_form_Meth ct = new cls_form_Meth();
                ct.from_control_clear(panel1, mtxtMbid);
                                
                butt_Exp.Text = ct._chang_base_caption_search("확장");
                butt_Exp_2.Text = "...";

                cls_Grid_Base_info_Put cgbp10 = new cls_Grid_Base_info_Put();
                cgbp10.dGridView_Put_baseinfo(dGridView_Up_S, "saveup");

                cls_Grid_Base_info_Put cgbp11 = new cls_Grid_Base_info_Put();
                cgbp11.dGridView_Put_baseinfo(dGridView_Up_N, "nominup");

                tabC_Up.SelectedIndex = 0;

                //chart_Save.Series.Clear();
                //chart_Nom.Series.Clear();

                if (cls_app_static_var.save_uging_Pr_Flag == 0) //후원인 기능 사용하지 마라.
                {
                    tb_Sort_TF.Visible = false;
                    opt_C_3.Checked = true;
                    opt_C_2.Checked = false;
                }

                if (cls_app_static_var.nom_uging_Pr_Flag == 0)  //추천인 기능 사용하지 마라
                {
                    tb_Sort_TF.Visible = false;
                    opt_C_2.Checked = true;
                    opt_C_3.Checked = false;
                }

                opt_C_2.Checked = true;
                mtxtMbid.Focus();            
            }


            if (bt.Name == "butt_Exp")
            {
                cls_form_Meth cfm = new cls_form_Meth();
                trv_Member.Visible = false;

                if (bt.Text == cfm._chang_base_caption_search("확장"))
                {
                    if (opt_C_2.Checked == true )
                    {
                        foreach (string nodeKey in dic_TreeEx.Keys)
                        {
                            TreeNode tn = dic_TreeEx[nodeKey];
                            tn.Expand();
                        }
                    }
                    else
                    {
                        foreach (string nodeKey in dic_TreeEx_2.Keys)
                        {
                            TreeNode tn = dic_TreeEx_2[nodeKey];
                            tn.Expand();
                        }
                    }

                    

                    bt.Text = cfm._chang_base_caption_search("축소");
                }
                else
                {
                    if (opt_C_2.Checked == true)
                    {
                        foreach (string nodeKey in dic_TreeEx.Keys)
                        {
                            TreeNode tn = dic_TreeEx[nodeKey];
                            tn.Collapse();
                        }
                    }

                    else
                    {
                        foreach (string nodeKey in dic_TreeEx_2.Keys)
                        {
                            TreeNode tn = dic_TreeEx_2[nodeKey];
                            tn.Collapse();
                        }
                    }

                    bt.Text = cfm._chang_base_caption_search("확장");
                }
                trv_Member.Visible = true;
            }

                  
            else if (bt.Name == "butt_Select")
            {
                if (Check_TextBox_Error() == false) return;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                prB_Save.Visible = true; prB_Nom.Visible = true;

                cls_form_Meth cfm = new cls_form_Meth();
                butt_Exp.Text = cfm._chang_base_caption_search("확장");

                if (TreeDic != null)
                    TreeDic.Clear();
                if (LineDic != null)
                    LineDic.Clear();
                if (TreeDic_Cnt != null)
                    TreeDic_Cnt.Clear();

                //if (TreeDic_Nom != null)
                //    TreeDic_Nom.Clear();
                //if (LineDic_Nom != null)
                //    LineDic_Nom.Clear();
                //if (TreeDic_Nom_Cnt != null)
                //    TreeDic_Nom_Cnt.Clear();

                combo_Se_Code.SelectedIndex = combo_Se.SelectedIndex;
                trv_Member.Visible = false; trv_Member_Nom.Visible = false;

                if (opt_C_2.Checked == true)
                    Make_TreeView_Save();  //뿌려주는 곳                
                else
                    Make_TreeView_Nom();  //뿌려주는 곳

                //Save_Nom_Line_Down_Cnt(); //차트관련

                cls_Grid_Base_info_Put cgbp = new cls_Grid_Base_info_Put();
                cgbp.dGridView_Put_baseinfo(this, dGridView_Up_S, "saveup", mtxtMbid.Text.Trim());
          
                cls_Grid_Base_info_Put cgbp2 = new cls_Grid_Base_info_Put();
                cgbp2.dGridView_Put_baseinfo(this, dGridView_Up_N, "nominup", mtxtMbid.Text.Trim());


                //Set_Form_Date_Up("S");

                trv_Member.Visible = true; trv_Member_Nom.Visible = true;
                prB_Save.Visible = false; prB_Nom.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }

       

            else if (bt.Name == "butt_Up")
            {

                cls_Check_Input_Error c_er = new cls_Check_Input_Error();
                if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int Ret = 0;
                    Ret = c_er._Member_Nmumber_Split(mtxtMbid);
                    if (Ret == -1)
                    {
                        mtxtMbid.Focus(); return ;
                    } 

                    Db_Grid_Popup(txtName ,mtxtMbid , butt_Up, "");            
                }   
            }


            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
            

        }



       


        private void tree_View_CallBack(IAsyncResult ar)
        {
            TreeviewDel trd = (TreeviewDel)ar.AsyncState;
        }


        private void Db_Grid_Popup(TextBox tb, MaskedTextBox tb1_Code, Button butt_Loc, string strSql)
        {
            cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            DataGridView Popup_gr = new DataGridView();
            Popup_gr.Name = "Popup_gr";
            this.Controls.Add(Popup_gr);
            cgb_Pop.basegrid = Popup_gr;
            cgb_Pop.Base_fr = this;
            cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = butt_Loc;

            cgb_Pop.basegrid.DoubleClick += new EventHandler(Popup_basegrid_DoubleClick);
            cgb_Pop.basegrid.KeyDown += new KeyEventHandler(basegrid_KeyDown);
            
            string Mbid = "" ;  int Mbid2 = 0 ;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) == 1)
            {           
                string Tsql;
                Tsql = "Select ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " mbid + '-' + Convert(Varchar,mbid2) AS M_Number ";
                else
                    Tsql = Tsql + " mbid2  AS M_Number";

                Tsql = Tsql + " , M_Name  ";
                Tsql = Tsql + " From ufn_SaveUp_Member_Search_mannatech ( ";
                Tsql = Tsql + " '" + Mbid + "'" ;
                Tsql = Tsql + " ," + Mbid2 +  ")" ;
                Tsql = Tsql + " Where lvl > 0 ";
                Tsql = Tsql + " Order By lvl ";

                cgb_Pop.db_grid_Popup_Base(2, "회원_번호", "성명", "M_Number", "M_Name", Tsql, 0);           
            }
        }

        void basegrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Popup_basegrid_DoubleClick(sender, e);
            }  
        }

        void Popup_basegrid_DoubleClick(object sender, EventArgs e)
        {
            trv_Member.Nodes.Clear();            trv_Member_Nom.Nodes.Clear();
            dic_TreeEx.Clear();            dic_TreeEx_2.Clear();
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);
            butt_Exp.Text = "...";            butt_Exp_2.Text = "...";
            chart_Save.Series.Clear();            chart_Nom.Series.Clear();

            

            
            DataGridView T_Gd = (DataGridView)sender;                        

            cls_form_Meth cfm = new cls_form_Meth();
            cfm.form_Group_Panel_Enable_True(this);

            T_Gd.Visible = false;
            if (T_Gd.CurrentRow.Cells[0].Value != null)
            {
                mtxtMbid.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();
                mtxtMbid.Focus();
           }

            
            T_Gd.Dispose();
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
                    mtxtMbid.Focus(); return false;
                }
            }

            if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                cls_Search_DB csb = new cls_Search_DB();
                string Search_Name = csb.Member_Name_Search(mtxtMbid.Text);


                if (Search_Name == "-1") //회원번호가 올바르게 입력 되어 있는지
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }


                else if (Search_Name != "")  //이름이 튀어나오면 회원이 존재하고 올바르게 입력된거임
                    txtName.Text = Search_Name;

                else if (Search_Name == "")  //이름이 안튀어 나오면 회원이 존재하지 않는 거임.
                {
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                           + "\n" +
                           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    mtxtMbid.Focus();
                    return false;
                }
                else //이도 저도 아닌 -2 같은 에러가 나온다. 그럼 다 리셋 시켜 버린다.
                {
                    mtxtMbid.Text = ""; txtName.Text = "";
                }

            }//센타장으로 해서 회원번호를 입력한 경우
            else
                txtName.Text = "";   //회원번호 입력 안되어있는 데 회원명 입력 될수 있기 때문에 그런 경우를 대비해서  회원명을 빈칸으로 함.


            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                          + "\n" +
                          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus();
                return false;
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


            return true;
        }



        private void Make_TreeView_Nom()
        {
            string Mbid = ""; int Mbid2 = 0 ;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) <= 0 ) return ;
            
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();                                 
            string Sql = "";

            Sql = "select ufn_GetSubTreeView_Mem_Nomin_mannatech.Mbid,ufn_GetSubTreeView_Mem_Nomin_mannatech.Mbid2, ";
            Sql = Sql + " ufn_GetSubTreeView_Mem_Nomin_mannatech.M_Name, lvl, ";
            Sql = Sql + " ufn_GetSubTreeView_Mem_Nomin_mannatech.idx_Nomin,ufn_GetSubTreeView_Mem_Nomin_mannatech.idx_Nomin2,";
            Sql = Sql + " Level_Cnt,ufn_GetSubTreeView_Mem_Nomin_mannatech.Nomin_Cur,MaxLevel , ";
            Sql = Sql + " idx_Save,idx_Save2, ";
            Sql = Sql + " B_Name,C_Name,SumPV ";

            Sql = Sql + " From ufn_GetSubTreeView_Mem_Nomin_mannatech('" + Mbid + "'," + Mbid2 + ")";

            Sql = Sql + " Where ufn_GetSubTreeView_Mem_Nomin_mannatech.Nomin_Cur <>0 ";
            Sql = Sql + " And   ufn_GetSubTreeView_Mem_Nomin_mannatech.Nomin_Cur <= 100 ";
            Sql = Sql + " And   Lvl >= 0 ";
            Sql = Sql + " Order by lvl asc, ufn_GetSubTreeView_Mem_Nomin_mannatech.Nomin_Cur asc";


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Sql, "Tree_Nomin", ds, this.Name, this.Text) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;


            string TextMbid = ""; string TMbid = ""; int TMbid2 = 0;
            string TName = ""; string TreeText = ""; string Saveid = "";
            string T_N_Mbid = ""; int T_N_Mbid2 = 0; int TCur = 0;
            int sw_for = 0; int Lvl = 0; 
            trv_Member.Nodes.Clear();
            dic_TreeEx_2.Clear();
            txtDownCnt.Text = (Temp_Connect.DataSet_ReCount - 1).ToString();
            
            prB_Nom.Minimum = 0;       prB_Nom.Maximum =  Temp_Connect.DataSet_ReCount;
            prB_Nom.Step = 1;          prB_Nom.Value = 0;

            Dictionary<string, cls_Mem_TreeView> T_TreeDic = new Dictionary<string, cls_Mem_TreeView>();

            for (int RowCnt = 0; RowCnt < Temp_Connect.DataSet_ReCount; RowCnt++)
            {
                cls_Mem_TreeView t_Mem_Treel = new cls_Mem_TreeView();

                TMbid = ds.Tables["Tree_Nomin"].Rows[RowCnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["Mbid2"].ToString());
                TName = ds.Tables["Tree_Nomin"].Rows[RowCnt]["M_Name"].ToString();

                T_N_Mbid = ds.Tables["Tree_Nomin"].Rows[RowCnt]["idx_Nomin"].ToString();
                T_N_Mbid2 = int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["idx_Nomin2"].ToString());
                TCur = int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["Nomin_Cur"].ToString());

                

                //Make_Tree_Nom_Text(ds, RowCnt, ref TreeText, TextMbid, TName);     //트리를 구성하는 텍스트를 만들어서 나온다.       

                if (cls_app_static_var.Member_Number_1 > 0)
                {
                    TextMbid = TMbid + "-" + TMbid2.ToString();
                    Saveid = T_N_Mbid + "-" + T_N_Mbid2.ToString();                 
                }
                else
                {
                    TextMbid =  TMbid2.ToString();
                    Saveid =  T_N_Mbid2.ToString();                    
                }

                TreeText = TextMbid + " " + TName + "(" + TCur + ")";
                //if (cls_app_static_var.Member_Number_1 > 0)
                //{
                //    TextMbid = TMbid + "-" + TMbid2.ToString();
                //    Saveid = T_N_Mbid + "-" + T_N_Mbid2.ToString();
                //}
                //else
                //{
                //    TextMbid = TMbid2.ToString();
                //    Saveid = T_N_Mbid2.ToString();
                //}

                t_Mem_Treel.IDKey = TextMbid;
                t_Mem_Treel.KeyName = TreeText;
                if (RowCnt > 0)
                    if (cls_app_static_var.Member_Number_1 > 0)    
                        t_Mem_Treel.ParentKey = T_N_Mbid + "-" + T_N_Mbid2.ToString();
                    else
                        t_Mem_Treel.ParentKey =  T_N_Mbid2.ToString();
                else
                    t_Mem_Treel.ParentKey = "";

                Lvl = int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["lvl"].ToString());
                t_Mem_Treel.Lvl = Lvl;
                t_Mem_Treel.ChildNumber = new Dictionary<int, cls_Mem_TreeView>();

                T_TreeDic[t_Mem_Treel.IDKey] = t_Mem_Treel;

                if (RowCnt > 0)
                    InputNextKey(ref T_TreeDic, t_Mem_Treel.ParentKey, RowCnt, t_Mem_Treel.IDKey);

                TreeDic_Cnt[RowCnt] = t_Mem_Treel.IDKey;



                if (dic_TreeEx_2 != null)
                {
                    
                    if (dic_TreeEx_2.ContainsKey(Saveid) == true)
                    {
                        sw_for = 1;
                        TreeNode tn2 = dic_TreeEx_2[Saveid];

                        if (tn2 != null)
                        {
                            TreeNode node2 = new TreeNode(TreeText);
                            tn2.Nodes.Add(node2);
                            dic_TreeEx_2[TextMbid] = node2;
                            if (int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["Lvl"].ToString()) <= 2)
                                tn2.Expand();
                        }
                    }
                    else
                        sw_for = 0;
                }
                

                if (sw_for == 0)
                {
                    TreeNode tn = trv_Member.Nodes.Add(TreeText);
                    dic_TreeEx_2[TextMbid] = tn;
                    if (int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["Lvl"].ToString()) <= 2)
                        tn.Expand();
                }

                prB_Nom.PerformStep();
            }


            TreeDic = T_TreeDic;


        }


        private void InputNextKey(ref Dictionary<string, cls_Mem_TreeView> T_TreeDic, string ParentKey, int fi_cnt, string IDKey)
        {
            T_TreeDic[ParentKey].ChildCount++;
            T_TreeDic[ParentKey].NextDataNum = fi_cnt;

            T_TreeDic[ParentKey].ChildNumber[T_TreeDic[ParentKey].ChildCount] = T_TreeDic[IDKey];
            T_TreeDic[IDKey].ParentClass = T_TreeDic[ParentKey];
        }



        private void Make_TreeView_Save()
        {
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) <= 0) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Sql = "";
            string SDate1 = mtxtSellDate1.Text.Replace("-", "").Trim();
            string SDate2 = mtxtSellDate2.Text.Replace("-", "").Trim() ;

            if (SDate1 == "" ) SDate1 = "19000101" ;
            if (SDate2 == "" ) SDate2 = "21001231";

            Sql = "select ufn_GetSubTreeView_Sell_Mem_mannatech.Mbid ";
            Sql = Sql + " , ufn_GetSubTreeView_Sell_Mem_mannatech.Mbid2 ";
            Sql = Sql + " , ufn_GetSubTreeView_Sell_Mem_mannatech.M_Name " ;
            Sql = Sql + " , lvl ";
            Sql = Sql + " , ufn_GetSubTreeView_Sell_Mem_mannatech.idx_Nomin " ;
            Sql = Sql + " , ufn_GetSubTreeView_Sell_Mem_mannatech.idx_Nomin2";
            Sql = Sql + " , Level_Cnt";
            Sql = Sql + " , ufn_GetSubTreeView_Sell_Mem_mannatech.Save_Cur";
            Sql = Sql + " , MaxLevel  ";
            Sql = Sql + " , idx_Save ";
            Sql = Sql + " , idx_Save2 ";
            Sql = Sql + " , B_Name ";    //후원인명
            Sql = Sql + " , C_Name ";    //추천인명
            Sql = Sql + " , SumPV ";
            Sql = Sql + " , RegDate ";
            Sql = Sql + " , cpno ";
            Sql = Sql + " , LeaveDate ";
            Sql = Sql + " , BusName "; //센타명
            Sql = Sql + " , G_Name ";   //직급명
            Sql = Sql + " , DownPV ";   //하선매출PV
            Sql = Sql + " , Save_Cur ";   //라인을 찾기위함.
            Sql = Sql + " , LineCnt ";   //라인을 찾기위함.



            Sql = Sql + " From ufn_GetSubTreeView_Sell_Mem_mannatech('" + Mbid + "'," + Mbid2 + ",'" + SDate1 + "','" + SDate2 + "','" + combo_Se_Code.Text.Trim() + "')";

            Sql = Sql + " Where ufn_GetSubTreeView_Sell_Mem_mannatech.LineCnt <>0 ";
            Sql = Sql + " And   ufn_GetSubTreeView_Sell_Mem_mannatech.LineCnt <= 100 ";
            Sql = Sql + " And   Lvl >= 0 ";
            Sql = Sql + " Order by lvl asc, ufn_GetSubTreeView_Sell_Mem_mannatech.Save_Cur asc";


            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Sql, "Tree_Save", ds, this.Name , this.Text) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;


            string TextMbid = ""; string TMbid = ""; int TMbid2 = 0;
            string TName = ""; string TreeText = ""; string Saveid = "";
            string T_N_Mbid = ""; int T_N_Mbid2 = 0; int TCur = 0;
            int sw_for = 0; int Lvl = 0;
            trv_Member.Nodes.Clear();
            dic_TreeEx.Clear();
            txtDownCnt.Text = (Temp_Connect.DataSet_ReCount - 1).ToString () ;

            prB_Save.Minimum = 0;            prB_Save.Maximum = Temp_Connect.DataSet_ReCount;
            prB_Save.Step = 1;               prB_Save.Value = 0;

            Dictionary<string, cls_Mem_TreeView> T_TreeDic = new Dictionary<string, cls_Mem_TreeView>();
            Dictionary<int, int> Save_Cnt = new Dictionary<int, int>();            
                        

            for (int RowCnt = 0; RowCnt < Temp_Connect.DataSet_ReCount; RowCnt++)
            {
                cls_Mem_TreeView t_Mem_Treel = new cls_Mem_TreeView();

                TMbid = ds.Tables["Tree_Save"].Rows[RowCnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["Mbid2"].ToString());
                TName = ds.Tables["Tree_Save"].Rows[RowCnt]["M_Name"].ToString();

                T_N_Mbid = ds.Tables["Tree_Save"].Rows[RowCnt]["idx_Save"].ToString();
                T_N_Mbid2 = int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["idx_Save2"].ToString());
                TCur = int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["LineCnt"].ToString());

                if (cls_app_static_var.Member_Number_1 > 0)
                {
                    TextMbid = TMbid + "-" + TMbid2.ToString();
                    Saveid = T_N_Mbid + "-" + T_N_Mbid2.ToString();
                }
                else
                {
                    TextMbid = TMbid2.ToString();
                    Saveid =  T_N_Mbid2.ToString();
                }
                
                //TreeText = TextMbid + " " + TName + "(" + TCur + ")";

                Make_Tree_Text(ds, RowCnt, ref TreeText, TextMbid, TName);     //트리를 구성하는 텍스트를 만들어서 나온다.                           
                

                t_Mem_Treel.IDKey = TextMbid;
                t_Mem_Treel.KeyName = TreeText ;
                if (RowCnt > 0)
                    if (cls_app_static_var.Member_Number_1 > 0)
                        t_Mem_Treel.ParentKey = T_N_Mbid + "-" + T_N_Mbid2.ToString();
                    else
                        t_Mem_Treel.ParentKey = T_N_Mbid2.ToString();
                else
                    t_Mem_Treel.ParentKey = "";

                Lvl = int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["lvl"].ToString());
                t_Mem_Treel.Lvl = Lvl;
                t_Mem_Treel.ChildNumber = new Dictionary<int, cls_Mem_TreeView>();

                T_TreeDic[t_Mem_Treel.IDKey] = t_Mem_Treel;

                if (RowCnt > 0)
                    InputNextKey(ref T_TreeDic, t_Mem_Treel.ParentKey, RowCnt, t_Mem_Treel.IDKey, Lvl);
                
                TreeDic_Cnt[RowCnt] = t_Mem_Treel.IDKey;

                if (dic_TreeEx != null)
                {                    
                    if (dic_TreeEx.ContainsKey(Saveid) == true)
                    {
                        sw_for = 1;
                        TreeNode tn2 = dic_TreeEx[Saveid];

                        if (tn2 != null)
                        {
                            TreeNode node2 = new TreeNode(TreeText);
                            tn2.Nodes.Add(node2);
                            dic_TreeEx[TextMbid] = node2;

                            if (int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["Lvl"].ToString() ) <= 2)
                                tn2.Expand();
                        }
                    }
                    else
                        sw_for = 0;
                }


                if (sw_for == 0)
                {
                    TreeNode tn = trv_Member.Nodes.Add(TreeText);
                    dic_TreeEx[TextMbid] = tn;
                    if (int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["Lvl"].ToString()) <= 2)
                        tn.Expand();                    
                }

                if (t_Mem_Treel.Lvl > 0)
                {
                    string Save_Cur =  ds.Tables["Tree_Save"].Rows[RowCnt]["Save_Cur"].ToString() ;
                    int TCur_2 = int.Parse (Save_Cur.Substring(1, 2));
                    if (Save_Cnt.ContainsKey(TCur_2) == false)
                        Save_Cnt[TCur_2] = 1;
                    else
                        Save_Cnt[TCur_2]++;
                }

                prB_Save.PerformStep();
            } //end for

            TreeDic = T_TreeDic;

            //Save_Nom_Line_Chart(Save_Cnt);

        }

        private void InputNextKey(ref Dictionary<string, cls_Mem_TreeView> T_TreeDic, string ParentKey, int fi_cnt, string IDKey, int Lvl)
        {
            T_TreeDic[ParentKey].ChildCount++;
            T_TreeDic[ParentKey].NextDataNum = fi_cnt;

            T_TreeDic[ParentKey].ChildNumber[T_TreeDic[ParentKey].ChildCount] = T_TreeDic[IDKey];            
            T_TreeDic[IDKey].ParentClass = T_TreeDic[ParentKey];            
        }



        private void Make_Tree_Text( DataSet ds, int RowCnt, ref string  TreeText, string  TextMbid  , string  TName)
        {
            string T_N_Mbid = ""; int T_N_Mbid2 = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (chb_1.Checked == true) TreeText = TextMbid;
            if (chb_2.Checked == true) TreeText = TreeText + " ▶" + TName;
            if (chb_3.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("가입일") + ":" + ds.Tables["Tree_Save"].Rows[RowCnt]["RegDate"].ToString();
            if (chb_4.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("센타") + ":" + ds.Tables["Tree_Save"].Rows[RowCnt]["BusName"].ToString();

            string t_cpno = ds.Tables["Tree_Save"].Rows[RowCnt]["cpno"].ToString();
            if (chb_5.Checked == true && t_cpno != "")
            {
                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("주민번호") + ":" + t_cpno.Substring(0, 6) + "-" + t_cpno.Substring(6, 7);
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("주민번호") + ":" + t_cpno.Substring(0, 6) + "-*******";
            }
            if (chb_6.Checked == true)
            {
                if (ds.Tables["Tree_Save"].Rows[RowCnt]["LeaveDate"].ToString() == "")
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("활동");
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("탈퇴");
            }
            
            if (chb_7.Checked == true)
            {
                T_N_Mbid = ds.Tables["Tree_Save"].Rows[RowCnt]["idx_Nomin"].ToString();
                T_N_Mbid2 = int.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["idx_Nomin2"].ToString());
                if (cls_app_static_var.Member_Number_1 > 0)
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("추천인") + ":" + T_N_Mbid + "-" + T_N_Mbid2.ToString();
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("추천인") + ":" + T_N_Mbid2.ToString();
            }
            if (chb_8.Checked == true) TreeText = TreeText + " " + ds.Tables["Tree_Save"].Rows[RowCnt]["C_Name"].ToString();

            if (chb_9.Checked == true)
            {
                double T_pr = double.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["SumPV"].ToString());
                TreeText = TreeText + " ▶" + cm._chang_base_caption_search("총판매") + ":" + string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            }

            if (chb_10.Checked == true)
            {
                double T_pr = double.Parse(ds.Tables["Tree_Save"].Rows[RowCnt]["DownPV"].ToString());
                TreeText = TreeText + " ▶" + cm._chang_base_caption_search("총하선") + ":" + string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            }

            if (chb_11.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("직급") + ":" + ds.Tables["Tree_Save"].Rows[RowCnt]["G_Name"].ToString();

        }

        private void Make_Tree_Nom_Text(DataSet ds, int RowCnt, ref string TreeText, string TextMbid, string TName)
        {
            string T_N_Mbid = ""; int T_N_Mbid2 = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (chb_1.Checked == true) TreeText = TextMbid;
            if (chb_2.Checked == true) TreeText = TreeText + " ▶" + TName;
            if (chb_3.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("가입일") + ":" + ds.Tables["Tree_Nomin"].Rows[RowCnt]["RegDate"].ToString();
            if (chb_4.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("센타") + ":" + ds.Tables["Tree_Nomin"].Rows[RowCnt]["BusName"].ToString();

            string t_cpno = ds.Tables["Tree_Nomin"].Rows[RowCnt]["cpno"].ToString();
            if (chb_5.Checked == true && t_cpno != "")
            {
                if (cls_app_static_var.Member_Cpno_Visible_TF == 1)
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("주민번호") + ":" + t_cpno.Substring(0, 6) + "-" + t_cpno.Substring(6, 7);
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("주민번호") + ":" + t_cpno.Substring(0, 6) + "-*******";
            }
            if (chb_6.Checked == true)
            {
                if (ds.Tables["Tree_Nomin"].Rows[RowCnt]["LeaveDate"].ToString() == "")
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("활동");
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("탈퇴");
            }

            if (chb_7.Checked == true)
            {
                T_N_Mbid = ds.Tables["Tree_Nomin"].Rows[RowCnt]["idx_Nomin"].ToString();
                T_N_Mbid2 = int.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["idx_Nomin2"].ToString());
                if (cls_app_static_var.Member_Number_1 > 0)
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("추천인") + ":" + T_N_Mbid + "-" + T_N_Mbid2.ToString();
                else
                    TreeText = TreeText + " ▶" + cm._chang_base_caption_search("추천인") + ":" + T_N_Mbid2.ToString();
            }
            if (chb_8.Checked == true) TreeText = TreeText + " " + ds.Tables["Tree_Nomin"].Rows[RowCnt]["C_Name"].ToString();

            if (chb_9.Checked == true)
            {
                double T_pr = double.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["SumPV"].ToString());
                TreeText = TreeText + " ▶" + cm._chang_base_caption_search("총판매") + ":" + string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            }

            if (chb_10.Checked == true)
            {
                double T_pr = double.Parse(ds.Tables["Tree_Nomin"].Rows[RowCnt]["DownPV"].ToString());
                TreeText = TreeText + " ▶" + cm._chang_base_caption_search("총하선") + ":" + string.Format(cls_app_static_var.str_Currency_Type, T_pr);
            }

            if (chb_11.Checked == true) TreeText = TreeText + " ▶" + cm._chang_base_caption_search("직급") + ":" + ds.Tables["Tree_Nomin"].Rows[RowCnt]["G_Name"].ToString();

        }





        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // "Member_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Base;
        }



        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {            
            if ((sender as DataGridView).CurrentRow.Cells[0].Value != null)
            {
                string t_Mbid =  (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                

                EventArgs ee = null;
                //Base_Button_Click(butt_Clear, ee);

                mtxtMbid.Text = t_Mbid; 
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Name = "";
                reCnt = cds.Member_Name_Search(mtxtMbid.Text, ref Search_Name);

                if (reCnt == 1)
                {
                    txtName.Text = Search_Name;
                    //EventArgs ee = null;
                    Base_Button_Click(butt_Select, ee);
                }
            }
        }


        


        private void Save_Nom_Line_Down_Cnt()
        {
            Dictionary<int, int> Save_Cnt = new Dictionary<int, int>();
            Dictionary<int, int> Nom_Cnt = new Dictionary<int, int>();

            string TMbid = "";  int TMbid2 = 0;  string TName = "" ;  int TCur = 0 ;
            string TreeText ="";

            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(mtxtMbid.Text, ref Mbid, ref Mbid2) <= 0) return;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Sql = "";

            Sql = "select Mbid,Mbid2,LineCnt,M_Name From tbl_Memberinfo (nolock)";
            Sql = Sql + " Where Saveid='" + Mbid + "'" ;
            Sql = Sql + " And   Saveid2=" + Mbid2 ;
            Sql = Sql + " And LineCnt > 0 ";
            Sql = Sql + " Order by LineCnt ";
            
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Sql, "tbl_Memberinfo", ds) == false) return;
            if (Temp_Connect.DataSet_ReCount == 0) return;
            
            for (int RowCnt = 0 ; RowCnt < Temp_Connect.DataSet_ReCount ; RowCnt++)
            {
                TMbid = ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["Mbid"].ToString();
                TMbid2 = int.Parse(ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["Mbid2"].ToString());
                TName = ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["M_Name"].ToString();
                TCur = int.Parse(ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["LineCnt"].ToString());
            
                TreeText = TMbid + "-" + TMbid2.ToString() + " " + TName + "(" + TCur + ")";
                
                foreach (string nodeKey in dic_TreeEx.Keys)
                {
                    TreeNode tn = dic_TreeEx[nodeKey];
                    string str = tn.FullPath;
                    if (str.Contains(TreeText) == true )
                    {
                        if (Save_Cnt.ContainsKey(TCur) == false)
                            Save_Cnt[TCur] = 1;
                        else
                            Save_Cnt[TCur]++;
                    }
                }
             }

            //Sql = "select Mbid,Mbid2,N_LineCnt,M_Name From tbl_Memberinfo (nolock)";
            //Sql = Sql + " Where Nominid ='" + Mbid + "'";
            //Sql = Sql + " And   Nominid2 =" + Mbid2;
            //Sql = Sql + " And N_LineCnt > 0 ";
            //Sql = Sql + " Order by N_LineCnt ";

            //ds.Clear();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(Sql, "tbl_Memberinfo", ds) == false) return;
            //if (Temp_Connect.DataSet_ReCount == 0) return;

            //for (int RowCnt = 0; RowCnt < Temp_Connect.DataSet_ReCount; RowCnt++)
            //{
            //    TMbid = ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["Mbid"].ToString();
            //    TMbid2 = int.Parse(ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["Mbid2"].ToString());
            //    TName = ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["M_Name"].ToString();
            //    TCur = int.Parse(ds.Tables["tbl_Memberinfo"].Rows[RowCnt]["N_LineCnt"].ToString());

            //    TreeText = TMbid + "-" + TMbid2.ToString() + " " + TName + "(" + TCur + ")";

            //    foreach (string nodeKey in dic_TreeEx_2.Keys)
            //    {
            //        TreeNode tn = dic_TreeEx_2[nodeKey];
            //        string str = tn.FullPath;
            //        if (str.Contains(TreeText) == true)
            //        {
            //            if (Nom_Cnt.ContainsKey(TCur) == false)
            //                Nom_Cnt[TCur] = 1;
            //            else
            //                Nom_Cnt[TCur]++;
            //        }
            //    }
            //}


           // Save_Nom_Line_Chart(Save_Cnt, Nom_Cnt);
        }


        


        private void Push_data(Series series, string p, int p_3)
        {

            DataPoint dp = new DataPoint();
            dp.SetValueXY(p, p_3);
            dp.Label = p_3.ToString();
            series.Points.Add(dp);
        }


        private void Save_Nom_Line_Chart(Dictionary<int, int> Save_Cnt)
        {
            cls_form_Meth cm = new cls_form_Meth();
                        
            

            chart_Save.Series.Clear();
            series_Save.Points.Clear();
            series_Save.Name = cm._chang_base_caption_search("인원수");
            series_Save["DrawingStyle"] = "Emboss";
            series_Save["PointWidth"] = "0.5";
            series_Save.ChartArea = "ChartArea1";
            series_Save.ChartType = SeriesChartType.Column;
            series_Save.Legend = "Legend1";
            chart_Save.Series.Add(series_Save);

            foreach (int nodeKey in Save_Cnt.Keys)
            {
                Push_data(series_Save, nodeKey.ToString () + "Line", Save_Cnt[nodeKey]);
            }


            chart_Save.ChartAreas[0].AxisX.Interval = 1;
            chart_Save.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("맑은고딕", 9);
            chart_Save.ChartAreas[0].AxisX.LabelAutoFitMaxFontSize = 8;
            chart_Save.ChartAreas[0].AxisY.Interval = 500;

            chart_Save.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //chart_Center.ChartAreas["ChartArea1"].BackColor = Color.White;
            chart_Save.Legends[0].Enabled = true;



            //chart_Nom.Series.Clear();
            //series_Nom.Points.Clear();
            //series_Nom.Name = cm._chang_base_caption_search("추천"); ;            
            //series_Nom.ChartArea = "ChartArea1";
            //series_Nom.ChartType = SeriesChartType.Column;
            //series_Nom.Legend = "Legend1";

            //chart_Nom.Series.Add(series_Nom);

            //foreach (int nodeKey in Nom_Cnt.Keys)
            //{
            //    Push_data(series_Nom, nodeKey.ToString() + "Line", Nom_Cnt[nodeKey]);
            //}

        }






        private void ExportSiteStructure()
        {
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Charset = "";
            //this.EnableViewState = false;
            //System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            //TreeVWSite.RenderControl(oHtmlTextWriter);
            //if (trv_Member.Nodes.Count > 0)
            //{
            //    trv_Member.ren RenderControl(oHtmlTextWriter);
            //}
            //Response.Write(oStringWriter.ToString());
            //Response.End();
        }


        private void Save_TreeView_XLM()
        {
            XmlDocument d = new XmlDocument();
            XmlNode n = d.CreateNode(XmlNodeType.Element, "root", "");
            foreach (TreeNode t in trv_Member.Nodes)
            {
                n.AppendChild(getXmlNode(t, d));
            }

            d.AppendChild(n);

            int Excel_File_Cnt = 0;

            string strFolder = Application.StartupPath.ToString();
            string Base_File_Name = mtxtMbid.Text.Trim() + "의트리구조";

        _Excel_File_Re_Check:
            string Temp_Name = System.IO.Path.Combine(strFolder + "\\Excel\\" + Base_File_Name + ".XML");

            if (System.IO.File.Exists(Temp_Name) == true)
            {
                Excel_File_Cnt++;
                Base_File_Name = Base_File_Name + "(" + Excel_File_Cnt.ToString() + ")";
                goto _Excel_File_Re_Check;
            }

            d.Save(Temp_Name);
            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Save")
                        + "\n" + "File Path : " + Temp_Name);
                        

        }


        private XmlNode getXmlNode(TreeNode tnode, XmlDocument d)
        {
            XmlNode n = d.CreateNode(XmlNodeType.Element, tnode.Text.Replace(" ", "").Replace("(", "").Replace(")", ""), "");

            foreach (TreeNode t in tnode.Nodes)
            {
                n.AppendChild(getXmlNode(t, d));
            }
            return n;
        }




        private void Tree_View_Image_Make()        
        {
            LineDic.Clear();
            MakeTreeLebel_Save();
            DrawData_Label_Position_Left();
            Label_Drow_Top();
            Line_Drow_Position();
            Group_Tree_Print_Pre_BP_Work();
        }



            
        private void MakeTreeLebel_Save()
        {
            int MaxWidthNum = 0; int MaxHeightNum = 0; LastLvl = 0;
            lblY.AutoSize = true; 
            foreach (string t_key in TreeDic.Keys)
            {
                lblY.Font = new System.Drawing.Font("돋움", 8);
                lblY.Text = TreeDic[t_key].KeyName;

                if (MaxWidthNum < lblY.Width)
                    MaxWidthNum = lblY.Width;

                if (MaxHeightNum < lblY.Height)
                    MaxHeightNum = lblY.Height;                                

                if  (TreeDic[t_key].Lvl >  LastLvl)
                    LastLvl = TreeDic[t_key].Lvl ;
            }

            MaxWidthNum= MaxWidthNum + 2 ;
            MaxHeightNum = MaxHeightNum + 2;

            foreach (string t_key in TreeDic.Keys)
            {
                TreeDic[t_key].Height = MaxHeightNum;
                TreeDic[t_key].Width = MaxWidthNum;
            }     
        }
        

        private void DrawData_Label_Position_Left()
        {   string R_Key ="" ;

            R_Key = TreeDic_Cnt[0];
            TreeDic[R_Key].Left = IntervalWidth;
            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Cnt[t_key];
                    TreeDic[R_Key].Left = TreeDic[R_Key].ParentClass.Left + IntervalWidth;
                }

            }  
        }




        private void Label_Drow_Top()
        {
            string R_Key = ""; string P_Key = "";
            int move_with = 0;
            R_Key = TreeDic_Cnt[0];
            TreeDic[R_Key].Top  = 10;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Cnt[t_key];
                    P_Key = TreeDic[R_Key].ParentClass.IDKey;
                                       
                    int f_Cnt = 0 ;
                    foreach (int C_key in TreeDic[P_Key].ChildNumber.Keys )
                    {
                        if (TreeDic[P_Key].ChildNumber[C_key].IDKey == R_Key)
                        {
                            TreeDic[R_Key].Top = TreeDic[P_Key].Top + (TreeDic[R_Key].Height * (f_Cnt+1)) + (IntervalHeight * (f_Cnt+1));

                            //if (f_Cnt > 0)
                            //{
                                move_with = TreeDic[R_Key].Height   + IntervalHeight;
                                Up_Left_Total_Move(move_with, TreeDic[R_Key].Top, TreeDic[R_Key].Lvl, t_key);
                            //}
                            break;
                        }

                        f_Cnt ++ ;
                    }                        
                    
                }
            } 


        }



        private void Up_Left_Total_Move(int move_with ,int Base_Left , int Base_Lvl , int Base_Cnt )
        {
            string R_Key = ""; 

            foreach (int t_key in TreeDic_Cnt.Keys)
            {               
                R_Key = TreeDic_Cnt[t_key];

                if (TreeDic[R_Key].Lvl == Base_Lvl)
                {
                    if (t_key < Base_Cnt)
                    {
                        if (TreeDic[R_Key].Top >= Base_Left)
                            TreeDic[R_Key].Top = TreeDic[R_Key].Top + move_with;
                    }
                    else
                        break;
                }
                else
                {
                    if (TreeDic[R_Key].Top >= Base_Left)
                        TreeDic[R_Key].Top = TreeDic[R_Key].Top + move_with;

                
                
                
                }                                
            } 
        }




        private void Line_Drow_Position()
        {
            string R_Key = ""; int LineSrcCntNum = 1; int Half = 0;
            int StartY = 0; int EndY = 0;
            Dictionary<int, cls_Tree_Line> T_TreeDic = new Dictionary<int, cls_Tree_Line>();

            

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                Half = TreeDic[R_Key].Width / 2;


                cls_Tree_Line t_Line_Treel = new cls_Tree_Line();

                if (t_key > 0)  //제일위에 최상위는 위로 올라가는 선을 그릴필요 없으므로 넣지 않는다.
                {                   
                    t_Line_Treel.VisibleTF = true;
                    t_Line_Treel.X1 = TreeDic[R_Key].ParentClass.Left ;
                    t_Line_Treel.X2 = TreeDic[R_Key].Left ;

                    t_Line_Treel.Y1 = TreeDic[R_Key].Top + (TreeDic[R_Key].Height  / 2);
                    t_Line_Treel.Y2 = TreeDic[R_Key].Top + (TreeDic[R_Key].Height / 2);

                    t_Line_Treel.BX1 = t_Line_Treel.X1; t_Line_Treel.BX2 = t_Line_Treel.X2;
                    t_Line_Treel.BY1 = t_Line_Treel.Y1; t_Line_Treel.BY2 = t_Line_Treel.Y2;

                    T_TreeDic[LineSrcCntNum] = t_Line_Treel;
                    LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.                                        
                }


                if (TreeDic[R_Key].ChildCount > 0)  //하선에 사람이 잇으면 아래로 내려가는 선과 2명이상시 옆으로 가는 선을
                {

                    StartY = TreeDic[R_Key].Top + TreeDic[R_Key].Height;
                    EndY = TreeDic[R_Key].ChildNumber[TreeDic[R_Key].ChildCount].Top + (TreeDic[R_Key].Height / 2);

                    cls_Tree_Line t_Line_Treel_3 = new cls_Tree_Line();

                    t_Line_Treel_3.VisibleTF = true;

                    if (t_key > 0)  //제일위에 최상위는 위로 올라가는 선을 그릴필요 없으므로 넣지 않는다.
                    {
                        t_Line_Treel_3.X1 = t_Line_Treel.X2;               
                        t_Line_Treel_3.X2 = t_Line_Treel.X2;
                        
                    }
                    else
                    {
                        t_Line_Treel_3.X1 = TreeDic[R_Key].Left ;
                        t_Line_Treel_3.X2 = TreeDic[R_Key].Left;
                    }

                    t_Line_Treel_3.Y1 = StartY;
                    t_Line_Treel_3.Y2 = EndY;


                    t_Line_Treel_3.BX1 = t_Line_Treel_3.X1; t_Line_Treel_3.BX2 = t_Line_Treel_3.X2;
                    t_Line_Treel_3.BY1 = t_Line_Treel_3.Y1; t_Line_Treel_3.BY2 = t_Line_Treel_3.Y2;

                    T_TreeDic[LineSrcCntNum] = t_Line_Treel_3;
                    LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.

                }


            }

            LineDic = T_TreeDic;
        }







        private void Group_Tree_Print_Pre_BP_Work()
        {

            int BH = 2338   ;
            int BW = 1654;





            pbox_T.Height = BH; pbox_T.Width = BW;


            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            PB_W_Print_PagCnt = 0;
            PB_H_Print_PagCnt = 0;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                if (maxLeft < TreeDic[R_Key].Left)
                    maxLeft = TreeDic[R_Key].Left + TreeDic[R_Key].Width;

                if (minTop < TreeDic[R_Key].Top)
                    minTop = TreeDic[R_Key].Top + TreeDic[R_Key].Height;
            }
            PB_W_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                PB_W_Print_PagCnt = PB_W_Print_PagCnt + 1;

            PB_H_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                PB_H_Print_PagCnt = PB_H_Print_PagCnt + 1;

            PB_Print_W_Cur_PagCnt = 1;
            PB_Print_H_Cur_PagCnt = 1;

            int SW = 0; 
            string ap_path = Application.StartupPath.ToString();
            string SaveImage_path = Path.Combine(ap_path, "Doc");

            
            while (PB_Print_H_Cur_PagCnt <= PB_H_Print_PagCnt)
            {
                PB_Print_W_Cur_PagCnt = 1; 
                while (PB_Print_W_Cur_PagCnt <= PB_W_Print_PagCnt)
                {
                    Drawing_PictureBox_GropTree();

                    string Image_Name = mtxtMbid.Text.Trim() + "_Save_" + PB_Print_H_Cur_PagCnt + "_" + PB_W_Print_PagCnt ;
                    int Excel_File_Cnt = 0;

                _Excel_File_Re_Check:
                    string Temp_Name = System.IO.Path.Combine(SaveImage_path +"\\" + Image_Name + ".jpg");

                if (System.IO.File.Exists(Temp_Name) == true)
                    {
                        Excel_File_Cnt++;
                        Image_Name = Image_Name + "(" + Excel_File_Cnt.ToString() + ")";
                        goto _Excel_File_Re_Check;
                    }

                    pbox_T.Image.Save(Temp_Name);
                    PB_Print_W_Cur_PagCnt++;
                    SW = 1;
                }

                PB_Print_H_Cur_PagCnt++;
            }

            if (SW >=1)
                MessageBox.Show(SaveImage_path  +" " + cls_app_static_var.app_msg_rm.GetString("Msg_Save_Folder")) ;                                      


        }


        //Group_Tree_Print_Pre_BP_Work
        private void Drawing_PictureBox_GropTree()
        {
            int BH = 2338 ;
            int BW = 1654;

            Bitmap bt;
            bt = new Bitmap(BW, BH);
            pbox_T.Image = bt;

            // Graphics graphic = Graphics.FromImage(bt);

            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = BH * PB_Print_H_Cur_PagCnt - BH;
            int End_h = BH * PB_Print_H_Cur_PagCnt;

            int Start_W = BW * PB_Print_W_Cur_PagCnt - BW;
            int End_W = BW * PB_Print_W_Cur_PagCnt;

            Cut_W = BW * PB_Print_W_Cur_PagCnt - BW;
            Cut_H = BH * PB_Print_H_Cur_PagCnt - BH;

            //Graphics graphic = pbox_T.CreateGraphics();
            Graphics graphic = Graphics.FromImage(bt);

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];

                if (
                        TreeDic[R_Key].Left + TreeDic[R_Key].Width >= Start_W && TreeDic[R_Key].Left <= End_W
                        && TreeDic[R_Key].Top + TreeDic[R_Key].Height >= Start_h && TreeDic[R_Key].Top <= End_h
                        )
                {
                    tt.X = TreeDic[R_Key].Left - Cut_W;
                    tt.Y = TreeDic[R_Key].Top - Cut_H;

                    tt2.X = TreeDic[R_Key].Left - Cut_W;
                    tt2.Y = TreeDic[R_Key].Top - Cut_H;

                    tt.Width = TreeDic[R_Key].Width; tt.Height = TreeDic[R_Key].Height;
                    tt2.Width = TreeDic[R_Key].Width; tt2.Height = TreeDic[R_Key].Height;

                    msg = TreeDic[R_Key].KeyName;

                    graphic.DrawString(msg, new System.Drawing.Font("돋움",8), Brushes.Black, tt);
                    //graphic.DrawRectangle(T_p, tt2);

                }
            }

            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].BX1 - Cut_W;
                LineX2 = LineDic[t_key].BX2 - Cut_W;

                LineY1 = LineDic[t_key].BY1 - Cut_H;
                LineY2 = LineDic[t_key].BY2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    graphic.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }


        }

        private void butt_Nom_Image_Click(object sender, EventArgs e)
        {
            
        }






        private void Tree_View_Image_Make(int Cnt)
        {
            LineDic.Clear();
            MakeTreeLebel_Save(Cnt);
            DrawData_Label_Position_Left(Cnt);
            Label_Drow_Top(Cnt);
            Line_Drow_Position(Cnt);
            Group_Tree_Print_Pre_BP_Work(Cnt);
        }




        private void MakeTreeLebel_Save(int Cnt)
        {
            int MaxWidthNum = 0; int MaxHeightNum = 0; LastLvl = 0;
            lblY.AutoSize = true;
            foreach (string t_key in TreeDic_Nom.Keys)
            {
                lblY.Font = new System.Drawing.Font("돋움", 8);
                lblY.Text = TreeDic_Nom[t_key].KeyName;

                if (MaxWidthNum < lblY.Width)
                    MaxWidthNum = lblY.Width;

                if (MaxHeightNum < lblY.Height)
                    MaxHeightNum = lblY.Height;

                if (TreeDic_Nom[t_key].Lvl > LastLvl)
                    LastLvl = TreeDic_Nom[t_key].Lvl;
            }

            MaxWidthNum = MaxWidthNum + 2;
            MaxHeightNum = MaxHeightNum + 2;

            foreach (string t_key in TreeDic_Nom.Keys)
            {
                TreeDic_Nom[t_key].Height = MaxHeightNum;
                TreeDic_Nom[t_key].Width = MaxWidthNum;
            }
        }


        private void DrawData_Label_Position_Left(int Cnt)
        {
            string R_Key = "";

            R_Key = TreeDic_Nom_Cnt[0];
            TreeDic_Nom[R_Key].Left = IntervalWidth;
            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Nom_Cnt[t_key];
                    TreeDic_Nom[R_Key].Left = TreeDic_Nom[R_Key].ParentClass.Left + IntervalWidth;
                }

            }
        }




        private void Label_Drow_Top(int Cnt)
        {
            string R_Key = ""; string P_Key = "";
            int move_with = 0;
            R_Key = TreeDic_Nom_Cnt[0];
            TreeDic_Nom[R_Key].Top = 10;

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                if (t_key > 0)
                {
                    R_Key = TreeDic_Nom_Cnt[t_key];
                    P_Key = TreeDic_Nom[R_Key].ParentClass.IDKey;

                    int f_Cnt = 0;
                    foreach (int C_key in TreeDic_Nom[P_Key].ChildNumber.Keys)
                    {
                        if (TreeDic_Nom[P_Key].ChildNumber[C_key].IDKey == R_Key)
                        {
                            TreeDic_Nom[R_Key].Top = TreeDic_Nom[P_Key].Top + (TreeDic_Nom[R_Key].Height * (f_Cnt + 1)) + (IntervalHeight * (f_Cnt + 1));

                            //if (f_Cnt > 0)
                            //{
                            move_with = TreeDic_Nom[R_Key].Height + IntervalHeight;
                            Up_Left_Total_Move(move_with, TreeDic_Nom[R_Key].Top, TreeDic_Nom[R_Key].Lvl, t_key,Cnt);
                            //}
                            break;
                        }

                        f_Cnt++;
                    }

                }
            }


        }



        private void Up_Left_Total_Move(int move_with, int Base_Left, int Base_Lvl, int Base_Cnt, int Cnt)
        {
            string R_Key = "";

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];

                if (TreeDic_Nom[R_Key].Lvl == Base_Lvl)
                {
                    if (t_key < Base_Cnt)
                    {
                        if (TreeDic_Nom[R_Key].Top >= Base_Left)
                            TreeDic_Nom[R_Key].Top = TreeDic_Nom[R_Key].Top + move_with;
                    }
                    else
                        break;
                }
                else
                {
                    if (TreeDic_Nom[R_Key].Top >= Base_Left)
                        TreeDic_Nom[R_Key].Top = TreeDic_Nom[R_Key].Top + move_with;




                }
            }
        }




        private void Line_Drow_Position(int Cnt)
        {
            string R_Key = ""; int LineSrcCntNum = 1; int Half = 0;
            int StartY = 0; int EndY = 0;
            Dictionary<int, cls_Tree_Line> T_TreeDic_Nom = new Dictionary<int, cls_Tree_Line>();



            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];
                Half = TreeDic_Nom[R_Key].Width / 2;


                cls_Tree_Line t_Line_Treel = new cls_Tree_Line();

                if (t_key > 0)  //제일위에 최상위는 위로 올라가는 선을 그릴필요 없으므로 넣지 않는다.
                {
                    t_Line_Treel.VisibleTF = true;
                    t_Line_Treel.X1 = TreeDic_Nom[R_Key].ParentClass.Left;
                    t_Line_Treel.X2 = TreeDic_Nom[R_Key].Left;

                    t_Line_Treel.Y1 = TreeDic_Nom[R_Key].Top + (TreeDic_Nom[R_Key].Height / 2);
                    t_Line_Treel.Y2 = TreeDic_Nom[R_Key].Top + (TreeDic_Nom[R_Key].Height / 2);

                    t_Line_Treel.BX1 = t_Line_Treel.X1; t_Line_Treel.BX2 = t_Line_Treel.X2;
                    t_Line_Treel.BY1 = t_Line_Treel.Y1; t_Line_Treel.BY2 = t_Line_Treel.Y2;

                    T_TreeDic_Nom[LineSrcCntNum] = t_Line_Treel;
                    LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.                                        
                }


                if (TreeDic_Nom[R_Key].ChildCount > 0)  //하선에 사람이 잇으면 아래로 내려가는 선과 2명이상시 옆으로 가는 선을
                {

                    StartY = TreeDic_Nom[R_Key].Top + TreeDic_Nom[R_Key].Height;
                    EndY = TreeDic_Nom[R_Key].ChildNumber[TreeDic_Nom[R_Key].ChildCount].Top + (TreeDic_Nom[R_Key].Height / 2);

                    cls_Tree_Line t_Line_Treel_3 = new cls_Tree_Line();

                    t_Line_Treel_3.VisibleTF = true;

                    if (t_key > 0)  //제일위에 최상위는 위로 올라가는 선을 그릴필요 없으므로 넣지 않는다.
                    {
                        t_Line_Treel_3.X1 = t_Line_Treel.X2;
                        t_Line_Treel_3.X2 = t_Line_Treel.X2;

                    }
                    else
                    {
                        t_Line_Treel_3.X1 = TreeDic_Nom[R_Key].Left;
                        t_Line_Treel_3.X2 = TreeDic_Nom[R_Key].Left;
                    }

                    t_Line_Treel_3.Y1 = StartY;
                    t_Line_Treel_3.Y2 = EndY;


                    t_Line_Treel_3.BX1 = t_Line_Treel_3.X1; t_Line_Treel_3.BX2 = t_Line_Treel_3.X2;
                    t_Line_Treel_3.BY1 = t_Line_Treel_3.Y1; t_Line_Treel_3.BY2 = t_Line_Treel_3.Y2;

                    T_TreeDic_Nom[LineSrcCntNum] = t_Line_Treel_3;
                    LineSrcCntNum++;  //<<< 여기까지 본인에서 위로 반까지 올라가는 선을 그린다.

                }


            }

            LineDic = T_TreeDic_Nom;
        }







        private void Group_Tree_Print_Pre_BP_Work(int Cnt)
        {

            int BH = 2338;
            int BW = 1654;





            pbox_T.Height = BH; pbox_T.Width = BW;


            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            PB_W_Print_PagCnt = 0;
            PB_H_Print_PagCnt = 0;

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];
                if (maxLeft < TreeDic_Nom[R_Key].Left)
                    maxLeft = TreeDic_Nom[R_Key].Left + TreeDic_Nom[R_Key].Width;

                if (minTop < TreeDic_Nom[R_Key].Top)
                    minTop = TreeDic_Nom[R_Key].Top + TreeDic_Nom[R_Key].Height;
            }
            PB_W_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                PB_W_Print_PagCnt = PB_W_Print_PagCnt + 1;

            PB_H_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                PB_H_Print_PagCnt = PB_H_Print_PagCnt + 1;

            PB_Print_W_Cur_PagCnt = 1;
            PB_Print_H_Cur_PagCnt = 1;


            int SW = 0;
            string ap_path = Application.StartupPath.ToString();
            string SaveImage_path = Path.Combine(ap_path, "SaveImage");


            while (PB_Print_H_Cur_PagCnt <= PB_H_Print_PagCnt)
            {
                PB_Print_W_Cur_PagCnt = 1;
                while (PB_Print_W_Cur_PagCnt <= PB_W_Print_PagCnt)
                {
                    Drawing_PictureBox_GropTree(Cnt);
                    string Image_Name = mtxtMbid.Text.Trim() + "_Nom_" + PB_Print_H_Cur_PagCnt + "_" + PB_W_Print_PagCnt;
                    int Excel_File_Cnt = 0;

                _Excel_File_Re_Check:
                    string Temp_Name = System.IO.Path.Combine(SaveImage_path + "\\" + Image_Name + ".jpg");

                    if (System.IO.File.Exists(Temp_Name) == true)
                    {
                        Excel_File_Cnt++;
                        Image_Name = Image_Name + "(" + Excel_File_Cnt.ToString() + ")";
                        goto _Excel_File_Re_Check;
                    }
                    pbox_T.Image.Save(Temp_Name);

                    PB_Print_W_Cur_PagCnt++;
                    SW =1 ;
                }

                PB_Print_H_Cur_PagCnt++;
            }

            
            if (SW >=1)
                MessageBox.Show(SaveImage_path  +" " + cls_app_static_var.app_msg_rm.GetString("Msg_Save_Folder")) ;                                      


        }


        //Group_Tree_Print_Pre_BP_Work
        private void Drawing_PictureBox_GropTree(int Cnt)
        {
            int BH = 2338;
            int BW = 1654;

            Bitmap bt;
            bt = new Bitmap(BW, BH);
            pbox_T.Image = bt;

            // Graphics graphic = Graphics.FromImage(bt);

            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = BH * PB_Print_H_Cur_PagCnt - BH;
            int End_h = BH * PB_Print_H_Cur_PagCnt;

            int Start_W = BW * PB_Print_W_Cur_PagCnt - BW;
            int End_W = BW * PB_Print_W_Cur_PagCnt;

            Cut_W = BW * PB_Print_W_Cur_PagCnt - BW;
            Cut_H = BH * PB_Print_H_Cur_PagCnt - BH;

            //Graphics graphic = pbox_T.CreateGraphics();
            Graphics graphic = Graphics.FromImage(bt);

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];

                if (
                        TreeDic_Nom[R_Key].Left + TreeDic_Nom[R_Key].Width >= Start_W && TreeDic_Nom[R_Key].Left <= End_W
                        && TreeDic_Nom[R_Key].Top + TreeDic_Nom[R_Key].Height >= Start_h && TreeDic_Nom[R_Key].Top <= End_h
                        )
                {
                    tt.X = TreeDic_Nom[R_Key].Left - Cut_W;
                    tt.Y = TreeDic_Nom[R_Key].Top - Cut_H;

                    tt2.X = TreeDic_Nom[R_Key].Left - Cut_W;
                    tt2.Y = TreeDic_Nom[R_Key].Top - Cut_H;

                    tt.Width = TreeDic_Nom[R_Key].Width; tt.Height = TreeDic_Nom[R_Key].Height;
                    tt2.Width = TreeDic_Nom[R_Key].Width; tt2.Height = TreeDic_Nom[R_Key].Height;

                    msg = TreeDic_Nom[R_Key].KeyName;

                    graphic.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
                    //graphic.DrawRectangle(T_p, tt2);

                }
            }

            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].BX1 - Cut_W;
                LineX2 = LineDic[t_key].BX2 - Cut_W;

                LineY1 = LineDic[t_key].BY1 - Cut_H;
                LineY2 = LineDic[t_key].BY2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    graphic.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }


        }


        private void BaseDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            RectangleF tt = new RectangleF();
           // Rectangle tt2 = new Rectangle();
            string msg = "";
            
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = e.PageBounds.Height * Print_H_Cur_PagCnt - e.PageBounds.Height;
            int End_h = e.PageBounds.Height * Print_H_Cur_PagCnt;

            int Start_W = e.PageBounds.Width * Print_W_Cur_PagCnt - e.PageBounds.Width;
            int End_W = e.PageBounds.Width * Print_W_Cur_PagCnt;

            Cut_W = e.PageBounds.Width * Print_W_Cur_PagCnt - e.PageBounds.Width;
            Cut_H = e.PageBounds.Height * Print_H_Cur_PagCnt - e.PageBounds.Height;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];

                if (
                        TreeDic[R_Key].Left + TreeDic[R_Key].Width >= Start_W && TreeDic[R_Key].Left <= End_W
                        && TreeDic[R_Key].Top + TreeDic[R_Key].Height >= Start_h && TreeDic[R_Key].Top <= End_h
                        )
                {
                    tt.X = TreeDic[R_Key].Left - Cut_W;
                    tt.Y = TreeDic[R_Key].Top - Cut_H;

                    //tt2.X = TreeDic[R_Key].Left - Cut_W;
                    //tt2.Y = TreeDic[R_Key].Top - Cut_H;

                    tt.Width = TreeDic[R_Key].Width + 10 ; tt.Height = TreeDic[R_Key].Height;
                    //tt2.Width = TreeDic[R_Key].Width; tt2.Height = TreeDic[R_Key].Height;

                    msg = TreeDic[R_Key].KeyName;

                   // msg = msg + " " + msg;

                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt.X, tt.Y);
                    //e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
                    //e.Graphics.DrawRectangle(T_p, tt2);

                }
            }
            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].X1 - Cut_W;
                LineX2 = LineDic[t_key].X2 - Cut_W;

                LineY1 = LineDic[t_key].Y1 - Cut_H;
                LineY2 = LineDic[t_key].Y2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    e.Graphics.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }

            e.HasMorePages = true;

            if (Print_W_Cur_PagCnt == W_Print_PagCnt)
            {
                Print_W_Cur_PagCnt = 0;
                if (Print_H_Cur_PagCnt > H_Print_PagCnt)
                {
                    e.HasMorePages = false;
                }

                Print_H_Cur_PagCnt++;
            }

            Print_W_Cur_PagCnt++;

            //Print_Page_Cnt++;

            //if (Print_Page_Cnt == (W_Print_PagCnt * H_Print_PagCnt))
            //    e.HasMorePages = false;


            if (e.HasMorePages == false)
                Group_Tree_Print_Pre_Work();

        }

        private void Group_Tree_Print_Pre_Work()
        {
            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width;
            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            W_Print_PagCnt = 0;
            H_Print_PagCnt = 0;

            foreach (int t_key in TreeDic_Cnt.Keys)
            {
                R_Key = TreeDic_Cnt[t_key];
                if (maxLeft < TreeDic[R_Key].Left)
                    maxLeft = TreeDic[R_Key].Left + TreeDic[R_Key].Width;

                if (minTop < TreeDic[R_Key].Top)
                    minTop = TreeDic[R_Key].Top + TreeDic[R_Key].Height;
            }
            W_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                W_Print_PagCnt = W_Print_PagCnt + 1;

            H_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                H_Print_PagCnt = H_Print_PagCnt + 1;

            Print_W_Cur_PagCnt = 1;
            Print_H_Cur_PagCnt = 1;
        }


         private void BaseDoc_Nom_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            RectangleF tt = new RectangleF();
            Rectangle tt2 = new Rectangle();
            string msg = "";
            int Cut_H = 0; int Cut_W = 0;
            string R_Key = "";
            Pen T_p = new Pen(Color.Black);

            int Start_h = e.PageBounds.Height * Print_H_NOM_Cur_PagCnt - e.PageBounds.Height;
            int End_h = e.PageBounds.Height * Print_H_NOM_Cur_PagCnt;

            int Start_W = e.PageBounds.Width * Print_W_NOM_Cur_PagCnt - e.PageBounds.Width;
            int End_W = e.PageBounds.Width * Print_W_NOM_Cur_PagCnt;

            Cut_W = e.PageBounds.Width * Print_W_NOM_Cur_PagCnt - e.PageBounds.Width;
            Cut_H = e.PageBounds.Height * Print_H_NOM_Cur_PagCnt - e.PageBounds.Height;

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];

                if (
                        TreeDic_Nom[R_Key].Left + TreeDic_Nom[R_Key].Width >= Start_W && TreeDic_Nom[R_Key].Left <= End_W
                        && TreeDic_Nom[R_Key].Top + TreeDic_Nom[R_Key].Height >= Start_h && TreeDic_Nom[R_Key].Top <= End_h
                        )
                {
                    tt.X = TreeDic_Nom[R_Key].Left - Cut_W;
                    tt.Y = TreeDic_Nom[R_Key].Top - Cut_H;

                    tt2.X = TreeDic_Nom[R_Key].Left - Cut_W;
                    tt2.Y = TreeDic_Nom[R_Key].Top - Cut_H;

                    tt.Width = TreeDic_Nom[R_Key].Width; tt.Height = TreeDic_Nom[R_Key].Height;
                    tt2.Width = TreeDic_Nom[R_Key].Width; tt2.Height = TreeDic_Nom[R_Key].Height;

                    msg = TreeDic_Nom[R_Key].KeyName;

                    e.Graphics.DrawString(msg, new System.Drawing.Font("돋움", 8), Brushes.Black, tt);
                    //e.Graphics.DrawRectangle(T_p, tt2);

                }
            }
            int LineX1 = 0; int LineX2 = 0; int LineY1 = 0; int LineY2 = 0;

            foreach (int t_key in LineDic.Keys)
            {
                LineX1 = LineDic[t_key].X1 - Cut_W;
                LineX2 = LineDic[t_key].X2 - Cut_W;

                LineY1 = LineDic[t_key].Y1 - Cut_H;
                LineY2 = LineDic[t_key].Y2 - Cut_H;

                if ((LineX1 >= 0 || LineX2 >= 0) && (LineY1 >= 0 || LineY2 >= 0))
                {
                    e.Graphics.DrawLine(T_p, LineX1, LineY1, LineX2, LineY2);
                }

            }

            e.HasMorePages = true;

            if (Print_W_NOM_Cur_PagCnt == W_NOM_Print_PagCnt)
            {

                Print_W_NOM_Cur_PagCnt = 0;
                if (Print_H_NOM_Cur_PagCnt > H_NOM_Print_PagCnt)
                {
                    e.HasMorePages = false;
                }

                Print_H_NOM_Cur_PagCnt++;


            }

            Print_W_NOM_Cur_PagCnt++;

            if (e.HasMorePages == false)
                Group_Tree_Print_Pre_Work(1);
        }



        private void Group_Tree_Print_Pre_Work(int Count_1)
        {
            int BH = BaseDoc.DefaultPageSettings.PaperSize.Height;
            int BW = BaseDoc.DefaultPageSettings.PaperSize.Width;
            string R_Key = ""; int maxLeft = 0; int minTop = 0;
            W_NOM_Print_PagCnt = 0;
            H_NOM_Print_PagCnt = 0;

            foreach (int t_key in TreeDic_Nom_Cnt.Keys)
            {
                R_Key = TreeDic_Nom_Cnt[t_key];
                if (maxLeft < TreeDic_Nom[R_Key].Left)
                    maxLeft = TreeDic_Nom[R_Key].Left + TreeDic_Nom[R_Key].Width;

                if (minTop < TreeDic_Nom[R_Key].Top)
                    minTop = TreeDic_Nom[R_Key].Top + TreeDic_Nom[R_Key].Height;
            }
            W_NOM_Print_PagCnt = (maxLeft / BH);
            if ((maxLeft % BW) > 0)
                W_NOM_Print_PagCnt = W_NOM_Print_PagCnt + 1;

            H_NOM_Print_PagCnt = (minTop / BH);
            if ((minTop % BH) > 0)
                H_NOM_Print_PagCnt = H_NOM_Print_PagCnt + 1;

            Print_W_NOM_Cur_PagCnt = 1;
            Print_H_NOM_Cur_PagCnt = 1;
        }



        private void butt_Save_Print_Click(object sender, EventArgs e)
        {
            

                Button bt = (Button)sender;


                if (bt.Name == "butt_Save_Print")
                {
                   
                    if (TreeDic_Cnt == null)
                        return;
                    if (TreeDic_Cnt.Count == 0)
                        return;



                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    LineDic.Clear();
                    MakeTreeLebel_Save();
                    DrawData_Label_Position_Left();
                    Label_Drow_Top();
                    Line_Drow_Position();
                    Group_Tree_Print_Pre_Work();
                    this.Cursor = System.Windows.Forms.Cursors.Default;

                    prPrview.ShowDialog();

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    string Tsql = "";
                    string fileName = mtxtMbid.Text.Trim() + "_Print";
                    Tsql = "Insert Into tbl_Excel_User Values ( ";
                    Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                    Tsql = Tsql + "'" + this.Name + "',";
                    Tsql = Tsql + "'" + fileName + "') ";

                    Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User");
                }

                if (bt.Name == "butt_Nom_Print")
                {
                    LineDic.Clear();
                    MakeTreeLebel_Save(1);
                    DrawData_Label_Position_Left(1);
                    Label_Drow_Top(1);
                    Line_Drow_Position(1);
                    Group_Tree_Print_Pre_Work(1);

                    prPrview_Nom.ShowDialog();
                }

                if (bt.Name == "butt_Nom_Image")
                {
                    if (TreeDic_Nom_Cnt == null)
                        return;
                    if (TreeDic_Nom_Cnt.Count == 0)
                        return;


                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    Tree_View_Image_Make(1);

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

                if (bt.Name == "butt_Save_Image")
                {

                    if (TreeDic_Cnt == null)
                        return;
                    if (TreeDic_Cnt.Count == 0)
                        return;

                    //XmlHandler ttt = new XmlHandler();
                    //ttt.TreeViewToXml(trv_Member, "D:/TT");
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    //Save_TreeView_XLM();
                    Tree_View_Image_Make();

                    cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                    string Tsql = "";
                    string fileName = mtxtMbid.Text.Trim() + "_Image";
                    Tsql = "Insert Into tbl_Excel_User Values ( ";
                    Tsql = Tsql + "'" + cls_User.gid + "',Convert(Varchar(25),GetDate(),21),";
                    Tsql = Tsql + "'" + this.Name + "',";
                    Tsql = Tsql + "'" + fileName + "') ";

                    Temp_Connect.Insert_Data(Tsql, "tbl_Excel_User");
                    this.Cursor = System.Windows.Forms.Cursors.Default;

                }
            
        }






        private void Set_Form_Date_Up(string strTemp)
        {
            
            dGridView_Up_S_Header_Reset(dGridView_Up_S); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Up_S.d_Grid_view_Header_Reset();

            Base_Grid_Set(" ufn_Up_Search_Save_mannatech ");           
            
        }


        private void dGridView_Up_S_Header_Reset(DataGridView t_Dgv)
        {
            cg_Up_S.Grid_Base_Arr_Clear();

            cg_Up_S.grid_col_Count = 5;
            cg_Up_S.basegrid = t_Dgv; //dGridView_Up_S;
            cg_Up_S.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Up_S.grid_Frozen_End_Count = 2;
            cg_Up_S.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"회원_번호"  , "성명"   , "위치"  , "대수"   , ""        
                                    };
            cg_Up_S.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 70, 30, 40, 0                               
                            };
            cg_Up_S.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                                                                                   
                                   };
            cg_Up_S.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5      
                              };
            cg_Up_S.grid_col_alignment = g_Alignment;
            cg_Up_S.basegrid.RowHeadersWidth = 25;

            cg_Up_S.basegrid.ColumnHeadersDefaultCellStyle.Font =
            new Font(cg_Up_S.basegrid.Font.FontFamily, 8);
        }



        private void Base_Grid_Set(string Ufn_Name)
        {
            string T_Mbid = "";
            T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) != 1) return;

            string Tsql = "";

            Tsql = "Select  ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " T_up.mbid + '-' + Convert(Varchar,T_up.mbid2) ";
            else
                Tsql = Tsql + " T_up.mbid2 ";

            Tsql = Tsql + " ,T_up.M_Name ";
            Tsql = Tsql + " ,T_up.curP ";
            Tsql = Tsql + " ,T_up.lvl ";

            Tsql = Tsql + " From " + Ufn_Name;
            Tsql = Tsql + " ('" + Mbid + "'," + Mbid2.ToString() + ") AS T_up";

            Tsql = Tsql + " Where    lvl > 0 ";
            Tsql = Tsql + " Order BY lvl Desc ";

            //당일 등록된 회원을 불러온다.

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic_Line(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }
            cg_Up_S.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Up_S.db_grid_Obj_Data_Put();
        }




        private void Set_gr_dic_Line(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]  
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                //,ds.Tables[base_db_name].Rows[fi_cnt][4]                                                               
                                 };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void chk_Total_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (Control t_c in pb_De.Controls)
            {
                CheckBox t_cb = (CheckBox) t_c ;
                if (t_cb.Visible == true)
                    t_cb.Checked = chk_Total.Checked;
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


        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
            // SendKeys.Send("{TAB}");
        }

        private int but_Exp_Base_Left = 0;
        private int Parent_but_Exp_Base_Width = 0;

        private void but_Exp_Click(object sender, EventArgs e)
        {
            if (but_Exp.Text == "<<")
            {
                Parent_but_Exp_Base_Width = but_Exp.Parent.Width;
                but_Exp_Base_Left = but_Exp.Left;

                but_Exp.Parent.Width = but_Exp.Width;
                but_Exp.Left = 0;
                but_Exp.Text = ">>";
            }
            else
            {
                but_Exp.Parent.Width = Parent_but_Exp_Base_Width;
                but_Exp.Left = but_Exp_Base_Left;
                but_Exp.Text = "<<";
            }
        }

        private void butt_Excel_N_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_N_Info);
            e_f.ShowDialog();
        }

        private DataGridView e_f_Send_Export_Excel_N_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // this.Text; // "Sell_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Up_N;
        }

        private void butt_Excel_S_Click(object sender, EventArgs e)
        {
            frmBase_Excel e_f = new frmBase_Excel();
            e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_S_Info);
            e_f.ShowDialog();
        }


        private DataGridView e_f_Send_Export_Excel_S_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            Excel_Export_File_Name = this.Text; // this.Text; // "Sell_Select";
            Excel_Export_From_Name = this.Name;
            return dGridView_Up_S;
        }

    }
}
