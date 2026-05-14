using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Token
{
    public interface ITokenService
    {
        Task<ResponseModelDto<TokenResponseDto>> GetAccessTokenAsync(GetAccessTokenRequestDto request, CancellationToken cancellationToken);
    }
}
