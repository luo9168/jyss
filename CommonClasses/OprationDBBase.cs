using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using MyWebControlLib;
using System.Data.OleDb;
namespace CommonClasses
{

    public enum ConditionLinkOperater { And, Or };
    public class OprationDBBase
    {

        //子类必须重写_tablename 和 _viewname的值
        protected string _tablename = "";
        protected string _viewname = "";
        protected string _columnNames = " * ";
        protected string _conditionOperater = " and ";
        protected bool _findthenupdate = true;
        private SqlConnection conn;
        protected string _connectionStr = "";
        public string log_moduleName = "";
        public string log_userName = "";
        public OprationDBBase()
        {
            _connectionStr = System.Configuration.ConfigurationSettings.AppSettings.Get("ConnectionString").ToString();
            conn = new SqlConnection(_connectionStr);
        }
       
        public void SetViewName(string viewName)
        {
            _viewname = viewName;
        }
        protected void SetConnection(string connectionStr)
        {
            _connectionStr = connectionStr;
            conn = new SqlConnection(_connectionStr);
        }
        public DataTable GetOptionNonBlank(string TableName)
        {
            if (TableName != "")
            {
                string sql = "select code,name from " + TableName;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetOptionWithBlank(string TableName)
        {
            if (TableName != "")
            {
                string sql = "select '' as code,'' as name union select code,name from " + TableName;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetOptionWithBlank(string TableName, string condition)
        {
            if (TableName != "")
            {
                string sql = "select '' as code,'' as name union select code,name from " + TableName + " where " + condition;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDistinctOptionWithBlank(string fieldname, string condition)
        {
            if (_tablename != "")
            {
                string sql = "select '' as " + fieldname + " union select " + fieldname + " from " + _tablename + " where " + condition;
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDistinctOptionWithBlank(string name, string code, string condition)
        {
            if (_tablename != "")
            {
                string sql = "select '' as code,'' as name union select " + code + "," + name + " from " + _tablename + " where " + condition + " and " + name + " <>'' ";
                // string sql = " select " + code + "as code ," + name + " as name from " + _tablename + " where " + condition;

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds.Tables[0];
            }
            else
                return null;
        }

        public DataTable GetOriginDataTable()
        {
            if (_tablename != "")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from " + _tablename, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetBlankOriginDataTable()
        {
            if (_tablename != "")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from " + _tablename + " where tableID=-1", conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTableBySql(String sql)
        {
            if(conn.State==ConnectionState.Closed)
                conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
            adapter.SelectCommand.CommandTimeout = 0;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            conn.Close();
            return dt;
        }
        public DataTable GetOriginTableByKeys(MyWebControlLib.FieldValues FVS)
        {
            if (_tablename != "")
            {
                string values = "";
                string condition = "(1=1)";
                for (int i = 0; i < FVS.Count; i++)
                {
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = _conditionOperater + FVS[i].FieldName + " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = _conditionOperater + FVS[i].FieldName + " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = _conditionOperater + FVS[i].FieldName + " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = _conditionOperater + FVS[i].FieldName + " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = _conditionOperater + FVS[i].FieldName + " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = _conditionOperater + FVS[i].FieldName + " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = _conditionOperater + FVS[i].FieldName + " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            values = "Order by " + FVS[i].FieldName + " " + FVS[i].orderMethod;
                            break;
                        default:
                            values = _conditionOperater + FVS[i].FieldName + " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + values;
                }

                SqlDataAdapter adapter = new SqlDataAdapter("select * from " + _tablename + " where " + condition, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                return ds.Tables[0];
            }
            else
                return null;
        }

        public DataTable GetDataTable()
        {
            if (_viewname != "")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;

        }
        public DataTable GetDataTable(string condition)
        {
            if (condition != "")
                condition = " where " + condition;
            if (_viewname != "")
            {
                string str = "select * from " + _viewname + condition;
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + condition, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;

        }
        public DataTable GetDataTable(MyWebControlLib.FieldValues FVS)
        {
            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = _conditionOperater + FVS[i].FieldName + " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = _conditionOperater + FVS[i].FieldName + " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = _conditionOperater + FVS[i].FieldName + " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = _conditionOperater + FVS[i].FieldName + " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = _conditionOperater + FVS[i].FieldName + " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = _conditionOperater + FVS[i].FieldName + " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = _conditionOperater + FVS[i].FieldName + " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = _conditionOperater + FVS[i].FieldName + " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + values;
                }
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                    {
                        orderlist.Add(FVS[i].FieldName);
                        if (FVS[i].orderMethod == OrderMenthod.ASC)
                            ordermethod.Add(" ASC ");
                        else
                            ordermethod.Add(" DESC ");
                    }
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }

                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where " + condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTable(MyWebControlLib.FieldValues FVS, MyWebControlLib.FieldValues FVS1)
        {
            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = _conditionOperater + FVS[i].FieldName + " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = _conditionOperater + FVS[i].FieldName + " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = _conditionOperater + FVS[i].FieldName + " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = _conditionOperater + FVS[i].FieldName + " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = _conditionOperater + FVS[i].FieldName + " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = _conditionOperater + FVS[i].FieldName + " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = _conditionOperater + FVS[i].FieldName + " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        case FVOperater.Is:
                            values = _conditionOperater + FVS[i].FieldName + " is " + FVS[i].Value + " ";
                            break;
                        default:
                            values = _conditionOperater + FVS[i].FieldName + " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + values;
                }
                string condition1 = "";
                for (int k = 0; k < FVS1.Count; k++)
                {
                    string values1 = "";
                    string conditionOR = " or ";
                    FVOperater OP = FVS1[k].FVOperater;
                    switch (OP)
                    {

                        case FVOperater.Great:
                            values1 = conditionOR + FVS1[k].FieldName + " > '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values1 = conditionOR + FVS1[k].FieldName + " < '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values1 = conditionOR + FVS1[k].FieldName + " <= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values1 = conditionOR + FVS1[k].FieldName + " >= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.In:
                            values1 = conditionOR + FVS1[k].FieldName + " in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values1 = conditionOR + FVS1[k].FieldName + " not in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values1 = conditionOR + FVS1[k].FieldName + " <> '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        case FVOperater.Is:
                            values1 = conditionOR + FVS1[k].FieldName + " is " + FVS1[k].Value + " ";
                            break;
                        default:
                            values1 = conditionOR + FVS1[k].FieldName + " = '" + FVS1[k].Value + "' ";
                            break;
                    }
                    condition1 = condition1 + values1;
                }
                if (condition1.Length > 3)
                {
                    condition1 = condition1.Substring(3, condition1.Length - 3);
                    condition = condition + " and (" + condition1 + ")";
                }
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                    {
                        orderlist.Add(FVS[i].FieldName);
                        if (FVS[i].orderMethod == OrderMenthod.ASC)
                            ordermethod.Add(" ASC ");
                        else
                            ordermethod.Add(" DESC ");
                    }
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }

                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where " + condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTable(int PageSize, int CurrentPageIndex, MyWebControlLib.FieldValues FVS)
        {
            // int RecordCount = GetRecordCount(FVS);
            int hadReadRecordCount = (CurrentPageIndex - 1) * PageSize;

            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = _conditionOperater + FVS[i].FieldName + " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = _conditionOperater + FVS[i].FieldName + " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = _conditionOperater + FVS[i].FieldName + " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = _conditionOperater + FVS[i].FieldName + " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = _conditionOperater + FVS[i].FieldName + " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = _conditionOperater + FVS[i].FieldName + " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = _conditionOperater + FVS[i].FieldName + " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = _conditionOperater + FVS[i].FieldName + " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + values;
                }
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                    {
                        orderlist.Add(FVS[i].FieldName);
                        if (FVS[i].orderMethod == OrderMenthod.ASC)
                            ordermethod.Add(" ASC ");
                        else
                            ordermethod.Add(" DESC ");
                    }
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }

                SqlDataAdapter adapter = new SqlDataAdapter("select top " + Convert.ToString(PageSize) +
                    " " + _columnNames + " from " + _viewname + " where (tableID not in (select top " +
                    Convert.ToString(hadReadRecordCount) +
                    " tableID from " + _viewname + " where " + condition + order + ")) and " +
                    condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        //public DataTable GetDataTable(int PageSize, int CurrentPageIndex, string condition,string order)
        //{
        //    // int RecordCount = GetRecordCount(FVS);
        //    int hadReadRecordCount = (CurrentPageIndex - 1) * PageSize;

        //    if (_viewname != "")
        //    {


        //        SqlDataAdapter adapter = new SqlDataAdapter("select top " +PageSize.ToString() +
        //            " " + _columnNames + " from " + _viewname + " where (tableID not in (select top " +
        //            hadReadRecordCount.ToString() +
        //            " tableID from " + _viewname + " where " + condition + order + ")) and " +
        //            condition + order, conn);
        //        adapter.SelectCommand.CommandTimeout = 0;
        //        DataSet ds = new DataSet(_viewname);
        //        adapter.Fill(ds, _viewname);
        //        return ds.Tables[0];
        //    }
        //    else
        //        return null;
        //}
        public DataTable GetDataTable(int PageSize, int CurrentPageIndex, string condition)
        {
            // int RecordCount = GetRecordCount(FVS);
            int hadReadRecordCount = (CurrentPageIndex - 1) * PageSize;

            if (_viewname != "")
            {


                SqlDataAdapter adapter = new SqlDataAdapter("select top " + PageSize.ToString() +
                    " " + _columnNames + " from " + _viewname + " where (tableID not in (select top " +
                    hadReadRecordCount.ToString() +
                    " tableID from " + _viewname + " where " + condition + ")) and " +
                    condition, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTable(int PageSize, int CurrentPageIndex, MyWebControlLib.FieldValues FVS, MyWebControlLib.FieldValues FVS1)
        {
            // int RecordCount = GetRecordCount(FVS);
            int hadReadRecordCount = (CurrentPageIndex - 1) * PageSize;

            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = _conditionOperater + FVS[i].FieldName + " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = _conditionOperater + FVS[i].FieldName + " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = _conditionOperater + FVS[i].FieldName + " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = _conditionOperater + FVS[i].FieldName + " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = _conditionOperater + FVS[i].FieldName + " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = _conditionOperater + FVS[i].FieldName + " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = _conditionOperater + FVS[i].FieldName + " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = _conditionOperater + FVS[i].FieldName + " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = _conditionOperater + FVS[i].FieldName + " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + values;
                }

                string condition1 = "";
                for (int k = 0; k < FVS1.Count; k++)
                {
                    string values1 = "";
                    string conditionOR = " or ";
                    FVOperater OP = FVS1[k].FVOperater;
                    switch (OP)
                    {

                        case FVOperater.Great:
                            values1 = conditionOR + FVS1[k].FieldName + " > '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values1 = conditionOR + FVS1[k].FieldName + " < '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values1 = conditionOR + FVS1[k].FieldName + " <= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values1 = conditionOR + FVS1[k].FieldName + " >= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.In:
                            values1 = conditionOR + FVS1[k].FieldName + " in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values1 = conditionOR + FVS1[k].FieldName + " not in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values1 = conditionOR + FVS1[k].FieldName + " <> '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values1 = conditionOR + FVS1[k].FieldName + " = '" + FVS1[k].Value + "' ";
                            break;
                    }
                    condition1 = condition1 + values1;
                }
                if (condition1.Length > 3)
                {
                    condition1 = condition1.Substring(3, condition1.Length - 3);
                    condition = condition + " and (" + condition1 + ")";
                }

                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                    {
                        orderlist.Add(FVS[i].FieldName);
                        if (FVS[i].orderMethod == OrderMenthod.ASC)
                            ordermethod.Add(" ASC ");
                        else
                            ordermethod.Add(" DESC ");
                    }
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }

                SqlDataAdapter adapter = new SqlDataAdapter("select top " + Convert.ToString(PageSize) +
                    " " + _columnNames + " from " + _viewname + " where (tableID not in (select top " +
                    Convert.ToString(hadReadRecordCount) +
                    " tableID from " + _viewname + " where " + condition + order + ")) and " +
                    condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTable(MyWebControlLib.FieldValues FieldFVS, string condition)
        {
            if (_viewname != "")
            {
                string DestinationField = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    string fieldname = FieldFVS[i].FieldName;
                    MyWebControlLib.StatMethod statmethod = FieldFVS[i].statMethod;
                    string distinct = "";
                    if (FieldFVS[i].Distinct)
                        distinct = " Distinct ";
                    else
                        distinct = "";
                    string method = "Sum";
                    switch (statmethod)
                    {
                        case StatMethod.Avg:
                            method = "Avg";
                            break;
                        case StatMethod.Count:
                            method = "Count";
                            break;
                        case StatMethod.Max:
                            method = "Max";
                            break;
                        case StatMethod.Min:
                            method = "Min";
                            break;
                        case StatMethod.Sum:
                            method = "Sum";
                            break;
                        default:
                            method = "";
                            break;
                    }
                    if (method == "")
                        DestinationField = DestinationField + method + distinct + fieldname + " as " + FieldFVS[i].FieldName + ",";
                    else
                        DestinationField = DestinationField + method + "(" + distinct + fieldname + ") as " + FieldFVS[i].FieldName + ",";
                }
                DestinationField = DestinationField.Substring(0, DestinationField.Length - 1);

                //生成非聚合字段分组条件
                string groupby = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if (FieldFVS[i].statMethod == MyWebControlLib.StatMethod.None)
                        groupby = groupby + FieldFVS[i].FieldName + ",";
                }
                if (groupby != "")
                {
                    groupby = " Group by " + groupby.Substring(0, groupby.Length - 1);
                }

                //排序字段
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if ((FieldFVS[i].Ordered == true) & (orderlist.IndexOf(FieldFVS[i].FieldName) == -1))
                        orderlist.Add(FieldFVS[i].FieldName);
                    if (FieldFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                if (condition.Trim() == "")
                    condition = " 1=1 ";
                SqlDataAdapter adapter = new SqlDataAdapter("select " + DestinationField + " from " + _viewname + " where " + condition + " " + groupby + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }

        public DataTable GetDataTable(string coulmnNames, string condition)
        {
            if (_viewname != "")
            {
           
                if (condition.Trim() == "")
                    condition = " 1=1 ";
                SqlDataAdapter adapter = new SqlDataAdapter("select " + coulmnNames + " from " + _viewname + " where " + condition , conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public DataTable GetDataTable(string coulmnNames, string condition,string orderstr,int pagesize,int currentpage)
        {
            if (_viewname != "")
            {

                if (condition.Trim() == "")
                    condition = " 1=1 ";
                SqlDataAdapter adapter = new SqlDataAdapter("select " + coulmnNames + " from " + _viewname + " where " + condition, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds.Tables[0];
            }
            else
                return null;
        }
        public int AddLog(string type)
        {
            return 1;
        }
        public DataRow GetDataRowByID(int tableID)
        {
            if (_viewname != "")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where tableID=" + tableID, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
                else
                    return null;
            }
            else
                return null;
        }

        public DataRow GetDataRowByID(string tableID)
        {
            if (_viewname != "")
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where tableID ='" + tableID + "'", conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
                else
                    return null;
            }
            else
                return null;
        }
        public DataRow GetDataRowByKeys(FieldValues FVS)
        {
            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + FVS[i].FieldName + values;
                }
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                        orderlist.Add(FVS[i].FieldName);
                    if (FVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where " + condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                if (ds.Tables[0].Rows.Count > 0)
                    return ds.Tables[0].Rows[0];
                else
                    return null;
            }
            else
                return null;
        }

        public DataSet GetDataSet()
        {
            if (_viewname != "")
            {

                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds;
            }
            else
                return null;
        }
        public DataSet GetDataSet(MyWebControlLib.FieldValues FVS)
        {
            if (_viewname != "")
            {
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + FVS[i].FieldName + values;
                }
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < FVS.Count; i++)
                {
                    if ((FVS[i].Ordered == true) & (orderlist.IndexOf(FVS[i].FieldName) == -1))
                        orderlist.Add(FVS[i].FieldName);
                    if (FVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select " + _columnNames + " from " + _viewname + " where " + condition + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds;
            }
            else
                return null;
        }
        public DataSet GetDataSet(MyWebControlLib.FieldValues FieldFVS, MyWebControlLib.FieldValues ConditionFVS)
        {
            if (_viewname != "")
            {
                //生成查询条件
                string condition = " (1=1) ";
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = ConditionFVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + ConditionFVS[i].Value + "' ";
                            break;
                        default:
                            values = " = '" + ConditionFVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + ConditionFVS[i].FieldName + values;
                }
                //生成目标列
                string DestinationField = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    string fieldname = FieldFVS[i].FieldName;
                    MyWebControlLib.StatMethod statmethod = FieldFVS[i].statMethod;
                    string distinct = "";
                    if (FieldFVS[i].Distinct)
                        distinct = " Distinct ";
                    else
                        distinct = "";
                    string method = "Sum";
                    switch (statmethod)
                    {
                        case StatMethod.Avg:
                            method = "Avg";
                            break;
                        case StatMethod.Count:
                            method = "Count";
                            break;
                        case StatMethod.Max:
                            method = "Max";
                            break;
                        case StatMethod.Min:
                            method = "Min";
                            break;
                        case StatMethod.Sum:
                            method = "Sum";
                            break;
                        default:
                            method = "";
                            break;
                    }
                    if (method == "")
                        DestinationField = DestinationField + method + distinct + fieldname + " as " + FieldFVS[i].FieldName + ",";
                    else
                        DestinationField = DestinationField + method + "(" + distinct + fieldname + ") as " + FieldFVS[i].FieldName + ",";
                }
                DestinationField = DestinationField.Substring(0, DestinationField.Length - 1);

                //生成非聚合字段分组条件
                string groupby = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if (FieldFVS[i].statMethod == MyWebControlLib.StatMethod.None)
                        groupby = groupby + FieldFVS[i].FieldName + ",";
                }
                if (groupby != "")
                {
                    groupby = " Group by " + groupby.Substring(0, groupby.Length - 1);
                }

                //排序字段
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    if ((ConditionFVS[i].Ordered == true) & (orderlist.IndexOf(ConditionFVS[i].FieldName) == -1))
                        orderlist.Add(ConditionFVS[i].FieldName);
                    if (ConditionFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if ((FieldFVS[i].Ordered == true) & (orderlist.IndexOf(FieldFVS[i].FieldName) == -1))
                        orderlist.Add(FieldFVS[i].FieldName);
                    if (FieldFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select " + DestinationField + " from " + _viewname + " where " + condition + groupby + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds;
            }
            else
                return null;
        }
        public DataSet GetDataSetFromTable(MyWebControlLib.FieldValues FieldFVS, MyWebControlLib.FieldValues ConditionFVS)
        {
            if (_tablename != "")
            {
                //生成查询条件
                string condition = " (1=1) ";
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = ConditionFVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + ConditionFVS[i].Value + "' ";
                            break;
                        default:
                            values = " = '" + ConditionFVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + ConditionFVS[i].FieldName + values;
                }
                //生成目标列
                string DestinationField = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    string fieldname = FieldFVS[i].FieldName;
                    MyWebControlLib.StatMethod statmethod = FieldFVS[i].statMethod;
                    string distinct = "";
                    if (FieldFVS[i].Distinct)
                        distinct = " Distinct ";
                    else
                        distinct = "";
                    string method = "Sum";
                    switch (statmethod)
                    {
                        case StatMethod.Avg:
                            method = "Avg";
                            break;
                        case StatMethod.Count:
                            method = "Count";
                            break;
                        case StatMethod.Max:
                            method = "Max";
                            break;
                        case StatMethod.Min:
                            method = "Min";
                            break;
                        case StatMethod.Sum:
                            method = "Sum";
                            break;
                        default:
                            method = "";
                            break;
                    }
                    if (method == "")
                        DestinationField = DestinationField + method + distinct + fieldname + " as " + FieldFVS[i].FieldName + ",";
                    else
                        DestinationField = DestinationField + method + "(" + distinct + fieldname + ") as " + FieldFVS[i].FieldName + ",";
                }
                DestinationField = DestinationField.Substring(0, DestinationField.Length - 1);

                //生成非聚合字段分组条件
                string groupby = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if (FieldFVS[i].statMethod == MyWebControlLib.StatMethod.None)
                        groupby = groupby + FieldFVS[i].FieldName + ",";
                }
                if (groupby != "")
                {
                    groupby = " Group by " + groupby.Substring(0, groupby.Length - 1);
                }

                //排序字段
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    if ((ConditionFVS[i].Ordered == true) & (orderlist.IndexOf(ConditionFVS[i].FieldName) == -1))
                        orderlist.Add(ConditionFVS[i].FieldName);
                    if (ConditionFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if ((FieldFVS[i].Ordered == true) & (orderlist.IndexOf(FieldFVS[i].FieldName) == -1))
                        orderlist.Add(FieldFVS[i].FieldName);
                    if (FieldFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select " + DestinationField + " from " + _tablename + " where " + condition + groupby + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                return ds;
            }
            else
                return null;
        }
        public DataSet GetDataSet(MyWebControlLib.FieldValues FieldFVS, MyWebControlLib.FieldValues ConditionFVS, MyWebControlLib.FieldValues FVS1)
        {
            if (_viewname != "")
            {
                //生成查询条件
                string condition = " (1=1) ";
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = ConditionFVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + ConditionFVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + ConditionFVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + ConditionFVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + ConditionFVS[i].Value + "' ";
                            break;
                        default:
                            values = " = '" + ConditionFVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + ConditionFVS[i].FieldName + values;
                }

                string condition1 = "";
                for (int k = 0; k < FVS1.Count; k++)
                {
                    string values1 = "";
                    string conditionOR = " or ";
                    FVOperater OP = FVS1[k].FVOperater;
                    switch (OP)
                    {

                        case FVOperater.Great:
                            values1 = conditionOR + FVS1[k].FieldName + " > '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values1 = conditionOR + FVS1[k].FieldName + " < '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '%" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values1 = conditionOR + FVS1[k].FieldName + " Like '" + FVS1[k].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values1 = conditionOR + FVS1[k].FieldName + " <= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values1 = conditionOR + FVS1[k].FieldName + " >= '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.In:
                            values1 = conditionOR + FVS1[k].FieldName + " in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values1 = conditionOR + FVS1[k].FieldName + " not in (" + FVS1[k].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values1 = conditionOR + FVS1[k].FieldName + " <> '" + FVS1[k].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        case FVOperater.Is:
                            values1 = conditionOR + FVS1[k].FieldName + " is " + FVS1[k].Value + " ";
                            break;
                        default:
                            values1 = conditionOR + FVS1[k].FieldName + " = '" + FVS1[k].Value + "' ";
                            break;
                    }
                    condition1 = condition1 + values1;
                }
                if (condition1.Length > 3)
                {
                    condition1 = condition1.Substring(3, condition1.Length - 3);
                    condition = condition + " and (" + condition1 + ")";
                }


                //生成目标列
                string DestinationField = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    string fieldname = FieldFVS[i].FieldName;
                    MyWebControlLib.StatMethod statmethod = FieldFVS[i].statMethod;
                    string distinct = "";
                    if (FieldFVS[i].Distinct)
                        distinct = " Distinct ";
                    else
                        distinct = "";
                    string method = "Sum";
                    switch (statmethod)
                    {
                        case StatMethod.Avg:
                            method = "Avg";
                            break;
                        case StatMethod.Count:
                            method = "Count";
                            break;
                        case StatMethod.Max:
                            method = "Max";
                            break;
                        case StatMethod.Min:
                            method = "Min";
                            break;
                        case StatMethod.Sum:
                            method = "Sum";
                            break;
                        default:
                            method = "";
                            break;
                    }
                    if (method == "")
                        DestinationField = DestinationField + method + distinct + fieldname + " as " + FieldFVS[i].FieldName + ",";
                    else
                        DestinationField = DestinationField + method + "(" + distinct + fieldname + ") as " + FieldFVS[i].FieldName + ",";
                }
                DestinationField = DestinationField.Substring(0, DestinationField.Length - 1);

                //生成非聚合字段分组条件
                string groupby = "";
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if (FieldFVS[i].statMethod == MyWebControlLib.StatMethod.None)
                        groupby = groupby + FieldFVS[i].FieldName + ",";
                }
                if (groupby != "")
                {
                    groupby = " Group by " + groupby.Substring(0, groupby.Length - 1);
                }

                //排序字段
                string order = "";
                ArrayList orderlist = new ArrayList();
                ArrayList ordermethod = new ArrayList();
                for (int i = 0; i < ConditionFVS.Count; i++)
                {
                    if ((ConditionFVS[i].Ordered == true) & (orderlist.IndexOf(ConditionFVS[i].FieldName) == -1))
                        orderlist.Add(ConditionFVS[i].FieldName);
                    if (ConditionFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < FieldFVS.Count; i++)
                {
                    if ((FieldFVS[i].Ordered == true) & (orderlist.IndexOf(FieldFVS[i].FieldName) == -1))
                        orderlist.Add(FieldFVS[i].FieldName);
                    if (FieldFVS[i].orderMethod == OrderMenthod.ASC)
                        ordermethod.Add(" ASC ");
                    else
                        ordermethod.Add(" DESC ");
                    // order = order + ConditionFVS[i].FieldName + ",";
                }
                for (int i = 0; i < orderlist.Count; i++)
                {
                    order = order + orderlist[i].ToString() + ordermethod[i].ToString() + ",";
                }
                if (order != "")
                {
                    order = " Order By " + order;
                    order = order.Substring(0, order.Length - 1);
                }
                SqlDataAdapter adapter = new SqlDataAdapter("select " + DestinationField + " from " + _viewname + " where " + condition + groupby + order, conn);
                adapter.SelectCommand.CommandTimeout = 0;
                DataSet ds = new DataSet(_viewname);
                adapter.Fill(ds, _viewname);
                return ds;
            }
            else
                return null;
        }

        public int Update(MyWebControlLib.FieldValues FV, int tableID)
        {
            if (_tablename != "")
            {
                DBAccess.DBHelper h = new DBAccess.DBHelper();
                SqlDataAdapter adapter = h.GetSqlDataAdapter(_tablename);
                adapter.SelectCommand.CommandText = "select * from " + _tablename + " where tableID=" + tableID.ToString();
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    for (int i = 0; i < FV.Count; i++)
                    {
                        if (FV[i].Value == null)
                        {
                            row[FV[i].FieldName] = System.DBNull.Value;
                        }
                        else
                        {
                            row[FV[i].FieldName] = FV[i].Value;
                        }
                    }
                    try
                    {
                        adapter.Update(ds, _tablename);
                        return 0;
                    }
                    catch
                    {
                        return -1;
                    }
                }
                else
                    //为找到相关记录
                    return -2;
            }
            else
                return -3;//未设置表名
        }
        public int Update(MyDataRows ds)
        {
            if (_tablename != "")
            {
                string keyname = "";//Helper
                string condition = "";
                int rowcount = ds.Count;
                char[] charSeparators = new char[] { ',' };
                DBAccess.DBHelper h = new DBAccess.DBHelper();
                keyname = h.GetKeyField(_tablename);
                SqlDataAdapter adapter = h.GetSqlDataAdapter(_tablename);

                string[] keylist = keyname.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < rowcount; i++)
                {
                    condition = "(1=1)";
                    for (int j = 0; j < keylist.Length; j++)
                    {
                        for (int k = 0; k < ds[i].Count; k++)
                            if (ds[i][k].FieldName == keylist[j])
                                condition = condition + " and " + keylist[j] + "='" + ds[i][k].Value + "' ";
                    }
                    adapter.SelectCommand.CommandText = "select * from " + _tablename + " where " + condition;
                    DataSet desDS = new DataSet(_tablename);

                    adapter.Fill(desDS);
                    if (desDS.Tables[0].Rows.Count > 0)//如果存在则更新
                    {
                        if (_findthenupdate)
                        {
                            for (int descount = 0; descount < desDS.Tables[0].Rows.Count; descount++)
                                for (int k = 0; k < (ds[i].Count - 1); k++)//默认情况下，各数据表最后一个字段均为tableID且为自动编号，这里忽略对此字段的处理
                                    desDS.Tables[0].Rows[descount][ds[i][k].FieldName] = ds[i][k].Value;
                        }
                        else
                        {
                            //存在主键相同的记录,更新请求被回滚
                            throw new DBKeyViolationException();
                        }
                    }
                    else//如果不存在则追加
                    {
                        DataRow row = desDS.Tables[0].NewRow();
                        string str = "";
                        for (int k = 0; k < ds[i].Count; k++)
                        {
                            row[ds[i][k].FieldName] = ds[i][k].Value;
                            str = str + ds[i][k].FieldName + "=" + ds[i][k].Value + ";";

                        }
                        desDS.Tables[0].Rows.Add(row);
                    }
                    adapter.Update(desDS);

                }

                return 0;
            }
            else
                return -3;//未设置表名
        }
        public int Update(DataTable table)
        {
            if (_tablename != "")
            {
                string keyname = "";//Helper
                string condition = "";
                int rowcount = table.Rows.Count;
                char[] charSeparators = new char[] { ',' };
                DBAccess.DBHelper h = new DBAccess.DBHelper();
                keyname = h.GetKeyField(_tablename);
                SqlDataAdapter adapter = h.GetSqlDataAdapter(_tablename);
                string[] keylist = keyname.Split(charSeparators, System.StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < rowcount; i++)
                {
                    condition = "(1=1)";

                    for (int j = 0; j < keylist.Length; j++)
                    {
                        condition = condition + " and " + keylist[j] + "='" + table.Rows[i][keylist[j]].ToString() + "' ";
                    }
                    adapter.SelectCommand.CommandText = "select * from " + _tablename + " where " + condition;
                    DataSet desDS = new DataSet(_tablename);
                    adapter.Fill(desDS);
                    if (desDS.Tables[0].Rows.Count > 0)//如果存在则更新
                    {
                        for (int descount = 0; descount < desDS.Tables[0].Rows.Count; descount++)
                            for (int k = 0; k < (table.Columns.Count - 1); k++)//默认情况下，各数据表最后一个字段均为tableID且为自动编号，这里忽略对此字段的处理
                                desDS.Tables[0].Rows[descount][table.Columns[k].ColumnName.ToString()] = table.Rows[i][table.Columns[k].ColumnName.ToString()];
                    }
                    else//如果不存在则追加
                    {
                        if (_findthenupdate)
                        {
                            DataRow row = desDS.Tables[0].NewRow();
                            for (int k = 0; k < (table.Columns.Count - 1); k++)
                                row[table.Columns[k].ColumnName.ToString()] = table.Rows[i][table.Columns[k].ColumnName.ToString()];
                            desDS.Tables[0].Rows.Add(row);
                        }
                        else
                        {
                            throw new DBKeyViolationException();
                        }
                    }
                    string s = adapter.UpdateCommand.CommandText;
                    adapter.Update(desDS);

                }

                return 0;
            }
            else
                return -3;//未设置表名
        }
        public int Updata(DataSet ds)
        {
            DBAccess.DBHelper dbhelper = new DBAccess.DBHelper();
            if (dbhelper.Update(ds) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int GetRecordCount(MyWebControlLib.FieldValues FVS)
        {
            if (_viewname != "")
            {
                //生成查询条件
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + FVS[i].Value + "' ";
                            break;
                        default:
                            values = " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + FVS[i].FieldName + values;
                }
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand command = new SqlCommand("select count(tableID) from " + _viewname + " where " + condition, conn);
                command.CommandTimeout = 0;

                SqlDataReader reader = command.ExecuteReader();
                int returnvalue = 0;
                if ((reader.HasRows) && (reader.Read()))
                    returnvalue = Convert.ToInt32(reader.GetValue(0).ToString());
                conn.Close();
                return returnvalue;
            }
            else
                return 0;

        }
        public int GetRecordCount(string condition)
        {
            if (_viewname != "")
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand command = new SqlCommand("select count(tableID) from " + _viewname + " where " + condition, conn);
                command.CommandTimeout = 0;
                SqlDataReader reader = command.ExecuteReader();
                int returnvalue = 0;
                if ((reader.HasRows) && (reader.Read()))
                    returnvalue = Convert.ToInt32(reader.GetValue(0).ToString());
                conn.Close();
                return returnvalue;
            }
            else
                return 0;
        }

        public int Add(MyWebControlLib.FieldValues FV)
        {
            if (_tablename != "")
            {
                DBAccess.DBHelper h = new DBAccess.DBHelper();
                SqlDataAdapter adapter = h.GetSqlDataAdapter(_tablename);
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);
                DataRow row = ds.Tables[0].NewRow();
                for (int i = 0; i < FV.Count; i++)
                {
                    row[FV[i].FieldName] = FV[i].Value;
                }
                ds.Tables[0].Rows.Add(row);
                try
                {
                    adapter.Update(ds, _tablename);
                    return 0;
                }
                catch
                {
                    return -1;
                }
            }
            else
                return -3;//未设定表名
        }
        public int Add(DataSet ds)
        {
            return 0;
        }

        public int Delete(MyWebControlLib.FieldValues FVS)
        {
            if (_tablename != "")
            {
                DBAccess.DBHelper h = new DBAccess.DBHelper();
                SqlDataAdapter adapter = h.GetSqlDataAdapter(_tablename);
                string condition = " (1=1) ";
                for (int i = 0; i < FVS.Count; i++)
                {
                    string values = "";
                    FVOperater o = FVS[i].FVOperater;
                    switch (o)
                    {

                        case FVOperater.Great:
                            values = " > '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Less:
                            values = " < '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeB:
                            values = " Like '%" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.LikeBE:
                            values = " Like '%" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.LikeE:
                            values = " Like '" + FVS[i].Value + "%' ";
                            break;
                        case FVOperater.NotGreat:
                            values = " <= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.NotLess:
                            values = " >= '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.In:
                            values = " in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotIn:
                            values = " not in (" + FVS[i].Value + ") ";
                            break;
                        case FVOperater.NotEqual:
                            values = " <> '" + FVS[i].Value + "' ";
                            break;
                        case FVOperater.Order:
                            break;
                        default:
                            values = " = '" + FVS[i].Value + "' ";
                            break;
                    }
                    condition = condition + _conditionOperater + FVS[i].FieldName + values;
                }
                adapter.SelectCommand.CommandText = "select * from " + _tablename + " where " + condition;
                DataSet ds = new DataSet(_tablename);
                adapter.Fill(ds, _tablename);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    ds.Tables[0].Rows[i].Delete();
                //使用事务处理V2
                DBAccess.DBHelper dh = new DBAccess.DBHelper();
                dh.Update(ds);
                //不使用事务处理V1
                //adapter.Update(ds,_tablename);
                return 0;
            }
            else
                return -3;
        }

        public ConditionLinkOperater ConditionLinkOp
        {
            set
            {
                if (value == ConditionLinkOperater.Or)
                    _conditionOperater = " or ";
                else
                    _conditionOperater = " and ";
            }
        }
        public bool FindThenUpdate
        {
            get
            {
                return _findthenupdate;
            }
            set
            {
                _findthenupdate = value;
            }
        }
        /// <summary>
        /// 用于执行单条sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int FAPR(string sql)
        {
            int i = -1;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Transaction = trans;
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                i = cmd.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch
            {
                trans.Rollback();
                conn.Close();
            }
            if (i >= 1)
                return 0;
            else
                return -1;



        }
        public int ExecSql(string sql)
        {
            int i = -1;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
           
            try
            {
                SqlCommand cmd = new SqlCommand(sql);
                
                cmd.Connection = conn;
                cmd.CommandTimeout = 0;
                i = cmd.ExecuteNonQuery();
               
                conn.Close();
            }
            catch
            {
                
                conn.Close();
            }
            if (i >= 1)
                return 0;
            else
                return -1;
        }
        public void ExecSqlWithTrans(string sql)
        {
            
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;                
                cmd.CommandTimeout = 0;

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                trans.Commit();
                conn.Close();
            }
            catch
            {
                trans.Rollback();
                conn.Close();
            }
           
        }
        public static void SaveDataTableToExcel(string fileName, DataTable dt)
        {
            //string urlPath = HttpContext.Current.Request.ApplicationPath + "/Data/";
            string physicPath ="";
            //string fileName = _user.UserName + "_Visa.Xls";
            DataTable table = new DataTable();
            try
            {
                if (System.IO.File.Exists(physicPath + fileName))
                    System.IO.File.Delete(physicPath + fileName);
                string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + physicPath + fileName + ";Extended Properties=Excel 8.0;";
                OleDbConnection objConn = new OleDbConnection(connString);
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = objConn;
                objCmd.Connection.Open();


                //建立表结构
                //objCmd.CommandText = @"CREATE TABLE "+dt.TableName+"1"+"(";
                string commandStr = @"CREATE TABLE " + dt.TableName + " (";
                for (int iii = 0; iii < dt.Columns.Count; iii++)
                {
                    commandStr += dt.Columns[iii].ToString();
                    commandStr += " varchar,";
                }
                commandStr = commandStr.Substring(0, commandStr.Length - 1);
                commandStr += ")";
                objCmd.CommandText = commandStr;
                objCmd.ExecuteNonQuery();
                //建立插入动作的Command
                commandStr = @"INSERT INTO  " + dt.TableName + " (";
                string parcom = "";
                objCmd.Parameters.Clear();
                for (int kkk = 0; kkk < dt.Columns.Count; kkk++)
                {
                    commandStr += dt.Columns[kkk].ToString();
                    commandStr += ",";
                    parcom += "@" + kkk.ToString();
                    parcom += ",";
                    objCmd.Parameters.Add(new OleDbParameter("@" + kkk.ToString(), OleDbType.VarChar));
                }
                commandStr = commandStr.Substring(0, commandStr.Length - 1);
                commandStr += ") values (";
                parcom = parcom.Substring(0, parcom.Length - 1);
                parcom += " )";
                objCmd.CommandText = commandStr + parcom;

                //遍历DataSet将数据插入新建的Excel文件中
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < objCmd.Parameters.Count; i++)
                    {
                        objCmd.Parameters[i].Value = row[i].ToString();
                    }
                    objCmd.ExecuteNonQuery();
                }



                objCmd.Connection.Close();

            }
            catch
            {
            }
        }

     
    }
       
}
