using Elaunch_POS_Repository.Data;
using Elaunch_POS_Repository.DataServices;
using Elaunch_POS_Repository.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Elaunch_POS_Repository.Service
{
    public class Cities_Repository : ICities_Repository, IDisposable
    {
        private elaunch_posEntities context;

        public Cities_Repository()
        {
            context = new elaunch_posEntities();
        }

        public List<City> Get()
        {
            try
            {
                return new dalc().selectbyqueryList<City>("select * from Cities with(nolock)");
            }
            catch (Exception ex)
            {
                ex.SetLog("Get city ,Repository");
                throw;
            }
        }

        public City GetById(int id)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter().CreateParameter("@id", id);
                return new dalc().GetList_Text<City>("select * from Cities  with(nolock) where City_ID = @id", para).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.SetLog("Get city by ID,Repository");
                throw;
            }
        }

        public void Insert(City objCity)
        {
            try
            {
                context.Cities.Add(objCity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            //catch (DbEntityValidationException ex)
            //{
            //    List<string> errorMessages = new List<string>();
            //    foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
            //    {
            //        string entityName = validationResult.Entry.Entity.GetType().Name;
            //        foreach (DbValidationError error in validationResult.ValidationErrors)
            //        {
            //            errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
            //        }
            //    }
            //}
        }

        public void Update(City objCity)
        {
            try
            {
                context.Entry(objCity).State = System.Data.Entity.EntityState.Modified;
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
                City objCity = context.Cities.Where(z => z.City_ID == id).FirstOrDefault();
                context.Cities.Remove(objCity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsExist(string cityName, int id)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];
                para[0] = new SqlParameter().CreateParameter("@City_Name", cityName);
                para[1] = new SqlParameter().CreateParameter("@City_ID", id);
                DataTable dt = new dalc().GetDataTable_Text("SELECT * FROM Cities with(nolock) WHERE  City_ID<>@City_ID AND  RTRIM(LTRIM(City_Name))= RTRIM(LTRIM(@City_Name))", para);
                return dt.Rows.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                ex.SetLog("IS Exist Cities,Repository");
                throw;
            }
        }

        public List<SelectListItem> GetCityListDropdownData()
        {
            var categorylist = Get();
            List<SelectListItem> lstCity = new List<SelectListItem>();
            // lstCity.Add(new SelectListItem { Text = "--Select City--", Value = "0" });
            foreach (var m in categorylist)
            {
                lstCity.Add(new SelectListItem { Text = m.City_Name, Value = m.City_ID.ToString() });
            }
            return lstCity.ToList();
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
