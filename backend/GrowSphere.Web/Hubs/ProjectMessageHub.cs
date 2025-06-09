using GrowSphere.Domain.Models.ProjectMessage;
using GrowSphere.Domain.Models.ProjectModel;
using GrowSphere.Domain.Models.UserModel;
using GrowSphere.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Hubs;

public class ProjectMessageHub: Hub
{
    private readonly ApplicationDbContext _context;

    public ProjectMessageHub(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task SendMessage(Guid projectId, string content)
    {
        var userIdString = Context.UserIdentifier;
        if (!Guid.TryParse(userIdString, out var senderId))
            return; 
        // можно добавить проверку, что пользователь — участник проекта
        
        var projectIdResult = ProjectId.Create(projectId);
        var senderIdResult = UserId.Create(senderId);
        var projectMessageId = ProjectMessageId.NewId();
        
        var result = ProjectMessage.Create(projectMessageId, projectIdResult, senderIdResult, content);
        if (result.IsFailure)
            return;
        
        var message = result.Value;
        _context.ProjectMessages.Add(message);
        await _context.SaveChangesAsync();
        
        var sender = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == senderId);

        await Clients.Group(projectId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                message.Id,
                message.ProjectId,
                message.SenderId,
                message.Content,
                message.SentAt,
                Sender = new
                {
                    sender!.Id,
                    Name = sender.Name.Value
                }
            });
    }
    
    public override async Task OnConnectedAsync()
    {
        var projectId = Context.GetHttpContext()?.Request.Query["projectId"];
        if (!string.IsNullOrEmpty(projectId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId!);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var projectId = Context.GetHttpContext()?.Request.Query["projectId"];
        if (!string.IsNullOrEmpty(projectId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, projectId!);
        }

        await base.OnDisconnectedAsync(exception);
    }
}