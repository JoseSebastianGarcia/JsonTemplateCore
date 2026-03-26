using JsonParsing;
using System.Text.Json;

JsonTemplate json = @"
	{
		""Name"": ""John"",
		""Age"": 30,
		""Address"": {
			""Street"": ""123 Main St"",
			""City"": ""Anytown""
		},
		""Users"": [
			{ ""Name"": ""Alice"", ""Age"": 25 },
			{ ""Name"": ""Bob"", ""Age"": 28 }
		]
	}
";

string template = "Name: $(Name), Age: $(Age), Street: $(Address.Street), First User Name: $(Users[0].Name)";
string message =json.Render(template, TemplateEngine.Dollar);

Console.WriteLine(message);
Console.ReadKey();
