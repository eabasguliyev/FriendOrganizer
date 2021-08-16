using Prism.Events;

namespace FriendOrganizer.UI.Events
{
    public class OpenDetailViewEvent:PubSubEvent<OpenDetailViewEventArgs>
    {
        
    }

    public class OpenDetailViewEventArgs
    {
        public int? Id { get; set; }
        public string ViewModelName { get; set; }
    }
}