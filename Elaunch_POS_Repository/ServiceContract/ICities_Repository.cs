using Elaunch_POS_Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Elaunch_POS_Repository.ServiceContract
{
    public interface ICities_Repository : IDisposable
    {
        List<City> Get();
        City GetById(int id);
        void Insert(City objCity);
        void Update(City objCity);
        void Delete(int id);
        Boolean IsExist(string cityName, int id);
        List<SelectListItem> GetCityListDropdownData();
    }
}
