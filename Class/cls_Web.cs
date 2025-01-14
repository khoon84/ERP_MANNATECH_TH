﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace MLM_Program
{
    #region Web multipart/form-data file upload class
    // 참고 URL: https://spirit32.tistory.com/entry/C-multipartform-data-%ED%8C%8C%EC%9D%BC-%EC%97%85%EB%A1%9C%EB%93%9C
    public class FormFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public Stream Stream { get; set; }
    }

    public class RequestHelper
    {
        public static string PostMultipart(string url, Dictionary<string, object> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response;

            Stream responseStream;
            StreamReader reader;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                request.Method = "POST";
                request.UserAgent = "POVAS";
                request.KeepAlive = true;
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;


                if (parameters != null && parameters.Count > 0)
                {

                    using (Stream requestStream = request.GetRequestStream())
                    {

                        foreach (KeyValuePair<string, object> pair in parameters)
                        {

                            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                            if (pair.Value is FormFile)
                            {
                                FormFile file = pair.Value as FormFile;
                                string header = "Content-Disposition: form-data; name=\"" + pair.Key + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(header);
                                requestStream.Write(bytes, 0, bytes.Length);
                                byte[] buffer = new byte[32768];
                                int bytesRead;
                                if (file.Stream == null)
                                {
                                    // upload from file
                                    using (FileStream fileStream = File.OpenRead(file.FilePath))
                                    {
                                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                            requestStream.Write(buffer, 0, bytesRead);
                                        fileStream.Close();
                                    }
                                }
                                else
                                {
                                    // upload from given stream
                                    while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                        requestStream.Write(buffer, 0, bytesRead);
                                }
                            }
                            else
                            {
                                string data = "Content-Disposition: form-data; name=\"" + pair.Key + "\"\r\n\r\n" + pair.Value;
                                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
                                requestStream.Write(bytes, 0, bytes.Length);
                            }
                        }

                        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        requestStream.Write(trailer, 0, trailer.Length);
                        requestStream.Close();
                    }
                }

                // Web return 값 받기
                using (response = request.GetResponse())
                {
                    using (responseStream = response.GetResponseStream())
                    using (reader = new StreamReader(responseStream))
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ecx)
            {
                //Console.WriteLine(ecx.Message);

                using (response = request.GetResponse())
                {
                    using (responseStream = response.GetResponseStream())
                    using (reader = new StreamReader(responseStream))
                        return reader.ReadToEnd();
                }
            }
        }
    }




    #endregion


    class cls_Web
    {
        private static byte[] getbyte = new byte[270];
        private static byte[] setbyte = new byte[5400];
        private string DT_Time = "";

        StringEncrypter encrypter = new StringEncrypter(cls_User.con_EncryptKey, cls_User.con_EncryptKeyIV);

        const char STX = (char)0x02;
        const char FS = (char)0x1c;
        const char ETX = (char)0x03;

        /// <summary>
        /// 오토쉽 카드결제 승인 - 태국
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="ErrMessage"></param>
        /// <returns></returns>
        public string Dir_Card_AutoShip_OK_TH(string OrderNumber, int C_Index, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            //Card_AutoShip_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref Conn, ref tran);
            Card_AutoShip_OK_Data_TH(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveCardURL_TH;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            //SuccessYN = Return_Card_AutoShip_OK_Data(getstring, OrderNumber, C_Index, Seq_No, ref ErrMessage, ref Conn, ref tran);
            SuccessYN = Return_Card_AutoShip_OK_Data_TH(getstring, OrderNumber, C_Index, Seq_No, ref ErrMessage);

            return SuccessYN;
        }

        /// <summary>
        /// 태국 전용 오토쉽 카드결제 함수_3 - syhuh
        /// </summary>
        /// <param name="Getstring"></param>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Seq_No"></param>
        /// <param name="ErrMessage"></param>
        /// <returns></returns>
        private string Return_Card_AutoShip_OK_Data_TH(string Getstring, string OrderNumber, int C_Index, int Seq_No, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number2 = "", C_Number3 = "", StrSql = "";
            string CardCode = "", CardName = "", Cash_Sort_TF = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }
            if (SuccessYN == "Y")
            {
                //C_Number2 = ReturnData["r_auth_no"].ToString(); //거래번호
                //C_Number3 = ReturnData["r_cno"].ToString();     //승인번호
                C_Number3 = ReturnData["chargeId"].ToString();     //승인번호
                //CardCode = ReturnData["r_acquirer_cd"].ToString();
                //CardName = ReturnData["r_acquirer_nm"].ToString();
                Price = double.Parse(ReturnData["amount"].ToString());  // 결제 승인 완료 금액

                //if (ReturnData["r_card_gubun"].ToString() == "Y")
                //    Cash_Sort_TF = "1";
                //else if (ReturnData["r_card_gubun"].ToString() == "N")
                //    Cash_Sort_TF = "0";
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Code = '" + CardCode + "' ";      //카드코드
                StrSql = StrSql + " , C_Price1 = " + Price;           // 결제 승인 완료 금액
                StrSql = StrSql + " , C_CodeName = '" + CardName + "' ";    //카드명
                StrSql = StrSql + " , C_Number3  = '" + C_Number3 + "'";  //거래번호
                StrSql = StrSql + " , C_Number2 = '" + C_Number2 + "'";  //승인번호                        
                StrSql = StrSql + " , C_Number4 = ''"; //승인번호                        
                StrSql = StrSql + " , Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " , C_CancelTF = 0 ";
                StrSql = StrSql + " , C_CancelDate = '' ";
                StrSql = StrSql + " , C_CancelPrice = 0 ";
                //StrSql = StrSql + " , C_Cash_Sort_TF = " + Cash_Sort_TF;
                StrSql = StrSql + " , C_Cash_Sort_TF = '" + Cash_Sort_TF + "'";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "Auto", "Auto");

            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc + '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "Auto", "Auto");
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rAuthNo = '" + C_Number2 + "'";    //승인번호
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'"; //거래번호
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            //if (SuccessYN == "Y")
            //    StrSql = StrSql + " ,C_C_Price1 = " + Price;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "Auto", "Auto");
            return SuccessYN;
        }

        void Card_AutoShip_OK_Data_TH(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql += Environment.NewLine + " tbl_SalesDetail.OrderNumber,C_index   ";
            Tsql += Environment.NewLine + " , LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  ";
            Tsql += Environment.NewLine + " , C_Price1 ";
            Tsql += Environment.NewLine + " , LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)  ";
            Tsql += Environment.NewLine + " , C_Number1 , C_Number2 , C_P_Number, C_B_Number   ";
            Tsql += Environment.NewLine + " , C_Installment_Period , C_Name1 , C_Price2 ,C_Etc   ";
            Tsql += Environment.NewLine + " , Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per   ";
            //Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 2) ELSE C_Period1 END C_Period1";
            Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 4) ELSE C_Period1 END C_Period1";  // 태국 - 카드 연도 4자리 나오도록 변경
            Tsql += Environment.NewLine + " , Case When LEN(C_Period2) = 4 THEN RIGHT(C_Period2, 2) ELSE C_Period2 END C_Period2";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CardType ";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CVC ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.mbid2 ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.M_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Email ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hometel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 AS Address ";

            Tsql += Environment.NewLine + " , Isnull(S_Item.ItemName,'')  ItemName ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 
            //Tsql += Environment.NewLine + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            //Tsql += Environment.NewLine + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 


            Tsql += Environment.NewLine + "  From tbl_Sales_Cacu (nolock)  ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber   ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";

            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 
            Tsql += Environment.NewLine + "  LEFT Join (SELECT TOP 1 isnull(Tbl_Goods.Name,'' ) ItemName ,Ordernumber FROM TBL_SALESITEMDETAIL (NOLOCK) ";
            Tsql += Environment.NewLine + "                      LEFT Join Tbl_Goods  (NOLOCK) ON Tbl_Goods.ncode = TBL_SALESITEMDETAIL.itemCode   ";
            Tsql += Environment.NewLine + "                     Where OrderNumber ='" + OrderNumber + "' Order by Salesitemindex ) S_Item ON  S_Item.Ordernumber = tbl_SalesDetail.Ordernumber ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 



            Tsql += Environment.NewLine + "  Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_index = " + C_Index;


            //**김영수 팀장  수정 **
            //오토쉽인지를 아는데 굳이 이부분은 필요 없는 조건이라 생각해서 뺌 2019-05-29
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_TF = 3   ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.Sugi_TF = '1' ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Number3 = ''  ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price2 > 0   ";
            //**김영수 팀장  수정 **



            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string card_no = "";        //카드번호           // 필요
            double card_amt = 0;        //결제금액           // 필요
            string expire_date = "";    //유효기간           // 필요
            string expire_date_year = "";    //유효기간 - 년           // 필요
            string expire_date_month = "";    //유효기간 - 월           // 필요
            //string install_period = ""; //할부기간
            //string cert_type = "";      //인증:0 , 비인증:1
            //string cardtype = "";       //개인:0 , 법인:1
            //string password = "";       //비밀번호 앞 2자기
            //string auth_value = "";     //생년월일 6자리
            //int mbid2 = 0;              //회원번호
            //string M_name = "";         //회원명
            //string Email = "";          //회원메일
            //string HomeTel = "";        //회원전화번호
            //string HpTel = "";          //회원휴대폰번호
            //string Address = "";        //회원주소
            //string ItemName = "";       //제품명
            string securityCode = "";     //카드보안코드(CVC)           // 필요
            string C_Name = "";           // 주문자 이름

            card_no = encrypter.Decrypt(ds.Tables["Card_OK"].Rows[0]["C_Number1"].ToString());
            card_amt = double.Parse(ds.Tables["Card_OK"].Rows[0]["C_Price2"].ToString());
            expire_date = ds.Tables["Card_OK"].Rows[0]["Card_Per"].ToString();
            expire_date_year = ds.Tables["Card_OK"].Rows[0]["C_Period1"].ToString();
            expire_date_month = ds.Tables["Card_OK"].Rows[0]["C_Period2"].ToString();
            //install_period = ds.Tables["Card_OK"].Rows[0]["C_Installment_Period"].ToString();
            //if (install_period == "일시불" || install_period == "")
            //{
            //    install_period = "00";
            //}
            //cert_type = "1";    //비인증으로 함
            //password = ds.Tables["Card_OK"].Rows[0]["C_P_Number"].ToString();
            //auth_value = ds.Tables["Card_OK"].Rows[0]["C_B_Number"].ToString();
            //cardtype = ds.Tables["Card_OK"].Rows[0]["C_CardType"].ToString();
            //if (cardtype == string.Empty)
            //    cardtype = "0";


            //cert_type = "1";    //비인증으로 함  2019-04-12 비인증로 하기로함


            //mbid2 = int.Parse(ds.Tables["Card_OK"].Rows[0]["mbid2"].ToString());
            //M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
            //Email = ds.Tables["Card_OK"].Rows[0]["Email"].ToString();
            //HomeTel = ds.Tables["Card_OK"].Rows[0]["hometel"].ToString().Replace("-", "");
            //HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");
            //Address = ds.Tables["Card_OK"].Rows[0]["Address"].ToString();
            //ItemName = ds.Tables["Card_OK"].Rows[0]["ItemName"].ToString();
            securityCode = ds.Tables["Card_OK"].Rows[0]["C_CVC"].ToString();
            C_Name = ds.Tables["Card_OK"].Rows[0]["C_Name1"].ToString();

            //str_sendvalue = "Ep_cert_type=" + cert_type;                             //인증구분
            //str_sendvalue = str_sendvalue + "&Ep_order_no=" + OrderNumber;            //주문번호
            //str_sendvalue = str_sendvalue + "&Ep_card_amt=" + card_amt;               //결제금액
            str_sendvalue = str_sendvalue + "&amount=" + card_amt;                      //결제금액
            //str_sendvalue = str_sendvalue + "&Ep_card_user_type=" + cardtype;       //카드구분
            //str_sendvalue = str_sendvalue + "&Ep_card_no=" + card_no;                 //카드번호
            str_sendvalue = str_sendvalue + "&cardNumber=" + card_no;                   //카드번호
            //str_sendvalue = str_sendvalue + "&number=" + card_no;                   //카드번호
            //str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            str_sendvalue = str_sendvalue + "&expirationMonth=" + expire_date_month;     //유효기간(년)
            str_sendvalue = str_sendvalue + "&expirationYear=" + expire_date_year;     //유효기간(월)
            //str_sendvalue = str_sendvalue + "&Ep_install_period=" + install_period;   //할부개월
            //str_sendvalue = str_sendvalue + "&Ep_password=" + password;              //비밀번호 앞 2자리
            //str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리
            //str_sendvalue = str_sendvalue + "&Ep_user_id=" + mbid2;                   //고객ID (회원번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_nm=" + M_name;                  //고객명 (회원명)
            //str_sendvalue = str_sendvalue + "&Ep_user_mail=" + Email;                 //고객메일 (회원메일)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone1=" + HomeTel;             //고객전화번호 (회원전화번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone2=" + HpTel;               //고객휴대폰번호 (회원휴대폰번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_addr=" + Address;               //고객주소 (회원주소)
            //str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;             //제품명
            str_sendvalue = str_sendvalue + "&securityCode=" + securityCode;            //카드보안코드(CVC)
            str_sendvalue = str_sendvalue + "&name=" + C_Name;                          //카드 고객명

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','" + encrypter.Encrypt(card_no) + "','" + expire_date + "','A','','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        /*오토쉽 카드결제 승인*/
        //public string Dir_Card_AutoShip_OK(string OrderNumber, int C_Index, ref string ErrMessage, ref SqlConnection Conn, ref SqlTransaction tran)
        public string Dir_Card_AutoShip_OK(string OrderNumber, int C_Index, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            //Card_AutoShip_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref Conn, ref tran);
            Card_AutoShip_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            //SuccessYN = Return_Card_AutoShip_OK_Data(getstring, OrderNumber, C_Index, Seq_No, ref ErrMessage, ref Conn, ref tran);
            SuccessYN = Return_Card_AutoShip_OK_Data(getstring, OrderNumber, C_Index, Seq_No, ref ErrMessage);

            return SuccessYN;
        }

        //void Card_AutoShip_OK_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref SqlConnection Conn, ref SqlTransaction tran)
        void Card_AutoShip_OK_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql += Environment.NewLine + " tbl_SalesDetail.OrderNumber,C_index   ";
            Tsql += Environment.NewLine + " , LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  ";
            Tsql += Environment.NewLine + " , C_Price1 ";
            Tsql += Environment.NewLine + " , LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)  ";
            Tsql += Environment.NewLine + " , C_Number1 , C_Number2 , C_P_Number, C_B_Number   ";
            Tsql += Environment.NewLine + " , C_Installment_Period , C_Name1 , C_Price2 ,C_Etc   ";
            Tsql += Environment.NewLine + " , Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per   ";
            // Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 2) ELSE C_Period1 END C_Period1";
            // Tsql += Environment.NewLine + " , Case When LEN(C_Period2) = 4 THEN RIGHT(C_Period2, 2) ELSE C_Period2 END C_Period2";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CardType ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.mbid2 ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.M_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Email ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hometel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 AS Address ";

            Tsql += Environment.NewLine + " , Isnull(S_Item.ItemName,'')  ItemName ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 
            //Tsql += Environment.NewLine + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            //Tsql += Environment.NewLine + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 


            Tsql += Environment.NewLine + "  From tbl_Sales_Cacu (nolock)  ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber   ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";

            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 
            Tsql += Environment.NewLine + "  LEFT Join (SELECT TOP 1 isnull(Tbl_Goods.Name,'' ) ItemName ,Ordernumber FROM TBL_SALESITEMDETAIL (NOLOCK) ";
            Tsql += Environment.NewLine + "                      LEFT Join Tbl_Goods  (NOLOCK) ON Tbl_Goods.ncode = TBL_SALESITEMDETAIL.itemCode   ";
            Tsql += Environment.NewLine + "                     Where OrderNumber ='" + OrderNumber + "' Order by Salesitemindex ) S_Item ON  S_Item.Ordernumber = tbl_SalesDetail.Ordernumber ";
            //카드승인시 비사이측 상품 목록이 중요한 부분이 아니므로 이부분 변경 2019-05-30 김영수팀장 



            Tsql += Environment.NewLine + "  Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_index = " + C_Index;


            //**김영수 팀장  수정 **
            //오토쉽인지를 아는데 굳이 이부분은 필요 없는 조건이라 생각해서 뺌 2019-05-29
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_TF = 3   ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.Sugi_TF = '1' ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Number3 = ''  ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price2 > 0   ";
            //**김영수 팀장  수정 **



            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string card_no = "";        //카드번호
            double card_amt = 0;        //결제금액
            string expire_date = "";    //유효기간
            string install_period = ""; //할부기간
            string cert_type = "";      //인증:0 , 비인증:1
            string cardtype = "";       //개인:0 , 법인:1
            string password = "";       //비밀번호 앞 2자기
            string auth_value = "";     //생년월일 6자리
            int mbid2 = 0;              //회원번호
            string M_name = "";         //회원명
            string Email = "";          //회원메일
            string HomeTel = "";        //회원전화번호
            string HpTel = "";          //회원휴대폰번호
            string Address = "";        //회원주소
            string ItemName = "";       //제품명

            card_no = encrypter.Decrypt(ds.Tables["Card_OK"].Rows[0]["C_Number1"].ToString());
            card_amt = double.Parse(ds.Tables["Card_OK"].Rows[0]["C_Price2"].ToString());
            expire_date = ds.Tables["Card_OK"].Rows[0]["Card_Per"].ToString();
            install_period = ds.Tables["Card_OK"].Rows[0]["C_Installment_Period"].ToString();
            if (install_period == "일시불" || install_period == "")
            {
                install_period = "00";
            }
            cert_type = "1";    //비인증으로 함
            //password = ds.Tables["Card_OK"].Rows[0]["C_P_Number"].ToString();
            //auth_value = ds.Tables["Card_OK"].Rows[0]["C_B_Number"].ToString();
            cardtype = ds.Tables["Card_OK"].Rows[0]["C_CardType"].ToString();
            if (cardtype == string.Empty)
                cardtype = "0";


            cert_type = "1";    //비인증으로 함  2019-04-12 비인증로 하기로함


            mbid2 = int.Parse(ds.Tables["Card_OK"].Rows[0]["mbid2"].ToString());
            M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
            Email = ds.Tables["Card_OK"].Rows[0]["Email"].ToString();
            HomeTel = ds.Tables["Card_OK"].Rows[0]["hometel"].ToString().Replace("-", "");
            HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");
            Address = ds.Tables["Card_OK"].Rows[0]["Address"].ToString();
            ItemName = ds.Tables["Card_OK"].Rows[0]["ItemName"].ToString();

            str_sendvalue = "Ep_cert_type=" + cert_type;                             //인증구분
            str_sendvalue = str_sendvalue + "&Ep_order_no=" + OrderNumber;            //주문번호
            str_sendvalue = str_sendvalue + "&Ep_card_amt=" + card_amt;               //결제금액
            str_sendvalue = str_sendvalue + "&Ep_card_user_type=" + cardtype;       //카드구분
            str_sendvalue = str_sendvalue + "&Ep_card_no=" + card_no;                 //카드번호
            str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            str_sendvalue = str_sendvalue + "&Ep_install_period=" + install_period;   //할부개월
            //str_sendvalue = str_sendvalue + "&Ep_password=" + password;              //비밀번호 앞 2자리
            //str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리
            str_sendvalue = str_sendvalue + "&Ep_user_id=" + mbid2;                   //고객ID (회원번호)
            str_sendvalue = str_sendvalue + "&Ep_user_nm=" + M_name;                  //고객명 (회원명)
            str_sendvalue = str_sendvalue + "&Ep_user_mail=" + Email;                 //고객메일 (회원메일)
            str_sendvalue = str_sendvalue + "&Ep_user_phone1=" + HomeTel;             //고객전화번호 (회원전화번호)
            str_sendvalue = str_sendvalue + "&Ep_user_phone2=" + HpTel;               //고객휴대폰번호 (회원휴대폰번호)
            str_sendvalue = str_sendvalue + "&Ep_user_addr=" + Address;               //고객주소 (회원주소)
            str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;               //제품명


            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','" + encrypter.Encrypt(card_no) + "','" + expire_date + "','A','','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }



        //private string Return_Card_AutoShip_OK_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, ref string ErrMessage, ref SqlConnection Conn, ref SqlTransaction tran)
        private string Return_Card_AutoShip_OK_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number2 = "", C_Number3 = "", StrSql = "";
            string CardCode = "", CardName = "", Cash_Sort_TF = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }
            if (SuccessYN == "Y")
            {
                C_Number2 = ReturnData["r_auth_no"].ToString(); //거래번호
                C_Number3 = ReturnData["r_cno"].ToString();     //승인번호
                CardCode = ReturnData["r_acquirer_cd"].ToString();
                CardName = ReturnData["r_acquirer_nm"].ToString();
                Price = double.Parse(ReturnData["r_amount"].ToString());

                if (ReturnData["r_card_gubun"].ToString() == "Y")
                    Cash_Sort_TF = "1";
                else if (ReturnData["r_card_gubun"].ToString() == "N")
                    Cash_Sort_TF = "0";
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Code = '" + CardCode + "' ";      //카드코드
                StrSql = StrSql + " , C_Price1 = " + Price;
                StrSql = StrSql + " , C_CodeName = '" + CardName + "' ";    //카드명
                StrSql = StrSql + " , C_Number3  = '" + C_Number3 + "'";  //거래번호
                StrSql = StrSql + " , C_Number2 = '" + C_Number2 + "'";  //승인번호                        
                StrSql = StrSql + " , C_Number4 = ''"; //승인번호                        
                StrSql = StrSql + " , Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " , C_CancelTF = 0 ";
                StrSql = StrSql + " , C_CancelDate = '' ";
                StrSql = StrSql + " , C_CancelPrice = 0 ";
                StrSql = StrSql + " , C_Cash_Sort_TF = " + Cash_Sort_TF;
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "Auto", "Auto");

            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc + '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "Auto", "Auto");
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rAuthNo = '" + C_Number2 + "'";    //승인번호
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'"; //거래번호
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " ,C_C_Price1 = " + Price;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "Auto", "Auto");
            return SuccessYN;
        }



        /*카드결제 승인*/
        public string Dir_Card_Approve_OK(string OrderNumber, int C_Index)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            Card_Approve_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "VISI";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_OK_Data(getstring, OrderNumber, C_Index, Seq_No);

            return SuccessYN;
        }


        public string Dir_Card_Approve_OK_Err_2(string C_Card_Number, string C_Card_Year, string C_Card_Month, string C_B_Number, ref string Err_M, ref string rAcquirerNm)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            Err_M = "";
            Card_Approve_OK_Data_2(C_Card_Number, C_Card_Year, C_Card_Month, C_B_Number, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = "https://www.mannatech.co.kr/common/cs/cardAuth.do";

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            JObject ReturnData = new JObject();
            ReturnData = JObject.Parse(getstring);
            SuccessYN = ReturnData["successYN"].ToString();
            Err_M = ReturnData["errMessage"].ToString();
            if (SuccessYN == "Y")
            {

            }
            else
            {
                rAcquirerNm = ReturnData["rAcquirerNm"].ToString();
            }

            return SuccessYN;
        }

        /// <summary>
        /// 태국 전용 카드결제 함수 - syhuh
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Err_M"></param>
        /// <returns></returns>
        public string Dir_Card_Approve_OK_Err_TH(string OrderNumber, int C_Index, ref string Err_M)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            Err_M = "";
            Card_Approve_OK_Data_TH(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveCardURL_TH;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_OK_Data_Err_TH(getstring, OrderNumber, C_Index, Seq_No, ref Err_M);

            return SuccessYN;
        }

        /// <summary>
        /// 태국 전용 카드결제 함수_2 - syhuh
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Ord_SW"></param>
        /// <param name="str_sendvalue"></param>
        /// <param name="Seq_No"></param>
        void Card_Approve_OK_Data_TH(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql += Environment.NewLine + " tbl_SalesDetail.OrderNumber,C_index   ";
            Tsql += Environment.NewLine + " , LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  ";
            Tsql += Environment.NewLine + " , C_Price1 ";
            Tsql += Environment.NewLine + " , LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)  ";
            Tsql += Environment.NewLine + " , C_Number1 , C_Number2 , C_P_Number, C_B_Number   ";
            Tsql += Environment.NewLine + " , C_Installment_Period , C_Name1 , C_Price2 ,C_Etc   ";
            Tsql += Environment.NewLine + " , Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per   ";
            //Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 2) ELSE C_Period1 END C_Period1";
            Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 4) ELSE C_Period1 END C_Period1";  // 태국 - 카드 연도 4자리 나오도록 변경
            Tsql += Environment.NewLine + " , Case When LEN(C_Period2) = 4 THEN RIGHT(C_Period2, 2) ELSE C_Period2 END C_Period2";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CardType ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.mbid2 ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.M_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Email ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hometel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 AS Address ";
            Tsql += Environment.NewLine + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            Tsql += Environment.NewLine + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CVC ";
            Tsql += Environment.NewLine + "  From tbl_Sales_Cacu (nolock)  ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber   ";
            Tsql += Environment.NewLine + "  LEFT OUTER JOIN tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";
            Tsql += Environment.NewLine + "  Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_index = " + C_Index;
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_TF = 3   ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.Sugi_TF = '1' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Number3 = ''  ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price1 > 0   ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price2 > 0   ";
            //Tsql = Tsql + "  And C_P_Number <> ''   ";
            //Tsql = Tsql + "  And C_B_Number <> ''  ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string card_no = "";        //카드번호           // 필요
            double card_amt = 0;        //결제금액           // 필요
            string expire_date = "";    //유효기간           // 필요
            string expire_date_year = "";    //유효기간 - 년           // 필요
            string expire_date_month = "";    //유효기간 - 월           // 필요
            //string install_period = ""; //할부기간
            //string cert_type = "";      //인증:0 , 비인증:1
            //string cardtype = "";       //개인:0 , 법인:1
            //string password = "";       //비밀번호 앞 2자기
            //string auth_value = "";     //생년월일 6자리
            //int mbid2 = 0;              //회원번호
            //string M_name = "";         //회원명           // 필요
            //string Email = "";          //회원메일
            //string HomeTel = "";        //회원전화번호
            //string HpTel = "";          //회원휴대폰번호
            //string Address = "";        //회원주소
            //string ItemName = "";       //제품명
            string securityCode = "";     //카드보안코드(CVC)           // 필요
            string C_Name = "";           // 주문자 이름

            card_no = encrypter.Decrypt(ds.Tables["Card_OK"].Rows[0]["C_Number1"].ToString());
            card_amt = double.Parse(ds.Tables["Card_OK"].Rows[0]["C_Price2"].ToString());
            expire_date = ds.Tables["Card_OK"].Rows[0]["Card_Per"].ToString();
            expire_date_year = ds.Tables["Card_OK"].Rows[0]["C_Period1"].ToString();
            expire_date_month = ds.Tables["Card_OK"].Rows[0]["C_Period2"].ToString();
            //install_period = ds.Tables["Card_OK"].Rows[0]["C_Installment_Period"].ToString();
            //if (install_period == "일시불" || install_period == "")
            //{
            //    install_period = "00";
            //}
            //cert_type = "1";    //비인증으로 함
            //password = ds.Tables["Card_OK"].Rows[0]["C_P_Number"].ToString();
            //auth_value = ds.Tables["Card_OK"].Rows[0]["C_B_Number"].ToString();
            //cardtype = ds.Tables["Card_OK"].Rows[0]["C_CardType"].ToString();
            //if (cardtype == string.Empty)
            //    cardtype = "0";


            //cert_type = "1";    //비인증으로 함  2019-04-12 비인증로 하기로함


            //mbid2 = int.Parse(ds.Tables["Card_OK"].Rows[0]["mbid2"].ToString());
            //M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
            //Email = ds.Tables["Card_OK"].Rows[0]["Email"].ToString();
            //HomeTel = ds.Tables["Card_OK"].Rows[0]["hometel"].ToString().Replace("-", "");
            //HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");
            //Address = ds.Tables["Card_OK"].Rows[0]["Address"].ToString();
            //ItemName = ds.Tables["Card_OK"].Rows[0]["ItemName"].ToString();
            securityCode = ds.Tables["Card_OK"].Rows[0]["C_CVC"].ToString();
            C_Name = ds.Tables["Card_OK"].Rows[0]["C_Name1"].ToString();

            //str_sendvalue = "Ep_cert_type=" + cert_type;                              //인증구분
            //str_sendvalue = str_sendvalue + "&Ep_order_no=" + OrderNumber;            //주문번호
            str_sendvalue = str_sendvalue + "&amount=" + card_amt;                      //결제금액
            //str_sendvalue = str_sendvalue + "&Ep_card_user_type=" + cardtype;         //카드구분
            str_sendvalue = str_sendvalue + "&cardNumber=" + card_no;                   //카드번호
            //str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            str_sendvalue = str_sendvalue + "&expirationMonth=" + expire_date_month;     //유효기간(년)
            str_sendvalue = str_sendvalue + "&expirationYear=" + expire_date_year;     //유효기간(월)
            //str_sendvalue = str_sendvalue + "&Ep_install_period=" + install_period;   //할부개월
            //str_sendvalue = str_sendvalue + "&Ep_password=" + password;               //비밀번호 앞 2자리
            //str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리
            //str_sendvalue = str_sendvalue + "&Ep_user_id=" + mbid2;                   //고객ID (회원번호)
            //str_sendvalue = str_sendvalue + "&name=" + M_name;                        //고객명 (회원명)
            //str_sendvalue = str_sendvalue + "&Ep_user_mail=" + Email;                 //고객메일 (회원메일)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone1=" + HomeTel;             //고객전화번호 (회원전화번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone2=" + HpTel;               //고객휴대폰번호 (회원휴대폰번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_addr=" + Address;               //고객주소 (회원주소)
            //str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;             //제품명
            str_sendvalue = str_sendvalue + "&securityCode=" + securityCode;            //카드보안코드(CVC)
            str_sendvalue = str_sendvalue + "&name=" + C_Name;                          //카드 고객명

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','" + encrypter.Encrypt(card_no) + "','" + expire_date + "','A','','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        /// <summary>
        /// 태국 전용 카드결제 함수_3 - syhuh
        /// </summary>
        /// <param name="Getstring"></param>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Seq_No"></param>
        /// <param name="Err_M"></param>
        /// <returns></returns>
        private string Return_Card_Approve_OK_Data_Err_TH(string Getstring, string OrderNumber, int C_Index, int Seq_No, ref string Err_M)
        {
            string SuccessYN = "", C_Number2 = "", C_Number3 = "", StrSql = "";
            string CardCode = "", CardName = "", ErrMessage = "", Cash_Sort_TF = "";
            string TerminalID = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                //C_Number2 = ReturnData["r_auth_no"].ToString(); //거래번호
                //C_Number3 = ReturnData["r_cno"].ToString();     //승인번호
                C_Number3 = ReturnData["chargeId"].ToString();     //승인번호
                //CardCode = ReturnData["r_acquirer_cd"].ToString();
                //CardName = ReturnData["r_acquirer_nm"].ToString();
                //Price = double.Parse(ReturnData["r_amount"].ToString());

                //if (ReturnData["r_card_gubun"].ToString() == "Y")
                //    Cash_Sort_TF = "1";
                //else if (ReturnData["r_card_gubun"].ToString() == "N")
                //    Cash_Sort_TF = "0";

                //TerminalID = ReturnData["r_mall_id"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();

                Err_M = ErrMessage;
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Code = '" + CardCode + "' ";      //카드코드
                StrSql = StrSql + " , C_CodeName = '" + CardName + "' ";    //카드명
                StrSql = StrSql + " , C_Number3  = '" + C_Number3 + "'";  //거래번호
                StrSql = StrSql + " , C_Number2 = '" + C_Number2 + "'";  //승인번호                        
                StrSql = StrSql + " , C_Number4 = ''"; //승인번호                        
                StrSql = StrSql + " , Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " , C_CancelTF = 0 ";
                StrSql = StrSql + " , C_CancelDate = '' ";
                StrSql = StrSql + " , C_CancelPrice = 0 ";
                StrSql = StrSql + " , C_Cash_Sort_TF = '" + Cash_Sort_TF + "'";
                StrSql = StrSql + " , TerminalID = '" + TerminalID + "'";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);

            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc + '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rAuthNo = '" + C_Number2 + "'";    //승인번호
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'"; //거래번호
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " ,C_C_Price1 = " + Price;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        public string Dir_Card_Approve_OK_Err(string OrderNumber, int C_Index, ref string Err_M)
        {
            
            // 태국인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                return Dir_Card_Approve_OK_Err_TH(OrderNumber, C_Index, ref Err_M);
            }

            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            Err_M = "";
            Card_Approve_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_OK_Data_Err(getstring, OrderNumber, C_Index, Seq_No, ref Err_M);

            return SuccessYN;
        }

        void Card_Approve_OK_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql += Environment.NewLine + " tbl_SalesDetail.OrderNumber,C_index   ";
            Tsql += Environment.NewLine + " , LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  ";
            Tsql += Environment.NewLine + " , C_Price1 ";
            Tsql += Environment.NewLine + " , LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)  ";
            Tsql += Environment.NewLine + " , C_Number1 , C_Number2 , C_P_Number, C_B_Number   ";
            Tsql += Environment.NewLine + " , C_Installment_Period , C_Name1 , C_Price2 ,C_Etc   ";
            Tsql += Environment.NewLine + " , Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per   ";
            // Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 2) ELSE C_Period1 END C_Period1";
            // Tsql += Environment.NewLine + " , Case When LEN(C_Period2) = 4 THEN RIGHT(C_Period2, 2) ELSE C_Period2 END C_Period2";
            Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CardType ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.mbid2 ";
            Tsql += Environment.NewLine + " , tbl_SalesDetail.M_Name ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Email ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hometel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            Tsql += Environment.NewLine + " , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 AS Address ";
            Tsql += Environment.NewLine + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            Tsql += Environment.NewLine + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            Tsql += Environment.NewLine + "  From tbl_Sales_Cacu (nolock)  ";
            Tsql += Environment.NewLine + "  LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber   ";
            Tsql += Environment.NewLine + "  LEFT OUTER JOIN tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";
            Tsql += Environment.NewLine + "  Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_index = " + C_Index;
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_TF = 3   ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.Sugi_TF = '1' ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Number3 = ''  ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price1 > 0   ";
            Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price2 > 0   ";
            //Tsql = Tsql + "  And C_P_Number <> ''   ";
            //Tsql = Tsql + "  And C_B_Number <> ''  ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string card_no = "";        //카드번호
            double card_amt = 0;        //결제금액
            string expire_date = "";    //유효기간
            string install_period = ""; //할부기간
            string cert_type = "";      //인증:0 , 비인증:1
            string cardtype = "";       //개인:0 , 법인:1
            string password = "";       //비밀번호 앞 2자기
            string auth_value = "";     //생년월일 6자리
            int mbid2 = 0;              //회원번호
            string M_name = "";         //회원명
            string Email = "";          //회원메일
            string HomeTel = "";        //회원전화번호
            string HpTel = "";          //회원휴대폰번호
            string Address = "";        //회원주소
            string ItemName = "";       //제품명

            card_no = encrypter.Decrypt(ds.Tables["Card_OK"].Rows[0]["C_Number1"].ToString());
            card_amt = double.Parse(ds.Tables["Card_OK"].Rows[0]["C_Price2"].ToString());
            expire_date = ds.Tables["Card_OK"].Rows[0]["Card_Per"].ToString();
            install_period = ds.Tables["Card_OK"].Rows[0]["C_Installment_Period"].ToString();
            if (install_period == "일시불" || install_period == "")
            {
                install_period = "00";
            }
            cert_type = "1";    //비인증으로 함
            //password = ds.Tables["Card_OK"].Rows[0]["C_P_Number"].ToString();
            //auth_value = ds.Tables["Card_OK"].Rows[0]["C_B_Number"].ToString();
            cardtype = ds.Tables["Card_OK"].Rows[0]["C_CardType"].ToString();
            if (cardtype == string.Empty)
                cardtype = "0";


            cert_type = "1";    //비인증으로 함  2019-04-12 비인증로 하기로함


            mbid2 = int.Parse(ds.Tables["Card_OK"].Rows[0]["mbid2"].ToString());
            M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
            Email = ds.Tables["Card_OK"].Rows[0]["Email"].ToString();
            HomeTel = ds.Tables["Card_OK"].Rows[0]["hometel"].ToString().Replace("-", "");
            HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");
            Address = ds.Tables["Card_OK"].Rows[0]["Address"].ToString();
            ItemName = ds.Tables["Card_OK"].Rows[0]["ItemName"].ToString();

            str_sendvalue = "Ep_cert_type=" + cert_type;                             //인증구분
            str_sendvalue = str_sendvalue + "&Ep_order_no=" + OrderNumber;            //주문번호
            str_sendvalue = str_sendvalue + "&Ep_card_amt=" + card_amt;               //결제금액
            str_sendvalue = str_sendvalue + "&Ep_card_user_type=" + cardtype;       //카드구분
            str_sendvalue = str_sendvalue + "&Ep_card_no=" + card_no;                 //카드번호
            str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            str_sendvalue = str_sendvalue + "&Ep_install_period=" + install_period;   //할부개월
            //str_sendvalue = str_sendvalue + "&Ep_password=" + password;              //비밀번호 앞 2자리
            //str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리
            str_sendvalue = str_sendvalue + "&Ep_user_id=" + mbid2;                   //고객ID (회원번호)
            str_sendvalue = str_sendvalue + "&Ep_user_nm=" + M_name;                  //고객명 (회원명)
            str_sendvalue = str_sendvalue + "&Ep_user_mail=" + Email;                 //고객메일 (회원메일)
            str_sendvalue = str_sendvalue + "&Ep_user_phone1=" + HomeTel;             //고객전화번호 (회원전화번호)
            str_sendvalue = str_sendvalue + "&Ep_user_phone2=" + HpTel;               //고객휴대폰번호 (회원휴대폰번호)
            str_sendvalue = str_sendvalue + "&Ep_user_addr=" + Address;               //고객주소 (회원주소)
            str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;               //제품명



            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','" + encrypter.Encrypt(card_no) + "','" + expire_date + "','A','','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        void Card_Approve_OK_Data_2(string C_Card_Number, string C_Card_Year, string C_Card_Month, string C_B_Number, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            //string Tsql = "";

            //Tsql = " Select ";
            //Tsql += Environment.NewLine + " tbl_SalesDetail.OrderNumber,C_index   ";
            //Tsql += Environment.NewLine + " , LEFT(tbl_SalesDetail.SellDate,4) +'-' + LEFT(RIGHT(tbl_SalesDetail.SellDate,4),2) + '-' + RIGHT(tbl_SalesDetail.SellDate,2)  ";
            //Tsql += Environment.NewLine + " , C_Price1 ";
            //Tsql += Environment.NewLine + " , LEFT(C_AppDate1,4) +'-' + LEFT(RIGHT(C_AppDate1,4),2) + '-' + RIGHT(C_AppDate1,2)  ";
            //Tsql += Environment.NewLine + " , C_Number1 , C_Number2 , C_P_Number, C_B_Number   ";
            //Tsql += Environment.NewLine + " , C_Installment_Period , C_Name1 , C_Price2 ,C_Etc   ";
            //Tsql += Environment.NewLine + " , Case When C_Period1 <>'' And C_Period2 <> '' then   Right (C_Period1,2 ) + C_Period2 ELSE '' End  AS Card_Per   ";
            //// Tsql += Environment.NewLine + " , Case When LEN(C_Period1) = 4 THEN RIGHT(C_Period1, 2) ELSE C_Period1 END C_Period1";
            //// Tsql += Environment.NewLine + " , Case When LEN(C_Period2) = 4 THEN RIGHT(C_Period2, 2) ELSE C_Period2 END C_Period2";
            //Tsql += Environment.NewLine + " , tbl_Sales_Cacu.C_CardType ";
            //Tsql += Environment.NewLine + " , tbl_SalesDetail.mbid2 ";
            //Tsql += Environment.NewLine + " , tbl_SalesDetail.M_Name ";
            //Tsql += Environment.NewLine + " , tbl_Memberinfo.Email ";
            //Tsql += Environment.NewLine + " , tbl_Memberinfo.hometel ";
            //Tsql += Environment.NewLine + " , tbl_Memberinfo.hptel ";
            //Tsql += Environment.NewLine + " , tbl_Memberinfo.Address1 + ' ' + tbl_Memberinfo.Address2 AS Address ";
            //Tsql += Environment.NewLine + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            //Tsql += Environment.NewLine + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            //Tsql += Environment.NewLine + "  From tbl_Sales_Cacu (nolock)  ";
            //Tsql += Environment.NewLine + "  LEFT Join tbl_SalesDetail  (nolock) ON  tbl_SalesDetail.OrderNumber = tbl_Sales_Cacu.OrderNumber   ";
            //Tsql += Environment.NewLine + "  LEFT OUTER JOIN tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";
            //Tsql += Environment.NewLine + "  Where tbl_SalesDetail.OrderNumber = '" + OrderNumber + "' ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_index = " + C_Index;
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_TF = 3   ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.Sugi_TF = '1' ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Number3 = ''  ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price1 > 0   ";
            //Tsql += Environment.NewLine + "  And tbl_Sales_Cacu.C_Price2 > 0   ";
            ////Tsql = Tsql + "  And C_P_Number <> ''   ";
            ////Tsql = Tsql + "  And C_B_Number <> ''  ";

            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;

            //if (ReCnt == 0) return;
            ////++++++++++++++++++++++++++++++++
            string card_no = "";        //카드번호
            double card_amt = 0;        //결제금액
            string expire_date = "";    //유효기간
            string install_period = ""; //할부기간
            string cert_type = "";      //인증:0 , 비인증:1
            string cardtype = "";       //개인:0 , 법인:1
            string password = "";       //비밀번호 앞 2자기
            string auth_value = "";     //생년월일 6자리
            int mbid2 = 0;              //회원번호
            string M_name = "";         //회원명
            string Email = "";          //회원메일
            string HomeTel = "";        //회원전화번호
            string HpTel = "";          //회원휴대폰번호
            string Address = "";        //회원주소
            string ItemName = "";       //제품명

            card_no = C_Card_Number.ToString();
            expire_date = C_Card_Year + C_Card_Month;
            auth_value = C_B_Number.ToString();
            //if (install_period == "일시불" || install_period == "")
            //{
            //    install_period = "00";
            //}
            //cert_type = "1";    //비인증으로 함
            ////password = ds.Tables["Card_OK"].Rows[0]["C_P_Number"].ToString();
            ////auth_value = ds.Tables["Card_OK"].Rows[0]["C_B_Number"].ToString();
            //cardtype = ds.Tables["Card_OK"].Rows[0]["C_CardType"].ToString();
            //if (cardtype == string.Empty)
            //    cardtype = "0";


            //cert_type = "1";    //비인증으로 함  2019-04-12 비인증로 하기로함


            //mbid2 = int.Parse(ds.Tables["Card_OK"].Rows[0]["mbid2"].ToString());
            //M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
            //Email = ds.Tables["Card_OK"].Rows[0]["Email"].ToString();
            //HomeTel = ds.Tables["Card_OK"].Rows[0]["hometel"].ToString().Replace("-", "");
            //HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");
            //Address = ds.Tables["Card_OK"].Rows[0]["Address"].ToString();
            //ItemName = ds.Tables["Card_OK"].Rows[0]["ItemName"].ToString();


            str_sendvalue = str_sendvalue + "&Ep_card_no=" + card_no;                 //카드번호
            str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리

            //str_sendvalue = "Ep_cert_type=" + cert_type;                             //인증구분
            //str_sendvalue = str_sendvalue + "&Ep_order_no=" + OrderNumber;            //주문번호
            //str_sendvalue = str_sendvalue + "&Ep_card_amt=" + card_amt;               //결제금액
            //str_sendvalue = str_sendvalue + "&Ep_card_user_type=" + cardtype;       //카드구분
            //str_sendvalue = str_sendvalue + "&Ep_card_no=" + card_no;                 //카드번호
            //str_sendvalue = str_sendvalue + "&Ep_expire_date=" + expire_date;         //유효기간(년월)
            //str_sendvalue = str_sendvalue + "&Ep_install_period=" + install_period;   //할부개월
            ////str_sendvalue = str_sendvalue + "&Ep_password=" + password;              //비밀번호 앞 2자리
            ////str_sendvalue = str_sendvalue + "&Ep_auth_value=" + auth_value;           //생년월일 6자리
            //str_sendvalue = str_sendvalue + "&Ep_user_id=" + mbid2;                   //고객ID (회원번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_nm=" + M_name;                  //고객명 (회원명)
            //str_sendvalue = str_sendvalue + "&Ep_user_mail=" + Email;                 //고객메일 (회원메일)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone1=" + HomeTel;             //고객전화번호 (회원전화번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_phone2=" + HpTel;               //고객휴대폰번호 (회원휴대폰번호)
            //str_sendvalue = str_sendvalue + "&Ep_user_addr=" + Address;               //고객주소 (회원주소)
            //str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;               //제품명



            //string StrSql = "";
            //StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','" + encrypter.Encrypt(card_no) + "','" + expire_date + "','A','','" + cls_User.gid + "'";
            //Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            //Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }
        private string Return_Card_Approve_OK_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No)
        {
            string SuccessYN = "", C_Number2 = "", C_Number3 = "", StrSql = "";
            string CardCode = "", CardName = "", ErrMessage = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number3 = ReturnData["tid"].ToString(); //거래번호
                C_Number2 = ReturnData["cardAuthNo"].ToString();     //승인번호
                CardCode = ReturnData["cardCode"].ToString();
                CardName = ReturnData["cardName"].ToString();
                Price = double.Parse(ReturnData["amount"].ToString());
            }
            else
            {
                ErrMessage = ReturnData["message"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Code = '" + CardCode + "' ";      //카드코드
                //StrSql = StrSql + " , C_CodeName = '" + CardName + "' ";    //카드명
                StrSql = StrSql + ", C_CodeName = (SELECT TOP 1 CardName FROM tbl_Card where ncode = '" + CardCode + "') "; //카드명
                StrSql = StrSql + " , C_Number3  = '" + C_Number3 + "'";  //거래번호
                StrSql = StrSql + " , C_Number2 = '" + C_Number2 + "'";  //승인번호                        
                StrSql = StrSql + " , C_Number4 = ''"; //승인번호                        
                StrSql = StrSql + " , Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " , C_CancelTF = 0 ";
                StrSql = StrSql + " , C_CancelDate = '' ";
                StrSql = StrSql + " , C_CancelPrice = 0 ";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);

            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc + '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rAuthNo = '" + C_Number2 + "'";    //승인번호
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'"; //거래번호
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " ,C_C_Price1 = " + Price;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }



        private string Return_Card_Approve_OK_Data_Err(string Getstring, string OrderNumber, int C_Index, int Seq_No, ref string Err_M)
        {
            string SuccessYN = "", C_Number2 = "", C_Number3 = "", StrSql = "";
            string CardCode = "", CardName = "", ErrMessage = "", Cash_Sort_TF = "";
            string TerminalID = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number2 = ReturnData["r_auth_no"].ToString(); //거래번호
                C_Number3 = ReturnData["r_cno"].ToString();     //승인번호
                CardCode = ReturnData["r_acquirer_cd"].ToString();
                CardName = ReturnData["r_acquirer_nm"].ToString();
                Price = double.Parse(ReturnData["r_amount"].ToString());

                if (ReturnData["r_card_gubun"].ToString() == "Y")
                    Cash_Sort_TF = "1";
                else if (ReturnData["r_card_gubun"].ToString() == "N")
                    Cash_Sort_TF = "0";

                TerminalID = ReturnData["r_mall_id"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();

                Err_M = ErrMessage;
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Code = '" + CardCode + "' ";      //카드코드
                StrSql = StrSql + " , C_CodeName = '" + CardName + "' ";    //카드명
                StrSql = StrSql + " , C_Number3  = '" + C_Number3 + "'";  //거래번호
                StrSql = StrSql + " , C_Number2 = '" + C_Number2 + "'";  //승인번호                        
                StrSql = StrSql + " , C_Number4 = ''"; //승인번호                        
                StrSql = StrSql + " , Sugi_TF = '2' ";  //승인이 제대로 이루어 졋다. 2번으로 넣는다.
                StrSql = StrSql + " , C_AppDate1 = CONVERT(VARCHAR(8), GETDATE(), 112) ";
                StrSql = StrSql + " , C_CancelTF = 0 ";
                StrSql = StrSql + " , C_CancelDate = '' ";
                StrSql = StrSql + " , C_CancelPrice = 0 ";
                StrSql = StrSql + " , C_Cash_Sort_TF = " + Cash_Sort_TF;
                StrSql = StrSql + " , TerminalID = '" + TerminalID + "'";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);

            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc + '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rAuthNo = '" + C_Number2 + "'";    //승인번호
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number3 + "'"; //거래번호
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " ,C_C_Price1 = " + Price;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        /*오토쉽 카드 취소*/
        public string Dir_Card_AutoShip_Approve_Cancel(string OrderNumber, int C_Index, ref SqlConnection Conn, ref SqlTransaction tran)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            double ReturnPrice = 0;
            Card_AutoShip_Approve_Cancel_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice, ref Conn, ref tran);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_AutoShip_Approve_Cancel_Data(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ref Conn, ref tran);

            return SuccessYN;
        }

        void Card_AutoShip_Approve_Cancel_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice, ref SqlConnection Conn, ref SqlTransaction tran)
        {
            string Tsql = "";

            Tsql = " Select C_index, C_Price1, C_Number1, C_Number3, C_Number2 , C_Period1, C_Period2, '05540305' TerminalID";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string mgr_txtype = "40";                                                   //40:취소(승인/매입 자동판단취소)
            string C_Number3 = ds.Tables["Card_Cancel"].Rows[0]["C_Number3"].ToString();  //PG거래번호
            string req_id = cls_User.gid;                                               //요청자ID
            string mgr_msg = "";                                                        //취소사유
            string C_Number1 = encrypter.Decrypt(ds.Tables["Card_Cancel"].Rows[0]["C_Number1"].ToString());    //카드번호
            string mall_id = ds.Tables["Card_Cancel"].Rows[0]["TerminalID"].ToString();     //단말기ID
            ReturnPrice = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());

            str_sendvalue = "mgr_txtype=" + mgr_txtype;
            str_sendvalue = str_sendvalue + "&org_cno=" + C_Number3;
            str_sendvalue = str_sendvalue + "&req_id=" + req_id;
            str_sendvalue = str_sendvalue + "&mgr_msg=" + mgr_msg;
            str_sendvalue = str_sendvalue + "&mall_id=" + mall_id;

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C','" + C_Number3 + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;

        }

        private string Return_Card_AutoShip_Approve_Cancel_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, ref SqlConnection Conn, ref SqlTransaction tran)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            string ErrMessage = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch (Exception e)
            {
                //MessageBox.Show("통신에러");
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number4 = ReturnData["r_cno"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + "  C_Number3 = ''";  //거래번호
                StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //거래번호
                StrSql = StrSql + " ,C_CancelTF = 1 ";
                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                StrSql = StrSql + " ,C_Price1 = 0 ";

                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, Conn, tran, "", "");
            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, Conn, tran, "", "");

            return SuccessYN;
        }

        /*선결제 카드 취소
         * 그냥 무조건 취소로 시켜버린다
         */
        public string Before_Card_Cancel(string OrderNumber, int C_Index)
        {
            int Seq_No = 0;
            //string SuccessYN = "";
            double ReturnPrice = 0;

            string Tsql = "";

            Tsql = " Select C_index , C_Price1 , C_Number1 C_Number1, C_Number3 , C_Number2 ,C_Period1, C_Period2";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return "N";
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0)
            {
                //0값이면 결제가안된거니 그냥 이력상으로 취소가되게끔해줍시다.
                Tsql = " Select C_index , C_Price1 , C_Number1, C_Number3, C_Number2, C_Period1, C_Period2";
                Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
                Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
                Tsql = Tsql + " And   C_TF   = 3 ";
                Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
                Tsql = Tsql + " And C_Price1 = 0 ";
                //++++++++++++++++++++++++++++++++

                ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return "N";
                ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt > 0 && ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString().Equals("0"))
                    return "Y";

                return "N";
            }
            //++++++++++++++++++++++++++++++++
            double Amount = 0;                  //취소금액(필수)
            string tid = "";                    //다날 원거래키(필수)


            ReturnPrice = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());

            Amount = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());
            tid = ds.Tables["Card_Cancel"].Rows[0]["C_Number3"].ToString();


            string C_Number1 = "";
            C_Number1 = encrypter.Decrypt(ds.Tables["Card_Cancel"].Rows[0]["C_Number1"].ToString());

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C','" + tid + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());


            StrSql = "Update tbl_Sales_Cacu SET ";
            StrSql = StrSql + "  C_Number3 = '' ";  //거래번호
            StrSql = StrSql + " ,C_Number4 = '' ";  //거래번호
            StrSql = StrSql + " ,C_CancelTF = 1 ";
            StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
            StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
            StrSql = StrSql + " ,C_Price1 = 0 ";

            StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
            StrSql = StrSql + " And   C_index = " + C_Index;

            Temp_Connect.Update_Data(StrSql, "", "", 1);


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = 'Y'";
            StrSql = StrSql + " ,rTransactionNo = ''";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return "Y";
        }


        /// <summary>
        /// 카드 취소 - 태국 - syhuh
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="ErrMessage"></param>
        /// <returns></returns>
        public string Dir_Card_Approve_Cancel_TH(string OrderNumber, int C_Index, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            double ReturnPrice = 0;

            Card_Approve_Cancel_Data_TH(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelCardURL_TH;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_Cancel_Data_TH(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ref ErrMessage);

            return SuccessYN;
        }

        /// <summary>
        /// 카드 취소_2 - 태국 - syhuh
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Ord_SW"></param>
        /// <param name="str_sendvalue"></param>
        /// <param name="Seq_No"></param>
        /// <param name="ReturnPrice"></param>
        void Card_Approve_Cancel_Data_TH(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice)
        {
            string Tsql = "";

            Tsql = " Select C_index, C_Price1, C_Number1, C_Number3 , C_Number2 ,C_Period1,C_Period2, TerminalID";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            //string mgr_txtype = "40";                                                   //40:취소(승인/매입 자동판단취소)
            string C_Number3 = ds.Tables["Card_Cancel"].Rows[0]["C_Number3"].ToString();  //PG거래번호
            //string req_id = cls_User.gid;                                               //요청자ID
            //string mgr_msg = "";                                                        //취소사유
            string C_Number1 = encrypter.Decrypt(ds.Tables["Card_Cancel"].Rows[0]["C_Number1"].ToString());    //카드번호
            //string mall_id = ds.Tables["Card_Cancel"].Rows[0]["TerminalID"].ToString();     //단말기ID
            ReturnPrice = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());

            //str_sendvalue = "mgr_txtype=" + mgr_txtype;
            str_sendvalue = "&chargeId=" + C_Number3;
            //str_sendvalue = str_sendvalue + "&req_id=" + req_id;
            //str_sendvalue = str_sendvalue + "&mgr_msg=" + mgr_msg;
            //str_sendvalue = str_sendvalue + "&mall_id=" + mall_id;
            str_sendvalue = str_sendvalue + "&amount=" + ReturnPrice;

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C','" + C_Number3 + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;

        }

        /// <summary>
        /// 카드 취소_3 - 태국 - syhuh
        /// </summary>
        /// <param name="Getstring"></param>
        /// <param name="OrderNumber"></param>
        /// <param name="C_Index"></param>
        /// <param name="Seq_No"></param>
        /// <param name="ReturnPrice"></param>
        /// <param name="ErrMessage"></param>
        /// <returns></returns>
        private string Return_Card_Approve_Cancel_Data_TH(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch (Exception e)
            {
                MessageBox.Show("통신에러");

                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                //C_Number4 = ReturnData["r_cno"].ToString();
                C_Number4 = ReturnData["refundId"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                //StrSql = StrSql + "  C_Number3 = ''";  //거래번호
                StrSql = StrSql + "  C_Number4 = '" + C_Number4 + "'";  //거래번호
                StrSql = StrSql + " ,C_CancelTF = 1 ";
                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                StrSql = StrSql + " ,C_Price1 = 0 ";

                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "", "", 1);
            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }

        /*카드 부분 취소*/
        public string Dir_Card_Approve_Return_TH(string OrderNumber, int C_Index, string OrderNumber_R, int C_Index_R, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "", ReturnGubun = "";
            double ReturnPrice = 0;
            Card_Approve_Return_Data_TH(OrderNumber, C_Index, OrderNumber_R, C_Index_R, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice, ref ReturnGubun);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelCardURL_TH;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_Return_Data_TH(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ReturnGubun, ref ErrMessage);

            //승인부분취소, 매입부분취소를 던졌는데 에러가 났다......그러면 반대로 다시 태워버리자
            if (SuccessYN == "N")
            {
                Ord_SW = 0;
                Seq_No = 0;
                str_sendvalue = "";
                SuccessYN = "";
                ReturnGubun = "";
                ReturnPrice = 0;
                Card_Approve_Return_Data_TH(OrderNumber, C_Index, OrderNumber_R, C_Index_R, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice, ref ReturnGubun);

                if (Ord_SW == 0)
                    return "";

                string URL_RE = cls_app_static_var.CancelCardURL_TH;

                HttpWebRequest hwr_RE = (HttpWebRequest)WebRequest.Create(URL_RE);
                hwr_RE.Method = "POST"; // 포스트 방식으로 전달                
                hwr_RE.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr_RE.UserAgent = "mannatech";
                Encoding encoding_RE = Encoding.UTF8;
                byte[] buffer_RE = encoding_RE.GetBytes(str_sendvalue);
                hwr_RE.ContentLength = buffer_RE.Length;

                Stream sendStream_RE = hwr_RE.GetRequestStream(); // sendStream 을 생성한다.
                sendStream_RE.Write(buffer, 0, buffer_RE.Length); // 데이터를 전송한다.
                sendStream_RE.Close(); // sendStream 을 종료한다.

                HttpWebResponse wRes_RE;
                try
                {
                    wRes_RE = (HttpWebResponse)hwr_RE.GetResponse();
                }
                catch (Exception ee)
                {
                    return "-1";
                }

                Stream respPostStream_RE = wRes_RE.GetResponseStream();
                StreamReader readerPost_RE = new StreamReader(respPostStream_RE, Encoding.UTF8);

                string getstring_RE = null;
                getstring_RE = readerPost_RE.ReadToEnd().ToString();

                SuccessYN = Return_Card_Approve_Return_Data_TH(getstring_RE, OrderNumber, C_Index, Seq_No, ReturnPrice, ReturnGubun, ref ErrMessage);
            }

            return SuccessYN;
        }

        void Card_Approve_Return_Data_TH(string OrderNumber, int C_Index, string OrderNumber_R, int C_Index_R, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice, ref string ReturnGubun)
        {
            string Tsql = "";

            /*원승인내역*/
            Tsql = " Select ";
            Tsql = Tsql + " CASE WHEN C_C_Sum_Price1 > 0 THEN 'Y' ELSE 'N' END Chk_Return ";
            Tsql = Tsql + " , C_Number3 ";
            Tsql = Tsql + " , C_Price1 ";
            Tsql = Tsql + " , C_AppDate1 ";
            Tsql = Tsql + " , C_Number1 ";
            Tsql = Tsql + " , TerminalID  ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " And C_index = " + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Approve", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            string C_Number1 = "", C_Number3 = "", ChkReturn = "", AppDate = "", mall_id = "";
            double PaymentPrice = 0;
            string req_id = cls_User.gid;  //요청자ID
            string mgr_msg = "";

            C_Number3 = ds.Tables["Card_Approve"].Rows[0]["C_Number3"].ToString();
            ChkReturn = ds.Tables["Card_Approve"].Rows[0]["Chk_Return"].ToString();
            PaymentPrice = double.Parse(ds.Tables["Card_Approve"].Rows[0]["C_Price1"].ToString());
            AppDate = ds.Tables["Card_Approve"].Rows[0]["C_AppDate1"].ToString();
            C_Number1 = encrypter.Decrypt(ds.Tables["Card_Approve"].Rows[0]["C_Number1"].ToString());
            mall_id = ds.Tables["Card_Approve"].Rows[0]["TerminalID"].ToString();

            /*환불내역*/
            Tsql = " Select ";
            Tsql = Tsql + " C_Price1 ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber_R + "' ";
            Tsql = Tsql + " And C_index = " + C_Index_R;

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Return", ds) == false) return;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            ReturnPrice = Math.Abs(double.Parse(ds.Tables["Card_Return"].Rows[0]["C_Price1"].ToString()) );


            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            string mgr_txtype = "";

            if (ChkReturn == "Y")       //부분취소
            {
                if (int.Parse(AppDate) < int.Parse(NowDate))    //매입부분취소
                {
                    mgr_txtype = "31";
                }
                else  //승인부분취소
                {
                    mgr_txtype = "32";
                }
            }
            else    //전체환불이 가능한 상태에서
            {
                if (PaymentPrice == ReturnPrice)    //환불금액하고 승인금액하고 같으면 전체 환불
                {
                    mgr_txtype = "40";
                }
                else
                {
                    if (int.Parse(AppDate) < int.Parse(NowDate))    //매입부분취소
                    {
                        mgr_txtype = "31";
                    }
                    else  //승인부분취소
                    {
                        mgr_txtype = "32";
                    }
                }
            }
            /*
            str_sendvalue = "mgr_txtype=" + mgr_txtype;                 //거래구분(31:매입부분취소, 32:승인부분취소, 40:취소(승인/매입 자동판단 취소)
            str_sendvalue = str_sendvalue + "&org_cno=" + C_Number3;    //거래번호
            if (mgr_txtype != "40")
            {
                str_sendvalue = str_sendvalue + "&mgr_amt=" + ReturnPrice;  //금액
            }
            str_sendvalue = str_sendvalue + "&req_id=" + cls_User.gid;  //요청자ID
            str_sendvalue = str_sendvalue + "&mall_id=" + mall_id;
            */

            //일단 구매취소건으로 가자 
            str_sendvalue = "&chargeId=" + C_Number3;
            str_sendvalue = str_sendvalue + "&amount=" + ReturnPrice;


            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C3','" + C_Number3 + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());
            ReturnGubun = mgr_txtype;
            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }


        private string Return_Card_Approve_Return_Data_TH(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, string ReturnGubun, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number4 = ReturnData["refundId"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";

                if (ReturnGubun == "40")
                {
                  //  StrSql = StrSql + "  C_Number3 = ''";  //ksnet 거래번호
                    StrSql = StrSql + "  C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호
                    StrSql = StrSql + " ,C_CancelTF = 1 ";
                    StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                    StrSql = StrSql + " ,C_Price1 = 0 ";
                }
                else
                {
                    StrSql = StrSql + " C_C_Price1 = " + ReturnPrice;
                    StrSql = StrSql + " , C_Price1 = C_Price1 - " + ReturnPrice;
                    StrSql = StrSql + " , C_C_Sum_Price1 = C_C_Sum_Price1 + " + ReturnPrice;
                }
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "", "", 1);
            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Etc = C_Etc + '" + ErrMessage + "'";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;
            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        /*카드 취소*/
        public string Dir_Card_Approve_Cancel(string OrderNumber, int C_Index, ref string ErrMessage)
        {
            // 태국인 경우
            if (cls_User.gid_CountryCode == "TH")
            {
                return Dir_Card_Approve_Cancel_TH(OrderNumber, C_Index, ref ErrMessage);
            }


            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            double ReturnPrice = 0;

            Card_Approve_Cancel_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_Cancel_Data(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ref ErrMessage);

            return SuccessYN;
        }

        void Card_Approve_Cancel_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice)
        {
            string Tsql = "";

            Tsql = " Select C_index, C_Price1, C_Number1, C_Number3 , C_Number2 ,C_Period1,C_Period2, TerminalID";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 3 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string mgr_txtype = "40";                                                   //40:취소(승인/매입 자동판단취소)
            string C_Number3 = ds.Tables["Card_Cancel"].Rows[0]["C_Number3"].ToString();  //PG거래번호
            string req_id = cls_User.gid;                                               //요청자ID
            string mgr_msg = "";                                                        //취소사유
            string C_Number1 = encrypter.Decrypt(ds.Tables["Card_Cancel"].Rows[0]["C_Number1"].ToString());    //카드번호
            string mall_id = ds.Tables["Card_Cancel"].Rows[0]["TerminalID"].ToString();     //단말기ID
            ReturnPrice = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());

            str_sendvalue = "mgr_txtype=" + mgr_txtype;
            str_sendvalue = str_sendvalue + "&org_cno=" + C_Number3;
            str_sendvalue = str_sendvalue + "&req_id=" + req_id;
            str_sendvalue = str_sendvalue + "&mgr_msg=" + mgr_msg;
            str_sendvalue = str_sendvalue + "&mall_id=" + mall_id;

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C','" + C_Number3 + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;

        }


        /*네이버페이 취소*/
        public string Dir_Naver_Approve_Cancel(string OrderNumber, int C_Index, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            double ReturnPrice = 0;

            Naver_Approve_Cancel_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice);

            if (Ord_SW == 0)
                return "";

            string URL = "https://www.mannatech.co.kr/common/cs/naverPay/cancel.do";

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Naver_Approve_Cancel_Data(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ref ErrMessage);

            return SuccessYN;
        }

        void Naver_Approve_Cancel_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice)
        {
            string Tsql = "";

            Tsql = " Select C_index, C_Price1, C_Number1, C_Number3 , C_Number2 ,C_Period1,C_Period2, TerminalID";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "'";
            Tsql = Tsql + " And   C_TF   = 7 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index =" + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";
            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Cancel", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            string mgr_txtype = "40";                                                   //40:취소(승인/매입 자동판단취소)
            string C_Number3 = ds.Tables["Card_Cancel"].Rows[0]["C_Number3"].ToString();  //PG거래번호
            string req_id = cls_User.gid;                                               //요청자ID
            string mgr_msg = "";                                                        //취소사유
            string C_Number1 = encrypter.Decrypt(ds.Tables["Card_Cancel"].Rows[0]["C_Number1"].ToString());    //카드번호
            string mall_id = ds.Tables["Card_Cancel"].Rows[0]["TerminalID"].ToString();     //단말기ID
            ReturnPrice = double.Parse(ds.Tables["Card_Cancel"].Rows[0]["C_Price1"].ToString());

            //str_sendvalue = "mgr_txtype=" + mgr_txtype;
            str_sendvalue = str_sendvalue + "paymentId=" + C_Number3;
            str_sendvalue = str_sendvalue + "&cancelAmount=" + ReturnPrice;


            //string StrSql = "";
            //StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C','" + C_Number3 + "','" + cls_User.gid + "'";
            //Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            //Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;

        }
        private string Return_Card_Approve_Cancel_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch (Exception e)
            {
                MessageBox.Show("통신에러");

                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number4 = ReturnData["r_cno"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                //StrSql = StrSql + "  C_Number3 = ''";  //거래번호
                StrSql = StrSql + "  C_Number4 = '" + C_Number4 + "'";  //거래번호
                StrSql = StrSql + " ,C_CancelTF = 1 ";
                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                StrSql = StrSql + " ,C_Price1 = 0 ";

                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "", "", 1);
            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }
        private string Return_Naver_Approve_Cancel_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch (Exception e)
            {
                MessageBox.Show("통신에러");

                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                //C_Number4 = ReturnData["r_cno"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["message"].ToString();
            }


            if (SuccessYN == "Y")
            {
                //StrSql = "Update tbl_Sales_Cacu SET ";
                //StrSql = StrSql + "  C_Number3 = ''";  //거래번호
                //StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //거래번호
                //StrSql = StrSql + " ,C_CancelTF = 1 ";
                //StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                //StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                //StrSql = StrSql + " ,C_Price1 = 0 ";

                //StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                //StrSql = StrSql + " And   C_index = " + C_Index;

                //Temp_Connect.Update_Data(StrSql, "", "", 1);
            }

            //StrSql = "Update tbl_Sales_Cacu_Card SET ";
            //StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            //StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            //StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            //if (SuccessYN == "Y")
            //    StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            //StrSql = StrSql + " Where Seqno  =" + Seq_No;

            //Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        /*카드 부분 취소*/
        public string Dir_Card_Approve_Return(string OrderNumber, int C_Index, string OrderNumber_R, int C_Index_R, ref string ErrMessage)
        {
            //태국
            if(cls_User.gid_CountryCode == "TH")
            {
                return Dir_Card_Approve_Return_TH(OrderNumber, C_Index, OrderNumber_R, C_Index_R, ref ErrMessage);
            }


            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "", ReturnGubun = "";
            double ReturnPrice = 0;
            Card_Approve_Return_Data(OrderNumber, C_Index, OrderNumber_R, C_Index_R, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice, ref ReturnGubun);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelCardURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            SuccessYN = Return_Card_Approve_Return_Data(getstring, OrderNumber, C_Index, Seq_No, ReturnPrice, ReturnGubun, ref ErrMessage);

            //승인부분취소, 매입부분취소를 던졌는데 에러가 났다......그러면 반대로 다시 태워버리자
            if (SuccessYN == "N")
            {
                Ord_SW = 0;
                Seq_No = 0;
                str_sendvalue = "";
                SuccessYN = "";
                ReturnGubun = "";
                ReturnPrice = 0;
                Card_Approve_Return_Data(OrderNumber, C_Index, OrderNumber_R, C_Index_R, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref ReturnPrice, ref ReturnGubun);

                if (Ord_SW == 0)
                    return "";

                string URL_RE = cls_app_static_var.CancelCardURL;

                HttpWebRequest hwr_RE = (HttpWebRequest)WebRequest.Create(URL_RE);
                hwr_RE.Method = "POST"; // 포스트 방식으로 전달                
                hwr_RE.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
                hwr_RE.UserAgent = "mannatech";
                Encoding encoding_RE = Encoding.UTF8;
                byte[] buffer_RE = encoding_RE.GetBytes(str_sendvalue);
                hwr_RE.ContentLength = buffer_RE.Length;

                Stream sendStream_RE = hwr_RE.GetRequestStream(); // sendStream 을 생성한다.
                sendStream_RE.Write(buffer, 0, buffer_RE.Length); // 데이터를 전송한다.
                sendStream_RE.Close(); // sendStream 을 종료한다.

                HttpWebResponse wRes_RE;
                try
                {
                    wRes_RE = (HttpWebResponse)hwr_RE.GetResponse();
                }
                catch (Exception ee)
                {
                    return "-1";
                }

                Stream respPostStream_RE = wRes_RE.GetResponseStream();
                StreamReader readerPost_RE = new StreamReader(respPostStream_RE, Encoding.UTF8);

                string getstring_RE = null;
                getstring_RE = readerPost_RE.ReadToEnd().ToString();

                SuccessYN = Return_Card_Approve_Return_Data(getstring_RE, OrderNumber, C_Index, Seq_No, ReturnPrice, ReturnGubun, ref ErrMessage);
            }

            return SuccessYN;
        }

        void Card_Approve_Return_Data(string OrderNumber, int C_Index, string OrderNumber_R, int C_Index_R, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double ReturnPrice, ref string ReturnGubun)
        {
            string Tsql = "";

            /*원승인내역*/
            Tsql = " Select ";
            Tsql = Tsql + " CASE WHEN C_C_Sum_Price1 > 0 THEN 'Y' ELSE 'N' END Chk_Return ";
            Tsql = Tsql + " , C_Number3 ";
            Tsql = Tsql + " , C_Price1 ";
            Tsql = Tsql + " , C_AppDate1 ";
            Tsql = Tsql + " , C_Number1 ";
            Tsql = Tsql + " , TerminalID  ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " And C_index = " + C_Index;
            Tsql = Tsql + " And C_Price1 > 0 ";

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Approve", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            string C_Number1 = "", C_Number3 = "", ChkReturn = "", AppDate = "", mall_id = "";
            double PaymentPrice = 0;

            C_Number3 = ds.Tables["Card_Approve"].Rows[0]["C_Number3"].ToString();
            ChkReturn = ds.Tables["Card_Approve"].Rows[0]["Chk_Return"].ToString();
            PaymentPrice = double.Parse(ds.Tables["Card_Approve"].Rows[0]["C_Price1"].ToString());
            AppDate = ds.Tables["Card_Approve"].Rows[0]["C_AppDate1"].ToString();
            C_Number1 = encrypter.Decrypt(ds.Tables["Card_Approve"].Rows[0]["C_Number1"].ToString());
            mall_id = ds.Tables["Card_Approve"].Rows[0]["TerminalID"].ToString();

            /*환불내역*/
            Tsql = " Select ";
            Tsql = Tsql + " C_Price1 ";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            Tsql = Tsql + " Where OrderNumber = '" + OrderNumber_R + "' ";
            Tsql = Tsql + " And C_index = " + C_Index_R;

            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "Card_Return", ds) == false) return;
            ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            ReturnPrice = Math.Abs(double.Parse(ds.Tables["Card_Return"].Rows[0]["C_Price1"].ToString()));

            string NowDate = DateTime.Now.ToString("yyyyMMdd");
            string mgr_txtype = "";

            if (ChkReturn == "Y")       //부분취소
            {
                if (int.Parse(AppDate) < int.Parse(NowDate))    //매입부분취소
                {
                    mgr_txtype = "31";
                }
                else  //승인부분취소
                {
                    mgr_txtype = "32";
                }
            }
            else    //전체환불이 가능한 상태에서
            {
                if (PaymentPrice == ReturnPrice)    //환불금액하고 승인금액하고 같으면 전체 환불
                {
                    mgr_txtype = "40";
                }
                else
                {
                    if (int.Parse(AppDate) < int.Parse(NowDate))    //매입부분취소
                    {
                        mgr_txtype = "31";
                    }
                    else  //승인부분취소
                    {
                        mgr_txtype = "32";
                    }
                }
            }

            str_sendvalue = "mgr_txtype=" + mgr_txtype;                 //거래구분(31:매입부분취소, 32:승인부분취소, 40:취소(승인/매입 자동판단 취소)
            str_sendvalue = str_sendvalue + "&org_cno=" + C_Number3;    //거래번호
            if (mgr_txtype != "40")
            {
                str_sendvalue = str_sendvalue + "&mgr_amt=" + ReturnPrice;  //금액
            }
            str_sendvalue = str_sendvalue + "&req_id=" + cls_User.gid;  //요청자ID
            str_sendvalue = str_sendvalue + "&mall_id=" + mall_id;



            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + ",'" + OrderNumber + "','" + encrypter.Encrypt(C_Number1) + "','','C3','" + C_Number3 + "','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());
            ReturnGubun = mgr_txtype;
            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }


        private string Return_Card_Approve_Return_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, double ReturnPrice, string ReturnGubun, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number4 = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number4 = ReturnData["r_cno"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }


            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";

                if (ReturnGubun == "40")
                {
                    StrSql = StrSql + "  C_Number3 = ''";  //ksnet 거래번호
                    StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "'";  //ksnet 거래번호
                    StrSql = StrSql + " ,C_CancelTF = 1 ";
                    StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                    StrSql = StrSql + " ,C_CancelPrice = C_Price1 ";
                    StrSql = StrSql + " ,C_Price1 = 0 ";
                }
                else
                {
                    StrSql = StrSql + " C_C_Price1 = " + ReturnPrice;
                    StrSql = StrSql + " , C_Price1 = C_Price1 - " + ReturnPrice;
                    StrSql = StrSql + " , C_C_Sum_Price1 = C_C_Sum_Price1 + " + ReturnPrice;
                }
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql, "", "", 1);
            }
            else
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Etc = C_Etc + '" + ErrMessage + "'";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;
            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "'";
            StrSql = StrSql + " ,rTransactionNo = '" + C_Number4 + "'";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            if (SuccessYN == "Y")
                StrSql = StrSql + " , C_C_Price1 = " + ReturnPrice;
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        /*가상계좌 발행*/
        public string Dir_VR_Account_Approve_OK(string OrderNumber, int C_Index, int mbid2, ref string ErrMessage)
        {
            int Ord_SW = 0;
            int Seq_No = 0;
            double Amount = 0;
            string str_sendvalue = "";
            VR_Account_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref Amount);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.ApproveAccountURL;


            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            string SuccessYN = "";
            SuccessYN = Return_AV_Account_OK_Data(getstring, OrderNumber, C_Index, mbid2, Seq_No, ref ErrMessage);

            return SuccessYN;
        }

        void VR_Account_OK_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref double Amount)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql = Tsql + " C_Price2 ";
            Tsql = Tsql + " , C_Code BankCode ";
            Tsql = Tsql + " , CASE WHEN C_Cash_Send_TF IN (1,2) THEN '1' ELSE '0' END ReceiptYN ";
            Tsql = Tsql + " , CASE WHEN C_Cash_Bus_TF = 0 THEN '01' WHEN C_Cash_Bus_TF = 1 THEN '02' ELSE '' END Receipt_Gubun1 ";
            Tsql = Tsql + " , CASE WHEN C_Cash_Bus_TF = 0 THEN '3' WHEN C_Cash_Bus_TF = 1 THEN '4' ELSE '' END Receipt_Gubun2 ";
            Tsql = Tsql + " , C_Cash_Send_Nu Receipt_Num"; //현금인수증신청번호
            Tsql = Tsql + " , tbl_Sales_Cacu.OrderNumber ";
            Tsql = Tsql + " , tbl_SalesDetail.mbid2 ";
            Tsql = Tsql + " , tbl_SalesDetail.M_Name ";
            Tsql = Tsql + " , ISNULL(tbl_Memberinfo.Email, '') Email";
            Tsql = Tsql + " , ISNULL(tbl_Memberinfo.hometel, '') HomeTel";
            Tsql = Tsql + " , ISNULL(tbl_Memberinfo.hptel, '') HpTel ";
            Tsql = Tsql + " , ISNULL(tbl_Memberinfo.Address1, '') + ' ' + ISNULL(tbl_Memberinfo.Address2, '') AS Address ";
            Tsql = Tsql + " , CONVERT(VARCHAR(8), GETDATE(), 112) DT ";
            Tsql = Tsql + " , (SELECT TOP 1 ItemName FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER) +  ";
            Tsql = Tsql + "   (SELECT CASE WHEN COUNT(ORDERNUMBER) < 2 THEN '' ELSE '외' + CONVERT(VARCHAR, COUNT(ORDERNUMBER) - 1) + '개' END FROM TBL_SALESITEMDETAIL (NOLOCK) WHERE ORDERNUMBER = tbl_SalesDetail.ORDERNUMBER AND SellState <> 'C_1') AS ItemName   ";
            Tsql = Tsql + " From tbl_Sales_Cacu (NOLOCK) ";
            Tsql = Tsql + " INNER JOIN tbl_SalesDetail (NOLOCK) ON tbl_Sales_Cacu.OrderNumber = tbl_SalesDetail.OrderNumber ";
            Tsql = Tsql + " INNER JOIN tbl_Memberinfo (NOLOCK) ON tbl_SalesDetail.mbid = tbl_Memberinfo.mbid AND tbl_SalesDetail.mbid2 = tbl_Memberinfo.mbid2 ";
            Tsql = Tsql + " WHERE tbl_Sales_Cacu.OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_TF = 5 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_Number3 = '' ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_Number1 = '' ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_Price2 > 0 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_Index = " + C_Index;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "AV_ACCOUNT", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++
            int mbid2 = 0;
            string BankCode = "", ReceiptYN = "", ReceiptGubun_1 = "", ReceiptGubun_2 = "", ReceiptNum = "";
            string Name = "", Email = "", HomeTel = "", HpTel = "", Address = "", DT = "";
            string ItemName = "";

            Amount = double.Parse(ds.Tables["AV_ACCOUNT"].Rows[0]["C_Price2"].ToString());
            BankCode = ds.Tables["AV_ACCOUNT"].Rows[0]["BankCode"].ToString();
            ReceiptYN = ds.Tables["AV_ACCOUNT"].Rows[0]["ReceiptYN"].ToString();
            ReceiptGubun_1 = ds.Tables["AV_ACCOUNT"].Rows[0]["Receipt_Gubun1"].ToString();
            ReceiptGubun_2 = ds.Tables["AV_ACCOUNT"].Rows[0]["Receipt_Gubun2"].ToString();
            ReceiptNum = ds.Tables["AV_ACCOUNT"].Rows[0]["Receipt_Num"].ToString();
            mbid2 = int.Parse(ds.Tables["AV_ACCOUNT"].Rows[0]["mbid2"].ToString());
            Name = ds.Tables["AV_ACCOUNT"].Rows[0]["M_Name"].ToString();
            Email = ds.Tables["AV_ACCOUNT"].Rows[0]["Email"].ToString();
            HomeTel = ds.Tables["AV_ACCOUNT"].Rows[0]["HomeTel"].ToString().Replace("-", "");
            HpTel = ds.Tables["AV_ACCOUNT"].Rows[0]["HpTel"].ToString().Replace("-", "");
            Address = ds.Tables["AV_ACCOUNT"].Rows[0]["Address"].ToString();
            DT = ds.Tables["AV_ACCOUNT"].Rows[0]["DT"].ToString();
            ItemName = ds.Tables["AV_ACCOUNT"].Rows[0]["ItemName"].ToString();

            str_sendvalue = "EP_tr_cd=00101000";                            //요청구분 00101000 고정
            str_sendvalue = str_sendvalue + "&EP_vacct_txtype=10";          //처리종류 10:계좌발급            
            str_sendvalue = str_sendvalue + "&EP_tot_amt=" + Amount;      //총금액
            str_sendvalue = str_sendvalue + "&EP_currency=00";              //통화코드
            str_sendvalue = str_sendvalue + "&EP_bank_cd=" + BankCode;      //계좌은행 코드
            //str_sendvalue = str_sendvalue + "&EP_expire_date=" + DT;        //계좌사용 만료일자
            //str_sendvalue = str_sendvalue + "&EP_expire_time=235959";       //계좌사용 만료시간
            str_sendvalue = str_sendvalue + "&EP_cash_yn=" + ReceiptYN;     //현금영수증 신청 유무
            if (ReceiptYN == "1")
            {
                str_sendvalue = str_sendvalue + "&EP_cash_issue_type=" + ReceiptGubun_1;    //현금영수증 용도 ('01':소득공제, '02':지출증빙)
                str_sendvalue = str_sendvalue + "&EP_cash_auth_type=" + ReceiptGubun_2;     //인증구분(2:주민등록번호, 3:핸드폰번호, 4:사업자번호)
                str_sendvalue = str_sendvalue + "&EP_cash_auth_value=" + ReceiptNum;        //현금영수증 신청번호
            }
            str_sendvalue = str_sendvalue + "&EP_order_no=" + OrderNumber;  //주문번호
            str_sendvalue = str_sendvalue + "&EP_user_id=" + mbid2;         //회원번호
            str_sendvalue = str_sendvalue + "&EP_user_nm=" + Name;          //회원명
            str_sendvalue = str_sendvalue + "&EP_user_mail=" + Email;       //회원메일
            str_sendvalue = str_sendvalue + "&EP_user_phone1=" + HomeTel;   //회원 집 연락처
            str_sendvalue = str_sendvalue + "&EP_user_phone2=" + HpTel;     //회원 휴대폰 연락처
            str_sendvalue = str_sendvalue + "&EP_user_addr=" + Address;     //회원주소
            str_sendvalue = str_sendvalue + "&EP_product_nm=" + ItemName;    //제품명

            string StrSql = "";
            StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','','가상계좌','A','','" + cls_User.gid + "'";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        private string Return_AV_Account_OK_Data(string Getstring, string OrderNumber, int C_Index, int mbid2, int Seq_No, ref string ErrMessage)
        {
            string SuccessYN = "", C_Number1 = "", C_Number3 = "", BankCode = "", BankName = "", StrSql = "";
            double Price = 0;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
            }
            catch
            {
                return "N";
            }

            SuccessYN = ReturnData["successYN"].ToString();

            if (SuccessYN == "Y")
            {
                C_Number3 = ReturnData["r_cno"].ToString();
                C_Number1 = ReturnData["r_account_no"].ToString();
                BankCode = ReturnData["r_bank_cd"].ToString();
                BankName = ReturnData["r_bank_nm"].ToString();
                Price = double.Parse(ReturnData["r_amount"].ToString());
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }

            if (SuccessYN == "Y")
            {

                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Number3  = '" + C_Number3 + "'"; //거래번호
                StrSql = StrSql + " , C_Number1 = dbo.ENCRYPT_AES256('" + C_Number1 + "')"; //가상계좌번호
                StrSql = StrSql + " ,C_Code = '" + BankCode + "' ";
                StrSql = StrSql + " ,C_CodeName = '" + BankName + "' ";
                StrSql = StrSql + " ,C_CancelTF = 0 ";
                StrSql = StrSql + " ,C_CancelDate = '' ";
                StrSql = StrSql + " ,C_CancelPrice = 0 ";

                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);


                StrSql = "Insert into tbl_Sales_Cacu_ACC   ";
                StrSql = StrSql + " (OrderNumber ,C_index ,C_Cash_Receipt_TF , Bank_Code , Bank_ACC_Account , C_Price2, mbid, mbid2 ,expire_date , Cul_Send_TF , Exi_TF_OrderNumber ) ";
                StrSql = StrSql + " Values ('" + OrderNumber + "'";
                StrSql = StrSql + "," + C_Index;
                StrSql = StrSql + ",0";
                StrSql = StrSql + ",'" + BankCode + "'";
                StrSql = StrSql + ", '" + C_Number1 + "'";
                StrSql = StrSql + "," + Price;
                StrSql = StrSql + ",'' ";
                StrSql = StrSql + "," + mbid2;
                StrSql = StrSql + ",'',0 , '' ) ";

                Temp_Connect.Update_Data(StrSql);
            }
            else
            {
                C_Number3 = "";
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Price1  = 0 ";
                StrSql = StrSql + " , C_Etc = C_Etc+ '" + ErrMessage + "'";  //승인 오류시 비고칸에 내역을 넣도록 한다.
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);
            }


            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " Card_No = '" + C_Number1 + "'";
            StrSql = StrSql + " ,C_Number3 = '" + C_Number3 + "'";
            StrSql = StrSql + " ,rStatus = '" + SuccessYN + "' ";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);

            return SuccessYN;
        }


        /*가상계좌 취소*/
        public string Dir_VR_Account_Approve_Cancel(string OrderNumber, int C_Index)
        {
            int Ord_SW = 0;
            int Seq_No = 0;
            string str_sendvalue = "";
            VR_Account_Cancel_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CancelAccountURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "PMI";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            string SuccessYN = "";
            SuccessYN = Return_AV_Account_Cancel_Data(getstring, OrderNumber, C_Index, Seq_No);

            return SuccessYN;
        }

        void VR_Account_Cancel_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";

            Tsql = " Select ";
            Tsql = Tsql + " tbl_Sales_Cacu.C_Number3 ";
            Tsql = Tsql + " From tbl_Sales_Cacu (NOLOCK) ";
            Tsql = Tsql + " Where tbl_Sales_Cacu.OrderNumber = '" + OrderNumber + "' ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_TF = 5 ";
            Tsql = Tsql + " And tbl_Sales_Cacu.C_index = " + C_Index;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "AV_ACCOUNT_CANCEL", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            string tid = "";                        //다날 거래키(필수)
            string TxType = "SESSIONCLOSE";         //트랜잭션 타입(고정값)
            string ServiceType = "DANALVACCOUNT";   //서비스 타입(고정값)

            tid = ds.Tables["AV_ACCOUNT_CANCEL"].Rows[0]["C_Number3"].ToString();

            str_sendvalue = "tid=" + tid;
            str_sendvalue = str_sendvalue + "&txType=" + TxType;
            str_sendvalue = str_sendvalue + "&serviceType=" + ServiceType;

            string StrSql = "";
            StrSql = " EXEC Usp_Insert_tbl_Sales_Cacu_Card " + C_Index + " ,'" + OrderNumber + "','','가상계좌','C','" + tid + "','" + cls_User.gid + "' ";
            Temp_Connect.Open_Data_Set(StrSql, "Cacu_Card", ds);

            Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        private string Return_AV_Account_Cancel_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No)
        {
            string SuccessYN = "", C_Number4 = "", ErrMessage = "", StrSql = "";
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();
            }
            catch
            {
                return "N";
            }

            if (SuccessYN == "Y")
            {
                C_Number4 = ReturnData["tid"].ToString();
            }
            else
            {
                ErrMessage = ReturnData["errMessage"].ToString();
            }

            if (SuccessYN == "Y")
            {
                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Number3 = ''";  //ksnet 거래번호
                StrSql = StrSql + " ,C_Number4 = '" + C_Number4 + "' ";
                StrSql = StrSql + " ,C_CancelTF = 1 ";
                StrSql = StrSql + " ,C_CancelDate = Convert(Varchar(25),GetDate(),21) ";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);

                StrSql = "Update tbl_Sales_Cacu_ACC SET ";
                StrSql = StrSql + " Cul_Send_TF = 1";
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                Temp_Connect.Update_Data(StrSql);

            }

            StrSql = "Update tbl_Sales_Cacu_Card SET ";
            StrSql = StrSql + " rStatus = '" + SuccessYN + "' ";
            StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
            StrSql = StrSql + " Where Seqno  =" + Seq_No;

            Temp_Connect.Update_Data(StrSql, "", "", 1);


            return SuccessYN;
        }




        /*현금영수증 취소*/
        public string Dir_VR_Cash_Receipt_All_Cancel(string OrderNumber, int C_Index)
        {
            int Ord_SW = 0;
            int Seq_No = 0;
            string str_sendvalue = string.Empty;
            string Send_Number = string.Empty;
            VR_Cash_Receipt_All_Cancel_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref Send_Number);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CashCancelURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);
            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            getstring = getstring.Trim();
            string SuccessYN = "";
            SuccessYN = Return_VR_Cash_Receipt_All_Cancel_Data(getstring, OrderNumber, C_Index, Seq_No, Send_Number);

            return SuccessYN;
        }

        void VR_Cash_Receipt_All_Cancel_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref string Send_Number)
        {
            string Tsql = string.Empty;

            ///*원승인내역*/
            //Tsql = " Select C_Cash_Number, C_Cash_Send_Nu";
            //Tsql = Tsql + " From tbl_Sales_Cacu (nolock) ";
            //Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "' ";
            //Tsql = Tsql + " And C_index = " + C_Index;

            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(Tsql, "AV_ACCOUNT", ds) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;

            //if (ReCnt == 0) return;
            ////++++++++++++++++++++++++++++++++

            //string transactionNo = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Number"].ToString();
            //Send_Number = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_Nu"].ToString();

            //str_sendvalue = "transactionNo=" + transactionNo;

            ////setbyte = Encoding.Default.GetBytes(str_sendvalue);
            //Ord_SW = 1;


            //20221021현금영수증 부분취소도 넣는다.
            ///*원승인내역*/
            //Tsql = " Select c_tf,c_number3, c_cash_number4 ";
            //Tsql = Tsql + " From tbl_Sales_Cacu (nolock)  ";
            //Tsql = Tsql + " Where OrderNumber = '" + OrderNumber + "' ";
            //Tsql = Tsql + " And C_index = " + C_Index;

            Tsql = " Select A.c_tf, A.c_number3, A.c_cash_number4 , ISNULL(B.RETURNTF,'')  AS RETURNTF ,ISNULL( B.TOTALINPUTPRICE * -1 , 0) AS TOTALINPUTPRICE";
            Tsql = Tsql + " From tbl_Sales_Cacu (nolock) A ";
            Tsql = Tsql + " LEFT join (SELECT * FROM tbl_SalesDetail (NOLOCK) WHERE Re_BaseOrderNumber = '" + OrderNumber + "') B ON A.OrderNumber = B.Re_BaseOrderNumber ";
            Tsql = Tsql + "Where A.OrderNumber = '" + OrderNumber + "'  ";
            Tsql = Tsql + "And C_index = " + C_Index;

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(Tsql, "AV_ACCOUNT", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++

            string c_number3 = ds.Tables["AV_ACCOUNT"].Rows[0]["c_number3"].ToString();//거래번호
            string c_tf = ds.Tables["AV_ACCOUNT"].Rows[0]["c_tf"].ToString();//거래번호
            string c_cash_number4 = ds.Tables["AV_ACCOUNT"].Rows[0]["c_cash_number4"].ToString();//거래번호
            string RETURNTF = ds.Tables["AV_ACCOUNT"].Rows[0]["RETURNTF"].ToString();//부분반품인지 아닌지.
            string TOTALINPUTPRICE = ds.Tables["AV_ACCOUNT"].Rows[0]["TOTALINPUTPRICE"].ToString().Replace(".0000", "");
            //Send_Number = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_Nu"].ToString();
            //부분반품일시
            if (RETURNTF == "3")
            {
                if (c_tf == "5")
                {
                    str_sendvalue = "org_cno=" + c_number3;
                    str_sendvalue += "&mgr_txtype=52";//부분취소
                    str_sendvalue += "&mgr_amt=" + TOTALINPUTPRICE;
                }
                if (c_tf == "1")
                {
                    str_sendvalue = "org_cno=" + c_cash_number4;
                    str_sendvalue += "&mgr_txtype=52";//부분취소
                    str_sendvalue += "&mgr_amt=" + TOTALINPUTPRICE;
                }
            }
            //전부취소시
            if (RETURNTF != "3")
            {
                //전부취소시
                if (c_tf == "5")
                {
                    str_sendvalue = "org_cno=" + c_number3;
                    str_sendvalue += "&mgr_txtype=51";//전부취소
                }
                if (c_tf == "1")
                {
                    str_sendvalue = "org_cno=" + c_cash_number4;
                    str_sendvalue += "&mgr_txtype=51";//전부취소
                }
            }
            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        private string Return_VR_Cash_Receipt_All_Cancel_Data(string Getstring, string OrderNumber, int C_Index, int Seq_No, string Send_Number)
        {
            string SuccessYN = string.Empty;
            string Cash_Canel_No = string.Empty;//승인번호
            string C_Cash_Cancel_Number = string.Empty;//거래번호
            string StrSql = string.Empty;
            string ErrMessage = string.Empty;
            string StatusMessage = string.Empty;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();

                if (SuccessYN == "Y")
                {
                    StatusMessage = ReturnData["successYN"].ToString();
                    Cash_Canel_No = ReturnData["r_auth_no"].ToString();//승인번호
                    C_Cash_Cancel_Number = ReturnData["r_cno"].ToString();//거래번호
                }
                else
                {
                    StatusMessage = ReturnData["successYN"].ToString();

                }
            }
            catch
            {
                return "N";
            }

            if (SuccessYN == "Y")
            {
                DataSet ds = new DataSet();
                StrSql = "EXEC Usp_Insert_tbl_Sales_Cacu_Bank " + C_Index + ",'" + OrderNumber + "','''','C' ,'','" + cls_User.gid + "' ";
                if (Temp_Connect.Open_Data_Set(StrSql, "CashReceiptCancel", ds) == false) return "N";
                if (ds.Tables["CashReceiptCancel"].Rows.Count == 0) return "N";

                if (int.TryParse(ds.Tables["CashReceiptCancel"].Rows[0][0].ToString(), out Seq_No))
                {
                    if (Seq_No.Equals(0))
                        return "N";

                }
                else
                {
                    return "N";
                }


                StrSql = "Update tbl_Sales_Cacu SET ";
                StrSql = StrSql + " C_Cash_Cancel_Number = '" + C_Cash_Cancel_Number + "'";
                StrSql = StrSql + ",C_Number4 = '" + Cash_Canel_No + "'";  //LG 취소 거래번호 
                StrSql = StrSql + " ,C_CancelTF = 1 ";
                StrSql = StrSql + " , C_CancelDate = Convert(Varchar(25),GetDate(),21) ";

                //StrSql = StrSql + " ,C_Price1 = C_Price1 - " + Price; //현금영수증을 취소하면 현금이나 가상계좌 입금금액도 초기화하는거같다. 왜?

                //StrSql = StrSql + " ,C_C_Price1 = " + Price;
                //StrSql = StrSql + " ,C_C_Sum_Price1 = C_C_Sum_Price1 + " + Price;
                StrSql = StrSql + " Where OrderNumber ='" + OrderNumber + "'";
                StrSql = StrSql + " And   C_index = " + C_Index;

                if (Temp_Connect.Update_Data(StrSql) == false) return "N";

                StrSql = "Update tbl_Sales_Cacu_Bank SET ";
                StrSql = StrSql + " rStatus = 'Cancel'";
                StrSql = StrSql + " ,rHTradeDate = ''"; //LG에 중간 사업자가 거래번호를 관리하기에 전송 시간날짜를 우리는 터치하지않는다.
                StrSql = StrSql + " ,rHTradeTime = ''";
                StrSql = StrSql + " ,rHMessage1 = '" + StatusMessage + "'";
                StrSql = StrSql + " ,rHCashTransactionNo = '" + Cash_Canel_No + "'";  //현금영수증 승인번호
                StrSql = StrSql + " ,C_Number4 = '" + Cash_Canel_No + "'";
                StrSql = StrSql + " ,Return_Date = Convert(Varchar(25),GetDate(),21)";
                StrSql = StrSql + " Where Seqno  =" + Seq_No;

                if (Temp_Connect.Update_Data(StrSql) == false) return "N";
            }

            return SuccessYN;
        }


        /*현금영수증 생성 */
        public string Dir_Cash_Receipt_Approve(string OrderNumber, int C_Index, string Send_Number)
        {
            int Ord_SW = 0;
            string str_sendvalue = "";
            string _SendNumber = Send_Number;

            VR_Cash_Receipt_Approve_Data(OrderNumber, C_Index, ref _SendNumber, ref Ord_SW, ref str_sendvalue);

            if (Ord_SW == 0)
                return "";

            string URL = cls_app_static_var.CashReceiptURL;

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();

            string ErrMessage = string.Empty;
            string ResultValue = string.Empty;


            return Return_VR_Cash_Receipt_Approve_Data(OrderNumber, C_Index, _SendNumber, getstring);
        }

        private void VR_Cash_Receipt_Approve_Data(string OrderNumber, int C_Index, ref string Send_Number, ref int Ord_SW, ref string str_sendvalue)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("SELECT Cacu.OrderNumber");
            //sb.AppendLine(",Cacu.C_Price1");
            //sb.AppendLine(",Cacu.C_Price2");
            //sb.AppendLine(",Mem.hptel");
            //sb.AppendLine(",Cacu.C_Cash_Send_TF ");//-- 1 개인, 2 사업자
            //sb.AppendLine(",Cacu.C_Cash_Send_Nu ");//--신청번호 (회사면 사업자번호가 들어갈테고 회원이면 회원의공제코드나 휴대폰번호가 들어갈것이다)
            //sb.AppendLine(",CASE WHEN SumItemDetail.CNT = 1 THEN SumItemDetail.itemName ELSE SumItemDetail.itemName + '외 ' + CAST(CNT AS NVARCHAR)+'개' END ItemNames");
            //sb.AppendLine("FROM tbl_Sales_Cacu Cacu");
            //sb.AppendLine(" JOIN tbl_SalesDetail Detail on Cacu.OrderNumber = Detail.OrderNumber");
            //sb.AppendLine(" JOIN tbl_Memberinfo Mem on Detail.mbid2 = Mem.mbid2");
            //sb.AppendLine(" JOIN (SELECT OrderNumber,min(itemname) itemName, SUM(ItemCount) CNT FROM tbl_SalesItemDetail GROUP BY OrderNumber) as SumItemDetail ON Detail.OrderNumber = SumItemDetail.OrderNumber");
            //sb.AppendLine("WHERE Cacu.OrderNumber ='"  + OrderNumber + "'");
            //sb.AppendLine("AND Cacu.C_index = " + C_Index);
            //sb.AppendLine("AND Cacu.C_TF IN (1, 5) ");//--현금과 가상계좌인경우에만 
            //sb.AppendLine("AND Detail.ReturnTF = 1 ");
            //sb.AppendLine("AND Cacu.C_Price1 > 0 ");
            //sb.AppendLine("AND Cacu.C_Cash_Send_TF IN (1,2)");//-- 신청을하지않았다면 -1 또는 0 값이 걸려있을것이기에 

            ////++++++++++++++++++++++++++++++++
            //cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            //DataSet ds = new DataSet();
            ////테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            //if (Temp_Connect.Open_Data_Set(sb.ToString(), "AV_ACCOUNT", ds) == false) return;
            //int ReCnt = Temp_Connect.DataSet_ReCount;

            //if (ReCnt == 0) return;
            ////++++++++++++++++++++++++++++++++


            //string ordernumber     = ds.Tables["AV_ACCOUNT"].Rows[0]["ordernumber"].ToString();
            //string amount = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Price1"].ToString();
            //string itemNames       = ds.Tables["AV_ACCOUNT"].Rows[0]["itemNames"].ToString();
            //string cashReceiptType = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_TF"].ToString().Equals("1") ? "1" : "2";//(1:소득공제, 2:지출증빙)"
            //string cashReceiptInfo = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_Nu"].ToString().Replace("-","").Replace(" ","").Trim();

            //Send_Number = cashReceiptInfo;

            //str_sendvalue = "ordernumber="+ ordernumber;                           
            //str_sendvalue += "&amount=" + amount;
            //str_sendvalue += "&itemName=" +itemNames;    
            //str_sendvalue += "&cashReceiptType="  + cashReceiptType;
            //str_sendvalue += "&cashReceiptInfo=" + cashReceiptInfo;
            //Ord_SW = 1;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" Cacu.C_Price1"); //거래금액
            sb.AppendLine(",'0' AS service_amt ");// 봉사료(없으면 안던져도 됨)
            sb.AppendLine(",FLOOR(Cacu.C_Price1 - ( Cacu.C_Price1 / 1.1)) as C_VAT "); //부가세
            sb.AppendLine(",Cacu.C_Cash_Send_TF ");//-- 1 개인, 2 사업자
            sb.AppendLine(",Cacu.C_Cash_Send_Nu ");//--신청번호 (회사면 사업자번호가 들어갈테고 회원이면 회원의공제코드나 휴대폰번호가 들어갈것이다)
            sb.AppendLine(",'0' AS  sub_mall_yn");// 하위 가맹점사용여부(0:미사용, 1:사용)
            sb.AppendLine(",'0' AS sub_mall_buss ");// 하위 가맹점 사업자번호
            sb.AppendLine(",Cacu.OrderNumber");// 주문번호
            sb.AppendLine(",Mem.mbid2");// 고객 ID
            sb.AppendLine(",Mem.m_name"); // 고객명
            sb.AppendLine("FROM tbl_Sales_Cacu Cacu");
            sb.AppendLine(" JOIN tbl_SalesDetail Detail on Cacu.OrderNumber = Detail.OrderNumber");
            sb.AppendLine(" JOIN tbl_Memberinfo Mem on Detail.mbid2 = Mem.mbid2");
            sb.AppendLine("WHERE Cacu.OrderNumber ='" + OrderNumber + "'");
            sb.AppendLine("AND Cacu.C_index = " + C_Index);
            sb.AppendLine("AND Cacu.C_TF IN (1, 5) ");//--현금과 가상계좌인경우에만 
            sb.AppendLine("AND Detail.ReturnTF = 1 ");
            sb.AppendLine("AND Cacu.C_Price1 > 0 ");
            sb.AppendLine("AND Cacu.C_Cash_Send_TF IN (1,2)");//-- 신청을하지않았다면 -1 또는 0 값이 걸려있을것이기에 

            //++++++++++++++++++++++++++++++++
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();

            DataSet ds = new DataSet();
            //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
            if (Temp_Connect.Open_Data_Set(sb.ToString(), "AV_ACCOUNT", ds) == false) return;
            int ReCnt = Temp_Connect.DataSet_ReCount;

            if (ReCnt == 0) return;
            //++++++++++++++++++++++++++++++++



            string amount = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Price1"].ToString();//거래금액
            string service_amt = ds.Tables["AV_ACCOUNT"].Rows[0]["service_amt"].ToString(); //봉사료
            string C_VAT = ds.Tables["AV_ACCOUNT"].Rows[0]["C_VAT"].ToString(); //부가세
            string issue_type = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_TF"].ToString().Equals("1") ? "01" : "02";//발행용도(1:소득공제, 2:지출증빙)"
            string auth_type = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_TF"].ToString().Equals("1") ? "03" : "04";//인증구분(02:주민번호 일단 보류 03:휴대폰, 04:사업자번호)"
            string cashReceiptInfo = ds.Tables["AV_ACCOUNT"].Rows[0]["C_Cash_Send_Nu"].ToString().Replace("-", "").Replace(" ", "").Trim(); // 인증번호
            string sub_mall_yn = ds.Tables["AV_ACCOUNT"].Rows[0]["sub_mall_yn"].ToString();// 하위 가맹점사용여부(0:미사용, 1:사용)
            string sub_mall_buss = ds.Tables["AV_ACCOUNT"].Rows[0]["sub_mall_buss"].ToString();// 하위 가맹점 사업자번호
            string ordernumber = ds.Tables["AV_ACCOUNT"].Rows[0]["ordernumber"].ToString();// 주문번호
            string mbid2 = ds.Tables["AV_ACCOUNT"].Rows[0]["mbid2"].ToString();   //고객 ID
            string m_name = ds.Tables["AV_ACCOUNT"].Rows[0]["m_name"].ToString();// 고객명

            Send_Number = cashReceiptInfo;

            str_sendvalue = "EP_tot_amt=" + amount;
            str_sendvalue += "&EP_service_amt=" + service_amt;
            str_sendvalue += "&EP_vat=" + C_VAT;
            str_sendvalue += "&EP_issue_type=" + issue_type;
            str_sendvalue += "&EP_auth_type=" + auth_type;
            str_sendvalue += "&EP_auth_value=" + cashReceiptInfo;
            str_sendvalue += "&EP_sub_mall_yn=" + sub_mall_yn;
            str_sendvalue += "&EP_sub_mall_buss=" + sub_mall_buss;
            str_sendvalue += "&EP_order_no=" + ordernumber;
            str_sendvalue += "&EP_user_id=" + mbid2;
            str_sendvalue += "&EP_user_nm=" + m_name;
            Ord_SW = 1;

        }

        private string Return_VR_Cash_Receipt_Approve_Data(string OrderNumber, int C_Index, string Send_Number, string Getstring)
        {
            StringBuilder sb = new StringBuilder();
            string SuccessYN = string.Empty;
            string tId = string.Empty;//승인번호
            string r_cno = string.Empty; //거래번호
            string StatusMessage = string.Empty;
            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();

                if (SuccessYN == "Y")
                {
                    tId = ReturnData["r_auth_no"].ToString();
                    r_cno = ReturnData["r_cno"].ToString();
                    StatusMessage = ReturnData["successYN"].ToString();

                }
                else
                {
                    StatusMessage = ReturnData["successYN"].ToString();
                }
            }
            catch
            {
                SuccessYN = "N";
            }

            if (SuccessYN.Equals("Y"))
            {

                try
                {
                    sb.Clear();
                    sb.AppendLine("EXEC Usp_Insert_tbl_Sales_Cacu_Bank " + C_Index + ",'" + OrderNumber + "','" + Send_Number + "','A' ,'''' ,'" + cls_User.gid + "' ");
                    DataSet ds = new DataSet();
                    Temp_Connect.Open_Data_Set(sb.ToString(), "Cacu_Card", ds);

                    int Seq_No = int.Parse(ds.Tables["Cacu_Card"].Rows[0][0].ToString());

                    sb.Clear();
                    sb.AppendLine("Update tbl_Sales_Cacu SET ");
                    sb.AppendLine(" C_Cash_Number = '" + tId + "'");  //승인번호
                    sb.AppendLine(" ,C_Cash_Number4 = '" + r_cno + "'");  //거래번호
                    sb.AppendLine(", C_CancelTF = 0 "); //취소된거 다시 재승인할때 업뎃필요하다.
                    sb.AppendLine(" Where OrderNumber ='" + OrderNumber + "'");
                    sb.AppendLine(" And   C_index = " + C_Index);
                    Temp_Connect.Update_Data(sb.ToString(), "", "");

                    sb.Clear();
                    sb.AppendLine("Update tbl_Sales_Cacu_Bank SET ");
                    sb.AppendLine(" rStatus = 'OK'");
                    sb.AppendLine(" ,rHTradeDate = ''");
                    sb.AppendLine(" ,rHTradeTime = ''");
                    sb.AppendLine(" ,rHMessage1 = '" + StatusMessage + "'");
                    sb.AppendLine(" ,rHCashTransactionNo = '" + tId + "'");  //현금영수증 승인번호
                    sb.AppendLine(" ,C_Number3 = '" + r_cno + "'"); // 2018-09-05 LG 모듈은TID 값과 같음 
                    //sb.AppendLine(" ,C_Number3 = '" + tId + "'"); // 2018-09-05 LG 모듈은TID 값과 같음 에이뉴힐은이렇게되어잇음
                    sb.AppendLine(" ,Return_Date = Convert(Varchar(25),GetDate(),21)");
                    sb.AppendLine(" Where Seqno  =" + Seq_No);
                    Temp_Connect.Update_Data(sb.ToString(), "", "");
                }
                catch
                {
                    SuccessYN = "N";
                }
            }


            return SuccessYN;
        }

        /// <summary>
        /// 태국 메일 전송 함수
        /// </summary>
        /// <param name="mbid2">회원번호</param>
        /// <param name="orderNumber">주문번호</param>
        /// <param name="AutoshipNumber">오토십번호</param>
        /// <param name="RegDate">추천/후원인 변경 신청일</param>
        /// <param name="InputMailType">joinMail: 회원가입</param>
        /// <returns></returns>
        public string SendMail_TH(int mbid2, string orderNumber, string AutoshipNumber, string RegDate, ESendMailType_TH InputMailType)
        {
            int Ord_SW = 0;
            string str_sendvalue = "";
            string SuccessYN = "";

            string URL = "";

            switch (InputMailType)
            {
                case ESendMailType_TH.joinMail:
                    URL = cls_app_static_var.joinMail_TH;
                    str_sendvalue = "&mbid2=" + mbid2;
                    break;
                case ESendMailType_TH.autoshipMail:
                    URL = cls_app_static_var.autoshipMail_TH;
                    str_sendvalue = "&mbid2=" + mbid2 + "&autoSeq=" + AutoshipNumber;
                    break;
                case ESendMailType_TH.orderCompleteMail:
                    URL = cls_app_static_var.orderCompleteMail_TH;
                    str_sendvalue = "&orderNumber=" + orderNumber;
                    break;
                case ESendMailType_TH.orderCancelMail:
                    URL = cls_app_static_var.orderCancelMail_TH;
                    str_sendvalue = "&orderNumber=" + orderNumber;
                    break;
                case ESendMailType_TH.changeNominSaveMail:
                    URL = cls_app_static_var.changeNominSaveMail_TH;
                    str_sendvalue = "&mbid2=" + mbid2 + "&RegDate=" + RegDate;
                    break;
                default:
                    break;
            }

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();
            return SuccessYN = Return_Mail(getstring);
        }

        private string Return_Mail(string Getstring)
        {
            string SuccessYN = string.Empty;
            string StrSql = string.Empty;
            string ErrMessage = string.Empty;
            string StatusMessage = string.Empty;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();

                if (SuccessYN == "Y")
                {
                    StatusMessage = ReturnData["successYN"].ToString();
                }
                else
                {
                    StatusMessage = ReturnData["successYN"].ToString();

                }
            }
            catch
            {
                SuccessYN = "N";
            }
            return SuccessYN;
        }


        /*태국문자전송 신규 모듈*/
        /*구현호 과장*/
        public string TH_SMS(int MBID2, string ORDERNUMBER, int C_Index, ref string ErrMessage)
        {
            int Ord_SW = 0, Seq_No = 0;
            string str_sendvalue = "";
            string SuccessYN = "";
            //Card_AutoShip_OK_Data(OrderNumber, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No, ref Conn, ref tran);
            TH_SMS_DETAIL(MBID2, ORDERNUMBER, C_Index, ref Ord_SW, ref str_sendvalue, ref Seq_No);

            if (Ord_SW == 0)
                return "";

            string URL = "https://uat.mannatech.co.th/common/cs/sendSMS.do";

            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(URL);
            hwr.Method = "POST"; // 포스트 방식으로 전달                
            hwr.ContentType = @"application/x-www-form-urlencoded; charset=utf-8";
            hwr.UserAgent = "mannatech";
            Encoding encoding = Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(str_sendvalue);
            hwr.ContentLength = buffer.Length;

            Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
            sendStream.Write(buffer, 0, buffer.Length); // 데이터를 전송한다.
            sendStream.Close(); // sendStream 을 종료한다.

            HttpWebResponse wRes;
            try
            {
                wRes = (HttpWebResponse)hwr.GetResponse();
            }
            catch (Exception ee)
            {
                return "-1";
            }

            Stream respPostStream = wRes.GetResponseStream();
            StreamReader readerPost = new StreamReader(respPostStream, Encoding.UTF8);

            string getstring = null;
            getstring = readerPost.ReadToEnd().ToString();
            return SuccessYN = Return_SMS(getstring);
            //return SuccessYN;
        }

        //void Card_AutoShip_OK_Data(string OrderNumber, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No, ref SqlConnection Conn, ref SqlTransaction tran)
        void TH_SMS_DETAIL(int MBID2, string ORDERNUMBER, int C_Index, ref int Ord_SW, ref string str_sendvalue, ref int Seq_No)
        {
            string Tsql = "";
            //4:주문결제완료
            if (C_Index == 4)
            {
                Tsql = " Select ";
                Tsql += Environment.NewLine + "mbid2,m_name,hptel  from tbl_memberinfo (nolock)  ";
                Tsql += Environment.NewLine + "  Where tbl_memberinfo.mbid2 = " + MBID2 + " ";

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                string M_name = "";         //회원명
                string HpTel = "";          //회원휴대폰번호


                M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
                HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");

                str_sendvalue = "sendNumber=" + HpTel;                             //인증구분
                str_sendvalue = str_sendvalue + "&sendMessage=[MANNATECH] คำสั่งซื้อของคุณ<" + M_name + ">(<" + ORDERNUMBER + ">)ได้รับการชำระแล้ว";            //주문번호

            }

            //5:주문결제취소
            if (C_Index ==5)
            {
                Tsql = " Select ";
                Tsql += Environment.NewLine + "mbid2,m_name,hptel  from tbl_memberinfo (nolock)  ";
                Tsql += Environment.NewLine + "  Where tbl_memberinfo.mbid2 = " + MBID2 + " ";

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                string M_name = "";         //회원명
                string HpTel = "";          //회원휴대폰번호


                M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
                HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");

                str_sendvalue = "sendNumber=" + HpTel;                             //인증구분
                str_sendvalue = str_sendvalue + "&sendMessage=[MANNATECH] คำสั่งซื้อของคุณ<" + M_name + ">(<" + ORDERNUMBER + ">)ถูกยกเลิกแล้ว";            //주문번호

            }

            //6:반품접수등록
            if (C_Index == 6)
            {
                Tsql = " Select ";
                Tsql += Environment.NewLine + "mbid2,m_name,hptel  from tbl_memberinfo (nolock)  ";
                Tsql += Environment.NewLine + "  Where tbl_memberinfo.mbid2 = " + MBID2 + " ";

                //++++++++++++++++++++++++++++++++
                cls_Connect_DB Temp_Connect = new cls_Connect_DB();

                DataSet ds = new DataSet();
                //테이블에 맞게  DataSet에 내역을 넣고 제대로되었으면 true가 오고 아니면 걍 튀어나간다.
                if (Temp_Connect.Open_Data_Set(Tsql, "Card_OK", ds) == false) return;
                int ReCnt = Temp_Connect.DataSet_ReCount;

                if (ReCnt == 0) return;
                //++++++++++++++++++++++++++++++++
                string M_name = "";         //회원명
                string HpTel = "";          //회원휴대폰번호


                M_name = ds.Tables["Card_OK"].Rows[0]["M_Name"].ToString();
                HpTel = ds.Tables["Card_OK"].Rows[0]["hptel"].ToString().Replace("-", "");

                str_sendvalue = "sendNumber=" + HpTel;                             //인증구분
                str_sendvalue = str_sendvalue + "&sendMessage=[MANNATECH] ได้รับการส่งคืนสินค้าของคุณ<" + M_name + ">(<" + ORDERNUMBER + ">)แล้ว";            //주문번호

            }
            setbyte = Encoding.Default.GetBytes(str_sendvalue);
            Ord_SW = 1;
        }

        private string Return_SMS(string Getstring)
        {
            string SuccessYN = string.Empty;
            string Cash_Canel_No = string.Empty;//승인번호
            string C_Cash_Cancel_Number = string.Empty;//거래번호
            string StrSql = string.Empty;
            string ErrMessage = string.Empty;
            string StatusMessage = string.Empty;

            cls_Connect_DB Temp_Connect = new cls_Connect_DB();
            JObject ReturnData = new JObject();

            try
            {
                ReturnData = JObject.Parse(Getstring);
                SuccessYN = ReturnData["successYN"].ToString();

                if (SuccessYN == "Y")
                {
                    StatusMessage = ReturnData["successYN"].ToString();
                    Cash_Canel_No = ReturnData["r_auth_no"].ToString();//승인번호
                    C_Cash_Cancel_Number = ReturnData["r_cno"].ToString();//거래번호
                }
                else
                {
                    StatusMessage = ReturnData["successYN"].ToString();

                }
            }
            catch
            {
                SuccessYN = "N";
            }
            return SuccessYN;
        }
    }
}
