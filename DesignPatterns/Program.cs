// See https://aka.ms/new-console-template for more information

using DesignPatterns;
using DesignPatterns.Composite;

var shipment = SeedShipment.CreateDummyShipment();

var countProductsVisitor = new CountProductsVisitor();
shipment.Accept(countProductsVisitor);
Console.WriteLine(countProductsVisitor.Count);

var displayProductsVisitor = new DisplayProductsVisitor();
shipment.Accept(displayProductsVisitor);

// var secondShipment = SeedShipment.CreateShipmentWithoutPallets();
// secondShipment.Accept(displayProductsVisitor);


// var count = shipment.CountProducts();
// Console.WriteLine(count);
//
// shipment.DisplayProducts();



// Console.WriteLine(value);

// Console.WriteLine("Hello, World!");
//
// // LinkedListClient.Main();
//
// var expression = CreateExpression();
//
// // var intValues = expression.GetAllIntValues();
// var intsVisitor = new GetIntsVisitor();
// expression.Accept(intsVisitor);
//
// Console.WriteLine(intsVisitor.IntValues.Dump());
//
// var addVisitor = new CountAddsVisitor();
// expression.Accept(addVisitor);
//
// Console.WriteLine(addVisitor.Count);
//
//
// var evaluationVisitor = new EvaluateVisitor();
// expression.Accept(evaluationVisitor);
//
// Console.WriteLine(evaluationVisitor.Result);
//
//
//
// Expression CreateExpression()
// {
//     IntValue x = new IntValue(10);
//     IntValue y = new IntValue(2);
//     IntValue z = new IntValue(3);
//
// //plus = x + y = 10 + 2 = 12
//     AddExpression plus = new AddExpression(x, y);
// //plus = 12 * y = 12 * 3 = 36
//     MultiplyExpression multiplyExpression1 = new MultiplyExpression(plus, z);
//     return multiplyExpression1;
// }
//
// var persons = new List<Person>
// {
//     new Person { Name = "John", Age = 20, },
//     new Person { Name = "Thomas", Age = 30, },
// };
//
// var personsDump = ObjectDumper.Dump(persons);
//
// Console.WriteLine(personsDump);
// Console.ReadLine();
//
// public class Person
// {
//     public string Name { get; set; }
//     public int Age { get; set; }
// }


