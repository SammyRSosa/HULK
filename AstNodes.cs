public abstract class ExpressionNode
{

    public abstract dynamic Evaluate();
    public abstract bool CheckSemantics();
    public abstract string GetNodeType();
}

public abstract class BinaryNode : ExpressionNode
{
    public ExpressionNode Left => this.left;

    public ExpressionNode Right => this.right;

    private ExpressionNode left;
    private ExpressionNode right;

    public BinaryNode(ExpressionNode left, ExpressionNode right)
    {
        this.left = left;
        this.right = right;
    }


}
public abstract class BinaryArithmeticNode : BinaryNode
{
    protected BinaryArithmeticNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {
    }

    public override bool CheckSemantics()
    {
        return this.Left.GetNodeType() == "int" && this.Right.GetNodeType() == "int";
    }
    public override string GetNodeType()
    {
        return "int";
    }
}

public abstract class UnaryNode : ExpressionNode
{

    public ExpressionNode Exp => this.exp;

    private ExpressionNode exp;
    public UnaryNode(ExpressionNode exp)
    {
        this.exp = exp;
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
public class AddNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    { 
        return this.Left.Evaluate() + this.Right.Evaluate();
    }

    public AddNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

}
public class SubNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() - this.Right.Evaluate();
    }

    public SubNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

}

public class MulNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() * this.Right.Evaluate();
    }

    public MulNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }
}

public class DivNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() / this.Right.Evaluate();
    }

    public DivNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }
}

public class RestNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() % this.Right.Evaluate();
    }

    public RestNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }
}

public class PowNode : BinaryArithmeticNode
{
    public override dynamic Evaluate()
    {
        return Math.Pow(Left.Evaluate(), Right.Evaluate());

    }

    public PowNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }
}

public class NegNode : UnaryNode
{
    public NegNode(ExpressionNode exp) : base(exp)
    {
    }

    public override dynamic Evaluate()
    {
        return -this.Exp.Evaluate();
    }
}
public class Integer : ExpressionNode
{
    public int value;
    public Integer(string sValue)
    {
        this.value = int.Parse(sValue);
    }
    public override dynamic Evaluate()
    {
        return this.value;
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

public abstract class NumeriComparison : BinaryNode
{
    protected NumeriComparison(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }
    public override bool CheckSemantics()
    {
        return this.Left.GetNodeType() == "int" && this.Right.GetNodeType() == "int";
    }
    public override string GetNodeType()
    {
        return "bool";
    }
}

public class LessNode : NumeriComparison
{
    public LessNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() < this.Right.Evaluate();
    }
}

public class LessEqualNode : NumeriComparison
{
    public LessEqualNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() <= this.Right.Evaluate();
    }
}

public class GreaterEqualNode : NumeriComparison
{
    public GreaterEqualNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() >= this.Right.Evaluate();
    }
}

public class GreaterNode : NumeriComparison
{
    public GreaterNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {

    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() > this.Right.Evaluate();
    }
}

public class EqualNode : BinaryNode
{
    public EqualNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {
    }

    public override bool CheckSemantics()
    {
        throw new NotImplementedException();
    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() == this.Right.Evaluate();
    }

    public override string GetNodeType()
    {
        throw new NotImplementedException();
    }
}

public abstract class ConditionalExpression : BinaryNode
{
    public ConditionalExpression(ExpressionNode left, ExpressionNode right) : base(left, right)
    {
    }

    public override bool CheckSemantics()
    {
        return this.Left.GetNodeType() == "bool" && this.Right.GetNodeType() == "bool";
    }


    public override string GetNodeType()
    {
        return "bool";
    }
}


public class AndNode : ConditionalExpression
{
    public AndNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {
    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() && this.Right.Evaluate();
    }
}

public class OrNode : ConditionalExpression
{
    public OrNode(ExpressionNode left, ExpressionNode right) : base(left, right)
    {
    }

    public override dynamic Evaluate()
    {
        return this.Left.Evaluate() || this.Right.Evaluate();
    }
}

public class LiteralNode : ExpressionNode
{
    public string value;
    public LiteralNode(string sValue)
    {
        this.value = sValue.Substring(1, sValue.Length - 2);
    }
    public override dynamic Evaluate()
    {
        return this.value;
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