using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.Common
{
	public static class TypeHelpers
	{
		public static object GetDefaultValue(this Type type)
		{
			return typeof(TypeHelpers).GetMethod("GetDefaultValue", Type.EmptyTypes).MakeGenericMethod(type).Invoke(null, null);
		}

		public static T GetDefaultValue<T>()
		{
			return typeof(T) == typeof(string) ? (T)(object)string.Empty : default(T);
		}

		public static T ConvertDbValue<T>(dynamic value)
		{
			if ((value == null) || (value is DBNull))
				return GetDefaultValue<T>();

			if ((typeof(T).IsGenericType) && (typeof(T).GetGenericTypeDefinition() == typeof(List<>)))
				return (T)typeof(T).GetConstructor(new Type[] { typeof(IEnumerable<>).MakeGenericType(typeof(T).GetGenericArguments()[0]) }).Invoke(new object[] { value });

			return value is T ? (T)value : Convert.ChangeType(value, typeof(T));
		}

		public static bool IsList(this Type type)
		{
			return (type.IsArray) || ((type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(List<>)));
		}

		public static TypeReference MapType(this Type systemType)
		{
			Type elementType;
			if (systemType.IsArray)
				elementType = systemType.GetElementType();
			else if ((systemType.IsGenericType) && (systemType.GetGenericTypeDefinition() == typeof(List<>)))
				elementType = systemType.GetGenericArguments()[0];
			else if (systemType == typeof(Int16) || systemType == typeof(Int32))
				elementType = typeof(Int64);
			else
				elementType = systemType;

			if ((!elementType.IsPrimitive) && (elementType != typeof(decimal)) && (elementType != typeof(string)) && (elementType != typeof(DateTime)))
				elementType = typeof(string);

			return systemType.IsList() ? (TypeReference)TypeReference.CreateList(TypeReference.Create(elementType)) : TypeReference.Create(elementType);
		}

		public static string GetCodeStringForType(this Type type)
		{
			if (type.IsGenericType)
			{
				string genericTypeName = type.GetGenericTypeDefinition().FullName;
				return string.Format("{0}<{1}>", genericTypeName.Substring(0, genericTypeName.IndexOf('`')), string.Join(", ", (object[])type.GetGenericArguments()));
			}
			return type.ToString();
		}

		public static string GetCodeStringForType(ITypeReference typeReference)
		{
			return typeReference.IsList ? "System.Collections.Generic.List<" + typeReference.GetEnumerableContentType().GetUnderlyingType() + ">" : typeReference.GetUnderlyingType().ToString();
		}

		public static object ToArray(object enumerableValue)
		{
			Type genericType = enumerableValue.GetType().GetInterfaces().First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
			return typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(genericType).Invoke(null, new object[] { enumerableValue });
		}

		public static T[] GetEnumerationValueAttributes<T>(Enum enumValue) where T : Attribute
		{
			return (T[])enumValue.GetType().GetMember(enumValue.ToString())[0].GetCustomAttributes(typeof(T), false);
		}
	}


	public class EnumWrapper<T> where T : struct, IComparable, IFormattable, IConvertible
	{
		public EnumWrapper(T enumValue)
		{
			EnumValue = enumValue;
		}

		public T EnumValue { get; private set; }

		public override string ToString()
		{
			DescriptionAttribute[] descriptionAttributes = TypeHelpers.GetEnumerationValueAttributes<DescriptionAttribute>((Enum)(object)EnumValue);
			return descriptionAttributes.Length == 0 ? EnumValue.ToString() : descriptionAttributes[0].Description;
		}

		public override bool Equals(object obj)
		{
			return (obj is EnumWrapper<T>) && (Enum.Equals(EnumValue, ((EnumWrapper<T>)obj).EnumValue));
		}

		public override int GetHashCode()
		{
			return EnumValue.GetHashCode();
		}


		public static implicit operator EnumWrapper<T>(T enumValue)
		{
			return new EnumWrapper<T>(enumValue);
		}

		public static implicit operator T(EnumWrapper<T> wrapper)
		{
			return wrapper.EnumValue;
		}
	}
}
