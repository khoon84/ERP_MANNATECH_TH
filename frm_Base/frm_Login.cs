﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Management;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
//using System.Net.NetworkInformation ;


namespace MLM_Program
{



  


    public partial class frm_Login : Form
    {


        private int New_Ver = 0;
        private int Be_Ver = 0;
        private string New_FileName = "";
        private string U_TIP = ""; private string U_TID = ""; private string U_TPW = ""; private string U_Port = "";
        private int UpLoad_TF = 0;

        FTP Base_ftp = null;

        public frm_Login()
        {
            InitializeComponent();            
        }
         

        private void txtUserID_KeyPress(object sender, KeyPressEventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();

            T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);

            if (T_R.Text_KeyChar_Check(e) == false)
            {                               
                e.Handled = true;
                return;
            } // end if           

        }
           

        private void txtPass_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            //cls_Check_Text T_R = new cls_Check_Text();

            //T_R.Key_Enter_13 += new Key_13_Event_Handler(T_R_Key_Enter_13);


            

            //if (T_R.Text_KeyChar_Check(e) == false)
            //{
            //    e.Handled = true;
            //    return;
            //} // end if      


            if (e.KeyChar == 34 || e.KeyChar == 39)
            {
                e.Handled = true;
                return;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    EventArgs e1 = null;
                    btn_Login_Click(lbl_Login, e1);
                }
                //e.Handled = true;
                //return;
            }
           
        }



        void T_R_Key_Enter_13()
        {
            SendKeys.Send("{TAB}");
        }



        private void txtUserID_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();
            T_R.Text_Focus_All_Sel((TextBox)sender);
        }

        private void txtPass_Enter(object sender, EventArgs e)
        {
            cls_Check_Text T_R = new cls_Check_Text();
            T_R.Text_Focus_All_Sel((TextBox)sender);
        }




        private void frm_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape )
            {
                this.Close();
            }// end if
        }

