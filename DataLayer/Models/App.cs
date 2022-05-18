namespace DataLayer.Models
{
    public class App
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int OwnerId { get; set; }
        public DateTime Date { get; set; }

        public App() { }
        public App(int id, string name, int owner_id, DateTime date)
        {
            Id = id;
            Name = name;
            OwnerId = owner_id;
            Date = date;
        }
    }
}