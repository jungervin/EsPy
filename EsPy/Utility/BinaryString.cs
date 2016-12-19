using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class BinaryString
    {

        //  Escape Sequence       Meaning 
        // ================================================
        //  \newline              Ignored
        //  \\     	              Backslash(\)
        //  \'     	              Single quote (')
        //  \"     	              Double quote (")
        //  \a                    ASCII Bell(BEL)
        //  \b                    ASCII Backspace(BS)
        //  \f                    ASCII Formfeed(FF)
        //  \n                    ASCII Linefeed(LF)
        //  \r                    ASCII Carriage Return(CR)
        //  \t                    ASCII Horizontal Tab(TAB)
        //  \v                    ASCII Vertical Tab(VT)
        //  \ooo                  ASCII character with octal value ooo
        //  \xhh                  ASCII character with hex value hh


        // b'binarystring'

        // b'\\x00\\x01\\x02\\x03\\x04\\x05\\x06\\x07\\x08\\t\\n\\x0b\\x0c\\r\\x0e\\x0f\\x10\\x11\\x12\\x13\\x14\\x15\\x16\\x17\\x18\\x19\\x1a\\x1b\\x1c\\x1d\\x1e\\x1f !\"#$%&\\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\\x7f\\x80\\x81\\x82\\x83\\x84\\x85\\x86\\x87\\x88\\x89\\x8a\\x8b\\x8c\\x8d\\x8e\\x8f\\x90\\x91\\x92\\x93\\x94\\x95\\x96\\x97\\x98\\x99\\x9a\\x9b\\x9c\\x9d\\x9e\\x9f\\xa0\\xa1\\xa2\\xa3\\xa4\\xa5\\xa6\\xa7\\xa8\\xa9\\xaa\\xab\\xac\\xad\\xae\\xaf\\xb0\\xb1\\xb2\\xb3\\xb4\\xb5\\xb6\\xb7\\xb8\\xb9\\xba\\xbb\\xbc\\xbd\\xbe\\xbf\\xc0\\xc1\\xc2\\xc3\\xc4\\xc5\\xc6\\xc7\\xc8\\xc9\\xca\\xcb\\xcc\\xcd\\xce\\xcf\\xd0\\xd1\\xd2\\xd3\\xd4\\xd5\\xd6\\xd7\\xd8\\xd9\\xda\\xdb\\xdc\\xdd\\xde\\xdf\\xe0\\xe1\\xe2\\xe3\\xe4\\xe5\\xe6\\xe7\\xe8\\xe9\\xea\\xeb\\xec\\xed\\xee\\xef\\xf0\\xf1\\xf2\\xf3\\xf4\\xf5\\xf6\\xf7\\xf8\\xf9\\xfa\\xfb\\xfc\\xfd\\xfe\\xff'

        public static byte[] Text2Bin(string text)
        {
            MemoryStream ms = new MemoryStream();

            for (int i = 0; i < text.Length;)
            {
                if ((text[i] == '\\' || text[i] < 32 || text[i] >= 127) && (i + 1 < text.Length))
                {
                    i++;
                    switch (text[i])
                    {
                        case '\\':
                            ms.WriteByte(92);
                            i++;
                            break;

                        case '\'':
                            ms.WriteByte(39);
                            i++;
                            break;

                        case '"':
                            ms.WriteByte(34);
                            i++;
                            break;

                        //case 'a':
                        //    ms.WriteByte(7);
                        //    i++;
                        //    break;

                        case 'b':
                            ms.WriteByte(8);
                            i++;
                            break;

                        case 't':
                            ms.WriteByte(9);
                            i++;
                            break;

                        case 'n':
                            ms.WriteByte(10);
                            i++;
                            break;

                        //case 'v':
                        //    ms.WriteByte(11);
                        //    i++;
                        //    break;

                        //case 'f':
                        //    ms.WriteByte(12);
                        //    i++;
                        //    break;

                        case 'r':
                            ms.WriteByte(13);
                            i++;
                            break;

                        case 'x':
                            i++;
                            string hex = text.Substring(i, 2);
                            ms.WriteByte(Convert.ToByte(hex, 16));
                            i += 2;
                            break;

                        default:
                            ms.WriteByte((byte)text[i]);
                            //i++;
                            break;
                    }
                }
                else
                {
                    byte c = (byte)text[i];
                    ms.WriteByte(c);
                    i++;
                    
                }
            }
            byte[] res = new byte[ms.Length];
            ms.Position=0;
            Array.Copy(ms.GetBuffer(), res, ms.Length);
            
            return res;
        }

        public static string BinToString(byte[] buff, int offset, int count)
        {
            string res = "b'";

            for (int i = 0; i < count; i++)
            {
                // Todo: escape characters
                res += "\\x" + buff[offset + i].ToString("x2");
            }
            return res + "'";
        }
    }
}
