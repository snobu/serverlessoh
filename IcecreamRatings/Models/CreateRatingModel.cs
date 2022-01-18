using System;

namespace IcecreamRatings.Models
{
    public class CreateRatingModel
    {
        public string Id  {get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public DateTime Timestamp { get; set;}
        public int Rating { get; set; }
        public string UserNotes {get; set;}
    }
}