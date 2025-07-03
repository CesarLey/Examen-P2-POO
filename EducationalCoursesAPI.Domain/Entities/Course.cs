namespace EducationalCoursesAPI.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
    }
} 