using System;

namespace Books_tools
{
    class Owner
    {
        string s_fname, s_lname, s_nick;
        pref _pref;

        public string FirstName { get => s_fname; set => s_fname = value; }
        public string LastName { get => s_lname; set => s_lname = value; }
        public string NickName { get => s_nick; set => s_nick = value; }
        public string DisplayName 
        { 
            get 
            {
                switch (_pref)
                {
                    case pref.FNames:
                        return FirstName;

                    case pref.LNames:
                        return LastName;

                    case pref.Nick:
                        return NickName;

                    default:
                        return FirstName;
                }
            }
        }


        public enum pref
            {
                FNames,
                LNames,
                Nick
            }

            public Owner(string name, pref displayName)
            {
            new Owner(name, "", displayName);
            }
        public Owner(string name, string lastName, pref displayName)
        {
            s_fname = name;
            s_lname = lastName;
            s_nick = "";
            _pref = displayName;
        }
        }
    
}
