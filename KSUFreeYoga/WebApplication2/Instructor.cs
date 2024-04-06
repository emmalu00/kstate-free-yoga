using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSUFreeYogaAPI
{
    /// <summary>
    /// Represents an instructor
    /// </summary>
    public class Instructor
    {
        /// <summary>
        /// ID of instructor
        /// </summary>
        public int InstructorID { get; }

        /// <summary>
        /// Instructor's first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Instructor's last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Whether or not the instructor is a certified yoga instructor
        /// </summary>
        public bool Certified { get; set; }
    }
}
