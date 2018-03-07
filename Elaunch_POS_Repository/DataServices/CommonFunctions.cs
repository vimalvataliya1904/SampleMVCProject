using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI;
using System.Reflection;
using System.Transactions;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

namespace Elaunch_POS_Repository.DataServices
{

    public static class CommonFunctions
    {
        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                //T item = GetItem<T>(row);
                //data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(SqlDataReader dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == "barcode")
                    {
                        var abc = "";
                    }
                    if (pro.Name.ToLower() == dr.GetName(i).ToLower())
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[i])))
                        {
                            if (pro.PropertyType.Name == "String")
                                pro.SetValue(obj, Convert.ToString(dr[i]));
                            else if (pro.PropertyType.Name == "Byte[]" && string.IsNullOrEmpty(Convert.ToString(dr[i])))
                                pro.SetValue(obj, new byte[0]);
                            else
                                pro.SetValue(obj, dr[i]);
                        }
                        break;
                    }
                }
            }
            return obj;
        }


        public static void ExportToExcel(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;
            //GridView1.Style.Add("word-wrap", "break-word");
            //GridView1.Style.Add("width", "100");
            if (string.IsNullOrEmpty(FileName))
                FileName = "Excel";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
             "attachment;filename=" + FileName + ".xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            //TextBox txtAddress = new TextBox();
            //txtAddress.ReadOnly = false;
            //txtAddress.Style.Add("width", "99%");
            foreach (GridViewRow row in GridView1.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Style.Add("class", "textmode");
                    //txtAddress.Style = "width:100%;";
                }
            }
            //GridView1.Attributes.Add("style", "table-layout:fixed");
            //for (int i = 0; i < GridView1.Rows.Count; i++)
            //{
            //    //Apply text style to each Row
            //    GridView1.Rows[i].Attributes.Add("class", "textmode");
            //}
            GridView1.RenderControl(hw);
            //string style = @"<!--mce:2-->"; 
            //HttpContext.Current.Response.Write(style);
            //Open a memory stream that you can use to write back to the response
            byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
            MemoryStream s = new MemoryStream(byteArray);
            StreamReader sr = new StreamReader(s, Encoding.ASCII);
            HttpContext.Current.Response.Write(sr.ReadToEnd());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToWord(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;

            if (string.IsNullOrEmpty(FileName))
                FileName = "Word";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".doc");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToPdf(this DataTable dt, string FileName)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            //GridView1.HeaderStyle.BackColor = System.Drawing.Color.Black;
            //GridView1.HeaderStyle.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderStyle.Font.Bold = true;
            GridView1.HeaderStyle.Font.Size = 12;
            GridView1.Font.Size = 10;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.HeaderRow.BackColor = System.Drawing.Color.Blue;
            GridView1.HeaderRow.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderRow.Font.Bold = true;
            GridView1.Style.Add("width", "100");
            if (string.IsNullOrEmpty(FileName))
                FileName = "Pdf";
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".pdf");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            HttpContext.Current.Response.Write(pdfDoc);
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void ExportToCsv(this DataTable dt, string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                FileName = "csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition",
                "attachment;filename=" + FileName + ".csv");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/text";


            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            HttpContext.Current.Response.Output.Write(sb.ToString());
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void SetLog(this Exception ex, string msg, Boolean IsRedirect = true)
        {
            if (ex is System.Data.Entity.Validation.DbEntityValidationException)
            {
                foreach (var validationErrors in ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += ";" + string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                    }
                }
            }
            string FileName = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
            if (!File.Exists(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
            {
                using (StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
                {
                    // StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt"), true);
                    sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg:" + msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
                {
                    sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg: " + msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            if (IsRedirect)
                throw ex;
        }

        public static string GetErrorMsg(this Exception ex)
        {
            if (!string.IsNullOrEmpty(ex.Message))
            {
                if (ex.Message.Length > 100)
                {
                    return ex.Message.Substring(0, 100).ToString() + "...";
                }
                else
                {
                    return ex.Message.ToString();
                }
            }
            return null;
        }

        public static string RandomString(int Size)
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size).Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }
        public static void SendEmailWithContract(string ToEmail, string MailSubject, string MailBody)
        {
            try
            {
                var fromAddress = new MailAddress(ConfigurationManager.AppSettings["SMTPEmail"].ToString());
                var fromPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
                var toAddress = new MailAddress(ToEmail);

                string subject = MailSubject;
                string body = MailBody;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPEmail"].ToString(), fromPassword);
                //Timeout = 10000

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath("~/ContactDoc/contract.pdf"));
                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.CC.Add(ConfigurationManager.AppSettings["SMTPCCEmail"].ToString());
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;
                message.Attachments.Add(attachment);
                smtp.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        public static void SendEmail(string ToEmail, string MailSubject, string MailBody)
        {
            try
            {
                var fromAddress = new MailAddress(ConfigurationManager.AppSettings["SMTPEmail"].ToString());
                var fromPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
                var toAddress = new MailAddress(ToEmail);

                string subject = MailSubject;
                string body = MailBody;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPEmail"].ToString(), fromPassword);
                //Timeout = 10000

                MailMessage message = new MailMessage(fromAddress, toAddress);
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;
                smtp.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
        //Encode
        public static string Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception)
            {
                throw;
            }

        }
        //Decode
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string ConvertToJSON(this DataTable table, Boolean IsSkipTotalRow = true)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    if (IsSkipTotalRow && col.ColumnName.ToLower() != "totalrows")
                        dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(list);
        }

        public static string GetJsonForDataTableJS(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string data = dt.ConvertToJSON();
            sb.AppendLine("{\"data\":" + data);
            sb.Append(",\"draw\":\"" + Convert.ToString(HttpContext.Current.Request.Form["draw"]) + "\"");
            sb.Append(",\"recordsFiltered\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"");
            sb.Append(",\"recordsTotal\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"}");
            return sb.ToString();
        }

        public static string SendSMS(string mobile, string msg)
        {
            // string returnValue = "";
            try
            {
                string username = Convert.ToString(ConfigurationManager.AppSettings["SMS_UserName"]);
                var password = Convert.ToString(ConfigurationManager.AppSettings["SMS_Password"]);
                var sender = Convert.ToString(ConfigurationManager.AppSettings["SMS_Sender"]);
                var domain = Convert.ToString(ConfigurationManager.AppSettings["SMS_Domain"]);
                if (!mobile.StartsWith("966"))
                    mobile = "966" + mobile;
                string url = domain + "api/sendsms.php?username=" + username + "&password=" + password + "&message=" + msg + "&numbers=" + mobile + "&sender=" + sender + "&return=string";

                StreamReader strReader;
                WebRequest webReq = WebRequest.Create(url);
                WebResponse webRes = webReq.GetResponse();

                strReader = new StreamReader(webRes.GetResponseStream());

                return strReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                ex.SetLog("Send SMS Failed TO (" + mobile + ")");
                throw ex;
            }
            //if (!string.IsNullOrEmpty(returnValue))
            //{
            //    if (returnValue == "تم استلام الارقام بنجاح")
            //    {
            //        LblMessage.Visible = true;
            //        LblMessage.Text = returnValue;
            //    }

            //}
        }
    }
}
