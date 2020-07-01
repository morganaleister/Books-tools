using System;

namespace Books_tools
{
    class Program
    {
        class Book
        {
            string s_title, s_authorF, s_authorL, s_format;
            bool b_read;
            int i_pages;
            Owner o_owner;

            public Book()
            {
                throw new InvalidOperationException("This is an invalid instance creation. Please use any of the other constructor methods.");
            }

            public Book(string title)
            {
                new Book(title, "Físico");
            }

            public Book(string title, string format)
            {
                s_title = title;
                s_authorF = "nd";
                s_authorL = "nd";
                s_format = format;
                b_read = false;
                i_pages = 0;
            }
        }
        class Owner
        {
            string s_fname, s_lname, s_nick;
            pref _pref;
            public enum pref
            {
                FNames,
                LNames,
                Nick
            }

            public Owner()
            {
                throw new InvalidOperationException("This is an invalid instance creation. Please use any of the other constructor methods.");
            }

            public Owner(string name, pref prefDisplay)
            {
                s_fname = name;
                s_lname = "";
                s_nick = "";
                _pref = prefDisplay;
            }
        }

        Book[] books;
        Owner[] owners;

        static void Main(string[] args)
        {
            Welcome();
            ReadFiles();

        }

        private static void Welcome()
        {
            Console.WriteLine("Bienvenid@!!");
            Console.WriteLine("El programa está cargando la base de datos, espera un momento por favor...");
            

        }

        private static void ReadFiles()
        {
            ReadBooks();
            ReadOwners();
            
        }

        /// <summary>
        /// Returns a [LastName][comma][space][FirstName][period] string
        /// </summary>
        /// <returns>string. [LastName][comma][space][FirstName][period]</returns>
        public static string GetFullNameAuthor(string firstName, string lastName)
        {
            return string.Format("{0}, {1}.", lastName, firstName);
        }
        /// <summary>
        /// Returns a [FirstName][space][LastName] string
        /// </summary>
        /// <returns>string. [FirstName][space][LastName]</returns>
        public static string GetFullNameOwner(string firstName, string lastName)
        {
            return string.Format("{0} {1}", firstName, lastName);
        }

        private static void ReadOwners()
        {
            throw new NotImplementedException();
        }

        private static void ReadBooks()
        {
            throw new NotImplementedException();
        }

        private static void GiveRandom()
        {
            throw new NotImplementedException();
        }
    }
    
}
