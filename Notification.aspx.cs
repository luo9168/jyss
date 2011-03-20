using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using CommonClasses;
public partial class Notification1 :CommonClasses.MyPage
{
    DataTable GridSource;
    int total = 0;
    int currentPage = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        ModuleKey = "010101";
        ModuleName = "日常办公－通知通告";
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetCacheability(HttpCacheability.Public);
        Response.Cache.SetValidUntilExpires(true);
        AddButtons.Add(btnadd);
        EditButtons.Add(btnedit);
        DeleteButtons.Add(btndelete);
        //FinishButtons.Add(btnfinish);
        //ApproveButtons.Add(btnapprove);
        //PublishButtons.Add(btnpublish);

        if (!IsPostBack)
        {
            DDL_Year.Items.FindByValue(DateTime.Today.Year.ToString("0000")).Selected = true;
            DDL_Month.Items.FindByValue("").Selected = true;

            if (GridView1.Attributes["SortExpression"] == null)
            {
                ViewState["SortExpression"] = "chgdate";
                ViewState["SortDirection"] = "DESC";
            }
            currentPage = 1;
            GridBind();
            HF_CurUnitCode.Value = _user.UnitCode;
            GetLatestNews();
        }
        else//接收HF和session
        {
            try
            {                
                total = Int32.Parse(HF_total.Value);
                currentPage = Int32.Parse(HF_currentPage.Value);
                GridSource = (DataTable)Session["GridSource"];
            }
            catch
            {
                currentPage = 1;
                GridBind();//根据情况修改
            }
        }

    }
    protected void GetLatestNews()
    {
        
         
    }
    protected void GridBind()//第一次进入、查询、添加、删除和排序时使用
    {
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        
        note.ordername = ViewState["SortExpression"].ToString();
        note.orderMenthod = ViewState["SortDirection"].ToString();
        note.UnitCode = _user.UnitCode;
        note.DYear = DDL_Year.SelectedValue;
        note.DMonth = DDL_Month.SelectedValue;
        if (DDL_State.SelectedValue != "")
            note.State = int.Parse(DDL_State.SelectedValue);
        GridSource = note.getOriginDataTableWithPublish(GridView1.PageSize, currentPage);        
        if (GridSource.Rows.Count == 0 && currentPage > 1)
        {
            currentPage--;
            GridSource = note.getOriginDataTableWithPublish(GridView1.PageSize, currentPage);
        }
        total = note.total;
        GridView1.DataSource = GridSource;
        GridView1.DataKeyNames = new string[] { "tableid", "name", "state", "islocked", "unitcode" };
        GridView1.DataBind();
        GridView1.SelectedIndex = -1;
        HF_Key.Value = "";
        HF_State.Value = "";
        //给HF和Session赋值
        HF_total.Value = total.ToString();
        HF_currentPage.Value = currentPage.ToString();
        Session["GridSource"] = GridSource;
    }
    protected void GridBindFAPR()//完成、审批、发布和翻页时使用;修改时先调用updateGridsource再调用此方法
    {
        //Lb_result.Text = "";
        GridView1.DataSource = GridSource;
        GridView1.DataKeyNames = new string[] { "id", "name", "state", "islocked", "unitcode" };
        GridView1.DataBind();
        if (GridView1.SelectedIndex != -1)
            HF_State.Value = GridView1.DataKeys[GridView1.SelectedIndex].Values["state"].ToString();

    }
    protected void GridView1_DataBound(object sender, EventArgs e)
    {

        int pagenum = total / (GridView1.PageSize);
        if (GridView1.PageSize * pagenum < total)
            pagenum += 1;
        if (total == 0)
            panel2.Visible = false;
        else
            panel2.Visible = true;
        if (pagenum > 1)
        {
            linkBtnFirst.Enabled = true;
            linkBtnLast.Enabled = true;
        }
        if (currentPage >= pagenum)
            linkBtnNext.Enabled = false;
        else
            linkBtnNext.Enabled = true;
        if (currentPage > 1)
            linkBtnPrev.Enabled = true;
        else
            linkBtnPrev.Enabled = false;

        MessageLabel.Text = "共有" + total.ToString() + "条记录";

        linkBtnLast.CommandName = pagenum.ToString();
        linkBtnFirst.CommandName = "1";
        linkBtnNext.CommandName = Convert.ToString(currentPage + 1);
        linkBtnPrev.CommandName = Convert.ToString(currentPage - 1);
        CurrentPageLabel.Text = "第" + currentPage.ToString() +
              "/" + pagenum + "页";
        txt_page.Text = currentPage.ToString();
        TotalPages.Text = "/" + pagenum + "页";
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        currentPage = 1;
        if (ViewState["SortDirection"].ToString() == "ASC")
            ViewState["SortDirection"] = "DESC";
        else
            ViewState["SortDirection"] = "ASC";
        ViewState["SortExpression"] = e.SortExpression;
        GridBind();
    }
    //RowCreated 用来对某一字段排序后变换排序图片
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row != null && e.Row.RowType == DataControlRowType.Header)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                if (cell.HasControls())
                {
                    LinkButton button = cell.Controls[0] as LinkButton;
                    if (button != null)
                    {
                        Image image = new Image();
                        image.ImageUrl = "";
                        image.Visible = false;
                        string str = ViewState["SortExpression"].ToString();

                        if (str == button.CommandArgument)
                        {
                            image.Visible = true;
                            if (ViewState["SortDirection"].ToString() == "ASC")
                                image.ImageUrl = "images/arrowup.gif";
                            else
                                image.ImageUrl = "images/arrowdown.gif";
                        }
                        cell.Controls.Add(image);
                    }
                }
            }
        }

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
     
        string selectrow = e.Row.RowIndex.ToString();
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (total != 0)
            {
                string id = GridSource.Rows[Convert.ToInt32(selectrow)]["id"].ToString();
                e.Row.Attributes.Add("onclick", "javascript:mainkey('" + selectrow + "')");
                e.Row.Attributes.Add("OnDblClick", "javascript:DownContent('" + id + "')");
                e.Row.Attributes.Add("onmouseover", "this.bgColor='ccff00';style.cursor = 'hand';");
                e.Row.Attributes.Add("onmouseout", "this.bgColor='ffffff'");
            }
            else
            {
                Lb_result.Text = "当前没有记录！";
                
            }
        }

    }

    #region//翻页

    protected void PagerButtonClick(object sender, EventArgs e)
    {
        try
        {
            int pagenum = total / (GridView1.PageSize);
            if (GridView1.PageSize * pagenum < total)
                pagenum += 1;
            if (txt_page.Text == "0")
                txt_page.Text = "1";
            else if (int.Parse(txt_page.Text) > pagenum)
                txt_page.Text = pagenum.ToString();
            GridView1.SelectedIndex = -1;
            HF_Key.Value = "";
            HF_State.Value = "";
            linkBtnPage.CommandName = txt_page.Text;
            EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
            note.ordername = ViewState["SortExpression"].ToString();
            note.orderMenthod = ViewState["SortDirection"].ToString();
            note.UnitCode = _user.UnitCode;
            note.DYear = DDL_Year.SelectedValue;
            note.DMonth = DDL_Month.SelectedValue;
            if (DDL_State.SelectedValue != "")
                note.State = int.Parse(DDL_State.SelectedValue);
            currentPage = Convert.ToInt32(((LinkButton)sender).CommandName);
            GridSource = note.getOriginDataTableWithPublish(GridView1.PageSize, currentPage);
            GridBindFAPR();
            //给HF和Session赋值
            HF_total.Value = total.ToString();
            HF_currentPage.Value = currentPage.ToString();
            Session["GridSource"] = GridSource;
        }
        catch
        {
            Lb_result.Text = "输入页码非法！";
        }
    }

    #endregion

    protected void Lbtn_newsContent_Click(object sender, EventArgs e)
    {
        
        Lb_result.Text = "";
        EnterpriseOpration.Notification filePub = new EnterpriseOpration.Notification();
        filePub.ID = Convert.ToInt32(Request["__EVENTARGUMENT"]);
        DataTable dt = filePub.getOriginTableByID();
        if (dt.Rows[0]["thefile"] != System.DBNull.Value)
        {
            byte[] thefile = (byte[])dt.Rows[0]["thefile"];

            string fileName = dt.Rows[0]["name"].ToString() + "." + dt.Rows[0]["filetype"].ToString();
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            this.Response.ContentType = dt.Rows[0]["ContentType"].ToString();
            this.Response.AppendHeader("Content-Disposition", "attachment;filename = " + fileName);
            this.Response.OutputStream.Write(thefile, 0, thefile.Length);
            this.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }
        else
        {
            Lb_result.Text = "本记录没有文件！";
        }
    }
    protected void GetContent(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(((LinkButton)sender).CommandName.ToString());
       DownContent(id);
       

    }
    protected void Lbtn_Down_Click(object sender, EventArgs e)
    {
        string ss = Request["__EVENTARGUMENT"].ToString();
        int id = Convert.ToInt32(ss);
        DownContent(id);
    }
    public void DownContent(int ID)
    {
         Lb_result.Text = "";
        EnterpriseOpration.Notification filePub = new EnterpriseOpration.Notification();
        filePub.ID = ID;
        DataTable dt = filePub.getOriginTableByID();
        if (dt.Rows[0]["thefile"] != System.DBNull.Value)
        {
            byte[] thefile = (byte[])dt.Rows[0]["thefile"];

            string fileName = dt.Rows[0]["name"].ToString() + "." + dt.Rows[0]["filetype"].ToString();
            fileName = System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
            this.Response.ContentType = dt.Rows[0]["ContentType"].ToString();
            this.Response.AppendHeader("Content-Disposition", "attachment;filename = " + fileName);
            this.Response.OutputStream.Write(thefile, 0, thefile.Length);
            this.Response.OutputStream.Close();
            HttpContext.Current.Response.End();
        }
        else
        {
            Lb_result.Text = "本记录没有文件！";
            
            
        }
    }



    protected void Lbtn_query_Click(object sender, EventArgs e)
    {
        Lb_result.Text = "";

        currentPage = 1;
        GridBind();


    }

    protected void Lbtn_add_Click(object sender, EventArgs e)
    {
        if (Hidden_Data.Value == "0")
        {
            Lb_result.Text = "添加成功";
            currentPage = 1;
            GridBind();
        }
        else
            Lb_result.Text = "添加失败";
    }

    protected void Lbtn_edit_Click(object sender, EventArgs e)
    {
        if (Hidden_Data.Value == "0")
        {
            Lb_result.Text = "修改成功";
            UpdateGridSource();
            GridBindFAPR();
        }
        else
            Lb_result.Text = "修改失败";
    }

    protected void Lbtn_del_Click(object sender, EventArgs e)
    {
        int result = -1;
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        note.ID = Int32.Parse(HF_Key.Value);
        note.ChgPerson = User.Identity.Name;
        note.log_moduleName = ModuleName;
        note.log_userName = _user.UserName;
        result = note.Delete();
        if (result == 0)
        {
            GridBind();
            Lb_result.Text = "删除成功";
        }
        else
            Lb_result.Text = "删除失败";
    }

    protected void Lbtn_finish_Click(object sender, EventArgs e)
    {
        int result = -1;
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        note.ID = Int32.Parse(HF_Key.Value);
        note.ChgPerson = User.Identity.Name;
        result = note.Finish();
        if (result == 0)
        {
            //if (GridSource.Rows[GridView1.SelectedIndex]["ID"].ToString() == HF_Key.Value)
            //    {
            GridSource.Rows[GridView1.SelectedIndex]["state"] = "2";
            GridSource.Rows[GridView1.SelectedIndex]["chgDate"] = note.ChgDate;
            //    break;
            //}

            Lb_result.Text = "提交完毕";
            GridBindFAPR();

        }
        else
            Lb_result.Text = "提交完成失败";
    }

    protected void Lbtn_approve_Click(object sender, EventArgs e)
    {
        int result = -1;
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        note.ID = Int32.Parse(HF_Key.Value);
        note.ApprovePerson = User.Identity.Name;
        string app = Request["__EVENTARGUMENT"];
        if (app == "0")
            note.IsApproved = true;
        else
            note.IsApproved = false;
        note.log_moduleName = ModuleName;
        note.log_userName = _user.UserName;
        result = note.Approve();
        if (result == 0)
        {
            //for (int i = 0; i < GridSource.Rows.Count; i++)
            //{
            //    if (GridSource.Rows[i]["ID"].ToString() == HF_Key.Value)
            //    {
            if (app == "0")
                GridSource.Rows[GridView1.SelectedIndex]["state"] = "4";
            else
                GridSource.Rows[GridView1.SelectedIndex]["state"] = "3";
            GridSource.Rows[GridView1.SelectedIndex]["chgDate"] = note.ChgDate;
            //break;
            //    }
            //}            
            GridBindFAPR();
            
            Lb_result.Text = "审批完毕";


        }
        else
            Lb_result.Text = "审批失败";
    }

    protected void Lbtn_publish_Click(object sender, EventArgs e)
    {
        int result = -1;
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        note.ID = Int32.Parse(HF_Key.Value);
        note.ChgPerson = User.Identity.Name;
        string state = Request["__EVENTARGUMENT"];
        result = note.Publish(state);
        if (result == 0)
        {
            //for (int i = 0; i < GridSource.Rows.Count; i++)
            //{
            //    if (GridSource.Rows[i]["ID"].ToString() == HF_Key.Value)
            //    {
            GridSource.Rows[GridView1.SelectedIndex]["state"] = state;
            GridSource.Rows[GridView1.SelectedIndex]["chgDate"] = note.ChgDate;
            //        break;
            //    }
            //}
            GridBindFAPR();
            if (state == "6")
                Lb_result.Text = "发布成功";
            else
                Lb_result.Text = "取消发布成功";
        }
        else
        {
            if (state == "6")
                Lb_result.Text = "发布失败";
            else
                Lb_result.Text = "取消发布失败";
        }
    }

    protected void Lbtn_gridclick_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(Request["__EVENTARGUMENT"]);
        GridView1.SelectedIndex = id;
        HF_Key.Value = GridView1.DataKeys[id].Value.ToString();
        HF_State.Value = GridView1.DataKeys[id].Values[2].ToString();
        HF_IsLocked.Value = GridView1.DataKeys[id].Values[3].ToString();
        HF_UnitCode.Value = GridView1.DataKeys[id].Values[4].ToString();
        Lb_result.Text = "";
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
    public void UpdateGridSource()
    {
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        note.ID = Int32.Parse(HF_Key.Value);
        DataTable dt = note.getCoulmnNamesByID();
        int j;
        if (dt.Rows.Count > 0)
        {
            //for (i = 0; i < GridSource.Rows.Count; i++)
            //{
            //    if (GridSource.Rows[i]["ID"].ToString() == dt.Rows[0]["ID"].ToString())
            //    {
            for (j = 0; j < dt.Columns.Count; j++)
            {
                GridSource.Rows[GridView1.SelectedIndex][j] = dt.Rows[0][j];
            }

            //        break;
            //    }
            //}
        }
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        //ScriptManager1.RegisterPostBackControl(Lbtn_newsContent);
        for (int i = 0; i < GridView1.Rows.Count; i++)
            ScriptManager1.RegisterPostBackControl(GridView1.Rows[i].FindControl("LinkButton1"));
    }
}
