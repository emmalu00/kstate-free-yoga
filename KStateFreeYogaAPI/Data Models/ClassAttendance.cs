namespace KStateFreeYogaAPI
{
    /// <summary>
    /// Object that represents an attendance record
    /// </summary>
    public class ClassAttendance
    {
        /// <summary>
        /// Unique ID that represents a specific attendance record
        /// </summary>
        public int AttendanceID { get; }

        /// <summary>
        /// Unique ID of user
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Unique ID of yoga class
        /// </summary>
        public int ClassID { get; set; }

        /// <summary>
        /// Class date
        /// </summary>
        public string? AttendanceDate { get; set; }

        /// <summary>
        /// Status of attendance (attended or cancelled)
        /// </summary>
        public string? AttendanceStatus { get; set; }

        /// <summary>
        /// Whether or not the class is favorited by user
        /// </summary>
        public bool Favorited { get; set; }
    }
}
