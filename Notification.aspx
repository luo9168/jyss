<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notification.aspx.cs" Inherits="Notification1" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
</head>
<body style="margin-left:0; margin-top:0;"  onload=initMove()>
    <form id="form1" runat="server">
    
   
     <asp:ScriptManager ID="ScriptManager1" runat="server" >
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="conditional" >
      <Triggers> <asp:PostBackTrigger ControlID="Lbtn_newsContent" /><asp:PostBackTrigger ControlID="Lbtn_Down" /></Triggers>
        <ContentTemplate>
    <div id="latestNews"  style="display:none"  onmouseover="javascript:setFlag(true);" onmouseleave="javascript:setFlag(false);" style="  position:absolute;z-index:1;left:280px;top:60px; width:328px; height:220px;" >    
    <div style="background-image:url(images/newsBack.jpg); background-repeat:no-repeat; background-color:White" >
    
    <div style=" position:relative; left:3%;top:7px; width:94%; height:100%;">
    <table  width="100%"><tr><td width="20%"></td><td width="80%"><font size="4" color="#cc0033">考核评比情况</font></td><td  align="right" valign="top"><img src="images/bbCancel2.jpg"  style=" position:relative; " alt="关闭" onmouseover="this.style.cursor='hand'"  onclick="javascript:newsClose();"/></td></tr><tr style="height:20px"><td colspan="2"></td></tr></table>
    <asp:Label runat="server" id="newsContent" Font-Size="medium"></asp:Label>
    <asp:LinkButton ID="Lbtn_newsContent" runat="server" OnClick="Lbtn_newsContent_Click"></asp:LinkButton>
    <asp:LinkButton ID="LinkButton2" runat="server" OnClick="Lbtn_newsContent_Click"></asp:LinkButton>
    </div></div>
    </div>
     </ContentTemplate>
   </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="conditional" >
      
        <ContentTemplate>
    <table style="WIDTH: 100%; HEIGHT: 10% " cellspacing="0" cellpadding="0" align="center" border="0">
	  <tbody>
		<tr>
		   <td style="WIDTH: 100%; HEIGHT: 8px" valign="top">
			  <table style="WIDTH: 100%; HEIGHT: 13px; " cellspacing="0"
				cellpadding="0" border="0">
				<tbody>
				  <tr>			
				   <td colspan="3"  bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' style="height: 25px; background-image:url(images\contentBack.jpg);  background-repeat:no-repeat;  font-weight: bold;color:<%=Application[Session["Style"].ToString()+"xFont_color"]%>; text-align:left;">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        通知通告</td>		
						
					<td style="background-color: transparent; height: 25px; background-attachment: scroll; background-repeat: repeat; text-align: right;">
					    <table cellspacing="0" cellpadding="0" >
                            <tbody>
                                <tr style="width: 100%">
			                	 
                                    <td style="height: 27px">
                       <img alt="" onmouseup="javascript:document.btnadd.src='images/btn_add_on.gif';"
                                onmousedown="javascript:document.btnadd.src='images/btn_add_down.gif';"
                                id="btnadd" runat="server"
                                onmouseover="javascript:document.btnadd.src='images/btn_add_on.gif';"
                                onclick="javascript:add();"
                                onmouseout="javascript:document.btnadd.src='images/btn_add.gif';"                        
                                src="images/btn_add.gif"
                                name="btnadd" /></td>
                                    <td style="height: 27px">                      
                       <img alt="" onmouseup="javascript:document.btnedit.src='images/btn_edit_on.gif';"
                                onmousedown="javascript:document.btnedit.src='images/btn_edit_down.gif';"
                                id="btnedit" runat="server"
                                onmouseover="javascript:document.btnedit.src='images/btn_edit_on.gif';"
                                onclick="javascript:update();"
                                onmouseout="javascript:document.btnedit.src='images/btn_edit.gif';"                        
                                src="images/btn_edit.gif"
                                name="btnedit"/></td>
                                    <td style="height: 27px">                  
                       <img alt="" onmouseup="javascript:document.btndelete.src='images/btn_delete_on.gif';"
                                onmousedown="javascript:document.btndelete.src='images/btn_delete_down.gif';"
                                id="btndelete" runat="server"
                                onmouseover="javascript:document.btndelete.src='images/btn_delete_on.gif';"
                                onclick="javascript:del();"
                                onmouseout="javascript:document.btndelete.src='images/btn_delete.gif';"                        
                                src="images/btn_delete.gif"
                                name="btndelete" /></td>
                                  <td style="height: 27px">
                       <img alt="" src="images/btn_finish.gif" 
                                onmouseup="javascript:document.btnfinish.src='images/btn_finish_on.gif';"
                                onmousedown="javascript:document.btnfinish.src='images/btn_finish_down.gif';"
                                id="btnfinish" runat="server"
                                onmouseover="javascript:document.btnfinish.src='images/btn_finish_on.gif';"
                                onclick="javascript:finish();"
                                onmouseout="javascript:document.btnfinish.src='images/btn_finish.gif';"
                                name="btnfinish" 
                       /></td>
                              
                                    <td style="height: 27px;">
                       <img alt="" src="images/btn_publish.gif" 
                                onmouseup="javascript:document.btnpublish.src='images/btn_publish_on.gif';"
                                onmousedown="javascript:document.btnpublish.src='images/btn_publish_down.gif';"
                                id="btnpublish" runat="server"
                                onmouseover="javascript:document.btnpublish.src='images/btn_publish_on.gif';"
                                onclick="javascript:publish();"
                                onmouseout="javascript:document.btnpublish.src='images/btn_publish.gif';"
                                name="btnpublish" 
                       /></td>                                
                    
                                </tr>
			                </tbody>
			            </table>
					</td>
				  </tr>
				</tbody>
			  </table>
			  </td>
			  </tr>
			  <tr style="background-color: #999999; height: 1px">
                        <td style="width: 1013px; height: 1px;">
                        </td>
                    </tr>
			    <tr style="width: 100%; height: 30px">
                        <td style="height: 30px; width: 100%;">
                            <table cellspacing="0" cellpadding="0" style="width:100%">
                                <tbody>
                                    <tr style="width: 100%; height:30px;">
                                        <td  colspan="3">
                                            &nbsp; 时间：
                                            <asp:DropDownList ID="DDL_Year" runat="server" >
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>2005</asp:ListItem>
                                                <asp:ListItem>2006</asp:ListItem>
                                                <asp:ListItem>2007</asp:ListItem>
                                                <asp:ListItem>2008</asp:ListItem>
                                                <asp:ListItem>2009</asp:ListItem>
                                                <asp:ListItem>2010</asp:ListItem>
                                                <asp:ListItem>2011</asp:ListItem>
                                                <asp:ListItem>2012</asp:ListItem>                                                
                                            </asp:DropDownList>年
                                            <asp:DropDownList ID="DDL_Month" runat="server" >
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                            </asp:DropDownList>月
                                            状态：
                                            <asp:DropDownList ID="DDL_State" runat="server" >
                                                <asp:ListItem></asp:ListItem>
                                                <asp:ListItem Value="1">编辑</asp:ListItem>
                                                <asp:ListItem Value="2">完成</asp:ListItem>                                              
                                                <asp:ListItem Value="6">已发布</asp:ListItem>
                                            </asp:DropDownList>
                                            <img alt="查询" onmouseup="javascript:document.btnquery.src='images/btn_query_on.gif';"
                                                onmousedown="javascript:document.btnquery.src='images/btn_query_down.gif';" onmouseover="javascript:document.btnquery.src='images/btn_query_on.gif';"
                                                onclick="javascript:queryItem();" onmouseout="javascript:document.btnquery.src='images/btn_query.gif';"
                                                name="btnquery" src="images/btn_query.gif" style="border-style: none" id="btnquery" />
                                        </td>
                                    </tr>
                                    <tr style="background-color: #999999; height: 1px">
                                        <td colspan="3" style="height: 1px;">
                                        </td>
                                    </tr>
                                    </tbody></table>
                                        	<asp:UpdatePanel runat="server" ID="lb"><ContentTemplate>
                                        	<table>                                        	
                                    <tr style="width: 100%">
			                	 <td style="height: 27px">                     
                       <img alt="" runat="server"
                                onmouseup="javascript:document.btnbrowse.src='images/btn_browse_on.jpg';"
                                onmousedown="javascript:document.btnbrowse.src='images/btn_browse_down.jpg';"
                                id="btnbrowse"
                                onmouseover="javascript:document.btnbrowse.src='images/btn_browse_on.jpg';"
                                onclick="javascript:browse();"
                                onmouseout="javascript:document.btnbrowse.src='images/btn_browse.jpg';"                        
                                src="images/btn_browse.jpg"
                                name="btnbrowse"
                                 />&nbsp;</td><td align="center" ></td><td style="width:90%" align="right"><asp:Label ID="Lb_result"   runat="server"  ForeColor="Red" Text=""></asp:Label></td>                  
                    
                    </tr>
			           
			        </table>	
			        </td>
			    </tr>
			    
		</table>
                                        	</ContentTemplate></asp:UpdatePanel>
                                        	</ContentTemplate></asp:UpdatePanel>
              <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
              <Triggers  ></Triggers>
              <ContentTemplate>    
		
			<asp:Panel ID="panel1" runat="server" Style="overflow: scroll; position: absolute;"  Width="100%" Height="84%">
			
                    <asp:GridView ID="GridView1"  runat="server" style="position:relative; left:0px; top:0px;"
                           Width="100%" Font-Names="宋体" BorderWidth="1px" BorderColor="DimGray" CellPadding="4" EmptyDataText="对不起，没有符合条件的记录"
                           AutoGenerateColumns="False"   OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound" HorizontalAlign="Center" 
                           AllowPaging="True" PageSize="17"  AllowSorting="True" OnSorting="GridView1_Sorting"  OnPreRender="GridView1_PreRender"
                           OnRowCreated="GridView1_RowCreated" >                             
                            <Columns>
                                        <asp:BoundField DataField="name" HeaderText="通知名称" SortExpression="name" >
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="unitName" HeaderText="发布单位" SortExpression="unitName" >
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="chgdate" HeaderText="时间" SortExpression="chgdate">
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:BoundField>                                                                                                                            
                                        <asp:TemplateField HeaderText="状态" SortExpression="state">
                                            <ItemTemplate>
                                                <asp:Label ID="Lb_state" runat="server" Text='<%# GetState(Eval("state").ToString())  %>' ></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" />
                                            <HeaderStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="通知内容" SortExpression="id">
                                            <ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" CommandName='<%# Eval("tableid") %>' OnClick="GetContent" ><%# Eval("tableid").ToString()!="" ? "查看" : ""%></asp:LinkButton></ItemTemplate>
                                            <ItemStyle Wrap="False"  />
                                            <HeaderStyle Wrap="False" />
                                        </asp:TemplateField>
                                        
                                    </Columns>                    
                                    
                  </asp:GridView>                    
                     <asp:Panel ID="panel2" runat="server"  ScrollBars="None"  >
                    <asp:Label ID="MessageLabel" ForeColor="Blue" runat="server" />&nbsp; &nbsp;<asp:Label ID="CurrentPageLabel" ForeColor="Blue" runat="server" />&nbsp;
                                            <asp:LinkButton CommandName="Page" CommandArgument="First" ID="linkBtnFirst" runat="server" OnClick="PagerButtonClick">首页</asp:LinkButton>&nbsp;
                                            <asp:LinkButton CommandName="Page" CommandArgument="Prev" ID="linkBtnPrev" runat="server" OnClick="PagerButtonClick">上一页</asp:LinkButton>&nbsp;
                                            <asp:LinkButton CommandName="Page" CommandArgument="Next" ID="linkBtnNext" runat="server" OnClick="PagerButtonClick">下一页</asp:LinkButton>&nbsp;
                                            <asp:LinkButton CommandName="Page" CommandArgument="Last" ID="linkBtnLast" runat="server" OnClick="PagerButtonClick">末页</asp:LinkButton>
                                            <asp:Label ID="Label1" ForeColor="Blue" Text="跳转至第"  runat="server" />
                                            <asp:TextBox id="txt_page" runat="server" Width="34px" Text="<%#GridView1.PageIndex+1 %>"></asp:TextBox>                                                       
                                            <asp:Label ID="TotalPages" ForeColor="Blue" runat="server" />
                                            <asp:LinkButton CommandName="Page" CommandArgument="-1" ID="linkBtnPage" runat="server" OnClick="PagerButtonClick">Go</asp:LinkButton>
                                            <asp:RegularExpressionValidator runat="server" ID="regular" ControlToValidate="txt_page" ValidationExpression="\d*" ErrorMessage="页码请输入数字！">
                                            </asp:RegularExpressionValidator>                       
                </asp:Panel>                 
                </asp:Panel>
                
           
                    <asp:HiddenField ID="HF_Key" runat="server" />
                    <asp:HiddenField ID="HF_UnitCode" runat="server" />
                    <asp:HiddenField ID="HF_CurUnitCode" runat="server" />
                    <asp:HiddenField ID="HF_username" runat="server" />
                    <asp:HiddenField ID="HF_State" runat="server" />
                    <asp:HiddenField ID="HF_IsLocked" runat="server" />
                    <asp:HiddenField ID="HF_SelectIndex" runat="server" />
                    <asp:LinkButton ID="Lbtn_gridclick" runat="server" OnClick="Lbtn_gridclick_Click"></asp:LinkButton><asp:LinkButton ID="Lbtn_add" runat="server" OnClick="Lbtn_add_Click" ></asp:LinkButton><asp:LinkButton ID="lbtn_edit" runat="server" OnClick="Lbtn_edit_Click" ></asp:LinkButton><asp:LinkButton ID="Lbtn_del" runat="server" OnClick="Lbtn_del_Click" ></asp:LinkButton><asp:LinkButton ID="Lbtn_finish" runat="server" OnClick="Lbtn_finish_Click"></asp:LinkButton><asp:LinkButton ID="Lbtn_approve" runat="server" OnClick="Lbtn_approve_Click"></asp:LinkButton><asp:LinkButton ID="Lbtn_publish" runat="server" OnClick="Lbtn_publish_Click"></asp:LinkButton><asp:HiddenField  ID="Hidden_Data" runat="server" /> 
                    <asp:LinkButton ID="Lbtn_query" runat="server" OnClick="Lbtn_query_Click"></asp:LinkButton>
                     <asp:LinkButton ID="Lbtn_Down" runat="server" OnClick="Lbtn_Down_Click"></asp:LinkButton>
                    <asp:HiddenField ID="HF_total" runat="server" />
                    <asp:HiddenField ID="HF_currentPage" runat="server" />
                    
     </ContentTemplate> 
                   
                      </asp:UpdatePanel>  
                      
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
var flag=false;
var x1,x2,x3,y1,y2,y3;
var stepx,stepy;
stepx=5;
stepy=3;
x1=0;x2=280;x3=600;
y1=60;y2=200;y3=500;
var left,top;
left=x2;
top=y1;
var intervalid;
function initMove()
{
     latestNews.style.left=left;
     latestNews.style.top=top;
    intervalid=setInterval("move();", 200) ;     
}


