using System;

namespace Books_tools
{
    class Book
    {
        bool b_read;
        string s_title, s_authorF, s_authorL, s_format, s_subTitle;
        int i_pages;
        Owner o_owner;

        public bool Read { get => b_read; set => b_read = value; }
        public string Title { get => s_title; set => s_title = value; }
        public string Author_FirstName{ get => s_authorF; set => s_authorF = value; }
        public string Author_LastName { get => s_authorL; set => s_authorL = value; }
        public string Book_Format { get => s_format; set => s_format = value; }
        public string Subtitle { get => s_subTitle; set => s_subTitle = value; }
        public int Pages { get => i_pages; set => i_pages = value; }

        public Owner Owner { get => o_owner; set => o_owner = value; }


        public Book()
        {
            new Book("nd");
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

}
