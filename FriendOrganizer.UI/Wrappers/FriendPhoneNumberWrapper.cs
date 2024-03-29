﻿using System.Collections.Generic;
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
    }
}