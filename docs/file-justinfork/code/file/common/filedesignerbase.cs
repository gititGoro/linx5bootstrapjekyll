using System;
using System.Collections.Generic;
using System.ComponentModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File.Common
{
	public abstract class TextFileDesignerBase : FileDesignerBase<TextFileHandle>
	{
		public TextFileDesignerBase(IDesignerContext context) : base(context) { }

		public TextFileDesignerBase(IFunctionData data, IDesignerContext context) : base(data, context) { }
	}


	public abstract class BinaryFileDesignerBase : FileDesignerBase<BinaryFileHandle>
	{
		public BinaryFileDesignerBase(IDesignerContext context) : base(context) { }

		public BinaryFileDesignerBase(IFunctionData data, IDesignerContext context) : base(data, context) { }
	}


	public abstract class FileDesignerBase<FileHandleType> : FunctionDesigner
		where FileHandleType : FileHandle
	{
		public FileDesignerBase(IDesignerContext context)
			: base(context)
		{
			Properties.Add(new Property(FileShared.FilePathPropertyName, typeof(FileHandleType), ValueUseOption.RuntimeRead, null));
			Properties.Add(new Property(FileShared.OwnsFileHandlePropertyName, typeof(bool), ValueUseOption.DesignTime, true) { IsVisible = false });
			SetPropertyAttributes();
		}

		public FileDesignerBase(IFunctionData data, IDesignerContext context) : base(data, context) { }

		public bool OwnsFileHandle
		{
			get
			{
				object filePath = Properties[FileShared.FilePathPropertyName].Value;
				if (filePath is IExpression)
				{
					var filePathTypeReference = Context.GetTypeReference(filePath);
					bool isForeignFileHandle = (filePathTypeReference != null) && (filePathTypeReference.GetUnderlyingType() == typeof(FileHandleType));
					return !isForeignFileHandle;
				}
				return true;
			}
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		private void SetPropertyAttributes()
		{
			Property filePathProperty = Properties[FileShared.FilePathPropertyName];
			filePathProperty.Order = 0;
			filePathProperty.Description = "The full path of the file.";
			filePathProperty.Editor = typeof(FilePathEditor);
			filePathProperty.Validations.Add(new RequiredValidator());
			filePathProperty.ValueChanged += filePathProperty_ValueChanged;

			if (!Properties.Contains(FileShared.OwnsFileHandlePropertyName))
				Properties.Add(new Property(FileShared.OwnsFileHandlePropertyName, typeof(bool), ValueUseOption.DesignTime, true) { IsVisible = false });
			Properties[FileShared.OwnsFileHandlePropertyName].Value = OwnsFileHandle;
		}

		private void filePathProperty_ValueChanged(object sender, EventArgs e)
		{
			Property filePathProperty = Properties[FileShared.FilePathPropertyName];
			if (filePathProperty.Value is string)
				filePathProperty.Value = GetTypeConverter().ConvertFrom(filePathProperty.Value);

			Properties[FileShared.OwnsFileHandlePropertyName].Value = OwnsFileHandle;
		}

		private static TypeConverter GetTypeConverter()
		{
			string typeAssemblyQualifiedName = ((TypeConverterAttribute)typeof(FileHandleType).GetCustomAttributes(typeof(TypeConverterAttribute), false)[0]).ConverterTypeName;
			return (TypeConverter)Activator.CreateInstance(Type.GetType(typeAssemblyQualifiedName));
		}
	}
}
