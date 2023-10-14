namespace Project
{



    public class Parser
    {
       
        public Program.Lexer l;
        public Token lookahead;
        public ExpressionNode MAthExpression;
        public Scope scopeactual;


        public Parser(Program.Lexer l,Scope scope)
        {
            this.scopeactual = scope;
            this.l = l;
            this.lookahead = l.Next();
            this.MAthExpression = Expression();
        }



        public Token eat(string Type)
        {

            Token token = this.lookahead;
            if (token == null)
                throw new Exception("expected");

            if (token.Type != Type)
                throw new Exception($"{Type} expected");


            this.lookahead = this.l.Next();

            return token;

        }


        public ExpressionNode Expression()
        {
            ExpressionNode left = this.ArigmeticalExpression();

            while(this.lookahead != null && (this.lookahead.Type == "LESS" || this.lookahead.Type == "LESS-EQUAL" || this.lookahead.Type == "EQUAL" || this.lookahead.Type == "GREATER" || this.lookahead.Type == "GREATER-EQUAL"))
            {
                if (this.lookahead.Type == "LESS")
                {
                    this.eat("LESS");
                    ExpressionNode right = this.ArigmeticalExpression();
                    left = new LessNode(left,right);
                }
                else if (this.lookahead.Type == "LESS-EQUAL")
                {
                    this.eat("LESS-EQUAL");
                    ExpressionNode right = this.ArigmeticalExpression();
                    left = new LessEqualNode(left,right);
                }
                else if (this.lookahead.Type == "EQUAL")
                {
                    this.eat("EQUAL");
                    ExpressionNode right = this.ArigmeticalExpression();
                    left = new EqualNode(left,right);
                }
                else  if (this.lookahead.Type == "GREATER-EQUAL")
                {
                    this.eat("GREATER-EQUAL");
                    ExpressionNode right = this.ArigmeticalExpression();
                    left = new GreaterEqualNode(left,right);
                }else  if (this.lookahead.Type == "GREATER")
                {
                    this.eat("GREATER");
                    ExpressionNode right = this.ArigmeticalExpression();
                    left = new GreaterNode(left,right);
                }

            }
            return left;
        }

        public ExpressionNode ArigmeticalExpression()
        {
            ExpressionNode left = this.Term();

            while (this.lookahead != null && (this.lookahead.Type == "ADD" || this.lookahead.Type == "SUB"))
            {

                if (this.lookahead.Type == "ADD")
                {
                    this.eat("ADD");
                    ExpressionNode right = this.Term();
                    left = new AddNode(left, right);

                }
                else
                {
                    this.eat("SUB");
                    ExpressionNode right = this.Term();
                    left = new SubNode(left, right);
                }
            }

            return left;
        }

        public ExpressionNode Primary()
        {   
            if (this.lookahead.Type == "SYMBOL")
            {
                
                if (this.lookahead.Content == "print")
                {
                    eat("SYMBOL");
                    eat("LEFT");

                    ExpressionNode respuesta = this.Expression();

                    eat("RIGHT");

                    return new Printnode(respuesta);
                }
                
                if (this.lookahead.Content == "cos"|| this.lookahead.Content == "sin" || this.lookahead.Content == "tan" || this.lookahead.Content == "asin" || this.lookahead.Content == "acos" || this.lookahead.Content == "atan" )
                {
                    string funcion = this.lookahead.Content;
                    eat("SYMBOL");
                    eat("LEFT");

                    dynamic exp = this.Expression();
                    eat("RIGHT");

                    return new TrigNode(funcion,exp);

                }
                
                string name = this.lookahead.Content;
                eat("SYMBOL");

              

                if (this.lookahead != null && this.lookahead.Type == "LEFT")
                {
                      if(!scopeactual.Funciones.ContainsKey(name))
                    {
                        throw new Exception("missing Symbol");
                    }

                    Dictionary<int,dynamic> dic = new Dictionary<int, dynamic>{};
                    int count = 0;
                    
                    eat("LEFT");
                    while (true)
                    {
                        dynamic var = this.Expression().Evaluate();
                        dic.Add(count,var);

                        if (this.lookahead.Type == "RIGHT")
                        {
                            break;
                        }
                        eat("DOT");
                        count++;
                    }
                    eat("RIGHT");
                    

                    Scope scopefuncion = scopeactual;
                    scopefuncion.Variables = new Dictionary<string, dynamic>{};
                    
                    for (int i = 0; i < dic.Count; i++)
                    {
                        scopefuncion.Variables.Add(scopefuncion.Funciones[name].var[i].name,dic[i]);
                    }
                    

                    Program.Proyecto proyecto = new Program.Proyecto(scopeactual.Funciones[name].code,scopefuncion);
                    
                    ExpressionNode a = proyecto.p.MAthExpression;
                    return a;
                    
                }else
                {
                    if (!this.scopeactual.Variables.ContainsKey(name))
                    {
                        throw new Exception("missing Symbol");
                        return new ErrorNode("missing Symbol");
                    }
                    return new Variable(name,scopeactual);
                    
                }
            }            

            if (this.lookahead.Content == "let")
            {
                eat("KEYWORD");
                
                while(true)
                {
                    string name = this.lookahead.Content;
                    eat("SYMBOL");
                    eat("EQUAL");
                    dynamic value = this.Expression().Evaluate();

                    if (scopeactual.Variables.ContainsKey(name))
                    {
                        scopeactual.Variables[name] = value;
                    }
                    else
                    {
                        scopeactual.Variables.Add(name,value);
                    }

                    if (this.lookahead.Content == ",")
                    {
                        eat(this.lookahead.Type);
                        continue;
                    }

                    break;
                }

                eat("KEYWORD");

                return this.Expression();
            }

            if (this.lookahead.Content == "function")
            {
                eat("KEYWORD");
                string name = this.lookahead.Content;
                eat("SYMBOL");

                eat("LEFT");

                int count = 0;
                Dictionary<int,Variable> vars = new Dictionary<int, Variable>{}; 
                
                while(true)
                {
                    vars.Add(count,new Variable(this.lookahead.Content,scopeactual));
                    count++;
                    eat("SYMBOL");
                    if (this.lookahead.Type == "DOT")
                    {
                        eat("DOT");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                eat("RIGHT");
                
                eat("EQUAL");
                eat("GREATER");

                string Code = this.lookahead.Content;
                eat(this.lookahead.Type);
                while(this.lookahead != null)
                {
                    Code += this.lookahead.Content;
                    eat(this.lookahead.Type);
                }

                if (scopeactual.Funciones.ContainsKey(name))
                {
                    scopeactual.Funciones[name] = new Function(name,vars,Code);    
                }
                else
                {
                    scopeactual.Funciones.Add(name,new Function(name,vars,Code));
                }

                return new Function(name,vars,Code);

            }

            if(this.lookahead.Content == "if")
            {
                eat("KEYWORD");
                eat("LEFT");
                
                ExpressionNode Condition = this.Expression();
                
                eat("RIGHT");
                
                ExpressionNode Action = this.Expression();

                if (this.lookahead.Content == "else")
                {
                    eat("KEYWORD");
                }
                else
                {
                    throw new Exception("else expected");
                }
                

                ExpressionNode ElseAction = this.Expression();

                return new IfElseExpression(Condition,Action,ElseAction);
            }
            
            if (this.lookahead.Type == "LEFT")
            {
                return this.ParenthesizedExpression();
            }

            if (this.lookahead.Type == "SUB") 
            {
                return this.UnaryExpression();
            }

            if(this.lookahead.Type == "QUOTED-STRING")
            {
                ExpressionNode a = new LiteralNode(this.lookahead.Content);
                eat("QUOTED-STRING");

                return a;
            }
            Token token = eat("INT");
            
            return new Integer(token.Content);
        }
        public ExpressionNode Term()
        {
            ExpressionNode left = this.Factor();

            while (this.lookahead != null && (this.lookahead.Type == "MUL" || this.lookahead.Type == "DIV" || this.lookahead.Type == "REST"))
            {
                if (this.lookahead.Type == "MUL")
                {
                    this.eat("MUL");
                    ExpressionNode right = this.Factor();
                    left = new MulNode(left, right);
                }
                else if(this.lookahead.Type == "DIV")
                {
                    this.eat("DIV");
                    ExpressionNode right = this.Factor();
                    left = new DivNode(left, right);
                }else
                {
                    this.eat("REST");
                    ExpressionNode right = this.Factor();
                    left = new RestNode(left,right);
                }
            }
            return left;

        }

        public ExpressionNode Factor()
        {
            ExpressionNode left = this.Primary();

            if (this.lookahead != null)
            {
                while (this.lookahead != null && this.lookahead.Type == "POW")
                {
                    this.eat("POW");
                    ExpressionNode right = this.Factor();
                    left = new PowNode(left, right);
                }
            }

            return left;
        }
    
        public ExpressionNode ParenthesizedExpression()
        {
            this.eat("LEFT");
            ExpressionNode exp = this.ArigmeticalExpression();
            this.eat("RIGHT"); 

            return exp;
        }
        public ExpressionNode UnaryExpression()
        {
            this.eat("SUB");
            return new NegNode(this.Factor());
        }
    
        public ExpressionNode ConditionalExpression()
        {
            ExpressionNode left = this.Expression();

             while (this.lookahead != null && (this.lookahead.Type == "OR" || this.lookahead.Type == "AND"))
            {

                if (this.lookahead.Type == "AND")
                {
                    this.eat("ADD");
                    ExpressionNode right = this.Expression();
                    left = new AndNode(left, right);

                }
                else
                {
                    this.eat("OR");
                    ExpressionNode right = this.Expression();
                    left = new OrNode(left, right);
                }
            }

            return left;
        } 
    
    }
}
