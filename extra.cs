namespace Project
{
    

    public class IfElseExpression : ExpressionNode
    {
        public dynamic Con;
        public ExpressionNode act;
        public ExpressionNode elseact;
    public IfElseExpression(ExpressionNode Condicion,ExpressionNode Action,ExpressionNode ElseAction)
        {
            this.Con = Condicion;
            this.act = Action;
            this.elseact = ElseAction;

        }

        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {
            if (this.Con.Evaluate())
            {
                return act.Evaluate();
            }
            else
            {
                return elseact.Evaluate();
            }
        }

        public override string GetNodeType()
        {
            return "";
        }
    }

    public class Variable : ExpressionNode
    {
        public string name;
        public Scope Value;
        public Variable(string sValue,Scope scope)
        {
            this.name = sValue;
            this.Value = scope;
        }
        public override dynamic Evaluate()
        {
            return this.Value.Variables[this.name];
        }

        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override string GetNodeType()
        {
            throw new NotImplementedException();
        }
    }


    public class Function : ExpressionNode
    {
        public Dictionary<int,Variable> var;
        public string code;
        public string name;
        public Function(string name,Dictionary<int,Variable> variablearemplazar,string Codigobase)
        {
            this.name = name;
            this.var = variablearemplazar;
            this.code = Codigobase;
        }

        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {
            System.Console.WriteLine($"funcion {this.name} guardada");
            return new nullnode();
        }

        public override string GetNodeType()
        {
            throw new NotImplementedException();
        }
    }


    public class Scope
    {
        public Dictionary<string,dynamic> Variables;

        public Dictionary<string,Function> Funciones;
        
        public Scope()
        {
            this.Variables = new Dictionary<string, dynamic>();
            this.Funciones = new Dictionary<string, Function>();
        }
    }
}

public class Printnode : ExpressionNode
{
    public ExpressionNode Exp;
    public Printnode(ExpressionNode Expresion)
    {
        this.Exp = Expresion;
    }
    public override bool CheckSemantics()
    {
        throw new NotImplementedException();
    }

    public override dynamic Evaluate()
    {
        
        Console.WriteLine($"{this.Exp.Evaluate()}");
        return new nullnode();
    }

    public override string GetNodeType()
    {
        throw new NotImplementedException();
    }
}

public class TrigNode : ExpressionNode
{
    public string Type;
    ExpressionNode Input;
    public TrigNode(string Type, ExpressionNode Input)
    {
        this.Type = Type;
        this.Input = Input;

    }
    public override dynamic Evaluate()
    {
        switch (this.Type)
        {
            case "cos":
                return Math.Cos(Input.Evaluate());
            case "sin":
                return Math.Sin(Input.Evaluate());
            case "tan":
                return Math.Tan(Input.Evaluate());
            case "acos":
                return Math.Acos(Input.Evaluate());
            case "asin":
                return Math.Asin(Input.Evaluate());
            case "atan":
                return Math.Atan(Input.Evaluate());
            default:
                break;
        }
         throw new Exception("missed trig");
    }

    public override bool CheckSemantics()
    {
        throw new NotImplementedException();
    }

    public override string GetNodeType()
    {
        throw new NotImplementedException();
    }
}

public class nullnode : ExpressionNode
{
    public override bool CheckSemantics()
    {
        throw new NotImplementedException();
    }

    public override dynamic Evaluate()
    {
        throw new NotImplementedException();
    }

    public override string GetNodeType()
    {
        throw new NotImplementedException();
    }
}

public class ErrorNode : ExpressionNode
{
    string Message;
    public ErrorNode(string Message)
    {
        this.Message = Message;
    }

    public override bool CheckSemantics()
    {
        throw new NotImplementedException();
    }

    public override dynamic Evaluate()
    {
        System.Console.WriteLine($"{Message}");
        return true;
    }

    public override string GetNodeType()
    {
        throw new NotImplementedException();
    }
}