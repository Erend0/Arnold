using SQLite;
using System.Runtime.CompilerServices;

namespace NEA.Models
{
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
