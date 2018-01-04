
using System.Web.Mvc;

namespace SharkDev.MVC
{
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlExtension
    {
        #region Methods

        public static Instance SharkDev(this HtmlHelper htmlHelper)
        {
            return new Instance();
        }

        #endregion
    }
}
