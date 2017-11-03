using System;
using System.Windows;

namespace Twenty57.Linx.Components.Database.UI
{
	public class ShowMessageEventArgs : EventArgs
	{
		public ShowMessageEventArgs(string text, string caption, MessageBoxButton options = MessageBoxButton.OK, MessageBoxImage messageType = MessageBoxImage.None)
		{
			Text = text;
			Caption = caption;
			Options = options;
			MessageType = messageType;
		}

		public string Text { get; private set; }
		public string Caption { get; private set; }
		public MessageBoxButton Options { get; private set; }
		public MessageBoxImage MessageType { get; private set; }

		public MessageBoxResult MessageResponse { get; set; }
	}

	public delegate void ShowMessageEventHandler(object sender, ShowMessageEventArgs args);

	public interface IShowMessage
	{
		event ShowMessageEventHandler ShowMessage;
	}
}
