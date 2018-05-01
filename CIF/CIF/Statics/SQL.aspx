<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="SQL.aspx.cs" Inherits="CIF.Statics.SQL" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #TextArea1 {
            height: 162px;
            width: 503px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <textarea id="TextArea1" name="TextArea1" style="width: 100%; height: 500px"></textarea></div>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
        <p>
            <asp:Label ID="lbeError" runat="server" Text="Label"></asp:Label>
        </p>
        <p>
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </p>
    </form>
</body>
</html>
