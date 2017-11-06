// Code from: http://stackoverflow.com/questions/7346663/how-to-show-a-waitcursor-when-the-wpf-application-is-busy-databinding
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Twenty57.Linx.UI.Common.Helpers
{
	public static class UIHelpers
	{
		public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
		{
			do
			{
				if (current is T)
					return (T)current;

				current = VisualTreeHelper.GetParent(current);
			}
			while (current != null);

			return null;
		}
	}
}
