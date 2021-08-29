using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace BeatSouls_Translation
{
    public static class GameText
    {
        private static Dictionary<string, string>[] m_AppTexts;

        private static string ch_kan = null;
        public static string SChinese
        {
            get
            {
                if (ch_kan == null)
                {
                    var jsonSecrets = Properties.Resources.gametext_ch_kan;
                    //Stream stream = new MemoryStream(jsonSecrets);
                    ch_kan = Encoding.UTF8.GetString(jsonSecrets);

                    /*
                     Assembly assembly = Assembly.GetEntryAssembly();
                     var resPath = "BeatSouls_Translation.Resources.gametext_ch_kan.json";
                     resPath = assembly.GetManifestResourceNames()
                         .Single(str => str.EndsWith("gametext_ch_kan.json"));
                     Stream stream = assembly.GetManifestResourceStream(resPath);
                     if (stream == null)
                     {
                         Console.WriteLine(">>Unable to find resources");
                     }
                     else
                     {
                         using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                         {
                             ch_kan = reader.ReadToEnd();
                         }
                     }
                     */
                }
                return ch_kan;
            }
        }

        private static string gametext_jp = null;
        public static string Japanese
        {
            get
            {
                if (gametext_jp == null)
                {
                    var jsonSecrets = Properties.Resources.gametext_jp;
                    //Stream stream = new MemoryStream(jsonSecrets);
                    gametext_jp = Encoding.UTF8.GetString(jsonSecrets);
                }
                return gametext_jp;
            }
        }


        public static Dictionary<string, string>[] GetAppTexts()
        {
            if (m_AppTexts == null)
            {
                m_AppTexts = new Dictionary<string, string>[2];
                var json_SChinese = GameText.SChinese;
                var json_Japanese = GameText.Japanese;
                //System.Console.WriteLine(json);
                m_AppTexts[0] = JsonParser.Decode<Dictionary<string, string>>(json_SChinese);
                m_AppTexts[1] = JsonParser.Decode<Dictionary<string, string>>(json_Japanese);
            }

            return m_AppTexts;
        }
    }
}
