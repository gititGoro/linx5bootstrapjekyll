// Original code from: http://stackoverflow.com/questions/12344367/making-avalonedit-mvvm-compatible

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Twenty57.Linx.Components.Database.UI.Controls
{
	public class MvvmTextEditor : TextEditor
	{
		private static readonly Dictionary<int, MvvmTextEditor> caretMappings;

		private const int defaultOffsetValue = -1;

		static MvvmTextEditor()
		{
			caretMappings = new Dictionary<int, MvvmTextEditor>();
		}

		public static int GetBoundCaretOffset(DependencyObject obj)
		{
			return (int)obj.GetValue(BoundCaretOffsetProperty);
		}

		public static void SetBoundCaretOffset(DependencyObject obj, int value)
		{
			obj.SetValue(BoundCaretOffsetProperty, value);
		}

		public static string GetBoundText(DependencyObject obj)
		{
			return (string)obj.GetValue(BoundTextProperty);
		}

		public static void SetBoundText(DependencyObject obj, string value)
		{
			obj.SetValue(BoundTextProperty, value);
		}

		public static DependencyProperty BoundCaretOffsetProperty =
			DependencyProperty.Register(
				"BoundCaretOffset",
				typeof(int),
				typeof(MvvmTextEditor),
				new FrameworkPropertyMetadata(defaultOffsetValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BoundCaretOffsetChanged));

		public static DependencyProperty BoundTextProperty =
			DependencyProperty.Register(
				"BoundText",
				typeof(string),
				typeof(MvvmTextEditor),
				new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BoundTextChanged));

		private static void BoundCaretOffsetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			MvvmTextEditor editor = obj as MvvmTextEditor;
			if (editor != null)
			{
				if ((int)e.OldValue == defaultOffsetValue && (int)e.NewValue != defaultOffsetValue)
					editor.TextArea.Caret.PositionChanged += Caret_PositionChanged;

				if (!caretMappings.ContainsKey(editor.TextArea.Caret.GetHashCode()))
					caretMappings.Add(editor.TextArea.Caret.GetHashCode(), editor);

				int newValue = ((int)e.NewValue == defaultOffsetValue) ? default(int) : (int)e.NewValue;
				if (newValue != editor.CaretOffset)
					editor.CaretOffset = newValue;
			}
		}

		private static void Caret_PositionChanged(object sender, EventArgs e)
		{
			Caret caret = sender as Caret;
			if (caret != null)
			{
				MvvmTextEditor editor;
				if (caretMappings.TryGetValue(caret.GetHashCode(), out editor))
					SetBoundCaretOffset(editor, editor.CaretOffset);
			}
		}

		private static void BoundTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			MvvmTextEditor editor = obj as MvvmTextEditor;
			if (editor != null)
			{
				if (e.OldValue == null && e.NewValue != null)
					editor.TextChanged += Editor_TextChanged;

				string newValue = (e.NewValue == null) ? String.Empty : (string)e.NewValue;
				if (newValue != editor.Text)
					editor.Text = newValue;
			}
		}

		private static void Editor_TextChanged(object sender, EventArgs e)
		{
			MvvmTextEditor editor = sender as MvvmTextEditor;
			if (editor != null)
				SetBoundText(editor, editor.Text);
		}
	}
}
