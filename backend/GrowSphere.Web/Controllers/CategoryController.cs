using GrowSphere.Application.Categories;
using GrowSphere.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GrowSphere.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CategoryService service,
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken
        )
    {
        var result = await service.Create(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        Log.Information
        ("Category {Title} successfully create on {Time}",
            request.Title,
            DateTime.Now);

        return result.Value;
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetAll([FromServices] CategoryService service, CancellationToken cancellationToken)
    {
        var result = await service.GetAll(cancellationToken);
        
        return Ok(result.Value);
    }
}