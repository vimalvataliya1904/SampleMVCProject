using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using Elaunch_POS_Repository.DataServices;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1.Data;

namespace Elaunch_POS.Models
{
    public class Common
    {
        dalc odal = new dalc();
        public DataTable GetAutoNumber(string type, string code = "", int codeId = 0)
        {
            try
            {
                return odal.selectbyquerydt("select [dbo].[GetAutoNumber]('" + type + "','" + code + "','" + codeId + "')");
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public string NewEncrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public string NewDecrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        //Define the tripple Des Provider
        public TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();

        //Define the string Handler
        public UTF8Encoding m_utf8 = new UTF8Encoding();

        //Define the local Propertirs Array
        public byte[] m_key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 13, 15, 16, 17, 18, 19, 20, 21, 22, 24, 23 };
        public byte[] m_iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

        public string Encypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }
        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string RandomString(int Size)
        {
            Random random = new Random();
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            var chars = Enumerable.Range(0, Size)
                                   .Select(x => input[random.Next(0, input.Length)]);
            return new string(chars.ToArray());
        }

        private byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            //Create the Neccessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write);
            //transform the bytes as requested
            cryptoStream.Write(input, 0, input.Length);
            cryptoStream.FlushFinalBlock();
            //Reasd the memory stream and convert it to byte array
            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));
            memStream.Close();
            cryptoStream.Close();
            return result;
        }


        public string GenerateTinyURL(string longUrl)
        {
            try
            {
                UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
                {
                    ApiKey = System.Configuration.ConfigurationManager.AppSettings["GOOGLE_SHORTNER_API_KEY"],// "AIzaSyAvTLqX1YikMNBEP921NNidYssUJmkVJdI",
                    ApplicationName = "Top100Pos",
                });
                var response = service.Url.Insert(new Url { LongUrl = longUrl }).Execute();
                return response.Id;
            }
            catch (Exception ex)
            {
                return "";
            }
        }


        //public static int LoggedInUserId { get { return Convert.ToString(HttpContext.Current.Session["UserId"]).GetProperInt(); } }

        //public static int JobSeekerId { get { return Convert.ToString(HttpContext.Current.Session["JobSeekerid"]).GetProperInt(); } }


        //public static string GetFileName(string type, int id, string ext)
        //{
        //    string filename = "";
        //    if (type.ToLower() == "signature")
        //    {
        //        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Signature")))
        //            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Signature"));

        //        filename = id.ToString() + "_Signature_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
        //        for (int i = 0; ; i++)
        //        {
        //            if (!File.Exists(HttpContext.Current.Server.MapPath("~/Signature/" + filename + "." + ext)))
        //                break;
        //            else
        //                filename = id.ToString() + "_Signature_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + "_" + i.ToString();
        //        }
        //        filename = "/Signature/" + filename;
        //    }
        //    return filename + "." + ext;
        //}

        //public static int GetProperInt(this string str)
        //{
        //    try
        //    {
        //        return Convert.ToInt32(str);
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}

        //public static string fileupload(HttpPostedFileBase fu, string foldername, string filename)
        //{


        //    if (fu.FileName != "")
        //    {
        //        string filename1 = Path.GetFileNameWithoutExtension(fu.FileName);
        //        string ext = Path.GetExtension(fu.FileName);
        //        if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".gif" || ext == ".JPG" || ext == ".JPEG" || ext == ".PNG" || ext == ".GIF")
        //        {
        //            Stream strm = fu.InputStream;
        //            try
        //            {
        //                filename = "~/" + foldername + "/" + filename;
        //                System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
        //                img.Save(System.Web.HttpContext.Current.Server.MapPath(filename));
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }
        //            finally
        //            {
        //                strm.Flush();
        //                strm.Close();
        //            }
        //        }
        //    }
        //    return filename;
        //}


        //public static List<T> ConvertToList<T>(this DataTable dt)
        //{
        //    List<T> data = new List<T>();
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        T item = GetItem<T>(row);
        //        data.Add(item);
        //    }
        //    return data;
        //}
        //public static T GetItem<T>(DataRow dr)
        //{
        //    Type temp = typeof(T);
        //    T obj = Activator.CreateInstance<T>();

        //    foreach (DataColumn column in dr.Table.Columns)
        //    {
        //        foreach (PropertyInfo pro in temp.GetProperties())
        //        {
        //            if (pro.Name == column.ColumnName)
        //            {
        //                if (!string.IsNullOrEmpty(Convert.ToString(dr[column.ColumnName])))
        //                    pro.SetValue(obj, dr[column.ColumnName]);
        //            }
        //            else
        //                continue;
        //        }
        //    }
        //    return obj;
        //}

        //public static void ExportToExcel(this DataTable dt, string FileName)
        //{
        //    GridView GridView1 = new GridView();
        //    GridView1.AllowPaging = false;
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //    if (string.IsNullOrEmpty(FileName))
        //        FileName = "Excel";
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.AddHeader("content-disposition",
        //     "attachment;filename=" + FileName + ".xls");
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    //for (int i = 0; i < GridView1.Rows.Count; i++)
        //    //{
        //    //    //Apply text style to each Row
        //    //    GridView1.Rows[i].Attributes.Add("class", "textmode");
        //    //}
        //    GridView1.RenderControl(hw);
        //    //Open a memory stream that you can use to write back to the response
        //    byte[] byteArray = Encoding.ASCII.GetBytes(sw.ToString());
        //    MemoryStream s = new MemoryStream(byteArray);
        //    StreamReader sr = new StreamReader(s, Encoding.ASCII);
        //    HttpContext.Current.Response.Write(sr.ReadToEnd());
        //    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //public static void ExportToWord(this DataTable dt, string FileName)
        //{
        //    GridView GridView1 = new GridView();
        //    GridView1.AllowPaging = false;
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //    if (string.IsNullOrEmpty(FileName))
        //        FileName = "Word";
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.AddHeader("content-disposition",
        //        "attachment;filename=" + FileName + ".doc");
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    GridView1.RenderControl(hw);
        //    HttpContext.Current.Response.Output.Write(sw.ToString());
        //    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //public static void ExportToPdf(this DataTable dt, string FileName)
        //{
        //    GridView GridView1 = new GridView();
        //    GridView1.AllowPaging = false;
        //    GridView1.HeaderStyle.BackColor = System.Drawing.Color.Black;
        //    GridView1.HeaderStyle.ForeColor = System.Drawing.Color.White;
        //    GridView1.HeaderStyle.Font.Size = 12;
        //    GridView1.HeaderStyle.Font.Bold = true;
        //    GridView1.Font.Size = 10;
        //    GridView1.DataSource = dt;
        //    GridView1.DataBind();
        //    if (string.IsNullOrEmpty(FileName))
        //        FileName = "Pdf";
        //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    HttpContext.Current.Response.AddHeader("content-disposition",
        //        "attachment;filename=" + FileName + ".pdf");
        //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    GridView1.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    HttpContext.Current.Response.Write(pdfDoc);
        //    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //public static void ExportToCsv(this DataTable dt, string FileName)
        //{
        //    if (string.IsNullOrEmpty(FileName))
        //        FileName = "csv";
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Buffer = true;
        //    HttpContext.Current.Response.AddHeader("content-disposition",
        //        "attachment;filename=" + FileName + ".csv");
        //    HttpContext.Current.Response.Charset = "";
        //    HttpContext.Current.Response.ContentType = "application/text";


        //    StringBuilder sb = new StringBuilder();
        //    for (int k = 0; k < dt.Columns.Count; k++)
        //    {
        //        //add separator
        //        sb.Append(dt.Columns[k].ColumnName + ',');
        //    }
        //    //append new line
        //    sb.Append("\r\n");
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int k = 0; k < dt.Columns.Count; k++)
        //        {
        //            //add separator
        //            sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
        //        }
        //        //append new line
        //        sb.Append("\r\n");
        //    }
        //    HttpContext.Current.Response.Output.Write(sb.ToString());
        //    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        //    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}

        //public static string ConvertToJSON(this DataTable table, Boolean IsSkipTotalRow = true)
        //{
        //    var list = new List<Dictionary<string, object>>();

        //    foreach (DataRow row in table.Rows)
        //    {
        //        var dict = new Dictionary<string, object>();

        //        foreach (DataColumn col in table.Columns)
        //        {
        //            if (IsSkipTotalRow && col.ColumnName.ToLower() != "totalrows")
        //                dict[col.ColumnName] = row[col];
        //        }
        //        list.Add(dict);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    return serializer.Serialize(list);
        //    //var JSONString = new StringBuilder();
        //    //if (table.Rows.Count > 0)
        //    //{
        //    //    JSONString.Append("[");
        //    //    for (int i = 0; i < table.Rows.Count; i++)
        //    //    {
        //    //        JSONString.Append("{");
        //    //        for (int j = 0; j < table.Columns.Count; j++)
        //    //        {
        //    //            if (table.Columns[j].ColumnName.ToLower() != "totalrows")
        //    //                if (j < table.Columns.Count - 1)
        //    //                {
        //    //                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
        //    //                }
        //    //                else if (j == table.Columns.Count - 1)
        //    //                {
        //    //                    JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
        //    //                }
        //    //        }
        //    //        if (i == table.Rows.Count - 1)
        //    //        {
        //    //            JSONString.Append("}");
        //    //        }
        //    //        else
        //    //        {
        //    //            JSONString.Append("},");
        //    //        }
        //    //    }
        //    //    JSONString.Append("]");
        //    //}
        //    //return JSONString.ToString();
        //}

        //public static string GetJsonForDataTableJS(this DataTable dt)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string data = dt.ConvertToJSON();
        //    sb.AppendLine("{\"data\":" + data);
        //    sb.Append(",\"draw\":\"" + Convert.ToString(HttpContext.Current.Request.Form["draw"]) + "\"");
        //    sb.Append(",\"recordsFiltered\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"");
        //    sb.Append(",\"recordsTotal\":\"" + (dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()) + "\"}");
        //    return sb.ToString();
        //}

        //#region Maths Answere
        //public static int A1 = 116;
        //public static int B1 = 111;
        //public static int A2 = 16;
        //public static int B2 = 19;
        //public static int A3 = 439;
        //public static int B3 = 30488;
        //public static int C3 = 632;
        //public static int D3 = 821;
        //public static int E3 = 15;
        //public static int F3 = 1163;
        //public static string A4 = "295729AD";
        //public static string B4 = "35TGH789KUY";
        //public static string C4 = "48GH678URV";

        //#endregion
    }


    //public static class Utilities
    //{
    //    public static void SetLog(this Exception ex, string msg, Boolean IsRedirect = false)
    //    {
    //        if (ex is System.Data.Entity.Validation.DbEntityValidationException)
    //        {
    //            foreach (var validationErrors in ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors)
    //            {
    //                foreach (var validationError in validationErrors.ValidationErrors)
    //                {
    //                    msg += ";" + string.Format("{0}:{1}",
    //                        validationErrors.Entry.Entity.ToString(),
    //                        validationError.ErrorMessage);
    //                }
    //            }
    //        }
    //        string FileName = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
    //        if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Logs")))
    //            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
    //        if (!File.Exists(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
    //        {
    //            using (StreamWriter sw = File.CreateText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
    //            {
    //                sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg:" + msg);
    //                sw.Flush();
    //                sw.Close();
    //            }
    //        }
    //        else
    //        {
    //            using (StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath("~/Logs/" + FileName + ".txt")))
    //            {
    //                sw.WriteLine("Error on " + DateTime.Now + " ,Exception Message:" + ex.Message + ",Inner Message:" + ex.InnerException + ",Line:" + ex.StackTrace + ",Additional Msg: " + msg);
    //                sw.Flush();
    //                sw.Close();
    //            }
    //        }
    //        if (IsRedirect)
    //            throw ex;
    //    }


    //}
}