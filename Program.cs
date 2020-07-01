using System;

namespace Books_tools
{
    class Program
    {
        struct Book
        {
            string s_title, s_author;
            bool b_read;

            public Book(string title, string author = "n/d", bool read = false)
            {
                s_title = title;
                s_author = author;
                b_read = read;
            }
        }
        struct Owner
        {
            string s_fname, s_lname;
            public Owner(string names, string lastNames = "")
            {
                s_fname = names;
                s_lname = lastNames;
            }
        }

        Book[] books;
        string[] titles, fnames,lnames;
        bool[] states;
        int[] ppages;

        static void Main(string[] args)
        {
            ReadFile();

        }

        private static void ReadFile()
        {
            throw new NotImplementedException();
        }

        private static void GiveRandom()
        {
            throw new NotImplementedException();
        }
    }
    
}
