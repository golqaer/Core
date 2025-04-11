namespace DTO.UserSecurityManager;

public record UserClaimData
{
    public string? UserName { get; set; }
    public long? UserId { get; set; }
    public bool Admin { get; set; } = false;
}