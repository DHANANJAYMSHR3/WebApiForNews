
using System.Collections.Generic;
using News.Model;
using News.Interface;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace News.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;

        public StoryService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _httpClientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<List<int>> GetStorys()
        {

            using (var httpClient = _httpClientFactory.CreateClient("Story"))
            {
                // Make API requests using the httpClient instance
                HttpResponseMessage response = await httpClient.GetAsync(_configuration.GetValue<string>("ApiUrl:BaseUrl") + "topstories.json?print=pretty");

                // Process the response
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON array to a list of integers
                    List<int> idList = JsonConvert.DeserializeObject<List<int>>(responseData);
                    // Create a list of Shop instances with Id property set
                    //List<StoryDetail> story = idList.Select(id => new StoryDetail { id = id }).ToList();
                    return idList;
                    
                }
                else
                {
                    // Handle error cases
                    // For example, log the error or throw an exception
                }
            }
            return new List<int>();
          
        }
        public async Task<StoryDetail> GetStorysDetail(int id)
        {

            using (var httpClient = _httpClientFactory.CreateClient("Story"))
            {
                // Make API requests using the httpClient instance
                HttpResponseMessage response = await httpClient.GetAsync(_configuration.GetValue<string>("ApiUrl:BaseUrl") + "item/"+id+".json?print=pretty");
                // Process the response
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON array to a list of integers
                    StoryDetail storyDetails = JsonConvert.DeserializeObject<StoryDetail>(responseData);
                    return storyDetails;
                    // Process the response body
                }
                else
                {
                    // Handle error cases
                    // For example, log the error or throw an exception
                }
            }
            return new StoryDetail();
            
        }
   
    }

}


