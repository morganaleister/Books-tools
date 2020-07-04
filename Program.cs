using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace Books_tools
{
    class Program
    {
        private static readonly string default_path = "settings\\";
        private static readonly string settings_file = default_path + "settings.json";
        private static readonly string langs_path = default_path + "langs\\";
        private static readonly string default_language = "ES-MX";
        private static readonly string[] default_commands
            = new string[] { "exit", "help", "titles" };

        private static string lang;
        private static string[] langs;
        private static string[] Lines;
        private static string[][] Commands;
        

        private static Book[] books;

        static void Main(string[] args)
        {
            Awake();
            Welcome();
            ReadBooksDB();
            PrintInstructions();
            WaitForInput();

        }

        private static void WaitForInput()
        {
            string issuedCmd = Console.ReadLine();


        }

        private static void Awake()
        {
            SettingsCheck();
            InitializeProgram();
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
                SetCommands(serializer,jsonReader);
            }
        }

        /// <summary>
        /// Checks out for the system directories and files to exist and creates them if needed.
        /// </summary>
        private static void SettingsCheck()
        {
            ///Check for settings directory to exist and creates it if not.
            if (!Directory.Exists(default_path)) Directory.CreateDirectory(default_path);

            ///Check for languages directory to exist, and creates one if not along with default language file
            if (!Directory.Exists(langs_path))
            {
                Directory.CreateDirectory(langs_path);

                

                //create and write default language json file
                using (FileStream fs = File.OpenWrite(langs_path + "ES-MX.json"))
                using (TextWriter tw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(tw))
                {
                    jw.WriteStartArray();
                    foreach (string s in new string[] //default language strings
                {
                    "Bienvenid@!!       Lang: ",
                    "El programa está cargando la base de datos, espera un momento por favor...",
                    "[.    ]",
                    "[. .  ]",
                    "[. . .]",
                    "Presiona [Enter] para enviar un comando",
                    "Escribe 'ayuda' para mostrar todos los comandos",
                    "Escribe 'titulos' (sin acento) para enlistar los libros por título",
                    "Escribe el número dentro de los corchetes ([#]) junto al título de un libro para mostrar más información del libro"

                })
                    {
                        jw.WriteValue(s);
                    }
                    jw.WriteEndArray();
                }
            }

            ///Check for settings file to exist, and creates one if not
            if (!File.Exists(settings_file))
            {

                using (FileStream fs = File.OpenWrite(settings_file))
                using (TextWriter tw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(tw))
                {
                    jw.WriteStartObject();

                    jw.WritePropertyName("settings");
                    jw.WriteStartArray();

                        jw.WriteStartObject();
                        jw.WritePropertyName("lang");
                            jw.WriteValue(default_language);
                        jw.WriteEndObject();

                        jw.WriteStartObject();
                        jw.WritePropertyName("available_langs");
                        jw.WriteStartArray();

                            WriteLangs(jw);

                        jw.WriteEndArray();
                        jw.WriteEndObject();

                    jw.WriteEndArray();

                    jw.WritePropertyName("commands");
                    jw.WriteStartArray();

                        WriteCommands(jw);

                    jw.WriteEndArray();
                }
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

                            break;
                        case "available_langs":

                            jr.Read();
                            langs = serializer.Deserialize<string[]>(jr);
                            return; //interrupts the While loop and terminates SetLangs Method
                    }
                }
            }
            

            ///Set langStrings Array from the determined language
            using (FileStream fs = File.OpenRead(langs_path + lang + ".json"))
            using (TextReader sr = new StreamReader(fs))
            using (JsonReader jr2 = new JsonTextReader(sr))
            {
                List<string> strList = new List<string>();
                while (jr2.Read())
                {
                    if(jr2.TokenType == JsonToken.String)
                    {
                        strList.Add((string)jr2.Value);
                    }
                }
                Lines = strList.ToArray();
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

                }
            }
        }

  
        private static void WriteCommands(JsonWriter jsonWriter)
        {
            for (int cmdNo = 0; cmdNo < default_commands.Length; cmdNo++)
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WritePropertyName(default_commands[cmdNo]);
                jsonWriter.WriteStartArray();
                    //empty array []
                jsonWriter.WriteEndArray();
                jsonWriter.WriteEndObject();
            }            
        }

        private static void WriteLangs(JsonWriter jsonWriter)
        {
            string[] langspaths = Directory.GetFiles(langs_path);

            for (int i = 0; i < langspaths.Length; i++)
            {
                jsonWriter.WriteValue(Path.GetFileNameWithoutExtension(langspaths[i]));
            }
            
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


        private static void PrintInstructions()
        {
            for (int i = 6; i < 10; i++)
            {
                Console.WriteLine(Lines[i]);
            }
        }

        private static void ClearLines(int LinesToClear = 1)
        {
            int currentLineCursor = Console.CursorTop - LinesToClear;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void Welcome()
        {
            Console.WriteLine(Lines[0]);
            Console.WriteLine(Lines[1]);
        }

        private static void ReadBooksDB()
        {

            JsonSerializer serializer = new JsonSerializer();
            Newtonsoft.Json.Linq.JObject jsonBook;

            List<Book> booklist = new List<Book>();

            using (FileStream filestrm = File.OpenRead("booksdb.json"))
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
    

