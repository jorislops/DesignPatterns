using System.Linq.Expressions;
using System.Text;

namespace DesignPatterns.ExpressionVisitor;

public class ClientExpression
{
    public static void Main()
    {
        var expression = new AdditionExpression(
            new IntExpression(1),
            new AdditionExpression(
                new IntExpression(2),
                new IntExpression(3)
            )
        );

        
        var visitor = new ExpressionPrinter();
        expression.Accept(visitor);
        Console.WriteLine(visitor.ToString());
        
        // var visitor = new ExpressionPrinter();
        // visitor.Visit(expression);
        // Console.WriteLine(visitor.ToString());
        
        // var visitor = new ExpressionPrinter();
        // expression.Accept(visitor);
        // Console.WriteLine(visitor.ToString());
        
        var evalVisitor = new EvalExpressionVisitor();
        Console.WriteLine(expression.Accept(evalVisitor));
        


        // Expression<Func<int,int>> addOneDivideByTwo = v => v + 1;
        // CountBinaryNodes countBinaryNodes = new ();
        // countBinaryNodes.Visit(addOneDivideByTwo);
    }
}

public class CountBinaryNodes : System.Linq.Expressions.ExpressionVisitor
{
    public int Count { get; private set; }
    
    
    protected override System.Linq.Expressions.Expression VisitBinary(BinaryExpression node)
    {
        var nodeType = node.NodeType;
        Console.WriteLine(nodeType);
        Count++;
        
        return base.VisitBinary(node);
    }
}

public interface IExpressionVisitor
{
    void Visit(IntExpression intExpression);
    void Visit(AdditionExpression ae);
}

public interface IEvalExpressionVisitor
{
    int Visit(IntExpression intExpression);
    int Visit(AdditionExpression ae);
}

public class EvalExpressionVisitor : IEvalExpressionVisitor
{
    public int Visit(IntExpression intExpression)
    {
        return intExpression.ValueInt;
    }

    public int Visit(AdditionExpression ae)
    {
        return ae.Left.Accept(this) + ae.Right.Accept(this);
    }
}

public class ExpressionPrinter : IExpressionVisitor
{
    private StringBuilder _sb = new StringBuilder();

    public void Visit(IntExpression intExpression)
    {
        _sb.Append(intExpression.ValueInt);
    }

    public void Visit(AdditionExpression ae)
    {
        _sb.Append("(");
        ae.Left.Accept(this);
        _sb.Append("+");
        ae.Right.Accept(this);
        _sb.Append(")");
    }

    public override string ToString()
    {
        return _sb.ToString();
    }
}

public abstract class Expression
{
    public abstract void Accept(IExpressionVisitor visitor);
    
    public abstract int Accept(IEvalExpressionVisitor visitor);
}

public class IntExpression : Expression
{
    public int ValueInt { get; set; }

    public IntExpression(int value)
    {
        ValueInt = value;
    }

    public override void Accept(IExpressionVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override int Accept(IEvalExpressionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class AdditionExpression : Expression
{
    public Expression Left { get; set; }
    public Expression Right { get; set; }

    public AdditionExpression(Expression left, Expression right)
    {
        Left = left;
        Right = right;
    }

    public override void Accept(IExpressionVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override int Accept(IEvalExpressionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

