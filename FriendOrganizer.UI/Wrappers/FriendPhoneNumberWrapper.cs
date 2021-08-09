using System.Collections.Generic;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Wrappers
{
    public class FriendPhoneNumberWrapper:ModelWrapper<FriendPhoneNumber>
    {
        public FriendPhoneNumberWrapper(FriendPhoneNumber model) : base(model)
        {

        }

        public string Number
        {
            get => GetValue<string>(); 
            set => SetValue(value);
        }

        //protected override IEnumerable<string> ValidateProperty(string propertyName)
        //{
        //    switch (propertyName)
        //    {
        //        case nameof(FirstName):
        //            if (string.Equals(FirstName, "Robot", StringComparison.OrdinalIgnoreCase))
        //                yield return "Robots are not valid friends";
        //            break;
        //    }
        //}
    }
}