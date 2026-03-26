using System.Text.RegularExpressions;

namespace JsonParsing;

public class TemplateEngine
{
	public Regex Regex { get; }

	private TemplateEngine(string pattern, RegexOptions options = RegexOptions.Compiled)
	{
		Regex = new Regex(pattern, options);
	}

	/// <summary>
	/// Solo estilo $(...)
	/// </summary>
	public static TemplateEngine Dollar { get; } =
		new TemplateEngine(
			@"\$\(([a-zA-Z]+(?:\[[0-9]+\])?(?:\.[a-zA-Z]+(?:\[[0-9]+\])?)*)\)"
		);

	/// <summary>
	/// Solo estilo #(...)
	/// </summary>
	public static TemplateEngine Hash { get; } =
		new TemplateEngine(
			@"\#\(([a-zA-Z]+(?:\[[0-9]+\])?(?:\.[a-zA-Z]+(?:\[[0-9]+\])?)*)\)"
		);

	/// <summary>
	/// Solo estilo #...
	/// </summary>
	public static TemplateEngine HashStart { get; } =
		new TemplateEngine(
			@"\#([a-zA-Z]+(?:\[[0-9]+\])?(?:\.[a-zA-Z]+(?:\[[0-9]+\])?)*)"
		);

	/// <summary>
	/// Solo estilo $...
	/// </summary>
	public static TemplateEngine DollarStart { get; } =
		new TemplateEngine(
			@"\$([a-zA-Z]+(?:\[[0-9]+\])?(?:\.[a-zA-Z]+(?:\[[0-9]+\])?)*)"
		);

	/// <summary>
	/// Solo estilo $...$
	/// </summary>
	public static TemplateEngine DollarStartEnd { get; } =
		new TemplateEngine(
			@"\$([a-zA-Z]+(?:\[[0-9]+\])?(?:\.[a-zA-Z]+(?:\[[0-9]+\])?)*)\$"
		);
}
