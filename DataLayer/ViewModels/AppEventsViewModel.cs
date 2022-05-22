using DataLayer.Models;

namespace DataLayer.ViewModels
{
    public class AppEventsViewModel
    {
        List<AppEventsGroup> eventGroups;

        public int Count => eventGroups.Sum(e => e.Count);

        public IEnumerable<AppEventsGroup> EventGroups => eventGroups;

        public AppEventsViewModel(List<AppEventsGroup> input)
        {
            eventGroups = input;
        }
    }
}
