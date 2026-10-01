using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);
        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id,
                                         CancellationToken cancellationToken)
    {
        var result = await _pollService.GetAsync(id, cancellationToken);

        //if (poll is null)
        //    return NotFound("Id is not Found");

        //var response = poll.Adapt<PollResponse>();
        //return Ok(response);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }


    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
                                         CancellationToken cancellationToken)
    {
        var result = await _pollService.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Adapt<PollResponse>());
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
                                            [FromBody] PollRequest request,
                                            CancellationToken cancellationToken)
    {
        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Error);
        //var config = new TypeAdapterConfig<CreatePollRequest, Poll>();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id,
                                            CancellationToken cancellationToken)
    {
        Result? result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound();
    }


    [HttpPut("{id:int}/TogglePublish")]
    public async Task<IActionResult> TogglePublishStatusAsync([FromRoute] int id,
                                                              CancellationToken cancellationToken)
    {
        var result = await _pollService.TogglePollStatusAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

}