namespace DBNote.Models
{
    public class View : BaseTable
    {
        public View()
            : this("", "", "")
        {
        }

        public View(string id, string displayName, string name)
            : this(id, displayName, name,string.Empty)
        {
        }

        public View(string id, string displayName, string name, string comment)
            : base(id, displayName, name, comment)
        {
            _mataTypeName = "view";
        }
    }
}
