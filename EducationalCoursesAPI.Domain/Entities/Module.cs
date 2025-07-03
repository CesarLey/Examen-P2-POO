namespace EducationalCoursesAPI.Domain.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
} 