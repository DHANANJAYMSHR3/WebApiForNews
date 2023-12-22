using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using News.Controllers;
using News.Interface;
using News.Model;

public class StoryControllerTests
{
    [Fact]
    //This test case verifies that if the data is present in the cache, the controller returns an Ok result with the cached data without calling the _storyService.GetStorys() method.
    public async Task GetStories_ReturnsOkResultWithCachedData()
    {
       
        var mockCache = new Mock<IMemoryCache>();
        var cachedStories = new List<StoryDetail> { /* populate with sample data */ };
        mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cachedStories)).Returns(true);
        var mockStoryService = new Mock<IStoryService>();
        var controller = new StoryController(mockStoryService.Object, mockCache.Object);
        // Act
        var result = await controller.GetStories();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedStories = Assert.IsType<List<StoryDetail>>(okResult.Value);
        Assert.Equal(cachedStories, returnedStories);
        mockStoryService.Verify(s => s.GetStorys(), Times.Never); 
    }
    //This test case verifies that if the data is not present in the cache, the controller calls the _storyService.GetStorys() method, caches the fetched data, and returns an Ok result with the fetched data.
    [Fact]
    public async Task GetStories_FetchesDataFromServiceAndCaches()
    {
        // Arrange
        var mockCache = new Mock<IMemoryCache>();
        var mockStoryService = new Mock<IStoryService>();
        var controller = new StoryController(mockStoryService.Object, mockCache.Object);
        var dummyStories = new List<StoryDetail> { };
        mockStoryService.Setup(s => s.GetStorys());
        // Act
        var result = await controller.GetStories();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedStories = Assert.IsType<List<StoryDetail>>(okResult.Value);
        Assert.Equal(dummyStories, returnedStories);
        mockStoryService.Verify(s => s.GetStorys(), Times.Once); // Ensure GetStorys is called when data is not in the cache
        var cacheEntryOptions = It.IsAny<MemoryCacheEntryOptions>();
        mockCache.Verify(c => c.Set(It.IsAny<object>(), dummyStories, cacheEntryOptions), Times.Once); // Ensure Set is called to cache the data
    }
   
    
        [Fact]
        public async Task GetStoryDetail_CacheHit_ReturnsCachedStories()
        {
            // Arrange
            var id = 8863;
            var cacheKey = $"StoriesDetail_{id}";
            var cachedStories = new List<StoryDetail> {};
            var cacheMock = new Mock<IMemoryCache>();
            var storyServiceMock = new Mock<IStoryService>();
            var controller = new StoryController(storyServiceMock.Object, cacheMock.Object);
            // Act
            var result = await controller.GetStoryDetail(id);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStories = Assert.IsAssignableFrom<List<StoryDetail>>(okResult.Value);
            Assert.Equal(cachedStories, returnedStories);
        }

        [Fact]
        public async Task GetStoryDetail_CacheMiss_ReturnsFetchedStories()
        {
            
            var id = 8863;
            var cacheKey = $"StoriesDetail_{id}";
            var fetchedStories = new List<StoryDetail> {};
            var cacheMock = new Mock<IMemoryCache>();
            var storyServiceMock = new Mock<IStoryService>();
            storyServiceMock.Setup(s => s.GetStorysDetail(id));
            var controller = new StoryController(storyServiceMock.Object, cacheMock.Object);
            // Act
            var result = await controller.GetStoryDetail(id);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStories = Assert.IsAssignableFrom<List<StoryDetail>>(okResult.Value);
            Assert.Equal(fetchedStories, returnedStories);
            // Additional assertions to check if caching is used correctly
            cacheMock.Verify(c => c.Set(cacheKey, fetchedStories, It.IsAny<MemoryCacheEntryOptions>()), Times.Once);
        }
    }





