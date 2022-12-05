using System.Text;

namespace Steadsoft.Novus.Scanner.Classes
{
    /// <summary>
    /// Represents a source file as a collection of individual character items.
    /// </summary>
    public class SourceCode
    {
        public List<Character> Chars { get; private set; }
        public int Lines { get; private set; }
        public static SourceCode CreateFromText(string Text)
        {
            return new SourceCode(Text, SourceMode.Text);
        }
        public static SourceCode CreateFromFile(string Path)
        {
            return new SourceCode(Path, SourceMode.File);
        }
        private SourceCode(string Path, SourceMode Mode = SourceMode.File)
        {
            int line = 1;
            int col = 1;

            List<Character> source = new();

            if (Mode == SourceMode.File)
            {
                using FileStream fs = File.OpenRead(Path);
                using StreamReader sr = new(fs, Encoding.UTF8);
                while (!sr.EndOfStream)
                {
                    var C = (char)sr.Read();

                    var ch = new Character(C, line, col);

                    source.Add(ch);

                    col++;

                    if (ch.Char == '\n')
                    {
                        line++;
                        col = 1;
                    }
                }
            }
            else
            {
                var lines = Path.ToCharArray().Select(C => new Character(C, line, col));
                source.AddRange(lines);
            }

            Lines = line;

            Chars = source;
        }
    }
}