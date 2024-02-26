namespace KSUFreeYoga.Server
{
    public class YogaClass
    {
        public int ClassID { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public string InstructorLastName { get; set; } = string.Empty;

        public int LocationID { get; set; }

        public bool MatsAvailable { get; set; }
        public string Description { get;  set; } = string.Empty;

        

    }
}
