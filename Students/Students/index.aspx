﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Students.index" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblError" runat="server"></asp:Label>
    <asp:Label ID="lblMyHid" runat="server" Visible="False"></asp:Label>
    <script runat="server">
    protected void odsAllStudents_Filtering(object sender, ObjectDataSourceFilteringEventArgs e)
    {
        //Response.Write("Filtering:</br>  ");
        //foreach (var c in e.ParameterValues.Keys)
        //{
        //    if((e.ParameterValues[c.ToString()])!=null)
        //        Response.Write("[" + c.ToString() + "]=" + e.ParameterValues[c.ToString()].ToString() + ";");
        //}
        
        //{
        //    if (e.ParameterValues[0] != null)
        //        Response.Write("!!!" + e.ParameterValues[0] + "!!!");
        //}
    }
    </script>

    <asp:ObjectDataSource ID="odsAllStudents" runat="server" 
            DataObjectTypeName="Students.StudentDetails" 
            SelectMethod="GetStudents" 
            DeleteMethod="DeleteStudent" 
            InsertMethod="InsertStudent" 
            UpdateMethod="UpdateStudent" 
            FilterExpression="FirstName LIKE '{0}%' and SecondName LIKE '{1}%' "
            OnFiltering="odsAllStudents_Filtering">
            <filterparameters>
                <asp:FormParameter name="FirstName"  FormField="txtFilterByFirstName" DefaultValue="*"/>
                <asp:FormParameter Name="SecondName" FormField="txtFilterBySecondName" DefaultValue="*"/>
                <asp:FormParameter Name="DateOfBirthFrom" FormField="txtFilterByDateOfBirthFrom" DefaultValue="0" />
            </filterparameters>
    </asp:ObjectDataSource>
    
    <asp:Panel runat="server" HorizontalAlign="Right" style="margin-left: 761px">
    <asp:Label ID="lblDataSource" runat="server" Font-Size="Large" Text="источник данных:"></asp:Label>
    <asp:DropDownList ID="ddlListDB" runat="server" OnSelectedIndexChanged="ddlListDB_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem Value="SQL" >SQL</asp:ListItem>
        <asp:ListItem Value="XML" Selected="True">XML</asp:ListItem>
        <asp:ListItem Value="JSON">JSON</asp:ListItem>
    </asp:DropDownList>
    </asp:Panel>

    <asp:ObjectDataSource ID="odsGetOneStudent" runat="server" 
            DataObjectTypeName="Students.StudentDetails" 
            SelectMethod="GetStudent" 
            >
    </asp:ObjectDataSource>
    
    <br />
    
    <asp:DetailsView ID="dvInsertStudent" runat="server" DataSourceID="odsAllStudents" AutoGenerateRows="false"
        DefaultMode="Insert" OnItemInserting="dvInsertStudent_OnItemInserting" 
            >
        <FieldHeaderStyle BackColor="#DC143C" Font-Size="Large" Font-Bold="true" ForeColor="White" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <Fields>
            
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="SecondName" HeaderText="Second Name" />
            <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" />
            <asp:TemplateField>
                <HeaderTemplate><asp:Label ID="lblTemp" Text="Foto" runat="server"></asp:Label></HeaderTemplate>
                <ItemTemplate><asp:FileUpload ID="uploadFoto" runat="server" /></ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowInsertButton="true" ShowCancelButton="false" ButtonType="Button" InsertText="Добавить нового студента" />
        </Fields>
    </asp:DetailsView>
<!-- __________________________________________________________________________________________________________ -->  
        <br />
        <asp:Label ID="lblFilterBy" runat="server" Text="фильтровать по:"></asp:Label>
        <br />
        <asp:Label ID="lblFilterByFirstName" runat="server" Text="имя"></asp:Label>
        <asp:TextBox ID="txtFilterByFirstName" AutoPostBack="true" runat="server" ></asp:TextBox>
        <asp:Label ID="lblFilterBySecondName" runat="server" Text="фамилия"></asp:Label>
        <asp:TextBox ID="txtFilterBySecondName" AutoPostBack="true" runat="server"></asp:TextBox> 
        <br />
        
<!-- __________________________________________________________________________________________________________ -->  
  
  <div style="float:left">
  <asp:GridView id="gvAllStudents" runat="server" DataSourceID="odsAllStudents" DataKeyNames="StudentID,Foto" 
        AllowSorting="true" AutoGenerateColumns="false" AutoGenerateEditButton="false" 
        AutoGenerateDeleteButton="false" EnablePersistedSelection="true" 
        BorderWidth="1px"  Font-Size="Medium" ForeColor="#333333" CellPadding="4" GridLines="Vertical"  
        OnRowEditing="gvAllStudents_RowEditing" 
        OnRowUpdating="gvAllStudents_RowUpdating"
        OnRowCancelingEdit="gvAllStudents_RowCancelingEdit"
        OnSelectedIndexChanged="gvAllStudents_SelectedIndexChanged"
        OnRowDeleting="gvAllStudents_RowDeleting"
        OnRowUpdated="gvAllStudents_RowUpdated"
  >
        <HeaderStyle BackColor="#990000" Font-Size="Large" Font-Bold="true" ForeColor="White" />
        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#FFFB00" />
        <Columns>
            <asp:CommandField ShowEditButton="true" ButtonType="Button" EditText="Edit" />
            <asp:CommandField ShowDeleteButton="true" ButtonType="Button" DeleteText="удалить" />
            <asp:CommandField ShowSelectButton="true" Visible="true" ButtonType="Button" SelectText="выбрать" />
            <asp:BoundField Visible="true" ReadOnly="true" DataField="StudentID" HeaderText="ID"/>
            <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName"/>
            <asp:BoundField DataField="SecondName" HeaderText="Second Name" SortExpression="SecondName"/>
            <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" SortExpression="DateOfBirth" DataFormatString="{0:d}" />
            <asp:BoundField Visible="true" DataField="Foto" HeaderText="Foto" />
        </Columns>
  </asp:GridView>
  </div>
  <br />
  <asp:FileUpload ID="fuFoto" runat="server" Visible="False" />
  <br />
  </div>
  <asp:Button ID="btnExport"  runat="server" onclick="btnExport_Click" Text="Экспорт в Excel" />
  <asp:DetailsView ID="dvGetStudent" runat="server" AutoGenerateRows="false" DataSourceID = "odsGetOneStudent" Visible="false">
      <FieldHeaderStyle BackColor="#0000CD" Font-Size="Large" Font-Bold="true" ForeColor="White" />
      <RowStyle BackColor="#AFEEEE" ForeColor="#333333" /> 
      <Fields>
          <asp:BoundField DataField="FirstName" HeaderText="First Name" />
          <asp:BoundField DataField="SecondName" HeaderText="Second Name" />
          <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" />
      </Fields>
  </asp:DetailsView>
  <asp:Image runat="server" ID="imgShow"  />
  <asp:Button ID="btnExportToPdf"  runat="server" onclick="btnExportToPdf_Click" Text="Экспорт в PDF" Visible="false"/>
  </form>
</body>
</html>
