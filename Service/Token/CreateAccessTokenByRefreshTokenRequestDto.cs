namespace Service.Token
{
    public record CreateAccessTokenByRefreshTokenRequestDto(Guid RefreshToken);

}
