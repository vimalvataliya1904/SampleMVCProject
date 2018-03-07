using Elaunch_POS_Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Elaunch_POS_Repository.ServiceContract
{
    public interface IBanks_Repository : IDisposable
    {
        List<Bank> Get();
        Bank GetById(int id);
        void Insert(Bank objBank);
        void Update(Bank objBank);
        void Delete(int id);
        Boolean IsExist(string bankName, int id);
        List<SelectListItem> GetBankListDropDown();
    }
}
