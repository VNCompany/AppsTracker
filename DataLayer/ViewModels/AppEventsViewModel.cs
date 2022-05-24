using DataLayer.Models;

namespace DataLayer.ViewModels
{
    public class AppEventsViewModel
    {
        List<AppEventsGroup> eventGroups;

        public int GroupsCount => eventGroups.Sum(e => e.Count);

        public int Count => eventGroups.Count;

        public IEnumerable<AppEventsGroup> EventGroups => eventGroups;

        public AppEventsViewModel(List<AppEventsGroup> input)
        {
            eventGroups = input;
        }
    }
}
