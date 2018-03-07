using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elaunch_POS_Repository.Data
{
    [MetadataType(typeof(CityMetaData))]
    public partial class City
    {
    }
    internal sealed class CityMetaData
    {
        [Required(ErrorMessage = "City Name can't be blank.")]
        public string City_Name { get; set; }
    }
}
