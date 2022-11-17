using SQLite;
namespace NEA.Models
{
    // This class is used to store the user's login information
    [Table("Users")]
    public class User
    {
        [AutoIncrement, PrimaryKey]     
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int UserPin { get; set; }
        public bool HasLoggedIn { get; set; }
    }
}
