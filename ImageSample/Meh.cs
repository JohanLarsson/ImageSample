using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ImageSample
{
    public static class Meh
    {
        public static IEnumerable<DependencyObject> VisualAncestors(this DependencyObject o)
        {
            for (var parent = VisualTreeHelper.GetParent(o); parent != null; parent = VisualTreeHelper.GetParent(parent))
            {
                yield return parent;
            }
        }
    }
}