using System.Linq.Expressions;
using System.Text;

namespace DesignPatterns.ExpressionVisitor;

public class ClientExpression
{
    public static void Main()
    {
        var expression =
            new AdditionExpression(
                new IntExpression(4), 
                new MultiplyExpression(
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
        int result = expression.Accept(evalVisitor);
        Console.WriteLine(result);

        ExpressionEvaluator expressionEvaluator = new ();
        expression.Accept(expressionEvaluator);
        Console.WriteLine(expressionEvaluator.Value);
        
        

        // Expression<Func<int,int>> addOneDivideByTwo = v => v + 1;
        // CountBinaryNodes countBinaryNodes = new ();
        // countBinaryNodes.Visit(addOneDivideByTwo);
    }
}

public class CountBinaryNodes : System.Linq.Expressions.ExpressionVisitor
{
    public int Count { get; private set; }
    
    
    protected override System.Linq.Expressions.Expression 
        VisitBinary(BinaryExpression node)
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
    void Visit(MultiplyExpression multiplyExpression);
    void Visit(AdditionExpression additionExpression);
}

public interface IEvalExpressionVisitor
{
    int Visit(IntExpression intExpression);
    int Visit(MultiplyExpression multiplyExpression);
    int Visit(AdditionExpression additionExpression);
}

public class EvalExpressionVisitor : IEvalExpressionVisitor
{
    public int Visit(IntExpression intExpression)
    {
        return intExpression.ValueInt;
    }

    public int Visit(MultiplyExpression multiplyExpression)
    {
        return multiplyExpression.Left.Accept(this) * multiplyExpression.Right.Accept(this);
    }

    public int Visit(AdditionExpression additionExpression)
    {
        return additionExpression.Left.Accept(this) + additionExpression.Right.Accept(this);
    }
}

public class ExpressionEvaluator : IExpressionVisitor
{
    public int Value { get; private set; } = 0;
    
    public void Visit(IntExpression intExpression)
    {
        Value = intExpression.ValueInt;
    }

    public void Visit(MultiplyExpression multiplyExpression)
    {
        Value = multiplyExpression.Left.Accept(new EvalExpressionVisitor()) * 
                 multiplyExpression.Right.Accept(new EvalExpressionVisitor());
    }

    public void Visit(AdditionExpression additionExpression)
    {
        Value = additionExpression.Left.Accept(new EvalExpressionVisitor()) + 
                 additionExpression.Right.Accept(new EvalExpressionVisitor());
    }
}

public class ExpressionPrinter : IExpressionVisitor
{
    private StringBuilder _sb = new StringBuilder();

    public void Visit(IntExpression intExpression)
    {
        _sb.Append(intExpression.ValueInt);
    }

    public void Visit(MultiplyExpression multiplyExpression)
    {
        _sb.Append("(");
        multiplyExpression.Left.Accept(this);
        _sb.Append("*");
        multiplyExpression.Right.Accept(this);
        _sb.Append(")");
    }

    public void Visit(AdditionExpression additionExpression)
    {
        _sb.Append("(");
        additionExpression.Left.Accept(this);
        _sb.Append("+");
        additionExpression.Right.Accept(this);
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
    
    public abstract void AcceptClassic(IExpressionVisitor visitor);
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

    public override void AcceptClassic(IExpressionVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class MultiplyExpression : Expression
{
    public Expression Left { get; set; }
    public Expression Right { get; set; }

    public MultiplyExpression(Expression left, Expression right)
    {
        Left = left;
        Right = right;
    }

    public override void Accept(IExpressionVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public override void AcceptClassic(IExpressionVisitor visitor)
    {
        Left.AcceptClassic(visitor);
        visitor.Visit(this);
        Right.AcceptClassic(visitor);
    }

    public override int Accept(IEvalExpressionVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class AdditionExpression(Expression left, Expression right) : Expression
{
    public Expression Left { get; set; } = left;
    public Expression Right { get; set; } = right;

    public override void Accept(IExpressionVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override int Accept(IEvalExpressionVisitor visitor)
    {
        return visitor.Visit(this);
    }

    public override void AcceptClassic(IExpressionVisitor visitor)
    {
        throw new NotImplementedException();
    }
}

