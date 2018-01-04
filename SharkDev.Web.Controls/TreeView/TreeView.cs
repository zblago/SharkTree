using System.Collections.Generic;

using SharkDev.Web.Controls.TreeView.Model;
using System.Web.UI.HtmlControls;
using System.Linq;
using System;
using System.Text;
using System.IO;
using System.Web.UI;

namespace SharkDev.Web.Controls.TreeView
{
    /// <summary>
    /// 
    /// </summary>
    internal class TreeView
    {
        #region Properties

        #endregion

        #region Fields

        private TreeViewSettings _treeViewSettings = null;
        private ILookup<string, Node> _origTree = null;

        #endregion

        #region ctor

        public TreeView(TreeViewSettings settings)
        {
            _treeViewSettings = settings;
        }

        #endregion

        #region Methods

        public string Build(List<Node> originalTree)
        {            
            if (originalTree == null)
            {
                throw new Exception("List is empty. Can't build a tree.");
            }
            else
            {
                _origTree = originalTree.ToLookup(x => x.ParentId);
            }
            
            IList<Node> treeWithoutParents = originalTree.Where(x => String.IsNullOrEmpty(x.ParentId)).OrderBy(x => x.Term).ToList();

            //Base root.
            HtmlGenericControl ulContainer = TreeStructureCreator.CreateUl(_treeViewSettings);

            #if DEBUG
                DateTime start = DateTime.Now;
            #endif

            //Leafs
            foreach (Node item in treeWithoutParents)
            {
                Item newItem = new Item { Id = item.Id, Term = item.Term, ParentId = item.ParentId, Level = 0, Attributes = item.Attributes};

                HtmlGenericControl newLi = TreeStructureCreator.CreateLi(string.Empty, string.Empty, string.Empty);
                ulContainer.Controls.Add(newLi);

                this.InsertChildren(newLi, newItem, 0);
            }

            //Tree
            HtmlGenericControl tree = TreeStructureCreator.CreateTree(_treeViewSettings, ulContainer, originalTree);
            
            StringBuilder generatedHtml = new StringBuilder();
            HtmlTextWriter htw = new HtmlTextWriter(new StringWriter(generatedHtml));
            tree.RenderControl(htw);
            string output = generatedHtml.ToString();

            #if DEBUG
                DateTime end = DateTime.Now;
                output = output + "<br/><span>Loading time is " + (end - start).TotalMilliseconds + "</span></br/>";
            #endif

            return output;
        }

        private void InsertChildren(HtmlGenericControl container, Item item, int level)
        {
            ++level;
            IEnumerable<Node> childItems = _origTree[item.Id];

            HtmlGenericControl newUl = null;

            if (childItems.Count() > 0)
            {
                container.Attributes.Add("class", "hiddenUl");
                container.Controls.AddAt(0, TreeStructureCreator.CreateExpander(item.Id, item.Term, item.ParentId, item.Attributes, "divinsideli", "expand", "spaninsidediv"));

                newUl = new HtmlGenericControl("ul");
            }
            else if (childItems.Count() == 0)
            {
                container = TreeStructureCreator.CreateLiContainer(container, item.Id, item.Term, item.ParentId, item.Attributes, "spaninsidediv solo");
                return;
            }

            foreach (Node tItem in childItems)
            {
                Item itemToAdd = new Item { Id = tItem.Id, Term = tItem.Term, ParentId = tItem.ParentId,  Attributes = tItem.Attributes, Level = level };

                HtmlGenericControl newLi = TreeStructureCreator.CreateLi(string.Empty, string.Empty, string.Empty);
                newUl.Controls.Add(newLi);
                this.InsertChildren(newLi, itemToAdd, level);
            }

            container.Controls.Add(newUl);
        }

        #endregion
    }
}
