
namespace Stadium5Help.Models
{
	public class Item
	{
		private string displayName;

		public string Name { get; set; }
		
		public string DisplayName
		{
			get	{ return this.displayName ?? this.Name;	}
			set { this.displayName = value; }
		}
		
		public string Description { get; set; }
	}
}