using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UniCEC.Data.ViewModels.Entities.Department
{
    public class DepartmentUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
    }
}
