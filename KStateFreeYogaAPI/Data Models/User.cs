namespace KStateFreeYogaAPI
{
    public class User
    {
        public int UserID { get; }
        public string? GoogleID { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
