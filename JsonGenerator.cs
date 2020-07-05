using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Books_tools
{
    public static class JsonGenerator
    {
        internal static Dictionary<string, string[]> Lines = new Dictionary<string, string[]>();

        public enum FileTypes
        {
            en_us,
            es_mx,
            settings

        }

        public static void GenerateFile(string path, string fileName, FileTypes file, string ext = ".json")
        {
            try
            {
                //create and write default language json file
                using (FileStream fs = File.OpenWrite(path + fileName + ext))
                using (TextWriter tw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(tw))
                {
                    switch (file)
                    {
                        case FileTypes.es_mx:
                            GenerateES_MX(jw);
                            break;
                        case FileTypes.en_us:
                            GenerateEN_US(jw);
                            break;
                        case FileTypes.settings:
                            GenerateSettings(jw);
                            break;
                        default:
                            break;
                    }
                }
            }catch(UnauthorizedAccessException ex)
            {
                Console.WriteLine(Lines["Warnings"][4] + ":\n" + ex);
                Program.Terminate();
            }
        }

        private static void GenerateEN_US(JsonWriter jsonWriter)
        {
            Dictionary<string, string[]> DictTest = new Dictionary<string, string[]>();

            string[] WelcomeLines = 
            {
                "=====================================",
                "Lang:", 
                "Welcome to your Personal Library!!", 
                "Program is loading files, please wait..." 
            };
            string[] LoadingTextAnim =
            {
                "[.    ]",
                "[. .  ]",
                "[. . .]"
            };
            string[] IntroLines =
            {
                "Press [Enter] key to issue commands.",
                "Type 'help' to show more commands.",
            };
            string[] BasicHelpLines =
            {
                "Syntax Read: [ command identifier / alternative]: [ usage ] \n" +
                "[ - (modifier) ][ sub-command ]: [ usage ] \n",
                "[ exit / x ]: Exits the program",
                "[ help / h ]: Show this text screen",
                "[ list / l ]: Displays your registered books by Title.",
                "[ filter / f ]: Displays a filtered list, according to input parameters",
                "[ edit / add / modify / remove ]: Enters the Library Edit Mode",
                "[ settings / options ]: Displays current program settings"
            };
            string[] Warnings =
            {
                "=========Program Incomplete==========",
                "==========Vers. No. 0.0.1============",
                "=============Pre-Alpha===============",
                "===========Unknown Command===========",
                "UnauthorizedAccessException was thrown",
                "========Program terminated.\n Press any key to close console."
            };

            DictTest.Add("WelcomeLines", WelcomeLines);
            DictTest.Add("LoadingTextAnim", LoadingTextAnim);
            DictTest.Add("IntroLines", IntroLines);
            DictTest.Add("BasicHelpLines", BasicHelpLines);
            DictTest.Add("Warnings", Warnings);

            jsonWriter.WriteStartArray();
            foreach (var item in DictTest)
            {
                jsonWriter.WriteValue(item);
            }
            jsonWriter.WriteEndArray();
        }

        private static void GenerateES_MX(JsonWriter jsonWriter)
        {
            Dictionary<string, string[]> DictTest = new Dictionary<string, string[]>();

            string[] WelcomeLines =
            {
                "=====================================",
                "Idioma:",
                "Bienvenido a tu Biblioteca Personal!!",
                "Cargando archivos, por favor espera..."
            };
            string[] LoadingTextAnim =
            {
                "[.    ]",
                "[. .  ]",
                "[. . .]"
            };
            string[] IntroLines =
            {
                "Presiona [Enter] para enviar un comando.",
                "Escribe 'ayuda' para mostrar más comandos."
             

            };
            string[] BasicHelpLines =
            {
                "Léase: [ identificador / alternativo ) ]: [ descripción de uso ] \n" +
                "[ - (modificador) ][ sub-comando ]: [descripción de uso ] \n",
                "[ exit / x ]: Termina el programa",
                "[ help / h ]: Muestra éste texto",
                "[ list / l ]: Enlista los libros registrados.",
                "[ filter / f ]: Muestra una lista filtrada según los parámetros indicados.",
                "[ edit / add / remove ]: Accede al Modo Editar Biblioteca.",
                "[ settings / options ]: Muestra la configuración actual del programa."
            };
            string[] Warnings =
            {
                "=========Program Incomplete==========",
                "==========Vers. No. 0.0.1============",
                "=============Pre-Alpha===============",
                "========Comando Desconocido===========",
                "UnauthorizedAccessException",
                "========Programa terminado. \n Presione cualquier tecla para cerrar la consola."

            };
        }

        private static void GenerateSettings(JsonWriter jsonWriter)
        {
            
        }                   
    }
}
