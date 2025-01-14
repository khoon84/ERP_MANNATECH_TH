﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Data;
using System.Drawing;

namespace MLM_Program
{
    class cls_Grid_Base
    {
        public int grid_col_Count;
        public int[] grid_col_w;
        public int[] grid_col_h;
        public Boolean[] grid_col_Lock;
        public string[] grid_col_header_text;
        public string[] grid_col_name;
        public DataGridViewContentAlignment[] grid_col_alignment;
        public DataGridViewColumnSortMode[] grid_col_SortMode;
        public Dictionary<int, string[]> grid_name;
        public Dictionary<int, object[]> grid_name_obj;
        public Dictionary<int, string> grid_cell_format;
        public DataGridView basegrid;
        public DataGridViewSelectionMode grid_select_mod;
        public Boolean grid_Merge;
        public DataGridViewAutoSizeColumnsMode grid_Auto_Size_Mod;
        public Color[] gric_col_Color;
        public int grid_Merge_Col_Start_index;
        public int grid_Merge_Col_End_index;
        public int grid_Frozen_End_Count;
        public int RowTemplate_Height;
        public int Sort_Mod_Auto_TF;
        

        public void Grid_Base_Arr_Clear()
        {
            grid_col_w = null;
            grid_col_h = null;
            grid_col_Lock = null;
            grid_col_header_text = null;
            grid_col_name = null;
            grid_col_alignment = null;
            grid_col_SortMode = null; 
            grid_name = null;
            grid_name_obj = null;
            grid_cell_format = null;
            gric_col_Color = null; 
            

            grid_Merge_Col_Start_index = 0;
            grid_Merge_Col_End_index = 0;
            grid_Frozen_End_Count = 0;
            RowTemplate_Height = 0;
        }

        /*데이터바인딩 속도 빠르게 하기 위해서 새로 그리드뷰 셋팅하는 함수*/
        public void d_Grid_view_DataSource_Header_Reset(DataGridView dgv, string[] HeaderText_Arr, int[] Width_Arr
            , Boolean[] ReadOnly_Arr, DataGridViewContentAlignment[] Align_Arr, int frozenCnt
            , int Autosize_TF = 0)
        {
            cls_form_Meth cm = new cls_form_Meth();

            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                dgv.Columns[i].HeaderText = cm._chang_base_caption_search(HeaderText_Arr[i]);

                if (Width_Arr[i] == 0)
                    dgv.Columns[i].Visible = false;
                else
                    dgv.Columns[i].Width = Width_Arr[i];


                if (Autosize_TF == 0)
                    dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                //int Col_Width = dgv.Columns[i].Width;
                //dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //dgv.Columns[i].Width = Col_Width;

                dgv.Columns[i].ReadOnly = ReadOnly_Arr[i];
                dgv.Columns[i].DefaultCellStyle.Alignment = Align_Arr[i];
            }

            dgv.CellPainting += new DataGridViewCellPaintingEventHandler(dGridView_Base_CellPainting);

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("굴림", float.Parse("8.4"));
            dgv.RowTemplate.Height = 20;
            dgv.ColumnHeadersHeight = 22;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.ShowCellToolTips = false;
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            if (dgv.Columns.Count >= 1)
            {
                for (int Cnt = 0; Cnt < frozenCnt; Cnt++)
                {
                    dgv.Columns[Cnt].Frozen = true;
                }
            }
        }


