namespace UNICS.Data.ViewModels.Entities.Comment
{
    public class ViewComment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CompetitionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
