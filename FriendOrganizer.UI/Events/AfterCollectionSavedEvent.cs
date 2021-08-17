using Prism.Events;

namespace FriendOrganizer.UI.Events
{
    public class AfterCollectionSavedEvent:PubSubEvent<AfterCollectionSavedEventArgs>
    {
        
    }

    public class AfterCollectionSavedEventArgs
    {
        public string ViewModelName { get; set; }
    }
}