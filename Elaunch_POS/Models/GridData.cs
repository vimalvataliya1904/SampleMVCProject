using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elaunch_POS.Models
{
    public class GridData
    {
        public string TableName { get; set; }
        public string ColumnsName { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int RecordPerPage { get; set; }
        public string WhereClause { get; set; }
        public string JsonData { get; set; }
        public string ExportedColumns { get; set; }
        public string ExportedFileName { get; set; }
        public GridData(string type, string whereClause = "", Boolean IsExport = false)
        {
            if (type == "CitiMaster")
            {
                this.ColumnsName = "City_ID,City_Name";
                this.PageNumber = 1;
                this.RecordPerPage = 10;
                this.SortColumn = "City_ID";
                this.SortOrder = "desc";
                this.TableName = "Cities";
                this.WhereClause = whereClause;
                this.ExportedFileName = "City Information";
                this.ExportedColumns = "City_ID[Hidden],City_Name";
                GridFunctions oGrid = new GridFunctions();
                if (!IsExport)
                    this.JsonData = oGrid.GetJson(this);
                else
                    oGrid.Export(this);
            }           
            else if (type == "BankMaster")
            {
                this.ColumnsName = "Bank_ID,Bank_Name";
                this.PageNumber = 1;
                this.RecordPerPage = 10;
                this.SortColumn = "Bank_ID";
                this.SortOrder = "desc";
                this.TableName = "Banks";
                this.WhereClause = whereClause;
                this.ExportedFileName = "BanksList";
                this.ExportedColumns = "Bank_ID[hidden],Bank_Name";
                GridFunctions oGrid = new GridFunctions();
                if (!IsExport)
                    this.JsonData = oGrid.GetJson(this);
                else
                    oGrid.Export(this);
            }
        }
    }
}