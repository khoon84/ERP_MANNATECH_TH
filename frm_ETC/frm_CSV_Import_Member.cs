﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;


using System.IO;
using ExcelDataReader;
namespace MLM_Program
{
    public partial class frm_CSV_Import_Member : Form
    {

        string mbid2;
        string itemcode;
        string startdate;

        cls_Grid_Base cgb = new cls_Grid_Base();
        DataSet dsExcels = new DataSet();
        private int Data_Set_Form_TF;
        private int Load_TF = 0;
        private string idx_Na_Code = "";
        //private DataTable DT = new DataTable();

        private const string base_db_name = "JDE_PROC_Member";
        public frm_CSV_Import_Member()
        {
            InitializeComponent();
        }

        private void frm_CSV_Import_Member_Load(object sender, EventArgs e)
        {

            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            Base_Grid_Set();



            cls_form_Meth cm = new cls_form_Meth();
            cm.from_control_text_base_chang(this);

            txtFilePath.BackColor = cls_app_static_var.txt_Enable_Color;
        }


        private void dGridView_Base_Header_Reset()
        {
            cgb.grid_col_Count = 3;
            cgb.basegrid = dGridView_Base;
            cgb.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            cgb.grid_Frozen_End_Count = 1;

            string[] g_HeaderText = {"선택",  "프로모션등록번호",  "회원번호"
                                    };

            string[] g_ColsName = {"Selected","JDE_PROC_CODE", "MBID2"
                                    };
            cgb.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 80, 120, 100
                            };
            cgb.grid_col_w = g_Width;


