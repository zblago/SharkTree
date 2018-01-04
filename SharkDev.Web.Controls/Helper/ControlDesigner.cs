using System.Web.UI.HtmlControls;

namespace SharkDev.Web.Controls.Helper
{
    internal sealed class ControlDesigner : HtmlGenericControl
    {
        #region ctor

        public ControlDesigner(string controlType)
        {
            _control = new HtmlGenericControl(controlType);
        }

        #endregion

        #region Fields

        private HtmlGenericControl _control = null;

        #endregion

        #region Methods

        public void SetId(string id)
        {
            this.Attributes.Add("ID", id);
        }

        public void SetText(string text)
        {
            this.InnerText = text;
        }

        public void SetClass(string cssClass)
        {
            this.Attributes.Add("class", cssClass);
        }

        public void SetHref(string url)
        {
            this.Attributes.Add("href", url);
        }

        #endregion
    }
}
