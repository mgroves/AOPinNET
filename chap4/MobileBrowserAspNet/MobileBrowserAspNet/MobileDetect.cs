using System.Web;

namespace MobileBrowserAspNet
{
    public class MobileDetect
    {
        readonly HttpRequest _httpRequest;

        public MobileDetect(HttpContext httpContext)
        {
            _httpRequest = httpContext.Request;
        }

        public bool IsMobile()
        {
            return true;
//            return _httpRequest.Browser.IsMobileDevice && 
//                (IsAndroid() || IsApple() || IsWindowsPhone());
        }

        public bool IsWindowsPhone()
        {
            return _httpRequest.UserAgent.Contains("Windows Phone OS");
        }

        public bool IsApple()
        {
            return _httpRequest.UserAgent.Contains("iPhone")
                || _httpRequest.UserAgent.Contains("iPad");
        }

        public bool IsAndroid()
        {
            return _httpRequest.UserAgent.Contains("Android");
        }
    }
}