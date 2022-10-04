using SQLite;


namespace NEA
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool subscribed { get; set; }


    }
}
