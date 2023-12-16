
using System.Collections.Generic;
using News.Model;
using News.Interface;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace News.Services
{
    public class StoryService : IStoryService
    {
        private readonly SqlConnection _sqlConnection;

        public StoryService(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }


        public async Task<List<Story>> GetNewestStories(int page, int itemsPerPage, string searchTerm)
        {
            

            using (SqlCommand command = new SqlCommand("GetNewList", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Page", SqlDbType.Int).Value = page;
                command.Parameters.Add("@PageSize", SqlDbType.Int).Value = itemsPerPage;
                command.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 255).Value = searchTerm;
                await _sqlConnection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                var stories = new List<Story>();
                while (await reader.ReadAsync())
                {
                    // Map reader columns to Story object properties
                    var story = new Story
                    {
                        
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        UrlPath = reader.GetString(reader.GetOrdinal("Url")),
                        // Add other properties as needed
                    };

                    stories.Add(story);
                }
                return stories;
            }
        }

    }
}

