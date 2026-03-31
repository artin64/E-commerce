namespace ECommerce.Api.Services.Auth;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "ECommerce.Api";
    public string Audience { get; set; } = "ECommerce.Frontend";
    public string Key { get; set; } = "CHANGE_ME_DEV_ONLY_CHANGE_ME_DEV_ONLY_CHANGE_ME";
    public int ExpMinutes { get; set; } = 240;
}

