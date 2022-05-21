namespace DataLayer.Models
{
    public class AppEvent
    {
        public int? AppId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }

        public AppEvent() { }
        public AppEvent(int appId, string name, string description)
        {
            AppId = appId;
            Name = name;
            Description = description;
            Date = DateTime.Now;
        }
    }
}