function move()
{
    
    if(!flag)
    {
        
       
        xx=left;yy=top;
        if(xx<=x2 )
        {
            if(yy<=y2)
            {
                xx=xx+stepx;
                yy=yy-stepy;
                
            }
            else
            {
                xx=xx-stepx;
                yy=yy-stepy;
            }
        }
        else
        {
             if(yy<=y2)
            {
                xx=xx+stepx;
                yy=yy+stepy;
                
            }
            else
            {
                xx=xx-stepx;
                yy=yy+stepy;
            }
        }
        if(xx<x1)
            xx=x1;
        if(yy<y1)
            yy=y1;
        left=xx;top=yy;
        latestNews.style.left=xx;
        latestNews.style.top=yy;
    }
    
}
function setFlag(po)
{
    flag=po;
}
function newsClick(id,unitname,year)
{  
    var ID=id;
    var sum;
    var s=window.showModalDialog('Assess_View.aspx?unitcode='+ID+'&unitname='+encodeURI(unitname,'GB2312')+'&yy='+year+'&sum=0',null,'dialogWidth:985px;dialogHeight:520px;scroll:yes;center:yes;resizable:yes;toolbar:no;status:no');
}
function newsClose()
{
    clearInterval(intervalid);
    latestNews.style.display="none";
    
}

