namespace SharkDev.Web.Controls.TreeView
{
    public class TreeViewSettings
    {
        public TreeViewSettings()
        {
            Header = new Header();
            ClientHandlers = new ClientSide();
            Height = 0;
            Width = 0;
            DataOnClient = false;
        }

        public string Id { get; set; }
        public Header Header { get; set; }
        public ClientSide ClientHandlers { get; set; }
        public string AutoCompleteHandler { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public bool DataOnClient { get; set; }
    }

    public class Header 
    {
        public Header()
        {
            Visible = true;
            Expanded = true;
            Text = string.Empty;
        }

        public bool Visible { get; set; }
        public bool Expanded { get; set; }
        public string Text { get; set; }
    }

    public class ClientSide
    {        
        public string ContentSelect { get; set; }
    }
}