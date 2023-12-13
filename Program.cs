namespace Project
{

    using System.Text.RegularExpressions;

    public class Program
    {
        public static void Main(string[] args)
        {

            Scope mainscope = new Scope();


            System.Console.WriteLine("Bienvenido a HULK");

            while (true)
            {
                string sampl = Console.ReadLine();

                if (sampl != "")
                {
                    if (sampl[sampl.Length - 1] == ';')
                    {
                        if (sampl == "break;")
                        {
                            break;
                        }
                        try
                        {
                            Proyecto project = new Proyecto(sampl, mainscope);
                            mainscope = project.parser.scopeactual;
                        }
                        catch (System.Exception)
                        {

                        }
                    }
                    else
                    {
                        System.Console.WriteLine("; expected");
                    }
                }
            }


        }
        public class Proyecto
        {
            public Parser parser;
            public Lexer lexer;
            public Proyecto(string Code, Scope scope)
            {

                string sample = Code;
                TokenDefinition[] defs = new TokenDefinition[]
                {
                new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", "QUOTED-STRING"),
                new TokenDefinition(@"[0-9]+[a-z][0-9A-Za-z_]*", "BAD-SYMBOL"),
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
                this.lexer = new Lexer(r,defs);

                try
                {
                    this.parser = new Parser(lexer, scope);
                    dynamic m = parser.MAthExpression.Evaluate();
                }
                catch (System.Exception message)
                {
                    System.Console.WriteLine($"{message.Message}");
                }


            }
        }


    }
}