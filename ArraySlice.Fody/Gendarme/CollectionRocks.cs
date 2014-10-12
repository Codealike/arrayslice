using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corvalius.ArraySlice.Fody.Gendarme
{
    // Here we keep non-Gendarme/Cecil related rocks
    public static class CollectionRocks
    {

        /// <summary>
        /// Checks if the list does not contain the item. If so the item is added.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The list.</param>
        /// <param name="item">The item to add.</param>
        public static void AddIfNew<T>(this ICollection<T> self, T item)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (item == null)
                throw new ArgumentNullException("item");

            if (!self.Contains(item))
                self.Add(item);
        }

        public static void AddRangeIfNew<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (T item in items)
            {
                if (!self.Contains(item))
                    self.Add(item);
            }
        }
    }

    public static class SystemRocks
    {

        /// <summary>
        /// Check if a Version is empty (all zeros).
        /// </summary>
        /// <param name="self">The Version to check</param>
        /// <returns>True if empty, False otherwise.</returns>
        public static bool IsEmpty(this Version self)
        {
            if (self == null)
                return true;
            return ((self.Major == 0) && (self.Minor == 0) && (self.Build <= 0) && (self.Revision <= 0));
        }
    }
}
