namespace DesignPatterns.Decorator;

//Idea based on https://medium.com/@softwaretechsolution/decorator-design-pattern-cafdf7c3f0b2
public class TextEditor
{
    public static void Main()
    {
        var text = 
            new H1TextDecorator(new ItalicTextDecorator(new BoldTextDecorator(
                new Plaintext("Hello World"))));
        
        Console.WriteLine(text.GetToString());
        
        //if we want to remove the bold text from the text from above we have to modify the 
        //decorator "chain", this is the downside of the Decorator pattern
        
        //if we want to add styling in the text this is also not easy, because the first idea is to decorate
        //the text with a new decorator (StyleTextDecorator), but this will add the new style around the <h1> tag
        //instead of a attribute in the <h1> tag (<h1 style="color:red">Hello World</h1>)
        
        //an idea that I have is the following: if you look from above a decorator is looking similar to a linked list
        //the difference is that a decorator is behavior oriented and a linked list is data oriented.
        //Maybe we can implement the decorator chain as a linked list, and that the decorator chain is a list from the perspective
        //of the decorator, this way we can add and remove decorators easily and we can add new styles to the text
        //without modifying the existing decorators
        //we can also maybe search for a specific decorator in the chain and modify it
        //This is an idea that I have, I have to think about it more and implement it to see if it works.
    }
    
    
    public interface TextToString
    {
        // in C# you have the method ToString() which is always inherited from the Object class,
        // so we use GetToString() to avoid confusion with the ToString() method
        // this make the code more readable and mimics the structure of the Decorator pattern
        public string GetToString();
    }
    
    public class Plaintext : TextToString
    {
        private string _text;
        
        public Plaintext(string text)
        {
            _text = text;
        }
        
        public string GetToString()
        {
            return _text;
        }
    }
    
    public abstract class TextDecorator : TextToString
    {
        protected TextToString _textToString;
        
        public TextDecorator(TextToString textToString)
        {
            _textToString = textToString;
        }
        
        public abstract string GetToString();
    }
    
    public class BoldTextDecorator : TextDecorator
    {
        public BoldTextDecorator(TextToString textToString) : base(textToString)
        {
        }
        
        public override string GetToString()
        {
            return $"<b>{_textToString.GetToString()}</b>";
        }
    }
    
    public class ItalicTextDecorator : TextDecorator
    {
        public ItalicTextDecorator(TextToString textToString) : base(textToString)
        {
        }
        
        public override string GetToString()
        {
            return $"<i>{_textToString.GetToString()}</i>";
        }
    }
    
    public class H1TextDecorator : TextDecorator
    {
        public H1TextDecorator(TextToString textToString) : base(textToString)
        {
        }
        
        public override string GetToString()
        {
            return $"<h1>{_textToString.GetToString()}</h1>";
        }
    }
}