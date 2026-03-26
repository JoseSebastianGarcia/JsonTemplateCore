using System.Text.Json;

namespace JsonParsing;

public class JsonTemplate
{
	private readonly JsonDocument _document;

	public JsonElement Json => _document.RootElement;

	private JsonTemplate(string json)
	{
		_document = JsonDocument.Parse(json);
	}
	private JsonTemplate(JsonDocument document)
	{
		_document = document;
	}

	public string ResolvePath(string path)
	{
		var parts = path.Split('.');
		JsonElement current = Json;

		foreach (var part in parts)
		{
			string propertyName = part;
			int? index = null;

			// Detect array: Prop[0]
			if (part.Contains('['))
			{
				int start = part.IndexOf('[');
				int end = part.IndexOf(']');

				propertyName = part.Substring(0, start);
				var indexStr = part.Substring(start + 1, end - start - 1);

				if (!int.TryParse(indexStr, out var parsedIndex))
					return string.Empty;

				index = parsedIndex;
			}

			if (current.ValueKind == JsonValueKind.Object &&
				current.TryGetProperty(propertyName, out var next))
			{
				current = next;
			}
			else
			{
				return string.Empty;
			}

			if (index.HasValue)
			{
				if (current.ValueKind == JsonValueKind.Array &&
					current.GetArrayLength() > index.Value)
				{
					current = current[index.Value];
				}
				else
				{
					return string.Empty;
				}
			}
		}

		return ConvertToString(current);
	}

	private static string ConvertToString(JsonElement element)
	{
		return element.ValueKind switch
		{
			JsonValueKind.String => element.GetString() ?? "",
			JsonValueKind.Number => element.GetRawText(),
			JsonValueKind.True => "true",
			JsonValueKind.False => "false",
			JsonValueKind.Null => "",

			JsonValueKind.Object => element.GetRawText(),

			JsonValueKind.Array => string.Join(",",
				element.EnumerateArray().Select(ConvertToString)),

			_ => element.ToString()
		};
	}

	public string Render(string template, TemplateEngine engine)
	{
		return engine.Regex.Replace(template, match =>
		{
			var path = match.Groups[1].Value;
			var value = ResolvePath(path);

			return string.IsNullOrEmpty(value) ? match.Value : value;
		});
	}

	#region Operators
	public static implicit operator JsonTemplate(JsonDocument element)
		=> new JsonTemplate(element);

	public static implicit operator JsonTemplate(string json)
		=> new JsonTemplate(json);

	public static implicit operator string(JsonTemplate template)
		=> template.Json.GetRawText();
	#endregion
}