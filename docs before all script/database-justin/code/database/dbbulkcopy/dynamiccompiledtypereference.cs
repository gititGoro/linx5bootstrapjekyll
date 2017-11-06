using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	// Based on CompiledTypeReference.
	// We cannot subclass CompiledTypeReference because its constructors have 'internal'-access.
	[Serializable]
	public class DynamicCompiledTypeReference : TypeReference, ISerializable
	{
		private Type type;
		private List<ITypeProperty> properties = new List<ITypeProperty>();
		private bool isResource;

		public delegate ITypeReference TypeReferenceForDynamicPropertyDelegate(string dynamicPropertyName);

		internal DynamicCompiledTypeReference(string assemblyQualifiedName, TypeReferenceForDynamicPropertyDelegate typeBuilderDelegate, bool isResource = false)
			: this(Type.GetType(assemblyQualifiedName), typeBuilderDelegate, isResource)
		{ }

		internal DynamicCompiledTypeReference(Type type, TypeReferenceForDynamicPropertyDelegate typeBuilderDelegate, bool isResource = false)
		{
			if (type == null)
				throw new Exception("DynamicCompiledTypeReference constructor requires a type. The type provided is null.");

			this.type = type;
			Id = type.AssemblyQualifiedName;
			this.isResource = isResource;

			if (this.type != typeof(string) && !this.type.IsValueType)
			{
				properties = this.type.GetProperties(bindingAttr: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
					.Where(p => p.GetIndexParameters().Length == 0)
					.Select(p => CreateTypeProperty(p, typeBuilderDelegate)).ToList();
			}
		}

		protected DynamicCompiledTypeReference(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.type = Type.GetType(Id);
			properties = (List<ITypeProperty>)info.GetValue("properties", typeof(List<ITypeProperty>));
			isResource = info.GetBoolean("isResource");
		}

		public override string Name
		{
			get { return this.type.Name; }
		}

		public override bool IsEnumerable
		{
			get { return this.type != typeof(string) && this.type.GetInterface("IEnumerable") != null; }
		}

		public override bool IsList
		{
			get { return this.type.FullName.StartsWith("System.Collections.Generic.List`1") || this.type.GetInterface("IList") != null; }
		}

		public override bool IsCompiled
		{
			get { return true; }
		}

		public override bool IsResource
		{
			get { return isResource; }
		}

		public override Type GetUnderlyingType()
		{
			return this.type;
		}

		public override IEnumerable<ITypeProperty> GetProperties()
		{
			return properties;
		}

		public override ITypeProperty GetProperty(string name)
		{
			return properties.FirstOrDefault(p => p.Name == name);
		}

		public override ITypeReference GetEnumerableContentType()
		{
			if (IsEnumerable)
			{
				if (this.type.HasElementType)
				{
					return TypeReference.Create(this.type.GetElementType());
				}
				else if (this.type.IsGenericType && this.type.GetGenericArguments().Count() == 1)
				{
					return TypeReference.Create(this.type.GetGenericArguments()[0]);
				}

				return TypeReference.Create(typeof(object));
			}
			else
				return base.GetEnumerableContentType();
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("properties", this.properties);
			info.AddValue("isResource", isResource);
		}

		protected override bool AreEqual(ITypeReference reference)
		{
			if (Id != reference.Id)
				return false;

			var other = reference as DynamicCompiledTypeReference;
			if ((other == null) || (other.properties.Count != properties.Count))
				return false;

			for (int i = 0; i < properties.Count; i++)
				if (!properties[i].Equals(other.properties[i]))
					return false;

			return true;
		}

		private static ITypeProperty CreateTypeProperty(PropertyInfo property, TypeReferenceForDynamicPropertyDelegate typeBuilderDelegate)
		{
			bool hasGet = (property.GetMethod != null) && (property.GetMethod.IsPublic);
			bool hasSet = (property.SetMethod != null) && (property.SetMethod.IsPublic);
			AccessType access = (hasGet ? AccessType.Read : 0) | (hasSet ? AccessType.Write : 0);
			ITypeReference typeReference = property.PropertyType == typeof(object) ? typeBuilderDelegate(property.Name) : Create(property.PropertyType);
			return new TypeProperty(property.Name, typeReference, access);
		}
	}
}
