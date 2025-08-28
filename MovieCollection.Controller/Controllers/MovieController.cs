using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCollection.Core.Interfaces;





namespace MovieCollection.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllMoviesByUserId(int id)
        {
            var response = await _movieService.GetAllMoviesByUserIdAsync(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

    }
}
