using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Model;
using News.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Cors;

namespace News.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly IMemoryCache _cache;
        public StoryController(IStoryService storyService, IMemoryCache cache)
        {
            _cache = cache;
            _storyService = storyService;
        }

        [HttpGet]
        [Route("GetStories")]
        public async Task<ActionResult<List<Story>>> GetStories(int page = 1,int itemsPerPage = 10,string searchTerm = "")
        {
            try 
            { 
            if (page < 1 || itemsPerPage < 1)
            {
                return BadRequest("Invalid page or itemsPerPage values.");
            }
            string cacheKey = $"NewestStories_{page}_{itemsPerPage}_{searchTerm}";

            // Try to get the stories from the cache
            if (_cache.TryGetValue(cacheKey, out List<Story> cachedStories))
            {
                return Ok(cachedStories);
            }
            var stories = await _storyService.GetNewestStories(page, itemsPerPage, searchTerm);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1) // Refresh the cache if not accessed within this time
            };

            _cache.Set(cacheKey, stories, cacheEntryOptions);
            return Ok(stories);
        }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