//--------------------------------------------//
        private Boolean Check_Text_Box()
        {
            cls_Check_Text T_R = new cls_Check_Text();

            string me = T_R.Text_Null_Check(txtUserID);
            if (me != "")
            {
                MessageBox.Show (me);
                //txtUserID.Select();
                return false ;
            }
                        
            me = T_R.Text_Null_Check(txtPass);
            if (me != "")
            {
                MessageBox.Show(me);
                //txtPass.Select();
                return false;
            }

            return true ;
        }

        class PCInfo : IDisposable
        {
            public string PCName = string.Empty;
            public string IP_V6_Addr = string.Empty;
            public string IP_V4_Addr = string.Empty;
            public List<string> DiskInfos = new List<string>();

            #region IDisposable Support
            private bool disposedValue = false; // 중복 호출을 검색하려면

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    }

                    // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                    // TODO: 큰 필드를 null로 설정합니다.

                    disposedValue = true;
                }
            }

            // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
            // ~PCInfo() {
            //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            //   Dispose(false);
            // }

            // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
            public void Dispose()
            {
                // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
                Dispose(true);
                // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }


//--------------------------------------------//
        private void btn_Login_Click(object sender, EventArgs e)
        {

            //DateTime utcdt;
            ////System.Globalization.CultureInfo.CreateSpecificCulture("ja-JP")
            //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ja-JP"); //System.Globalization.CultureInfo("ja-JP");
            ////System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ko-KR");
            ////en-US             
            ////System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            ////utcdt = DateTime.UtcNow;

            //utcdt = DateTime.Now.ToUniversalTime();

            //MessageBox.Show("일본" + utcdt.ToString(culture));



            if (txtUserID.Text.Trim() == "" && txtPass.Text.Trim() == "")
            {
                string u_ip, u_computername;
                Hard_Number(out u_ip, out  u_computername);

                if (u_computername.ToUpper() == "DESKTOP-657BT4V".ToUpper() ||  u_computername.ToUpper() == "LANCE1".ToUpper() || u_computername.ToUpper() == "ilsong-dev".ToUpper() || u_computername.ToUpper() == "JRED-PC" || u_computername.ToUpper() == "DESKTOP-DCOJLJ6" || u_computername.ToUpper() == "DESKTOP-UN61U37" || u_computername.ToUpper() == "design1".ToUpper()
                    || u_computername.ToUpper() == "ilsong-csdev".ToUpper() // syhuh
                    || u_computername.ToUpper() == "LANCE31".ToUpper() // syhuh
                    ) 
                {
                    txtUserID.Text = cls_User.SuperUserID ;
                    txtPass.Text = cls_User.SuperUserPassWd;
                }
            }// end if


            if (Check_Text_Box() == false)
            {
                return;
            }// end if

            if (cls_User.SuperUserID.ToUpper() == (txtUserID.Text.Trim()).ToUpper() )                
            {
                if (cls_User.SuperUserPassWd.ToUpper() == (txtPass.Text.Trim()).ToUpper())
                {
                    Super_id_Connect();
                    MDi_Form_Load();
                    return;
                }
                else
                {                    
                    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Login_PW"));
                    txtPass.SelectAll();
                    txtPass.Focus();
                    return;
                }
            }// end if
            




            //로그인햇다고 tbl_user테이블에 흔적을 남기고. 로그인한 시간을 남긴다.
            if (Connect_User_Check_01() == true) 
            {
                Connect_User_Check_02(); //로그인한 컴터 아이피와 컴터이름을 가져오고. 로그인한 시간을 전역 변수에 넣는다.

                Connect_User_Check_03();// 사용자별 로그인 로그아웃 시간 테이블에 값을 넣는다.

                Connect_User_Set_File(); //파일에 마지막에 접속한 사람을 남긴다. 추후 이컴터에서 로그인한 사람을 그아이디가 다시 나오도록 하기 위함.

                

                MDi_Form_Load();
            }
            else
            {
                txtUserID.SelectAll();
                txtUserID.Focus();
            } // end if


        }

   


//--------------------------------------------//
        private Boolean  Connect_User_Check_01()
        {                           
            cls_Connect_DB Temp_Connect =new cls_Connect_DB ();                        
            string TSql;
            TSql = "Select U_Name, user_password, Log_Check,CenterCode , Menu1,";
            TSql = TSql + " SellInput , FarMenu , ETC_TF, LanNumber, Log_Date,log_check ";
            TSql = TSql + " , Na_code ,  Leave_TF ";
            TSql = TSql + " , Cpno_V_TF , Excel_Save_TF, For_Save_TF , CC_Save_TF  , Sell_Info_V_TF, Tree_Config , Cash_V_TF  , Card_Num_V_TF , Card_Sugi_TF , Sell_Mem_TF_Ch_TF, Name_Ch_TF ";
            TSql = TSql + " ,Nominid_Ch_TF, Rec_Ch_TF , Talk_In_TF ";//, Return_Cacu_Save_FLAG , Return_Cacu_Cancel_FLAG   "; 
            TSql = TSql + " From tbl_user  (nolock)  ";
            TSql = TSql + " Where upper(user_id) = '" + ((txtUserID.Text).Trim()).ToUpper() + "'";
            TSql = TSql + " And Leave_TF = 0 ";  //퇴사처리 되지 않은 사람만 로그인이 가능하다
            TSql = TSql + " And Using_TF = 0 ";  //프로그램 사용으로 체크한 사람만 조회가 되도록 한다.

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(TSql, "tbl_user", ds) == false) return false;

            int ReCnt = Temp_Connect.DataSet_ReCount;
            
            if (ReCnt == 0)
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Login_ID"));                
                return false ;
            }// end if

            cls_User.gid_CountryCode = ds.Tables["tbl_user"].Rows[0]["Na_code"].ToString();

            //메세지 관련 리소스를 넣어둔다.
            if (cls_User.gid_CountryCode == "" || cls_User.gid_CountryCode == "La" || cls_User.gid_CountryCode == "KR")
                cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Msg_Resource";
            if (cls_User.gid_CountryCode == "Ja")
                cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Japan_Msg_Resource";

            if (cls_User.gid_CountryCode == "US")
                cls_app_static_var.app_msg_resource = "MLM_Program.Resources.US_Msg_Resource";
            if (cls_User.gid_CountryCode == "TH")
                cls_app_static_var.app_msg_resource = "MLM_Program.Resources.US_Msg_Resource";
            //if (cls_app_static_var.Using_language == "Korean" || cls_app_static_var.Using_language == "")
            //{
            //    cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Msg_Resource";
            //}

            //if (cls_app_static_var.Using_language == "English")
            //{
            //    cls_app_static_var.app_msg_resource = "MLM_Program.Resources.US_Msg_Resource";
            //}

            //if (cls_app_static_var.Using_language == "Japanese")
            //{
            //    cls_app_static_var.app_msg_resource = "MLM_Program.Resources.Japan_Msg_Resource";
            //}

            //if (cls_app_static_var.Using_language == "Thai")
            //{
            //    cls_app_static_var.app_msg_resource = "MLM_Program.Resources.US_Msg_Resource";  // 230817 - 허성윤, 태국건은 영문으로 나오도록 설정
            //}

            cls_app_static_var.app_msg_rm = new System.Resources.ResourceManager(cls_app_static_var.app_msg_resource, cls_app_static_var.Assem); //국가별 메시지 리소스 찾기

            
            string u_password = ds.Tables["tbl_user"].Rows[0]["user_password"].ToString();

            if (u_password.Trim() == (txtPass.Text).Trim())
            {
                //admin은 몇명이든 들어가게 한다. 또 오류가 생겨서 프로그램 팅겻을 경우. 들어가는 사람 
                //하나는 있어야 하기에 admin은 막들어가게 해줌.
                if ("admin".ToUpper() != (txtUserID.Text.Trim()).ToUpper())
                {
                    ////동일한 아이디로 다른 컴터로 두명 동시 접속 못하게 막아버림.
                    //if (ds.Tables["tbl_user"].Rows[0]["Log_Check"].ToString() == "1")
                    //{
                    //    MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Login_ID_2"));
                    //    return false;
                    //} 
                }

                cls_User.gid_CountryCode = ds.Tables["tbl_user"].Rows[0]["Na_code"].ToString();
                cls_User.gid_CenterCode = ds.Tables["tbl_user"].Rows[0]["CenterCode"].ToString();
                cls_User.gid_FarMenu = ds.Tables["tbl_user"].Rows[0]["FarMenu"].ToString();
                cls_User.gid_Menu1 = ds.Tables["tbl_user"].Rows[0]["Menu1"].ToString();                
                cls_User.gid_SellInput = int.Parse(ds.Tables["tbl_user"].Rows[0]["SellInput"].ToString());
                //cls_User.gid_Sell_Del_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Sell_Del_TF"].ToString());
                cls_User.gid_Mem_Del_TF = 0;
                cls_User.gid_Cpno_V_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Cpno_V_TF"].ToString());
                cls_User.gid_Excel_Save_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Excel_Save_TF"].ToString());
                cls_User.gid_For_Save_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["For_Save_TF"].ToString());
                cls_User.gid_CC_Save_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["CC_Save_TF"].ToString());
                cls_User.gid_pan_Info_V_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Sell_Info_V_TF"].ToString());   //메인화면에 승인 관련 판낼을 보여주어라
                cls_User.gid_Tree_Config = ds.Tables["tbl_user"].Rows[0]["Tree_Config"].ToString();   //메인화면에 승인 관련 판낼을 보여주어라
                cls_User.gid_Cash_V_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Cash_V_TF"].ToString());


                cls_app_static_var.Member_Card_Num_Visible_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Card_Num_V_TF"].ToString());
                cls_app_static_var.Member_Card_Sugi_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Card_Sugi_TF"].ToString());
                cls_app_static_var.Member_Sell_Mem_TF_Ch_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Sell_Mem_TF_Ch_TF"].ToString());
                cls_app_static_var.Member_Name_Ch_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Name_Ch_TF"].ToString());
                cls_app_static_var.Member_Nominid_Ch_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Nominid_Ch_TF"].ToString());
                cls_app_static_var.Sales_Rec_Ch_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Rec_Ch_TF"].ToString());
                cls_app_static_var.Member_Talk_In_TF = int.Parse(ds.Tables["tbl_user"].Rows[0]["Talk_In_TF"].ToString());

                //cls_app_static_var.Member_Return_Cacu_Save_FLAG = int.Parse(ds.Tables["tbl_user"].Rows[0]["Return_Cacu_Save_FLAG"].ToString());
                //cls_app_static_var.Member_Return_Cacu_Cancel_FLAG = int.Parse(ds.Tables["tbl_user"].Rows[0]["Return_Cacu_Cancel_FLAG"].ToString());
                
                cls_User.gid_MACAddress = GetMACAddress();

                TSql = "Update tbl_User Set ";
                TSql = TSql + " LanNumber = '" + cls_User.gid_MACAddress + "' ";
                TSql = TSql + " ,log_check = '1' ";
                TSql = TSql + " ,Log_Date = Convert(Varchar(25),GetDate(),21) ";
                TSql = TSql + " Where upper(user_id) = '" + ((txtUserID.Text).Trim()).ToUpper() + "'";
                
                Temp_Connect.Update_Data( TSql);


                //if (("yjchun".Trim()).ToUpper() == txtUserID.Text.Trim().ToUpper())
                //{
                //    cls_User.gid_CenterCode = "004`101`102`103`104" ; 
                //}


                
            
            }
            else
            {
                MessageBox.Show(cls_app_static_var.app_msg_rm.GetString("Msg_Login_PW"));                
                return false;
            }// end else


            return true;

        }

        public string GetMACAddress()
        {

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            String sMacAddress = string.Empty;

            foreach (NetworkInterface adapter in nics)
            {

                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {

                    IPInterfaceProperties properties = adapter.GetIPProperties();

                    sMacAddress = adapter.GetPhysicalAddress().ToString();

                }

            } return sMacAddress;

        }

        private void Super_id_Connect()
        {
            cls_User.gid = cls_User.SuperUserID;
            cls_User.gid_date_time = DateTime.Today.ToString().Substring(0,10).Replace ("-","") ;
            cls_User.gid_CenterCode = "";
            cls_User.gid_FarMenu = "";
            cls_User.gid_Menu1 = "";
            cls_User.gid_SellInput = 0;
            cls_User.gid_Sell_Del_TF = 1;
            cls_User.gid_Mem_Del_TF = 1;
            cls_User.gid_CountryCode = "";
            cls_User.gid_Cpno_V_TF = 1; //주민번호 다 보여줘라
            cls_User.gid_Excel_Save_TF = 1; //엑셀 전환 되게 권한 주어라
            cls_User.gid_For_Save_TF = 1; //엑셀 전환 되게 권한 주어라
            cls_User.gid_CC_Save_TF = 1; //엑셀 전환 되게 권한 주어라
            
            cls_User.gid_pan_Info_V_TF = 1; //메인화면에 승인 관련 판낼을 보여주어라
            cls_User.gid_Cash_V_TF = 1; //매출등록시 현금 결제 가능

            cls_app_static_var.Member_Card_Sugi_TF = 1;
            cls_app_static_var.Member_Card_Num_Visible_TF = 1;
            cls_app_static_var.Member_Sell_Mem_TF_Ch_TF = 1;
            cls_app_static_var.Member_Name_Ch_TF = 1;
            cls_app_static_var.Member_Nominid_Ch_TF = 1;
            cls_app_static_var.Sales_Rec_Ch_TF = 1;
            cls_app_static_var.Member_Talk_In_TF = 1; 

            cls_app_static_var.Member_Return_Cacu_Save_FLAG = 1;
                cls_app_static_var.Member_Return_Cacu_Cancel_FLAG = 0;

            cls_User.gid_CountryCode = "";

            cls_User.gid_CountryCode = "KR";
            
        }

