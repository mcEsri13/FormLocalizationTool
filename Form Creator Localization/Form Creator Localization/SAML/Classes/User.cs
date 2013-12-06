using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace URLRedirectAPI.Classes.SAML
{
    public class User
    {
        public string Name { get; set; }
        internal Dictionary<string, string> CustomAttributes { get; set; }
        internal Dictionary<string, string> GroupsList { get; set; }
        public Dictionary<string, string> Attributes;
        public Dictionary<string, string> Groups;

        public User()
        {
            CustomAttributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Attributes = new Dictionary<string, string>(CustomAttributes);
            GroupsList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Groups = new Dictionary<string, string>(GroupsList);
        }

        public User(string username) : this()
        {
            Name = username;
        }

        public bool IsMemberOf(string groupName)
        {
            return Groups.ContainsKey(groupName);
        }
        }
}
