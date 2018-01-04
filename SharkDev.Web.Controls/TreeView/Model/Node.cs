using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkDev.Web.Controls.TreeView.Model
{
    /// <summary>
    /// For external use.
    /// </summary>
    public class Node
    {
        public string Id { get; set; }
        public string Term { get; set; }
        public string ParentId { get; set; }
        public IDictionary<string, string> Attributes { get; set; }
    }
}
