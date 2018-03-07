using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

namespace Elaunch_POS_Repository.DataServices
{

    public class dalc
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Dalc_Conn"].ConnectionString);
        // SqlConnection conn;

        public dalc()
        {
            //conn.ConnectionString = ConfigurationSettings.AppSettings["myconn"];
            // conn.ConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        }

        public DataSet selectbyquery(string str)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandText = str.ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        public List<T> selectbyqueryList<T>(string str)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str.ToString();
            SqlDataReader dr;
            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                List<T> lst = new List<T>();
                while (dr.Read())
                {
                    T item = CommonFunctions.GetItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }
        public DataTable selectbyquerydt(string str)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str.ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public SqlCommand GetCommand()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public DataSet GetDataset(string Spname, SqlParameter[] para)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = GetCommand();
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Spname.ToString();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        public DataTable GetDataTable(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        public List<T> GetList<T>(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataReader dr;
            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                List<T> lst = new List<T>();
                while (dr.Read())
                {
                    T item = CommonFunctions.GetItem<T>(dr);
                    lst.Add(item);
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public DataTable GetDataTable_Text(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                conn.Open();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public List<T> GetList_Text<T>(string Query, SqlParameter[] para)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddRange(para);
            cmd.CommandText = Query.ToString();
            SqlDataReader dr;
            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                List<T> lst = new List<T>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        T item = CommonFunctions.GetItem<T>(dr);
                        lst.Add(item);
                    }
                }
                return lst;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                // conn.Dispose();
            }
        }

        public void IUD(string SPName, SqlParameter[] para)
        {
            SqlCommand cmd = GetCommand();
            cmd.CommandText = SPName.ToString();
            cmd.Parameters.AddRange(para);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        public void updatedata(string str)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str;
            //cmd.Parameters.AddRange(para);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
    }
}
