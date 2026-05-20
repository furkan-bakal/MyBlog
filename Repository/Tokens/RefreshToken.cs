using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Tokens
{
    public class RefreshToken: BaseEntity<Guid>
    {
        public Guid Code { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid UserId { get; set; }
    }
}