//--------------------------------------------//
        private void Connect_User_Check_02()
        {
            cls_User.gid = (txtUserID.Text).Trim();
            //===================================
            //아이피 주소와 컴퓨터 이름을 알아온다.
            string u_ip, u_computername ;

            Hard_Number(out u_ip, out u_computername);
            cls_User.computer_ip = u_ip;
            cls_User.computer_net_name = Environment.MachineName;
            cls_User.gid_Connect_Time = "";
            //====================================


            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string TSql;
            TSql = "Select Log_Date ";
            TSql = TSql + " From tbl_user  (nolock) ";
            TSql = TSql + " Where upper(user_id) = '" + ((txtUserID.Text).Trim()).ToUpper() + "'";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(TSql, "tbl_user", ds) == false) return;

            //로그인한 날짜를 전역에 보관한다. 컴터 날짜를 변경해서 가끔 문제가 발생 되서 DB상에서 날짜를 가져 오는 거임.
            cls_User.gid_date_time = ds.Tables["tbl_user"].Rows[0]["Log_Date"].ToString();
            cls_User.gid_date_time = cls_User.gid_date_time.Substring(0,10);
            cls_User.gid_date_time=  cls_User.gid_date_time.Replace("-","");
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        }

//--------------------------------------------//
        private void Connect_User_Check_03()
        {
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string TSql;

            TSql = "Insert Into tbl_User_Con_Log Values (";
            TSql = TSql + " '" + ((txtUserID.Text).Trim()).ToUpper() + "' , Convert(Varchar(25),GetDate(),21) , '' ,  ";
            TSql = TSql + " '" + cls_User.computer_ip + "' , '" + cls_User.computer_net_name + "' ) ";

            //테이블에 맞게  insert 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Insert_Data(TSql, "tbl_User_Con_Log") == false) return ;


            TSql = "Select Top 1 Connect_Time ";
            TSql = TSql + " From tbl_User_Con_Log  (nolock) ";
            TSql = TSql + " Where upper(T_U_ID) = '" + ((txtUserID.Text).Trim()).ToUpper() + "'";
            TSql = TSql + " And Connect_IP = '"+  cls_User.computer_ip + "'" ;
            TSql = TSql + " And Connect_C_Name = '" + cls_User.computer_net_name + "'";
            TSql = TSql + " ORder by Connect_Time DESC ";

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(TSql, "tbl_User_Con_Log", ds) == false) return;

            cls_User.gid_Connect_Time = ds.Tables["tbl_User_Con_Log"].Rows[0]["Connect_Time"].ToString();
            
        }


 //--------------------------------------------//
        private bool SuperUserCheck(PCInfo PcInfo)
        {
            PCInfo ThisComputerInfo = new PCInfo();

            //-PCName Check
            ThisComputerInfo.PCName = Environment.MachineName;
            if (!PcInfo.PCName.Equals(ThisComputerInfo.PCName))
                return false;

            //-IP Check
            ThisComputerInfo.IP_V6_Addr = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();
            if (!PcInfo.IP_V6_Addr.Equals(ThisComputerInfo.IP_V6_Addr))
                return false;

            //-MacAddr Check
            ThisComputerInfo.IP_V4_Addr = ( 
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            if (!PcInfo.IP_V4_Addr.Equals(ThisComputerInfo.IP_V4_Addr))
                return false;


            //-Hard Serial Number Check
            foreach (ManagementObject wmi_HD in new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get())
            {
                if(!PcInfo.DiskInfos.Exists(x => x.Trim().Equals(wmi_HD["SerialNumber"].ToString().Trim())))
                    return false;
            }

            return true;
        }

        private void Connect_User_Set_File()
        {
            cls_app_static_var.Using_language = "Korean";
            string ap_path = Application.StartupPath.ToString() ;
            FileStream fs = new FileStream(Path.Combine(ap_path, "LogCon.dat"), FileMode.Create);
            StreamWriter Sw = new StreamWriter(fs);

            Sw.WriteLine(txtUserID.Text.Trim());

            if (pan_Language.Visible == true)
            {
                Sw.WriteLine(cbo_Language.Text.Trim());
                cls_app_static_var.Using_language = cbo_Language.Text.Trim(); 
            }
            Sw.Close();
            fs.Close();
        }

//--------------------------------------------//
        private void  btn_Exit_Click(object sender, EventArgs e)
        {            
            this.Close(); 
        }




//--------------------------------------------//
        private void frm_Login_Load(object sender, EventArgs e)
        {
           
            if (cls_app_static_var.app_multi_lang_query == 1)
            {
                pan_Language.Visible = true;
                cbo_Language.Items.Add("Korean");
                //cbo_Language.Items.Add("English");
                cbo_Language.Items.Add("Thai");
                //cbo_Language.Items.Add("Japanese");
                cbo_Language.SelectedIndex = 0;
            }
            else
                pan_Language.Visible = false ;


            //프로그램이 도는 기본 폴더에 SMS와 프린터 관련 폴더를 생성한다.
            string ap_path = Application.StartupPath.ToString() ;            
            //Directory.CreateDirectory(Path.Combine( ap_path , "SMS"));
            //Directory.CreateDirectory(Path.Combine(ap_path, "rpt"));            
            //Directory.CreateDirectory(Path.Combine(ap_path, "SaveImage"));

            Directory.CreateDirectory(Path.Combine(ap_path, "Doc")); //그림파일등 엑셀 파일 프린터 물이 저장되느 기본 폴더임.
            UpLoad_TF = 0;


            //string str = Environment.SystemDirectory;  //윈도우 깔리 폴더를 알아온다.
            //if (NetworkInterface.GetIsNetworkAvailable() == false)
            //{
            //    MessageBox.Show("네트워크가 연결 상태를 확인 하십시요.");
            //    this.Close();
            //}

            Connect_User_Get_File();

            //lbl_ver.Text  = Assembly.GetEntryAssembly().GetName().Version.ToString();
            
        }

        private void Connect_User_Get_File()
        {
            string ap_path = Application.StartupPath.ToString();

            if (File.Exists (Path.Combine(ap_path, "LogCon.dat")) == true )
            {                
                FileStream fs = new FileStream(Path.Combine(ap_path, "LogCon.dat"), FileMode.Open );
                StreamReader Sw = new StreamReader(fs);

                try
                {
                    txtUserID.Text = Sw.ReadLine().ToString();

                    if (pan_Language.Visible == true  )
                        cbo_Language.Text = Sw.ReadLine().ToString();
                }
                catch
                {
                    return;
                }

                finally
                {
                    Sw.Close();
                    fs.Close();                    
                }                
            }
        }


        private void MDi_Form_Load()
        {
            this.Close();

        }

        private void frm_Login_Activated(object sender, EventArgs e)
        {
           //19-03-11 깜빡임제거 this.Refresh();

            if (UpLoad_TF == 0)
            {
                //프로그램 관련해서 업데이트 받을 사항이 잇는지를 체크한다.
                UpLoad_TF = 1;

                //           if (Environment.MachineName.Equals("LANCE1"))
                //return;
                
                if (!cls_Connect_DB.LiveFlag)
                {
                    //MessageBox.Show("정식 Database가 아니기에 업데이트를 진행하지 않습니다.");
                    //MessageBox.Show(" 업데이트를 진행하지 않습니다.");
                }
                else
                {
                    ProGram_Update_Check();
                }

            }
        }




        private void ProGram_Update_Check()
        {
            New_Ver = 0; New_FileName = ""; Be_Ver = 0;
            U_TIP = ""; U_TID = ""; U_TPW = ""; U_Port = "";

            Check_Connect_FPT_Info();

            if (U_TIP == "")
            {
                MessageBox.Show("update Infomation not found.");
                return;
            }

            string ap_path = Application.StartupPath.ToString();

            FTP ftp = new FTP("ftp://" + U_TIP + ":" + U_Port, U_TID, U_TPW);
            ftp.Send_Download_Result += new FTP.Send_Download_Result_Dele(ftp_Send_Download_Ver_Result);
            ftp.Download("promax_data/Pro_update/" + cls_app_static_var.app_Company_Name, "NewVer.txt", ap_path, progress, true, "B_NewVer.txt");
        }



        void ftp_Send_Download_Ver_Result(int D_Result)
        {
            if (D_Result == -1) return;

            string ap_path = Application.StartupPath.ToString();

            string _sourceFile = Path.Combine(ap_path, "B_NewVer.txt");
            New_FileName = "";
            New_Ver = 0;

            FileInfo fileVer = new FileInfo(_sourceFile);
            if (fileVer.Exists)
            {
                FileStream fs = new FileStream(Path.Combine(ap_path, "B_NewVer.txt"), FileMode.Open);
                StreamReader Sw = new StreamReader(fs);

                try
                {
                    New_Ver = int.Parse(Sw.ReadLine().ToString());
                    New_FileName = Sw.ReadLine().ToString();                    
                }
                catch 
                {
                    return;
                }
                
                finally
                {
                    Sw.Close();
                    fs.Close();

                    fileVer.Delete();   //앞서 내려 받은 새로운 버전정보 파일을 삭제 시킨다.
                }
               
            }

            int F_SW = 0; 
            string _First_ver_File = Path.Combine(ap_path, "F_Ver.txt");
            FileInfo f_fileVer = new FileInfo(_First_ver_File);
            if (f_fileVer.Exists)
            {
                F_SW = 1;
                f_fileVer.Delete();   //프로그램을 처음 하는 사람이거나 이전 문제가 있어서 다시 셋업을 한사람인 경우에
            }
            


            if (New_FileName != "")
            {
                string u_ip, u_computername;
                Hard_Number(out u_ip, out  u_computername);

                //-----------------------------------------
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();
                string Tsql;
                
                Tsql = "Select Co_IP,  Co_Name , Up_Name , Up_Ver  ";
                Tsql = Tsql + " From Tbl_User_Ver  (nolock)   ";
                Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
                Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

                DataSet ds = new DataSet();

                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_User_Ver", ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0)
                {
                    Be_Ver = 0;
                }
                else
                {
                    Be_Ver = int.Parse(ds.Tables["Tbl_User_Ver"].Rows[0]["Up_Ver"].ToString());
                }

                if (F_SW == 1)
                    Be_Ver = 0; //새로 샛업을 햇거나 처음 하는 사람인 경우에 업데이트 받기 위함.
                //-----------------------------------------

            }
            cls_app_static_var.Program_Update_FileName = "";
            cls_app_static_var.Program_Update_NewVer = 0;

            if (New_FileName != "" && Be_Ver < New_Ver)
            {
                //cls_app_static_var.Program_Update_FileName = New_FileName;
                //cls_app_static_var.Program_Update_NewVer = New_Ver; 

                this.Enabled = false;
                progress.Visible = true; lab_Up.Visible = true;
               //19-03-11 깜빡임제거 this.Refresh();
                progress.Refresh(); lab_Up.Refresh();
                string ap_path_2 = Application.StartupPath.ToString();

                Base_ftp = new FTP("ftp://" + U_TIP + ":" + U_Port, U_TID, U_TPW);
                Base_ftp.Cancel_TF = 0;
                Base_ftp.Send_Download_Result += new FTP.Send_Download_Result_Dele(ftp_Send_Download_Result);
                Base_ftp.Download(tbDD.Text + cls_app_static_var.app_Company_Name, New_FileName, ap_path_2, progress, true, "Temp_Up_E.dat");

                
            }

        }

        private void Hard_Number(out string u_ip, out string u_computername)
        {
            u_ip = "";
            u_computername = "";

            u_ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0].ToString();

            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            //ManagementClass mc = new ManagementClass("Win32_DiskDrive");

            ManagementObjectCollection mOC = mc.GetInstances();

            foreach (ManagementObject mo in mOC)
            {
                u_computername = mo["name"].ToString();

            }

        }

        private void Check_Connect_FPT_Info()
        {

            //-----------------------------------------
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;


            Tsql = "Select U_TIP,  U_TIP2 , U_TID , U_TPW , U_PT1  ";
            Tsql = Tsql + " From Tbl_FT  (nolock)   ";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set_Base(Tsql, "Tbl_FT", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                return;
            }
            else
            {
                U_TIP = ds.Tables["Tbl_FT"].Rows[0]["U_TIP"].ToString();
                U_TID = ds.Tables["Tbl_FT"].Rows[0]["U_TID"].ToString();
                U_TPW = ds.Tables["Tbl_FT"].Rows[0]["U_TPW"].ToString();
                U_Port = ds.Tables["Tbl_FT"].Rows[0]["U_PT1"].ToString();
            }
            //-----------------------------------------
        }




        void ftp_Send_Download_Result(int D_Result)
        {

            progress.Visible = false; lab_Up.Visible = false;
            this.Enabled = true;
           //19-03-11 깜빡임제거 this.Refresh();

            if (D_Result == -1) return;

            string ap_path = Application.StartupPath.ToString();
            string _sourceFile = Path.Combine(ap_path, "Temp_Up_E.dat");

            if (Base_ftp.Cancel_TF == 1)
            {
                FileInfo fileDel = new FileInfo(_sourceFile);
                if (fileDel.Exists)
                    fileDel.Delete();
                return;
            }


            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            


            //현실행파일의 이름을 2~~.exe 로 바꿔버린다. 2를 앞에 붙여서..이름을 바꾼다. 업데이트후 삭제 하기 위함.
            string app_Name = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string app_Name2 = Path.GetFileName(app_Name);
            string Chang_app_Name = Path.Combine(ap_path, "2_" + app_Name2);
            FileInfo fileRename = new FileInfo(Path.Combine(ap_path, app_Name));
            if (fileRename.Exists)
            {
                FileInfo fileDel = new FileInfo(Chang_app_Name); //2_~~.exe 파일이 존재하는지를 체크해서 존재하면 지워라는임.
                if (fileDel.Exists)
                    fileDel.Delete();

                fileRename.MoveTo(Chang_app_Name); //이미있으면 에러
            }

            //업데이트 내역에 대해서 파일 압축을 푼다.
            DeCompression(_sourceFile);

            //압축 풀고 압축 파일을 삭제를 해버린다.
            FileInfo fileTempUpl = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileTempUpl.Exists)
                fileTempUpl.Delete();



            //실행 파일중에 삭제 해야 될꺼야 새로 실행해야 될 파일의 이름이 저장된 파일이 있는지 체크한다.
            FileInfo fileMUP_E = new FileInfo(Path.Combine(ap_path, "Temp_Up_E.dat"));
            if (fileMUP_E.Exists)
                fileMUP_E.Delete();


            //삭제해야 하는 실행 파일 명을 알려준다. 그리고 실행해야 하는 파일명도 저장한다.
            FileStream fs = new FileStream(Path.Combine(ap_path, "Temp_Up_E.dat"), FileMode.Create);
            StreamWriter Sw = new StreamWriter(fs);

            Sw.WriteLine(Chang_app_Name);  //삭제해야할 파일명
            Sw.WriteLine(app_Name);        //새로 실행해야할 파일명.     

            Sw.Close(); fs.Close();


            string u_ip, u_computername;
            Hard_Number(out u_ip, out  u_computername);

            //-----------------------------------------
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            string Tsql;

            Tsql = "Select Co_IP,  Co_Name , Up_Name , Up_Ver  ";
            Tsql = Tsql + " From Tbl_User_Ver  (nolock)   ";
            Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
            Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

            DataSet ds = new DataSet();

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Tbl_User_Ver", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                Tsql = "Insert Into Tbl_User_Ver (Up_Ver , Up_Name , Co_IP , Co_Name ) Values (";
                Tsql = Tsql + New_Ver + ",'" + New_FileName + "','" + u_ip + "','" + u_computername + "') ";

                if (Temp_Connect.Insert_Data(Tsql, "Tbl_User_Ver", this.Name.ToString(), this.Text) == false) return;

            }
            else
            {

                Tsql = "Update Tbl_User_Ver Set ";
                Tsql = Tsql + " Up_Ver = " + New_Ver;
                Tsql = Tsql + " ,Up_Name = '" + New_FileName + "'";
                Tsql = Tsql + " Where Co_IP ='" + u_ip + "'";
                Tsql = Tsql + " And   Co_Name ='" + u_computername + "'";

                if (Temp_Connect.Update_Data(Tsql, this.Name.ToString(), this.Text) == false) return;
            }
            //-----------------------------------------



            this.Cursor = System.Windows.Forms.Cursors.Default;


            string exe_file = Path.Combine(ap_path, "PUpDate.exe");

            FileInfo fileUp = new FileInfo(exe_file);
            if (fileUp.Exists)
            {
                Process UserProcess = new Process();

                UserProcess.StartInfo.UseShellExecute = true;

                UserProcess.StartInfo.FileName = exe_file;

                UserProcess.StartInfo.CreateNoWindow = true;

                UserProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                UserProcess.StartInfo.Arguments = "-1"; //argument가 필요없으면 삭제하세요.

                UserProcess.Start();
            }

            this.Close();
            Application.Exit();


        }




        void DeCompression(string filename)
        {
            string zipPath = filename;
            string extractDir = Environment.CurrentDirectory;

            System.IO.FileStream fs = new System.IO.FileStream(zipPath,
                                                 System.IO.FileMode.Open,
                                         System.IO.FileAccess.Read, System.IO.FileShare.Read);

            ICSharpCode.SharpZipLib.Zip.ZipInputStream zis =
                                    new ICSharpCode.SharpZipLib.Zip.ZipInputStream(fs);

            ICSharpCode.SharpZipLib.Zip.ZipEntry ze;

            while ((ze = zis.GetNextEntry()) != null)
            {
                if (!ze.IsDirectory)
                {
                    string fileName = System.IO.Path.GetFileName(ze.Name);

                    string destDir = System.IO.Path.Combine(extractDir,
                                     System.IO.Path.GetDirectoryName(ze.Name));

                    if (false == Directory.Exists(destDir))
                    {
                        System.IO.Directory.CreateDirectory(destDir);
                    }

                    string destPath = System.IO.Path.Combine(destDir, fileName);

                    System.IO.FileStream writer = new System.IO.FileStream(
                                    destPath, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write,
                                                System.IO.FileShare.Write);

                    byte[] buffer = new byte[2048];
                    int len;
                    while ((len = zis.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, len);
                    }

                    writer.Close();
                }
            }

            zis.Close();
            fs.Close();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData.Equals(Keys.Enter))
            {
                btn_Login_Click(null, null);
            }
        }


        


    
        
    }
}
