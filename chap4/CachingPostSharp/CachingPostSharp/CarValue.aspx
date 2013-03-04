<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="CarValue.aspx.cs" Inherits="CachingPostSharp.CarValue" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <title>Car Value</title>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p>Make: <asp:DropDownList ID="makeDropDown" runat="server" /></p>
    <p>Year: <asp:DropDownList ID="yearDropDown" runat="server" /></p>
    <p>Condition: <asp:DropDownList ID="conditionDropDown" runat="server" /></p>
    <asp:Button ID="getValueButton" Text="Get Value" runat="server" />
    <p>
        <strong>Value:</strong>
        <asp:Literal ID="valueLiteral" runat="server">
            Select Make, Year, Condition, and click "Get Value"
        </asp:Literal>
    </p>
    <p>Cache:</p>
    <asp:BulletedList ID="cachedItemsList" runat="server" />
</asp:Content>
