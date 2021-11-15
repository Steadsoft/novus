using System.Text;

namespace Steadsoft.Novus.Scanner.Classes
{
    /// <summary>
    /// Represents a source file as a collection of individual character items.
    /// </summary>
    public class SourceFile
    {
        public List<Character> Chars { get; private set; }
        public int Lines { get; private set; }
        public static SourceFile CreateFromString(string Text)
        {
            return new SourceFile(Text, false);
        }
        public static SourceFile CreateFromFile(string Path)
        {
            return new SourceFile(Path, true);
        }
        private SourceFile(string Path, bool FromFile = true)
        {
            

            int line = 1;
            int col = 1;

            List<Character> source = new();

            if (FromFile)
            {
                using (FileStream fs = File.OpenRead(Path))
                {
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