function mainkey(row)
{

setTimeout("testt("+row+")",300);
    
}
function testt(row)
{
__doPostBack('Lbtn_gridclick',row);
}
function DownContent(row) 
{ 
__doPostBack('Lbtn_Down',row);
}
function select()
{
    if(document.all.HF_Key.value=="")
        return false;
    else
        return true;
}

function TopPublished()
{
    if(document.all.HF_CurUnitCode.value==document.all.HF_UnitCode.value)
        return false;
    else
    {
        alert('上级单位发布的通知只能浏览！');
        return true;
    }
}

function queryItem()
{
    __doPostBack('Lbtn_query','');
}

function browse()
{
   if(select())
   { 
       var ID=document.form1.elements['HF_Key'].value;
       var s=window.showModalDialog('Notification_View.aspx?id='+ID+'&code=browse' ,null,'dialogWidth:685px;dialogHeight:490px;scroll:no;center:yes;resizable:no;toolbar:no;status:no');
    }
    else
    {
         alert("请先选择要浏览的记录！");
    }
}

function add()
{
 if(document.all.DDL_Year.value=="")
    {
        alert('请先选择年份！');
        return false;
    }
    if(document.all.DDL_Month.value=="")
    {
        alert('请选择月份！');
        return false;
    }
    var ID=document.form1.elements['HF_Key'].value;
    var s=window.showModalDialog('Notification_Edit.aspx?id='+ID+'&code=add&year='+document.all.DDL_Year.value+'&month='+document.all.DDL_Month.value,null,'dialogWidth:570px;dialogHeight:410px;scroll:no;center:yes;resizable:no;toolbar:no;status:no');
    if(s.success==true)
        {
           document.form1.elements['Hidden_Data'].value=s.data;
           __doPostBack('Lbtn_add','');
        }
}

