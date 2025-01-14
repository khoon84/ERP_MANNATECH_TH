﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MLM_Program
{
    public partial class frmBase_Card_Coupon_Delete : Form
    {
      

        cls_Grid_Base cgb = new cls_Grid_Base();
        cls_Grid_Base cg_Sub = new cls_Grid_Base();

        private const string base_db_name = "TLS_COUPON";
        private int Data_Set_Form_TF;

        public frmBase_Card_Coupon_Delete()
        {
            InitializeComponent();


        }


        private void frmBase_From_Load(object sender, EventArgs e)
        {
            Data_Set_Form_TF = 0;

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset(1);
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 


            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            Data_Set_Form_TF = 1;
            Data_Set_Form_TF = 0;

            cls_Pro_Base_Function cpbf = new cls_Pro_Base_Function();
            cpbf.Put_SellCode_ComboBox(combo_Se2, combo_Se_Code2);




            mtxtMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSMbid.Mask = cls_app_static_var.Member_Number_Fromat;
            mtxtSMbid2.Mask = cls_app_static_var.Member_Number_Fromat;

            mtxtSellDate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate2.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate3.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtSellDate4.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSellDate5.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtMakeDate1.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtMakeDate2.Mask = cls_app_static_var.Date_Number_Fromat;

            mtxtoutdate.Mask = cls_app_static_var.Date_Number_Fromat;
            mtxtSMbid.Mask = cls_app_static_var.Member_Number_Fromat;


            txtName.BackColor = cls_app_static_var.txt_Enable_Color;
            mtxtSellDate.BackColor = cls_app_static_var.txt_Enable_Color;
            txt_Pv.BackColor = cls_app_static_var.txt_Enable_Color;
        }



        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();

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
            butt_Delete.Left = butt_Clear.Left + butt_Clear.Width + 2;
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
            cfm.button_flat_change(button_Give);
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
            if (e.KeyValue == 116)
                T_bt = butt_Save;     //저장  F1
            if (e.KeyValue == 115)
                T_bt = butt_Delete;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Excel;    //엑셀  F8    
            if (e.KeyValue == 112)
                T_bt = butt_Clear;    //리셋  F5    
            if (e.KeyValue == 113)
                T_bt = butt_Search;    //조회  F5    
            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 115 || e.KeyValue == 119 || e.KeyValue == 112 || e.KeyValue == 115)
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


            mtxtMbid.ReadOnly = false;
            txtName4.ReadOnly = false;
            //mtxtSellDate.ReadOnly = false;

            mtxtMbid.BorderStyle = BorderStyle.Fixed3D;
            txtName4.BorderStyle = BorderStyle.Fixed3D;
            //mtxtSellDate.BorderStyle = BorderStyle.Fixed3D;

            mtxtMbid.BackColor = SystemColors.Window;
            txtName4.BackColor = SystemColors.Window;

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel19.Visible = false;
            radioB_1_2.Checked = true; 



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
                            txtName4.Text = Search_Name;
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
            mtxtMbid.Text = Send_Number; txtName4.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, 0) == true)
                Set_Form_Date(mtxtMbid.Text);
        }



        //회원번호 입력 박스의 내역이 모두 지워지면 하부 관련 회원데이타 내역을 다 리셋 시킨다. 
        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {

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

            else if (tb.Tag.ToString() == "a")
            {
                //쿼리문 오류관련 입력만 아니면 가능하다.
                if (T_R.Text_KeyChar_Check(e, tb, "a", 1, "a") == false)
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
            if (tb.TextLength >= tb.MaxLength)
            {
                SendKeys.Send("{TAB}");
                Sw_Tab = 1;
            }

            if (tb.Name == "txtSellCode")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtSellCode_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtCenter2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtCenter_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtCenter_Code2);
            //}

            if (tb.Name == "txtR_Id")
            {
                Data_Set_Form_TF = 1;
                if (tb.Text.Trim() == "")
                    txtR_Id_Code.Text = "";
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtR_Id2")
            //{
            //    if (tb.Text.Trim() == "")
            //        txtR_Id_Code2.Text = "";
            //    else if (Sw_Tab == 1)
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);
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
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txtR_Id_Code, "");
                //else
                //    Ncod_Text_Set_Data(tb, txtR_Id_Code);

                //SendKeys.Send("{TAB}");
                Data_Set_Form_TF = 0;
            }

            //if (tb.Name == "txtR_Id2")
            //{
            //    Data_Set_Form_TF = 1;
            //    if (tb.Text.ToString() == "")
            //        Db_Grid_Popup(tb, txtR_Id_Code2, "");
            //    else
            //        Ncod_Text_Set_Data(tb, txtR_Id_Code2);

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
                cgb_Pop.Next_Focus_Control = txtR_Id;

            if (tb.Name == "txtCenter2")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtBank")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtR_Id")
                cgb_Pop.Next_Focus_Control = txtRemark;

            if (tb.Name == "txtChange")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txtSellCode")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Base_Rec")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_Receive_Method")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_ItemCode")
                cgb_Pop.Next_Focus_Control = butt_Search;

            if (tb.Name == "txt_ItemName2")
                cgb_Pop.Next_Focus_Control = butt_Search;

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
            dGridView_Base_Sub_Header_Reset(1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 

            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cg_Sub.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            mtxtMbid.ReadOnly = false;
            txtName4.ReadOnly = false;
            //mtxtSellDate.ReadOnly = false;

            mtxtMbid.BorderStyle = BorderStyle.Fixed3D;
            txtName4.BorderStyle = BorderStyle.Fixed3D;
            //mtxtSellDate.BorderStyle = BorderStyle.Fixed3D;

            mtxtMbid.BackColor = SystemColors.Window;
            txtName4.BackColor = SystemColors.Window;

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel19.Visible = false;
            //mtxtSellDate.BackColor = SystemColors.Window;

            //DTP_SellDate.Visible = true;
            //tableLayoutPanel1.Visible = false ;

            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(this, mtxtMbid);

            radioB_1_2.Checked = true; 
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


            if (mtxtoutdate.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtoutdate.Text, mtxtoutdate, "Date") == false)
                {
                    mtxtoutdate.Focus(); return false;
                }
            }




            if (mtxtSellDate3.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate3.Text, mtxtSellDate3, "Date") == false)
                {
                    mtxtSellDate3.Focus(); return false;
                }
            }


            if (mtxtSellDate4.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate4.Text, mtxtSellDate4, "Date") == false)
                {
                    mtxtSellDate4.Focus(); return false;
                }
            }


            if (mtxtSellDate5.Text.Replace("-", "").Trim() != "")
            {
                if (Sn_Number_(mtxtSellDate5.Text, mtxtSellDate5, "Date") == false)
                {
                    mtxtSellDate5.Focus(); return false;
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


            //if (mtxtSellDate4.Text.Replace("-", "").Trim() != "")
            //{
            //    if (txtName2.Text == "")
            //    {
            //        MessageBox.Show("사용일자 검색시 필이 승인 번호도 조건에 넣으셔야 합니다."
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txtName2.Focus(); return false;
            //    }

            //}

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

            //Tsql = "Select  ";
            //Tsql = Tsql + " tbl_Sales_Cacu.OrderNumber, SellDate, Mbid2, M_Name, C_Price1,C_AppDate1 ,'','','','' , ''  ";
            //Tsql = Tsql + " From tbl_Sales_Cacu (nolock)";




            //S_Month  s_date
            //Tsql = "Select  ";
            //Tsql = Tsql + " TLS_COUPON.s_key , s_date,  s_cardno , s_okid,  s_Balco, s_balnm   , s_amount,  isnull(u_Price,0), s_amount - isnull(u_Price,0), S_Dno, S_Month  ";
            //Tsql = Tsql + " From TLS_COUPON (nolock)";
            //Tsql = Tsql + " LEFT JOIN (select isnull( Sum(c_price1),0) u_Price ,  c_Skey  From tbl_Sales_Cacu_OK_Card (nolock) Where Us_ing_TF = 1 Group by  c_Skey  ) C_BB  ";
            //Tsql = Tsql + " on TLS_COUPON.S_key =  C_BB.c_Skey  ";
            //Tsql = Tsql + " LEFT JOIN  TLS_COUPON_Not ON TLS_COUPON_Not.S_Key = TLS_COUPON.S_Key ";

            //string strSql = " Where TLS_COUPON.s_maketype = 1 ";
            //strSql = strSql + "  And TLS_COUPON.S_Key is not null ";

            //txt_CID  S_Dno
            Tsql = "Select  ";
            Tsql = Tsql + " TLS_COUPON.COUPONNUMBER , TLS_COUPON.MBID2,   tbl_Memberinfo.M_Name , TLS_COUPON.SALEPRICE  ";
           
            Tsql = Tsql + " ,Isnull(TLS_COUPON.RECORDID ,'') ";
            Tsql = Tsql + " ,Convert(varchar,TLS_COUPON.RECORDTIME,21) ";


            Tsql = Tsql + " ,Isnull(TLS_USE_COUPON.ORDERNUMBER,'') ";

            Tsql = Tsql + " ,Isnull(TLS_USE_COUPON.RECORDTIME,'') ";
            Tsql = Tsql + " ,Isnull(TLS_COUPON.USEENDDATE,'') ";
            Tsql = Tsql + " , TLS_COUPON.ETC_1 ,  TLS_COUPON.USEYN,'' "; 


            Tsql = Tsql + " From TLS_COUPON  (nolock) ";

            Tsql = Tsql + " LEFT  JOIN  TLS_USE_COUPON  (nolock)  ON TLS_COUPON.COUPONNUMBER = TLS_USE_COUPON.COUPONNUMBER";
            Tsql = Tsql + " LEFT  JOIN  tbl_Memberinfo   (nolock)  ON tbl_Memberinfo.Mbid = TLS_COUPON.Mbid And tbl_Memberinfo.Mbid2 = TLS_COUPON.Mbid2";


            string strSql = " Where TLS_COUPON.MBID2  > 0 ";
            

            string Mbid = ""; int Mbid2 = 0;
            //회원번호1로 검색
            if (
                (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() == "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And TLS_COUPON.Mbid ='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And TLS_COUPON.Mbid2 = " + Mbid2;
                }
            }


            //회원번호2로 검색
            if (
                (mtxtSMbid.Text.Replace("-", "").Replace("_", "").Trim() != "")
                &&
                (mtxtSMbid2.Text.Replace("-", "").Replace("_", "").Trim() != "")
                )
            {
                cls_Search_DB csb = new cls_Search_DB();
                if (csb.Member_Nmumber_Split(mtxtSMbid.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And TLS_COUPON.Mbid >='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And TLS_COUPON.Mbid2 >= " + Mbid2;
                }

                if (csb.Member_Nmumber_Split(mtxtSMbid2.Text, ref Mbid, ref Mbid2) == 1)
                {
                    if (Mbid != "")
                        strSql = strSql + " And TLS_COUPON.Mbid <='" + Mbid + "'";

                    if (Mbid2 >= 0)
                        strSql = strSql + " And TLS_COUPON.Mbid2 <= " + Mbid2;
                }
            }

            //회원명으로 검색
            if (txtName2.Text.Trim() != "")                
                strSql = strSql + " And   TLS_COUPON.COUPONNUMBER = '" + txtName2.Text.Trim() + "' ";


            if (txtName3.Text.Trim() != "")
                strSql = strSql + " And tbl_Memberinfo.M_Name  = '" + txtName3.Text.Trim() + "'";





            //가입일자로 검색 -1
            if ((mtxtSellDate2.Text.Replace("-", "").Trim() != "") && (mtxtSellDate3.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( TLS_USE_COUPON.RECORDTIME,10),'-','') = '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";

            //가입일자로 검색 -2
            if ((mtxtSellDate2.Text.Replace("-", "").Trim() != "") && (mtxtSellDate3.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( TLS_USE_COUPON.RECORDTIME,10),'-','') >= '" + mtxtSellDate2.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( TLS_USE_COUPON.RECORDTIME,10),'-','') <= '" + mtxtSellDate3.Text.Replace("-", "").Trim() + "'";

            }



            ////가입일자로 검색 -1
            //if ((mtxtSellDate4.Text.Replace("-", "").Trim() != "") && (mtxtSellDate5.Text.Replace("-", "").Trim() == ""))
            //{
            //    //ON TLS_COUPON.s_key = tbl_Sales_Cacu.C_Skey
            //    strSql = strSql + " And TLS_COUPON.TLS_USE_COUPON IN (Select  C_Skey From tbl_Sales_Cacu (nolock) Where  Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','') = '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "' And C_Skey > 0 )   ";
            //    //strSql = strSql + " And Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','') = '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";
            //}
            ////가입일자로 검색 -2
            //if ((mtxtSellDate4.Text.Replace("-", "").Trim() != "") && (mtxtSellDate5.Text.Replace("-", "").Trim() != ""))
            //{
            //    strSql = strSql + " And TLS_COUPON.TLS_USE_COUPON IN (Select  C_Skey From tbl_Sales_Cacu (nolock) Where  Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','') >= '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'  And Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','') <= '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "' And C_Skey > 0 )   ";

            //    //strSql = strSql + " And Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','')  >= '" + mtxtSellDate4.Text.Replace("-", "").Trim() + "'";
            //    //strSql = strSql + " And Replace( LEFT(tbl_Sales_Cacu.RecordTime,10 ) ,'-','')  <= '" + mtxtSellDate5.Text.Replace("-", "").Trim() + "'";
            //}



            //기록일자로 검색 -1
            if ((mtxtMakeDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakeDate2.Text.Replace("-", "").Trim() == ""))
                strSql = strSql + " And Replace(Left( convert(varchar,TLS_COUPON.RECORDTIME,112) ,10),'-','') = '" + mtxtMakeDate1.Text.Replace("-", "").Trim() + "'";

            //기록일자로 검색 -2
            if ((mtxtMakeDate1.Text.Replace("-", "").Trim() != "") && (mtxtMakeDate2.Text.Replace("-", "").Trim() != ""))
            {
                strSql = strSql + " And Replace(Left( convert(varchar,TLS_COUPON.RECORDTIME,112) ,10),'-','') >= '" + mtxtMakeDate1.Text.Replace("-", "").Trim() + "'";
                strSql = strSql + " And Replace(Left( convert(varchar,TLS_COUPON.RECORDTIME,112) ,10),'-','') <= '" + mtxtMakeDate2.Text.Replace("-", "").Trim() + "'";
            }


            if (txtR_Id_Code.Text.Trim() != "")
                strSql = strSql + " And TLS_COUPON.RECORDID = '" + txtR_Id_Code.Text.Trim() + "'";

            //if (txt_CID.Text.Trim() != "")
            //    strSql = strSql + " And TLS_COUPON.RECORDID = '" + txt_CID.Text.Trim() + "'";



            //if (opt_Leave_2.Checked == true)
            //{
            //    strSql = strSql + " And  TLS_COUPON_Not.S_Key is null ";
            //}

            //if (opt_Leave_3.Checked == true)
            //{
            //    strSql = strSql + " And  TLS_COUPON_Not.S_Key is not null ";
            //}



            Tsql = Tsql + strSql;
            Tsql = Tsql + " Order by TLS_COUPON.RECORDTIME DESC    ";

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


            //int Be_S_key = 0;

            //if (fi_cnt > 0)
            //{
            //    Be_S_key = int.Parse(ds.Tables[base_db_name].Rows[fi_cnt - 1][0].ToString());
            //}

            while (Col_Cnt < cg_Sub.grid_col_Count)
            {
                ////"_s_Key" , "승인날짜" ,"카드번호" , "승인번호"   , "카드코드"  
                ////                     , "카드명"   , "승인금액"   ,"사용금액" , "사용가능금액" , "단말기ID(회원번호)"

                //if (Be_S_key == int.Parse(ds.Tables[base_db_name].Rows[fi_cnt][0].ToString()) && Col_Cnt <= 10)
                //{
                //    double T_empD = 0;
                //    if (Col_Cnt == 0) row0[Col_Cnt] = "0";
                //    if (Col_Cnt == 1) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 2) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 3) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 4) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 5) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 6) row0[Col_Cnt] = T_empD;
                //    if (Col_Cnt == 7) row0[Col_Cnt] = T_empD;
                //    if (Col_Cnt == 8) row0[Col_Cnt] = T_empD;
                //    if (Col_Cnt == 9) row0[Col_Cnt] = "";
                //    if (Col_Cnt == 10) row0[Col_Cnt] = "";
                //}
                //else
                    row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];

                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_Sub_gr_dic2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
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



        private void dGridView_Base_Sub_Header_Reset()
        {
            cg_Sub.grid_col_Count = 12;
            cg_Sub.basegrid = dGridView_Base_Sub;
            cg_Sub.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cg_Sub.grid_Frozen_End_Count = 2;
            cg_Sub.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //Tsql = "Select  ";
            //Tsql = Tsql + " TLS_COUPON.COUPONNUMBER , MBID2,   tbl_Memberinfo.M_Name , TLS_COUPON.SALEPRICE  ";

            //Tsql = Tsql + " ,Isnull(TLS_COUPON.RECORDID ,'') ";
            //Tsql = Tsql + " ,Convert(varchar,TLS_COUPON.RECORDTIME,21) ";


            //Tsql = Tsql + " ,Isnull(TLS_USE_COUPON.ORDERNUMBER,'') ";

            //Tsql = Tsql + " ,Isnull(TLS_USE_COUPON.RECORDTIME,'') ";
            //Tsql = Tsql + " ,tbl_Sales_Cacu.OrderNumber, SellDate ,Mbid2, M_Name, C_Price1,  tbl_Sales_Cacu.RecordTime   ";

            string[] g_HeaderText = { "할인권번호" , "회원번호" ,"성명" , "할인권금액"   , "기록자"
                                     , "기록시간"   , "사용주문번호"   ,"사용시간"  ,"종료시간" , "비고" , "사용여부" , ""
                                     
                                    };
            cg_Sub.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 110,  110, 90, 80, 110
                             , 110 ,70 , 70 ,  100,  100,  100 , 0                             
                            };
            cg_Sub.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true      ,true        ,true
                                   };
            cg_Sub.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                                ,DataGridViewContentAlignment.MiddleLeft  //5


                                       ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft //10                                   
                            

                              };
            cg_Sub.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cg_Sub.grid_cell_format = gr_dic_cell_format;
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



            //if (mtxtSellDate.Text.Replace("-", "").Trim() == "")
            //{
            //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
            //           + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_In_Date")
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //    mtxtSellDate.Focus(); return false;
            //}

            //if (mtxtSellDate.Text.Replace("-", "").Trim() != "")
            //{
            //    if (Sn_Number_(mtxtSellDate.Text, mtxtSellDate, "Date") == false)
            //    {
            //        mtxtSellDate.Focus(); return false;
            //    }
            //}





            //me = T_R.Text_Null_Check(txt_Pv, "Msg_Sort_In_Price"); //적용 PV를
            //if (me != "")
            //{
            //    MessageBox.Show(me);
            //    return false;
            //}


            me = T_R.Text_Null_Check(txtName4, "Msg_Sort_In_Name"); //입금자명
            if (me != "")
            {
                MessageBox.Show(me);
                return false;
            }


            ////마감정산이 이루어진 판매 날짜인지 체크한다.                
            //cls_Search_DB csd = new cls_Search_DB();
            //if (csd.Close_Check_SellDate("tbl_CloseTotal_02", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
            //{
            //    mtxtSellDate.Focus(); return false;
            //}


            //if (txtKey.Text != "") //수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
            //{
            //    double app_Pv = double.Parse(txt_Pv.Text.Trim().Replace(",", ""));//입금액
            //    double app_Pv2 = double.Parse(txtKey2.Text.Trim().Replace(",", "")); //사용액
            //    double app_Pv3 = double.Parse(txtKey3.Text.Trim().Replace(",", "")); //사용가능금액

            //    if (app_Pv < app_Pv2) //입금액으로입력한 금액이 사용한 금액보다 작다. 그럼 수정 안되게 한다
            //    {
            //        MessageBox.Show("입금액으로 수정하신 금액이 이미 사용하신 금액보다 작습니다."
            //          + "\n" +
            //          cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
            //        txt_Pv.Focus();
            //        return false;
            //    }
            //}



            return true;
        }



        //저장 버튼을 눌럿을때 실행되는 메소드 실질적인 변경 작업이 이루어진다.
        private void Save_Base_Data(ref int Save_Error_Check)
        {
            Save_Error_Check = 0;
            string str_Q = "";

            if (txtKey.Text == "")
                str_Q = "Msg_Base_Save_Q";
            else
                str_Q = "Msg_Base_Edit_Q";


            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString(str_Q), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (Check_TextBox_Error() == false) return;

            if (txtKey.Text != "") //수정일 경우에는 수정 프로시져로 가고 이 프로시져를 빠져나가라
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
                double app_Pv = double.Parse(txt_Pv.Text.Trim().Replace(",", ""));

                StrSql = "INSERT INTO TLS_COUPON ";
                StrSql = StrSql + " (";
                StrSql = StrSql + "  S_MAKETYPE , S_TYPE , S_DATE ";
                StrSql = StrSql + " , S_TIME, S_AMOUNT , S_BALCO ";
                StrSql = StrSql + " , S_BALNM , S_REM ";
                StrSql = StrSql + " , S_IDATE , S_Sender ";
                StrSql = StrSql + " , S_IUSER, S_VIEW_YN ";
                StrSql = StrSql + " ) ";
                StrSql = StrSql + " Values ";
                StrSql = StrSql + " (";
                StrSql = StrSql + "'4'";
                StrSql = StrSql + ",'0210'";
                StrSql = StrSql + ",'" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                StrSql = StrSql + ",'100000'";
                StrSql = StrSql + "," + app_Pv;
                StrSql = StrSql + ",'000'";

                StrSql = StrSql + ",'현금'";
                StrSql = StrSql + ",'" + txtRemark.Text.Trim() + "'";

                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + ",'" + txtName.Text + "'";

                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",'Y'";
                StrSql = StrSql + ")";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);



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
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();


            try
            {

                string StrSql = "";
                double app_Pv = double.Parse(txt_Pv.Text.Trim().Replace(",", ""));

                StrSql = "INSERT INTO TLS_COUPON_Mod ";
                StrSql = StrSql + " Select  * ";
                StrSql = StrSql + ",'" + cls_User.gid + "'";
                StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " From TLS_COUPON (nolock) ";
                StrSql = StrSql + " Where S_KEY = " + txtKey.Text;

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "UpDate  TLS_COUPON Set ";
                StrSql = StrSql + "  S_AMOUNT = " + app_Pv;
                StrSql = StrSql + ", S_DATE = '" + mtxtSellDate.Text.Replace("-", "").Trim() + "'";
                //StrSql = StrSql + ", SellCode  = '" + txtSellCode_Code.Text.Trim() + "'";
                StrSql = StrSql + ", S_REM = '" + txtRemark.Text.Trim() + "'";
                StrSql = StrSql + " Where S_KEY = " + txtKey.Text;

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

                if (check_Not.Checked == true)
                {
                    string Tsql = "";
                    Tsql = "Select S_KEY From TLS_COUPON_Not (nolock) ";
                    Tsql = Tsql + " Where S_KEY = " + txtKey.Text;

                    DataSet ds = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text);
                    int ReCnt = Temp_Connect.DataSet_ReCount;
                    if (ReCnt == 0)
                    {
                        StrSql = "INSERT INTO TLS_COUPON_Not (S_KEY , Recordid, RecordTime ) Values ( ";
                        StrSql = StrSql + txtKey.Text + ",'" + cls_User.gid + "'";
                        StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                        StrSql = StrSql + " ) ";

                        Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                    }
                }
                else
                {
                    StrSql = "INSERT INTO TLS_COUPON_Not_Mod ";
                    StrSql = StrSql + " Select  * ";
                    StrSql = StrSql + ",'" + cls_User.gid + "'";
                    StrSql = StrSql + ",Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " From TLS_COUPON_Not (nolock) ";
                    StrSql = StrSql + " Where S_KEY = " + txtKey.Text;

                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                    StrSql = "Delete From   TLS_COUPON_Not ";
                    StrSql = StrSql + " Where S_KEY = " + txtKey.Text;

                    Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);
                }


                tran.Commit();

                Save_Error_Check = 1;

            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Edit_Err"));
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



            //사용금액이 잇다 그럼 삭제 하면 안된다.
            if (txtKey2.Text != "" )
            {
                MessageBox.Show("사용하신 내역이 있습니다. 삭제처리가 불가능 합니다. 확인후 다시 시도해주십시요.");
                return;
            }




            ////마감정산이 이루어진 판매 날짜인지 체크한다.                
            //cls_Search_DB csd = new cls_Search_DB();
            //if (csd.Close_Check_SellDate("tbl_CloseTotal_02", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
            //{
            //    mtxtSellDate.Focus(); return;
            //}


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();



            string Tsql = "Select  ";
            Tsql = Tsql + " TLS_USE_COUPON.COUPONNUMBER "; 
            Tsql = Tsql + " From TLS_USE_COUPON  (nolock) ";
            Tsql = Tsql + " Where TLS_USE_COUPON.COUPONNUMBER = '" + txtKey.Text + "'";        
            
            //++++++++++++++++++++++++++++++++
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt > 0) 
            {
                MessageBox.Show("사용하신 내역이 있습니다. 삭제처리가 불가능 합니다. 확인후 다시 시도해주십시요.");
                return;
            }


            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";

                StrSql = "Insert into  TLS_COUPON_Mod ";
                StrSql = StrSql + " Select *  ";
                StrSql = StrSql + ",'" + cls_User.gid + "', Convert(Varchar(25),GetDate(),21) From TLS_COUPON (nolock) ";
                StrSql = StrSql + " Where COUPONNUMBER = '" + txtKey.Text + "'";

                Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);


                StrSql = "Delete From TLS_COUPON  ";
                StrSql = StrSql + " Where COUPONNUMBER = '" + txtKey.Text + "'";

                Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);


                tran.Commit();
                Delete_Error_Check = 1;
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del"));

            }
            catch (Exception ee)
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
            //return; 
            //string[] g_HeaderText = { "등록일자" , "구분" ,"주문번호" , "사용금액"   , "회원번호"
            //                         , "등록자"   , ""   ,"" , "" , ""
            //                         ,""
            //                        };


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            dGridView_Base_Sub_Header_Reset(1); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            mtxtoutdate.Text = "";

            txtKey.Text = "";
            txtKey2.Text = ""; txtKey3.Text = "";
            txtName.Text = ""; mtxtSellDate.Text = ""; txt_Pv.Text = "";
            cls_form_Meth ct = new cls_form_Meth();
            ct.from_control_clear(groupBox1, mtxtMbid);
            if (((sender as DataGridView).CurrentRow != null) && ((sender as DataGridView).CurrentRow.Cells[0].Value != null))
            {
                if ((sender as DataGridView).CurrentRow.Cells[0].Value.ToString() == "")
                    return;

                string T_Mbid = ""; string T_M_Name = ""; string T_SellDate = "";
                double T_PV = 0, T_PV2 = 0, T_PV3 = 0; string T_SellTypeName = ""; string T_ETC = "", T_Use = "";
                int t_Seq = 0; string T_SellCode = "";
                Data_Set_Form_TF = 1;

                //string[] g_HeaderText = { "할인권번호" , "회원번호" ,"성명" , "할인권금액"   , "기록자"
                //                     , "기록시간"   , "사용주문번호"   ,"사용시간" , "" , ""

                

                T_M_Name = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString();
                T_SellDate = (sender as DataGridView).CurrentRow.Cells[5].Value.ToString();
                T_PV = double.Parse((sender as DataGridView).CurrentRow.Cells[3].Value.ToString());

                //T_PV2 = double.Parse((sender as DataGridView).CurrentRow.Cells[7].Value.ToString());
                //T_PV3 = double.Parse((sender as DataGridView).CurrentRow.Cells[8].Value.ToString());
                //T_ETC = (sender as DataGridView).CurrentRow.Cells[3].Value.ToString();
                //T_Use = (sender as DataGridView).CurrentRow.Cells[7].Value.ToString();
                //t_Seq = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString();


                txtKey.Text = (sender as DataGridView).CurrentRow.Cells[0].Value.ToString(); // t_Seq.ToString();

                txtKey2.Text = (sender as DataGridView).CurrentRow.Cells[6].Value.ToString().ToString(); //사용한 금액
                //txtKey3.Text = T_PV3.ToString(); // 사용 가능한 금액

                mtxtMbid.Text = (sender as DataGridView).CurrentRow.Cells[1].Value.ToString(); // t_Seq.ToString();
                txtName4.Text = (sender as DataGridView).CurrentRow.Cells[2].Value.ToString(); // t_Seq.ToString();
                txtRemark.Text = (sender as DataGridView).CurrentRow.Cells[9].Value.ToString(); // t_Seq.ToString();


                mtxtoutdate.Text = (sender as DataGridView).CurrentRow.Cells[8].Value.ToString().Substring(0,10); // t_Seq.ToString();

                txtName.Text = txtKey.Text;                               
                mtxtSellDate.Text = T_SellDate.Replace("-", "");
                txt_Pv.Text = string.Format(cls_app_static_var.str_Currency_Type, T_PV);


                //txtSellCode_Code.Text = T_SellCode;
                //txtSellCode.Text = T_SellTypeName;

                string useyn  = (sender as DataGridView).CurrentRow.Cells[10].Value.ToString();
                if (useyn == "Y")
                {
                    radioB_USE.Checked = true;
                    radioB_NOUSE.Checked = false;
                }
                else
                {
                    radioB_USE.Checked = false;
                    radioB_NOUSE.Checked = true;
                }

                // txtRemark.Text = T_ETC;

                mtxtMbid.ReadOnly = true;
                txtName4.ReadOnly = true;
                mtxtMbid.BorderStyle = BorderStyle.FixedSingle;
                txtName4.BorderStyle = BorderStyle.FixedSingle;
                mtxtMbid.BackColor = cls_app_static_var.txt_Enable_Color;
                txtName4.BackColor = cls_app_static_var.txt_Enable_Color;
                tableLayoutPanel1.Visible = false;
                tableLayoutPanel19.Visible = false;
                //if (T_Use == "중지")
                //    check_Not.Checked = true; 

                ////마감정산이 이루어진 판매 날짜인지 체크한다.                
                //cls_Search_DB csd = new cls_Search_DB();
                //if (csd.Close_Check_SellDate("tbl_CloseTotal_02", mtxtSellDate.Text.Replace("-", "").Trim()) == false)
                //{
                //    mtxtSellDate.ReadOnly = true;
                //    mtxtSellDate.BorderStyle = BorderStyle.FixedSingle;
                //    mtxtSellDate.BackColor = cls_app_static_var.txt_Enable_Color;
                //    DTP_SellDate.Visible = false; 
                //}


                // Base_Sub_Grid_Set(txtKey.Text);

                Data_Set_Form_TF = 0;
                mtxtSellDate.Focus();


            }
        }





        private void Base_Sub_Grid_Set(string S_Key)
        {


            //string[] g_HeaderText = { "등록일자" , "구분" ,"주문번호" , "사용금액"   , "회원번호"
            //                         , "등록자"   , ""   ,"" , "" , ""
            //                         ,""
            //                        };

            string Tsql = "";

            Tsql = "Select  ";
            Tsql = Tsql + " REG_DATE  ,S_GUBUN ,ORDERNUMBER ,S_AMOUNT , MBID2  , RecordID ,'','','','',''  ";
            Tsql = Tsql + " From TLS_SWIPE_HISTORY (nolock)";
            //Tsql = Tsql + " LEFT JOIN tbl_SalesDetail  (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber";

            Tsql = Tsql + "  Where TLS_SWIPE_HISTORY.S_KEY =  " + S_Key;
            Tsql = Tsql + " Order by TLS_SWIPE_HISTORY.REG_DATE ASC ";

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
                Set_Sub_gr_dic2(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
            }

            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();


        }



        private void dGridView_Base_Sub_Header_Reset(int i)
        {
            cgb.grid_col_Count = 11;
            cgb.basegrid = dGridView_Base_S;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 2;
            cgb.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            string[] g_HeaderText = { "등록일자" , "구분" ,"주문번호" , "사용금액"   , "회원번호"
                                     , "등록자"   , ""   ,"" , "" , ""
                                     ,""
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 90,  110, 90, 80, 110
                             , 110 ,0 , 0 ,  0 , 0
                             , 0
                            };
            cgb.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleRight
                               ,DataGridViewContentAlignment.MiddleLeft  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft //10                                   

                               ,DataGridViewContentAlignment.MiddleLeft //10                                   
                          
                              };
            cgb.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            cgb.grid_cell_format = gr_dic_cell_format;
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
                txtName4.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();

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

        private void button_Give_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return;

            if (txtName4.Text.Trim() == "")
            {
                return;
            }



            ////사용금액이 잇다 그럼 삭제 하면 안된다.
            //if (txtKey2.Text != "")
            //{
            //    MessageBox.Show("사용하신 내역이 있습니다. 삭제처리가 불가능 합니다. 확인후 다시 시도해주십시요.");
            //    return;
            //}



            if (Check_TextBox_Error() == false) return;

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
                string StrSql = "";
                if (radioB_3000.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '3000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_10000.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '10000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_20000.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '20000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_1_2.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '10000','" + cls_User.gid  + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '20000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + Mbid2 + "', '3000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                new cls_sms().Coupon_Membership(Mbid2); //문제터짐
                tran.Commit();                
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));

            }
            catch (Exception ee)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Form_Clear_();                
                Base_Sub_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void button_Give_Excel_Click(object sender, EventArgs e)
        {


            if (MessageBox.Show("쿠폰대상자 엑셀업로드에서 추가된 대상자들에게 쿠폰이 일괄 부여됩니다. \n 계속 진행하시겠습니까?","", MessageBoxButtons.YesNo) == DialogResult.No) return;

           
            ////사용금액이 잇다 그럼 삭제 하면 안된다.
            //if (txtKey2.Text != "")
            //{
            //    MessageBox.Show("사용하신 내역이 있습니다. 삭제처리가 불가능 합니다. 확인후 다시 시도해주십시요.");
            //    return;
            //}



           // if (Check_TextBox_Error() == false) return;

            cls_Search_DB csd = new cls_Search_DB();
            string T_Mbid = mtxtMbid.Text.Trim();
            string Mbid = ""; int Mbid2 = 0;
            csd.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();



            cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();
            string Tsql = "select * from tbl_memberinfo_Coupon";
            DataSet ds1 = new DataSet();

            if (Temp_Connect1.Open_Data_Set(Tsql, "tbl_memberinfo_Coupon", ds1) == false) return;
            int ReCnt = Temp_Connect1.DataSet_ReCount;
            for (int i = 0; i < ReCnt; i++)
            {
                int mbid2 = int.Parse(ds1.Tables["tbl_memberinfo_Coupon"].Rows[i][0].ToString());
              
                string StrSql = "";
                if (radioB_3000.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + mbid2 + "', '10000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_10000_2.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + mbid2 + "', '10000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_20000_2.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + mbid2 + "', '20000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }

                if (radioB_1_3.Checked == true)
                {
                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + mbid2 + "', '10000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);

                    StrSql = "exec USP_INSERT_TLS_COUPON_CS '" + mbid2 + "', '20000','" + cls_User.gid + "','" + txtRemark.Text.Trim() + "'";
                    Temp_Connect.Insert_Data(StrSql, base_db_name, Conn, tran);
                }
                new cls_sms().Coupon_Membership(mbid2); //문제터짐
            }


            try
            {
                tran.Commit();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
            }
 
            catch (Exception ee)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));

            }

            finally
            {
                tran.Dispose();
                Temp_Connect.Close_DB();

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                Form_Clear_();
                Base_Sub_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;

                
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_update_Click(object sender, EventArgs e)
        {
            if(txtName.Text == "")
            {
                MessageBox.Show("먼저 변경하실 쿠폰을 선택 해 주시기 바랍니다.");
                return;
            }


            string useyn = "";
            if (radioB_USE.Checked == true)
            {
                useyn = "Y";
            }
            else
            {
                useyn = "N";
            }

            string USEENDDATE = "";

            USEENDDATE = mtxtoutdate.Text + " 00:00:00.000";



            cls_Connect_DB Temp_Connect4 = new cls_Connect_DB();
            string Tsql2 = "select a.couponnumber from TLS_USE_COUPON a join tbl_salesdetail b on a.ordernumber = b.ordernumber  WHERE a.COUPONNUMBER = '" + txtName.Text + "'";
            DataSet ds2 = new DataSet();
            if (Temp_Connect4.Open_Data_Set(Tsql2, "tbl_salesdetail", ds2) == false) return;
            int ReCnt2 = Temp_Connect4.DataSet_ReCount;
            if (ReCnt2 > 0)
            {
           
                    MessageBox.Show("주문에 사용된 쿠폰은 변경 할 수 없습니다."
                  );
                    return;
                
            }



            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string StrSql = "";
            StrSql = "update  TLS_COUPON set  USEYN = '"+ useyn + "' , USEENDDATE = '"+ USEENDDATE + "'  WHERE COUPONNUMBER = '" + txtName.Text + "'";
            Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

            MessageBox.Show("사용여부와 종료일자가 변경되었습니다.");
            tran.Commit();
            Form_Clear_();
        }

        private void button_update2_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("먼저 변경하실 쿠폰을 선택 해 주시기 바랍니다.");
                return;
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            string StrSql = "";
            StrSql = "update  TLS_COUPON set ETC_1 = '" + txtRemark.Text + "' WHERE COUPONNUMBER = '" + txtName.Text + "'";
            Temp_Connect.Update_Data(StrSql, Conn, tran, this.Name, this.Text);

            MessageBox.Show("비고가 변경되었습니다.");
            tran.Commit();
            Form_Clear_();
        }

        private void mtxtSMbid_Click(object sender, EventArgs e)
        {

            MaskedTextBox mtb = (MaskedTextBox)sender;

            //cls_form_Meth ct = new cls_form_Meth();
            //ct.from_control_clear(this, mtxtSMbid);

            mtxtSMbid.Select(0, 0);
            mtxtSMbid.ReadOnly = false;
            
            //mtxtSellDate.ReadOnly = false;

            mtxtSMbid.BorderStyle = BorderStyle.Fixed3D;
            
            //mtxtSellDate.BorderStyle = BorderStyle.Fixed3D;

            mtxtSMbid.BackColor = SystemColors.Window;
            


            //마스크텍스트 박스에 입력한 내용이 있으면 그곳 다음으로 커서가 가게 한다.
            if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                mtb.SelectionStart = mtb.Text.Replace("-", "").Replace("_", "").Trim().Length + 0;
        }
    }
}
