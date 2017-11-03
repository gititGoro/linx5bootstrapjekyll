using System;
using System.Activities.Presentation.PropertyEditing;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.UI.Converters;

namespace Twenty57.Linx.Components.Database.UI.Controls
{
	public class StringEditor
	{
		private static readonly Type stringEditorType;
		private static readonly FieldInfo valuePropertyFieldInfo;
		private static readonly PropertyInfo valuePropertyInfo;

		private TextBox editor;
		private Binding valueBinding;

		static StringEditor()
		{
			stringEditorType = typeof(PropertyValueEditor).Assembly.GetType("System.Activities.Presentation.Internal.PropertyEditing.FromExpression.Framework.ValueEditors.StringEditor", true, false);
			valuePropertyFieldInfo = stringEditorType.GetField("ValueProperty", BindingFlags.Static | BindingFlags.Public);
			valuePropertyInfo = stringEditorType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
		}

		public StringEditor()
		{
			valueBinding = null;
			ConfigureStringEditor();

			editor.DataContextChanged += editor_DataContextChanged;
		}

		public StringEditor(string bindingPath)
		{
			ConfigureStringEditor();

			valueBinding = new Binding(bindingPath)
			{
				Mode = BindingMode.TwoWay
			};

			var valueProperty = (DependencyProperty)valuePropertyFieldInfo.GetValue(editor);
			editor.SetBinding(valueProperty, valueBinding);
		}

		public TextBox Editor { get { return this.editor; } }

		public string Value
		{
			get { return valuePropertyInfo.GetValue(this.editor) as string; }
			set { valuePropertyInfo.SetValue(this.editor, value); }
		}

		private void ConfigureStringEditor()
		{
			editor = (TextBox)Activator.CreateInstance(stringEditorType);

			var isEnabledBinding = new Binding("ParentProperty.IsReadOnly")
			{
				Mode = BindingMode.OneWay,
				Converter = new InverseBooleanConverter()
			};
			editor.SetBinding(UIElement.IsEnabledProperty, isEnabledBinding);
			editor.MaxLines = 3;
			editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			editor.KeyDown += EditorKeyDown;

			CommandManager.AddPreviewExecutedHandler(editor, HandlePasteEvent);
		}

		private void HandlePasteEvent(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Command == ApplicationCommands.Paste)
			{
				if (!Clipboard.ContainsText())
					return;

				var caret = editor.SelectionStart;
				editor.Text = editor.Text.Remove(caret, editor.SelectionLength);

				var text = Clipboard.GetData(DataFormats.Text) as string;

				editor.Text = editor.Text.Insert(caret, text);
				editor.CaretIndex = caret + text.Length;
				e.Handled = true;
			}
		}

		private void EditorKeyDown(object sender, KeyEventArgs e)
		{
			if (!(sender is TextBox))
				return;

			if (e.Key == Key.Enter)
			{
				if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
				{
					var caret = editor.SelectionStart;
					editor.Text = editor.Text.Remove(caret, editor.SelectionLength);

					editor.Text = editor.Text.Insert(caret, Environment.NewLine);
					editor.CaretIndex = caret + 1;
				}
				else
				{
					BindingExpression expression = (sender as TextBox).GetBindingExpression(TextBox.TextProperty);
					if (expression != null)
						expression.UpdateSource();
				}
			}
		}

		private void editor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((valueBinding == null) && (e.NewValue != null))
			{
				valueBinding = new Binding("Value") { Mode = BindingMode.TwoWay };
				var valueProperty = (DependencyProperty)valuePropertyFieldInfo.GetValue(editor);
				editor.SetBinding(valueProperty, valueBinding);
			}
		}
	}
}
