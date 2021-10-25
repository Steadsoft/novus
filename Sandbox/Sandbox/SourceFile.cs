using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sandbox
{
    /// <summary>
    /// Represents a source file as a collection of individual character items.
    /// </summary>
    public class SourceFile
    {
        public List<Character> Chars { get; private set; }
        public int Lines { get; private set; }

        public static SourceFile Create(string Path)
        {
            return new SourceFile(Path);
        }
        private SourceFile(string Path)
        {
            int line = 1;
            int col = 1;

            List<Character> source = new List<Character>();

            using (FileStream fs = File.OpenRead(Path))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    while (!sr.EndOfStream)
                    {
                        var C = (char)(sr.Read());

                        var ch = (new Character(C, line, col));

                        source.Add(ch);

                        col++;

                        if (ch.Kind == Kind.LF)
                        {
                            line++;
                            col = 1;
                        }
                    }
                }
            }

            Lines = line;

            Chars = source;
        }
    }
}