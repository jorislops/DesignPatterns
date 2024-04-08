using System.Text;
using System.Linq;
using System.Text.Json;
using Bogus.DataSets;

namespace DesignPatterns.Strategy;

public class Strategy
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
 
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }

        private sealed class NameRelationalComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }
        }

        public static IComparer<Person> NameComparer { get; } = new NameRelationalComparer();

        private sealed class AgeRelationalComparer : IComparer<Person>
        {
            public int Compare(Person x, Person y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.Age.CompareTo(y.Age);
            }
        }

        public static IComparer<Person> AgeComparer { get; } = new AgeRelationalComparer();

        public string PrintStrategy(Func<Person, string> formatter)
        {
            return formatter(this);
        }
        
        public static Func<Person, string> PrintAsHtml = p => $"<div>{p.Name} ({p.Age})</div>";
        public static Func<Person, string> PrintAsMarkdown = p => $"* {p.Name} ({p.Age})";
    }
    
   
    
    public static void Main()
    {
        var people = new List<Person>
        {
            new Person {Id = 1, Name = "John", Age = 25},
            new Person {Id = 2, Name = "Alice", Age = 22},
            new Person {Id = 3, Name = "Bob", Age = 30},
        };
        
        Console.WriteLine("Sort by Name ========= Strategy Pattern");
        people.Sort(Person.NameComparer);
        foreach (var person in people)
        {
            Console.Write(person);
        }
        Console.WriteLine();
        
        Console.WriteLine("Sort by Age ========= Strategy Pattern");
        people.Sort(Person.AgeComparer);
        foreach (var person in people)
        {
            Console.Write(person);
        }
        Console.WriteLine();

        //better way than the above (LINQ instead of the not so flexible IComparer (Strategy Pattern))
        var orderByName = people.OrderBy(x => x.Name).ToList();
        //better way than the above (LINQ instead of the not so flexible IComparer (Strategy Pattern))
        var orderByAge = people.OrderBy(x => x.Age).ToList();
        
        Console.WriteLine("Strategy Pattern ========= By using Lambda expressions as strategies");
        
        foreach (var person in orderByName)
        {
            Console.WriteLine(person.PrintStrategy(Person.PrintAsHtml));
        }
        
        foreach (var person in orderByName)
        {
            Console.WriteLine(person.PrintStrategy(Person.PrintAsMarkdown));
        }
        
        foreach(var person in orderByAge)
        {
            Console.WriteLine(person.PrintStrategy(x => JsonSerializer.Serialize(x)));
        }
        
        Console.WriteLine("Dynamic Strategy Pattern =========");
        var fruits = new[] { "Apple", "Banana", "Orange" };
        var tp = new TextProcessor();
        tp.SetOutputFormat(OutputFormat.Markdown);
        tp.AppendList(fruits);
        Console.WriteLine(tp);
        tp.Clear();
        tp.SetOutputFormat(OutputFormat.Html);
        tp.AppendList(fruits);
        Console.WriteLine(tp);

        Console.WriteLine("Static Strategy Pattern =========, the strategy is set at compile time in the generic argument");
        TextProcessorStatic<HtmlStrategy> htmlTpStatic = new();
        htmlTpStatic.AppendList(fruits);
        Console.WriteLine(htmlTpStatic);
            
        TextProcessorStatic<MarkdownStrategy> markdownTpStatic = new();
        markdownTpStatic.AppendList(fruits);
        Console.WriteLine(markdownTpStatic);
    }

    
    public enum OutputFormat
    {
        Markdown,
        Html
    }
    
    public interface IListStrategy
    {
        public void Start(StringBuilder sb) { }
        public void End(StringBuilder sb) { }
        public void AddListItem(StringBuilder sb, string item) { }
    }
    
    public class HtmlStrategy : IListStrategy
    {
        public void Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }

        public void End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($"  <li>{item}</li>");
        }
    }   
    
    public class MarkdownStrategy : IListStrategy
    {
        public void AddListItem(StringBuilder sb, string item)
        {
            sb.AppendLine($" * {item}");
        }
    }

    public class TextProcessor
    {
        private StringBuilder _sb = new StringBuilder();
        private IListStrategy _listStrategy;

        public void SetOutputFormat(OutputFormat format)
        {
            _listStrategy = format switch
            {
                OutputFormat.Markdown => new MarkdownStrategy(),
                OutputFormat.Html => new HtmlStrategy(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void AppendList(IEnumerable<string> items)
        {
            _listStrategy.Start(_sb);
            foreach (var item in items)
            {
                _listStrategy.AddListItem(_sb, item);
            }
            _listStrategy.End(_sb);
        }

        public StringBuilder Clear()
        {
            return _sb.Clear();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
    
    public class TextProcessorStatic<LS> where LS : IListStrategy, new()
    {
        private StringBuilder _sb = new StringBuilder();
        private IListStrategy _listStrategy = new LS();
        
        public void AppendList(IEnumerable<string> items)
        {
            _listStrategy.Start(_sb);
            foreach (var item in items)
            {
                _listStrategy.AddListItem(_sb, item);
            }
            _listStrategy.End(_sb);
        }

        public StringBuilder Clear()
        {
            return _sb.Clear();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}