using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModels;

namespace FriendOrganizer.UI.Wrappers
{
    public class FriendWrapper: ModelWrapper<Friend>
    {

        public FriendWrapper(Friend model) : base(model)
        {
        }

        public int Id => GetValue<int>();


        public string FirstName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string LastName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string Email
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
                        yield return "Robots are not valid friends";
                    break;
            }
        }
    }
}