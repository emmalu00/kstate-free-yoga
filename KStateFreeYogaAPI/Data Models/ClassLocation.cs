namespace KStateFreeYogaAPI
{
    /// <summary>
    /// Object that represents a class location
    /// </summary>
    public class ClassLocation
    {
        /// <summary>
        /// Unique ID that represents a specific location
        /// </summary>
        public int LocationID { get; }

        /// <summary>
        /// Building name where class is held
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
