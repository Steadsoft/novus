using Steadsoft.Novus.Scanner.Classes;
using Steadsoft.Novus.Scanner.Enums;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Net.Http.Headers;
using Steadsoft.Novus.Scanner.Statics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Hardcode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string TokenDefinitionsFile = @"..\..\..\hardcode.csv";
            string SourceFile = @"..\..\..\langtest_en_1.hcl";
            string LanguageDictionary = @"..\..\..\lingua.keywords";

            Dictionary<string, string> E = new Dictionary<string, string> { { "ABC", "123" }, { "DEF", "456" } };
            Dictionary<string, string> F = new Dictionary<string, string> { { "UVW", "321" }, { "XYZ", "654" } };

            Dictionary<string, Dictionary<string, string>> all = new Dictionary<string, Dictionary<string, string>>();

            all.Add("E", E);
            all.Add("F", F);

            var xxx = JsonSerializer.Serialize(all);




            double a = 12_345_678.11;
            double b = 12_3_45__6_78.11;
            bool f = false;

            f = Double.TryParse("1.234e4", out var _);
            f = Double.TryParse("+0x7ff.3edp+1", out var _);

            var r = new Regex(@"([0-9a-fA-F]*\.[0-9a-fA-F]+|[0-9a-fA-F]+\.?)[Pp][-+]?[0-9]+[flFL]?");

            f = r.IsMatch("1.234p4");
            f = r.IsMatch("+7ff.3edp+1");

            List<Token> tokes = new List<Token>();

            // Create an input source from a source file.

            var input_source = SourceCode.CreateFromFile(SourceFile);

            // Create a token enumator from the souce using defintions from a file, skip comments as well.

            var token_enumerator = new TokenEnumerator<Keywords>(input_source, "en", TokenDefinitionsFile, LanguageDictionary, TokenOrigin.File, TokenAugmentation.ValidateToken, TokenType.BlockComment, TokenType.LineComment);

            var t = token_enumerator.GetNextToken(true);

            while (t.TokenType != TokenType.NoMoreTokens) 
            { 
                tokes.Add(t);

                t = token_enumerator.GetNextToken(true);

            }

            var goods = tokes.Where(t => t.IsInvalid == false).ToList();
            var fails = tokes.Where(t => t.IsInvalid ).ToList();


        }

    }
}