function update()
{
    if (select())
    { 
        if(!TopPublished())
        {
            if(document.all.HF_State.value=='4')
                alert('该通告已通过审批，若要修改请先审批为不通过！');
            else if(document.all.HF_State.value=='6')
                alert('该通告已发布，不能再修改！');
            else
            {
                var ID=document.form1.elements['HF_Key'].value;
                var s=window.showModalDialog('Notification_Edit.aspx?id='+ID+'&code=edit&year='+document.all.DDL_Year.value+'&month='+document.all.DDL_Month.value,null,'dialogWidth:570px;dialogHeight:410px;scroll:yes;center:yes;resizable:no;toolbar:yes;status:yes');
                
                if (s.success==true)
                {
                    document.form1.elements['Hidden_Data'].value=s.data;
                    __doPostBack('Lbtn_edit','');              
                }
            }
        }
    }
    else
    {
       alert("请先选择要修改的记录！");
    }
}

function del()
{
    if(select())
    {
        if(!TopPublished())
        {
            if(document.all.HF_State.value!='1')
                alert('该通告已完成，不能删除！');          
            else if (window.confirm("确定删除此条记录？"))
                __doPostBack('Lbtn_del','');               
        }
    }
    else
    {
        alert("请先选择要删除的记录！");
    }
}

