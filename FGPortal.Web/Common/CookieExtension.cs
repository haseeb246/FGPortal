namespace FGPortal.Web
{
    public static class CookieExtension
    {
        /// <summary>  
        /// set the cookie  
        /// </summary>  
        /// <param name="key">key (unique indentifier)</param>  
        /// <param name="value">value to store in cookie object</param>  
        /// <param name="expireTime">expiration time</param>  
        /// <param name="OneDay minutes">1440</param>  
        public static void SetCookie(this HttpResponse Response, string key, string value, int? expireTimeInMinutes= 1440)
        {
            CookieOptions option = new CookieOptions();

            if (expireTimeInMinutes.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTimeInMinutes.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public static void RemoveCookie(this HttpResponse Response, string key)
        {
            Response.Cookies.Delete(key);
        }


        /// <summary>  
        /// Get the cookie  
        /// </summary>  
        /// <param name="key">Key </param>  
        /// <returns>string value</returns>  
        public static string GetCookie(this HttpRequest Request, string Key)
        {
            return Request.Cookies[Key];
        }
    }
}
