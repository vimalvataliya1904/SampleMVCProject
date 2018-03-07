using Elaunch_POS.Areas.Admin.Controllers;
using Elaunch_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Elaunch_POS_Repository.Data;
using Elaunch_POS_Repository.ServiceContract;
using Elaunch_POS_Repository.Service;

namespace Elaunch_POS
{
    public partial class ExportData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!string.IsNullOrEmpty(Request.Form["mode"]))
                {
                    try
                    {
                        string mode = Convert.ToString(Request.Form["mode"]);
                        string where = "";
                        GridData og = new GridData(mode, where, true);
                    }
                    catch (Exception ex)
                    {
                        //ex.SetLog("For Export Data");
                    }
                    finally
                    {
                        Response.Write("<script>window.parent.document.getElementsByClassName('dataTables_processing')[0].style.display='none';</script>");
                    }
                }
            }
        }
    }
}