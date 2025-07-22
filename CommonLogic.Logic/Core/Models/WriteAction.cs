using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLogic.Core.Models
{
    public class WriteAction
    {
        public ushort RegisterAddress { get; set; }
        public ushort ValueToWrite { get; set; }
    }
}
