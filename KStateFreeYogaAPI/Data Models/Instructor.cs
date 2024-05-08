namespace KStateFreeYogaAPI
{
    /// <summary>
    /// Object that represents an instructor
    /// </summary>
    public class Instructor
    {
        /// <summary>
        /// Unique ID that represents a specific instructor
        /// </summary>
        public int InstructorID { get; }

        /// <summary>
        /// Instructor first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Instructor last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Whether or not instructor is a certified yoga instructor
        /// </summary>
        public bool Certified { get; set; }
    }
}
