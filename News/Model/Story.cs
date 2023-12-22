using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Model
{
    //public class Story
    //{
        
    //    public int Id { get; set; }
    //}
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class StoryDetail
    {
        public string by { get; set; }
        public int descendants { get; set; }
        public int id { get; set; }
        public List<int> kids { get; set; }
        public int score { get; set; }
        public int time { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
    
}
