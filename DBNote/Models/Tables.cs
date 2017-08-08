using System.Collections.Generic;

namespace DBNote.Models
{
    public class Tables : Dictionary<string, Table>
    {
        public Tables()
        {
        }

        public Tables(int capacity)
            : base(capacity)
        {
        }
    }
}
