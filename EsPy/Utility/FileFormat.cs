using EsPy.Forms;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsPy.Utility
{
    public class FileFormat
    {
        public Type EditorType = null;
        public Lexer Lexer = Lexer.Null;
        public string Language = "";
        public string Filter = "";
        public string DefaultExt = "";

        public FileFormat(Type editor_type, string language, Lexer lexer, string filter, string default_ext)
        {
            this.EditorType = editor_type;
            this.Language = language;
            this.Lexer = lexer;
            this.Filter = filter;
            this.DefaultExt = default_ext;
        }

        public string[] Filters
        {
            get
            {
                string[] items = this.Filter.Split('|')[1].Split(';');
                return items;
            }
        }


        public static FileFormat Python = new FileFormat(typeof(EditorForm), "Python", Lexer.Python, "Python files(*.py)|*.py", ".py");

        // WEB
        public static FileFormat Html = new FileFormat(typeof(EditorForm), "Html", Lexer.Html, "HTML files (*.htm; *.html)|*.htm;*.html", ".html");
        public static FileFormat Js = new FileFormat(typeof(EditorForm), "Javascript.js", Lexer.Cpp, "JavaScript files (*.js)|*.js", ".js");
        public static FileFormat Css = new FileFormat(typeof(EditorForm), "Css", Lexer.Css, "CSS files (*.css)|*.css", ".css");
        public static FileFormat Json = new FileFormat(typeof(EditorForm), "Json", Lexer.Null, "JSON files (*.json)|*.json", ".json");
        public static FileFormat Jpg = new FileFormat(null, "", Lexer.Null, "JPEG files (*.jpg; *jpeg)|*.jpg;*.jpeg", ".jpg");
        public static FileFormat Png = new FileFormat(null, "", Lexer.Null, "PNG files (*.png)|*.png", ".png");
        public static FileFormat Gif = new FileFormat(null, "", Lexer.Null, "Gif files (*.gif)|*.gif", ".gif");

        //Others
        public static FileFormat Xml = new FileFormat(typeof(EditorForm), "Xml", Lexer.Xml, "XML files (*.xml)|*.xml", ".xml");
        public static FileFormat Txt = new FileFormat(typeof(EditorForm), "", Lexer.Null, "Text files (*.txt)|*.txt", ".txt");
        public static FileFormat All = new FileFormat(typeof(EditorForm), "", Lexer.Null, "All files (*.*)|*.*", "");

    }
}
