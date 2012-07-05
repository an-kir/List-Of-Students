<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Students.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblMessage" runat="server" Font-Size="Large" ></asp:Label>
    <asp:Table ID="tblLogin" runat="server">
        <asp:TableRow>
            <asp:TableCell>логин:</asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txtLogin" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>пароль:</asp:TableCell>
            <asp:TableCell><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="btnLogIn" runat="server" Text="submit"  OnClick="btnLogIn_Click"/>
    <asp:Label ID="lblErr" runat="server" Visible="false" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
