using Microsoft.AspNetCore.Mvc;
using SocialApp.Common.Search;

namespace SocialApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchHandler _searchService;

    public SearchController(ISearchHandler searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var items = await _searchService.Search(query, HttpContext.RequestAborted);

        return Ok(items);
    }
}