        public void d_Grid_view_Header_Reset()
        {
            //basegrid.Visible = false;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = grid_col_Count;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = grid_select_mod;

            basegrid.RowTemplate.Height = 20;
            if (RowTemplate_Height > 0)
                basegrid.RowTemplate.Height = RowTemplate_Height;


            basegrid.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));
            basegrid.ColumnHeadersHeight = 22;
            
            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            //basegrid.EnableHeadersVisualStyles = false;
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);
            
            
            basegrid.BorderStyle = BorderStyle.FixedSingle ;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single  ;
            
            basegrid.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color;
            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            basegrid.ColumnHeadersHeight = 20;

            basegrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);
                        

            if (grid_Frozen_End_Count > 0)
            {
                for (int Cnt = 0; Cnt < grid_Frozen_End_Count; Cnt++)
                {
                    basegrid.Columns[Cnt].Frozen = true;
                }
            }

            if (basegrid.RowHeadersVisible == true)
                basegrid.CellPainting += new DataGridViewCellPaintingEventHandler(dGridView_Base_CellPainting);

            if (grid_Merge == true)
            {
                this.basegrid.Paint += new PaintEventHandler(dGridView_Base_Paint);

                this.basegrid.Scroll += new ScrollEventHandler(dGridView_Base_Scroll);
            }



            int i = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (grid_col_header_text != null)
            {
                foreach (string t_ext in grid_col_header_text)
                {
                    basegrid.Columns[i].HeaderText = cm._chang_base_caption_search(t_ext);
                    //basegrid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;                     
                    i++;
                }
            }

 			i = 0;
            if (grid_col_name != null)
            {
                foreach (string t_ext in grid_col_name)
                {
                    basegrid.Columns[i].Name = t_ext;
                    
                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    i++;
                }
            }
			
            i = 0;
            if (grid_col_SortMode != null)
            {
                foreach (DataGridViewColumnSortMode t_wi in grid_col_SortMode)
                {
                    basegrid.Columns[i].SortMode = t_wi;
                    i++;
                }
            }

            i = 0;
            if (gric_col_Color != null)
            {
                foreach (Color t_color in gric_col_Color)
                {
                    basegrid.Columns[i].DefaultCellStyle.BackColor = t_color;
                    i++;
                }
            }

            

            if (grid_Auto_Size_Mod != 0 )
                basegrid.AutoSizeColumnsMode  = grid_Auto_Size_Mod;

            //foreach (DataGridViewHeaderCell header in basegrid.Rows)
            //{
            //    header.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //}



            i = 0;
            if (grid_col_w != null)
            {
                foreach (int t_wi in grid_col_w)
                {
                    basegrid.Columns[i].Width = t_wi;

                    if (t_wi == 0) basegrid.Columns[i].Visible = false;                    
                    i++;
                }
            }

            i = 0;
            if (grid_col_Lock != null)
            {
                foreach (Boolean t_ro in grid_col_Lock)
                {
                    basegrid.Columns[i].ReadOnly = t_ro;
                    i++;
                }
            }



            i = 0;
            if (grid_col_alignment != null)
            {
                foreach (DataGridViewContentAlignment t_ai in grid_col_alignment)
                {
                    basegrid.Columns[i].DefaultCellStyle.Alignment = t_ai;                    
                    i++;
                }
            }

            i = 0;
            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    basegrid.Columns[t_for_key].DefaultCellStyle.Format = grid_cell_format[t_for_key];
                    i++;
                }
            }


            basegrid.AllowUserToAddRows = false;

            //basegrid.Visible = true;
            //basegrid.Refresh();
            //   System.Globalization.NumberStyles.AllowThousands.ToString ()   ;
        }


        public void d_Grid_view_Header_Reset(int Start_F)
        {
           // basegrid.Visible = false;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = grid_col_Count;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = grid_select_mod;
            basegrid.RowTemplate.Height = 20;

            basegrid.DefaultCellStyle.Font = new System.Drawing.Font("돋움", float.Parse("8.4"));
            

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            
            //basegrid.EnableHeadersVisualStyles = false;
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            basegrid.RowHeadersDefaultCellStyle.SelectionBackColor = cls_app_static_var.Button_Parent_Color  ;
            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        
            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(208, 222, 176);
            //basegrid.BorderStyle = BorderStyle.Fixed3D;
            //basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            //basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            basegrid.ColumnHeadersHeight = 20;
            
            basegrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            if (grid_Frozen_End_Count > 0)
            {
                for (int Cnt = 0; Cnt < grid_Frozen_End_Count; Cnt++)
                {
                    basegrid.Columns[Cnt].Frozen = true;
                }
            }

          
            int i = 0;
            cls_form_Meth cm = new cls_form_Meth();

            if (grid_col_header_text != null)
            {
                foreach (string t_ext in grid_col_header_text)
                {
                    basegrid.Columns[i].HeaderText = cm._chang_base_caption_search(t_ext);
                    //basegrid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                    if (Sort_Mod_Auto_TF != 0 )
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;                     
                    //basegrid.Columns[i].a  DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; 
                    i++;
                }

            }

            i = 0;
            if (grid_col_name != null)
            {
                foreach (string t_ext in grid_col_name)
                {
                    basegrid.Columns[i].Name = t_ext;

                    if (Sort_Mod_Auto_TF != 0)
                        basegrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    i++;
                }
            }

            i = 0;
            if (grid_col_SortMode != null)
            {
                foreach (DataGridViewColumnSortMode t_wi in grid_col_SortMode)
                {
                    basegrid.Columns[i].SortMode = t_wi;
                    i++;
                }
            }

            if (grid_Auto_Size_Mod != 0)
                basegrid.AutoSizeColumnsMode = grid_Auto_Size_Mod;

            i = 0;
            if (grid_col_w != null)
            {
                foreach (int t_wi in grid_col_w)
                {
                    basegrid.Columns[i].Width = t_wi;

                    if (t_wi == 0) basegrid.Columns[i].Visible = false;
                    i++;
                }
            }

            i = 0;
            if (gric_col_Color != null)
            {
                foreach (Color t_color in gric_col_Color)
                {
                    basegrid.Columns[i].DefaultCellStyle.BackColor = t_color;
                    i++;
                }
            }


            i = 0;
            if (grid_col_Lock != null)
            {
                foreach (Boolean t_ro in grid_col_Lock)
                {
                    basegrid.Columns[i].ReadOnly = t_ro;
                    i++;
                }
            }



            i = 0;
            if (grid_col_alignment != null)
            {
                foreach (DataGridViewContentAlignment t_ai in grid_col_alignment)
                {
                    basegrid.Columns[i].DefaultCellStyle.Alignment = t_ai;
                    i++;
                }
            }

            i = 0;
            if (grid_cell_format != null)
            {
                foreach (int t_for_key in grid_cell_format.Keys)
                {
                    basegrid.Columns[t_for_key].DefaultCellStyle.Format = grid_cell_format[t_for_key];
                    i++;
                }
            }


            basegrid.AllowUserToAddRows = false;
           // basegrid.Visible = true;
           // basegrid.Refresh();
        }



        void basegrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                // Try to sort based on the cells in the current column.
                e.SortResult = System.String.Compare(
                    e.CellValue1.ToString().Replace(",", ""), e.CellValue2.ToString().Replace(",", ""));

                // If the cells are equal, sort based on the ID column.
                if (e.SortResult == 0 && e.Column.Name != "ID")
                {
                    e.SortResult = System.String.Compare(
                        (sender as DataGridView).Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                        (sender as DataGridView).Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                }
                e.Handled = true;



            }
            catch (Exception ec)
            {

            }
        }



        public void db_grid_Data_Put()
        {
            //basegrid.Visible = false;
            foreach (int t_key in grid_name.Keys)
            {
                basegrid.Rows.Add(grid_name[t_key]);
            }

            basegrid.AllowUserToAddRows = false;
            //basegrid.Visible = true;
            basegrid.Refresh();
        }

        public void db_grid_Obj_Data_Put()
        {
            //basegrid.Visible = false;
            foreach (int t_key in grid_name_obj.Keys)
            {
                basegrid.Rows.Add(grid_name_obj[t_key]);
            }
            basegrid.AllowUserToAddRows = false;
            //basegrid.Visible = true;
           // basegrid.Refresh();
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


        // 셀병합과 관련해서  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>   
        private void dGridView_Base_Scroll(object sender, ScrollEventArgs e)
        {
            System.Drawing.Rectangle rtHeader = this.basegrid.DisplayRectangle;
            this.basegrid.Invalidate(rtHeader, true);
        }


        private void dGridView_Base_Paint(object sender, PaintEventArgs e)
        {
            if (basegrid.RowCount == 0) return;
            Chang_Cell_Merge_001(sender, e);
        }


        private void Chang_Cell_Merge_001(object sender, PaintEventArgs e)
        {
            
            int Call_Cnt = 0;

            

            for (int ColCnt = grid_Merge_Col_Start_index; ColCnt <= grid_Merge_Col_End_index; ColCnt++)
            {
                int Strar_Row = 0; int End_Row = 0; string Mer_String = "";
                string Cur_string = ""; int Meg_Cnt = 0; int send_End_Row = 0;

                Mer_String = basegrid.Rows[0].Cells[ColCnt].Value.ToString();
                Cur_string = basegrid.Rows[0].Cells[ColCnt].Value.ToString();
                Call_Cnt = 0;

                for (int j = 0; j < this.basegrid.RowCount; j++)
                {
                    Cur_string = basegrid.Rows[j].Cells[ColCnt].Value.ToString();



                    if (Mer_String == Cur_string)
                        Meg_Cnt++;
                    else
                    {

                        End_Row = j - 1;
                        if (Meg_Cnt > 1)
                            Call_Cnt++;
                        Chang_Cell_Merge_002(sender, e, ColCnt, Strar_Row, Strar_Row + Meg_Cnt, Mer_String, Meg_Cnt, Call_Cnt);

                        Mer_String = Cur_string;
                        End_Row = 0;
                        Strar_Row = j;
                        Meg_Cnt = 0;
                    }
                }

                if (Meg_Cnt > 1)
                    Call_Cnt++;

                send_End_Row = Meg_Cnt;
                if (basegrid.RowCount <= send_End_Row)
                    send_End_Row = (basegrid.RowCount - 1);

                Chang_Cell_Merge_002(sender, e, ColCnt, Strar_Row, Strar_Row + send_End_Row, Mer_String, Meg_Cnt, Call_Cnt);
            }
        }


        private void Chang_Cell_Merge_002(object sender, PaintEventArgs e, int ColCnt, int Strar_Row, int End_Row, string Mer_String, int Meg_Cnt, int Call_Cnt)
        {
            if (basegrid.RowCount <= 1) return;

            //try
            //{
                DataGridView dgv = (DataGridView)sender;

                System.Drawing.Rectangle r1 = this.basegrid.GetCellDisplayRectangle(ColCnt, Strar_Row, true);

                System.Drawing.Rectangle r2 = this.basegrid.GetCellDisplayRectangle(ColCnt, End_Row, true);

                //int H2 = this.basegrid.GetCellDisplayRectangle(ColCnt, End_Row, true).Height * (End_Row - Strar_Row + 1);

                if ((r1.Y <= 0) && (r2.Y <= 0)) return;
                if (r1.X <= 0) r1.X = r2.X;
                if (r1.Width <= 0) r1.Width = r2.Width;

                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width - 2;




                if (Strar_Row == 0)
                    if (r2.Y == 0)
                        r1.Height = dgv.Height - r1.Y - r1.Height - 2;
                    else
                        if ((End_Row + 1) == basegrid.RowCount)
                        {
                            r1.Height = (r2.Y + r2.Height) - r1.Y - 2;
                        }
                        else
                        {
                            if (r1.Y <= (1))
                            {
                                r1.Y = r2.Height;
                                r1.Height = (r2.Y + r2.Height) - r2.Height - r2.Height - 2;
                            }
                            else
                                r1.Height = (r2.Y + r2.Height) - r1.Y - r1.Height - 2;
                        }
                else
                    if (r2.Y == 0)
                        r1.Height = dgv.Height - r1.Y - 2;
                    else
                        if (r1.Y <= (1))
                        {
                            r1.Y = this.basegrid.ColumnHeadersHeight + 1;
                            r1.Height = (r2.Y + r2.Height) - this.basegrid.ColumnHeadersHeight - 2;
                        }
                        else
                            r1.Height = (r2.Y + r2.Height) - r1.Y - 2;

                //r1.

                //e.Graphics.FillRectangle(new System.Drawing.SolidBrush(this.basegrid.ColumnHeadersDefaultCellStyle.BackColor), r1);
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(this.basegrid.BackgroundColor), r1);

                System.Drawing.StringFormat format = new System.Drawing.StringFormat();

                format.Alignment = System.Drawing.StringAlignment.Center;
                format.LineAlignment = System.Drawing.StringAlignment.Center;


                e.Graphics.DrawString(Mer_String,
                this.basegrid.ColumnHeadersDefaultCellStyle.Font,
                new System.Drawing.SolidBrush(this.basegrid.ColumnHeadersDefaultCellStyle.ForeColor),
                r1,
                format);
            //}

            //catch (Exception ec)
            //{
            //    return;
            //}

        }
        // 셀병합과 관련해서  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>          



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


    }// end cls_Grid_Base



    class cls_Grid_Base_Popup
    {
        public DataGridView basegrid;
        public object Base_tb;        
        public TextBox Base_tb_2;
        public object Base_Location_obj;
        public Form Base_fr;
        public Boolean Change_Header_Text_TF;
        public Dictionary<string, TextBox> Base_text_dic;
        public Control Next_Focus_Control;

        public void db_grid_Popup_Base(int gridCnt, string headerText_1, string headerText_2, string FieldName_1, string FieldName_2, string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;

            

            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;
            

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            //basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.RowTemplate.Height = 20;
            basegrid.ColumnHeadersHeight = 19;

            //dGridView_Base_Header_Reset(headerText_1, headerText_2);
            
            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2);

            dGridView_Base_Header_Reset(headerText_1, headerText_2);    // 240308 - 허성윤, db_grid_Popup_SetDate() 함수 뒤에 실행되도록 위치 조정.

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            //cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);

            basegrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   //240308 허성윤 추가
            

            basegrid.BringToFront();
            basegrid.RowHeadersVisible =true ;
            //basegrid.row  .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }

        public void db_grid_Popup_Base(int gridCnt, string headerText_1, string headerText_2, string FieldName_1, string FieldName_2, string Tsql, int MemberSort)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.BorderStyle = BorderStyle.FixedSingle;
            basegrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.RowTemplate.Height = 20;
            basegrid.ColumnHeadersHeight = 19;

            dGridView_Base_Header_Reset(headerText_1, headerText_2);
            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2);
         
            //cls_form_Meth cfm = new cls_form_Meth();
           // cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }



        public void db_grid_Popup_Base(int gridCnt,
                                        string headerText_1, string headerText_2, string headerText_3, string headerText_4,
                                        string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4,
                                        string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2,headerText_3,headerText_4);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2,FieldName_3,FieldName_4);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
           // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

           // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }

        public void db_grid_Popup_Base_member(int gridCnt,
                                      string headerText_1, string headerText_2, string headerText_3, string headerText_4,
                                      string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4,
                                      string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2, headerText_3, headerText_4);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2, FieldName_3, FieldName_4);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            // basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }


        void basegrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            try
            {
                // Try to sort based on the cells in the current column.
                e.SortResult = System.String.Compare(e.CellValue1.ToString().Replace(",", ""), e.CellValue2.ToString().Replace(",", ""));

                // If the cells are equal, sort based on the ID column.
                if (e.SortResult == 0 && e.Column.Name != "ID")
                {
                    e.SortResult = System.String.Compare(
                        (sender as DataGridView).Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                        (sender as DataGridView).Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                }
                e.Handled = true;



            }
            catch (Exception ec)
            {

            }
        }


        public void db_grid_Popup_Base(int gridCnt,
                                       string headerText_1, string headerText_2, string headerText_3, string headerText_4, string headerText_5,
                                       string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5,
                                       string Tsql)
        {
            basegrid.Tag = Base_tb_2.Name;
            basegrid.Rows.Clear();
            basegrid.ColumnCount = gridCnt;
            basegrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //basegrid.EnableHeadersVisualStyles = false;

            basegrid.GridColor = System.Drawing.Color.Black;
            basegrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightSkyBlue;
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 172, 142);
            basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            basegrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            ////basegrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(199, 220, 175);
            ////basegrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 241, 220);
            ////basegrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;


            dGridView_Base_Header_Reset(headerText_1, headerText_2, headerText_3, headerText_4, headerText_5);

            db_grid_Popup_SetDate(Tsql, FieldName_1, FieldName_2, FieldName_3, FieldName_4, FieldName_5);

            basegrid.DoubleClick += new System.EventHandler(dGridView_Base_DoubleClick);
            basegrid.KeyDown += new KeyEventHandler(dGridView_KeyDown);
            //basegrid.SortCompare += new DataGridViewSortCompareEventHandler(basegrid_SortCompare);

            // cls_form_Meth cfm = new cls_form_Meth();
            //cfm.form_Group_Panel_Enable_False(Base_fr);


            basegrid.BringToFront();
            basegrid.RowHeadersVisible = true;
            basegrid.BackgroundColor = System.Drawing.Color.White;
            basegrid.Visible = true;
            basegrid.Focus();
        }


        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2)
        {
            db_grid_Popup_Location();

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 100;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 150;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }




        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2, string headerText_3 , string headerText_4)
        {
            db_grid_Popup_Location(1);

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);
            basegrid.Columns[2].HeaderText = cm._chang_base_caption_search(headerText_3);
            basegrid.Columns[3].HeaderText = cm._chang_base_caption_search(headerText_4);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 200;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 90;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            if (headerText_3 != "")
                basegrid.Columns[2].Width = 70;
            else
            {
                basegrid.Columns[2].Width = 0;
                basegrid.Columns[2].Visible = false;
            }

            if (headerText_4 != "")
                basegrid.Columns[3].Width = 70;
            else
            {
                basegrid.Columns[3].Width = 0;
                basegrid.Columns[3].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[2].ReadOnly = true;
            basegrid.Columns[3].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dGridView_Base_Header_Reset(string headerText_1, string headerText_2, string headerText_3, string headerText_4, string headerText_5)
        {
            db_grid_Popup_Location(1);

            cls_form_Meth cm = new cls_form_Meth();
            basegrid.Columns[0].HeaderText = cm._chang_base_caption_search(headerText_1);
            basegrid.Columns[1].HeaderText = cm._chang_base_caption_search(headerText_2);
            basegrid.Columns[2].HeaderText = cm._chang_base_caption_search(headerText_3);
            basegrid.Columns[3].HeaderText = cm._chang_base_caption_search(headerText_4);
            basegrid.Columns[4].HeaderText = cm._chang_base_caption_search(headerText_5);

            if (headerText_1 != "")
                basegrid.Columns[0].Width = 200;
            else
            {
                basegrid.Columns[0].Width = 0;
                basegrid.Columns[0].Visible = false;
            }

            if (headerText_2 != "")
                basegrid.Columns[1].Width = 90;
            else
            {
                basegrid.Columns[1].Width = 0;
                basegrid.Columns[1].Visible = false;
            }

            if (headerText_3 != "")
                basegrid.Columns[2].Width = 70;
            else
            {
                basegrid.Columns[2].Width = 0;
                basegrid.Columns[2].Visible = false;
            }

            if (headerText_4 != "")
                basegrid.Columns[3].Width = 70;
            else
            {
                basegrid.Columns[3].Width = 0;
                basegrid.Columns[3].Visible = false;
            }

            if (headerText_5 != "")
                basegrid.Columns[4].Width = 70;
            else
            {
                basegrid.Columns[4].Width = 0;
                basegrid.Columns[4].Visible = false;
            }

            basegrid.Columns[0].ReadOnly = true;
            basegrid.Columns[1].ReadOnly = true;
            basegrid.Columns[2].ReadOnly = true;
            basegrid.Columns[3].ReadOnly = true;
            basegrid.Columns[4].ReadOnly = true;
            basegrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            basegrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            basegrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            basegrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }


        private void db_grid_Popup_Location()
        {
            if ((Base_Location_obj is TextBox) == true)
            {
                TextBox tb = (TextBox)Base_Location_obj;
                Control t_cn = tb.Parent ;
                int t_Left = 0;    int t_Top = 0;

                while (t_cn.Name != Base_fr.Name)
                {
                    t_Left = t_Left + t_cn.Left;
                    t_Top = t_Top + t_cn.Top;

                    t_cn = t_cn.Parent;                    
                }

                basegrid.Top = tb.Top + t_Top + 27;
                basegrid.Left = tb.Left + t_Left - 5;
    
            }

            if ((Base_Location_obj is Button) == true)
            {
                Button tb = (Button)Base_Location_obj;
                basegrid.Top = tb.Parent.Top + tb.Top + 27;
                basegrid.Left = tb.Parent.Left + tb.Left - 5;
            }
            basegrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);   //240308 허성윤 추가
            
            int FormWitdh = 150;
            foreach (DataGridViewColumn col in basegrid.Columns)
            {
                FormWitdh += col.Width;
            }

            basegrid.Width = FormWitdh;
            basegrid.Height = 300;
        }

        private void db_grid_Popup_Location(int TT)
        {
            if ((Base_Location_obj is TextBox) == true)
            {
                TextBox tb = (TextBox)Base_Location_obj;
                Control t_cn = tb.Parent ;
                int t_Left = 0; int t_Top = 0;

                while (t_cn.Name != Base_fr.Name)
                {
                    t_Left = t_Left + t_cn.Left;
                    t_Top = t_Top + t_cn.Top;

                    t_cn = t_cn.Parent;                    
                }

                basegrid.Top = tb.Top + t_Top + 27;
                basegrid.Left = tb.Left + t_Left - 5;              
            }

            if ((Base_Location_obj is Button) == true)
            {
                Button tb = (Button)Base_Location_obj;
                basegrid.Top = tb.Parent.Top + tb.Top + 27;
                basegrid.Left = tb.Parent.Left + tb.Left - 5;
            }

            int gridWitdh = 65;
            foreach(DataGridViewColumn col in basegrid.Columns)
            {
                gridWitdh += col.Width;
            }

            basegrid.Width = gridWitdh;
            basegrid.Height = 300;
        }



        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2)
        {
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());

                string[] row0 = { str_1 , 
                                  str_2                               
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() , 
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString()                                
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }




        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2, FieldName_3, FieldName_4);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }

        private void db_grid_Popup_SetDate(string Tsql, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5)
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "TempTable", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;
            if (ReCnt == 0) return;

            Dictionary<int, string[]> gr_dic_text = new Dictionary<int, string[]>();

            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                Set_gr_dic(ref ds, ref gr_dic_text, fi_cnt, FieldName_1, FieldName_2, FieldName_3, FieldName_4, FieldName_5);  //데이타를 배열에 넣는다.
            }

            db_grid_Data_Put(gr_dic_text);
        }


        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            //string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice)
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = ""; string str_3 = ""; string str_4 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());
                str_3 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3].ToString());
                str_4 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4].ToString());

                string[] row0 = { str_1 , 
                                  str_2 ,                              
                                  str_3 ,
                                  str_4 ,
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() , 
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString() ,                               
                                  string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3]) ,                               
                                  encrypter.Decrypt (string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4]))            
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }

        private void Set_gr_dic(ref DataSet ds, ref Dictionary<int, string[]> gr_dic_text, int fi_cnt, string FieldName_1, string FieldName_2, string FieldName_3, string FieldName_4, string FieldName_5)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            //string.Format(cls_app_static_var.str_Currency_Type, SalesDetail[OrderNumber].TotalInputPrice)
            if (Change_Header_Text_TF == true)
            {
                string str_1 = ""; string str_2 = ""; string str_3 = ""; string str_4 = ""; string str_5 = "";

                cls_form_Meth cm = new cls_form_Meth();
                str_1 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString());
                str_2 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString());
                str_3 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3].ToString());
                str_4 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4].ToString());
                str_5 = cm._chang_base_caption_search(ds.Tables["TempTable"].Rows[fi_cnt][FieldName_5].ToString());

                string[] row0 = { str_1 , 
                                  str_2 ,                              
                                  str_3 ,
                                  str_4 ,
                                   str_5 ,
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }
            else
            {
                string[] row0 = { ds.Tables["TempTable"].Rows[fi_cnt][FieldName_1].ToString() , 
                                  ds.Tables["TempTable"].Rows[fi_cnt][FieldName_2].ToString() ,                               
                                  string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["TempTable"].Rows[fi_cnt][FieldName_3]) ,                               
                                  // 20200806 왜 DEcrypt를 할까? 의미없어보임 encrypter.Decrypt (string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4])),
                                  string.Format(cls_app_static_var.str_Currency_Type,ds.Tables["TempTable"].Rows[fi_cnt][FieldName_4]),
                                  string.Format(cls_app_static_var.str_Currency_Type, ds.Tables["TempTable"].Rows[fi_cnt][FieldName_5]) 
                                };

                gr_dic_text[fi_cnt + 1] = row0;
            }


        }





        private void db_grid_Data_Put(Dictionary<int, string[]> gr_dic_text)
        {
            foreach (int t_key in gr_dic_text.Keys)
            {
                basegrid.Rows.Add(gr_dic_text[t_key]);
            }

            basegrid.AllowUserToAddRows = false;
        }


        private void dGridView_Base_DoubleClick(object sender, EventArgs e)
        {
            DataGridView T_Gd = (DataGridView)sender;

            if (T_Gd.CurrentRow?.Cells[0].Value != null)
            {
                if (Base_text_dic != null)
                {
                    TextBox tttbb = null;
                    int fCnt = 0;
                    foreach (string t_key in Base_text_dic.Keys)
                    {
                        Base_text_dic[t_key].Text = T_Gd.CurrentRow.Cells[fCnt].Value.ToString();

                        if (fCnt == 0)
                            tttbb = (TextBox)Base_text_dic[t_key];
                        fCnt++;
                    }

                    basegrid.Visible = false;
                    basegrid.Dispose();

                    //cls_form_Meth cfm = new cls_form_Meth();
                    //cfm.form_Group_Panel_Enable_True(Base_fr);

                    if (Next_Focus_Control == null)
                    {
                        tttbb.Focus();
                        Control tb21 = Base_fr.GetNextControl(Base_fr.ActiveControl, true);
                        if (tb21 != null) tb21.Focus();
                    }
                    else
                        Next_Focus_Control.Focus();
                }
                else
                {
                    if ((Base_tb is TextBox) == true)
                    {
                        TextBox tb = (TextBox)Base_tb;
                        tb.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                        //tb.Focus();
                    }

                    if ((Base_tb is MaskedTextBox) == true)
                    {
                        MaskedTextBox tb = (MaskedTextBox)Base_tb;
                        tb.Text = T_Gd.CurrentRow.Cells[0].Value.ToString();
                    }
                    Base_tb_2.Text = T_Gd.CurrentRow.Cells[1].Value.ToString();


                    basegrid.Visible = false;
                    basegrid.Dispose();

                    //cls_form_Meth cfm = new cls_form_Meth();
                    //cfm.form_Group_Panel_Enable_True(Base_fr);

                    if (Next_Focus_Control == null)
                    {
                        TextBox tb2 = (TextBox)Base_Location_obj;
                        Control t_Parent = tb2.Parent;
                                                
                        tb2.Focus();                        
                        
                        Control tb21 = Base_fr.GetNextControl(Base_fr.ActiveControl, true);
                        if (tb21 != null)  tb21.Focus();
                        }
                    else
                        Next_Focus_Control.Focus();

                    //tb2.SelectNextControl(Base_fr.ActiveControl, true, true, false, true);
                    //TextBox tb21 = (TextBox)tb2.Parent.GetNextControl(tb2.Parent, true);
                    //tb21.Focus();
                }
            }
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

                if (e.KeyValue == 13)
                {
                    dGridView_Base_DoubleClick(sender, e);
                }
            }
        }



        public void Db_Grid_Popup_Make_Sql(TextBox tb, TextBox tb1_Code, string Base_Na_Code, string T_SellDate = "", string And_Sql = "" , int io_TF = 1 , string EtcCode = "")
        {
            //cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            //DataGridView Popup_gr = new DataGridView();
            //Popup_gr.Name = "Popup_gr";
            //tfr.Controls.Add(Popup_gr);
            //cgb_Pop.basegrid = Popup_gr;
            //cgb_Pop.Base_fr = tfr;
            //cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            //cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            //cgb_Pop.Base_Location_obj = tb;

            string Tsql = "" ;

            //if (Base_Na_Code == "")
            //    Base_Na_Code = "KR";


            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter5")
            {                
                Tsql = "Select Ncode , Name  ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";

                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";

                    Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + Base_Na_Code + "') )";
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {
                    Tsql = Tsql + " And  Ncode in ( Select Center_Code From ufn_User_In_Center ('" + cls_User.gid_CenterCode + "','" + Base_Na_Code + "') )";
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (Base_fr.Name == "frmMember" || Base_fr.Name == "frmSell")
                {
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                }
                if (Base_fr.Name == "frmMember_Update" || Base_fr.Name == "frmMember_UpdateSelect" || // 2019-04-15 구현호 센터관리 회원관련 센터보여주기에 맞춰서 나옴
                    Base_fr.Name == "frmMember_Select" || Base_fr.Name == "frmMember_Center_Change" || Base_fr.Name == "frmMember_Select_Not_Sell"
                     || Base_fr.Name == "frmMember_Select_Group_Center" || Base_fr.Name == "frmMember_Select_Group_Date" || Base_fr.Name == "frmMember_Select_Group_Date_Center"
                     || Base_fr.Name == "frmMember_Select_Union" || Base_fr.Name == "frmSMS_Member"
                     )
                {
                    Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                }
                if (Base_fr.Name == "frmSell" || Base_fr.Name == "frmSell_Center_Change" || Base_fr.Name == "frmSell_Select_Group_Cacu" || Base_fr.Name == "frmSell_Select_Group_Card"
                   || Base_fr.Name == "frmSell_Select_Group_Date" || Base_fr.Name == "frmSell_Select_Group_Date_Item" || Base_fr.Name == "frmSell_Select_Group_Date_Sell_Cen"
                   || Base_fr.Name == "frmSell_Select_Group_Item" || Base_fr.Name == "frmSell_Select_Group_Sell_Cen" || Base_fr.Name == "frmSell_Select_Group_Sell_Cen_Card"
                   || Base_fr.Name == "frmSell_Select_Group_Sell_Cen_Item" || Base_fr.Name == "frmStock_OUT" || Base_fr.Name == "frmStock_OUT_Sell"
                   || Base_fr.Name == "frmStock_OUT_Sell_Cancel" || Base_fr.Name == "frmStock_OUT_Sell_Check" || Base_fr.Name == "frmStock_IN"
                   || Base_fr.Name == "frmStock_IN" || Base_fr.Name == "frmStock_IN_Sell" || Base_fr.Name == "frmStock_IN_Sell_Cancel"
                   || Base_fr.Name == "frmStock_OUT_Select" || Base_fr.Name == "frmStock_IN_Select" || Base_fr.Name == "frmStock_Move" || Base_fr.Name == "frmStock_Move_Confirm"
                   || Base_fr.Name == "frmStock_Move_Confirm" || Base_fr.Name == "frmStock_Move_Select" || Base_fr.Name == "frmStock_Select_Center"
                   || Base_fr.Name == "frmSell_Select_Union" || Base_fr.Name == "frmStock_Select_Union_Cancel" || Base_fr.Name == "frmStock_Select_Union")// 2019-04-15 구현호 센터관리 주문관련 센터보여주기에 맞춰서 나옴 
                {
                    Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                }
                if (Base_fr.Name == "frmSell_Select" || Base_fr.Name == "frmSell_Select_Detail" || Base_fr.Name == "frmSell_Select_History"
                    || Base_fr.Name == "frmSell_Select_Detail")// 2019-04-16 구현호 한폼에 회원, 주문 센터가 동시에 들어가 있는 폼만 작용된다..
                {
                    if (tb.Name == "txtCenter")
                    {
                        Tsql = Tsql + " And  ShowMemberCenter = 'Y' ";
                    }
                    else if (tb.Name == "txtCenter2")
                    {
                        Tsql = Tsql + " And  ShowOrderCenter = 'Y' ";
                    }
                }
                //회원관련, 주문관련 센터보여주기와 한꺼번에 보여주기는 일단 만들어뒀으나 주석처리함,말나오면 풀어준다
                if (And_Sql != "") Tsql = Tsql + And_Sql;

                //Tsql = Tsql + "  And ncode <> '002'"; //임시, 위에 작업 끝나면 지워야함.
                Tsql = Tsql + " Order by Ncode ";                
            }

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2" || tb.Name == "txtR_Id3")
            {
                
                Tsql = "Select user_id ,U_Name   ";
                Tsql = Tsql + " From tbl_User (nolock) ";
                Tsql = Tsql + " Where user_id <> '' ";
                cls_NationService.SQL_User_NationCode(ref Tsql);
                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  (U_Name like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    user_id like '%" + tb.Text.Trim() + "%')";
                }

                if (And_Sql != "") Tsql = Tsql + And_Sql;

                Tsql = Tsql + " Order by user_id ";                
            }

            if (tb.Name == "txtBank")
            {
                
                Tsql = "Select Ncode ,BankName    ";
                Tsql = Tsql + " From tbl_Bank (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";
                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    BankName like '%" + tb.Text.Trim() + "%' )";

                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (And_Sql != "") Tsql = Tsql + And_Sql;
                Tsql = Tsql + " Order by Ncode ";                                 
            }

            if (tb.Name == "txtChange")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;

                }
                else
                {
                    Tsql = "Select M_Detail ," + cls_app_static_var.Base_M_Detail_Ex + " ";
                    Tsql = Tsql + " From tbl_Memberinfo_Mod_Detail (nolock) ";
                    Tsql = Tsql + " Where " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by " + cls_app_static_var.Base_M_Detail_Ex;
                }
               
            }



            if (tb.Name == "txt_SellSort")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,S_Name    ";
                    Tsql = Tsql + " From tbl_Goods__Sort (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ,S_Name    ";
                    Tsql = Tsql + " From tbl_Goods__Sort (nolock) ";
                    Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    S_Name like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                }
            }

            if (tb.Name == "txtSellCode")
            {
                if (Base_fr.Name == "frmMember_Update_2")
                {
                    Tsql = "";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = "select [leavereason_code],[leavereason_name] from tbl_leavereason (nolock)   ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = "select [leavereason_code],[leavereason_name_EN] from tbl_leavereason (nolock)   ";
                    }
                    
                    Tsql = Tsql + " Where leavereason_code <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by leavereason_code ";
                }
                else
                {
                    if (tb.Text.Trim() == "")
                    {
                        //Tsql = "Select SellCode ,SellTypeName    ";
                        //Tsql = "Select SellCode , [SellTypeName] = " + ((cls_User.gid_CountryCode != "TH") ? "SellTypeName" : "SellTypeName_En") + " ";
                        Tsql = "Select SellCode , [SellTypeName] = " + cls_app_static_var.Base_SellTypeName + " ";
                        Tsql = Tsql + " From tbl_SellType (nolock) ";
                        Tsql = Tsql + " Where SellCode <> '' ";
                        if (And_Sql != "") Tsql = Tsql + And_Sql;
                        Tsql = Tsql + " Order by SellCode ";
                    }
                    else
                    {
                        //Tsql = "Select SellCode ,SellTypeName    ";
                        //Tsql = "Select * ";
                        Tsql = "Select SellCode , [SellTypeName] = " + cls_app_static_var.Base_SellTypeName + " ";
                        Tsql = Tsql + " From tbl_SellType (nolock) ";
                        Tsql = Tsql + " Where SellCode like '%" + tb.Text.Trim() + "%'";
                        //Tsql = Tsql + " OR    SellTypeName like '%" + tb.Text.Trim() + "%'";
                        //Tsql = Tsql + " OR SellTypeName like '%" + tb.Text.Trim() + "%' OR SellTypeName_En like '%" + tb.Text.Trim() + "%'";
                        Tsql = Tsql + " OR    " + cls_app_static_var.Base_SellTypeName + " like '%" + tb.Text.Trim() + "%'";
                        if (And_Sql != "") Tsql = Tsql + And_Sql;
                    }
                }
            }

            if (tb.Name == "txt_BaseOut")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_Out_Code (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_Out_Code (nolock) ";
                    Tsql = Tsql + " Where Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    T_Name like '%" + tb.Text.Trim() + "%'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txt_Base_Rec")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode , Name  ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select  Ncode, Name   ";
                    Tsql = Tsql + " From tbl_Base_Rec (nolock) ";
                    Tsql = Tsql + " Where ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (Base_Na_Code != "") Tsql = Tsql + " And   Na_Code = '" + Base_Na_Code + "'";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }
                       
            if (tb.Name == "txt_Receive_Method")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by M_Detail ";
                }
                else
                {
                    Tsql = "Select M_Detail , " + cls_app_static_var.Base_M_Detail_Ex;
                    Tsql = Tsql + " From tbl_Base_Change_Detail (nolock) ";
                    Tsql = Tsql + " Where M_Detail_S = 'tbl_Sales_Rece' ";
                    Tsql = Tsql + " And  ( M_Detail like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    " + cls_app_static_var.Base_M_Detail_Ex + " like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by M_Detail ";
                }
            }
            //20190313 구현호 여기다
            if (tb.Name == "txt_ItemCode" || tb.Name == "txt_ItemCodeUp" || tb.Name == "txt_ItemCodeUpPr" || tb.Name == "txt_ItemCodePr")
            {

                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Name , NCode  ,price2 ,price4, price5    ";
                    Tsql += string.Format(" From ufn_Good_Search_Web_Sell ('{0}', '{1}', '{2}') "
                        , T_SellDate.Replace("-", "").Trim()
                        , Base_Na_Code
                        , EtcCode);
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Name , NCode ,price2,price4 ,price5    ";
                    Tsql += string.Format(" From ufn_Good_Search_Web_Sell ('{0}', '{1}', '{2}') "
                          , T_SellDate.Replace("-", "").Trim()
                          , Base_Na_Code
                          , EtcCode);
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            //20190313 구현호 여기다
            if (tb.Name == "txt_promotion")
            {

                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select PROC_NAME , PRO_CODE    ";
                    Tsql += string.Format(" From JDE_PROC "
                        , T_SellDate.Replace("-", "").Trim()
                        , Base_Na_Code
                        , EtcCode);
                    Tsql = Tsql + " Where PRO_CODE <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by PRO_CODE ";
                }
                else
                {
                    Tsql = "Select PROC_NAME , PRO_CODE  ";
                    Tsql += string.Format("  From JDE_PROC "
                          , T_SellDate.Replace("-", "").Trim()
                          , Base_Na_Code
                          , EtcCode);
                    Tsql = Tsql + " Where (PRO_CODE like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    PROC_NAME like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by PRO_CODE ";
                }
            }

            if (tb.Name == "txt_ItemName2")
            {
                if (tb.Text.Trim() == "")
                {

                    Tsql = "Select Ncode ";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = Tsql + " ,Name ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = Tsql + " ,Name_e Name ";
                    }

                    Tsql = Tsql + " From ufn_Good_Search_ETC ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "') ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {
                    Tsql = "Select Ncode ";
                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        Tsql = Tsql + " ,Name ";
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        Tsql = Tsql + " ,Name_e Name ";
                    }

                    Tsql = Tsql + " From ufn_Good_Search_ETC ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "') ";
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }



            if (tb.Name == "txtIO")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                    Tsql = Tsql + " Where Kind_TF ='IO' And T_TF =   " + io_TF;
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,T_Name    ";
                    Tsql = Tsql + " From tbl_Base_IO_Code (nolock) ";
                    Tsql = Tsql + " Where Kind_TF ='IO' And T_TF =   " + io_TF;
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    T_Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txt_C_Card")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,cardname    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,cardname    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    cardname like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            if (tb.Name == "txtP2" || tb.Name == "txtP")
            {
                if (tb.Text.Trim() == "")
                {
                    Tsql = "Select Ncode ,Name    ";
                    Tsql = Tsql + " From tbl_purchase (nolock) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    Tsql = "Select Ncode ,Name    ";
                    Tsql = Tsql + " From tbl_Card (nolock) ";
                    Tsql = Tsql + " Where tbl_purchase <> '' ";
                    Tsql = Tsql + " And   (Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    if (And_Sql != "") Tsql = Tsql + And_Sql;
                    Tsql = Tsql + " Order by Ncode ";
                }
            }


            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();

                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                return; 
            }
           
            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter56")
                db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);

            if (tb.Name == "txtR_Id" || tb.Name == "txtR_Id2" || tb.Name == "txtR_Id3")
                db_grid_Popup_Base(2, "사용자ID", "사용자명", "user_id", "U_Name", Tsql);

            if (tb.Name == "txtBank")
                db_grid_Popup_Base(2, "은행_코드", "은행명", "Ncode", "BankName", Tsql);

            if (tb.Name == "txtChange")
                db_grid_Popup_Base(2, "", "변경내역", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);

            if (tb.Name == "txt_BaseOut")
                db_grid_Popup_Base(2, "코드", "출고_사유", "Ncode", "T_Name", Tsql);
            //Select Ncode ,T_Name 

            if (tb.Name == "txt_promotion")
                db_grid_Popup_Base(2, "프로모션이름", "프로모션코드", "PROC_NAME", "PRO_CODE", Tsql);

            if (tb.Name == "txtSellCode")
            {
                if (Base_fr.Name == "frmMember_Update_2")
                {
                    //db_grid_Popup_Base(2, "재등록가능여부코드", "재등록가능여부명칭", "leavereason_code", "leavereason_name", Tsql);

                    // 한국인 경우
                    if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
                    {
                        db_grid_Popup_Base(2, "재등록가능여부코드", "재등록가능여부명칭", "leavereason_code", "leavereason_name", Tsql);
                    }
                    // 태국인 경우
                    else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
                    {
                        db_grid_Popup_Base(2, "Re-registration availability code", "Name of re-registration availability", "leavereason_code", "leavereason_name_EN", Tsql);
                    }

                }
                else
                {
                    db_grid_Popup_Base(2, "주문_코드", "주문종류", "SellCode", "SellTypeName", Tsql);
                }
            }
            if (tb.Name == "txt_Base_Rec")
                db_grid_Popup_Base(2, "배송사_코드", "배송사", "ncode", "name", Tsql);

            if (tb.Name == "txt_Receive_Method")            
                db_grid_Popup_Base(2, "배송_코드", "배송_구분", "M_Detail", cls_app_static_var.Base_M_Detail_Ex, Tsql);

            if (tb.Name == "txt_ItemCode" || tb.Name == "txt_ItemCodeUp" || tb.Name == "txt_ItemCodePr" || tb.Name == "txt_ItemCodeUpPr")        //20190313 구현호 여기다 
                db_grid_Popup_Base(5, "상품명", "상품코드", "개별단가", "개별PV", "개별CV", "Name", "Ncode", "price2", "price4", "price5", Tsql);

            if (tb.Name == "txt_ItemName2")
                db_grid_Popup_Base(2, "상품_코드", "상품명", "Ncode", "Name", Tsql);

            if (tb.Name == "txtIO")
                db_grid_Popup_Base(2, "입고_코드", "입고종류", "Ncode", "T_Name", Tsql);

            if (tb.Name == "txt_C_Card")
                db_grid_Popup_Base(2, "카드_코드", "카드명", "ncode", "cardname", Tsql);

            
                 
        }


        public void Db_Grid_Popup_Make_Sql_Not(TextBox tb, TextBox tb1_Code, string Base_Na_Code)
        {
            //cls_Grid_Base_Popup cgb_Pop = new cls_Grid_Base_Popup();
            //DataGridView Popup_gr = new DataGridView();
            //Popup_gr.Name = "Popup_gr";
            //tfr.Controls.Add(Popup_gr);
            //cgb_Pop.basegrid = Popup_gr;
            //cgb_Pop.Base_fr = tfr;
            //cgb_Pop.Base_tb = tb1_Code;  //앞에게 코드
            //cgb_Pop.Base_tb_2 = tb;    //2번은 명임
            //cgb_Pop.Base_Location_obj = tb;

            string Tsql = "";

            //if (Base_Na_Code == "")
            //    Base_Na_Code = "KR";


            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter5")
            {
                Tsql = "Select Ncode , Name  ";
                Tsql = Tsql + " From tbl_Business (nolock) ";
                Tsql = Tsql + " Where Ncode <> '' ";

                if (tb.Text.Trim() != "")
                {
                    Tsql = Tsql + " And  ( Ncode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                                        
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }
                else
                {                    
                    if (Base_Na_Code != "") Tsql = Tsql + " And  Na_Code = '" + Base_Na_Code + "'";
                }

                if (Base_fr.Name == "frmMember" || Base_fr.Name == "frmSell")
                {
                    Tsql = Tsql + " And  U_TF = 0 "; //사용센타만 보이게 한다 
                }

                Tsql = Tsql + " Order by Ncode ";
            }

           
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();

                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                return;
            }

            if (tb.Name == "txtCenter" || tb.Name == "txtCenter2" || tb.Name == "txtCenter3" || tb.Name == "txtCenter4" || tb.Name == "txtCenter56")
                db_grid_Popup_Base(2, "센타_코드", "센타명", "Ncode", "Name", Tsql);

        }


        public void Db_Grid_Popup_Make_Sql(int i, TextBox tb, TextBox tb1_Code, string Base_Na_Code, string T_SellDate, string ABC_TF)
        {

            string Tsql = "";



            if (tb.Name == "txt_ItemCode")
            {
                if (tb.Text.Trim() == "")
                {
                    
                    if (ABC_TF == "1") Tsql = "Select Name , NCode  ,price2, price4 , price5   ";
                    else if (ABC_TF == "3") Tsql = "Select Name , NCode  ,price2 , price4 , price5  "; //직원가
                    Tsql = Tsql + " From ufn_Good_Search_Web_Sell ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "','" + ABC_TF +"'   ) ";
                    Tsql = Tsql + " Where Ncode <> '' ";
                    Tsql = Tsql + " Order by Ncode ";
                }
                else
                {

                    if (ABC_TF == "1") Tsql = "Select Name , NCode  ,price2, price4 , price5   ";
                    else if (ABC_TF == "3") Tsql = "Select Name , NCode  ,price2 , price4 , price5   "; //직원가
                    Tsql = Tsql + " From ufn_Good_Search_Web_Sell ('" + T_SellDate.Replace("-", "").Trim() + "','" + Base_Na_Code + "','" + ABC_TF + "' ) ";
                    Tsql = Tsql + " Where (NCode like '%" + tb.Text.Trim() + "%'";
                    Tsql = Tsql + " OR    Name like '%" + tb.Text.Trim() + "%')";
                    Tsql = Tsql + " Order by Ncode ";
                }
            }

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "t_P_table", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 1)
            {
                tb.Text = ds.Tables["t_P_table"].Rows[0][1].ToString();
                tb1_Code.Text = ds.Tables["t_P_table"].Rows[0][0].ToString();
               
                if (Next_Focus_Control != null)
                    Next_Focus_Control.Focus();

                basegrid.Visible = false;
                basegrid.Dispose();

                return;
            }

            db_grid_Popup_Base(5, "상품명", "상품코드", "개별단가", "개별PV", "개별CV", "Name", "Ncode", "price2", "price4", "price5" , Tsql);

        }




    }// end cls_Grid_Base_Popup









    class cls_Grid_Base_info_Put
    { 
        cls_Grid_Base Base_dgv = new cls_Grid_Base();
        private string  base_db_name  ="Temp_Table"  ;
        
        public void dGridView_Put_baseinfo(Form fr, DataGridView t_Dgv, string  intTemp , string Mbid, string Ordernumber = "")
        {
            Base_dgv.Grid_Base_Arr_Clear();
            Base_dgv.basegrid = t_Dgv;
            Base_dgv.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            Base_dgv.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Base_dgv.Sort_Mod_Auto_TF = 1;

             if (intTemp == "sell")            
                Sell_dGridView_Info_Header_Reset(t_Dgv );

             if (intTemp == "memc")            
                Mem_change_dGridView_Info_Header_Reset(t_Dgv );

            if (intTemp == "memupc")            
                Mem_UP_change_dGridView_Info_Header_Reset(t_Dgv );

            if (intTemp == "memadd")            
                Mem_Add_dGridView_Info_Header_Reset(t_Dgv );

            if (intTemp == "item")
                dGridView_Sell_Item_Header_Reset();
            if (intTemp == "item_mem")
                dGridView_Sell_Item_mem_Header_Reset();
            if (intTemp == "cacu")    
                dGridView_Sell_Cacu_Header_Reset();
            if (intTemp == "rece")    
                dGridView_Sell_Rece_Header_Reset();

            if (intTemp == "pay")
                dGridView_Pay_Header_Reset();

            if (intTemp == "talk")
                dGridView_Talk_Header_Reset();

            if (intTemp == "member")
                dGridView_Member_Header_Reset();

            if (intTemp == "RePay_D2")
                dGridView_RePay_D2_info_Header_Reset();

            if (intTemp == "RePay_D4")
                dGridView_RePay_D4_info_Header_Reset();
            if (intTemp == "gold")
                dGridView_gold();
            Base_dgv.basegrid.RowHeadersVisible = false;

            if (intTemp == "saveup" || intTemp == "nominup" || intTemp == "savedown" ||  intTemp == "nomindown" || intTemp == "savedefault")
                dGridView_Save_Up_Header_Reset();

            Base_dgv.d_Grid_view_Header_Reset();                       
            
            
            
            Base_Grid_info_Set(fr ,intTemp, Mbid, Ordernumber) ;
        }


        public void dGridView_Put_baseinfo( DataGridView t_Dgv, string intTemp)
        {
            Base_dgv.Grid_Base_Arr_Clear();
            Base_dgv.basegrid = t_Dgv;
            Base_dgv.grid_select_mod = DataGridViewSelectionMode.FullRowSelect;
            Base_dgv.basegrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


            if (intTemp == "sell")
                Sell_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memc")
                Mem_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memupc")
                Mem_UP_change_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "memadd")
                Mem_Add_dGridView_Info_Header_Reset(t_Dgv);

            if (intTemp == "item")
                dGridView_Sell_Item_Header_Reset();
            if (intTemp == "item_mem")
                dGridView_Sell_Item_mem_Header_Reset();
            if (intTemp == "cacu")
                dGridView_Sell_Cacu_Header_Reset();
            if (intTemp == "rece")
                dGridView_Sell_Rece_Header_Reset();

            if (intTemp == "pay")
                dGridView_Pay_Header_Reset();

            if (intTemp == "member")
                dGridView_Member_Header_Reset();

            if (intTemp == "talk")
                dGridView_Talk_Header_Reset();

            if (intTemp == "RePay_D2")
                dGridView_RePay_D2_info_Header_Reset();



            Base_dgv.basegrid.RowHeadersVisible = false;


            if (intTemp == "saveup" || intTemp == "nominup" || intTemp == "savedown" ||  intTemp == "nomindown" || intTemp == "savedefault")
                dGridView_Save_Up_Header_Reset();

            Base_dgv.d_Grid_view_Header_Reset();
            
        }


        private void Sell_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {

            Base_dgv.grid_col_Count = 13;

            string[] g_HeaderText = {"승인여부"  , "매출_일자" ,  "주문번호" ,  "주문_종류"   , "상태"
                                     , "매출액"  , "입급액"  ,"매출PV" , "매출CV" , "현금"
                                     , "카드"    , "무통장" , "비고"
                                    };

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_cell_format = gr_dic_cell_format;

            int[] g_Width = { 80,100, 90, 70, 80
                                , 80 , 80 , 80 , 80 ,80
                                , 80 , 80 , 100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {
                                DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter//5     

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleRight  //11
                                ,DataGridViewContentAlignment.MiddleRight  //12
                                ,DataGridViewContentAlignment.MiddleCenter
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                ,true , true,  true,  true ,true
                                ,true    ,true     ,true
                                };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }// end Sell_dGridView_Info_Header_Reset




        private void Mem_change_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;

            string[] g_HeaderText = {"변경일"  , "변경내역"   , "전_내역"  , "후_내역"   , "변경자"        
                                , ""   , ""    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 80
                                ,0 , 0 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                ,true , true,  true,  true ,true  
                                ,true                      
                                };        
            Base_dgv.grid_col_Lock = g_ReadOnly;     
        }//  end Mem_change_dGridView_Info_Header_Reset



        private void Mem_UP_change_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;
            string[] g_HeaderText = {"변경일"  , "전_상위번호"   , "전_상위성명"  , "후_상위번호"   , "후_상위성명"        
                                , "구분"   , "변경자"    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                ,true , true,  true,  true ,true  
                                ,true                      
                                };        
            Base_dgv.grid_col_Lock = g_ReadOnly;     
        } // end Mem_UP_change_dGridView_Info_Header_Reset


        private void Mem_Add_dGridView_Info_Header_Reset(DataGridView t_Dgv)
        {
            Base_dgv.grid_col_Count = 11;
            string[] g_HeaderText = {"구분"  , "우편_번호"   , "주소1"  , "주소2"   , "연락처1"        
                                , "연락처2"   , "수취인명"    , ""  , "" , ""
                                ,""
                                };

            int[] g_Width = { 120, 100, 100, 100, 100
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                ,true , true,  true,  true ,true  
                                ,true                      
                                };        
            Base_dgv.grid_col_Lock = g_ReadOnly;     
        } // Mem_Add_dGridView_Info_Header_Reset



        private void dGridView_Sell_Item_Header_Reset()
        {

            Base_dgv.grid_col_Count = 13;


            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"
                                , "개별CV", "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV"
                                , "구분" , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 160, 80, 70
                            ,  70,  80 ,  80 ,  80 ,  70
                            , 70, 200,100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight //10

                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true  , true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }

        private void dGridView_Sell_Item_mem_Header_Reset()
        {

            Base_dgv.grid_col_Count = 13;


            string[] g_HeaderText = {""  , "상품_코드"   , "상품명"  , "개별단가"   , "개별PV"
                                , "개별CV", "주문_수량"   , "총상품액"    , "총상품PV"  , "총상품CV"
                                , "구분" , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 160, 80, 0
                            ,  0,  80 ,  80 ,  0 ,  0
                            , 70, 200,100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight //10

                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;


            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                     ,true  , true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail
        //////SalesItemDetail___SalesItemDetail__SalesItemDetail__SalesItemDetail


        private void dGridView_Sell_Cacu_Header_Reset()
        {
            Base_dgv.grid_col_Count = 10;            

            string[] g_HeaderText = {""  , "결제방법"   , "결제액"  , "결제일자"   , "카드_은행명"        
                                , "카드_은행번호"   , "카드소유자"    , "입금자"  , "비고" ,"주문번호"
                                };

            int[] g_Width = { 0, 90, 70, 90, 100
                                ,120 , 100 , 90 , 150 , 100
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                            
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu
        //////Sales_Cacu___Sales_Cacu__Sales_Cacu__Sales_Cacu



        private void dGridView_Sell_Rece_Header_Reset()
        {
           
            Base_dgv.grid_col_Count = 13;   // 태국 주, 태국 도시 열 추가.          

            string[] g_HeaderText = {""  , "배송구분"   , "배송일"  , "수령인"   , "우편_번호"        
                                , "주소1"   , "주소2"    , "연락처_1"  , "연락처_2" , "비고"
                                ,"주문번호", "태국_주", "태국_도시"
                                };

            int[] g_Width;

            // 태국인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                g_Width = new int[] { 0, 90, 70, 90, 100
                                ,120, 100, 90, 150, 200
                                ,100, 100, 100
                            };
            }
            // 그 외 국가 인 경우
            else
            {
                g_Width = new int[] { 0, 90, 70, 90, 100
                                ,120, 100, 90, 150, 200
                                ,100, 0, 0
                            };
            }


            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter  //5    
  
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleCenter  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //11
                                ,DataGridViewContentAlignment.MiddleCenter  //12
                                ,DataGridViewContentAlignment.MiddleCenter  //13

                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[3 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true      
                                     ,true ,true ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            Base_dgv.basegrid.RowHeadersVisible = false;
        }
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece
        //////Sales_Rece___Sales_Rece__Sales_Rece__Sales_Rece





        private void dGridView_Pay_Header_Reset()
        {

            Base_dgv.grid_col_Count = 11;            

            string[] g_HeaderText = {"구분" ,  "마감일자" ,  "지급일자"   , "발생액"  , "소득세"  
                                    , "주민세"  ,"실지급액"  , ""  , "" , "" 
                                    , ""
                                    };

            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();
            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[5 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_cell_format = gr_dic_cell_format;

            int[] g_Width = { 100, 90, 70, 80, 80
                                ,80 , 80 , 0 , 0 , 0
                                ,0
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleCenter 
                                ,DataGridViewContentAlignment.MiddleCenter  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10

                                ,DataGridViewContentAlignment.MiddleCenter  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;
            

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true                      
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;            
        }


        private void dGridView_Talk_Header_Reset()
        {

            Base_dgv.grid_col_Count = 10;

            string[] g_HeaderText = {"상담_내역" ,  "기록자" ,  "기록일"   , "_Seq"  , ""  
                                    , ""  ,""  , ""  , "" , ""                                     
                                    };

        

            int[] g_Width = { 500, 100, 150, 0, 0
                                ,0 , 0 , 0 , 0 , 0                                
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleLeft
                                ,DataGridViewContentAlignment.MiddleLeft 
                                ,DataGridViewContentAlignment.MiddleLeft  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5    
  
                                ,DataGridViewContentAlignment.MiddleRight 
                                ,DataGridViewContentAlignment.MiddleRight  
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                };

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;


            Boolean[] g_ReadOnly = { true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true                                                        
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }


        private void dGridView_Member_Header_Reset()
        {
            Base_dgv.grid_col_Count = 27;
            
            string[] g_HeaderText = {"회원_번호"  , "성명"   , "주민번호"  , "현직급"   , "라인"        
                                , "센타명"   , "가입일"    , "집전화"   , "핸드폰"    , "교육일"
                                , "후원인"   , "후원인명"  , "추천인"   , "추천인명"   ,"우편_번호"
                                , "주소1"   , "주소2"   , "은행명"    , "계좌번호" , "예금주"    
                                , "구분" , "활동_여부", "_중지_여부"  , "탈퇴일"  , "_라인중지일"  
                                ,"기록자" , "기록일"
                                    };
            Base_dgv.grid_col_header_text = g_HeaderText;

            int[] g_Width = { 0, 90 , 130, 80, 60  
                             ,100, 90, 130, 130, 90  
                             ,cls_app_static_var.save_uging_Pr_Flag , cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag, 80
                             ,200 , 90, 120 , 90 , 60
                             ,70 , 70 , 0 , 90 , 0
                             ,0 , 0 
                            };
            Base_dgv.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true , true,  true,  true ,true  
                                    ,true ,true 
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                          
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //20

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //25   

                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter  
                              };
            Base_dgv.grid_col_alignment = g_Alignment;
        }


        private void dGridView_Save_Up_Header_Reset()
        {
            Base_dgv.grid_col_Count = 15;

            string[] g_HeaderText = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"        
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , ""   ,"위치"                                
                                    };

            string[] g_Cols = {"대수"  , "회원_번호"   , "직급"  , "성명"   , "가입일"
                                , "탈퇴일"   , "센타명"    , "후원인"   , "후원인명"    , "추천인"
                                , "추천인명"   , "집전화"  , "핸드폰"   , "Col1"   ,"위치"
                                    };
            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_col_name = g_Cols;

            int[] g_Width = { 90, 90 , 130, 80, 60  
                             ,100, 90, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.save_uging_Pr_Flag, cls_app_static_var.nom_uging_Pr_Flag  
                             ,cls_app_static_var.nom_uging_Pr_Flag , 90, 80, 0, 100                           
                            };
            Base_dgv.grid_col_w = g_Width;

            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                     
                                    ,true , true,  true,  true ,true                                      
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;

            DataGridViewContentAlignment[] g_Alignment =
                              {DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //5
                               
                               ,DataGridViewContentAlignment.MiddleLeft                              
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter //10

                               ,DataGridViewContentAlignment.MiddleCenter   
                               ,DataGridViewContentAlignment.MiddleCenter 
                               ,DataGridViewContentAlignment.MiddleCenter  
                               ,DataGridViewContentAlignment.MiddleCenter
                               ,DataGridViewContentAlignment.MiddleCenter  //15   
                  
                              };
            Base_dgv.grid_col_alignment = g_Alignment;

            Base_dgv.basegrid.RowHeadersVisible = true;
        }


        private void dGridView_RePay_D2_info_Header_Reset()
        {

            Base_dgv.grid_col_Count = 17;

            string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "반품주문번호"   ,  "반품회원번호" , "반품성명"
                                     ,  "추천" ,"_소비전환"    , "_패키지"  , "후원"  ,"매칭"
                                     ,"공제예상액합산","반품CV"    , "차감한도","후원좌 차감" , "후원우 차감"
                                     ,"매칭회원상세", "매칭금액상세"
                                    };

            int[] g_Width = { 90,120, 90, 100, 80
                              , 80  ,0 , 0 ,80 , 80
                              , 80    , 80, 80    , 80  , 80
                              ,200                              ,200
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleLeft

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //5      
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight  //10
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight

                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                ,DataGridViewContentAlignment.MiddleLeft  //10
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            gr_dic_cell_format[6 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[7 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[8 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[9 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[10 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[11 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[12 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[13 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[14 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            gr_dic_cell_format[15 - 1] = cls_app_static_var.str_Grid_Currency_Type;



            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true
                                    ,true , true,  true,  true ,true
                                    ,  true ,true      ,true        ,true        ,true
                                      ,true        ,true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }



        private void dGridView_gold()
        {

            Base_dgv.grid_col_Count = 4;

            string[] g_HeaderText = {"lv","최초골드이상 회원번호","최초골드이상 회원 직위"  ,"최초골드이상 회원명"
                                    };
            ///
            int[] g_Width = { 120,120,120,120
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                             ,DataGridViewContentAlignment.MiddleCenter
                             ,DataGridViewContentAlignment.MiddleCenter
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            //gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;

            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true, true, true
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }


        private void dGridView_RePay_D4_info_Header_Reset()
        {

            Base_dgv.grid_col_Count = 5;

            string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "구분" ,"금액" ,""
                                    };

            int[] g_Width = { 90,120, 90, 100, 0                              
                            };

            DataGridViewContentAlignment[] g_Alignment =
                                {DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleCenter
                                ,DataGridViewContentAlignment.MiddleRight
                                ,DataGridViewContentAlignment.MiddleRight
                           
                                };


            Dictionary<int, string> gr_dic_cell_format = new Dictionary<int, string>();


            gr_dic_cell_format[4 - 1] = cls_app_static_var.str_Grid_Currency_Type;
            
            Base_dgv.grid_col_header_text = g_HeaderText;
            Base_dgv.grid_cell_format = gr_dic_cell_format;
            Base_dgv.grid_col_w = g_Width;
            Base_dgv.grid_col_alignment = g_Alignment;



            Boolean[] g_ReadOnly = { true , true,  true,  true ,true                             
                                   };
            Base_dgv.grid_col_Lock = g_ReadOnly;
        }








        private void Base_Grid_info_Set(Form fr, string  intTemp , string SdMbid, string Ordernumber )
        {
            string T_Mbid = "";
            T_Mbid = SdMbid.Trim();
            string Mbid = ""; int Mbid2 = 0;
            cls_Search_DB csb = new cls_Search_DB();
            cls_form_Meth cm = new cls_form_Meth();
            csb.Member_Nmumber_Split(T_Mbid, ref Mbid, ref Mbid2);
            string Tsql = "";
            if (intTemp == "sell")            
                Sell_dGridView_Info_Put(Mbid, Mbid2,Ordernumber,ref Tsql );

             if (intTemp == "memc")            
                Mem_change_dGridView_Info_Put(Mbid, Mbid2,Ordernumber , ref Tsql );

            if (intTemp == "memupc")            
                Mem_UP_change_dGridView_Info_Put(Mbid, Mbid2,Ordernumber, ref Tsql );

            if (intTemp == "memadd")            
                Mem_Add_dGridView_Info_Put(Mbid, Mbid2,Ordernumber, ref Tsql );

             if (intTemp == "item" || intTemp == "item_mem")
                Set_SalesItemDetail(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "cacu")
                 Set_Sales_Cacu(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "rece")
                 Set_Sales_Rece(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "pay")
                 Set_Pay(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "member")
                 Set_Memberinfo(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "talk")
                 Set_Memberinfo_Talk(Mbid, Mbid2, Ordernumber, ref Tsql);

             if (intTemp == "saveup")
                 Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber, "SAVE", ref Tsql);

             if (intTemp == "nominup")
                 Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber,"NOM", ref Tsql);

             if (intTemp == "savedown")
                 Set_Memberinfo_Down(Mbid, Mbid2, Ordernumber, "SAVE", ref Tsql);

             if (intTemp == "nomindown")
                  Set_Memberinfo_Down(Mbid, Mbid2, Ordernumber, "NOM", ref Tsql);

             //if (intTemp == "RePay_D2")
             //   Set_Memberinfo_RePay_D2_info(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "RePay_D4")
                Set_Memberinfo_RePay_D4_info(Mbid, Mbid2, Ordernumber, ref Tsql);

            if (intTemp == "savedefault")
                Set_Memberinfo_Up(Mbid, Mbid2, Ordernumber, "SAVEDEFAULT", ref Tsql);
            //if (intTemp == "gold")
            //{

            //    cls_Connect_DB Temp_Connect1 = new cls_Connect_DB();
            //    StringBuilder sb = new StringBuilder();


            //    sb.AppendLine("Select  ");
            //    sb.AppendLine(" T_AA.Lvl ");
            //    sb.AppendLine(" , T_AA.mbid2");
            //    sb.AppendLine("   ,Isnull(CC_A.G_Name,'') ");
            //    sb.AppendLine("    ,A.M_Name ");
            //    sb.AppendLine("	 , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End  ");
            //    sb.AppendLine("	 , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ");
            //    sb.AppendLine("	 , Isnull( tbl_Business.name,'')  ,A.Saveid2  , Isnull(b.M_Name,'')  ,A.Nominid2  , Isnull(C.M_Name,'')  , A.hometel  , A.hptel  , '' ");
            //    sb.AppendLine("	  , A.LineCnt  From ufn_matrix_mem(''," + Ordernumber + ", '*') T_AA  LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2    ");
            //    sb.AppendLine("	  LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2    LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ");
            //    sb.AppendLine("	   LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code");
            //    sb.AppendLine("	    Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt  ");
            //    sb.AppendLine("		Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ");
            //    sb.AppendLine("		where  T_AA.mbid2 <> '" + Ordernumber + "' and A.LeaveDate = '' ");
            //    sb.AppendLine("	   ORder by Lvl ");

            //    DataSet ds1 = new DataSet();
            //    //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //    if (Temp_Connect1.Open_Data_Set(sb.ToString(), base_db_name, ds1) == false) return;
            //    int ReCnt1 = Temp_Connect1.DataSet_ReCount;

            //    if (ReCnt1 == 0) return;
            //    //++++++++++++++++++++++++++++++++


            //    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //    Dictionary<int, object[]> gr_dic_text1 = new Dictionary<int, object[]>();

            //    for (int fi_cnt = 0; fi_cnt <= ReCnt1 - 1; fi_cnt++)
            //    {
            //        string test = ds1.Tables[base_db_name].Rows[fi_cnt][2].ToString();
            //        if (test == "골드" || test == "루비" || test == "사파이어" || test == "에메랄드" || test == "다이아몬드" || test == "블루다이아몬드" || test == "레드다디아몬드" || test == "크라운" || test == "엠페리얼")
            //        {
            //            Set_gr_dic2(ref ds1, ref gr_dic_text1, fi_cnt);  //데이타를 배열에 넣는다.
            //            break;
            //        }

            //    }

            //    Base_dgv.grid_name_obj = gr_dic_text1;  //배열을 클래스로 보낸다.
            //    Base_dgv.db_grid_Obj_Data_Put();
            //    return;
            //}
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, base_db_name, ds, fr.Name, fr.Text) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            Dictionary<int, object[]> gr_dic_text = new Dictionary<int, object[]>();
            int T_cnt = 0;
            double S_cnt4 = 0;    double S_cnt5 = 0;    double S_cnt6 = 0;    double S_cnt7 = 0;   double S_cnt8 = 0;   double S_cnt9 = 0;  double S_cnt10 = 0;
            for (int fi_cnt = 0; fi_cnt <= ReCnt - 1; fi_cnt++)
            {
                if (intTemp == "sell" ||  intTemp == "item" || intTemp == "pay"
                    || intTemp == "RePay_D2"
                    || intTemp == "RePay_D4"
                    )   
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt,1);  //데이타를 배열에 넣는다.
                else if (intTemp == "cacu" )
                    Set_gr_dic_Info_Cacu(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.
                else
                    Set_gr_dic_Info(ref ds, ref gr_dic_text, fi_cnt);  //데이타를 배열에 넣는다.

                T_cnt = fi_cnt;
                if (intTemp == "sell")            
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][5].ToString() );
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][6].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][7].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][8].ToString());
                    S_cnt8 = S_cnt8 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][9].ToString());
                    S_cnt9 = S_cnt9 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt][10].ToString());
                }

                if (intTemp == "cacu")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["C_Price1"].ToString());                    
                }

                if (intTemp == "item")
                {
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPrice"].ToString());
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemPV"].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCV"].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemCount"].ToString());
                    S_cnt8 = S_cnt8 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPrice"].ToString());
                    S_cnt9 = S_cnt9 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalPV"].ToString());
                    S_cnt10 = S_cnt10 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ItemTotalCV"].ToString());
                }

                if (intTemp == "pay")
                {                    
                    S_cnt4 = S_cnt4 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["SumAllAllowance"].ToString());
                    S_cnt5 = S_cnt5 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["InComeTax"].ToString());
                    S_cnt6 = S_cnt6 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["ResidentTax"].ToString());
                    S_cnt7 = S_cnt7 + double.Parse(ds.Tables[base_db_name].Rows[fi_cnt]["TruePayment"].ToString());                   
                }
                                
            }


            if (intTemp == "sell")            
            {
                object[] row0 = { ""
                                    ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                    ,""
                                    ,""
                                    ,""

                                    ,S_cnt4
                                    ,S_cnt5
                                    ,S_cnt6
                                    ,S_cnt7
                                    ,S_cnt8

                                    ,S_cnt9
                                    ,""
                                     };

                gr_dic_text[T_cnt + 2] = row0;
            }

            if (intTemp == "cacu")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,S_cnt4
                                ,""
                                ,""

                                ,""
                                ,""
                                ,""
                                ,""
                                ,""
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }

            if (intTemp == "item")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,S_cnt4
                                ,S_cnt5
                                ,S_cnt6
                                ,S_cnt7
                                ,S_cnt8
                                ,S_cnt9
                                ,S_cnt10
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }


            if (intTemp == "pay")
            {
                object[] row0 = { ""
                                ,"<< " + cm._chang_base_caption_search("합계") + " >>"
                                ,""
                                ,S_cnt4
                                ,S_cnt5

                                ,S_cnt6
                                ,S_cnt7
                                ,""
                                ,""
                                ,""
                                 };

                gr_dic_text[T_cnt + 2] = row0;
            }
       

  

            Base_dgv.grid_name_obj = gr_dic_text;  //배열을 클래스로 보낸다.
            Base_dgv.db_grid_Obj_Data_Put();

        } // end Base_Grid_info_Set

        private void Set_gr_dic2(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {

            object[] row0 = {
                 ds.Tables[base_db_name].Rows[fi_cnt][0]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][1]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][2]
                                ,ds.Tables[base_db_name].Rows[fi_cnt][3]
                                 };


            gr_dic_text[0 + 1] = row0;
        }
        //private void get_gold(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        //{

        //    cls_form_Meth cm = new cls_form_Meth();

        //    //Tsql = "Select  top 1  T_AA.Lvl, T_AA.mbid2  ";
        //    //Tsql = Tsql + ",Isnull(CC_A.G_Name,'') as  gold";
        //    //Tsql = Tsql + ",A.M_Name  ";
        //    //Tsql = Tsql + " From ufn_GetSubTree_MemGroup('', " + Ordernumber + ") T_AA ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2  ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2  ";
        //    //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2    ";
        //    //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code ";
        //    //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
        //    //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ";
        //    //Tsql = Tsql + " Where T_AA.Lvl > 0  ";
        //    //Tsql = Tsql + " and C1.grade_cnt>=70";
        //    //Tsql = Tsql + " and  A.LeaveDate = '' ";
        //    //Tsql = Tsql + " ORder by Lvl ASC, ";
        //    //Tsql = Tsql + " LEFT(SaveCur,3) ASC   , SaveCur ASC ";


        //    Tsql = "Select  ";
        //    Tsql = Tsql + " T_AA.Lvl ";
        //    Tsql = Tsql + " , T_AA.mbid2";
        //    Tsql = Tsql + "   ,Isnull(CC_A.G_Name,'') ";
        //    Tsql = Tsql + "    ,A.M_Name ";
        //    Tsql = Tsql + "	 , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End  ";
        //    Tsql = Tsql + "	 , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
        //    Tsql = Tsql + "	 , Isnull( tbl_Business.name,'')  ,A.Saveid2  , Isnull(b.M_Name,'')  ,A.Nominid2  , Isnull(C.M_Name,'')  , A.hometel  , A.hptel  , '' ";
        //    Tsql = Tsql + "	  , A.LineCnt  From ufn_matrix_mem(''," + Ordernumber + ", '*') T_AA  LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2    ";
        //    Tsql = Tsql + "	  LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2    LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
        //    Tsql = Tsql + "	   LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code";
        //    Tsql = Tsql + "	    Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt  ";
        //    Tsql = Tsql + "		Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2  ";
        //    Tsql = Tsql + "		where  T_AA.mbid2 <> '" + Ordernumber + "'";
        //    Tsql = Tsql + "	   ORder by Lvl ";

        //}

        private void Set_Memberinfo_RePay_D2_info(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            //cls_form_Meth cm = new cls_form_Meth();

            ////string[] g_HeaderText = {"마감일"  ,"확정마감일"  , "반품주문번호"   ,  "반품회원번호" , "반품성명"    
            ////                         ,  "멤버" ,"소비전환"    , "패키지"  , "팀"  ,"매칭"
            ////                         ,"공제예상액합산","팀좌 차감" , "팀우 차감" 
            ////,"매칭회원상세", "매칭금액상세"
            ////                        };



            //Tsql = "Select  Re_T.clo_ToEndDate ";
            //Tsql = Tsql + " , Re_T.Cur_ToEndDate  ";
            //Tsql = Tsql + " , Re_T.Ordernumber  ";

            //Tsql = Tsql + " , tbl_SalesDetail.Mbid2  ";
            //Tsql = Tsql + " , tbl_SalesDetail.M_Name ";

            //Tsql = Tsql + " , Re_T.Ded_A_3  ";
            //Tsql = Tsql + " , Re_T.Ded_A_6 ";
            //Tsql = Tsql + " , Re_T.Ded_A_15 ";

            //Tsql = Tsql + " , Re_T.Ded_A_1 ";
            //Tsql = Tsql + " , Re_T.Ded_A_2 ";

            //Tsql = Tsql + " , Ded_A_3 + Ded_A_6 +  Ded_A_15 + Ded_A_1 +Ded_A_2 ";

            //Tsql = Tsql + " , Re_T.TotalPV";
            //Tsql = Tsql + ", Case When Re_T.Ded_PV_1 > 0 then Re_T.Ded_PV_1 ELSE  Re_T.Ded_PV_2 END  ";

            //Tsql = Tsql + " , Re_T.Re_Cur_PV_1 ";
            //Tsql = Tsql + " , Re_T.Re_Cur_PV_2 ";

            //Tsql = Tsql + " , Re_T.Req_Mbid_T ";
            //Tsql = Tsql + " , Re_T.Req_Pay_T ";


            //Tsql = Tsql + " FROM tbl_ClosePay_04_Ded_P_Detail_Mod (nolock)  ";
            ////Tsql = Tsql + " ( ";
            ////Tsql = Tsql + " Select clo_ToEndDate , Cur_ToEndDate ";
            ////Tsql = Tsql + "  , Ded_A_3 , Ded_A_6 , Ded_A_15 , Ded_A_1 , Ded_A_2 ";
            ////Tsql = Tsql + "  , Ded_A_3 + Ded_A_6 +  Ded_A_15 + Ded_A_1 +Ded_A_2 Sum_W_Ded ";
            ////Tsql = Tsql + " , TotalPV , Ded_PV_1, Ded_PV_2, Re_Cur_PV_1 , Re_Cur_PV_2 , Req_Mbid_T,Req_Pay_T  ";
            ////Tsql = Tsql + "  From  tbl_ClosePay_02_Ded_P_Detail_Mod (nolock)    ";
            ////Tsql = Tsql + " Union All ";
            ////Tsql = Tsql + " Select Close_Date clo_ToEndDate , ToEndDate Cur_ToEndDate ";
            ////Tsql = Tsql + "  , 0 Ded_A_3 , 0 Ded_A_6 , 0  Ded_A_15 , 0  Ded_A_1 , 0  Ded_A_2 ";
            ////Tsql = Tsql + "  , 0  Sum_W_Ded ";
            ////Tsql = Tsql + " , 0 TotalPV , 0 Ded_PV_1, 0 Ded_PV_2, 0  Re_Cur_PV_1 , 0  Re_Cur_PV_2 , '' Req_Mbid_T , '' Req_Pay_T  ";
            ////Tsql = Tsql + "  From  tbl_Close_Ret_Pay_Detail (nolock)    ";
            

            ////Tsql = Tsql + " ) "; 
            //Tsql = Tsql + " Re_T  ";

            //Tsql = Tsql + " LEFT JOIN tbl_SalesDetail  (nolock) ON Re_T.Ordernumber = tbl_SalesDetail.Ordernumber ";
            //Tsql = Tsql + " Where Re_T.Mbid2 ='" + Mbid2 + "'";
            //Tsql = Tsql + " And  Ded_A_3 + Ded_A_6 + Ded_A_7+ Ded_A_15 + Ded_A_1 +Ded_A_2  + Re_Cur_PV_1 + Re_Cur_PV_2 > 0 ";

            //Tsql = Tsql + " order by Re_T.Cur_ToEndDate  , tbl_SalesDetail.Mbid2  ";
        }



        private void Set_Memberinfo_RePay_D4_info(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            //string[] g_HeaderText = {"원마감일"  ,"확정마감일"  , "구분" ,"금액" ,""
            //                        };


            Tsql = "Select  ToEndDate  ";
            Tsql = Tsql + " ,  close_Date ";
            Tsql = Tsql + " , CAse When Pay_FLAG = 1  then '첫팩주문보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 2  then '멘토보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 3  then '비즈니스개발보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 4  then '유니레벨보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 5  then '사이드볼륨인피니티보너스' ";
            Tsql = Tsql + "        When Pay_FLAG = 6  then '리더체크매치보너스' ";
            Tsql = Tsql + "  END "; 

            Tsql = Tsql + " , Allowance  ";
            Tsql = Tsql + " , '' ";         
            Tsql = Tsql + " FROM mannatech_Return_Close.dbo.tbl_Close_Ret_Pay_Detail (nolock)  ";                        
            Tsql = Tsql + " Where Mbid2 ='" + Mbid2 + "'";
            Tsql = Tsql + " order by ToEndDate  , Pay_FLAG   ";
        }


        private void Sell_dGridView_Info_Put( string Mbid, int Mbid2 , string Ordernumber , ref string Tsql  )
        {
            
            cls_form_Meth cm = new cls_form_Meth ();

            Tsql = "Select  ";

            Tsql = Tsql + " Case When tbl_SalesDetail.Ga_Order = 0 Then '" + cm._chang_base_caption_search("승인") + "'";
            Tsql = Tsql + "  When tbl_SalesDetail.Ga_Order > 0 Then '" + cm._chang_base_caption_search("미승인") + "'";
            Tsql = Tsql + " END SellTFName ";

            Tsql = Tsql + " ,SellDate ";
            Tsql = Tsql + " ,tbl_SalesDetail.OrderNumber ";
            //Tsql = Tsql + " ,SellTypeName ";
            // 한국인 경우
            if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "KR")
            {
                Tsql = Tsql + " ,SellTypeName ";
            }
            // 태국인 경우
            else if (cls_NationService.GetCountryCodeOrDefault(cls_User.gid_CountryCode) == "TH")
            {
                Tsql = Tsql + " ,SellTypeName_En ";
            }
            
            //Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
            Tsql = Tsql + " ,Case When ReturnTF = 1 Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When ReturnTF = 2 Then '" + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 4 Then '" + cm._chang_base_caption_search("교환") + "'";
            Tsql = Tsql + "  When ReturnTF = 3 Then '" + cm._chang_base_caption_search("부분반품") + "'";
            Tsql = Tsql + "  When ReturnTF = 5 Then '" + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END ";

            Tsql = Tsql + " ,TotalPrice ";
            Tsql = Tsql + " ,TotalInputPrice ";
            Tsql = Tsql + " ,TotalPV ";
            Tsql = Tsql + ", TotalCV ";

            Tsql = Tsql + " ,InputCash ";
            Tsql = Tsql + " ,InputCard ";
            Tsql = Tsql + " ,InputPassbook ";
            Tsql = Tsql + " ,Etc1 ";

            Tsql = Tsql + " From tbl_SalesDetail (nolock) ";
            //Tsql = Tsql + " Left Join tbl_SalesDetail_TF (nolock) On tbl_SalesDetail_TF.OrderNumber =tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " Left Join tbl_SellType (nolock) On tbl_SellType.SellCode =tbl_SalesDetail.SellCode ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_SalesDetail' And  Ch_T.M_Detail = Convert(Varchar,tbl_SalesDetail.ReturnTF ) ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By OrderNumber ASC ";
        } // end Sell_dGridView_Info_Put



        private void Mem_change_dGridView_Info_Put( string Mbid, int Mbid2 , string Ordernumber, ref string Tsql )
        {

            Tsql = "Select  ";
            Tsql = Tsql + " A.ModRecordtime ";
            Tsql = Tsql + " ,Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " Ch_Detail ";
            Tsql = Tsql + " ,BeforeDetail ";
            Tsql = Tsql + " ,AfterDetail ";
            Tsql = Tsql + " ,A.ModRecordid ";

            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";

            Tsql = Tsql + " FROM tbl_Memberinfo_Mod AS A (nolock) " ;
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo_Mod_Detail Ch_T  (nolock) ON Ch_T.M_Detail = A.ChangeDetail";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON A.Mbid = B.Mbid And A.Mbid2 = B.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business         (nolock) ON B.BusinessCode = tbl_Business.ncode And b.Na_code = tbl_Business.Na_code ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where B.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where b.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   B.Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And Ch_T." + cls_app_static_var.Base_M_Detail_Ex + " IS NOT NULL ";
            Tsql = Tsql + " Order By Modrecordtime DESC ";
       } // end Mem_change_dGridView_Info_Put

        private void Mem_UP_change_dGridView_Info_Put( string Mbid, int Mbid2 , string Ordernumber , ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();
            string save_C = cm._chang_base_caption_search("후원인_변경");
            string nom_C = cm._chang_base_caption_search("추천인_변경");

            Tsql = "Select  ";
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.recordtime ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.Old_mbid2) ";
            else
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 ";
            Tsql = Tsql + " ,A.M_name AS oldname ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid + '-' + Convert(Varchar,tbl_Memberinfo_Save_Nomin_Change.New_mbid2) ";
            else
                Tsql = Tsql + ", tbl_Memberinfo_Save_Nomin_Change.New_mbid2 ";
            Tsql = Tsql + " ,B.M_name AS Newname";

            //Tsql = Tsql + " , Ch_T." + cls_app_static_var.Base_M_Detail_Ex + "  Ch_Detail ";
            Tsql = Tsql + " , Case When Save_Nomin_SW = 'Sav' Then '" + save_C + "' ELSE '" + nom_C + "' END";
            Tsql = Tsql + " ,tbl_Memberinfo_Save_Nomin_Change.Recordid ";

            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";


            Tsql = Tsql + " FROM      tbl_Memberinfo_Save_Nomin_Change  (nolock) ";

            Tsql = Tsql + " Left JOIN tbl_Memberinfo A (nolock)  ON"; 
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.Old_mbid = A.mbid ";
            Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.Old_mbid2 = A.mbid2 ";

            Tsql = Tsql + " Left Join tbl_Memberinfo B (nolock) ON ";
            Tsql = Tsql + " tbl_Memberinfo_Save_Nomin_Change.New_mbid = B.Mbid";
            Tsql = Tsql + " And tbl_Memberinfo_Save_Nomin_Change.New_mbid2 = B.Mbid2";

            //Tsql = Tsql + " LEFT JOIN tbl_Base_Change_Detail Ch_T (nolock) ON Ch_T.M_Detail_S = 'tbl_Memberinfo_Save_Nomin_Change' And  Ch_T.M_Detail = tbl_Memberinfo_Save_Nomin_Change.Save_Nomin_SW ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo_Save_Nomin_Change.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo_Save_Nomin_Change.Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By tbl_Memberinfo_Save_Nomin_Change.recordtime DESC  ";
        } // end Mem_UP_change_dGridView_Info_Put

        private void Mem_Add_dGridView_Info_Put( string Mbid, int Mbid2 , string Ordernumber , ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth ();
            Tsql = "Select  ";

            Tsql = Tsql + " Case When Sort_Add = 'C' Then '" + cm._chang_base_caption_search("직장") + "'";
            Tsql = Tsql + "  When Sort_Add = 'R' Then '" + cm._chang_base_caption_search("기본배송지") + "'";
            Tsql = Tsql + " END ";

            Tsql = Tsql + " ,ETC_Addcode1   ";
            Tsql = Tsql + " ,ETC_Address1 ";
            Tsql = Tsql + " ,ETC_Address2 ";

            Tsql = Tsql + " ,ETC_Tel_1 ";
            Tsql = Tsql + " ,ETC_Tel_2 ";
            Tsql = Tsql + " ,ETC_Name ";


            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + " ,'' ";

            Tsql = Tsql + " From tbl_Memberinfo_Address (nolock) ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order By Sort_Add ASC ";
            

            //당일 등록된 회원을 불러온다.

            
        } //end  Mem_Add_dGridView_Info_Put


        private void Set_SalesItemDetail(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();


            Tsql = "Select tbl_SalesitemDetail.SalesItemIndex ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCode ";
            Tsql = Tsql + " , tbl_Goods.Name Item_Name ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemPrice  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemPV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemCount  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPrice  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalPV  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.ItemTotalCV  ";
            
            Tsql = Tsql + " ,Case When SellState = 'N_1' Then '" + cm._chang_base_caption_search("정상") + "'";
            Tsql = Tsql + "  When SellState = 'N_3' Then '"      + cm._chang_base_caption_search("교환_정상") + "'";
            Tsql = Tsql + "  When SellState = 'R_1' Then '"      + cm._chang_base_caption_search("반품") + "'";
            Tsql = Tsql + "  When SellState = 'R_3' Then '"      + cm._chang_base_caption_search("교환_반품") + "'";
            Tsql = Tsql + "  When SellState = 'C_1' Then '"      + cm._chang_base_caption_search("취소") + "'";
            Tsql = Tsql + " END  SellStateName ";
            Tsql = Tsql + " , tbl_SalesitemDetail.Etc  ";
            Tsql = Tsql + " , tbl_SalesitemDetail.OrderNumber   ";
            
            Tsql = Tsql + " From tbl_SalesitemDetail (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Goods (nolock) ON tbl_Goods.Ncode = tbl_SalesitemDetail.ItemCode ";            
            

            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_SalesitemDetail.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By SalesItemIndex ASC ";
            }
            else
            {
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_SalesitemDetail.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";
            }
        }



        private void Set_Sales_Rece(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            
            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select " ;
            Tsql = Tsql + " tbl_Sales_Rece.SalesItemIndex  ";
            Tsql = Tsql + " ,Case When Receive_Method = 1 Then '" + cm._chang_base_caption_search("직접수령") + "'";
            Tsql = Tsql + "  When Receive_Method = 2 Then '" + cm._chang_base_caption_search("배송") + "'";
            Tsql = Tsql + "  When Receive_Method = 3 Then '" + cm._chang_base_caption_search("센타수령") + "'";
            Tsql = Tsql + "  When Receive_Method = 4 Then '" + cm._chang_base_caption_search("본사직접수령") + "'";           
            Tsql = Tsql + " END  Receive_Method_Name ";
            Tsql = Tsql + " ,Get_Date1 "; 
            Tsql = Tsql + " ,Get_Name1 "; 
            Tsql = Tsql + " ,Get_ZipCode "; 
            Tsql = Tsql + " ,Get_Address1 "; 
            Tsql = Tsql + " ,Get_Address2 "; 
            Tsql = Tsql + " ,Get_Tel1 "; 
            Tsql = Tsql + " ,Get_Tel2 "; 
            Tsql = Tsql + " ,Get_Etc1 ";
            Tsql = Tsql + " , tbl_Sales_Rece.OrderNumber   ";
            Tsql = Tsql + " , Get_state, Get_city ";

            Tsql = Tsql + " From tbl_Sales_Rece (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Base_Rec (nolock) on tbl_Base_Rec.ncode = tbl_Sales_Rece.Base_Rec ";            
            

            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_Sales_Rece.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By SalesItemIndex ASC ";
            }
            else
            {
                Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Rece.OrderNumber ";
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  SalesItemIndex ASC ";
            }
        }


        private void Set_Sales_Cacu(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {
            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select tbl_Sales_Cacu.C_index ";
            Tsql = Tsql + " ,Case When C_TF = 1 Then '" + cm._chang_base_caption_search("현금") + "'";
            Tsql = Tsql + "  When C_TF = 2 Then '" + cm._chang_base_caption_search("무통장") + "'";
            Tsql = Tsql + "  When C_TF = 3 Then '" + cm._chang_base_caption_search("카드") + "'";
            Tsql = Tsql + "  When C_TF = 4 Then '" + cm._chang_base_caption_search("마일리지") + "'";
            Tsql = Tsql + "  When C_TF = 5 Then '" + cm._chang_base_caption_search("가상계좌") + "'";            
            Tsql = Tsql + " END  C_TF_Name ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Price1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_AppDate1  ";
            Tsql = Tsql + " ,Case When Isnull(tbl_Bank.bankname , '') <> '' then Isnull(tbl_Bank.bankname , '') ELSE tbl_Sales_Cacu.C_CodeName END ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Number1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name1  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Name2  ";
            Tsql = Tsql + " ,tbl_Sales_Cacu.C_Etc  ";
            Tsql = Tsql + " , tbl_Sales_Cacu.OrderNumber   ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_SalesDetail (nolock) ON tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber ";
            Tsql = Tsql + " LEFT JOIN tbl_BankForCompany (nolock) ON tbl_Sales_Cacu.C_Code = tbl_BankForCompany.BankCode And  tbl_Sales_Cacu.C_Number1 = tbl_BankForCompany.BankAccountNumber And tbl_SalesDetail.Na_Code = tbl_BankForCompany.Na_Code  ";

            Tsql = Tsql + " LEFT JOIN tbl_Bank (nolock) ON Right(tbl_Sales_Cacu.C_Code,2)  = Right(tbl_Bank.Ncode,2)  And tbl_Sales_Cacu.C_TF = 5   ";
            cls_NationService.SQL_BankNationCode(ref Tsql);

            if (Ordernumber != "")
            {
                Tsql = Tsql + " Where tbl_Sales_Cacu.OrderNumber = '" + Ordernumber.ToString() + "'";
                Tsql = Tsql + " Order By C_index ASC ";
            }
            else
            {
                
                if (Mbid.Length == 0)
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                else
                {
                    Tsql = Tsql + " Where tbl_SalesDetail.Mbid = '" + Mbid + "' ";
                    Tsql = Tsql + " And   tbl_SalesDetail.Mbid2 = " + Mbid2.ToString();
                }
                Tsql = Tsql + " Order By tbl_SalesDetail.OrderNumber DESC,  C_index ASC ";
            }
            
        }


        private void Set_Pay(string  Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql =  " Select ST1 ";
            Tsql = Tsql + ", LEFT(ToEndDate,4) +'-' + LEFT(RIGHT(ToEndDate,4),2) + '-' + RIGHT(ToEndDate,2) ";
            Tsql = Tsql + ",LEFT(PayDate,4) +'-' + LEFT(RIGHT(PayDate,4),2) + '-' + RIGHT(PayDate,2) " ;
    
            Tsql = Tsql + " ,SumAllAllowance " ;
            Tsql = Tsql + " ,InComeTax " ;
            Tsql = Tsql + " ,ResidentTax " ;
            Tsql = Tsql + " ,TruePayment " ;
    
            Tsql = Tsql + " ,'','','' ,'' " ;
            Tsql = Tsql + " From ";    
    
            Tsql = Tsql + "  ( ";
            ////Tsql = Tsql + " Select '주간_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            ////Tsql = Tsql + " From tbl_ClosePay_01_Mod (nolock)  " ;            
            ////if (Mbid.Length == 0)
            ////    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            ////else
            ////{
            ////    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            ////    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            ////}
            ////Tsql = Tsql + " And  SumAllAllowance >0 "; 
                    
            ////Tsql = Tsql + " UNION ALL" ;

            Tsql = Tsql + " Select '주간_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  " ;            
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  SumAllAllowance >0 "; 

            Tsql = Tsql + " UNION ALL";

            Tsql = Tsql + " Select '월_마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            Tsql = Tsql + " From tbl_ClosePay_04_Mod (nolock)  ";
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " And  SumAllAllowance >0 "; 

            //Tsql = Tsql + " UNION ALL";

            //Tsql = Tsql + " Select '센타마감' ST1, PayDate,ToEndDate, SumAllAllowance , InComeTax , ResidentTax , TruePayment ";
            //Tsql = Tsql + " From tbl_ClosePay_100_Mod (nolock)  ";
            //if (Mbid.Length == 0)
            //    Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            //else
            //{
            //    Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
            //    Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            //}
            //Tsql = Tsql + " And  SumAllAllowance >0 "; 


            Tsql = Tsql + " )AS C  ";
            Tsql = Tsql + " Order By PayDate DESC ";

        }



        private void Set_Memberinfo (string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " tbl_Memberinfo.mbid + '-' + Convert(Varchar,tbl_Memberinfo.mbid2) ";
            else
                Tsql = Tsql + " tbl_Memberinfo.mbid2 ";

            Tsql = Tsql + " ,tbl_Memberinfo.M_Name ";

            Tsql = Tsql + ", tbl_Memberinfo.Cpno ";

            Tsql = Tsql + " , ISNULL(C1.Grade_Name,'') ";
            Tsql = Tsql + " , tbl_Memberinfo.LineCnt ";

            Tsql = Tsql + " ,Isnull(tbl_Business.Name,'') as B_Name";
            Tsql = Tsql + " , LEFT(tbl_Memberinfo.RegTime,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.RegTime,4),2) + '-' + RIGHT(tbl_Memberinfo.RegTime,2)   ";
            Tsql = Tsql + " , tbl_Memberinfo.hometel ";
            Tsql = Tsql + " , tbl_Memberinfo.hptel ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.Ed_Date <> '' Then  LEFT(tbl_Memberinfo.Ed_Date,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.Ed_Date,4),2) + '-' + RIGHT(tbl_Memberinfo.Ed_Date,2) ELSE '' End Ed_Date_2 ";



            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid + '-' + Convert(Varchar,tbl_Memberinfo.Saveid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Saveid2 ";

            Tsql = Tsql + " , Isnull(Sav.M_Name,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid + '-' + Convert(Varchar,tbl_Memberinfo.Nominid2) ";
            else
                Tsql = Tsql + " ,tbl_Memberinfo.Nominid2 ";

            Tsql = Tsql + " , Isnull(Nom.M_Name,'') ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.addcode1 <> '' Then  LEFT(tbl_Memberinfo.addcode1,3) +'-' + RIGHT(tbl_Memberinfo.addcode1,3) ELSE '' End ";

            Tsql = Tsql + " , tbl_Memberinfo.address1 ";
            Tsql = Tsql + " , tbl_Memberinfo.address2 ";
            Tsql = Tsql + " , tbl_Bank.BankName ";
            Tsql = Tsql + " , tbl_Memberinfo.bankaccnt ";
            Tsql = Tsql + " , tbl_Memberinfo.bankowner ";
            Tsql = Tsql + " , Case  When tbl_Memberinfo.Sell_Mem_TF = 0 then '" + cm._chang_base_caption_search("판매원") + "' ELSE  '" + cm._chang_base_caption_search("소비자") + "' End AS Sell_MEM_TF2";


            //Tsql = Tsql + " , Case tbl_Memberinfo.LeaveCheck When 1 then '" + cm._chang_base_caption_search("활동") + "' When 0 then '" + cm._chang_base_caption_search("탈퇴") + "' End AS LeaveCheck_2 ";

            Tsql = Tsql + " , Case  ";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 1 Then '" + cm._chang_base_caption_search("활동") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = 0 Then '" + cm._chang_base_caption_search("탈퇴") + "'";
            Tsql = Tsql + "  When tbl_Memberinfo.LeaveCheck = -100 Then '" + cm._chang_base_caption_search("휴면") + "'";
            Tsql = Tsql + "  End AS LeaveCheck_2 ";

            Tsql = Tsql + " , Case tbl_Memberinfo.LineUserCheck When 1 then '" + cm._chang_base_caption_search("사용") + "' When 0 then '" + cm._chang_base_caption_search("중지") + "' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LeaveDate <> '' Then  LEFT(tbl_Memberinfo.LeaveDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LeaveDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LeaveDate,2) ELSE '' End ";
            Tsql = Tsql + " , Case When tbl_Memberinfo.LineUserDate <> '' Then  LEFT(tbl_Memberinfo.LineUserDate,4) +'-' + LEFT(RIGHT(tbl_Memberinfo.LineUserDate,4),2) + '-' + RIGHT(tbl_Memberinfo.LineUserDate,2) ELSE '' End ";
            Tsql = Tsql + " , tbl_Memberinfo.recordid ";

            Tsql = Tsql + " , tbl_Memberinfo.recordtime ";

            Tsql = Tsql + " From tbl_Memberinfo (nolock) ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Sav (nolock) ON tbl_Memberinfo.Saveid = Sav.Mbid And tbl_Memberinfo.Saveid2 = Sav.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Memberinfo Nom (nolock) ON tbl_Memberinfo.Nominid = Nom.Mbid And tbl_Memberinfo.Nominid2 = Nom.Mbid2 ";
            Tsql = Tsql + " LEFT JOIN tbl_Business (nolock) ON tbl_Memberinfo.BusinessCode = tbl_Business.NCode And tbl_Memberinfo.Na_code = tbl_Business.Na_code ";
            Tsql = Tsql + " Left Join tbl_Bank On tbl_Memberinfo.bankcode=tbl_Bank.ncode ";
            cls_NationService.SQL_BankNationCode(ref Tsql);
            Tsql = Tsql + " Left Join tbl_Class C1 On tbl_Memberinfo.CurGrade=C1.Grade_Cnt ";

            if (Mbid.Length == 0)
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where tbl_Memberinfo.Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   tbl_Memberinfo.Mbid2 = " + Mbid2.ToString();
            }

        }

        private void Set_Memberinfo_Talk(string Mbid, int Mbid2, string Ordernumber, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();

            Tsql = "Select  ";            
            Tsql = Tsql + " TalkContent ";

            Tsql = Tsql + " ,Recordid ";

            Tsql = Tsql + ", Recordtime ";

            Tsql = Tsql + " , Seq ";
            Tsql = Tsql + " , ''   ,'','','','','' ";
                   

            Tsql = Tsql + " From tbl_Memberinfo_Talk (nolock) ";
            
            if (Mbid.Length == 0)
                Tsql = Tsql + " Where Mbid2 = " + Mbid2.ToString();
            else
            {
                Tsql = Tsql + " Where Mbid = '" + Mbid + "' ";
                Tsql = Tsql + " And   Mbid2 = " + Mbid2.ToString();
            }
            Tsql = Tsql + " Order by Seq DESC  ";
        }


        private void Set_Memberinfo_Up(string Mbid, int Mbid2, string Ordernumber, string S_TF , ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();                                                       

            
            //Tsql = "Select  ";
            //Tsql = Tsql + " T_AA.Lvl ";
            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            //else
            //    Tsql = Tsql + ", T_AA.mbid2 ";

            //Tsql = Tsql + " ,Isnull(CC_A.G_Name,'') ";
            //Tsql = Tsql + " ,A.M_Name ";
            //Tsql = Tsql + " , Case When A.Regtime <> '' Then  LEFT(A.Regtime,4) +'-' + LEFT(RIGHT(A.Regtime,4),2) + '-' + RIGHT(A.Regtime,2) ELSE '' End ";
            //Tsql = Tsql + " , Case When A.LeaveDate <> '' Then  LEFT(A.LeaveDate,4) +'-' + LEFT(RIGHT(A.LeaveDate,4),2) + '-' + RIGHT(A.LeaveDate,2) ELSE '' End ";
            //Tsql = Tsql + ", Isnull( tbl_Business.name,'') " ;


            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            //else
            //    Tsql = Tsql + " ,A.Saveid2 ";

            //Tsql = Tsql + " , Isnull(b.M_Name,'') ";

            //if (cls_app_static_var.Member_Number_1 > 0)
            //    Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            //else
            //    Tsql = Tsql + " ,A.Nominid2 ";

            //Tsql = Tsql + " , Isnull(C.M_Name,'') ";
            //Tsql = Tsql + " , A.hometel ";
            //Tsql = Tsql + " , A.hptel ";

            //Tsql = Tsql + " , '' ";


            //if (S_TF == "SAVE")
            //{
            //    Tsql = Tsql + " , A.LineCnt ";
            //    Tsql = Tsql + " From ufn_matrix_Mem_mannatech('" + Mbid + "'," + Mbid2 + ", '*') T_AA ";
            //}
            //else
            //{
            //    Tsql = Tsql + " , A.N_LineCnt ";
            //    Tsql = Tsql + " From ufn_matrix_Nominid_mannatech('" + Mbid + "'," + Mbid2 + ",  '*') T_AA ";
            //}
       
            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS A  (nolock) ON A.Mbid = T_AA.mbid And A.Mbid2 = T_AA.Mbid2   ";
            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS B  (nolock) ON a.Saveid = b.mbid And a.Saveid2 = b.mbid2   ";
            //Tsql = Tsql + " LEFT JOIN tbl_Memberinfo AS C  (nolock) ON a.Nominid=c.mbid And a.Nominid2 = c.mbid2   ";
            //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode  And a.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt " ;
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On CC_A.Mbid = A.Mbid And  CC_A.Mbid2 = A.Mbid2 ";            
    
            //Tsql = Tsql + " ORder by Lvl DESC";



            Tsql = "Select  ";
            Tsql = Tsql + " T_AA.Lvl ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            else
                Tsql = Tsql + ", T_AA.mbid2 ";

            Tsql = Tsql + " ,Isnull(C1.Grade_Name,'') ";
            Tsql = Tsql + " ,A.lastname+A.firstname ";
            Tsql = Tsql + " , Case When replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-','')   <> ''   Then  LEFT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4) +'-'  + LEFT(RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4),2) + '-'  + RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),2) ELSE '' End  ";
            Tsql = Tsql + " ,'' ";
            Tsql = Tsql + ", '' ";


            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            else
                Tsql = Tsql + " ,A.sponsoralkynumber ";

            Tsql = Tsql + " ,  Isnull(B.lastname+B.firstname,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            else
                Tsql = Tsql + " ,A.enrolleralkynumber ";

            Tsql = Tsql + " , Isnull(C.lastname+C.firstname ,'') ";
            Tsql = Tsql + " ,A.phonenumber  ";
            Tsql = Tsql + " , A.phonenumber  ";

            Tsql = Tsql + " , '' ";


            if (S_TF == "SAVE")
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_matrix_Mem_mannatech ('" + Mbid + "'," + Mbid2 + ", '*') T_AA ";
            }
            else
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_matrix_Nominid_mannatech ('" + Mbid + "'," + Mbid2 + ",  '*') T_AA ";
            }

            Tsql = Tsql + "LEFT JOIN  mannasync.dbo.CUSTOMER  AS A  (nolock) ON A.accountnumber = Convert(Varchar,T_AA.Mbid2)    ";
            Tsql = Tsql + " LEFT JOIN  mannasync.dbo.CUSTOMER  AS B  (nolock) ON A.sponsoralkynumber = B.accountnumber   ";
            Tsql = Tsql + "  LEFT JOIN  mannasync.dbo.CUSTOMER  AS C  (nolock) ON A.enrolleralkynumber = C.accountnumber     ";

            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On T_AA.Mbid = tbl_Memberinfo.Mbid   And T_AA.Mbid2 = tbl_Memberinfo.Mbid2 ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_Memberinfo.CurGrade = C1.Grade_Cnt ";
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On  CC_A.Mbid2 = A.accountnumber ";

            Tsql = Tsql + " ORder by Lvl DESC";
      }




        private void Set_Memberinfo_Down(string Mbid, int Mbid2, string Ordernumber, string S_TF, ref string Tsql)
        {

            cls_form_Meth cm = new cls_form_Meth();


            Tsql = "Select  ";
            Tsql = Tsql + " T_AA.Lvl ";
            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + ", T_AA.mbid + '-' + Convert(Varchar,T_AA.mbid2) ";
            else
                Tsql = Tsql + ", T_AA.mbid2 ";

            Tsql = Tsql + " , Isnull(C1.Grade_Name,'') ";
            Tsql = Tsql + " ,A.lastname+A.firstname ";
            Tsql = Tsql + " , Case When replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-','')   <> ''   Then  LEFT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4) +'-'  + LEFT(RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),4),2) + '-'  + RIGHT(replace(CONVERT(VARCHAR(10), A.lastrenewaldate, 121),'-',''),2) ELSE '' End   ";
            Tsql = Tsql + " , ''";
            Tsql = Tsql + ", '' ";


            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Saveid + '-' + Convert(Varchar,A.Saveid2) ";
            else
                Tsql = Tsql + " ,A.sponsoralkynumber";

            Tsql = Tsql + " , Isnull(B.lastname+B.firstname,'') ";

            if (cls_app_static_var.Member_Number_1 > 0)
                Tsql = Tsql + " ,A.Nominid + '-' + Convert(Varchar,A.Nominid2) ";
            else
                Tsql = Tsql + " ,A.enrolleralkynumber ";

            Tsql = Tsql + " , Isnull(C.lastname+C.firstname ,'') ";
            Tsql = Tsql + " , A.phonenumber ";
            Tsql = Tsql + " , A.phonenumber ";

            Tsql = Tsql + " , '' ";

            if (S_TF == "SAVE")
            {
                Tsql = Tsql + " , '1'  ";
                Tsql = Tsql + " From ufn_GetSubTree_MemGroup_mannatech ('" + Mbid + "'," + Mbid2 + ") T_AA ";
            }
 
            else
            {
                Tsql = Tsql + " , '1' ";
                Tsql = Tsql + " From ufn_GetSubTree_NomGroup_mannatech ('" + Mbid + "'," + Mbid2 + ") T_AA ";
             }

            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS A  (nolock) ON A.accountnumber = Convert (varchar, T_AA.Mbid2)    ";
            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS B  (nolock) ON A.sponsoralkynumber = B.accountnumber   ";
            Tsql = Tsql + " LEFT JOIN mannasync.dbo.CUSTOMER AS C  (nolock) ON A.enrolleralkynumber = C.accountnumber   ";
            //Tsql = Tsql + " LEFT Join tbl_Business  (nolock) On a.businesscode=tbl_Business.ncode And a.Na_code = tbl_Business.Na_code";
            //Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On A.CurGrade=C1.Grade_Cnt ";
            //Tsql = Tsql + " Left Join ufn_Mem_CurGrade_Mbid_Search ('',0) AS CC_A On  CC_A.Mbid2 = A.accountnumber";            


            Tsql = Tsql + " LEFT Join tbl_Memberinfo  (nolock) On T_AA.Mbid = tbl_Memberinfo.Mbid   And T_AA.Mbid2 = tbl_Memberinfo.Mbid2 ";
            Tsql = Tsql + " Left Join tbl_Class C1  (nolock) On tbl_Memberinfo.CurGrade = C1.Grade_Cnt ";

            Tsql = Tsql + " Where T_AA.Lvl > 0 ";
            Tsql = Tsql + " ORder by Lvl ASC, LEFT(SaveCur,3) ASC   , SaveCur ASC ";

        }


        private void Set_gr_dic_Info(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            int Col_Cnt = 0;

            object[] row0 = new object[Base_dgv.grid_col_Count];

            while (Col_Cnt < Base_dgv.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt].ToString ();
                Col_Cnt++;
            }            

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_gr_dic_Info(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt , int Sort_Number)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);
            int Col_Cnt = 0;

            object[] row0 = new object[Base_dgv.grid_col_Count];

            while (Col_Cnt < Base_dgv.grid_col_Count)
            {
                row0[Col_Cnt] = ds.Tables[base_db_name].Rows[fi_cnt][Col_Cnt];
                Col_Cnt++;
            }

            gr_dic_text[fi_cnt + 1] = row0;
        }


        private void Set_gr_dic_Info_Cacu(ref DataSet ds, ref Dictionary<int, object[]> gr_dic_text, int fi_cnt)
        {
            StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

            object[] row0 = { ds.Tables[base_db_name].Rows[fi_cnt][0] , 
                                ds.Tables[base_db_name].Rows[fi_cnt][1],
                                ds.Tables[base_db_name].Rows[fi_cnt][2] , 
                                ds.Tables[base_db_name].Rows[fi_cnt][3] , 
                                ds.Tables[base_db_name].Rows[fi_cnt][4], 

                                encrypter.Decrypt ( ds.Tables[base_db_name].Rows[fi_cnt][5].ToString () ), 
                                ds.Tables[base_db_name].Rows[fi_cnt][6] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][7] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][8] ,
                                ds.Tables[base_db_name].Rows[fi_cnt][8] ,
                                                                
                                 };

            
            gr_dic_text[fi_cnt + 1] = row0;
        }
                       
        }//end cls_Grid_Base_info_Put






    } 


