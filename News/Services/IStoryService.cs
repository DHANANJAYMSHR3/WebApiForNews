using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Model;

namespace News.Interface
{
    public interface IStoryService
    {
        // Task<List<StoryDetail>> GetStorys();
        Task<List<int>> GetStorys();
        Task<StoryDetail> GetStorysDetail(int id);
    }
}