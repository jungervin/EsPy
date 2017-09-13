using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class TextHelper
    {
        public class Words
        {
            public int Pos = 0;
            public string Word = "";
            public string Filter = "";
        }

        public static string[] Matches(string pattern, string text)
        {
            Regex re = new Regex(pattern);
            if (re.IsMatch(text))
            {
                MatchCollection items = re.Matches(text);
                string[] res = new string[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    res[i] = items[i].Groups[1].Value;
                }
                return res;
            }
            return null;
        }

        public static Words FindWords(string text, int from)
        {
            Words words = new Words();
            if (from >= 1)
            {
                words.Pos = from;
                from--;
                int dot = 0;

                while (from >= 0)
                {
                    char c = text[from];

                    if (c == '\r' || c == '\n')
                    {
                        from--;
                        continue;
                    }
                    else if (c == '.')
                    {
                        dot++;
                    }

                    if (c == '.' || c == '_' || char.IsLetterOrDigit(c))
                    {
                        if (dot == 0)
                        {
                            words.Filter = c + words.Filter;
                            words.Pos = from;
                        }
                        else
                        {
                            words.Word = c + words.Word;
                        };

                    }
                    else break;
                    from--;
                }
            }
            return words;
        }

        public static int KeywordStartPosition(string text, int pos)
        {
            while (pos > 0)
            {
                if (text[pos] == '_' || char.IsLetterOrDigit(text[pos]))
                    pos--;
                else break;
            }
            return pos;
        }

        public static int KeywordEndPosition(string text, int pos)
        {
            while (pos < text.Length)
            {
                if (text[pos] == '_' || char.IsLetterOrDigit(text[pos]))
                    pos++;
                else break;
            }
            return pos;
        }
    }
}
