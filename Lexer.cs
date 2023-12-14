namespace Project
{
    using System.Text.RegularExpressions;
    public class Lexer
    {
        private readonly TextReader reader;
        private readonly TokenDefinition[] tokenDefinitions;
        private string? lineRemaining;

        public Lexer(TextReader reader, TokenDefinition[] definitions)
        {
            this.reader = reader;
            this.lineRemaining = reader.ReadLine();
            this.tokenDefinitions = definitions;
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

                    if (Type == "BAD-SYMBOL")
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
    }

    public class RegexMatcher
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
        public readonly RegexMatcher Matcher;
        public readonly string Type;

        public TokenDefinition(string regex, string type)
        {
            this.Matcher = new RegexMatcher(regex);
            this.Type = type;
        }
    }

    public class Token
    {
        public string Type { get; private set; }
        public int Position { get; private set; }
        public string Content { get; private set; }
        public Token(string Type, string Content, int Position)
        {
            this.Type = Type;
            this.Content = Content;
            this.Position = Position;
        }
    }
}