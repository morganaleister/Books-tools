using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace Books_tools
{
    class Program
    {
        internal static readonly string default_path = "settings\\";
        internal static readonly string books_path = default_path + "booksdb.json";
        internal static readonly string settings_file = default_path + "settings.json";
        internal static readonly string langs_path = default_path + "langs\\";
        internal static readonly string default_language = "ES-MX";
        internal static readonly string[] default_commands
            = new string[] { "exit", "help", "list", "filter", "edit", "settings" };

        private static string lang;
        private static string[] langs;
        private static Dictionary<string,string[]> Lines;
        private static string[][] Commands;

        private enum IssuableCommands
        {
            none = -1,
            exit, 
            help,
            list,
            filter, 
            edit, 
            settings
        }
        

        private static Book[] books;

        static void Main(string[] args)
        {
            Awake();
            Welcome();
            ReadBooksDB();
            PrintInstructions();
            WaitForInput();

        }
        private static void Awake()
        {
            SettingsCheck();
            InitializeProgram();
        }
        /// <summary>
        /// Checks out for the system directories and files to exist and creates them if needed.
        /// </summary>
        private static void SettingsCheck()
        {
            ///Check for settings directory to exist and creates it if not.
            if (!Directory.Exists(default_path)) Directory.CreateDirectory(default_path);

            ///Check for languages directory to exist, and creates one if not along with default language file
            if (!Directory.Exists(langs_path)) Directory.CreateDirectory(langs_path);

            if (Directory.GetFiles(langs_path).Length < 2)
            {
                JsonGenerator.GenerateFile(langs_path, lang, JsonGenerator.FileTypes.es_mx);
                JsonGenerator.GenerateFile(langs_path, lang, JsonGenerator.FileTypes.en_us);
            }
            ///Check for settings file to exist, and creates one if not
            if (!File.Exists(settings_file))
            {

            }

            
        }

        
        
        private static void InitializeProgram()
        {
            JsonSerializer serializer = new JsonSerializer();
            ///Reads settings.json and set program initial settings
            using (FileStream fs = File.OpenRead(settings_file))
            using (TextReader sr = new StreamReader(fs))
            using (JsonReader jsonReader = new JsonTextReader(sr))
            {

                SetLangs(serializer, jsonReader);
                SetCommands(serializer, jsonReader);
            }
        }
        /// <summary>
        /// Initializes Program's Lang from settings
        /// </summary>
        private static void SetLangs(JsonSerializer serializer, JsonReader jr)
        {
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.PropertyName)
                {
                    switch ((string)jr.Value)
                    {
                        case "lang":
                            jr.Read();
                            lang = serializer.Deserialize<string>(jr);

                            SetLines();

                            break;
                        case "available_langs":

                            jr.Read();
                            langs = serializer.Deserialize<string[]>(jr);
                            return;
                    }
                }
            }
        }
        private static void SetLines()
        {
            ///Set Lines Array from the determined language
            using (FileStream fs = File.OpenRead(langs_path + lang + ".json"))
            using (TextReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                List<string> strList = new List<string>();
                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.String)
                    {
                        strList.Add((string)jr.Value);
                    }
                }
                //Lines = strList.ToArray();
            }
        }
        /// <summary>
        /// Initilalizes Program's Commands from settings
        /// </summary>
        private static void SetCommands(JsonSerializer serializer, JsonReader jr)
        {
            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.PropertyName
                    && (string)jr.Value == "commands")
                {

                    List<string[]> tempCmds = new List<string[]>();
                    int cmdcnt = 0;

                    do
                    {
                        jr.Read();
                        if (jr.TokenType == JsonToken.PropertyName
                            && (string)jr.Value == default_commands[cmdcnt])
                        {
                            List<string> tmpcmd = new List<string>();
                            tmpcmd.Add((string)jr.Value);

                            jr.Read();
                            tmpcmd.AddRange(serializer.Deserialize<string[]>(jr));

                            tempCmds.Add(tmpcmd.ToArray());
                            cmdcnt++;
                        }


                    } while (cmdcnt < default_commands.Length);

                    Commands = tempCmds.ToArray();
                    return;

                }
            }
        }

        private static void Welcome()
        {
         
            Console.WriteLine(Lines["WelcomeLines"][1] + lang);
            Console.WriteLine(Lines["WelcomeLines"][0]);
            Console.WriteLine(Lines["WelcomeLines"][2]);
            Console.WriteLine(Lines["WelcomeLines"][3]);
        }

        private static void ReadBooksDB()
        {

            JsonSerializer serializer = new JsonSerializer();
            Newtonsoft.Json.Linq.JObject jsonBook;

            List<Book> booklist = new List<Book>();

            using (FileStream filestrm = File.OpenRead(books_path))
            using (StreamReader streamrdr = new StreamReader(filestrm))
            using (JsonReader jreadr = new JsonTextReader(streamrdr))
            {
                while (jreadr.Read())
                {

                    // deserialize only when there's "{" character in the stream
                    if (jreadr.TokenType == JsonToken.StartObject)
                    {
                        jsonBook = (Newtonsoft.Json.Linq.JObject)serializer.Deserialize(jreadr);
                        Book b = new Book();

                        b.Read = jsonBook.Property("@read").ToObject<bool>();
                        b.Title = jsonBook.Property("Title").ToObject<string>();
                        b.Subtitle = jsonBook.Property("Subtitle").ToObject<string>();
                        b.Author_FirstName = jsonBook.Property("Author").Value.Value<string>("FName");
                        b.Author_LastName = jsonBook.Property("Author").Value.Value<string>("LName");
                        b.Pages = jsonBook.Property("Pages").ToObject<int>();
                        b.Book_Format = jsonBook.Property("Format").ToObject<string>();

                        booklist.Add(b);
                    }

                }
            }

            books = booklist.ToArray();
        }
        private static void PrintInstructions()
        {
            //for (int i = 5; i < Lines.Length; i++)
            //{
            //    Console.WriteLine(Lines[i]);
            //}
            //Console.WriteLine("\nIssue Command:");
        }
        private static void WaitForInput()
        {
            IssuableCommands issuedCmd = IssuableCommands.none;

            while (true)
            {
                string preCmd = Console.ReadLine().Replace(" ", "").ToLowerInvariant();

                bool broke = false;
                for (int i = 0; i < Commands.Length; i++)
                {
                    if (broke) break;
                    for (int j = 0; j < Commands[i].Length; j++)
                    {
                        if (preCmd == Commands[i][j])
                        {
                            issuedCmd = (IssuableCommands)i;
                            broke = true;
                            break;
                        }
                    }
                }

                DoCommand(issuedCmd);
            }
        }

        private static void DoCommand(IssuableCommands command)
        {
            switch (command)
            {                
                case IssuableCommands.exit:
                    Terminate();
                    break;
                case IssuableCommands.help:
                    break;
                case IssuableCommands.list:
                    break;
                case IssuableCommands.filter:
                    break;
                case IssuableCommands.edit:
                    break;
                case IssuableCommands.settings:
                    break;
                default:
                    Console.WriteLine(Lines["Warnings"][3]);
                    break;
            }
        }

        public static void Terminate()
        {
            //Do cleanup, save,etc PENDING

            //~bybymsg~
            Console.WriteLine(Lines["Warnings"][5]);
            Console.ReadKey();

            //~close~
            Environment.Exit(0);
        }

        private static void UpdateLangs()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (FileStream fs = File.Open(settings_file, FileMode.Open, 
                FileAccess.ReadWrite, FileShare.None))
            using (TextReader tr = new StreamReader(fs))
            using (TextWriter tw = new StreamWriter(fs)) 
            using(JsonReader jr = new JsonTextReader(tr))
            using (JsonWriter jw = new JsonTextWriter(tw)) 
            {

                while (jr.Read())
                {
                    if ((string)jr.Value == "available_langs")
                    {
                        jr.Read();
                        
                    }
                }


            }
        }
              
        private static void ClearLines(int LinesToClear = 1)
        {
            int currentLineCursor = Console.CursorTop - LinesToClear;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
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




        private static void GiveRandom()
        {
            throw new NotImplementedException();
        }
    }
}
    

