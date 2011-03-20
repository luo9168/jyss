using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Drawing;
namespace CommonClasses
{
    public enum CloseWindowType { OK, Cancel }
    public struct Screen
    {
       public  int Width;
       public  int Height;
    }
    public class MyPage : System.Web.UI.Page

    {
        //protected HtmlImage[] buttons = new HtmlImage[] { };
        private HtmlImages _addButtons = new HtmlImages();
        private HtmlImages _editButtons = new HtmlImages();
        private HtmlImages _deleteButtons = new HtmlImages();
        private HtmlImages _browseButtons = new HtmlImages();
        private HtmlImages _finishButtons = new HtmlImages();
        private HtmlImages _approveButtons = new HtmlImages();
        private HtmlImages _reportButtons = new HtmlImages();
        private HtmlImages _publishButtons = new HtmlImages();
        private HtmlImages _lockButtons = new HtmlImages();
        private HtmlImages _unlockButtons = new HtmlImages();
       
        public Screen screen;

        private string _configfile = "";
        private string _moduleKey = "";
        private string _moduleName = "";
        protected CommonClasses.UserRoles _user = null;

        #region getset
        public string ConfigFile
        {
            get
            {
                return _configfile;
            }
            set
            {
                _configfile = value;
            }
        }
        public string ModuleKey
        {
            get
            {
                return _moduleKey;
            }
            set
            {
                _moduleKey = value;
            }
        }
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }
            set
            {
                _moduleName = value;
            }
        }
        public HtmlImages AddButtons
        {
            get
            {
                return _addButtons;
            }
            set
            {
                _addButtons = value;
            }
        }
        public HtmlImages EditButtons
        {
            get
            {
                return _editButtons;
            }
            set
            {
                _editButtons = value;
            }
        }
        public HtmlImages DeleteButtons
        {
            get
            {
                return _deleteButtons;
            }
            set
            {
                _deleteButtons = value;
            }
        }
        public HtmlImages BrowseButtons
        {
            get
            {
                return _browseButtons;
            }
            set
            {
                _browseButtons = value;
            }
        }
        public HtmlImages FinishButtons
        {
            get
            {
                return _finishButtons;
            }
            set
            {
                _finishButtons = value;
            }
        }
        public HtmlImages ApproveButtons
        {
            get
            {
                return _approveButtons;
            }
            set
            {
                _approveButtons = value;
            }
        }
        public HtmlImages ReportButtons
        {
            get
            {
                return _reportButtons;
            }
            set
            {
                _reportButtons = value;
            }
        }
        public HtmlImages PublishButtons
        {
            get
            {
                return _publishButtons;
            }
            set
            {
                _publishButtons = value;
            }
        }
        public HtmlImages LockButtons
        {
            get
            {
                return _lockButtons;
            }
            set
            {
                _lockButtons = value;
            }
        }
        public HtmlImages UnlockButtons
        {
            get
            {
                return _unlockButtons;
            }
            set
            {
                _unlockButtons = value;
            }
        }
        #endregion

        public MyPage()
        {
            string loginname = User.Identity.Name;
            _user = new UserRoles(loginname);
        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                screen = (Screen)Session["screen"];
            }
            catch
            {
                screen.Width = 1024;
                screen.Height = 768;
            }
            string loginname=User.Identity.Name;
            _user = new UserRoles(loginname);
            base.OnPreRender(e);
            bool flag = true;//启用权限管理为true,不启用为false。
            flag = false;//上海 权限管理不启用
            if (loginname == "")
            {
                Response.Redirect("default.htm");
            }
            else if (_moduleKey != "" && flag)
            {

                //若取权限，把以下语句注释。
                //最上级单位没有上报功能
                if (_user.UnitCode == "00")
                {
                    for (int i = 0; i < _reportButtons.Count; i++)
                    {
                        _reportButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                //除最上级单位外的其他单位没有加锁解锁功能
                if (_user.UnitCode != "00")
                {
                    for (int i = 0; i < _lockButtons.Count; i++)
                    {
                        _lockButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (_user.UnitCode != "00")
                {
                    for (int i = 0; i < _unlockButtons.Count; i++)
                    {
                        _unlockButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (_user.ModuleKeys.IndexOf(_moduleKey) == -1)
                {
                    Response.Redirect("ErrorPage.aspx?ID=1");
                }
                ModuleRole role = _user.ModuleKeys[_user.ModuleKeys.IndexOf(_moduleKey)];

                if (role.Add)
                {

                }
                else
                {
                    for (int i = 0; i < _addButtons.Count; i++)
                    {
                        _addButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Browse)
                { }
                else
                {
                    for (int i = 0; i < _browseButtons.Count; i++)
                    {
                        _browseButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Delete)
                { }
                else
                {
                    for (int i = 0; i < _deleteButtons.Count; i++)
                    {
                        _deleteButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Edit)
                { }
                else
                {
                    for (int i = 0; i < _editButtons.Count; i++)
                    {
                        _editButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Finish)
                { }
                else
                {
                    for (int i = 0; i < _finishButtons.Count; i++)
                    {
                        _finishButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Approve)
                { }
                else
                {
                    for (int i = 0; i < _approveButtons.Count; i++)
                    {
                        _approveButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Report)
                { }
                else
                {
                    for (int i = 0; i < _reportButtons.Count; i++)
                    {
                        _reportButtons[i].Attributes.Add("style", "display:none");
                    }
                }
                if (role.Publish)
                { }
                else
                {
                    for (int i = 0; i < _publishButtons.Count; i++)
                    {
                        _publishButtons[i].Attributes.Add("style", "display:none");
                    }
                }
            }
        }
        public virtual void AddColumn(DataGrid _grid)
        {
            MyWebControlLib.Columns columns = new MyWebControlLib.Columns();
            MyWebControlLib.Column column = null;
            //根据_configfile文件载入列信息
            XmlDocument doc = new XmlDocument();
            doc.Load(_configfile);
            XmlElement root = doc.DocumentElement;
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    if (root.ChildNodes[i].LocalName.ToLower() == "columns")
                    {
                        XmlNode columnsnode = root.ChildNodes[i];
                        for (int j = 0; j < columnsnode.ChildNodes.Count; j++)
                        {
                            column = new MyWebControlLib.Column();
                            if (columnsnode.ChildNodes[j].Attributes["name"] != null)
                                column.FieldName = columnsnode.ChildNodes[j].Attributes["name"].Value.ToString();
                            if (columnsnode.ChildNodes[j].Attributes["namecn"] != null)
                                column.Title = columnsnode.ChildNodes[j].Attributes["namecn"].Value.ToString();
                            if (columnsnode.ChildNodes[j].Attributes["showingrid"] != null)
                            {
                                if (columnsnode.ChildNodes[j].Attributes["showingrid"].Value.ToString().ToLower() == "true")
                                    column.ShowInGrid = true;
                                else
                                    column.ShowInGrid = false;
                            }

                            if (columnsnode.ChildNodes[j].Attributes["width"] != null)
                            {
                                column.Width = Convert.ToInt32(columnsnode.ChildNodes[j].Attributes["width"].Value.ToString());
                            }
                            columns.Add(column);
                        }
                    }
                }
            }
            if (columns.Count > 0)
            {
                //将列增加至表格中
                BoundColumn field = null;
                //BoundField field = null;
                for (int i = 0; i < columns.Count; i++)
                {
                    field = new BoundColumn();//gridview 的boundfield对应datagrid的boundcolumn
                    field.HeaderText = columns[i].Title;
                    field.DataField = columns[i].FieldName;
                    field.HeaderStyle.Width = columns[i].Width;
                    if (columns[i].ShowInGrid == false)
                    {
                        field.Visible = false;
                    }
                    field.HeaderStyle.Wrap = false;
                    field.ItemStyle.Wrap = false;
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                    _grid.Columns.Add(field);
                }
            }

        }
        public virtual void AddGridView(GridView _gridView)
        {
            _gridView.BorderColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_bordercolorlight"].ToString());
            _gridView.BackColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xgriderow_bgcolor"].ToString());

            _gridView.BorderWidth = Unit.Pixel(1);
            _gridView.CellPadding = 3;
            _gridView.CellSpacing = 0;
           
            _gridView.HeaderStyle.BackColor= ColorTranslator.FromHtml(base.Application[this.Session["Style"].ToString() + "xgridhead_bgcolor"].ToString());
            
            bool flag = true;
            //flag = false;//上海 单位编码改为序号
            if (!flag)
            {

                if (_gridView.Columns[0].HeaderText == "单位编码")
                {
                    _gridView.Columns[0].HeaderText = "序号";
                    _gridView.Columns[0].SortExpression = "";
                    _gridView.Columns[0].HeaderStyle.Font.Size=9;
                }
            }
        }
        public void GridView_RowDataBound(GridViewRowEventArgs e,int pageSize,int currentPage,string i)
        {

            string selectrow = e.Row.RowIndex.ToString();
            e.Row.Attributes.Add("onclick", "javascript:mainkey" + i + "('" + selectrow + "')");
            e.Row.Attributes.Add("onmouseover", "this.bgColor='D7F2EB';style.cursor = 'hand';");
            e.Row.Attributes.Add("onmouseout", "this.bgColor='" + Application[Session["Style"].ToString() + "xgriderow_bgcolor"] + "'");
            
            bool flag = true;
            //flag = false;//上海 单位编码改为序号
            if (!flag)
            {
                try
                {
                    if (e.Row.Cells[0].Text.Substring(0, 2) == "00")
                    {
                        int index = (currentPage - 1) * pageSize + e.Row.RowIndex + 1;
                        e.Row.Cells[0].Text = index.ToString();
                        
                    }
                }
                catch
                {
                }
               
            }
        }
        public void CloseWindow(CloseWindowType CloseType)
        {
            if (CloseType == CloseWindowType.OK)
                Response.Write("<script language='javascript'>var s = new Object();s.success='ok';window.returnValue=s;this.close();</script>");
            else
                Response.Write("<script language='javascript'>var s = new Object();s.success='cancel';window.returnValue=s;this.close();</script>");
        }
        public static string[] Splitstring(string SourceStr,string SplitStr)
        {
            string[] stringSeparators = new string[] { "" };
            stringSeparators[0] = SplitStr;
            string[] array = SourceStr.Split(stringSeparators,StringSplitOptions.None);
            return array;
        }

        public static void AddAttr(HtmlImage btn, string imgOver, string imgUp, string imgDown, string imgOut, string src, string onclick)
        {
            if (btn != null)
            {
                btn.Attributes.Add("onmouseover", "document." + btn.Attributes["name"] + ".src='" + imgOver + "';");
                btn.Attributes.Add("onmouseup", "document." + btn.Attributes["name"] + ".src='" + imgUp + "';");
                btn.Attributes.Add("onmousedown", "document." + btn.Attributes["name"] + ".src='" + imgDown + "';");
                btn.Attributes.Add("onmouseout", "document." + btn.Attributes["name"] + ".src='" + imgOut + "';");
                btn.Attributes.Add("src", src);
                btn.Attributes.Add("onclick", onclick + "();");
            }
        }
        public static void RemoveAttr(HtmlImage btn, string disable)
        {
            if (btn != null)
            {
                btn.Attributes.Remove("onmouseover");
                btn.Attributes.Remove("onmouseup");
                btn.Attributes.Remove("onmousedown");
                btn.Attributes.Remove("onmouseout");
                btn.Attributes.Remove("onclick");
                btn.Attributes.Add("src", disable);
            }
        }
        public string GetState(string state)
        {
            switch (state)
            {
                case "2":
                    return "完成";
                case "3":
                    return "审批不通过";
                case "4":
                    return "审批通过";
                case "5":
                    return "已上报";
                case "6":
                    return "已发布";
                case "1":
                    return "编辑";
                default:
                    return "";
            }
        }
    }
    public class HtmlImages : System.Collections.CollectionBase
    {
        public HtmlImage this[int index]
        {
            get
            {
                return ((HtmlImage)List[index]);
            }
            set
            {
                List[index] = value;
            }
        }
        public HtmlImages()
        {

        }
        public int Add(HtmlImage value)
        {
            return (List.Add(value));
        }

        public int IndexOf(HtmlImage value)
        {
            return (List.IndexOf(value));
        }
        public void Insert(int index, HtmlImage value)
        {
            List.Insert(index, value);
        }
        public void Remove(HtmlImage value)
        {
            List.Remove(value);
        }
        public bool Contains(HtmlImage value)
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
            //if (value.GetType()!=Type.GetType("HtmlImage"))
            //	throw new ArgumentException("value must be of type HtmlImage");
        }
    }
}
