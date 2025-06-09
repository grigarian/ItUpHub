using GrowSphere.Application.Skills;
using GrowSphere.Domain.Models.SkillModel;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    [Authorize]
    [HttpPost("add")]
    public async Task<ActionResult<Guid>> AddSkill
    (
        [FromServices] SkillService skillService,
        [FromBody] AddSkillRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await skillService.Add(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return result.Value;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Skill>>> GetAllSkills
    (
        [FromServices] SkillService skillService,
        CancellationToken cancellationToken
    )
    {
        var result = await skillService.GetAll(cancellationToken);
        
        if(result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value.ToList());
    }
    
}