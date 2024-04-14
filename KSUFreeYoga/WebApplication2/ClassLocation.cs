namespace KSUFreeYogaAPI
{
    /// <summary>
    /// Represents a location
    /// </summary>
    public class ClassLocation
    {
        /// <summary>
        /// ID of location
        /// </summary>
        public int ClassLocationID { get; }

        /// <summary>
        /// Building where a class is held 
        /// </summary>
        public string? BuildingName { get; set; }

        /// <summary>
        /// Room number where class is held
        /// </summary>
        public string? RoomNumber { get; set; }

        /// <summary>
        /// Address of the location
        /// </summary>
        public string? LocationAddress { get; set; }
    }
}
