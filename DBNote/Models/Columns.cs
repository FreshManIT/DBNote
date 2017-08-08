using System.Collections.Generic;

namespace DBNote.Models
{
    public class Columns : Dictionary<string, Column>
    {
        public Columns()
        {
        }

        public Columns(int capacity)
            : base(capacity)
        {
        }
    }
}