function finish()
{
    if(select())
    {
        if(!TopPublished())
        {
            if(document.all.HF_State.value=='1')
            {
                if(confirm('确定完成提交该记录？'))
                    __doPostBack('Lbtn_finish','');
            }
            else 
                alert('该通告已完成！');          
        }
    }
    else
    {
        alert("请先选择要完成的记录！");
    }
}

function approve()
{
    if(select())
    { 
        if(!TopPublished())
        { 
            if(document.all.HF_State.value=='1') 
                alert('该通告尚未完成，不能审批！'); 
            else if(document.all.HF_State.value=='6')
                alert('该通告已审批并发布！');
            else if(document.all.HF_State.value=='4')    
            {
                if(window.confirm('确定审批为不通过？'))
                    __doPostBack('Lbtn_approve','1');
            }
            else
            {
                if(window.confirm('审批同意？'))
                    __doPostBack('Lbtn_approve','0');
                else
                    __doPostBack('Lbtn_approve','1');
            }
        }
    }
    else
    {
        alert("请先选择要审批的记录！");
    }
}

function publish()
{
    if(select())
    {
        if(!TopPublished())
        {
            if(document.all.HF_State.value!='4')
            {
                if(document.all.HF_State.value=='6')
                {
                    if(window.confirm('确定要取消发布该记录？'))
                        __doPostBack('Lbtn_publish','4');
                }
                else
                    alert('该通告未审批通过，不能发布！');
            }
            else
            {
                if(window.confirm('确定要发布该记录？'))
                    __doPostBack('Lbtn_publish','6');
            }
        }
    }
    else
    {
        alert("请先选择要发布的记录！");
    }
}
</script>

