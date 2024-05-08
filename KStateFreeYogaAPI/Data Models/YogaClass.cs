namespace KStateFreeYogaAPI
{
    /// <summary>
    /// Object that represents a yoga class
    /// </summary>
    public class YogaClass
    {
        /// <summary>
        /// Unique ID that represents a specific yoga class
        /// </summary>
        public int ClassID { get; }

        /// <summary>
        /// Name of the yoga class
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Class start time
        /// </summary>
        public string? StartTime { get; set; }

        /// <summary>
        /// Class end time
        /// </summary>
        public string? EndTime { get; set; }

        /// <summary>
        /// Class date
        /// </summary>
        public string? ClassDate { get; set; }

        /// <summary>
        /// Unique ID of instructor teaching the class
        /// </summary>
        public int InstructorID { get; set; }

        /// <summary>
        /// Unique ID of location where class is held
        /// </summary>
        public int LocationID { get; set; }

        /// <summary>
        /// Whether or not mats are provided
        /// </summary>
        public bool MatsProvided { get; set; }

        /// <summary>
        /// Description of the class
        /// </summary>
        public string? ClassDescription { get; set; }

    }
}
