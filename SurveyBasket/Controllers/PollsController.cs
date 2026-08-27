using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var polls = await _pollService.GetAllAsync(cancellationToken);
        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id,
                                             CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetByIdAsync(id, cancellationToken);

        if (poll is null)
            return NotFound("Id is not Found");

        var response = poll.Adapt<PollResponse>();
        return Ok(response);
    }


    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
                                         CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = newPoll.Id }, newPoll);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id,
                                            [FromBody] PollRequest request,
                                            CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

        if (!isUpdated)
            return NotFound();
        //var config = new TypeAdapterConfig<CreatePollRequest, Poll>();
        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id,
                                            CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

        return isDeleted ? NoContent() : NotFound();
    }


    [HttpPut("{id:int}/TogglePublish")]
    public async Task<IActionResult> TogglePublishStatusAsync([FromRoute] int id,
                                                              CancellationToken cancellationToken)
    {
        var isToggled = await _pollService.TogglePollStatusAsync(id, cancellationToken);

        return isToggled ? NoContent() : NotFound();
    }

}