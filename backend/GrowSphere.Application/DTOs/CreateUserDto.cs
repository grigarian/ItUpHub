namespace GrowSphere.Application.DTOs;

public record CreateUserDto
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
        
    public string Email { get; set; }
        
    public DateTime CreatedDate { get; set; }
}