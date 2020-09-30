using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VS.Core.DomainObjects
{
    public class Email
    {
        public const int EmailAddressMaxLength = 254;
        public const int EmailAddressMinLength = 5;
        public string EmailAddress { get; private set; }
        
        protected Email() { }
        public Email(string emailAddress)
        {
            if (!Validate(emailAddress)) throw new DomainException("E-mail isn't valid.");
            this.EmailAddress = emailAddress;
        }
        
        public bool Validate(string emailAddress)
        {
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isValid = Regex.IsMatch(emailAddress, regex, RegexOptions.IgnoreCase);
            return isValid;
        }
    }
}
