<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MobileInterstitial.aspx.cs" Inherits="MobileBrowserAspNet.MobileInterstitial" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Mobile Interstitial</title>
    <meta name="viewport" content="user-scalable=no, width=device-width" />
</head>
<body>
    <h2>Check out our mobile app!</h2>
    <p>We've written a great mobile app for you, but if you just want to use a browser, that's cool too.</p>
    <form id="form1" runat="server">
        <asp:Button ID="btnDownload" runat="server" Text="Get the App" />
        <asp:Button ID="btnNoThanks" runat="server" Text="No thanks" />
    </form>
</body>
</html>
