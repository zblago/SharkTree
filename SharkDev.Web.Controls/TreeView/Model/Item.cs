using System.Collections.Generic;

namespace SharkDev.Web.Controls.TreeView.Model
{
    internal sealed class Item
    {
        public string Id { get; set; }
        public string Term { get; set; }
        public string ParentId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
        public int Level { get; set; }
        public IList<Item> ChildItems { get; set; }
    }
}
