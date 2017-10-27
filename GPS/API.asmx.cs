using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Data;
using GPSTrackingBLL;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;

namespace GPS
{
    /// <summary>
    /// Summary description for API
    /// </summary>
    [WebService(Namespace = "http://.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class API : System.Web.Services.WebService
    {
        SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ToString());
       

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object login(string userName, string password)
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            try
            {
                cls_User obj_usr = new cls_User();
                DataSet ds = new DataSet();
                ds = obj_usr.fn_Login(userName, password, 2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime expiresOn = DateTime.Parse(ds.Tables[0].Rows[0]["expiresOn"].ToString());

                    TimeSpan span = expiresOn - DateTime.Now;
                    double i = span.TotalSeconds;
                    if (expiresOn < DateTime.Now)
                    {
                        row.Add("Status", "User account is not active..");
                        row.Add("id", null);                        
                    }
                    else
                    {
                        row.Add("Status", "Success");
                        row.Add("id", ds.Tables[0].Rows[0]["ID"].ToString());
                    }
                }
                else
                {
                    row.Add("Status", "Wrong Credentials..");
                    row.Add("id", null);
                }
                return serializer.Serialize(row);                  
            }
            catch (Exception e)
            {
                row.Add("Status", "Faliure");
                row.Add("Message", e.Message);
            }
            //  row.Add("Status", "Success");
            return serializer.Serialize(row);
        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getList()
        {
            cls_Carriers obj_carrier = new cls_Carriers();
            DataSet ds = new DataSet();
            ds = obj_carrier.fn_CarrierLastLoc_Fetch(1, 0, 0);

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            var jsonString = serializer.Serialize(rows);
            return jsonString;          
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object getLocation(int vehicleId)
        {
            cls_Carriers obj_carrier = new cls_Carriers();
            DataSet ds = new DataSet();
            DataTable carrierdt = new DataTable();
            carrierdt.Columns.Add("carrierId");
            carrierdt.Rows.Add(vehicleId);
            ds = obj_carrier.fn_fetchLastLocation(carrierdt, null);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            var jsonString = serializer.Serialize(rows);
            return jsonString;  
        }
        
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object uploadPic()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            try
            {               
                int carrierId = int.Parse(HttpContext.Current.Request.Form["carrierID"]);
                string carrierName = HttpContext.Current.Request.Form["carrierName"].ToString();
                string mobile = HttpContext.Current.Request.Form["mobile"].ToString();
                HttpPostedFile file = HttpContext.Current.Request.Files["photo1"];
                string serverpath = Server.MapPath("Photos");
                string targetFilePath = serverpath + "\\" + file.FileName;
                file.SaveAs(targetFilePath);
                string imagePath = file.FileName;

                sendMessage(file.FileName, mobile, carrierName, file.FileName);

                file = HttpContext.Current.Request.Files["photo2"];
                serverpath = Server.MapPath("Photos");
                targetFilePath = serverpath + "\\" + file.FileName;
                file.SaveAs(targetFilePath);


                sendMessage(file.FileName, mobile, carrierName, file.FileName);

                row.Add("Status", "Success");                
                return serializer.Serialize(row);

            }
            catch (Exception e)
            {
                row = new Dictionary<string, object>();
                row.Add("Status", "Failure");
                row.Add("Message", e.Message);
                return serializer.Serialize(row);
            }
        }

        void sendMessage(string path, string number, string carrierName,string message)
        {
            string sUser = "atulchikane";
            string spwd = "8336320";
            string sNumber = number;
            string sMessage = "Panic button pressed for" + carrierName + ": http://209.190.31.226/tracking/photos/" + message;
            string sSenderID = "WEBSMS";
            string sURL = string.Format("http://login.smsgatewayhub.com/smsapi/pushsms.aspx?user={0}&pwd={1} &to={2}&sid={3}&msg={4}&fl=0", sUser, spwd, sNumber, sSenderID, sMessage);

            // "http://login.smsgatewayhub.com/smsapi/pushsms.aspx?user=" + sUser + "&pwd=" + spwd + "&to=" + sNumber + "&sid=" + sSenderID + "&msg=" + sMessage + "&fl=0?";
            string sResponse = GetResponse(sURL);
        }
        
        public static string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest
            .Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch
            {
                return "";
            }
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RegisterGCM(int carrierId, string GCMId,string imei)
        {
            SqlCommand cmd = new SqlCommand("updateGCMId", Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@carrierId", SqlDbType.Int).Value = carrierId;
            cmd.Parameters.Add("@GCMId", SqlDbType.NVarChar).Value = GCMId;
            cmd.Parameters.Add("@imei", SqlDbType.NVarChar).Value = imei;

            Connection.Open();
            int response = int.Parse(cmd.ExecuteScalar().ToString());
            Connection.Close();
            return showMessage(response);
        }



        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string RegisterGCMPolice(int carrierId, string GCMId)
        //{
        //    SqlCommand cmd = new SqlCommand("updateGCMIdPolice", Connection);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("@carrierId", SqlDbType.Int).Value = carrierId;
        //    cmd.Parameters.Add("@GCMId", SqlDbType.NVarChar).Value = GCMId;

        //    Connection.Open();
        //    int response = int.Parse(cmd.ExecuteScalar().ToString());
        //    Connection.Close();
        //    return showMessage(response);
        //}

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string uploadPhotos()
        //{
        //    Dictionary<string, object> row = new Dictionary<string, object>();
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    try
        //    {
        //        string sos;
        //        sos = HttpContext.Current.Request.Form["SOS"];
        //        int carrierId = int.Parse(HttpContext.Current.Request.Form["carrierId"]);
              
        //        SqlCommand cmd = new SqlCommand("uploadPhotos", Connection);
        //        cmd.CommandType = CommandType.StoredProcedure;
              
        //        HttpPostedFile file = null;
        //        string imagePath = null;
        //        try
        //        {
        //            file = HttpContext.Current.Request.Files["photo1"];
        //            string serverpath = Server.MapPath("Photos");
        //            string targetFilePath = serverpath + "\\" + file.FileName;
        //            file.SaveAs(targetFilePath);
        //            imagePath = file.FileName;
        //        }
        //        catch { }
        //        cmd.Parameters.Add("@image", SqlDbType.VarChar).Value = file == null ? null : file.FileName;

        //        file = null;
        //        string audioPath = null;
        //        try
        //        {
        //            file = HttpContext.Current.Request.Files["photo2"];
        //            string serverpath = Server.MapPath("Photos");
        //            string targetFilePath = serverpath + "\\" + file.FileName;
        //            file.SaveAs(targetFilePath);
        //            audioPath = file.FileName;
        //        }
        //        catch { }
        //        cmd.Parameters.Add("@audio", SqlDbType.VarChar).Value = file == null ? null : file.FileName;

               
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //        dap.Fill(ds);
        //        row.Add("Status", "Success");


        //        string tickerText = "problemNotification";
        //        string contentTitle = ds.Tables[0].Rows[0]["name"] + " wrote a problem.";

        //        IList<string> GCMIDS = new List<string>();
        //        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)// for voters table to get gcm ids
        //        {
        //            GCMIDS.Add(ds.Tables[1].Rows[i]["GCMId"].ToString());
        //        }


        //        Problem prob = new Problem
        //        {
        //            data = new Data
        //            {
        //                json = new JSONData
        //                {
        //                    tickerText = tickerText,
        //                    contentTitle = contentTitle,
        //                    problem = problem,
        //                    patientName = ds.Tables[0].Rows[0]["name"].ToString(),
        //                    audio = audioPath,
        //                    patientId = int.Parse(ds.Tables[0].Rows[0]["id"].ToString()),
        //                    problemId = patientFId,
        //                    video = videoPath,
        //                    image = imagePath
        //                }
        //            },
        //            registration_ids = GCMIDS
        //        };

        //        string json = JsonConvert.SerializeObject(prob, Formatting.Indented);

        //        //GCM.SendGCMNotification("AIzaSyC3pJwjCr93IO7Oe2E0ySrHCYZnFDbH-lk", json);
        //        row.Add("GcmStatus", GCM.SendGCMNotification("AIzaSyC3pJwjCr93IO7Oe2E0ySrHCYZnFDbH-lk", json));
        //        return serializer.Serialize(row);

        //    }
        //    catch (Exception e)
        //    {
        //        row = new Dictionary<string, object>();
        //        row.Add("Status", "Failure");
        //        row.Add("Message", e.Message);
        //        return serializer.Serialize(row);
        //    }
        //}


        public string showMessage(int i)
        {
            if (i > 0)
            {
                return "{\"Result\":\"Success\"}";
            }
            else
            {
                return "{\"Result\":\"Failure\"}";
            }
        }        
    }
}
