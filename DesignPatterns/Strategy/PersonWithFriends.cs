using System.Text;
using Person = DesignPatterns.Strategy.Strategy.Person;

namespace DesignPatterns.Strategy;

public class PersonWithFriends(Person person, List<Person> friends)
{
    public static void Main()
    {
        var personWithFriends = new PersonWithFriends(
            new Person { Id = 1, Name = "John", Age = 30 },
            new List<Person>
            {
                new Person { Id = 2, Name = "Jane", Age = 25 },
                new Person { Id = 3, Name = "Doe", Age = 35 }
            }
        );
        
        var html = personWithFriends.PrintStrategy(Person.PrintAsHtml);
        Console.WriteLine(html);
    }
    
    public string PrintStrategy(Func<Person, string> formatter)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var friend in friends)
        {
            sb.AppendLine("\t " +formatter(friend));
        }
            
        //Not so good, because it's not flexible,
        //maybe a more sophisticated strategy pattern would be better with begin and end tags end child methods
        return formatter(person) + Environment.NewLine + sb.ToString();
    }
}