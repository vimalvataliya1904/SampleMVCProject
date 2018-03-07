using Elaunch_POS_Repository.Data;
using Elaunch_POS_Repository.DataServices;
using Elaunch_POS_Repository.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Elaunch_POS_Repository.Service
{
    public class Banks_Repository : IBanks_Repository, IDisposable
    {
        private elaunch_posEntities context;
        public Banks_Repository()
        {
            context = new elaunch_posEntities();
        }
        public List<Bank> Get()
        {
            try
            {
                return new dalc().selectbyqueryList<Bank>("select * from Banks with(nolock)");
            }
            catch (Exception ex)
            {
                ex.SetLog("Get bank ,Repository");
                throw;
            }
        }
        public Bank GetById(int id)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter().CreateParameter("@id", id);
                return new dalc().GetList_Text<Bank>("select * from Banks  with(nolock) where Bank_ID = @id", para).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.SetLog("Get Bank by ID,Repository");
                throw;
            }
        }
        public void Insert(Bank objBank)
        {
            try
            {
                context.Banks.Add(objBank);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public void Update(Bank objBank)
        {
            try
            {
                context.Entry(objBank).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.SetLog("Update City information");
                throw;
            }
        }
        public void Delete(int id)
        {
            try
            {
                Bank objBank = context.Banks.Where(z => z.Bank_ID == id).FirstOrDefault();
                context.Banks.Remove(objBank);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Boolean IsExist(string bankName, int id)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter().CreateParameter("@Bank_Name", bankName);
                para[1] = new SqlParameter().CreateParameter("@Bank_ID", id);
                DataTable dt = new dalc().GetDataTable_Text("SELECT * FROM Banks with(nolock) WHERE  Bank_ID<>@Bank_ID AND  RTRIM(LTRIM(Bank_Name))= RTRIM(LTRIM(@Bank_Name))", para);
                return dt.Rows.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                ex.SetLog("IS Exist Bank,Repository");
                throw;
            }
        }
        public List<SelectListItem> GetBankListDropDown()
        {
            var categorylist = Get();
            List<SelectListItem> lstBankType = new List<SelectListItem>();
            //  lstBankType.Add(new SelectListItem { Text = "--Select Bank--", Value = "0" });
            foreach (var m in categorylist)
            {
                lstBankType.Add(new SelectListItem { Text = m.Bank_Name, Value = m.Bank_ID.ToString() });
            }
            return lstBankType.ToList();
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
