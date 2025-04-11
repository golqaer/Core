namespace DTO.Database;

public record SystemUserSettings
{
    public long UserId => 1;
    public string UserName { get; set; } = null!;
    public bool IsAdmin => true;
}