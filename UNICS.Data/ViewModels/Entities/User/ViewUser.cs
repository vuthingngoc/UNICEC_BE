namespace UNICS.Data.ViewModels.Entities.User
{
    public class ViewUser
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int? UniversityId { get; set; }
        public int? MajorId { get; set; }
        public string Fullname { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public string Dob { get; set; }
        public string Description { get; set; }
    }
}
