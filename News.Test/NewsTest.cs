
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using News.Controllers;
using News.Interface;
using News.Model;
using System.Collections.Generic;
using Xunit;

namespace NewsTest
{
    public class NewsTest
    {
        
        [Fact]
        public void GetStories_ReturnsCorrectType()
        {
            
            // Arrange
            var storyServiceMock = new Mock<IStoryService>();
            var cacheMock = new Mock<IMemoryCache>();
            var controller = new StoryController(storyServiceMock.Object, cacheMock.Object);

            // Act
            var result = controller.GetStories(1, 10,"");

            // Assert
            Assert.IsType<ActionResult<List<Story>>>(result);
        }

    }
}

