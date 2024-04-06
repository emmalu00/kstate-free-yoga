namespace KSUFreeYogaAPI
{
    /// <summary>
    /// Represents a yoga class
    /// </summary>
    public class YogaClass
    {
        /// <summary>
        /// ID of yoga class
        /// </summary>
        public int ClassID { get; }

        /// <summary>
        /// Yoga class name
        /// </summary>
        public string? ClassName {  get; set; }
        
        /// <summary>
        /// Yoga class start time
        /// </summary>
        public string? StartTime { get; set; } //change to Time type?

        /// <summary>
        /// Yoga class end time
        /// </summary>
        public string? EndTime { get; set; } //change to Time type?

        /// <summary>
        /// Yoga class date
        /// </summary>
        public string? ClassDate { get; set; } //change to DateTime type?

        /// <summary>
        /// ID of instructor teaching the class
        /// </summary>
        public int InstructorID { get; set; }

        /// <summary>
        /// ID of location where the class is held
        /// </summary>
        public int LocationID { get; set; }

        /// <summary>
        /// Whether or not mats are provided or not
        /// </summary>
        public bool MatsProvided { get; set; }

        /// <summary>
        /// Description of the yoga class
        /// </summary>
        public string? ClassDescription { get; set; }
    }
}
