namespace GrowSphere.DTOs;

public record UserDto
{
    public string UserName { get; set; }
        
    public string Email { get; set; }
        
    public string Password { get; set; }
        
    public UserProfileDto Profile { get; set; }
        
    public UserStatusDto UserStatus { get; set; }
        
    public DateTime CreatedDate { get; set; }
}

public record UserProfileDto
{
    public string Bio { get; set; }
        
    public string Picture { get; set; }
}

public record UserStatusDto
{
    public string Status { get; set; }
}
