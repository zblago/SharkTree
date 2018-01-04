using System;
using SharkDev.Web.Controls.TreeView;

namespace SharkDev.MVC
{
    /// <summary>
    /// Just for referencing all SharkDev extensions
    /// </summary>
    public class Instance
    {
        public TreeViewExtension TreeView(Action<TreeViewSettings> settings)
        {
            TreeViewSettings treeSettings = new TreeViewSettings();
            settings.Invoke(treeSettings);

            return new TreeViewExtension(treeSettings);
        }
    }
}
