using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Model;
using News.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Cors;
using System.Linq;

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
        public async Task<ActionResult<List<StoryDetail>>> GetStories()
        {
            string cacheKey = $"NewestStories";

            // Try to get the stories from the cache
            if (_cache.TryGetValue(cacheKey, out List<StoryDetail> cachedStories))
            {
                return Ok(cachedStories);
            }
            var stories = await _storyService.GetStorys();
            
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1) // Refresh the cache if not accessed within this time
            };
            _cache.Set(cacheKey, stories, cacheEntryOptions);
            return Ok(stories.Select(id => new StoryDetail { id = id }).ToList());
        }

       
        [HttpGet]
        [Route("GetStoryDetail")]
        public async Task<ActionResult<StoryDetail>> GetStoryDetail(int id=0)
        {
            string cacheKey = $"StoriesDetail_"+id;

            // Try to get the stories from the cache
            if (_cache.TryGetValue(cacheKey, out List<StoryDetail> cachedStories))
            {
                return Ok(cachedStories);
            }
            var stories = await _storyService.GetStorysDetail(id);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(1) // Refresh the cache if not accessed within this time
            };

            _cache.Set(cacheKey, stories, cacheEntryOptions);
          
            return Ok(stories);
        }

    }
}
