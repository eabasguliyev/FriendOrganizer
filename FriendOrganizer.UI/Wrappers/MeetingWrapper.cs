using System;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrappers
{
    public class MeetingWrapper:ModelWrapper<Meeting>
    {
        public MeetingWrapper(Meeting model) : base(model)
        {
        }

        public int Id => GetValue<int>();

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public DateTime DateFrom
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);

                if (DateTo < DateFrom)
                    DateTo = DateFrom;
            }
        }

        public DateTime DateTo
        {
            get => GetValue<DateTime>();
            set
            {
                SetValue(value);
                if (DateTo < DateFrom)
                    DateFrom = DateTo;
            }
        }
    }
}