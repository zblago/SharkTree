using SharkDev.Web.Controls.TreeView;
using System.Collections.Generic;
using System.Web.Mvc;

using SharkDev.Web.Controls.TreeView.Model;

namespace SharkDev.MVC
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeViewExtension
    {
        private TreeViewSettings _treeViewSettings; 

        public TreeViewExtension(TreeViewSettings treeViewSettings)
        {
            this._treeViewSettings = treeViewSettings;
        }

        public MvcHtmlString GetContent(List<Node> items)
        {            
            return MvcHtmlString.Create(new TreeView(_treeViewSettings).Build(items));
        }
    }
}
