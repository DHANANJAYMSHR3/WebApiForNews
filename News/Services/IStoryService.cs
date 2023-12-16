using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using News.Model;

namespace News.Interface
{
    public interface IStoryService
    {
        Task<List<Story>> GetNewestStories(int page, int itemsPerPage,string searchterm);
    }
}