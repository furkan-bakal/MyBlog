using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Token
{
    public record Clients
    {
        public List<ClientItem> Items { get; set; } = default!;
    }

    public record ClientItem(string Id, string Secret);
}
