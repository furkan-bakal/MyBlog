using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Token
{
    public record TokenOptions
    {
        public string Signature { get; set; } = default!;
        public int ExpireByHour { get; set; }
    }
}
