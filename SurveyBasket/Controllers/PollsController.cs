using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Models;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController : ControllerBase
{
    private readonly List<Poll> _polls = [];

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_polls);
    }
}
