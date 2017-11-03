//http://stackoverflow.com/questions/17560085/problems-using-json-net-with-expandableobjectconverter
//Expandable object converter which allows for JSon Serialization

using System.ComponentModel;

namespace Twenty57.Linx.Components.File.Converters
{
	public class SerializableExpandableObjectConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
		{
			return destinationType != typeof(string) && base.CanConvertTo(context, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
		{
			return sourceType != typeof(string) && base.CanConvertFrom(context, sourceType);
		}
	}
}
