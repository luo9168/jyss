<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notification_Edit.aspx.cs"
    Inherits="Notification_Edit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新增通知通告</title>
</head>
<base target="_self" />
<body>
     <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" Style="left: 30px; position: relative; top: 20px;">
            <table border="0" cellpadding="0" cellspacing="0">
                <caption ><font color="black" style="font-family: 楷体_GB2312">通知通告</font></caption>
                <tr>
                    <td width="15" style="background-image: url(images/title/11.gif); ">
                    </td>
                    <td width="470" bgcolor="#ece9d8" style="font-weight: bolder; color: white; " align="center">&nbsp;
                    </td>
                    <td width="15" style="background-image: url(images/title/12.gif); ">
                    </td>
                </tr>
                <tr>
                    <td colspan="3" >
                    
                        <table width="500" border="1" cellspacing="0" cellpadding="0"  rules="none" bordercolor="#ece9d8" style="height: 180px">
                        <tr>
                           <td colspan="2" style="height:30px"></td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 38px;" align="right">
                                &nbsp;通知通告名称：</td>
                            <td style="width: 219px; height: 38px;">
                                <input id="TB_name" type="text" runat="server"  maxlength="100" style="width: 360px" /></td>
                        </tr>
                        <tr>
                            <td style="width: 120px; height: 37px;" align="right">
                                &nbsp;请上传文件：</td>
                            <td style="width: 219px; height: 37px;">
                                <input id="File1" accept="application/msword" type="file" runat="server" style="width: 360px" /></td>
                        </tr>
                        <tr>
                            <td style="height: 98px; width: 120px;" align="right">
                                &nbsp;备注：
                            </td>
                            <td style="height: 98px; width: 219px;">
                                <textarea id="TA_Memo" runat="server" style="width: 360px; height: 89px"></textarea>
                            </td>
                        </tr>
                        <tr>
                           <td colspan="2" style="height:30px"></td>
                        </tr>
                    </table>
                    </td>
                </tr>
                <tr>
                    <td><img src="images/title/21.gif" />
                    </td>
                    <td bgcolor="#ece9d8">
                    </td>
                    <td><img src="images/title/22.gif" />
                    </td>
                </tr>                
            </table>
           
        </asp:Panel>
    <br />
    <br />
    <br />
    <div align="right">
        <img id="btnok1"  name="btnok1" 
                                 runat="server" alt="" onclick="ok()" 
                                 onmousedown="javascript:document.btnok1.src='images/btn_ok_down.gif';" 
                                 onmouseout="javascript:document.btnok1.src='images/btn_ok.gif';" 
                                 onmouseover="javascript:document.btnok1.src='images/btn_ok_on.gif';" 
                                 onmouseup="javascript:document.btnok1.src='images/btn_ok_on.gif';" src="images/btn_ok.gif" />
            &nbsp; &nbsp; &nbsp;
                                 
                <img id="btncancel1" name="btncancel1"                                
                                runat="server" alt="" onclick="cancel()" 
                                onmousedown="javascript:document.btncancel1.src='images/btn_cancel_down.gif';" 
                                onmouseout="javascript:document.btncancel1.src='images/btn_cancel.gif';" 
                                onmouseover="javascript:document.btncancel1.src='images/btn_cancel_on.gif';" 
                                onmouseup="javascript:document.btncancel1.src='images/btn_cancel_on.gif';" src="images/btn_cancel.gif" />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div> 
        <div>
        <div>
            <asp:Label ID="LB_File" runat="server"></asp:Label>
            <asp:HiddenField ID="HF_Operate" runat="server" />
            <asp:HiddenField ID="HF_ID" runat="server" />
            <asp:LinkButton ID="Lbtn_OK" runat="server" OnClick="Lbtn_OK_Click"></asp:LinkButton>
            <asp:LinkButton ID="Lbtn_Cancel" runat="server" OnClick="Lbtn_Cancel_Click"></asp:LinkButton>
        </div>
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">



function ok()
{
    if(document.all.TB_name.value=="")
    {
        alert("通知通告名称不能为空！");
        document.all.TB_name.focus();
        return false;
    }
    var memo=document.form1.elements['TA_Memo'].value;    
    if(memo.replace(/[^\x00-\xff]/g,"**").length>1000)
    {
        alert("备注超过了规定长度500字");
        return false;
     }
    __doPostBack('Lbtn_OK','');
}

function cancel()
{
    window.close();
}
</script>

