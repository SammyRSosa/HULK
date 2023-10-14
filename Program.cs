namespace Project
{

    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {

            Scope mainscope = new Scope();
        

                System.Console.WriteLine("Bienvenido a HULK");

                while(true)
                {
                    string sampl = Console.ReadLine();

                    if (sampl == "break;")
                    {
                        break;
                    }
                    Proyecto project = new Proyecto(sampl,mainscope);
                    mainscope = project.p.scopeactual;

                }  
            

        }
        public class Proyecto
        {
            public Parser p;
            public Proyecto(string Code,Scope scope)
            {
                
                string sample = Code;
                TokenDefinition[] defs = new TokenDefinition[]
                {
                // Thanks to [steven levithan][2] for this great quoted string
                // regex
                new TokenDefinition(@"([??'])(?:\\\1|.)*?\1", "QUOTED-STRING"),
                new TokenDefinition(@"[0-9]+[a-z][0-9A-Za-z_]*", "BAD-SYMBOL"),
                // Thanks to http://www.regular-expressions.info/floatingpoint.html
                new TokenDefinition(@"\d*\.\d+([eE][-+]?\d+)?", "FLOAT"),
                new TokenDefinition(@"\d+", "INT"),
                new TokenDefinition(@"\+","ADD"),
                new TokenDefinition(@"\-","SUB"),
                new TokenDefinition(@"\*","MUL"),
                new TokenDefinition(@"\/","DIV"),
                new TokenDefinition(@"\%","REST"),
                new TokenDefinition(@"<","LESS"),
                new TokenDefinition(@"<=","LESS-EQUAL"),
                new TokenDefinition(@"=","EQUAL"),
                new TokenDefinition(@">","GREATER"),
                new TokenDefinition(@">=","GREATE-EQUAL"),
                new TokenDefinition(@"\||","OR"),
                new TokenDefinition(@"\&&","AND"),
                new TokenDefinition(@"\^","POW"),
                // new TokenDefinition(@"#t", "TRUE"),
                // new TokenDefinition(@"#f", "FALSE"),
                new TokenDefinition(@"(let|in|function|if|else)","KEYWORD"),
                new TokenDefinition(@"[a-z][0-9A-Za-z_]*", "SYMBOL"),

                new TokenDefinition(@"\,", "DOT"),
                new TokenDefinition(@"\.", "SEMI"),
                new TokenDefinition(@"\(", "LEFT"),
                new TokenDefinition(@"\)", "RIGHT"),
                new TokenDefinition(@"\s", "SPACE"),
                new TokenDefinition(@"\@", "AT"),
                new TokenDefinition(@"\;", "SEMI"),
                };

                TextReader r = new StringReader(sample);
                Lexer l = new Lexer(r, defs);
                
               try
               {
                    this.p = new Parser(l,scope);
                    dynamic m = p.MAthExpression.Evaluate();
               }
               catch (System.Exception message)
               {
                    System.Console.WriteLine($"{message.Message}");
               }
                

            }
        }
        public interface IMatcher
        {
            /// <summary>
            /// Return the number of characters that this "regex" or equivalent
            /// matches.
            /// </summary>
            /// <param name="text">The text to be matched</param>
            /// <returns>The number of characters that matched</returns>
            int Match(string text);
        }

        sealed class RegexMatcher : IMatcher
        {
            private readonly Regex regex;
            public RegexMatcher(string regex) => this.regex = new Regex(string.Format("^{0}", regex));

            public int Match(string text)
            {
                var m = regex.Match(text);
                return m.Success ? m.Length : 0;
            }
            public override string ToString() => regex.ToString();
        }

        public class TokenDefinition
        {
            public readonly IMatcher Matcher;
            public readonly string Type;

            public TokenDefinition(string regex, string type)
            {
                this.Matcher = new RegexMatcher(regex);
                this.Type = type;
            }
        }

        public class Lexer : IDisposable
        {
            private readonly TextReader reader;
            private readonly TokenDefinition[] tokenDefinitions;

            private string? lineRemaining;

            public Lexer(TextReader reader, TokenDefinition[] tokenDefinitions)
            {
                this.reader = reader;
                this.tokenDefinitions = tokenDefinitions;
                nextLine();
            }

            private void nextLine()
            {
                do
                {
                    lineRemaining = reader.ReadLine();
                    ++LineNumber;
                    Position = 0;
                } while (lineRemaining != null && lineRemaining.Length == 0);
            }

            public Token Next()
            {
                if (lineRemaining == null)
                    return null;
                foreach (var def in tokenDefinitions)
                {
                    var matched = def.Matcher.Match(lineRemaining);
                    if (matched > 0)
                    {
                        Position += matched;
                        Type = def.Type;
                        TokenContents = lineRemaining.Substring(0, matched);
                        lineRemaining = lineRemaining.Substring(matched);

                        if (lineRemaining.Length == 0)
                            nextLine();

                        if (Type == "SPACE")
                            return Next();

                        if(Type == "BAD-SYMBOL")
                        {
                            System.Console.WriteLine("found Bad Symbol");
                        }

                        return new Token(Type, TokenContents, Position); ;
                    }
                }
                throw new Exception(string.Format("Unable to match against any tokens at line {0} position {1} \"{2}\"",
                                                  LineNumber, Position, lineRemaining));
            }

            public string? TokenContents { get; private set; }
            public string Type { get; private set; }
            public int LineNumber { get; private set; }
            public int Position { get; private set; }

            public void Dispose() => reader.Dispose();
        }
    }
}