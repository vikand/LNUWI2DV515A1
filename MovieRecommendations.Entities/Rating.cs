namespace MovieRecommendations.Entities
{
    public class Rating
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public float UserRating { get; set; }
    }
}