            Boolean[] g_ReadOnly = { true , true,  true
                                   };
            cgb.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleLeft
                               ,DataGridViewContentAlignment.MiddleLeft

                              };
            cgb.grid_col_alignment = g_Alignment;

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            cgb.grid_cell_format = gr_dic_cell_format;
        }

        private void Make_Base_Query(ref string Tsql)
        {
            Tsql = " SELECT '' as Selected,* from JDE_PROC_Member (nolock) ";

        }
        private void Base_Grid_Set()
        {

            string Tsql = "";

            //if (mtxtMbid.Text.Replace("_", "").Trim() == "")
            //{
            //    MessageBox.Show("회원번호를 입력하시기 바랍니다.");
            //    mtxtMbid.Focus();
            //    return;
            //}

            Make_Base_Query(ref Tsql);

            cls_form_Meth cm = new cls_form_Meth();
            //cm._chang_base_caption_search(m_text);

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
            Dictionary<string, double> Center_MemCnt = new Dictionary<string, double>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.                
            }
            cgb.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            cgb.db_grid_Obj_Data_Put();
        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0]
                                , ds.Tables[base_db_name].Rows[fi_cnt][1]
                                , ds.Tables[base_db_name].Rows[fi_cnt][2]

                            };

            gr_dic_text[fi_cnt + 1] = row0;
        }

        private void Base_Clear()
        {
            dGridView_Base.Enabled = true;
            dGridView_Base.DataSource = null;
            dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
            cgb.d_Grid_view_Header_Reset();
            Base_Grid_Set();

            txtFilePath.Text = "";
            mtxtMbid.Text = "";
            txtName.Text = "";
            txt_promotion.Text = "";
            txt_ItemName.Text = "";
            txt_promotion_2.Text = "";
            mtxtMbid_2.Text = "";
            mtxtitemcount.Text = "";
            mtxtSellDate1.Text = "";
            mtxtSellDate2.Text = "";
            combo_Sheet.SelectedIndex = -1;
            mbid2 = null;
            itemcode = null;

        }

        private void Base_Clear2()
        {


            txtFilePath.Text = "";
            mtxtMbid.Text = "";
            txtName.Text = "";
            txt_promotion.Text = "";
            txt_ItemName.Text = "";
            txt_promotion_2.Text = "";
            mtxtMbid_2.Text = "";
            mtxtitemcount.Text = "";
            mtxtSellDate1.Text = "";
            mtxtSellDate2.Text = "";
            combo_Sheet.SelectedIndex = -1;
        }


        private void Base_Button_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;


            if (bt.Name == "butt_Clear")
            {
                Base_Clear();
            }
            if (bt.Name == "butt_Save")
            {
                butt_Save_Click(bt, e);
            }

            else if (bt.Name == "butt_Select")
            {
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();
                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


                Base_Grid_Set();  //뿌려주는 곳
                this.Cursor = System.Windows.Forms.Cursors.Default;

            }

            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }
            if (bt.Name == "butt_Order_End")
            {
                butt_Order_End_Click(bt, e);
            }
            else if (bt.Name == "butt_S_check")
            {
                dGridView_Base.Visible = false;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "V";
                }
                dGridView_Base.Visible = true;
            }
            else if (bt.Name == "butt_S_Not_check")
            {
                dGridView_Base.Visible = false;
                dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                for (int i = 0; i <= dGridView_Base.Rows.Count - 1; i++)
                {
                    dGridView_Base.Rows[i].Cells[0].Value = "";
                }
                dGridView_Base.Visible = true;
            }
            else if (bt.Name == "btn_S_Multy")
            {
                dGridView_Base.Visible = false;
                foreach (DataGridViewRow row in dGridView_Base.SelectedRows)
                {
                    row.Cells[0].Value = "V";
                }
                dGridView_Base.Visible = true;

            }
            else if (bt.Name == "btn_S_Multy_C")
            {
                dGridView_Base.Visible = false;
                foreach (DataGridViewRow row in dGridView_Base.SelectedRows)
                {
                    row.Cells[0].Value = "";
                }
                dGridView_Base.Visible = true;
            }
        }



        private void btnLoad_Click(object sender, EventArgs e)
        {
            int RCnt = dGridView_Base.Rows.Count - 1;

            if (dGridView_Base.DataSource != null)
            {
                dGridView_Base.DataSource = null;
            }
            else if (RCnt > 0)
            {
                dGridView_Base.Visible = true;
            }

            dGridView_Base.Rows.Clear();
            dGridView_Base.Columns.Clear();
            //dGridView_Base.Rows.Clear();
            txtFilePath.Text = "";
            //작업
            combo_Sheet.Items.Clear();
            Load_TF = 0;
            LoadNewFile();

            //int RCnt = dGridView_Base.Rows.Count-1;

            //DT.Rows.Clear();
            //DT.Columns.Clear();

            //if (RCnt > 0)
            //{
            //    dGridView_Base.Visible = false;
            //    //for (int TCnt = 0; TCnt <= RCnt; TCnt++)
            //    //    dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);

            //    dGridView_Base.Visible = true;
            //}

            //txtFilePath.Text = "";
            //Load_TF = 0;
            //LoadNewFile();
        }


        private void LoadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            System.Windows.Forms.DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;

                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcel_Sheet();
                }

                catch (System.Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);


                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(theException.Message);
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }

            }
        }


        private void loadExcel_Sheet()
        {
            dsExcels = new DataSet();
            var extension = Path.GetExtension(txtFilePath.Text).ToLower();
            using (var stream = new FileStream(txtFilePath.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {

                IExcelDataReader reader = null;
                if (extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else if (extension == ".csv")
                {
                    reader = ExcelReaderFactory.CreateCsvReader(stream);
                }

                if (reader == null)
                    return;

                dsExcels = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

            }

            foreach (DataTable dt in dsExcels.Tables)
            {
                //작업
                combo_Sheet.Items.Add(dt.TableName);
            }


            Load_TF = 1;

            //TextFieldParser ps = new TextFieldParser(txtFilePath.Text, System.Text.Encoding.GetEncoding("EUC-KR"));
            //ps.TextFieldType = FieldType.Delimited;
            //ps.SetDelimiters(",");

            //int i = 0;

            //while (!ps.EndOfData)
            //{
            //    string[] row = ps.ReadFields();
            //    if (i == 0)
            //    {
            //        for (int j = 0; j < row.Length; j++)
            //        {
            //            DT.Columns.Add(row[j].Trim());
            //        }
            //    }
            //    else
            //    {
            //        if (row[0].ToString() != "EOF")
            //            DT.Rows.Add(row);
            //    }
            //    i = i + 1;
            //}

            //loadExcelToDataGrid();
        }

        private void Grid_Base_Seting()
        {
            dGridView_Base.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));
            dGridView_Base.ColumnHeadersHeight = 18;
            dGridView_Base.GridColor = System.Drawing.Color.Black;
            dGridView_Base.EnableHeadersVisualStyles = false;
            //dGridView_Base.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);
            dGridView_Base.DefaultCellStyle.SelectionBackColor = Color.FromArgb(89, 117, 159);
            dGridView_Base.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dGridView_Base.BorderStyle = BorderStyle.FixedSingle;
            dGridView_Base.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dGridView_Base.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            dGridView_Base.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dGridView_Base.ReadOnly = true;
            dGridView_Base.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dGridView_Base.AllowUserToAddRows = false;
            dGridView_Base.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

        }


        private void loadExcelToDataGrid()
        {
            Grid_Base_Seting();

            dGridView_Base.DataSource = dsExcels.Tables[combo_Sheet.SelectedIndex];
            //if (dGridView_Base.DataSource == null)
            //    dGridView_Base.DataSource = DT;

        }

        private void dGridView_Base_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            //Header인지 확인
            if (e.ColumnIndex < 0 & e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                //행 번호를 표시할 범위를 결정
                System.Drawing.Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);
                //행번호를 표시
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                      e.CellStyle.Font, indexRect, e.CellStyle.ForeColor,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
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
            cfm.button_flat_change(butt_Search);

            cfm.button_flat_change(butt_Save);
            cfm.button_flat_change(butt_S_check);
            cfm.button_flat_change(butt_S_Not_check);
            cfm.button_flat_change(butt_Order_End);
        }


        private void frm_Base_Activated(object sender, EventArgs e)
        {
            this.Refresh();
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

                        }
                    }
                }

            }

            Button T_bt = butt_Exit;
            if (e.KeyValue == 123)
                T_bt = butt_Exit;    //닫기  F12
            if (e.KeyValue == 112)
                T_bt = butt_Clear;     //조회  F1
            if (e.KeyValue == 115)
                T_bt = butt_Order_End;   // 삭제  F4
            if (e.KeyValue == 119)
                T_bt = butt_Order_End;    //엑셀  F8    
            if (e.KeyValue == 113)
                T_bt = butt_Save;    //새로고침  F5    

            if (T_bt.Visible == true)
            {
                EventArgs ee1 = null;
                if (e.KeyValue == 123 || e.KeyValue == 113 || e.KeyValue == 119 || e.KeyValue == 116 || e.KeyValue == 115 || e.KeyValue == 112)
                    Base_Button_Click(T_bt, ee1);

            }
        }

        private void butt_Exit_Click(object sender, EventArgs e)
        {
            dGridView_Base.Enabled = true;
            Button bt = (Button)sender;


            if (bt.Name == "butt_Select")
            {
                Boolean chage_Center_tf = false;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                progress.Visible = true; progress.Value = 0;
                chage_Center_tf = Chang_CenterCode();  //실질적인 센터 변경이 이루어지는 메소드
                progress.Visible = false;
                this.Cursor = System.Windows.Forms.Cursors.Default;

                if (chage_Center_tf == true)
                {
                    int RCnt = dGridView_Base.Rows.Count - 1;
                    if (dGridView_Base.DataSource != null)
                        dGridView_Base.DataSource = null;
                    else if (RCnt > 0)
                    {
                        dGridView_Base.Visible = false;
                        dGridView_Base.Rows.Clear();
                        dGridView_Base.Visible = true;
                    }

                    txtFilePath.Text = "";
                    combo_Sheet.Items.Clear();
                    Load_TF = 0;
                }
                //Boolean chage_Center_tf = false;

                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                //progress.Visible = true; progress.Value = 0;
                //chage_Center_tf = Chang_CenterCode();  //실질적인 센터 변경이 이루어지는 메소드
                //progress.Visible = false;
                //this.Cursor = System.Windows.Forms.Cursors.Default;

                //DT.Rows.Clear();
                //DT.Columns.Clear();

                //if (chage_Center_tf == true)
                //{
                //    int RCnt = dGridView_Base.Rows.Count - 1;

                //    if (RCnt > 0)
                //    {
                //        dGridView_Base.Visible = false;
                //        //for (int TCnt = 0; TCnt <= RCnt ; TCnt++)
                //        //    dGridView_Base.Rows.Remove(dGridView_Base.Rows[0]);
                //        dGridView_Base.Visible = true;
                //    }

                //    txtFilePath.Text = "";
                //    Load_TF = 0;
                //}

            }
            else if (bt.Name == "butt_Exit")
            {
                this.Close();
            }


        }

        private Boolean Chang_CenterCode()
        {
            //if (MessageBox.Show("기존내용이 있으면 해당 엑셀내용이 추가됩니다. 계속 진행 하시겠습니까?", ""
            //  , MessageBoxButtons.YesNo) == DialogResult.No) return false;
            //if (Check_TextBox_Save_Error() == false) return false;
            //if (Delete_Data_Copy() == false) return false;
            //return false;

            //++++++++++++++++++++++++++++++++


            if (combo_Sheet.Text == "")
            {
                MessageBox.Show("엑셀자료를 올려주시기 바랍니다.");
                return false;
            }





            cls_Connect_DB Temp_Connect = new cls_Connect_DB();


            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();



            string StrSql = "";
            cls_form_Meth cm = new cls_form_Meth();

            DataTable dt = dGridView_Base.DataSource as DataTable;
            SqlBulkCopy bulkCopy = new SqlBulkCopy(cls_Connect_DB.Conn_Str);

            DataSet ds = new DataSet();
            Temp_Connect.Open_Data_Set("SELECT * FROM JDE_PROC_Member (nolock) ", "JDE_PROC_Member", ds, "", "");
            DataTable dtBulk = ds.Tables["JDE_PROC_Member"].Copy();
       

            int ReCnt1 = 0;


            foreach (DataRow row in dt.Rows)
            {
                DataRow nR = dtBulk.NewRow();



                if (row[0] == DBNull.Value || row[0].ToString() == string.Empty) continue;
                nR["MBID2"] = Convert.ToInt32(row[1]);
                nR["JDE_PROC_CODE"] = Convert.ToString(row[0]);
                //nR["PROC_COUNT"] = Convert.ToString(row[2]);
                //nR["startdate"] = Convert.ToString(row[3]);
                //nR["enddate"] = Convert.ToString(row[4]);


                cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();

                string Tsql1;
                Tsql1 = "select * from JDE_PROC_Member (nolock)  where MBID2 = '" + nR["MBID2"] + "' and JDE_PROC_CODE = '" + nR["JDE_PROC_CODE"] + "' ";
                DataSet ds1 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                Temp_Connect1.Open_Data_Set(Tsql1, base_db_name, ds1);
                int ReCnt3 = Temp_Connect1.DataSet_ReCount;
                if (ReCnt3 == 1)
                {
                    ReCnt1 = ReCnt1 + 1;
                }
            }
            //ReCnt = Temp_Connect1.DataSet_ReCount;
            if (ReCnt1 > 0)
            {
                MessageBox.Show("기존 내용에 중복되는 프로모션, 회원번호가 '" + ReCnt1 + "'개 있습니다.  ");
                if (Check_TextBox_Save_Error() == false) return false;
            }
            int a = 0;
            foreach (DataRow row in dt.Rows)
            {
                DataRow nR = dtBulk.NewRow();

                if (row[0] == DBNull.Value || row[0].ToString() == string.Empty) continue;
                nR["MBID2"] = Convert.ToInt32(row[1]);
                nR["JDE_PROC_CODE"] = Convert.ToString(row[0]);
                //nR["PROC_COUNT"] = Convert.ToString(row[2]);
                //nR["startdate"] = Convert.ToString(row[3]);
                //nR["enddate"] = Convert.ToString(row[4]);


                cls_Connect_DB Temp_Connect2 = new cls_Connect_DB();

                string Tsql2;
                Tsql2 = "select * from JDE_PROC_Member (nolock)  where mbid2 = '" + nR["MBID2"] + "' and JDE_PROC_CODE = '" + nR["JDE_PROC_CODE"] + "'";
                DataSet ds2 = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                Temp_Connect2.Open_Data_Set(Tsql2, base_db_name, ds2);
                int ReCnt2 = Temp_Connect2.DataSet_ReCount;

                if (ReCnt2 > 0)
                {
                    cls_Connect_DB Temp_Connect3 = new cls_Connect_DB();

                    string Tsql3;
                    Tsql3 = "update JDE_PROC_Member ";
                    Tsql3 = Tsql3 + "set MBID2 = '" + nR["MBID2"] + "' ,JDE_PROC_CODE = '" + nR["JDE_PROC_CODE"] + "'";
                    Tsql3 = Tsql3 + " where MBID2 = '" + nR["MBID2"] + "' and JDE_PROC_CODE = '" + nR["JDE_PROC_CODE"] + "'";
                    DataSet ds3 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect3.Open_Data_Set(Tsql3, base_db_name, ds3);


                    //dtBulk.Rows.RemoveAt(a);

                }
                else
                {

                    cls_Connect_DB Temp_Connect4 = new cls_Connect_DB();

                    string Tsql4;
                    Tsql4 = "INSERT JDE_PROC_Member ";
                    Tsql4 = Tsql4 + "VALUES('" + nR["JDE_PROC_CODE"] + "','" + nR["MBID2"] + "')";

                    DataSet ds4 = new DataSet();
                    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                    Temp_Connect4.Open_Data_Set(Tsql4, base_db_name, ds4);
                    //dtBulk.Rows.Add(nR);
                    //a = a + 1;
                }
            }

            //bulkCopy.DestinationTableName = "JDE_PROC_Member";

            //bulkCopy.WriteToServer(dtBulk);


            tran.Commit();


            MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));
            return true;
            //try
            //{



            //if (MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Q"), "", MessageBoxButtons.YesNo) == DialogResult.No) return false;

            //SqlBulkCopy bulkCopy = new SqlBulkCopy(cls_Connect_DB.Conn_Str, SqlBulkCopyOptions.KeepIdentity);

            //try
            //{
            //    if (Delete_Data_Copy() == false)
            //        return false;

            //    bulkCopy.DestinationTableName = "dbo.TBL_MEMBERINFO_ORDER";

            //    bulkCopy.WriteToServer(DT);
            //    ////작업
            //    //Save_Data_Copy();                 
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            //finally
            //{
            //    bulkCopy.Close();
            //}
            //return true;
        }


        private Boolean Delete_Data_Copy()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";
                DataSet ds = new DataSet();

                StrSql = " DELETE FROM JDE_PROC_Member ";
                Temp_Connect.Insert_Data(StrSql, "JDE_PROC_Member", Conn, tran, this.Text, this.Name);

                tran.Commit();
                return true;
            }
            catch
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
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
            //string Min_SellDate = cls_User.gid_date_time;
            //string S_SellDate = "";

            if (dGridView_Base.Rows.Count <= 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Excel_Not_Import")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                dGridView_Base.Focus(); return false;
            }

            if (txtFilePath.Text.Trim() == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Excel_Not_Import")
                     + "\n" +
                     cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txtFilePath.Focus(); return false;
            }

            return true;
        }




        private void dGridView_KeyDown(object sender, KeyEventArgs e)
        {
            //그리드일 경우에는 DEL키로 행을 삭제하는걸 막는다.
            if (sender is DataGridView)
            {
                if (e.KeyValue == 46)
                {
                    e.Handled = true;
                } // end if

            }
        }

        private void butt_Excel_Click(object sender, EventArgs e)
        {

        }

        private void combo_Pay_SelectedIndexChanged(object sender, EventArgs e)
        {
            dGridView_Base.Enabled = false;
            if (Load_TF == 0)
                return;

            if (combo_Sheet.Text != "")
            {
                try
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    loadExcelToDataGrid();

                }

                catch (System.Exception theException)
                {

                    String errorMessage;
                    errorMessage = "Error: ";
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);


                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Export_Err"));

                    if (cls_User.gid == cls_User.SuperUserID)
                        MessageBox.Show(theException.Message);
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }

        }

        private void dGridView_Base_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).CurrentCell.ColumnIndex == 0)
            {
                string Chk = "N";
                int A = 0;
                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        A = A + 1;
                    }
                }
                DataGridView T_DGv = (DataGridView)sender;
                if ((T_DGv.CurrentCell.Value == null)
                || (T_DGv.CurrentCell.Value.ToString() == ""))
                {
                    T_DGv.CurrentCell.Value = "V";
                    if (A == 0)
                    {
                        mtxtMbid.Text = T_DGv.CurrentRow.Cells[2].Value.ToString();
                        txt_promotion.Text = T_DGv.CurrentRow.Cells[1].Value.ToString();

                        mtxtMbid_2.Text = T_DGv.CurrentRow.Cells[2].Value.ToString();
                        txt_promotion_2.Text = T_DGv.CurrentRow.Cells[1].Value.ToString();


                        string Tsql = "";


                        Tsql = "Select b.m_name  ,c.PROC_NAME from JDE_PROC_Member  (nolock) a join tbl_memberinfo (nolock)  b on a.mbid2 = b.mbid2 ";
                        Tsql = Tsql + " LEFT JOIN  JDE_PROC c ON a.JDE_PROC_CODE = c.pro_code  ";
                        Tsql = Tsql + " where b.mbid2 = '" + mtxtMbid.Text + "' and c.PRO_CODE = '" + txt_promotion.Text + "'";
                        cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                        DataSet ds = new DataSet();
                        if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;

                        int ReCnt = Temp_Connect.DataSet_ReCount;

                        if (ReCnt == 0)
                        {
                            MessageBox.Show("해당 아이디로 등록된 회원이 존재하지 않습니다.");
                            Base_Clear2();
                            return;
                        }

                        txtName.Text = ds.Tables[base_db_name].Rows[0]["m_name"].ToString();
                        txt_ItemName.Text = ds.Tables[base_db_name].Rows[0]["PROC_NAME"].ToString();
                    }
                }
                else
                {
                    T_DGv.CurrentCell.Value = "";
                    mtxtMbid.Text = "";
                    txt_promotion.Text = "";
                    mtxtMbid_2.Text = "";
                    txt_promotion_2.Text = "";
                    txtName.Text = "";
                    txt_ItemName.Text = "";
                }
            }
        }
        private Boolean Check_Order_End()
        {

            string Chk = "N";

            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    Chk = "Y";

                    break;
                }
            }

            if (Chk == "N")
            {
                MessageBox.Show("선택된 내역이 없습니다.");
                return false;
            }

            return true;
        }
        private void butt_Order_End_Click(object sender, EventArgs e)
        {
            if (Check_Order_End() == false) return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            int Save_Error_Check = 0;

            Save_Order_End(ref Save_Error_Check);

            if (Save_Error_Check > 0)
            {
                dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                cgb.d_Grid_view_Header_Reset();

                Base_Grid_Set();
            }
            Base_Clear();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void Save_Order_End(ref int Save_Error_Check)
        {
            cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();
            Temp_Connect1.Connect_DB();
            //SqlConnection Conn = Temp_Connect1.Conn_Conn();
            //SqlTransaction tran = Conn.BeginTransaction();

            try
            {
                string StrSql = "";
                string mbid2 = "";
                string itemcode = "";
                string itemcount = "";
                string startdate = "";
                string enddate = "";

                cls_Search_DB csd_2 = new cls_Search_DB();

                for (int i = 0; i < dGridView_Base.Rows.Count; i++)
                {
                    if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                    {
                        mbid2 = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                        itemcode = dGridView_Base.Rows[i].Cells[1].Value.ToString();

                        StrSql = " EXEC Usp_End_JDE_PROC_Member_MOD '" + mbid2 + "', '" + itemcode + "','" + cls_User.gid + "' ";
                        DataSet ds = new DataSet();
                        if (Temp_Connect1.Open_Data_Set(StrSql, "tbl_Memberinfo", ds) == false)
                            return;


                    }
                }


                Save_Error_Check = 1;

                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save"));


            }
            catch (Exception)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Save_Err"));
            }
            finally
            {
                Temp_Connect1.Close_DB();
            }
        }
        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (Data_Set_Form_TF == 1) return;
            int Sw_Tab = 0;

            if ((sender is TextBox) == false) return;

            TextBox tb = (TextBox)sender;
            //if (tb.TextLength >= tb.MaxLength)
            //{
            //    SendKeys.Send("{TAB}");
            //    Sw_Tab = 1;
            //}



        }
        private void txtData_KeyPress(object sender, KeyPressEventArgs e)
        {

            cls_Check_Text T_R = new cls_Check_Text();

            //엔터키를 눌럿을 경우에 탭을 다음 으로 옴기기 위한 이벤트 추가
            //T_R.Key_Enter_13_tb += new Key_13_tb_Event_Handler(T_R_Key_Enter_13_tb);
            T_R.Key_Enter_13_Ncode += new Key_13_Ncode_Event_Handler(T_R_Key_Enter_13_Ncode);
            T_R.Key_Enter_13_Name += new Key_13_Name_Event_Handler(T_R_Key_Enter_13_Name);
            TextBox tb = (TextBox)sender;

            if ((tb.Tag == null) || (tb.Tag.ToString() == ""))
            {
                //쿼리문상 오류를 잡기 위함.
                if (T_R.Text_KeyChar_Check(e, tb, tb) == false)
                {
                    e.Handled = true;
                    return;
                } // end if   
            }
            else if ((tb.Tag != null) && (tb.Tag.ToString() == "1"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e, tb, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "2"))
            {
                //숫자만 입력 가능
                if (T_R.Text_KeyChar_Check(e, tb, 1) == false)
                {
                    e.Handled = true;
                    return;
                } // end if
            }

            else if ((tb.Tag != null) && (tb.Tag.ToString() == "-"))
            {
                //숫자와  - 만
                if (T_R.Text_KeyChar_Check(e, tb, "-") == false)
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
        void T_R_Key_Enter_13_Ncode(string txt_tag, TextBox tb)
        {
            if (tb.Name == "txt_promotion")
            {
                Data_Set_Form_TF = 1;


                Db_Grid_Popup(tb, txt_ItemName);
                //if (tb.Text.ToString() == "")
                //    Db_Grid_Popup(tb, txt_ItemName, "");
                //else
                //    Ncod_Text_Set_Data(tb, txt_ItemName);

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

            string And_Sql = "";
            if (tb.Name == "txt_promotion")
            {
                //cgb_Pop.Next_Focus_Control = txt_ItemCount;

                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, idx_Na_Code, "", And_Sql, 1, "");
            }
            else
            {
                cgb_Pop.Db_Grid_Popup_Make_Sql(tb, tb1_Code, idx_Na_Code, "", And_Sql);
            }

        }
        void T_R_Key_Enter_13_Name(string txt_tag, TextBox tb)
        {
            if (txt_tag != "")
            {
                int reCnt = 0;
                cls_Search_DB cds = new cls_Search_DB();
                string Search_Mbid = "";
                reCnt = cds.Member_Name_Search_S_N(ref Search_Mbid, txt_tag);

                if (reCnt == 1)
                {
                    if (tb.Name == "txtName")
                    {
                        mtxtMbid.Text = Search_Mbid; //회원명으로 검색해서 나온 사람이 한명일 경우에는 회원번호를 넣어준다.                    
                        if (Input_Error_Check(mtxtMbid, "m") == true)
                            Set_Form_Date(mtxtMbid.Text, "m");
                        SendKeys.Send("{TAB}");
                    }
                }
                else if (reCnt != 1)  //동명이인이 존재해서 사람이 많을 경우나 또는 이름 없이 엔터친 경우에.
                {

                    //cls_app_static_var.Search_Member_Name = txt_tag;
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
        void e_f_Send_Mem_Number(string Send_Number, string Send_Name)
        {
            mtxtMbid.Text = Send_Number; txtName.Text = Send_Name;
            if (Input_Error_Check(mtxtMbid, "m") == true)
                Set_Form_Date(mtxtMbid.Text, "m");
        }
        void e_f_Send_MemName_Info(ref string searchMbid, ref int searchMbid2, ref string seachName)
        {
            searchMbid = ""; searchMbid2 = 0;
            seachName = txtName.Text.Trim();
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

            //if (!(Char.IsLetter(e.KeyChar)) && e.KeyChar != 8)
            //{
            //    e.Handled = true;
            //}



            if (e.KeyChar == 13)
            {
                MaskedTextBox mtb = (MaskedTextBox)sender;

                if (mtb.Text.Replace("-", "").Replace("_", "").Trim() != "")
                {
                    int reCnt = 0;
                    cls_Search_DB cds = new cls_Search_DB();
                    string Search_Name = "";
                    reCnt = cds.Member_Name_Search_S_N(mtb.Text, ref Search_Name);

                    if (reCnt == 1)
                    {
                        if (mtb.Name == "mtxtMbid")
                        {
                            txtName.Text = Search_Name;
                            if (Input_Error_Check(mtb, "m") == true)
                                Set_Form_Date(mtb.Text, "m");
                            SendKeys.Send("{TAB}");
                        }
                    }

                    else if (reCnt > 1)  //회원번호 비슷한 사람들이 많은 경우
                    {
                        string Mbid = "";
                        int Mbid2 = 0;
                        cds.Member_Nmumber_Split(mtb.Text, ref Mbid, ref Mbid2);

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

        private void mtxtMbid_TextChanged(object sender, EventArgs e)
        {

        }

        private void Set_Form_Date(string T_Mbid, string T_sort)
        {
            //idx_Mbid = ""; idx_Mbid2 = 0;
            string Mbid = ""; int Mbid2 = 0; idx_Na_Code = "";
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

                Tsql = Tsql + " ,tbl_Memberinfo.mbid ";
                Tsql = Tsql + " ,tbl_Memberinfo.mbid2 ";
                Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";
                Tsql = Tsql + " , tbl_Memberinfo.Na_Code ";
                Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

                Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

                Tsql = Tsql + " , tbl_Memberinfo.RegTime ";
                Tsql = Tsql + " , tbl_Memberinfo.hptel ";
                Tsql = Tsql + " , tbl_Memberinfo.hometel ";

                Tsql = Tsql + " , tbl_Memberinfo.businesscode ";
                Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";

                Tsql = Tsql + " , tbl_Memberinfo.Remarks ";

                Tsql = Tsql + " , tbl_Memberinfo.LeaveDate ";
                Tsql = Tsql + " , tbl_Memberinfo.LineUserDate ";
                Tsql = Tsql + " , tbl_Memberinfo.WebID ";
                Tsql = Tsql + " , tbl_Memberinfo.WebPassWord ";
                Tsql = Tsql + " , tbl_Memberinfo.Ed_Date ";
                Tsql = Tsql + " , tbl_Memberinfo.PayStop_Date ";

                Tsql = Tsql + " , tbl_Memberinfo.For_Kind_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Sell_Mem_TF ";
                Tsql = Tsql + " , tbl_Memberinfo.Us_Num ";

                Tsql = Tsql + " , tbl_Memberinfo.CurPoint ";



                Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
                Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";

                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
                }

                //// Tsql = Tsql + " And  tbl_Memberinfo.Full_Save_TF  = 1 ";
                //Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
                //Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";


                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                Set_Form_Date(ds); //위의 DataSet객체를 가져가서 회원 정보를 넣는다
                                   //20200709
                                   //dGridView_Base_Header_Reset(); //디비그리드 헤더와 기본 셋팅을 한다.
                                   //cgb.d_Grid_view_Header_Reset();


                mtxtMbid.Focus();
            }

            Data_Set_Form_TF = 0;
        }

        private void Set_Form_Date(DataSet ds)
        {
            mtxtMbid.Text = ds.Tables[base_db_name].Rows[0]["Mbid2"].ToString();
            txtName.Text = ds.Tables[base_db_name].Rows[0]["M_Name"].ToString();
        }
        private Boolean Input_Error_Check(MaskedTextBox m_tb, string s_Kind, int Check_Leave_TF = 0)
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

            if (Mbid2 == 0) //올바르게 회원번호 양식에 맞춰서 입력햇는가.
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input_Err")
                        + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_MemNumber")
                       + "\n" +
                       cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                m_tb.Focus(); return false;
            }

            string Tsql = "";
            Tsql = "Select Mbid , Mbid2, M_Name , Sell_Mem_TF  ";
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
            //Tsql = Tsql + " And tbl_Memberinfo.BusinessCode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + cls_User.gid_CountryCode  + "') )";
            //Tsql = Tsql + " And tbl_Memberinfo.Na_Code in ( Select Na_Code From ufn_User_In_Na_Code ('" + cls_User.gid_CountryCode + "') )";

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
            else
            {
                if (Check_Leave_TF == 1)
                {
                    //if (txt_OrderNumber.Text == "") //신규 저장건에 한해서.
                    //{
                    //    //주문할려고 하는 회원이 탈퇴 회원이다
                    //    if (ds.Tables[base_db_name].Rows[0]["LeaveDate"].ToString() != "")
                    //    {

                    //        MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Mem_Leave_Sell")
                    //        + "\n" +
                    //        cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                    //        m_tb.Focus(); return false;
                    //    }
                    //}
                }
            }
            //++++++++++++++++++++++++++++++++            

            return true;
        }

        private void butt_Save_Click(object sender, EventArgs e)
        {

            if (Base_Error_Check__01() == false) return;
            bool check = false;

            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    mbid2 = dGridView_Base.Rows[i].Cells[2].Value.ToString();
                    itemcode = dGridView_Base.Rows[i].Cells[1].Value.ToString();

                    check = true;
                    break;
                }
            }
            if (mbid2 == null && itemcode == null)
            {
                mbid2 = mtxtMbid.Text;
                itemcode = txt_promotion.Text;
            }
            string mbid2_2 = mbid2;
            string itemcode_2 = itemcode;
            //중복값이면 업데이트
            string Tsql = "";

            Tsql = " SELECT * from JDE_PROC_Member  (nolock) where mbid2 = '" + mbid2 + "' and JDE_PROC_CODE = '" + itemcode + "' ";

            cls_form_Meth cm = new cls_form_Meth();
            //cm._chang_base_caption_search(m_text);

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            Temp_Connect.Connect_DB();
            SqlConnection Conn = Temp_Connect.Conn_Conn();
            SqlTransaction tran = Conn.BeginTransaction();

            DataSet ds = new DataSet();
            try
            {
                string StrSql = "";
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect1.Open_Data_Set(Tsql, base_db_name, ds, this.Name, this.Text) == false) return;
                int ReCnt = Temp_Connect1.DataSet_ReCount;

                if (ReCnt > 0)
                {

                    if (check == true)
                    {

                        StrSql = " update  JDE_PROC_Member set mbid2 = '" + mtxtMbid.Text + "', JDE_PROC_CODE = '" + txt_promotion.Text + "'  where mbid2= '" + mbid2_2 + "' AND JDE_PROC_CODE = '" + itemcode_2 + "'";
                        Temp_Connect.Insert_Data(StrSql, "JDE_PROC_Member", Conn, tran, this.Text, this.Name);

                        tran.Commit();
                    }
                    else
                    {

                        MessageBox.Show("이미 해당 회원번호와 프로모션으로 등록 되어 있습니다.");
                        Base_Clear();
                        return;
                    }
                }
                else
                {
                    StrSql = " insert into JDE_PROC_Member values ('" + txt_promotion.Text + "','" + mtxtMbid.Text + "')";
                    Temp_Connect.Insert_Data(StrSql, "JDE_PROC_Member", Conn, tran, this.Text, this.Name);

                    tran.Commit();
                }

                tran.Dispose();
                Temp_Connect.Close_DB();
                MessageBox.Show("저장이 완료 되었습니다.");
                Base_Clear();


            }
            catch (Exception)
            {
                tran.Rollback();
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Base_Del_Err"));
            }

            finally
            {
                mbid2_2 = "";
                itemcode_2 = "";
                check = false;
            }

        }

        private bool Base_Error_Check__01(int SellCode_TF = 0)
        {
            //회원
            if (txtName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Mem")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                mtxtMbid.Focus(); return false;
            }
            //아이템코드
            if (txt_promotion.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_promotion.Focus(); return false;
            }
            //아이템이름
            if (txt_ItemName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_promotion.Focus(); return false;
            }
            //아이템수량
            if (txt_ItemName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_promotion.Focus(); return false;
            }
            //시작날짜
            if (txt_ItemName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_promotion.Focus(); return false;
            }
            //종료날짜
            if (txt_ItemName.Text == "")
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Not_Input")
                       + "-" + cls_app_static_var.app_msg_rm.GetString("Msg_Sort_Goods")
                      + "\n" +
                      cls_app_static_var.app_msg_rm.GetString("Msg_Re_Action"));
                txt_promotion.Focus(); return false;
            }
            string Chk = "N";

            int A = 0;
            for (int i = 0; i < dGridView_Base.Rows.Count; i++)
            {
                if (dGridView_Base.Rows[i].Cells[0].Value.ToString() == "V")
                {
                    A = A + 1;
                }
            }

            if (A > 1)
            {
                MessageBox.Show("다중선택시 수정 및 삽입을 할 수 없습니다.");
                return false;
            }


            return true;
        }
        private void DTP_Base_CloseUp(object sender, EventArgs e)
        {
            cls_form_Meth ct = new cls_form_Meth();
            ct.form_DateTimePicker_Search_TextBox(this, (DateTimePicker)sender);
        }

    }



}
