using SharkDev.Web.Controls.TreeView.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

namespace SharkDev.Web.Controls.TreeView
{
    internal sealed class TreeStructureCreator
    {
        public static HtmlGenericControl CreateScripts(TreeViewSettings settings)
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");

            StringBuilder html = new StringBuilder();
            html.AppendLine(@"$(document).ready(function () {");
            html.AppendLine(String.Format(@"$('#{0}').SharkDevTreeView({{", settings.Id));
            html.AppendLine(String.Format(@"'AutoCompleteHandler': '{0}',",  settings.AutoCompleteHandler));
            html.AppendLine(String.Format(@"'AfterSelectHandler': {0},", settings.ClientHandlers.ContentSelect));
            html.AppendLine(String.Format(@"'DataOnClient': {0}", settings.DataOnClient.ToString().ToLower()));
            html.AppendLine("});");
            html.AppendLine("});");

            script.InnerHtml = html.ToString();
            return script;
        }

        public static HtmlGenericControl CreateAutoCompleteContainer()
        {
            HtmlGenericControl divAcContainer = new HtmlGenericControl("div");
            HtmlGenericControl input = new HtmlGenericControl("input");
            HtmlGenericControl spanProgress = new HtmlGenericControl("span");

            divAcContainer.Controls.AddAt(0, input);
            divAcContainer.Controls.AddAt(1, spanProgress);

            return divAcContainer;
        }

        public static HtmlGenericControl CreateTreeHeaderContainer(TreeViewSettings settings, string id, string containerCssClass, string contentCssClass)
        {
            HtmlGenericControl container = new HtmlGenericControl("div");
            if (!string.IsNullOrEmpty(containerCssClass))
            {
                container.Attributes.Add("class", containerCssClass);
            }

            if (!string.IsNullOrEmpty(id))
            {
                container.Attributes.Add("id", String.Format("{0}_header", settings.Id));
            }

            if (settings.Header.Visible)
            {
                HtmlGenericControl expand = new HtmlGenericControl("a");
                HtmlGenericControl content = new HtmlGenericControl("span");

                if (!string.IsNullOrEmpty(id))
                {
                    expand.Attributes.Add("id", string.Format("Expander_{0}", id));
                    content.Attributes.Add("id", string.Format("Content_{0}", id));
                }

                if (!string.IsNullOrEmpty(settings.Header.Text))
                {
                    content.InnerText = settings.Header.Text;
                }

                if (settings.Header.Expanded)
                {
                    expand.Attributes.Add("class", "collapse");
                }
                else
                {
                    expand.Attributes.Add("class", "expand");   
                }

                if (!string.IsNullOrEmpty(contentCssClass))
                {
                    content.Attributes.Add("class", contentCssClass);
                }

                container.Controls.AddAt(0, expand);
                container.Controls.AddAt(1, content);
            }
            return container;
        }

        public static HtmlGenericControl CreateTreeContainer(HtmlGenericControl content, TreeViewSettings settings, string cssClass)
        {
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            mainDiv.Controls.AddAt(0, content);
            mainDiv.Attributes.Add("class", cssClass);
            mainDiv.Attributes.Add("id", String.Format("{0}_tree", settings.Id));
            if (settings.Height > 0)
            {
                mainDiv.Attributes.Add("style", String.Format("height:{0}px;", settings.Height));
            }

            return mainDiv;
        }

        public static HtmlGenericControl CreateDataContainer(TreeViewSettings settings, HtmlGenericControl content, List<Node> data)
        {
            HtmlGenericControl divData = new HtmlGenericControl("div");
            divData.Attributes.Add("id", String.Format("{0}_JsonData", settings.Id));
            divData.Attributes.Add("style", "display:none;");

            JavaScriptSerializer jSer = new JavaScriptSerializer();
            jSer.MaxJsonLength = int.MaxValue;

            divData.InnerText = jSer.Serialize(data);

            return divData;
        }

