using agileTrackerServer.Models.Dtos.Backlog;
using agileTrackerServer.Models.ViewModels;
using agileTrackerServer.Services;
using agileTrackerServer.Utils.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace agileTrackerServer.Controllers;

[Authorize]
[ApiController]
[Route("api/projects/{projectId:guid}/backlog")]
public class BacklogController : ControllerBase
{
    private readonly BacklogService _service;

    public BacklogController(BacklogService service)
    {
        _service = service;
    }

    // POST api/projects/{projectId}/backlog/epics
    [HttpPost("epics")]
    [Authorize(Policy = "CanViewProject")]
    [SwaggerOperation(Summary = "Cria um épico no product backlog do projeto.")]
    public async Task<IActionResult> CreateEpic(Guid projectId, [FromBody] CreateEpicDto dto)
    {
        var userId = User.GetUserId();

        await _service.CreateEpicAsync(projectId, userId, dto);

        return Ok(ResultViewModel.Ok("Épico criado com sucesso."));
    }

    // POST api/projects/{projectId}/backlog/epics/{epicId}/stories
    [HttpPost("epics/{epicId:int}/stories")]
    [Authorize(Policy = "CanViewProject")]
    [SwaggerOperation(Summary = "Cria uma user story dentro de um épico do projeto.")]
    public async Task<IActionResult> CreateStory(Guid projectId, int epicId, [FromBody] CreateUserStoryDto dto)
    {
        var userId = User.GetUserId();

        await _service.CreateUserStoryAsync(projectId, epicId, userId, dto);

        return Ok(ResultViewModel.Ok("User story criada com sucesso."));
    }
    
    // GET api/projects/{projectId}/backlog
    [HttpGet]
    [Authorize(Policy = "CanViewProject")]
    [SwaggerOperation(Summary = "Retorna o product backlog completo (overview + epics + user stories).")]
    [ProducesResponseType(typeof(ProductBacklogResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductBacklogResponseDto>> GetBacklog(Guid projectId)
    {
        var userId = User.GetUserId();

        var result = await _service.GetBacklogByProjectIdAsync(projectId, userId);

        return Ok(result);
    }
    
    // PATCH api/projects/{projectId}/backlog
    [HttpPatch]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> UpdateOverview(Guid projectId, [FromBody] UpdateBacklogOverviewDto dto)
    {
        var userId = User.GetUserId();
        await _service.UpdateOverviewAsync(projectId, userId, dto);
        return Ok(ResultViewModel.Ok("Overview atualizado com sucesso."));
    }

    // PATCH api/projects/{projectId}/backlog/epics/{epicId}
    [HttpPatch("epics/{epicId:int}")]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> UpdateEpic(Guid projectId, int epicId, [FromBody] UpdateEpicDto dto)
    {
        var userId = User.GetUserId();
        await _service.UpdateEpicAsync(projectId, epicId, userId, dto);
        return Ok(ResultViewModel.Ok("Épico atualizado com sucesso."));
    }

    // PATCH api/projects/{projectId}/backlog/stories/{storyId}
    [HttpPatch("stories/{storyId:int}")]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> UpdateStory(Guid projectId, int storyId, [FromBody] UpdateUserStoryDto dto)
    {
        var userId = User.GetUserId();
        await _service.UpdateUserStoryAsync(projectId, storyId, userId, dto);
        return Ok(ResultViewModel.Ok("User story atualizada com sucesso."));
    }
    [HttpPatch("epics/reorder")]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> ReorderEpic(Guid projectId, [FromBody] ReorderEpicDto dto)
    {
        var userId = User.GetUserId();
        await _service.ReorderEpicAsync(projectId, userId, dto);
        return Ok(ResultViewModel.Ok("Épico reordenado com sucesso."));
    }

    [HttpPatch("stories/reorder")]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> ReorderStory(Guid projectId, [FromBody] ReorderUserStoryDto dto)
    {
        var userId = User.GetUserId();
        await _service.ReorderUserStoryAsync(projectId, userId, dto);
        return Ok(ResultViewModel.Ok("User story reordenada com sucesso."));
    }
    
    [HttpPatch("stories/move")]
    [Authorize(Policy = "CanViewProject")]
    public async Task<IActionResult> MoveStory(Guid projectId, [FromBody] MoveUserStoryDto dto)
    {
        var userId = User.GetUserId();
        await _service.MoveUserStoryAsync(projectId, userId, dto);
        return Ok(ResultViewModel.Ok("User story movida com sucesso."));
    }
}