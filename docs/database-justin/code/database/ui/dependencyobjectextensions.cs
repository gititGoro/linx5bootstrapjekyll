using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Twenty57.Linx.Components.Database.UI
{
	public static class DependencyObjectExtensions
	{
		public static TDescendant GetFirstDescendantBreadthFirst<TDescendant>(this DependencyObject dependencyObject) where TDescendant : DependencyObject
		{
			if (dependencyObject == null) { throw new ArgumentNullException(); }
			return GetFirstDescendantBreadthFirst<TDescendant>(GetAllChildren(dependencyObject));
		}

		private static TDescendant GetFirstDescendantBreadthFirst<TDescendant>(IEnumerable<DependencyObject> descendants) where TDescendant : DependencyObject
		{
			if (!descendants.Any()) return null;
			var descendant = descendants.OfType<TDescendant>().FirstOrDefault();
			if (descendant != null) return descendant;
			return GetFirstDescendantBreadthFirst<TDescendant>(descendants.SelectMany(GetAllChildren));
		}

		private static IEnumerable<DependencyObject> GetAllChildren(DependencyObject dependencyObject)
		{
			return Enumerable
			   .Range(0, VisualTreeHelper.GetChildrenCount(dependencyObject))
			   .Select(i => VisualTreeHelper.GetChild(dependencyObject, i));
		}
	}
}
