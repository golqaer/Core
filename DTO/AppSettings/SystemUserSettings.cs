namespace DTO.AppSettings;

public record SystemUserSettings
{
    public long UserId => 1;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin => true;
}