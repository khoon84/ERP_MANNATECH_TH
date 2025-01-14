﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Prom_Base : Form
    {
     




        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cgb_Sell = new cls_Grid_Base();
        cls_Grid_Base cgb_Sell_Pr = new cls_Grid_Base();
        cls_Grid_Base cgb_Prom = new cls_Grid_Base();
        cls_Grid_Base cgb_Sell_2 = new cls_Grid_Base();
        cls_Grid_Base cgb_Prom_2 = new cls_Grid_Base();
        cls_Grid_Base cg_Sub = new cls_Grid_Base();

        private const string base_db_name = "tbl_Sham_Line_New";
        private int Data_Set_Form_TF;

        public frmBase_Prom_Base()
        {
            InitializeComponent();


        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
           
            Data_Set_Form_TF = 0;

            ////>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //dGridView_Good_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            //cg_Sub.d_Grid_view_Header_Reset(1);
            ////<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 1;
            Data_Set_Form_TF = 0;

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se2, combo_Se_Code2);

            Put_Good_Good_Sort_ComboBox(combo_C2Grade, combo_C2Grade_Code ,"2");
            Put_Good_Good_Sort_ComboBox(combo_CGrade, combo_CGrade_Code, "1");

            Put_Good_Good_Sort_ComboBox(combo_C2GradePr, combo_C2GradePr_Code, "2");
            Put_Good_Good_Sort_ComboBox(combo_CGradePr, combo_CGradePr_Code, "1");

            // panel9.Width = 520;
            groupBox8.Left = groupBox2.Left;
            groupBox8.Top  = groupBox2.Top;
            groupBox8.Visible = false;

            groupBox4.Left = groupBox2.Left;
            groupBox4.Top = groupBox2.Top;
            groupBox4.Visible = false;


            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate4.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate3.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakeDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakeDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            radioB_SellTF1.Checked = true;

            

            Base_Grid_Set_Good(); //상품정보를 불러온다.

            Base_Grid_Set_Good_Sell(); //상품정보를 불러온다.

            Base_Grid_Set_Good_Prom(); //상품정보를 불러온다.


            Base_Grid_Set_Good_Sell_Pr(); //상품정보를 불러온다.


            //Base_Grid_Set_Good_Sell_2(); //상품정보를 불러온다.

            //Base_Grid_Set_Good_Prom_2(); //상품정보를 불러온다.


            radioB_SellTF1.Checked = true;
            groupBox2.Visible = true;
            groupBox4.Visible = false;
            groupBox8.Visible = false;

            radioB_Mem_Reg_FLAG_C.Checked = true;
            radioB_Center.Checked = true;
            radioB_Ba.Checked = true;
            radioB_Ba_Pr.Checked = true;
            radioB_Using_FLAG_Y.Checked = true;
            radioB_Auto_Using_FLAG_Y.Checked = true;

            radioB_Ba2.Checked = true;
            radioB_Over2.Checked = true;

            radioB_Over.Checked = true;

            radioB_Item_Prom_FLAG_1.Checked = true;
            radioB_Order_Count_First_FLAG_Y.Checked = true;

        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
            //19-03-11 깜빡임제거 this.Refresh();

            if (cls_User.uSearch_MemberNumber != "")
            {
                Data_Set_Form_TF = 1;
                mtxtMbid.Text = cls_User.uSearch_MemberNumber;
                mtxtSMbid.Text = cls_User.uSearch_MemberNumber;
                cls_User.uSearch_MemberNumber = "";

                EventArgs ee1 = null; Base_Button_Click(butt_Search, ee1);
                Set_Form_Date(mtxtMbid.Text);
                Data_Set_Form_TF = 0;
            }
        }

        private void frmBase_Resize(object sender, EventArgs e)
        {
            butt_Clear.Left = 0;
            butt_Save.Left = butt_Clear.Left + butt_Clear.Width + 2;
            butt_Delete.Left = butt_Save.Left + butt_Save.Width + 2;
            //butt_Delete.Left = butt_Excel.Left + butt_Excel.Width + 2;
            butt_Exit.Left = this.Width - butt_Exit.Width - 17;


            cls_form_Meth cfm = new cls_form_Meth();
            cfm.button_flat_change(butt_Clear);
            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_Delete);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(butt_Exit);

            cfm.button_flat_change(butt_Search);
            cfm.button_flat_change(butt_Excel);
            cfm.button_flat_change(button_Sort);
            cfm.button_flat_change(button_Add_Up);
            cfm.button_flat_change(button_Add_UpPr);
            cfm.button_flat_change(button_SortPr);

            cfm.button_flat_change(button_Add_Down);
            cfm.button_flat_change(button_Add_Down_Pr);
            


        }


        private void frmBase_From_KeyDown(object sender, KeyEventArgs e)
        {
            //폼일 경우에는 ESC버튼에 폼이 종료 되도록 한다
            if (sender is Form)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    if (!this.Controls.ContainsKey("Popup_gr"))
                    {
                        this.Close();
                        return;
                    }
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
                            return;
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
                T_bt = butt_Save;     //저장  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //리셋  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112)
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

                if (tb.Name == "txt_Pv")
                {
                    if (tb.Text != "")
                        tb.Text = string.Format(cls_app_static_var.str_Currency_Type, double.Parse(tb.Text.Replace(",", "")));
                }

            }

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                if (tb.ReadOnly == false)
                    tb.BackColor = Color.White;
            }
        }



        //회원번호 클릿햇을때. 관련 정보들 다 리셋 시킨다.
        //추후 번호만 변경하고 엔터 안누눌러서.. 데이타가 엉키는 것을 방지하기 위함.
        private void mtxtMbid_Click(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);


            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 1;
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

                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid")
                        {
                            txtName.Text = Search_Name;
                            if (Input_Error_Check(mtb, 0) == true)
                                Set_Form_Date(mtb.Text);
                            //SendKeys.Send("{TAB}");

                        }

                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

                        //cls_app_static_var.Search_Member_Number_Mbid = Mbid;
                        //cls_app_static_var.Search_Member_Number_Mbid2 = Mbid2;
                        frmBase_Member_Search e_f = new frmBase_Member_Search();

                        if (mtb.Name == "mtxtMbid")
                        {
                            e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                            e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemNumber_Info);
                        }

                        e_f.ShowDialog();

                        SendKeys.Send("{TAB}");
                    }
                }
                else
                    SendKeys.Send("{TAB}");
            }

        }



        void e_f_Send_MemNumber_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            seachName = "";
            cls_Search_DB csb = new cls_Search_DB();
            csb.Member_Nmumber_Split(mtxtMbid.Text.Trim(), ref searchMbid, ref searchMbid2);
        }

        //변경할려는 대상자에 대한 회원번호에서 회원 검색창을 뛰엇을 경우에
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, 0) == true)
                Set_Form_Date(mtxtMbid.Text);
        }



        //회원번호 입력 박스의 내역이 모두 지워지면 하부 관련 회원데이타 내역을 다 리셋 시킨다. 
        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {
            MaskedTextBox mtb = (MaskedTextBox)sender;

            if (mtb.Text.Replace("_", "").Replace("-", "").Replace(" ", "") == "")
            {
                cls_form_Meth ct = new cls_form_Meth();
                if (mtb.Name == "mtxtMbid")
                {
                    ct.from_control_clear(this, mtb);

                }
                //    ct.from_control_clear(groupBox2, mtb);

                //ct.from_control_clear((GroupBox)mtb.Parent, mtb);
            }
        }



        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);

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


            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-1"))
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, -1) == false)
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

            else if (tb.Tag.ToString() == "name")  //회원 정보 관련해서 이름 검색을 필요로 하는 텍스트 박스이다.
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(tb, e) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

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
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, 0) == true)
                            Set_Form_Date(mtxtMbid.Text);

                        //SendKeys.Send("{TAB}");
                    }


                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    frmBase_Member_Search e_f = new frmBase_Member_Search();
                    if (tb.Name == "txtName")
                    {
                        e_f.Send_Mem_Number += new frmBase_Member_Search.SendNumberDele(e_f_Send_Mem_Number);
                        e_f.Call_searchNumber_Info += new frmBase_Member_Search.Call_searchNumber_Info_Dele(e_f_Send_MemName_Info);
                    }

                    e_f.ShowDialog();

                    SendKeys.Send("{TAB}");
                }


            }
            else
                SendKeys.Send("{TAB}");

        }

        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
        }




        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;

            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemName.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCodeUp")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemNameUp.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCodeUpPr")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemNameUpPr.Text = "";
                Data_Set_Form_TF = 0;
            }


            if (tb.Name == "txt_ItemCodePr")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txt_ItemNamePr.Text = "";
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txtR_Id")
            {
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                else if (Sw_Tab == 1)
                    Ncod_Text_Set_Data(tb, txtR_Id_Code);
            }

        }



        void T_R_Key_Enter_13()
        {

            SendKeys.Send("{TAB}");
        }


        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
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

            if (tb.Name == "txtCenter")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtCenter_Code);              
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtCenter2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtCenter_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtCenter_Code2);

            //    SendKeys.Send("{TAB}");
            //    Data_Set_Form_TF = 0;
            //}

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txtR_Id_Code);               
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCode")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemName);               
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCodeUp")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemNameUp);
                Data_Set_Form_TF = 0;
            }

            if (tb.Name == "txt_ItemCodeUpPr")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemNameUpPr);
                Data_Set_Form_TF = 0;
            }
            if (tb.Name == "txt_ItemCodePr")
            {
                Data_Set_Form_TF = 1;
                Db_Grid_Popup(tb, txt_ItemNamePr);
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
                cgb_Pop.Next_Focus_Control = txtRemark_Sell;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = txt_ItemCount;

            if (tb.Name == "txt_ItemCodeUp")
                cgb_Pop.Next_Focus_Control = txt_ItemCountUp;


            if (tb.Name == "txt_ItemCodeUpPr")
                cgb_Pop.Next_Focus_Control = txt_ItemCountUpPr;



            if (tb.Name == "txt_ItemCodePr")
                cgb_Pop.Next_Focus_Control = txt_ItemCountPr;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name != "txt_ItemCodePr" || tb.Name == "txt_ItemCode" || tb.Name == "txt_ItemCodeUp" || tb.Name == "txt_ItemCodeUpPr")
            {
                if (mtxtSellDate.Text.Replace("-", "") == "")
                    cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, "KR", mtxtSellDate.Text, "", 1, "");
                else
                    cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, "KR",cls_User.gid_date_time , "", 1, "");
            }
            else
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
            cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            cgb_Pop.Base_Location_obj = tb;

            if (strSql != "")
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", strSql);

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", strSql);

                if (tb.Name == "txt_ItemName2")
                    cgb_Pop.db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", strSql);

                if (tb.Name == "txtSellCode")
                {
                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", strSql);
                    cgb_Pop.Next_Focus_Control = txt_Pv;
                }

            }
            else
            {
                if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
                {
                    string Tsql;
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Business (nolock) ";
                    Tsql = Tsql + " Where  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                    Tsql = Tsql + " Order by Ncode ";

                    cgb_Pop.db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);
                }

                if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
                {
                    string Tsql;
                    Tsql = "Select user_id ,U_Name   ";
                    Tsql = Tsql + " From tbl_User (nolock) ";
                    Tsql = Tsql + " Order by user_id ";

                    cgb_Pop.db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);
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

                if (tb.Name == "txtSellCode")
                {
                    string Tsql;
                    Tsql = "Select SellCode ,SellTypeName    ";
                    Tsql = Tsql + " From tbl_SellType (nolock) ";
                    Tsql = Tsql + " Order by SellCode ";

                    cgb_Pop.db_grid_Popup_Base(2, "구매_코드", "구매종류", "SellCode", "SellTypeName", Tsql);
                    cgb_Pop.Next_Focus_Control = txt_Pv;
                }

            }
        }



        private void Ncod_Text_Set_Data(TextBox tb, TextBox tb1_Code)
        {
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql = "";

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2")
            {
                Tsql = "Select  Ncode, Name   ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
                Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
            }

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2")
            {
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where U_Name like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%'";
            }

            if (tb.Name == "txt_ItemName2")
            {
                Tsql = "Select Ncode , Name    ";
                Tsql = Tsql + " From tbl_Goods (nolock) ";
                Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%'";
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


















        private void Form_Clear_()
        {



            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //mtxtMbid.ReadOnly = false;
            //txtName.ReadOnly = false;
            //mtxtSellDate.ReadOnly = false;

            //mtxtMbid.BorderStyle = BorderStyle.Fixed3D;
            //txtName.BorderStyle = BorderStyle.Fixed3D;
            //mtxtSellDate.BorderStyle = BorderStyle.Fixed3D;

            //mtxtMbid.BackColor = SystemColors.Window;
            //txtName.BackColor = SystemColors.Window;
            //mtxtSellDate.BackColor = SystemColors.Window;

            //DTP_SellDate.Visible = true;
            //tableLayoutPanel1.Enabled = true;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txt_Pro_Code);

            Base_Grid_Set_Good_Sell_Pr(); //상품정보를 불러온다.

            Base_Grid_Set_Good(); //상품정보를 불러온다.

            Base_Grid_Set_Good_Sell(); //상품정보를 불러온다.
            
            Base_Grid_Set_Good_Prom(); //상품정보를 불러온다.


           // Base_Grid_Set_Good_Sell_2(); //상품정보를 불러온다.

           // Base_Grid_Set_Good_Prom_2(); //상품정보를 불러온다.


            radioB_SellTF1.Checked = true;
            groupBox2.Visible = true;
            groupBox4.Visible = false;
            groupBox8.Visible = false;

            radioB_Mem_Reg_FLAG_C.Checked = true;
            radioB_Center.Checked = true;
            radioB_Ba.Checked = true;
            radioB_Ba_Pr.Checked = true;
            
            radioB_Using_FLAG_Y.Checked = true;
            radioB_Auto_Using_FLAG_Y.Checked = true;

            radioB_Ba2.Checked = true;
            radioB_Over2.Checked = true;

            radioB_Over.Checked = true;

            radioB_Item_Prom_FLAG_1.Checked = true;
            radioB_Order_Count_First_FLAG_Y.Checked = true;

            panel_Pro_Code.Enabled = true; 
            panel_SellTF.Enabled = true;

        }







        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Form_Clear_();
            }

            else if (bt.Name == "butt_Save")
            {
                int Save_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Save_Base_Data(ref Save_Error_Check);

                if (Save_Error_Check > 0)
                {
                    Form_Clear_();
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }

            else if (bt.Name == "butt_Delete")
            {
                int Delete_Error_Check = 0;
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Delete_Base_Data(ref Delete_Error_Check);

                if (Delete_Error_Check > 0)
                    Form_Clear_();

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            else if (bt.Name == "butt_Search")
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                combo_Se_Code2.SelectedIndex = combo_Se2.SelectedIndex;
                Base_Sub_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            else if (bt.Name == "butt_Excel")
            {
                frmBase_Excel e_f = new frmBase_Excel();
                e_f.Send_Export_Excel_Info += new frmBase_Excel.Send_Export_Excel_Info_Dele(e_f_Send_Export_Excel_Info);
                e_f.ShowDialog();
            }

        }


        private DataGridView e_f_Send_Export_Excel_Info(ref string Excel_Export_From_Name, ref string Excel_Export_File_Name)
        {
            cls_form_Meth cm = new cls_form_Meth();
            Excel_Export_File_Name = this.Text; // cm._chang_base_caption_search ( "인정_매출") ;
            Excel_Export_From_Name = this.Name;
            return dGridView_Base_Sub;
        }




















        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
        }





        private Boolean Search_Check_TextBox_Error()
        {

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            if (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtSMbid);

                if (Ret == -1)
                {
                    mtxtSMbid.Focus(); return false;
                }
            }


            if (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            {
                int Ret = 0;
                Ret = c_er._Member_Nmumber_Split(mtxtSMbid2);

                if (Ret == -1)
                {
                    mtxtSMbid2.Focus(); return false;
                }
            }


            if (mtxtSellDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate2.Text, mtxtSellDate2, "Date") == false)
                {
                    mtxtSellDate2.Focus(); return false;
                }
            }


            if (mtxtSellDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate3.Text, mtxtSellDate3, "Date") == false)
                {
                    mtxtSellDate3.Focus(); return false;
                }
            }


            if (mtxtMakeDate1.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakeDate1.Text, mtxtMakeDate1, "Date") == false)
                {
                    mtxtMakeDate1.Focus(); return false;
                }
            }

            if (mtxtMakeDate2.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtMakeDate2.Text, mtxtMakeDate2, "Date") == false)
                {
                    mtxtMakeDate2.Focus(); return false;
                }
            }


            return true;
        }



        private void Base_Sub_Grid_Set()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            if (Search_Check_TextBox_Error() == false) return;


            string Tsql = "";

            //string[] g_HeaderText = {"_Seq"  , "등록번호"   , "명칭"  , "적용시작일"   , "적용종료일"        
            //                        , "구분"   , "회원적용"    , "오토쉽적용"   , "사용여부"    , "주문내역비고"    
            //                         ,"비고"    ,"기록자"     ,"기록일" ,"_Prom_FLAG" , "_Mem_Reg_FLAG"
            //                          , "_Auto_Using_FLAG" , "_Using_FLAG" , "_Item_Prom_FLAG" 
            //                            };

            //if (radioB_SellTF1.Checked == true) Prom_FLAG = "S";  // 상품기준
            //if (radioB_SellTF2.Checked == true) Prom_FLAG = "P";  // 금액기준
            //if (radioB_SellTF3.Checked == true) Prom_FLAG = "SP";
            //if (radioB_SellTF4.Checked == true) Prom_FLAG = "V";  //PV기준
            //if (radioB_SellTF5.Checked == true) Prom_FLAG = "SV";

            //if (radioB_Mem_Reg_FLAG_C.Checked == true) Mem_Reg_FLAG = "C";  // 당월가입회원
            //if (radioB_Mem_Reg_FLAG_T.Checked == true) Mem_Reg_FLAG = "T";  // 모든 회원 대상

            //if (radioB_Using_FLAG_Y.Checked == true) Using_FLAG = "Y";  // 사용함
            //if (radioB_Using_FLAG_N.Checked == true) Using_FLAG = "N";  // 사용안함

            //if (radioB_Auto_Using_FLAG_Y.Checked == true) Auto_Using_FLAG = "Y";  //오토쉽에 적용
            //if (radioB_Auto_Using_FLAG_Y.Checked == true) Auto_Using_FLAG = "N";  //오토쉽에는 미적용


            //string Item_Prom_FLAG = "";
            //if (radioB_SellTF1.Checked == true)
            //{
            //    if (radioB_Item_Prom_FLAG_1.Checked == true) Item_Prom_FLAG = "1";  // 1:1  증정 프로모션 
            //    if (radioB_Item_Prom_FLAG_2.Checked == true) Item_Prom_FLAG = "2";  // 다: 1 증정 프로모션
            //}
            // StrSql = StrSql + ", ETC_Sell = '" + txtRemark_Sell.Text + "'";
            //StrSql = StrSql + ", ETC_Memo = '" + txtRemark.Text + "'";

            //Pro_Code  Seq

            Tsql = "Select Seq,  Pro_Code ";            
            Tsql = Tsql + " ,tbl_Goods_Prom_Base.Pro_Name ";
            Tsql = Tsql + " ,LEFT(Start_Date,4) +'-' + LEFT(RIGHT(Start_Date,4),2) + '-' + RIGHT(Start_Date,2) ";
            Tsql = Tsql + " ,LEFT(End_Date,4) +'-' + LEFT(RIGHT(End_Date,4),2) + '-' + RIGHT(End_Date,2) ";

            Tsql = Tsql + " , Case When Prom_FLAG = 'S'  then '상품기준'   ";
            Tsql = Tsql + "        When Prom_FLAG = 'P'  then '금액기준' ";
            Tsql = Tsql + "        When Prom_FLAG = 'V'  then 'PV기준' ";
            Tsql = Tsql + "        END ";  //구분

            Tsql = Tsql + " , Case When Mem_Reg_FLAG = 'C'  then '당월가입회원'   ";
            Tsql = Tsql + "        When Mem_Reg_FLAG = 'T'  then '모든회원대상' ";            
            Tsql = Tsql + "        END ";  //회원적용

            Tsql = Tsql + " , Case When BusCode_FLAG <> ''  then isnull(tbl_Business.Name,'')   ";
            Tsql = Tsql + "        ELSE  '모든센타' ";
            Tsql = Tsql + "        END ";  //회원적용


            

            Tsql = Tsql + " , Case When Auto_Using_FLAG = 'Y'  then 'Y'   ";
            Tsql = Tsql + "        ELSE 'N' ";
            Tsql = Tsql + "        END ";  //오토쉽적용

            Tsql = Tsql + " , Case When Using_FLAG = 'Y'  then 'Y'   ";
            Tsql = Tsql + "        ELSE 'N' ";
            Tsql = Tsql + "        END ";  //사용여부

            Tsql = Tsql + " ,tbl_Goods_Prom_Base.ETC_Sell ";  //주문내역비고
            Tsql = Tsql + " ,tbl_Goods_Prom_Base.ETC_Memo ";   //비고
            Tsql = Tsql + " ,tbl_Goods_Prom_Base.RecordID ";
            Tsql = Tsql + " ,tbl_Goods_Prom_Base.RecordTime ";


            Tsql = Tsql + " ,Prom_FLAG ";
            Tsql = Tsql + " ,Mem_Reg_FLAG ";
            Tsql = Tsql + " ,Auto_Using_FLAG ";
            Tsql = Tsql + " ,Using_FLAG ";            
            Tsql = Tsql + " ,Item_Prom_FLAG ";
            Tsql = Tsql + " ,BusCode_FLAG ";
            Tsql = Tsql + " ,Order_Count_First_FLAG ";
            
            Tsql = Tsql + " From tbl_Goods_Prom_Base  (nolock) ";
            Tsql = Tsql + " LEFT JOIN  tbl_Business   (nolock) on tbl_Business.Ncode = tbl_Goods_Prom_Base.BusCode_FLAG ";
            string strSql = " Where   tbl_Goods_Prom_Base.Pro_Code <> '' ";

            //string Mbid = ""; int Mbid2 = 0;
            ////회원번호1로 검색
            //if (
            //    (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    &&
            //    (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid ='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid2 = " + Mbid2;
            //    }
            //}


            ////회원번호2로 검색
            //if (
            //    (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    &&
            //    (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //    )
            //{
            //    cls_Search_DB csb = new cls_Search_DB();
            //    if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid >='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid2 >= " + Mbid2;
            //    }

            //    if (csb.Member_Nmumber_Split(mtxtSMbid2.Text, ref Mbid, ref Mbid2) == 1)
            //    {
            //        if (Mbid != "")
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid <='" + Mbid + "'";

            //        if (Mbid2 >= 0)
            //            strSql = strSql + " And tbl_Goods_Prom_Base.Mbid2 <= " + Mbid2;
            //    }
            //}

            //회원명으로 검색
            if (txtName2.Text.Trim() != "")
                strSql = strSql + " And tbl_Goods_Prom_Base.Pro_Name Like '%" + txtName2.Text.Trim() + "%'";

         


            //가입일자로 검색 -1
            if ((mtxtSellDate2.Text.Replace("-", "").Trim() != "") && (mtxtSellDate3.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Goods_Prom_Base.Start_Date = '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate2.Text.Replace("-", "").Trim() != "") && (mtxtSellDate3.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Goods_Prom_Base.Start_Date >= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Goods_Prom_Base.Start_Date <= '" + mtxtSellDate3.Text.Replace("-", "").Trim() + "'";
            }


            //기록일자로 검색 -1
            if ((mtxtMakeDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakeDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And tbl_Goods_Prom_Base.End_Date = '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
            //strSql = strSql + " And Replace(Left( tbl_Goods_Prom_Base.recordtime ,10),'-','') = '" + mtxtMakeDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakeDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakeDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And tbl_Goods_Prom_Base.End_Date >= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And tbl_Goods_Prom_Base.End_Date <= '" + mtxtSellDate3.Text.Replace("-", "").Trim() + "'";
                //strSql = strSql + " And Replace(Left( tbl_Goods_Prom_Base.recordtime ,10),'-','') >= '" + mtxtMakeDate1.Text.Replace("-", "").Trim() + "'";
                //strSql = strSql + " And Replace(Left( tbl_Goods_Prom_Base.recordtime ,10),'-','') <= '" + mtxtMakeDate2.Text.Replace("-", "").Trim() + "'";
            }


            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And tbl_Goods_Prom_Base.recordid = '" + txtR_Id_Code.Text.Trim() + "'";





            //strSql = strSql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            //strSql = strSql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";



            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by tbl_Goods_Prom_Base.Pro_Code ";

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
                Set_Sub_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cg_Sub.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cg_Sub.db_grid_Obj_Data_Put();
        }


        private void Set_Sub_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cg_Sub.grid_col_Count];

            while (Col_Cnt < cg_Sub.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Base_Sub_Header_Reset()
        {
            cg_Sub.grid_col_Count = 21;
            cg_Sub.basegrid = dGridView_Base_Sub;
            cg_Sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub.grid_Frozen_End_Count = 2;
            cg_Sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"_Seq"  , "등록번호"   , "명칭"  , "적용시작일"   , "적용종료일"
                                    , "구분"   , "회원적용"  ,"적용센타"  , "오토쉽적용"   , "사용여부"
                                     , "주문내역비고" ,"비고"    ,"기록자"     ,"기록일" ,"_Prom_FLAG"
                                     , "_Mem_Reg_FLAG" , "_Auto_Using_FLAG" , "_Using_FLAG" , "_Item_Prom_FLAG" , "_BusCode_FLAG"
                                     , "_Order_Count_First_FLAG_Y"
                                        };

            cg_Sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0,  110, 100, 90, 90
                             , 110 ,70 , 100 ,  100 , 100
                             , 110 ,70 , 100 ,  100 , 0
                             , 0 ,0 , 0 ,  0 , 0
                             ,0
                            };
            cg_Sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            cg_Sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft //10                                   

                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft 
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter

                               ,DataGridViewContentAlignment.MiddleCenter

                              };
            cg_Sub.grid_col_alignment = g_Alignment;

            //Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            //gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            //cg_Sub.grid_cell_format = gr_dic_cell_format;
        }















        //private bool numericCheck(string ss)
        //{
        //     cls_Check_Text T_R = new cls_Check_Text();

        //    //쿼리문 오류관련 입력만 아니면 가능하다.
        //    if (T_R.Text_KeyChar_Check(e, 1) == false)
        //    {
        //        e.Handled = true;
        //        return;
        //    } // end if   

        //    //try
        //    //{
        //    //    int ll = Convert.ToInt32(ss);
        //    //    return true;
        //    //}
        //    //catch
        //    //{
        //    //    return false;
        //    //}
        //}





        private Boolean Check_TextBox_Error()
        {

            cls_Check_Text T_R = new cls_Check_Text();
            string me = "";




            //if (txtSellCode_Code.Text == "" || txtSellCode.Text == "")
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_SellCode")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txtSellCode.Focus(); return false;
            //}

            cls_Check_Input_Error c_er = new cls_Check_Input_Error();

            //if (mtxtMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
            //{
            //    int Ret = 0;
            //    Ret = c_er._Member_Nmumber_Split(mtxtMbid);

            //    if (Ret == -1)
            //    {
            //        mtxtMbid.Focus(); return false;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
            //           + "\n" +
            //           cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    mtxtMbid.Focus(); return false;
            //}





            if (txt_Pro_Code.Text != "" && panel_Pro_Code.Enabled == false)  //수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
            {
            }
            else           
            {
                if (txt_Pro_Code.Text == "")
                {
                    MessageBox.Show("등록 번호를 입력해 주십시요."
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Pro_Code.Focus(); return false;
                }

                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                string StrSql = "Select Seq   ";
                StrSql = StrSql + " From tbl_Goods_Prom_Base (nolock) ";
                StrSql = StrSql + " Where Pro_Code = '" + txt_Pro_Code.Text + "'";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return false ;
                int ReCnt22 = Temp_Connect.DataSet_ReCount;

                int Seq = 0;
                if (ReCnt22 > 0)
                {
                    MessageBox.Show("동일한 프로모션 등록 코드가 존재합니다."
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txt_Pro_Code.Focus(); return false;
                }
            }


            if (txtName.Text == "")
            {
                MessageBox.Show("프로모션 명칭을 입력해 주십시요."
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtName.Focus(); return false;
            }

            if (mtxtSellDate.Text.Replace("-", "").Trim() == "" )
            {
                MessageBox.Show("적용기간 시작일을 입력해 주십시요."
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }


            if (mtxtSellDate4.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show("적용기간 종료일을 입력해 주십시요."
                  + "\n" +
                  cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate4.Focus(); return false;
            }



            if (mtxtSellDate.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sham_Date")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }

            if (mtxtSellDate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate.Text, mtxtSellDate, "Date") == false)
                {
                    mtxtSellDate.Focus(); return false;
                }
            }


            if (mtxtSellDate4.Text.Replace("-", "").Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Sham_Date")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate4.Focus(); return false;
            }

            if (mtxtSellDate4.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate4.Text, mtxtSellDate, "Date") == false)
                {
                    mtxtSellDate4.Focus(); return false;
                }
            }


            if (int.Parse (mtxtSellDate.Text.Replace("-", "").Trim()) > int.Parse ( mtxtSellDate4.Text.Replace("-", "").Trim() ))
            {
                MessageBox.Show("기간 설정이 잘못 되었습니다. 시작일이 종료일 보다 큽니다."
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtSellDate.Focus(); return false;
            }

            if (radioB_Center.Checked == true)
            {
                if (txtCenter_Code.Text == "")
                {
                    MessageBox.Show("적용 센타를 선택해 주십시요."
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtCenter.Focus(); return false;
                }
            }

            if (radioB_Center_Not.Checked == true)
            {
                if (txtCenter_Code.Text != "")
                {
                    MessageBox.Show("적용 센타를 선택 할 필요가 없습니다."
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    txtCenter.Focus(); return false;
                }
            }



            //me = T_R.Text_Null_Check(txt_Pv, "Msg_Sort_Sham_PV"); //적용 PV를
            //if (me != "")
            //{
            //    MessageBox.Show(me);
            //    return false;
            //}



            ////마감정산이 이루어진 판매 날짜인지 체크한다.                
            //cls_Search_DB csd = new cls_Search_DB();
            //if (csd.Close_Check_SellDate("tbl_CloseTotal_02", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
            //{
            //    mtxtSellDate.Focus(); return false;
            //}


            ////if (csd.Close_Check_SellDate("tbl_CloseTotal_04", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
            ////{
            ////    mtxtSellDate.Focus(); return false;
            ////}


            return true;
        }


        private Boolean Check_TextBox_Error_S()
        {
                                  
            int Sell_ItemCnt = 0;             
            for (int i = 0; i < dGridView_Good_Sell.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good_Sell.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    Sell_ItemCnt++;                       
                }
            }
              

            if (Sell_ItemCnt == 0)
            {
                MessageBox.Show("구매상품 선택 상품이 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                dGridView_Good_Sell.Focus();
                return false;
            }


            int Prom_ItemCnt = 0;
            for (int i = 0; i < dGridView_Good_Prom.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good_Prom.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    Prom_ItemCnt++;
                }
            }


            if (Prom_ItemCnt == 0)
            {
                MessageBox.Show("증정상품 선택 상품이 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                dGridView_Good_Prom.Focus();
                return false;
            }          

            return true;
        }


        private Boolean Check_TextBox_Error_P()
        {

            int Sell_ItemCnt = 0;
            for (int i = 0; i < dGridView_Good.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    Sell_ItemCnt++;
                }
            }


            if (Sell_ItemCnt == 0)
            {
                MessageBox.Show("증정상품 선택 상품이 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                dGridView_Good_Sell.Focus();
                return false;
            }

            if (radioB_Over.Checked == true)
            {
                if (txt_Pv.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv.Focus();
                    return false;
                }

                if (txt_Pv2.Text == "" || txt_Pv2.Text == "0")
                {
                }
                else
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv2.Focus();
                    return false;
                }
            }

            if (radioB_Inner.Checked == true)
            {
                if (txt_Pv.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv.Focus();
                    return false;
                }

                if (txt_Pv2.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv2.Focus();
                    return false;
                }

                if (int.Parse (txt_Pv.Text.Replace (",","")) > int.Parse(txt_Pv2.Text.Replace(",", "")))
                {                
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv2.Focus();
                    return false;
                }


                if (radioB_Ba_Pr.Checked == true && radioB_Over.Checked == false  )  //배수로지급할때. 초과가 아니면 배수ㅏ로 지급이 안된다.
                {
                    MessageBox.Show("초과 일때만 배수 지급 체크가 가능 합니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    radioB_Ba_Pr.Focus();
                    return false;
                }
            }

            return true;
        }


        private Boolean Check_TextBox_Error_SP()
        {

            int Sell_ItemCnt = 0;
            for (int i = 0; i < dGridView_Good_Sell_2.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good_Sell_2.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    Sell_ItemCnt++;
                }
            }


            if (Sell_ItemCnt == 0)
            {
                MessageBox.Show("구매상품 선택 상품이 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                dGridView_Good_Sell_2.Focus();
                return false;
            }


            int Prom_ItemCnt = 0;
            for (int i = 0; i < dGridView_Good_Prom_2.Rows.Count; i++)
            {
                if (int.Parse(dGridView_Good_Prom_2.Rows[i].Cells[0].Value.ToString()) > 0)
                {
                    Prom_ItemCnt++;
                }
            }


            if (Prom_ItemCnt == 0)
            {
                MessageBox.Show("증정상품 선택 상품이 없습니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                dGridView_Good_Prom_2.Focus();
                return false;
            }

            if (radioB_Over2.Checked == true)
            {
                if (txt_Pv3.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv3.Focus();
                    return false;
                }

                if (txt_Pv4.Text != "" && txt_Pv4.Text != "0")
                {
                }
                else
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv4.Focus();
                    return false;
                }
            }

            if (radioB_Inner2.Checked == true)
            {
                if (txt_Pv3.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv3.Focus();
                    return false;
                }

                if (txt_Pv4.Text == "")
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv4.Focus();
                    return false;
                }

                if (int.Parse(txt_Pv3.Text) > int.Parse(txt_Pv4.Text))
                {
                    MessageBox.Show("기준금액이상 과 관련된 금액 설정 오류 입니다."
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                    txt_Pv3.Focus();
                    return false;
                }
            }



            return true;
        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            if (txt_Pro_Code.Text != "" && panel_Pro_Code.Enabled == false)
                str_Q = "Msg_Base_Edit_Q";            
            else
                str_Q = "Msg_Base_Save_Q";


             if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;
                                   
           
            if (Check_TextBox_Error() == false) return;

            if (radioB_SellTF1.Checked == true)
            {
                if (Check_TextBox_Error_S() == false) return;
            }

            if (radioB_SellTF2.Checked == true || radioB_SellTF4.Checked == true)
            {
                if (Check_TextBox_Error_P() == false) return;
            }

            if (radioB_SellTF3.Checked == true)
            {
                if (Check_TextBox_Error_SP() == false) return;
            }


            if (txt_Pro_Code.Text != "" && panel_Pro_Code.Enabled == false)  //수정을 하는 경우에는//수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
            {
                Save_Base_Data_UpDate(ref Save_Error_Check);
                if (Save_Error_Check > 0)
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit"));
                return;
            }

            string StrSql = "";
            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string Prom_FLAG = "", Mem_Reg_FLAG = "", Using_FLAG = "", Auto_Using_FLAG = ""; ;

                if (radioB_SellTF1.Checked == true) Prom_FLAG = "S";  // 상품기준
                if (radioB_SellTF2.Checked == true) Prom_FLAG = "P";  // 금액기준
                if (radioB_SellTF3.Checked == true) Prom_FLAG = "SP";
                if (radioB_SellTF4.Checked == true) Prom_FLAG = "V";  //PV기준
                if (radioB_SellTF5.Checked == true) Prom_FLAG = "SV";

                if (radioB_Mem_Reg_FLAG_C.Checked == true) Mem_Reg_FLAG = "C";  // 당월가입회원
                if (radioB_Mem_Reg_FLAG_T.Checked == true) Mem_Reg_FLAG = "T";  // 모든 회원 대상

                if (radioB_Using_FLAG_Y.Checked == true) Using_FLAG = "Y";  // 사용함
                if (radioB_Using_FLAG_N.Checked == true) Using_FLAG = "N";  // 사용안함

                if (radioB_Auto_Using_FLAG_Y.Checked == true) Auto_Using_FLAG = "Y";  //오토쉽에 적용
                if (radioB_Auto_Using_FLAG_N.Checked == true) Auto_Using_FLAG = "N";  //오토쉽에는 미적용


                string Item_Prom_FLAG = ""; 
                if (radioB_SellTF1.Checked == true)
                {
                    if (radioB_Item_Prom_FLAG_1.Checked == true) Item_Prom_FLAG = "1";  // 1:1  증정 프로모션 
                    if (radioB_Item_Prom_FLAG_2.Checked == true) Item_Prom_FLAG = "2";  // 다: 1 증정 프로모션
                }


                string Order_Count_First_FLAG_Y = "";
                if (radioB_Order_Count_First_FLAG_Y.Checked == true) Order_Count_First_FLAG_Y = "Y";  //첫구매
                if (radioB_Order_Count_First_FLAG_N.Checked == true) Order_Count_First_FLAG_Y = "N";  //모든구매

                



                StrSql = "INSERT INTO tbl_Goods_Prom_Base ";
                StrSql = StrSql + " (";
                StrSql = StrSql + "  Pro_Code , Pro_Name , Start_Date , End_Date  ";
                StrSql = StrSql + " , Prom_FLAG, Mem_Reg_FLAG , BusCode_FLAG ";
                StrSql = StrSql + " , Auto_Using_FLAG , Using_FLAG , ETC_Sell, ETC_Memo  , Item_Prom_FLAG , Order_Count_First_FLAG     ";
                StrSql = StrSql + " , RecordID, RecordTime ";
                StrSql = StrSql + " ) ";
                StrSql = StrSql + " Values ";
                StrSql = StrSql + " (";
                StrSql = StrSql + "'" + txt_Pro_Code.Text  + "'";
                StrSql = StrSql + ",'" + txtName.Text + "'";
                StrSql = StrSql + ",'" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + ",'" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";

                StrSql = StrSql + ",'" + Prom_FLAG + "'";
                StrSql = StrSql + ",'" + Mem_Reg_FLAG + "'";
                
                if (radioB_Center.Checked == true )
                    StrSql = StrSql + ",'" + txtCenter_Code.Text + "'";  
                else
                    StrSql = StrSql + ",''";

                StrSql = StrSql + ",'" + Auto_Using_FLAG + "'";
                StrSql = StrSql + ",'" + Using_FLAG + "'";
                StrSql = StrSql + ",'" + txtRemark_Sell.Text + "'";
                StrSql = StrSql + ",'" + txtRemark.Text + "'";

                StrSql = StrSql + ",'" + Item_Prom_FLAG + "'";
                StrSql = StrSql + ",'" + Order_Count_First_FLAG_Y + "'";

                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + ")";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                StrSql = "Select Seq   ";                
                StrSql = StrSql + " From tbl_Goods_Prom_Base (nolock) ";
                StrSql = StrSql + " Where Pro_Code = '" + txt_Pro_Code.Text  + "'";                
           
                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt22 = Temp_Connect.DataSet_ReCount;

                int Seq = 0;
                if (ReCnt22 > 0)
                {
                    Seq = int.Parse(ds.Tables[base_db_name].Rows[0]["Seq"].ToString());
                }


                //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 
                if (radioB_SellTF1.Checked == true)  //상품기준 프로모션 
                {
                    for (int i = 0; i < dGridView_Good_Sell.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Sell.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Sell.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Sell.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Sell_ItemCode , Sell_ItemCount  ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ")";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }

                    string Give_ItemCnt_FALG = "";

                    if (radioB_Ba.Checked == true) Give_ItemCnt_FALG = "B";  //주문수량 대비 배수   
                    if (radioB_Dir.Checked == true) Give_ItemCnt_FALG = "D"; //증정품 입력된 수량으로 
                    

                    for (int i = 0; i < dGridView_Good_Prom.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Prom.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Prom.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Prom.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Give_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Give_ItemCode , Give_ItemCount , Give_ItemCnt_FALG   ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ",'" + Give_ItemCnt_FALG + "')";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }
                }//상품기준 프로모션 //상품기준 프로모션 
                //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 



                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                if (radioB_SellTF2.Checked == true || radioB_SellTF4.Checked == true)
                {

                    for (int i = 0; i < dGridView_Good_Sell_Pr.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Sell_Pr.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Sell_ItemCode , Sell_ItemCount  ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ")";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }

                    if (txt_Pv.Text == "") txt_Pv.Text = "0";
                    if (txt_Pv2.Text == "") txt_Pv2.Text = "0";

                    string Give_ItemCnt_FALG = "";

                    if (radioB_Ba_Pr.Checked == true) Give_ItemCnt_FALG = "B";  //기준금액 대비 배수
                    if (radioB_Dir_Pr.Checked == true) Give_ItemCnt_FALG = "D";  //증정품 입력된 수량으로
                    

                    for (int i = 0; i < dGridView_Good.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Give_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Give_ItemCode , Give_ItemCount , Give_ItemCnt_FALG   ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ",'" + Give_ItemCnt_FALG + "')";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }

                    //Sell_Price_1  에만 값이 있거나   Sell_Price_1 = Sell_Price_2  인경우에는 기준금액 이상으로 봄 
                    // Sell_Price_1  과 Sell_Price_2 에 값이 있고 Sell_Price_1 <>  Sell_Price_2 인 경우에는 사이의 매출액을 기준으로 해서 상품이 나감
                    StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Pr ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Seq , Sell_Price_1 , Sell_Price_2  ";
                    StrSql = StrSql + ") ";
                    StrSql = StrSql + " Values (" + Seq + ", " + txt_Pv.Text.Replace (",","") + "," + txt_Pv2.Text.Replace(",", "") + ")";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);

                }
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 



                if (radioB_SellTF3.Checked == true)
                {
                    if (txt_Pv3.Text == "") txt_Pv3.Text = "0";
                    if (txt_Pv4.Text == "") txt_Pv4.Text = "0";

                    for (int i = 0; i < dGridView_Good_Sell_2.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Sell_2.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Sell_2.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Sell_2.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Sell_ItemCode , Sell_ItemCount  ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ")";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }

                    string Give_ItemCnt_FALG = "";

                    if (radioB_Ba.Checked == true) Give_ItemCnt_FALG = "B";
                    if (radioB_Dir.Checked == true) Give_ItemCnt_FALG = "D";


                    for (int i = 0; i < dGridView_Good_Prom_2.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Prom_2.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Prom_2.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Prom_2.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Give_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Give_ItemCode , Give_ItemCount , Give_ItemCnt_FALG   ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ",'" + Give_ItemCnt_FALG + "')";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }


                    StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Pr ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Seq , Sell_Price_1 , Sell_Price_2  ";
                    StrSql = StrSql + ") ";
                    StrSql = StrSql + " Values (" + Seq + ", " + txt_Pv3.Text  + "," + txt_Pv4.Text + ")";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                    


                }



                tran.Commit();

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }



        private void Save_Base_Data_UpDate(ref int Save_Error_Check)
        {
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            //Temp_Connect.Connect_DB();
            //SqlConnection Conn = Temp_Connect.Conn_Conn();
            //SqlTransaction tran = Conn.BeginTransaction();


            //try
            //{

            //    string SortKind2 = "";

            //    if (radioB_SellTF1.Checked == true)
            //        SortKind2 = "1";
            //    else
            //        SortKind2 = "2";

            //    string StrSql = "";
            //    double app_Pv = double.Parse(txt_Pv.Text.Trim().Replace(",", ""));

            //    StrSql = "INSERT INTO tbl_Sham_Line_New_Mod ";
            //    StrSql = StrSql + " Select  * ";
            //    StrSql = StrSql + ",'" + cls_User.gid + "'";
            //    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
            //    StrSql = StrSql + " From tbl_Sham_Line_New ";
            //    StrSql = StrSql + " Where seq = " + txtKey.Text;

            //    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


            //    StrSql = "UpDate  tbl_Sham_Line_New Set ";
            //    StrSql = StrSql + "  Apply_Pv = " + app_Pv;
            //    StrSql = StrSql + ", Apply_Date = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
            //    StrSql = StrSql + ", SortKind2  = " + SortKind2;
            //    StrSql = StrSql + ", Etc = '" + txtRemark.Text.Trim() + "'";
            //    StrSql = StrSql + " Where seq = " + txtKey.Text;

            //    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


            //    tran.Commit();

            //    Save_Error_Check = 1;

            //}
            //catch (Exception)
            //{
            //    tran.Rollback();
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
            //}

            //finally
            //{
            //    tran.Dispose();
            //    Temp_Connect.Close_DB();
            //}


            string StrSql = "";
            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string Prom_FLAG = "", Mem_Reg_FLAG = "", Using_FLAG = "", Auto_Using_FLAG = ""; ;

                if (radioB_SellTF1.Checked == true) Prom_FLAG = "S";  // 상품기준
                if (radioB_SellTF2.Checked == true) Prom_FLAG = "P";  // 금액기준
                if (radioB_SellTF3.Checked == true) Prom_FLAG = "SP";
                if (radioB_SellTF4.Checked == true) Prom_FLAG = "V";  //PV기준
                if (radioB_SellTF5.Checked == true) Prom_FLAG = "SV";

                if (radioB_Mem_Reg_FLAG_C.Checked == true) Mem_Reg_FLAG = "C";  // 당월가입회원
                if (radioB_Mem_Reg_FLAG_T.Checked == true) Mem_Reg_FLAG = "T";  // 모든 회원 대상

                if (radioB_Using_FLAG_Y.Checked == true) Using_FLAG = "Y";  // 사용함
                if (radioB_Using_FLAG_N.Checked == true) Using_FLAG = "N";  // 사용안함

                if (radioB_Auto_Using_FLAG_Y.Checked == true) Auto_Using_FLAG = "Y";  //오토쉽에 적용
                if (radioB_Auto_Using_FLAG_N.Checked == true) Auto_Using_FLAG = "N";  //오토쉽에는 미적용


                string Item_Prom_FLAG = "";
                if (radioB_SellTF1.Checked == true)
                {
                    if (radioB_Item_Prom_FLAG_1.Checked == true) Item_Prom_FLAG = "1";  // 1:1  증정 프로모션 
                    if (radioB_Item_Prom_FLAG_2.Checked == true) Item_Prom_FLAG = "2";  // 다: 1 증정 프로모션
                }

                string Order_Count_First_FLAG_Y = "";
                if (radioB_Order_Count_First_FLAG_Y.Checked == true) Order_Count_First_FLAG_Y = "Y";  //첫구매
                if (radioB_Order_Count_First_FLAG_N.Checked == true) Order_Count_First_FLAG_Y = "N";  //모든구매




                StrSql = "INSERT INTO tbl_Goods_Prom_Base_Mod ";
                StrSql = StrSql + " Select  * ";
                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " From tbl_Goods_Prom_Base (nolock) ";
                StrSql = StrSql + " Where Pro_Code = '" + txt_Pro_Code.Text + "'";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                StrSql = "UpDate  tbl_Goods_Prom_Base Set ";
                StrSql = StrSql + "  Pro_Name = '" + txtName.Text + "'";
                StrSql = StrSql + ", Start_Date = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + ", End_Date = '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + ", Mem_Reg_FLAG  = '" + Mem_Reg_FLAG + "'";
                if (radioB_Center.Checked == true)
                    StrSql = StrSql + ", BusCode_FLAG  = '" + txtCenter_Code.Text + "'";
                else
                    StrSql = StrSql + ", BusCode_FLAG  = ''";
                StrSql = StrSql + ", Auto_Using_FLAG = '" + Auto_Using_FLAG + "'";
                StrSql = StrSql + ", Using_FLAG = '" + Using_FLAG + "'";
                StrSql = StrSql + ", ETC_Sell = '" + txtRemark_Sell.Text + "'";
                StrSql = StrSql + ", ETC_Memo = '" + txtRemark.Text + "'";
                StrSql = StrSql + ", Item_Prom_FLAG = '" + Item_Prom_FLAG  + "'";
                StrSql = StrSql + ", Order_Count_First_FLAG = '" + Order_Count_First_FLAG_Y + "'";
                
                StrSql = StrSql + " Where Pro_Code = '" + txt_Pro_Code.Text + "'";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);



                StrSql = "Select Seq   ";
                StrSql = StrSql + " From tbl_Goods_Prom_Base (nolock) ";
                StrSql = StrSql + " Where Pro_Code = '" + txt_Pro_Code.Text + "'";

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(StrSql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt22 = Temp_Connect.DataSet_ReCount;

                int Seq = 0;
                if (ReCnt22 > 0)
                {
                    Seq = int.Parse(ds.Tables[base_db_name].Rows[0]["Seq"].ToString());
                }


                //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 
                if (radioB_SellTF1.Checked == true)  //상품기준 프로모션 
                {
                    StrSql = "INSERT INTO tbl_Goods_Prom_Base_Sell_Item_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_Goods_Prom_Base_Sell_Item (nolock) ";
                    StrSql = StrSql + " Where Seq = " + Seq; 

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "Delete From tbl_Goods_Prom_Base_Sell_Item ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                    for (int i = 0; i < dGridView_Good_Sell.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Sell.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Sell.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Sell.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Sell_ItemCode , Sell_ItemCount  ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ")";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }


                    StrSql = "INSERT INTO tbl_Goods_Prom_Base_Give_Item_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_Goods_Prom_Base_Give_Item (nolock) ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "Delete From tbl_Goods_Prom_Base_Give_Item ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                    string Give_ItemCnt_FALG = "";

                    if (radioB_Ba.Checked == true) Give_ItemCnt_FALG = "B";  //주문수량 대비 배수   
                    if (radioB_Dir.Checked == true) Give_ItemCnt_FALG = "D"; //증정품 입력된 수량으로 


                    for (int i = 0; i < dGridView_Good_Prom.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Prom.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Prom.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Prom.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Give_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Give_ItemCode , Give_ItemCount , Give_ItemCnt_FALG   ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ",'" + Give_ItemCnt_FALG + "')";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }
                }//상품기준 프로모션 //상품기준 프로모션 
                //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 //상품기준 프로모션 



                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                if (radioB_SellTF2.Checked == true || radioB_SellTF4.Checked == true)
                {


                    StrSql = "INSERT INTO tbl_Goods_Prom_Base_Sell_Item_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_Goods_Prom_Base_Sell_Item (nolock) ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "Delete From tbl_Goods_Prom_Base_Sell_Item ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



                    for (int i = 0; i < dGridView_Good_Sell_Pr.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good_Sell_Pr.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Sell_ItemCode , Sell_ItemCount  ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ")";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }



                    StrSql = "INSERT INTO tbl_Goods_Prom_Base_Give_Item_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_Goods_Prom_Base_Give_Item (nolock) ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "Delete From tbl_Goods_Prom_Base_Give_Item ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    if (txt_Pv.Text == "") txt_Pv.Text = "0";
                    if (txt_Pv2.Text == "") txt_Pv2.Text = "0";

                    string Give_ItemCnt_FALG = "";

                    if (radioB_Ba_Pr.Checked == true) Give_ItemCnt_FALG = "B";  //기준금액 대비 배수
                    if (radioB_Dir_Pr.Checked == true) Give_ItemCnt_FALG = "D";  //증정품 입력된 수량으로


                    for (int i = 0; i < dGridView_Good.Rows.Count; i++)
                    {
                        if (int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString()) > 0)
                        {
                            int ItemCnt = int.Parse(dGridView_Good.Rows[i].Cells[0].Value.ToString());
                            string ItemCode = dGridView_Good.Rows[i].Cells[2].Value.ToString();

                            StrSql = "Insert into tbl_Goods_Prom_Base_Give_Item ";
                            StrSql = StrSql + " (";
                            StrSql = StrSql + " Seq , Give_ItemCode , Give_ItemCount , Give_ItemCnt_FALG   ";
                            StrSql = StrSql + ") ";
                            StrSql = StrSql + " Values (" + Seq + ", '" + ItemCode + "'," + ItemCnt + ",'" + Give_ItemCnt_FALG + "')";

                            Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);
                        }
                    }


                    StrSql = "INSERT INTO tbl_Goods_Prom_Base_Sell_Pr_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From tbl_Goods_Prom_Base_Sell_Pr (nolock) ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "Delete From tbl_Goods_Prom_Base_Sell_Pr ";
                    StrSql = StrSql + " Where Seq = " + Seq;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                    //Sell_Price_1  에만 값이 있거나   Sell_Price_1 = Sell_Price_2  인경우에는 기준금액 이상으로 봄 
                    // Sell_Price_1  과 Sell_Price_2 에 값이 있고 Sell_Price_1 <>  Sell_Price_2 인 경우에는 사이의 매출액을 기준으로 해서 상품이 나감
                    StrSql = "Insert into tbl_Goods_Prom_Base_Sell_Pr ";
                    StrSql = StrSql + " (";
                    StrSql = StrSql + " Seq , Sell_Price_1 , Sell_Price_2  ";
                    StrSql = StrSql + ") ";
                    StrSql = StrSql + " Values (" + Seq + ", " + txt_Pv.Text.Replace(",", "") + "," + txt_Pv2.Text.Replace(",", "") + ")";

                    Temp_Connect.Insert_Data(StrSql, "tbl_Goods_Prom_Base_Sell_Item", Conn, tran, this.Name, this.Text);

                }
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 
                // 금액기준 Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 // 금액기준Or PV 기준 


                


                tran.Commit();

                Save_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }






        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Delete_Base_Data(ref int Delete_Error_Check)
        {
            Delete_Error_Check = 0;
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (txtKey.Text.Trim() == "")
            {
                return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select Ordernumber  ";
            Tsql = Tsql + " From  tbl_SalesitemDetail  (nolock)  ";
            Tsql = Tsql + " Where Prom_TF_SORT =  '" + txt_Pro_Code.Text  + "'";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "tbl_Memberinfo", ds) == false) return ;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0)  //한건이라도 있으면 마감이 돌았음 그럼 안됨
            {
               
                MessageBox.Show("적용된 매출이 존재 합니다. 삭제가 불가능 합니다."
                                + "\n" +
                                cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));

                return;


            }
            
            //////마감정산이 이루어진 판매 날짜인지 체크한다.                
            //cls_Search_DB csd = new cls_Search_DB();
            //if (csd.Close_Check_SellDate("tbl_CloseTotal_02", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
            //{
            //    mtxtSellDate.Focus(); return;
            //}



            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                //tbl_Goods_Prom_Base
                //tbl_Goods_Prom_Base_Sell_Item
                //tbl_Goods_Prom_Base_Sell_Pr
                //tbl_Goods_Prom_Base_Give_Item


                string StrSql = "";

                StrSql = "Insert into  tbl_Goods_Prom_Base_Mod ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Prom_Base (nolock) ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                StrSql = "Insert into  tbl_Goods_Prom_Base_Sell_Item_Mod ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Prom_Base_Sell_Item (nolock) ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Insert into  tbl_Goods_Prom_Base_Sell_Pr_Mod ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Prom_Base_Sell_Pr (nolock) ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Insert into  tbl_Goods_Prom_Base_Give_Item_Mod ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From tbl_Goods_Prom_Base_Give_Item (nolock) ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From tbl_Goods_Prom_Base  ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                StrSql = "Delete From tbl_Goods_Prom_Base_Sell_Item  ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                StrSql = "Delete From tbl_Goods_Prom_Base_Sell_Pr  ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                StrSql = "Delete From tbl_Goods_Prom_Base_Give_Item  ";
                StrSql = StrSql + " Where seq = " + txtKey.Text;

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                tran.Commit();
                Delete_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));

            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();
            }

        }






        private void dGridView_Base_Sub_DoubleClick(object sender, EventArgs e)
        {

            //cls_form_Meth ct = new cls_form_Meth();
            //ct.from_control_clear(groupBox1, mtxtSellDate);

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, txt_Pro_Code);

            Base_Grid_Set_Good_Sell_Pr(); //상품정보를 불러온다.

            Base_Grid_Set_Good(); //상품정보를 불러온다.

            Base_Grid_Set_Good_Sell(); //상품정보를 불러온다.

            Base_Grid_Set_Good_Prom(); //상품정보를 불러온다.

            radioB_SellTF1.Checked = true;
            groupBox2.Visible = true;
            groupBox4.Visible = false;
            groupBox8.Visible = false;

            radioB_Mem_Reg_FLAG_C.Checked = true;
            radioB_Center.Checked = true;
            radioB_Ba.Checked = true;
            radioB_Ba_Pr.Checked = true;

            radioB_Using_FLAG_Y.Checked = true;
            radioB_Auto_Using_FLAG_Y.Checked = true;

            radioB_Ba2.Checked = true;
            radioB_Over2.Checked = true;
            radioB_Over.Checked = true;
            radioB_Item_Prom_FLAG_1.Checked = true;
            radioB_Order_Count_First_FLAG_Y.Checked = true;


            panel_Pro_Code.Enabled = true;
            panel_SellTF.Enabled = true;


            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                
                Data_Set_Form_TF = 1;



                //StrSql = "INSERT INTO tbl_Goods_Prom_Base ";
                //StrSql = StrSql + " (";
                //StrSql = StrSql + "  Pro_Code , Pro_Name , Start_Date , End_Date  ";
                //StrSql = StrSql + " , Prom_FLAG, Mem_Reg_FLAG , BusCode_FLAG ";
                //StrSql = StrSql + " , Auto_Using_FLAG , Using_FLAG , ETC_Sell, ETC_Memo  , Item_Prom_FLAG    ";
                //StrSql = StrSql + " , RecordID, RecordTime ";
                //StrSql = StrSql + " ) ";
                //StrSql = StrSql + " Values ";
                //StrSql = StrSql + " (";
                //StrSql = StrSql + "'" + txt_Pro_Code.Text + "'";
                //StrSql = StrSql + "'" + txtName.Text + "'";
                //StrSql = StrSql + ",'" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                //StrSql = StrSql + ",'" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";

                //StrSql = StrSql + "'" + Prom_FLAG + "'";
                //StrSql = StrSql + "'" + Mem_Reg_FLAG + "'";

                //if (radioB_Center.Checked == true)
                //    StrSql = StrSql + "'" + txtCenter_Code.Text + "'";
                //else
                //    StrSql = StrSql + "''";

                //StrSql = StrSql + "'" + Auto_Using_FLAG + "'";
                //StrSql = StrSql + "'" + Using_FLAG + "'";
                //StrSql = StrSql + "'" + txtRemark_Sell.Text + "'";
                //StrSql = StrSql + "'" + txtRemark.Text + "'";

                //StrSql = StrSql + "'" + Item_Prom_FLAG + "'";

                //StrSql = StrSql + ",'" + cls_User.gid + "'";
                //StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                //StrSql = StrSql + ")";

                //string[] g_HeaderText = {"_Seq"  , "등록번호"   , "명칭"  , "적용시작일"   , "적용종료일"
                //                    , "구분"   , "회원적용"  ,"적용센타"  , "오토쉽적용"   , "사용여부"
                //                     , "주문내역비고" ,"비고"    ,"기록자"     ,"기록일" ,"_Prom_FLAG"
                //                     , "_Mem_Reg_FLAG" , "_Auto_Using_FLAG" , "_Using_FLAG" , "_Item_Prom_FLAG" , "_BusCode_FLAG"
                //                        };



                string Seq = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();
                string Pro_Code = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString();
                string Pro_Name = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();

                string Start_Date = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString().Replace("-", "");
                string End_Date = (sender as DataGridView).CurrentRow.Cells[4].Value.ToString().Replace("-", "");

                string Prom_FLAG = (sender as DataGridView).CurrentRow.Cells[14].Value.ToString();

                string Mem_Reg_FLAG = (sender as DataGridView).CurrentRow.Cells[15].Value.ToString();
                string Auto_Using_FLAG = (sender as DataGridView).CurrentRow.Cells[16].Value.ToString();
                string Using_FLAG = (sender as DataGridView).CurrentRow.Cells[17].Value.ToString();
                string Item_Prom_FLAG = (sender as DataGridView).CurrentRow.Cells[18].Value.ToString();
                string BusCode_FLAG = (sender as DataGridView).CurrentRow.Cells[19].Value.ToString();
                string BusCode_FLAG_Name = (sender as DataGridView).CurrentRow.Cells[7].Value.ToString();
                string Order_Count_First_FLAG_Y = (sender as DataGridView).CurrentRow.Cells[20].Value.ToString();

                txtRemark_Sell.Text = (sender as DataGridView).CurrentRow.Cells[10].Value.ToString();
                txtRemark.Text = (sender as DataGridView).CurrentRow.Cells[11].Value.ToString();
                
                txtKey.Text = Seq.ToString();

                txt_Pro_Code.Text = Pro_Code;
                txtName.Text = Pro_Name;

                mtxtSellDate.Text = Start_Date;
                mtxtSellDate4.Text = End_Date;

                radioB_SellTF1.Checked = false;
                radioB_SellTF2.Checked = false;
                radioB_SellTF3.Checked = false;
                radioB_SellTF4.Checked = false;
                radioB_SellTF5.Checked = false;
                if (Prom_FLAG == "S") radioB_SellTF1.Checked = true ;  // 상품기준
                if (Prom_FLAG == "P") radioB_SellTF2.Checked = true;  // 금액기준
                if (Prom_FLAG == "SP") radioB_SellTF3.Checked = true;  // 상품금액기준
                if (Prom_FLAG == "V") radioB_SellTF4.Checked = true;  // Pv기준
                if (Prom_FLAG == "SV") radioB_SellTF5.Checked = true;  // 상품PV기준

                radioB_Center.Checked = false;
                radioB_Center_Not.Checked = false; 
                if (BusCode_FLAG != "")
                {
                    txtCenter_Code.Text = BusCode_FLAG;
                    txtCenter.Text = BusCode_FLAG_Name;
                    radioB_Center.Checked = true ;
                }
                else
                {
                    txtCenter_Code.Text = "";
                    txtCenter.Text = "";
                    radioB_Center_Not.Checked = true;
                }

                radioB_Mem_Reg_FLAG_C.Checked = false;
                radioB_Mem_Reg_FLAG_T.Checked = false;
                if (Mem_Reg_FLAG == "C") radioB_Mem_Reg_FLAG_C.Checked = true;  // 당월가입회원
                if (Mem_Reg_FLAG == "T") radioB_Mem_Reg_FLAG_T.Checked = true;  // 모든 회원 대상

                radioB_Using_FLAG_Y.Checked = false;
                radioB_Using_FLAG_N.Checked = false;
                if (Using_FLAG == "Y")
                    radioB_Using_FLAG_Y.Checked = true;  // 사용함
                else
                    radioB_Using_FLAG_N.Checked = true;  // 사용안함

                radioB_Auto_Using_FLAG_Y.Checked = false;
                radioB_Auto_Using_FLAG_N.Checked = false;
                if (Auto_Using_FLAG == "Y")
                    radioB_Auto_Using_FLAG_Y.Checked = true;  // 오토쉽에 적용
                else
                    radioB_Auto_Using_FLAG_N.Checked = true;  // 오토쉽에는 미적용


                radioB_Order_Count_First_FLAG_Y.Checked = false;
                radioB_Order_Count_First_FLAG_N.Checked = false;
                if (Order_Count_First_FLAG_Y == "Y")
                    radioB_Order_Count_First_FLAG_Y.Checked = true;  // 첫구매적용여부
                else
                    radioB_Order_Count_First_FLAG_N.Checked = true;  // 첫구매적용여부



                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                if (radioB_SellTF1.Checked == true)
                 {
                    radioB_Item_Prom_FLAG_1.Checked = false;
                    radioB_Item_Prom_FLAG_2.Checked = false;

                    if (Item_Prom_FLAG == "1") radioB_Item_Prom_FLAG_1.Checked = true;  // 1:1  증정 프로모션 
                    if (Item_Prom_FLAG == "2") radioB_Item_Prom_FLAG_2.Checked = true;  // 다: 1 증정 프로모션 적용

                    groupBox2.Visible = true;
                    groupBox4.Visible = false;
                    groupBox8.Visible = false;

                    //tbl_Goods_Prom_Base_Sell_Item
                    //if (radioB_Ba.Checked == true) Give_ItemCnt_FALG = "B";  //주문수량 대비 배수   
                    // if (radioB_Dir.Checked == true) Give_ItemCnt_FALG = "D"; //증정품 입력된 수량으로 
                    //tbl_Goods_Prom_Base_Give_Item

                    string Tsql = "Select  ";
                    Tsql = Tsql + "  Sell_ItemCode  ItemCode ";
                    Tsql = Tsql + " ,Sell_ItemCount ItemCount ";
                    Tsql = Tsql + " From tbl_Goods_Prom_Base_Sell_Item (nolock) ";
                    Tsql = Tsql + " Where Seq = " + Seq ;
                    Tsql = Tsql + " Order BY Sell_ItemCode  ";
                    
                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds2, this.Name, this.Text);
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt2 > 0)
                    {                        
                        for (int fi_cnt = 0; fi_cnt <= ReCnt2 - 1; fi_cnt++)
                        {
                            int ItemCnt = int.Parse(ds2.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                            string ItemCode = ds2.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString();

                            for (int i = 0; i <= dGridView_Good_Sell.Rows.Count - 1; i++)
                            {
                                //빈칸으로 들어간 내역을 0으로 바꾼다
                                if (dGridView_Good_Sell.Rows[i].Cells[2].Value.ToString() == ItemCode)
                                {
                                    dGridView_Good_Sell.Rows[i].Cells[0].Value = ItemCnt.ToString();                                    
                                }
                            }
                        }
                    }

                    string Give_ItemCnt_FALG = "";
                    radioB_Ba.Checked = false;
                    radioB_Dir.Checked = false;

                    Tsql = "Select  ";
                    Tsql = Tsql + "  Give_ItemCode  ItemCode ";
                    Tsql = Tsql + " ,Give_ItemCount ItemCount ";
                    Tsql = Tsql + " ,Give_ItemCnt_FALG "; 
                    Tsql = Tsql + " From tbl_Goods_Prom_Base_Give_Item (nolock) ";
                    Tsql = Tsql + " Where Seq = " + Seq;
                    Tsql = Tsql + " Order BY Give_ItemCode  ";

                    DataSet ds3 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds3, this.Name, this.Text);
                    int ReCnt3 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                    {
                        for (int fi_cnt = 0; fi_cnt <= ReCnt3 - 1; fi_cnt++)
                        {
                            int ItemCnt = int.Parse(ds3.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                            string ItemCode = ds3.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString();
                            Give_ItemCnt_FALG = ds3.Tables[base_db_name].Rows[fi_cnt]["Give_ItemCnt_FALG"].ToString(); 

                            for (int i = 0; i <= dGridView_Good_Prom.Rows.Count - 1; i++)
                            {
                                //빈칸으로 들어간 내역을 0으로 바꾼다
                                if (dGridView_Good_Prom.Rows[i].Cells[2].Value.ToString() == ItemCode)
                                {
                                    dGridView_Good_Prom.Rows[i].Cells[0].Value = ItemCnt.ToString ();
                                }
                            }
                        }
                        if (Give_ItemCnt_FALG == "B") radioB_Ba.Checked = true;
                        if (Give_ItemCnt_FALG == "D") radioB_Dir.Checked = true;
                    }

                }


                if (radioB_SellTF2.Checked == true || radioB_SellTF4.Checked == true)
                {

                    if (radioB_SellTF4.Checked == true)
                    {
                        radioB_Over.Text = "기준PV이상";
                        radioB_Inner.Text = "기준PV이내";
                        label16.Text = "기준PV";
                        label9.Text = "기준PV";
                        radioB_Ba_Pr.Text = "기준PV 대비 배수";
                        radioB_Dir_Pr.Text = "증정품 입력된 수량으로";
                        groupBox4.Text = "PV기준";
                    }
                    else
                    {
                        radioB_Over.Text = "기준금액이상";
                        radioB_Inner.Text = "기준금액이내";
                        label16.Text = "기준금액";
                        label9.Text = "기준금액";
                        radioB_Ba_Pr.Text = "기준금액 대비 배수";
                        radioB_Dir_Pr.Text = "증정품 입력된 수량으로";
                        groupBox4.Text = "금액기준";
                    }
                    groupBox2.Visible = false;
                    groupBox4.Visible = true;
                    groupBox8.Visible = false;

                    //if (radioB_Ba_Pr.Checked == true) Give_ItemCnt_FALG = "B";  //기준금액 대비 배수
                    //if (radioB_Dir_Pr.Checked == true) Give_ItemCnt_FALG = "D";  //증정품 입력된 수량으로
                    //tbl_Goods_Prom_Base_Give_Item
                    //tbl_Goods_Prom_Base_Sell_Pr

                    string Tsql = "Select  ";
                    Tsql = Tsql + "  Sell_ItemCode  ItemCode ";
                    Tsql = Tsql + " ,Sell_ItemCount ItemCount ";
                    Tsql = Tsql + " From tbl_Goods_Prom_Base_Sell_Item (nolock) ";
                    Tsql = Tsql + " Where Seq = " + Seq;
                    Tsql = Tsql + " Order BY Sell_ItemCode  ";

                    DataSet ds33 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds33, this.Name, this.Text);
                    int ReCnt33 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt33 > 0)
                    {
                        for (int fi_cnt = 0; fi_cnt <= ReCnt33 - 1; fi_cnt++)
                        {
                            int ItemCnt = int.Parse(ds33.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                            string ItemCode = ds33.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString();

                            for (int i = 0; i <= dGridView_Good_Sell_Pr.Rows.Count - 1; i++)
                            {
                                //빈칸으로 들어간 내역을 0으로 바꾼다
                                if (dGridView_Good_Sell_Pr.Rows[i].Cells[2].Value.ToString() == ItemCode)
                                {
                                    dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value = ItemCnt.ToString();
                                }
                            }
                        }
                    }




                    string Give_ItemCnt_FALG = "";
                    radioB_Ba_Pr.Checked = false;
                    radioB_Dir_Pr.Checked = false;

                    Tsql = "Select  ";
                    Tsql = Tsql + "  Give_ItemCode  ItemCode ";
                    Tsql = Tsql + " ,Give_ItemCount ItemCount ";
                    Tsql = Tsql + " ,Give_ItemCnt_FALG ";
                    Tsql = Tsql + " From tbl_Goods_Prom_Base_Give_Item (nolock) ";
                    Tsql = Tsql + " Where Seq = " + Seq;
                    Tsql = Tsql + " Order BY Give_ItemCode  ";

                    DataSet ds2 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds2, this.Name, this.Text);
                    int ReCnt2 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt2 > 0)
                    {
                        for (int fi_cnt = 0; fi_cnt <= ReCnt2 - 1; fi_cnt++)
                        {
                            int ItemCnt = int.Parse(ds2.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                            string ItemCode = ds2.Tables[base_db_name].Rows[fi_cnt]["ItemCode"].ToString();
                            Give_ItemCnt_FALG = ds2.Tables[base_db_name].Rows[fi_cnt]["Give_ItemCnt_FALG"].ToString();

                            for (int i = 0; i <= dGridView_Good.Rows.Count - 1; i++)
                            {
                                //빈칸으로 들어간 내역을 0으로 바꾼다
                                if (dGridView_Good.Rows[i].Cells[2].Value.ToString() == ItemCode)
                                {
                                    dGridView_Good.Rows[i].Cells[0].Value = ItemCnt.ToString();
                                }
                            }
                        }

                        if (Give_ItemCnt_FALG == "B") radioB_Ba_Pr.Checked = true;
                        if (Give_ItemCnt_FALG == "D") radioB_Dir_Pr.Checked = true;
                    }

                    Tsql = "Select  ";
                    Tsql = Tsql + "  Sell_Price_1";
                    Tsql = Tsql + " ,Sell_Price_2 ";                    
                    Tsql = Tsql + " From tbl_Goods_Prom_Base_Sell_Pr (nolock) ";
                    Tsql = Tsql + " Where Seq = " + Seq;                    

                    DataSet ds3 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds3, this.Name, this.Text);
                    int ReCnt3 = Temp_Connect.DataSet_ReCount;

                    if (ReCnt3 > 0)
                    {
                        
                        int Sell_Price_1 = int.Parse(ds3.Tables[base_db_name].Rows[0]["Sell_Price_1"].ToString());
                        int Sell_Price_2 = int.Parse(ds3.Tables[base_db_name].Rows[0]["Sell_Price_2"].ToString());

                        txt_Pv.Text = Sell_Price_1.ToString();
                        txt_Pv2.Text = Sell_Price_2.ToString();

                        radioB_Over.Checked = false;
                        radioB_Inner.Checked = false;

                        if (Sell_Price_1 > 0 && (Sell_Price_2 == 0 || Sell_Price_1 == Sell_Price_2))
                        {
                            radioB_Over.Checked = true;
                        }
                        else
                            radioB_Inner.Checked = true;

                    }

                    
                }


                if (radioB_SellTF3.Checked == true || radioB_SellTF5.Checked == true)
                {
                    groupBox2.Visible = false;
                    groupBox4.Visible = false;
                    groupBox8.Visible = true;
                }



                panel_Pro_Code.Enabled = false ;
                panel_SellTF.Enabled = false;

                Data_Set_Form_TF = 0;
                mtxtSellDate.Focus();


            }
        }





        private Boolean Input_Error_Check(MaskedTextBox m_tb, int s_Kind)
        {
            string T_Mbid = m_tb.Text;
            string Mbid = ""; int Mbid2 = 0;

            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == -1) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name ";
            Tsql = Tsql + " , LineCnt , N_LineCnt  ";
            Tsql = Tsql + " , LeaveDate , LineUserDate  ";
            Tsql = Tsql + " , Saveid  , Saveid2  ";
            Tsql = Tsql + " , Nominid , Nominid2  ";
            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
            Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode + "') )";
            Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return false;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)  //실제로 존재하는 회원 번호 인가.
            {

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }
            //++++++++++++++++++++++++++++++++   


            return true;
        }







        private void Set_Form_Date(string T_Mbid)
        {
            string Mbid = ""; int Mbid2 = 0;
            Data_Set_Form_TF = 1;

            cls_Search_DB csb = new cls_Search_DB();
            if (csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2) == 1)
            {
                string Tsql = "";
                Tsql = "Select  ";
                if (cls_app_static_var.Member_Number_1 > 0)
                    Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) AS M_Mbid ";
                else
                    Tsql = Tsql + " tbl_Memberinfo.mbid2 AS M_Mbid ";

                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

                Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

                Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)  AS RegTime  ";

                Tsql = Tsql + "  , Add_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.hometel ";
                Tsql = Tsql + " , tbl_Memberinfo.address1 ";
                Tsql = Tsql + " , tbl_Memberinfo.address2 ";
                Tsql = Tsql + " , tbl_Memberinfo.Addcode1 ";

                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }
                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++

                mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["M_Mbid"].ToString();
                txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();

            }

            Data_Set_Form_TF = 0;



        }

        private void radioB_R_Base_Click(object sender, EventArgs e)
        {
            //RadioButton _Rb = (RadioButton)sender;
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtSellDate2, mtxtSellDate3, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }

        private void radioB_S_Base_Click(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 1;
            cls_form_Meth ct = new cls_form_Meth();
            ct.Search_Date_TextBox_Put(mtxtMakeDate1, mtxtMakeDate2, (RadioButton)sender);
            Data_Set_Form_TF = 0;
        }




        private void S_MtxtData_KeyPress(object sender, KeyPressEventArgs e)
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

        private void S_MtxtMbid_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            MaskedTextBox tb = (MaskedTextBox)sender;
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
            }
        }







        private void Base_Grid_Set_Good()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";

            
            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";
            
            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }

        private void Base_Grid_Set_Good_Sell_Pr()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Sell_Pr_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Sell_Pr.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good_Sell_Pr.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";


            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";

            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Sell_Pr.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Sell_Pr.db_grid_Obj_Data_Put();
        }



        private void Base_Grid_Set_Good_Sell()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Sell_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Sell.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";


            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";

            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Sell.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Sell.db_grid_Obj_Data_Put();
        }


        private void Base_Grid_Set_Good_Sell_2()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Sell_2_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Sell_2.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";


            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";

            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Sell_2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Sell_2.db_grid_Obj_Data_Put();
        }


        private void Base_Grid_Set_Good_Prom()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Prom_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Prom.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";


            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";

            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Prom.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Prom.db_grid_Obj_Data_Put();
        }

        private void Base_Grid_Set_Good_Prom_2()
        {
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Good_Prom_2_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb_Prom_2.d_Grid_view_Header_Reset();

            ((DataGridViewTextBoxColumn)dGridView_Good.Columns[0]).MaxInputLength = 6;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            string Tsql = "";


            Tsql = "Select '0', Name , NCode ,price4 ,price2    ";
            Tsql = Tsql + " , Up_itemCode, '' ,'' ,'' ,'' ";
            Tsql = Tsql + " From ufn_Good_Search_01 ('" + cls_User.gid_date_time + "') ";

            Tsql = Tsql + " Order by Ncode ";

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
                Set_gr_dic_Good(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb_Prom_2.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb_Prom_2.db_grid_Obj_Data_Put();
        }


        private void Set_gr_dic_Good(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            int Col_Cnt = 0;
            object[] row0 = new object[cgb.grid_col_Count];

            while (Col_Cnt < cgb.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }



        private void dGridView_Good_Base_Header_Reset()
        {
            cgb.grid_col_Count = 10;
            cgb.basegrid = dGridView_Good;
            cgb.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb.grid_Frozen_End_Count = 2;
          //  cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"증정수량"  , "증정상품명"   , "증정상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 250, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;

            cgb.basegrid.RowHeadersVisible = false;
        }


        private void dGridView_Good_Sell_Base_Header_Reset()
        {
            cgb_Sell.grid_col_Count = 10;
            cgb_Sell.basegrid = dGridView_Good_Sell;
            cgb_Sell.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Sell.grid_Frozen_End_Count = 2;
           // cgb_Sell.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"구매수량"  , "구매상품명"   , "구매상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb_Sell.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 250, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb_Sell.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Sell.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb_Sell.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb_Sell.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Sell.grid_cell_format = gr_dic_cell_format;

            cgb_Sell.basegrid.RowHeadersVisible = false;
        }


        private void dGridView_Good_Sell_Pr_Base_Header_Reset()
        {
            cgb_Sell_Pr.grid_col_Count = 10;
            cgb_Sell_Pr.basegrid = dGridView_Good_Sell_Pr;
            cgb_Sell_Pr.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Sell_Pr.grid_Frozen_End_Count = 2;
            // cgb_Sell_Pr.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"구매수량"  , "구매상품명"   , "구매상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb_Sell_Pr.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 250, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb_Sell_Pr.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Sell_Pr.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb_Sell_Pr.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb_Sell_Pr.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Sell_Pr.grid_cell_format = gr_dic_cell_format;

            cgb_Sell_Pr.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Good_Prom_Base_Header_Reset()
        {
            cgb_Prom.grid_col_Count = 10;
            cgb_Prom.basegrid = dGridView_Good_Prom;
            cgb_Prom.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Prom.grid_Frozen_End_Count = 2;
          //  cgb_Prom.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"증정수량"  , "증정상품명"   , "증정상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb_Prom.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 70, 250, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb_Prom.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Prom.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb_Prom.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb_Prom.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Prom.grid_cell_format = gr_dic_cell_format;

            cgb_Prom.basegrid.RowHeadersVisible = false;
        }



        private void dGridView_Good_Sell_2_Base_Header_Reset()
        {
            cgb_Sell_2.grid_col_Count = 10;
            cgb_Sell_2.basegrid = dGridView_Good_Sell_2;
            cgb_Sell_2.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Sell_2.grid_Frozen_End_Count = 2;
            cgb_Sell_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"구매수량"  , "증정상품명"   , "증정상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb_Sell_2.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 130, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb_Sell_2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Sell_2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb_Sell_2.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb_Sell_2.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Sell_2.grid_cell_format = gr_dic_cell_format;

            cgb_Sell_2.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Good_Prom_2_Base_Header_Reset()
        {
            cgb_Prom_2.grid_col_Count = 10;
            cgb_Prom_2.basegrid = dGridView_Good_Prom_2;
            cgb_Prom_2.grid_select_mod = DataGridViewSelectionMode.CellSelect;
            cgb_Prom_2.grid_Frozen_End_Count = 2;
            cgb_Prom_2.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string[] g_HeaderText = {"증정수량"  , "증정상품명"   , "증정상품코드"  , ""   , ""
                                , ""   , ""    , ""   , ""    , ""
                                    };
            cgb_Prom_2.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 60, 130, 100, 0, 0
                             ,0 , 0 ,  0 , 0 ,  0
                            };
            cgb_Prom_2.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { false , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                   };
            cgb_Prom_2.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleRight  //5
                               
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter //10                                                           
                              };
            cgb_Prom_2.grid_col_alignment = g_Alignment;


            DataGridViewColumnSortMode[] g_SortM =
                              {DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic  //5
                               
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic
                               ,DataGridViewColumnSortMode.Automatic //10                                                           
                              };
            cgb_Prom_2.grid_col_SortMode = g_SortM;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb_Prom_2.grid_cell_format = gr_dic_cell_format;

            cgb_Prom_2.basegrid.RowHeadersVisible = false;
        }




        private void dGridView_Good_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);


        }

        private void textBoxPart_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }


        }

        private void dGridView_Good_Sell_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good_Sell.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }

        private void dGridView_Good_Sell_Pr_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //dGridView_Good_Sell_Pr.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }


        private void dGridView_Good_Prom_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good_Prom.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }


        private void dGridView_Good_Sell_2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good_Sell_2.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }

        private void dGridView_Good_Prom_2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dGridView_Good_Prom_2.EditingControl.KeyPress += new KeyPressEventHandler(textBoxPart_TextChanged);
        }

        private void radioB_SellTF1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioB_SellTF1.Checked == true)
            {
                groupBox2.Visible = true;
                groupBox4.Visible = false;
                groupBox8.Visible = false ;
            }

            if (radioB_SellTF2.Checked == true || radioB_SellTF4.Checked == true)
            {
                if (radioB_SellTF4.Checked == true)
                {
                    radioB_Over.Text = "기준PV이상";
                    radioB_Inner.Text = "기준PV이내";
                    label16.Text = "기준PV";
                    label9.Text = "기준PV";
                    radioB_Ba_Pr.Text = "기준PV 대비 배수";
                    radioB_Dir_Pr.Text = "증정품 입력된 수량으로";
                    groupBox4.Text = "PV기준";
                }
                else
                {
                    radioB_Over.Text = "기준금액이상";
                    radioB_Inner.Text = "기준금액이내";
                    label16.Text = "기준금액";
                    label9.Text = "기준금액";
                    radioB_Ba_Pr.Text = "기준금액 대비 배수";
                    radioB_Dir_Pr.Text = "증정품 입력된 수량으로";
                    groupBox4.Text = "금액기준";
                }
                groupBox2.Visible = false;
                groupBox4.Visible = true;
                groupBox8.Visible = false;
            }

            if (radioB_SellTF3.Checked == true || radioB_SellTF5.Checked == true)
            {
                groupBox2.Visible = false;
                groupBox4.Visible = false;
                groupBox8.Visible = true;
            }
        }





        private  void Put_Good_Good_Sort_ComboBox(ComboBox cb_1, ComboBox cb_1_Code, string Sort, string Big_FLAG = "")
        {

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;
            Tsql = "Select ItemCode ,ItemName  ";
            if (Sort == "1") Tsql = Tsql + " From tbl_MakeItemCode1  (nolock)  ";
            if (Sort == "2")
            {
                Tsql = Tsql + " From tbl_MakeItemCode2  (nolock)  ";
                if (Big_FLAG != "")
                {
                    Tsql = Tsql + " Where UpitemCode ='" + Big_FLAG + "'";
                }
            }
            Tsql = Tsql + " Order by ItemCode ASC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            Temp_Connect.Open_Data_Set(Tsql, "tbl_Class", ds);
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt <= 0)
                return;

            cb_1.Items.Clear();
            cb_1_Code.Items.Clear();

            cb_1.Items.Add("");
            cb_1_Code.Items.Add("");

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                cb_1.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["ItemName"].ToString());
                cb_1_Code.Items.Add(ds.Tables["tbl_Class"].Rows[fi_cnt]["ItemCode"].ToString());
            }

            cb_1.SelectedIndex = -1;
            cb_1_Code.SelectedIndex = cb_1.SelectedIndex;
            //++++++++++++++++++++++++++++++++
        }


        private bool Item_Rece_Error_Check__01()
        {

            //상품은 선택 안햇네 그럼 그것도 넣어라.
            if (txt_ItemName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCode.Focus(); return false;
            }


            //구매수량을 입력 안햇네 그럼 그것도 넣어라.
            if (txt_ItemCount.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCount.Focus(); return false;
            }


            //구매수량을 0  입력햇네  그럼 제대로 넣어라.
            if (int.Parse(txt_ItemCount.Text.Trim()) == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCount.Focus(); return false;
            }

            return true;

        }

        


        private void button_Add_Down_Click(object sender, EventArgs e)
        {
            if (Item_Rece_Error_Check__01() == false) return;

            for (int i = 0; i <= dGridView_Good_Prom.Rows.Count - 1; i++)
            {
                if (dGridView_Good_Prom.Rows[i].Cells[2].Value.ToString() == txt_ItemCode.Text.Trim())
                {
                    //dGridView_Good_Prom.Rows[i].Cells[0].Value = "V";
                    dGridView_Good_Prom.Rows[i].Cells[0].Value = txt_ItemCount.Text.Trim();
                    txt_ItemCode.Text = "";
                    txt_ItemName.Text = "";
                    txt_ItemCount.Text = "";
                    break;
                }
            }
        }


        private bool Item_Rece_Error_Check_Up__01()
        {

            //상품은 선택 안햇네 그럼 그것도 넣어라.
            if (txt_ItemNameUp.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemNameUp.Focus(); return false;
            }


            //구매수량을 입력 안햇네 그럼 그것도 넣어라.
            if (txt_ItemCountUp.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCountUp.Focus(); return false;
            }


            //구매수량을 0  입력햇네  그럼 제대로 넣어라.
            if (int.Parse(txt_ItemCountUp.Text.Trim()) == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCountUp.Focus(); return false;
            }

            return true;

        }
        private void button_Add_Up_Click(object sender, EventArgs e)
        {
            if (Item_Rece_Error_Check_Up__01() == false) return;

            for (int i = 0; i <= dGridView_Good_Sell.Rows.Count - 1; i++)
            {
                if (dGridView_Good_Sell.Rows[i].Cells[2].Value.ToString() == txt_ItemCodeUp.Text.Trim())
                {
                    //dGridView_Good_Sell.Rows[i].Cells[0].Value = "V";
                    dGridView_Good_Sell.Rows[i].Cells[0].Value = txt_ItemCountUp.Text.Trim();
                    txt_ItemCodeUp.Text = "";
                    txt_ItemNameUp.Text = "";
                    txt_ItemCountUp.Text = "";
                    break; 
                }
            }


        }

        private void combo_CGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            combo_CGrade_Code.SelectedIndex = combo_CGrade.SelectedIndex;
            Put_Good_Good_Sort_ComboBox(combo_C2Grade, combo_C2Grade_Code, "2", combo_CGrade_Code.Text );
        }

        private void button_Sort_Click(object sender, EventArgs e)
        {
            combo_CGrade_Code.SelectedIndex = combo_CGrade.SelectedIndex;
            combo_C2Grade_Code.SelectedIndex = combo_C2Grade.SelectedIndex;

            if (combo_CGrade_Code.Text != "" || combo_C2Grade_Code.Text != "")
            {
                string UpCode = combo_CGrade_Code.Text + combo_C2Grade_Code.Text;

                

                for (int i = 0; i <= dGridView_Good_Sell.Rows.Count - 1; i++)
                {
                    if (dGridView_Good_Sell.Rows[i].Cells[5].Value.ToString().Substring (0 , UpCode.Length ) == UpCode)
                    {
                        //dGridView_Good_Sell.Rows[i].Cells[0].Value = "V";
                        dGridView_Good_Sell.Rows[i].Cells[0].Value = "1" ;
                    }
                }
            }
        }


        private bool Item_Rece_Error_Check__01_Pr()
        {

            //상품은 선택 안햇네 그럼 그것도 넣어라.
            if (txt_ItemNamePr.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCodePr.Focus(); return false;
            }


            //구매수량을 입력 안햇네 그럼 그것도 넣어라.
            if (txt_ItemCountPr.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCountPr.Focus(); return false;
            }


            //구매수량을 0  입력햇네  그럼 제대로 넣어라.
            if (int.Parse(txt_ItemCountPr.Text.Trim()) == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemCountPr.Focus(); return false;
            }

            return true;

        }


        private void button_Add_Down_Pr_Click(object sender, EventArgs e)
        {
            if (Item_Rece_Error_Check__01_Pr() == false) return;

            for (int i = 0; i <= dGridView_Good.Rows.Count - 1; i++)
            {
                if (dGridView_Good.Rows[i].Cells[2].Value.ToString() == txt_ItemCodePr.Text.Trim())
                {                    
                    dGridView_Good.Rows[i].Cells[0].Value = txt_ItemCountPr.Text.Trim();
                    txt_ItemCodePr.Text = "";
                    txt_ItemNamePr.Text = "";
                    txt_ItemCountPr.Text = "";
                    break;
                }
            }
        }

        private void button_SortPr_Click(object sender, EventArgs e)
        {
            combo_CGradePr_Code.SelectedIndex = combo_CGradePr.SelectedIndex;
            combo_C2GradePr_Code.SelectedIndex = combo_C2GradePr.SelectedIndex;

            if (combo_CGradePr_Code.Text != "" || combo_C2GradePr_Code.Text != "")
            {
                string UpCode = combo_CGradePr_Code.Text + combo_C2GradePr_Code.Text;



                for (int i = 0; i <= dGridView_Good_Sell_Pr.Rows.Count - 1; i++)
                {
                    if (dGridView_Good_Sell_Pr.Rows[i].Cells[5].Value.ToString().Substring(0, UpCode.Length) == UpCode)
                    {
                        //dGridView_Good_Sell.Rows[i].Cells[0].Value = "V";
                        dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value = "1";
                    }
                }
            }
        }


        private bool Item_Rece_Error_Check_Up_Pr__01()
        {

            //상품은 선택 안햇네 그럼 그것도 넣어라.
            if (txt_ItemNameUpPr.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                        + "\n" +
                        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_ItemNameUpPr.Focus(); return false;
            }


            ////구매수량을 입력 안햇네 그럼 그것도 넣어라.
            //if (txt_ItemCountUpPr.Text == "")
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
            //            + "\n" +
            //            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txt_ItemCountUpPr.Focus(); return false;
            //}


            //구매수량을 0  입력햇네 그럼 제대로 넣어라.
            //if (int.Parse(txt_ItemCountUpPr.Text.Trim()) == 0)
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //            + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Count")
            //            + "\n" +
            //            cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    txt_ItemCountUpPr.Focus(); return false;
            //}

            return true;

        }

        private void button_Add_UpPr_Click(object sender, EventArgs e)
        {
            if (Item_Rece_Error_Check_Up_Pr__01() == false) return;

            for (int i = 0; i <= dGridView_Good_Sell_Pr.Rows.Count - 1; i++)
            {
                if (dGridView_Good_Sell_Pr.Rows[i].Cells[2].Value.ToString() == txt_ItemCodeUpPr.Text.Trim())
                {
                    //dGridView_Good_Sell.Rows[i].Cells[0].Value = "V";
                    dGridView_Good_Sell_Pr.Rows[i].Cells[0].Value = "1";  txt_ItemCountUpPr.Text.Trim();
                    txt_ItemCodeUpPr.Text = "";
                    txt_ItemNameUpPr.Text = "";
                    txt_ItemCountUpPr.Text = "";
                    break;
                }
            }

        }
    }
}
