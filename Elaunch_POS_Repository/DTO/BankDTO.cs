using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Elaunch_POS_Repository.Data
{

    [MetadataType(typeof(BanksMetaData))]
    public partial class Bank
    {
    }

    internal sealed class BanksMetaData
    {
        [Required(ErrorMessage = "Bank Name Required.")]
        public string Bank_Name { get; set; }
    }  
}
