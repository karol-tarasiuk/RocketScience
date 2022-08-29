namespace RocketScience.Library.Utilities;

[AttributeUsage(AttributeTargets.Field)]
public class DisplayNameAttribute : Attribute
{
	public DisplayNameAttribute(string name)
	{
		Name = name;
	}

	public string Name { get; }
}
