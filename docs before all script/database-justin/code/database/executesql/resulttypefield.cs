using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class ResultTypeField : ISerializable
	{
		public ResultTypeField() { }

		private ResultTypeField(SerializationInfo info, StreamingContext context)
		{
			ColumnName = info.GetString("ColumnName");
			Name = info.GetString("Name");
			Type = (Type)info.GetValue("Type", typeof(Type));
		}

		public string ColumnName { get; set; }
		public string Name { get; set; }
		public Type Type { get; set; }

		public ITypeReference TypeReference
		{
			get
			{
				return TypeHelpers.MapType(Type);
			}
			set
			{
				Type = value is ListTypeReference ? typeof(List<>).MakeGenericType(value.GetEnumerableContentType().GetUnderlyingType()) : value.GetUnderlyingType();
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ColumnName", ColumnName);
			info.AddValue("Name", Name);
			info.AddValue("Type", Type);
		}
	}

	public class ResultTypeFields : ObservableCollection<ResultTypeField>
	{
		private bool blockChangeEvents;

		public void AddRange(IEnumerable<ResultTypeField> newFields)
		{
			blockChangeEvents = true;
			foreach (var nextField in newFields)
				Add(nextField);
			blockChangeEvents = false;

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newFields));
		}

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (blockChangeEvents)
				return;
			base.OnCollectionChanged(e);
		}
	}

	public class ResultType
	{
		public ResultType()
		{
			Fields = new ResultTypeFields();
		}

		public ITypeReference CustomType { get; set; }
		public ResultTypeFields Fields { get; private set; }

		public ITypeReference BuildRowTypeFromFields()
		{
			if (Fields.Count == 0)
				return null;

			if (CustomType != null)
				return CustomType;

			TypeBuilder typeBuilder = new TypeBuilder();
			foreach (ResultTypeField nextField in Fields)
				typeBuilder.AddProperty(nextField.Name, nextField.TypeReference);
			return typeBuilder.CreateTypeReference();
		}
	}
}
