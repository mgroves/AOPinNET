using System;
using System.Web;

namespace MobileBrowserAspNet
{
    public partial class MobileInterstitial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDownload.Click += btnDownload_Click;
            btnNoThanks.Click += btnNoThanks_Click;
        }

        void btnNoThanks_Click(object sender, EventArgs e)
        {
            var cookie = new HttpCookie("NoThanks", "set");
            cookie.Expires = DateTime.Now.AddMinutes(2);
            Response.Cookies.Add(cookie);

            var returnUrl = Request.QueryString["returnUrl"];
            Response.Redirect(HttpUtility.UrlDecode(returnUrl));
        }

        void btnDownload_Click(object sender, EventArgs e)
        {
            var mobileDetect = new MobileDetect(Context);
            if (mobileDetect.IsAndroid())
                Response.Redirect("market://search?q=pname:com.myappname.android");
            if (mobileDetect.IsApple())
                Response.Redirect("http://itunes.com/apps/appname");
            if (mobileDetect.IsWindowsPhone())
                Response.Redirect("...");
        }
    }
}