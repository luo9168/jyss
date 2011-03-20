using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace CommonClasses
{
    public class UserRoles
    {
        #region 变量
        bool isvalid = false;
        string username = "";
        string password = "";
        string truename = "";
        string unitcode = "";
        string unitname = "";
        bool isonline = false;
        bool isPrivate = false;
        ModuleRoles modulekeys = new ModuleRoles();
        private UserRolesDB udb = new UserRolesDB();
        #endregion

        #region 属性

        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }
        public string TrueName
        {
            get
            {
                return truename;
            }
            set
            {
                truename = value;
            }
        }
        public string UnitCode
        {
            get
            {
                return unitcode;
            }
            set
            {
                unitcode = value;
            }
        }
        public string UnitName
        {
            get
            {
                return unitname;
            }            
        }
        
        public bool IsValid
        {
            get
            {
                return isvalid;
            }
        }
        public bool IsOnLine
        {
            get
            {
                return isonline;
            }
        }
        public bool IsPrivate
        {
            get
            {
                return isPrivate;
            }
        }
        public ModuleRoles ModuleKeys
        {
            get
            {
                return modulekeys;
            }
        }
        #endregion

        #region 构造函数


        /// <summary>
        /// 默认构造函数

        /// </summary>
        public UserRoles()
        {
        }

        /// <summary>
        /// 构造函数：已知用户名，从视图中收集信息创建用户实例
        /// </summary>
        /// <param name="userName">用户名（即员工号）</param>
        public UserRoles(string userName)
        {
            username = userName; 
            MyWebControlLib.FieldValues fvs = new MyWebControlLib.FieldValues();
            MyWebControlLib.FieldValue fv = new MyWebControlLib.FieldValue();
            fv.FieldName = "userName";
            fv.Value = userName;                                   
            fvs.Add(fv);
            DataTable table = udb.GetDataTable(fvs);
            if (table.Rows.Count > 0)
            {
               
                unitcode = table.Rows[0]["unitCode"].ToString();
                unitname = table.Rows[0]["unitName"].ToString();
                isvalid = true;
                bool flag=true;
                //flag = false;//上海 权限管理不启用
                if (flag)
                {
                    truename = table.Rows[0]["truename"].ToString();                   
                    
                    if (table.Rows[0]["isonline"].ToString() == "1")
                        isonline = true;
                    if (table.Rows[0]["isprivate"].ToString() == "1")
                        isPrivate = true;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        ModuleRole mr = new ModuleRole();
                        mr.ModuleKey = table.Rows[i]["ModuleKey"].ToString();
                        bool first = false;
                        int index = modulekeys.IndexOf(mr.ModuleKey);//该modulekey第一次出现的位置，若此次为第一次出现，则为-1
                        if (index == -1)
                            first = true;
                        if (table.Rows[i]["isadd"].ToString() == "1")
                        {
                            if (first)//第一次出现

                                mr.Add = true;
                            else//只对第一次出现该ModuleKey位置的ModuleRole赋值

                                modulekeys[index].Add = true;
                        }
                        if (table.Rows[i]["ischg"].ToString() == "1")
                        {
                            if (first)
                                mr.Edit = true;
                            else
                                modulekeys[index].Edit = true;
                        }
                        if (table.Rows[i]["isdel"].ToString() == "1")
                        {
                            if (first)
                                mr.Delete = true;
                            else
                                modulekeys[index].Delete = true;
                        }
                        if (table.Rows[i]["isapp"].ToString() == "1")
                        {
                            if (first)
                                mr.Approve = true;
                            else
                                modulekeys[index].Approve = true;
                        }
                        if (table.Rows[i]["iscom"].ToString() == "1")
                        {
                            if (first)
                                mr.Finish = true;
                            else
                                modulekeys[index].Finish = true;
                        }
                        if (table.Rows[i]["isrep"].ToString() == "1")
                        {
                            if (first)
                                mr.Report = true;
                            else
                                modulekeys[index].Report = true;
                        }
                        if (table.Rows[i]["ispub"].ToString() == "1")
                        {
                            if (first)
                                mr.Publish = true;
                            else
                                modulekeys[index].Publish = true;
                        }
                        modulekeys.Add(mr);

                    }
                }
            }
        }

        /// <summary>
        /// 构造函数：已知用户名和密码，创建用户实例

        /// </summary>
        /// <param name="userName">用户名（即员工号）</param>
        /// <param name="passWord">密码</param>
        public UserRoles(string userName, string passWord)
        {
            MyWebControlLib.FieldValues fvs= new MyWebControlLib.FieldValues();
            MyWebControlLib.FieldValue fv= new MyWebControlLib.FieldValue();
            fv.FieldName ="userName";
            userName= userName.Replace("'","");
            passWord = passWord.Replace("'", "");
            fv.Value =userName;
            username = userName;
            password = passWord;
            fvs.Add(fv);
            fv= new MyWebControlLib.FieldValue();
            fv.FieldName ="password";
            fv.Value =passWord;
            fvs.Add(fv);
            DataTable table = udb.GetDataTable(fvs);
            if (table.Rows.Count > 0)
            {
                isvalid = true;
                
               
                unitcode = table.Rows[0]["unitCode"].ToString();
                unitname = table.Rows[0]["unitName"].ToString();
                bool flag = true;
                //flag = false;//上海
                if (flag)
                {
                    truename = table.Rows[0]["truename"].ToString();
                    if (table.Rows[0]["isonline"].ToString() == "1")
                        isonline = true;
                    if (table.Rows[0]["isprivate"].ToString() == "1")
                        isPrivate = true;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        ModuleRole mr = new ModuleRole();
                        mr.ModuleKey = table.Rows[i]["ModuleKey"].ToString();
                        bool first = false;
                        int index = modulekeys.IndexOf(mr.ModuleKey);//该modulekey第一次出现的位置，若此次为第一次出现，则为-1
                        if (index == -1)
                            first = true;
                        if (table.Rows[i]["isadd"].ToString() == "1")
                        {
                            if (first)//第一次出现

                                mr.Add = true;
                            else//只对第一次出现该ModuleKey位置的ModuleRole赋值

                                modulekeys[index].Add = true;
                        }
                        if (table.Rows[i]["ischg"].ToString() == "1")
                        {
                            if (first)
                                mr.Edit = true;
                            else
                                modulekeys[index].Edit = true;
                        }
                        if (table.Rows[i]["isdel"].ToString() == "1")
                        {
                            if (first)
                                mr.Delete = true;
                            else
                                modulekeys[index].Delete = true;
                        }
                        if (table.Rows[i]["isapp"].ToString() == "1")
                        {
                            if (first)
                                mr.Approve = true;
                            else
                                modulekeys[index].Approve = true;
                        }
                        if (table.Rows[i]["iscom"].ToString() == "1")
                        {
                            if (first)
                                mr.Finish = true;
                            else
                                modulekeys[index].Finish = true;
                        }
                        if (table.Rows[i]["isrep"].ToString() == "1")
                        {
                            if (first)
                                mr.Report = true;
                            else
                                modulekeys[index].Report = true;
                        }
                        if (table.Rows[i]["ispub"].ToString() == "1")
                        {
                            if (first)
                                mr.Publish = true;
                            else
                                modulekeys[index].Publish = true;
                        }
                        modulekeys.Add(mr);
                    }
                }
                
            }
            

        }
        #endregion  

        public DataTable getOriginTableByUserName()
        {
            MyWebControlLib.FieldValues fvs = new MyWebControlLib.FieldValues();
            MyWebControlLib.FieldValue fv = new MyWebControlLib.FieldValue();
            fv.FieldName = "username";
            fv.Value = username;
            fvs.Add(fv);
            return udb.GetOriginTableByKeys(fvs);
        }

        /// <summary>
        /// 设置在线
        /// </summary>
        public void SetOnLine()
        {
            //DataTable table = getOriginTableByUserName();
            //table.Rows[0]["isonline"] = 1;
            //udb.Update(table);
           
        }

        /// <summary>
        /// 设置下线
        /// </summary>
        public void SetOffLine()
        {
            //DataTable table = getOriginTableByUserName();
            //table.Rows[0]["isonline"] = 0;
            //udb.Update(table);            
        }
        
    }
    public class UserRolesDB : OprationDBBase
    {
        public UserRolesDB()
        {
            _tablename = "ZC_USERS";
            _viewname = "UserRoles_View";
            _columnNames = "*";
        }
    }

    public class ModuleRole
    {
        #region 属性

        bool browse = false;
        bool add = false;
        bool edit = false;        
        bool delete = false;
        bool finish = false;
        bool approve = false;
        bool report = false;
        bool publish = false;
        string modulekey = "";
        public ModuleRole()
        {

        }
        #endregion

        #region ModuleRole Property
        public bool Browse
        {
            get
            {
                return browse;
            }
            set
            {
                browse = value;
            }
        }        
        public bool Add
        {
            get
            {
                return add;
            }
            set
            {
                add = value;
            }
        }
        public bool Edit
        {
            get
            {
                return edit;
            }
            set
            {
                edit = value;
            }
        }        
        public bool Delete
        {
            get
            {
                return delete;
            }
            set
            {
                delete = value;
            }
        }
        public bool Finish
        {
            get
            {
                return finish;
            }
            set
            {
                finish = value;
            }
        }
        public bool Approve
        {
            get
            {
                return approve;
            }
            set
            {
                approve = value;
            }
        }
        public bool Report
        {
            get
            {
                return report;
            }
            set
            {
                report = value;
            }
        }
        public bool Publish
        {
            get
            {
                return publish;
            }
            set
            {
                publish = value;
            }
        }
        public string ModuleKey
        {
            get
            {
                return modulekey;
            }
            set
            {
                modulekey = value;
            }
        }
        #endregion
    }

    public class ModuleRoles : System.Collections.CollectionBase
    {
        public ModuleRole this[int index]
        {
            get
            {
                return ((ModuleRole)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// 默认空构造函数

        /// </summary>
        public ModuleRoles()
        {

        }

        /// <summary>
        /// 增加一个角色

        /// </summary>
        /// <param name="value">角色</param>
        /// <returns></returns>
        public int Add(ModuleRole value)
        {
            return (List.Add(value));
        }

        public int IndexOf(ModuleRole value)
        {
            return (List.IndexOf(value));
        }

        public int IndexOf(string ModuleKey)
        {
            int result = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].ModuleKey == ModuleKey)
                {
                    result = i;
                    return result;
                }
            }
            return result;
        }
        public void Insert(int index, ModuleRole value)
        {
            List.Insert(index, value);
        }
        public void Remove(ModuleRole value)
        {
            List.Remove(value);
        }
        public bool Contains(ModuleRole value)
        {
            return (List.Contains(value));
        }
        protected override void OnInsert(int index, Object value)
        {

        }
        protected override void OnSet(int index, Object oldValue, Object newValue)
        {

        }
        protected override void OnValidate(Object value)
        {
            //if (value.GetType()!=Type.GetType("ModuleRole"))
            //	throw new ArgumentException("value must be of type ModuleRole");
        }
    }
}