        public static HtmlGenericControl CreateLi(string id, string text, string cssClass)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");
            if (!string.IsNullOrEmpty(id)) li.Attributes.Add("id", id);
            if (!string.IsNullOrEmpty(text)) li.InnerText = text;
            if (!string.IsNullOrEmpty(cssClass)) li.Attributes.Add("class", cssClass);

            return li;
        }

        public static HtmlGenericControl CreateUl(TreeViewSettings settings)
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            if (!string.IsNullOrEmpty(settings.Id)) ul.Attributes.Add("id", String.Format("{0}_rootUl", settings.Id));
            ul.Attributes.Add("class", String.Format("{0} {1}", "rootUl", settings.Header.Expanded || !settings.Header.Visible ? string.Empty : "hiddenUl").TrimEnd());

            return ul;
        }

        public static HtmlGenericControl CreateExpander(string id, string text, string parentId, IDictionary<string, string> attributes, string containerCssClass, string expandCssClass, string contentCssClass)
        {
            HtmlGenericControl container = new HtmlGenericControl("div");
            HtmlGenericControl expand = new HtmlGenericControl("a");
            HtmlGenericControl content = new HtmlGenericControl("span");

            if (!string.IsNullOrEmpty(id))
            {
                expand.Attributes.Add("id", string.Format("Expander_{0}", id));
                content.Attributes.Add("id", string.Format("Content_{0}", id));

                if (attributes != null && attributes.Count > 0)
                {
                    string strAttributes = string.Empty;
                    int i = 0;
                    foreach (KeyValuePair<string, string> item in attributes)
                    {
                        ++i;
                        strAttributes += String.Format("\"{0}\":\"{1}\"", item.Key, item.Value);
                        strAttributes += i == attributes.Count ? "" : ",";
                    }
                    content.Attributes.Add("obj", String.Format("{{\"id\":\"{0}\", \"name\":\"{1}\", \"parentid\": \"{2}\", {3} }}", id, text, parentId, strAttributes));
                }
                else
                {
                    content.Attributes.Add("obj", String.Format("{{\"id\":\"{0}\", \"name\":\"{1}\", \"parentid\": \"{2}\"}}", id, text, parentId));
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                content.InnerText = text;
            }
            if (!string.IsNullOrEmpty(containerCssClass))
            {
                container.Attributes.Add("class", containerCssClass);
            }

            if (!string.IsNullOrEmpty(expandCssClass))
            {
                expand.Attributes.Add("class", expandCssClass);
            }

            if (!string.IsNullOrEmpty(contentCssClass))
            {
                content.Attributes.Add("class", contentCssClass);
            }

            container.Controls.AddAt(0, expand);
            container.Controls.AddAt(1, content);

            return container;
        }

        public static HtmlGenericControl CreateLiContainer(HtmlGenericControl container, string id, string text, string parentId, IDictionary<string, string> attributes, string cssClass)
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            HtmlGenericControl content = new HtmlGenericControl("span");

            if (!string.IsNullOrEmpty(id))
            {                
                content.Attributes.Add("id", string.Format("Content_{0}", id));
                if (attributes != null && attributes.Count > 0)
                {
                    string strAttributes = string.Empty;
                    int i = 0;
                    foreach (KeyValuePair<string, string> item in attributes)
                    {
                        ++i;
                        strAttributes += String.Format("\"{0}\":\"{1}\"", item.Key, item.Value);
                        strAttributes += i == attributes.Count ? "" : ",";
                    }
                    content.Attributes.Add("obj", String.Format("{{\"id\":\"{0}\", \"name\":\"{1}\", \"parentid\": \"{2}\", {3} }}", id, text, parentId, strAttributes));
                }
                else
                {
                    content.Attributes.Add("obj", String.Format("{{\"id\":\"{0}\", \"name\":\"{1}\", \"parentid\": \"{2}\"}}", id, text, parentId));
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                content.InnerText = text;
            }
            if (!string.IsNullOrEmpty(cssClass))
            {
                content.Attributes.Add("class", cssClass);
            }
            
            content.Attributes.Add("class", cssClass);
            div.Attributes.Add("class", "divInsideLi");
            div.Controls.AddAt(0, content);
            container.Controls.AddAt(0, div);

            return container;
        }

        public static HtmlGenericControl CreateAutocompleteContainer(string id, string containerClass, string inputId, string inputClass, string progressId, string progressClass)
        {
            HtmlGenericControl container = new HtmlGenericControl("div");
            HtmlGenericControl input = new HtmlGenericControl("input");
            HtmlGenericControl progress = new HtmlGenericControl("span");

            if (!string.IsNullOrEmpty(id))
            {
                container.Attributes.Add("id", id);
            }

            if (!string.IsNullOrEmpty(containerClass))
            {
                container.Attributes.Add("class", containerClass);
            }

            if (!string.IsNullOrEmpty(inputId))
            {
                input.Attributes.Add("id", inputId);
            }

            if (!string.IsNullOrEmpty(progressId))
            {
                progress.Attributes.Add("id", progressId);
            }

            if (!string.IsNullOrEmpty(inputClass))
            {
                input.Attributes.Add("class", inputClass);
            }

            if (!string.IsNullOrEmpty(progressClass))
            {
                progress.Attributes.Add("class", progressClass);
            }

            HtmlGenericControl progress1Cell = new HtmlGenericControl();
            progress1Cell.Attributes.Add("id", "progressCell1");
            progress1Cell.InnerText = HttpUtility.HtmlDecode("&nbsp;");
            progress1Cell.Attributes.Add("style", "background-color: #89867D; width:5px;padding-right: 4px;margin-left: 4px;");

            HtmlGenericControl progress2Cell = new HtmlGenericControl();
            progress2Cell.Attributes.Add("id", "progressCell2");
            progress2Cell.InnerText = HttpUtility.HtmlDecode("&nbsp;");
            progress2Cell.Attributes.Add("style", "background-color: #89867D; width:5px;padding-right: 4px;margin-left: 1px;");

            HtmlGenericControl progress3Cell = new HtmlGenericControl();
            progress3Cell.Attributes.Add("id", "progressCell3");
            progress3Cell.InnerText = HttpUtility.HtmlDecode("&nbsp;");
            progress3Cell.Attributes.Add("style", "background-color: #89867D; width:5px; padding-right: 4px;margin-left:1px;");

            progress.Controls.Add(progress1Cell);
            progress.Controls.Add(progress2Cell);
            progress.Controls.Add(progress3Cell);

            container.Controls.AddAt(0, input);
            container.Controls.AddAt(1, progress);

            return container;
        }

        public static HtmlGenericControl CreateTree(TreeViewSettings settings, HtmlGenericControl container, List<Node> data)
        {
            HtmlGenericControl tree = new HtmlGenericControl("div");

            if (settings.Width > 0)
            {
                tree.Attributes.Add("style", String.Format("width:{0}px;", settings.Width));
            }

            tree.Attributes.Add("id", settings.Id);
            tree.Attributes.Add("class", "sharkTreeView");
            tree.Controls.AddAt(0, TreeStructureCreator.CreateScripts(settings));
            tree.Controls.AddAt(1, TreeStructureCreator.CreateAutocompleteContainer("search", "search", String.Format("{0}_autoCompleteInput", settings.Id), "autoCompleteInput", "progressParent", "progress"));
            tree.Controls.AddAt(2, TreeStructureCreator.CreateTreeHeaderContainer(settings, 0.ToString(), "header", "headerspan"));
            tree.Controls.AddAt(3, TreeStructureCreator.CreateTreeContainer(container, settings, "tree"));
            
            if (settings.DataOnClient)
            {
                tree.Controls.AddAt(4, TreeStructureCreator.CreateDataContainer(settings, container, data));
            }

            return tree;
            
        }
    }
}
