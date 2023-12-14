using Project;

namespace Project
{


    public class IfElseExpression : ExpressionNode
    {
        public dynamic Con;
        public string act;
        public string elseact;
        Scope scope;
        public IfElseExpression(ExpressionNode Condicion, string Action, string ElseAction, Scope scope)
        {
            this.Con = Condicion;
            this.act = Action;
            this.elseact = ElseAction;
            this.scope = scope;
        }

        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {
            if (this.Con.Evaluate())
            {
                Program.Proyecto proyecto = new Program.Proyecto(act, scope);
                return proyecto.result;
            }
            else
            {
                Program.Proyecto proyecto = new Program.Proyecto(elseact, scope);
                return proyecto.result;
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
        public Variable(string sValue, Scope scope)
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
        public Dictionary<int, Variable> var;
        public string code;
        public string name;
        public Function(string name, Dictionary<int, Variable> variablearemplazar, string Codigobase)
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
        public Dictionary<string, dynamic> Variables;

        public Dictionary<string, Function> Funciones;

        public Scope()
        {
            this.Variables = new Dictionary<string, dynamic>();
            this.Funciones = new Dictionary<string, Function>();
        }

        public static Scope Copy(Scope scope)
        {
            Scope return_scope = new Scope();

            foreach (var item in scope.Variables.Keys)
            {
                return_scope.Variables.Add(item, scope.Variables[item]);
            }
            foreach (var item in scope.Funciones.Keys)
            {
                return_scope.Funciones.Add(item, scope.Funciones[item]);
            }
            
            return return_scope;

        }
    }


    public class Printnode : ExpressionNode
    {
        public dynamic Exp;
        public Printnode(dynamic Expresion)
        {
            this.Exp = Expresion;
        }
        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {
            Console.WriteLine($"{this.Exp}");
            return new nullnode();
        }

        
        public override string GetNodeType()
        {
            throw new NotImplementedException();
        }
    }
    public class node_function : ExpressionNode
    {
        public Scope scope { get; private set; }
        Function function;
        public node_function(Scope scope, Function function)
        {
            this.scope = scope;
            this.function = function;
        }
        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {

            Program.Proyecto proyecto = new Program.Proyecto(function.code, scope);
            return proyecto.result;
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
            return "null";
        }
    }

    public class Log_node : ExpressionNode
    {
        ExpressionNode bas;
        ExpressionNode exponent;
        public Log_node(ExpressionNode bas, ExpressionNode exponent)
        {
            this.bas = bas;
            this.exponent = exponent;
        }
        public override bool CheckSemantics()
        {
            throw new NotImplementedException();
        }

        public override dynamic Evaluate()
        {
            return Math.Log(exponent.Evaluate(), bas.Evaluate());
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
}