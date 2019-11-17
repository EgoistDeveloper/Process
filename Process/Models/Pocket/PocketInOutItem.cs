using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.Models.Pocket
{
    public class PocketInOutItem
    {
        public PocketInOut PocketInOut { get; set; }
        public PocketInOutTypeItem PocketInOutTypeItem { get; set; }
        public PocketActionItem PocketActionItem { get; set; }
    }
}
