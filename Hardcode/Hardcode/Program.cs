using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Hardcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Token> tokes = new List<Token>();

            var source = SourceFile.CreateFromFile(@"..\..\..\tokens_test_01.hcl");

            var tokenizer = new Tokenizer<Keywords>(@"..\..\..\hardcode.csv", TokenDefinition.Pathname, Assembly.GetExecutingAssembly());
            var tokens = new TokenEnumerator(tokenizer, source, TokenType.BlockComment, TokenType.LineComment);

            var t = tokens.GetNextToken();

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = tokens.GetNextToken();
            }
        }
    }
}