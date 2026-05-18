using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.User
{
    public record SignUpRequestDto(string UserName, string Email, string Password);
}
