using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elaunch_POS_Repository.DataServices
{
    public static class CreatePara
    {
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, string paraVal, int size = 50, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.Size = size;
            para.SqlDbType = System.Data.SqlDbType.NVarChar;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, int paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Int;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, decimal paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Decimal;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, float paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Float;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, DateTime paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.IsNullable = true;
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.DateTime;
            para.Direction = dir;
            return para;
        }
        public static SqlParameter CreateParameter(this SqlParameter para, string paraName, System.Data.DataTable paraVal, ParameterDirection dir = ParameterDirection.Input)
        {
            para.ParameterName = paraName;
            para.Value = paraVal;
            para.SqlDbType = System.Data.SqlDbType.Structured;
            para.Direction = dir;
            return para;
        }
    }
}
