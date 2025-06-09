using GrowSphere.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectMessageController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectMessageController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{projectId:guid}/messages")]
    public async Task<IActionResult> GetMessages(Guid projectId, CancellationToken cancellationToken)
    {
        var messages = await _context.ProjectMessages
            .Where(m => m.ProjectId == projectId)
            .OrderByDescending(m => m.SentAt)
            .Take(50)
            .Select(m => new {
                m.Id,
                m.Content,
                m.SentAt,
                Sender = new { m.Sender.Id, Name = m.Sender.Name.Value }
            })
            .ToListAsync(cancellationToken);

        return Ok(messages);
    }
}