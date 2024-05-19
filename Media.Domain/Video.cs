using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Media.Domain
{
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoIdentificator { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// Duration in seconds
        /// </summary>
        public int Duration { get; set; }
        public string Url { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public DateTime QueriedFromDate { get; set; }
        public DateTime QueriedToDate { get; set; }
        public DateTime AddedDateTime { get; set; } = DateTime.UtcNow;
        public string FilteredByChannelName { get; set; }

        public IEnumerable<string> GetNullProperties()
        {
            return GetType()
                .GetProperties()
                .Where(p => p.GetValue(this) == null)
                .Select(p => p.Name);
        }

        //Many to many relationshiop to users
        public ICollection<User> Users{ get; set; }
    }
}
