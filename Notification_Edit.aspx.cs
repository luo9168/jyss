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
using System.IO;


public partial class Notification_Edit :CommonClasses.MyPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ModuleKey = "010101";
        ModuleName = "日常办公－通知通告";
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetCacheability(HttpCacheability.Public);
        Response.Cache.SetValidUntilExpires(true);
        if (!IsPostBack)
        {
            string operate = Request["code"].ToString();
            HF_Operate.Value = operate;
            if (operate != "add")
            {
                string id = Request["id"].ToString();
                HF_ID.Value = id;
                EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
                note.ID = Int32.Parse(id);
                DataTable table = note.getOriginTableByID();
                TB_name.Value = table.Rows[0]["name"].ToString();
                TA_Memo.Value = table.Rows[0]["memo"].ToString();
                this.Title = "修改通知通告";                
            }
        }
    }

    protected void Lbtn_OK_Click(object sender, EventArgs e)
    {
        EnterpriseOpration.Notification note = new EnterpriseOpration.Notification();
        int result = -1;
        note.Name = TB_name.Value;
        note.UnitCode = _user.UnitCode;
        note.IsApproved = false;
        //note.Memo = "";
        try
        {
            Stream myStream = File1.PostedFile.InputStream;
            int imgDataLen = File1.PostedFile.ContentLength;
            byte[] imgData = new byte[imgDataLen];
            myStream.Read(imgData, 0, imgDataLen);
            note.File = imgData;

            note.ContentType = File1.PostedFile.ContentType;
            string fielName = File1.PostedFile.FileName;
            int i = fielName.LastIndexOf('.');
            note.FileType = fielName.Substring(i + 1, fielName.Length - i - 1);
        }
        catch
        {
        }
        note.log_moduleName = ModuleName;
        note.log_userName = _user.UserName;
        note.Memo = TA_Memo.Value;
        if (HF_Operate.Value == "add")
        {
            note.AddPerson = User.Identity.Name;
            note.DYear = Request["year"].ToString();
            note.DMonth = Request["month"].ToString();
            result = note.Insert();
        }
        if (HF_Operate.Value == "edit")
        {
            note.ChgPerson = User.Identity.Name;
            note.ID = Int32.Parse(HF_ID.Value);
            result = note.Update();
        }
        Response.Write("<script language='javascript'>javascript:var s=new Object();s.success=true;s.data='"+result+"';window.returnValue=s;self.close();</script>");
    }

    protected void Lbtn_Cancel_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>javascript:var s=new Object();s.success=false;window.returnValue=s;self.close();</script>");
    }
}
