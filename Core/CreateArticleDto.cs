using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public record CreateArticleDto(string Content, string Title, string Author)
    {
    }
}
