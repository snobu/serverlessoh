using System;

namespace IcecreamRatings
{
    public class CreateRatingResponse
    {
        public string UserID { get; set; }
        public string ProductId { get; set; }
        public string LocationName { get; set; }
        public DateTime Timestamp { get; set;}
        public int Rating { get; set; }
        public string UserNotes {get; set;}
    }
}