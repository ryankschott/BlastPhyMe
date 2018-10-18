using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Pilgrimage
{
    public class PropertyComparer<T> : IComparer<T>
    {
        private readonly IComparer comparer;
        private PropertyDescriptor propertyDescriptor;
        private int reverse;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            this.propertyDescriptor = property;
            Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
            this.comparer = (IComparer)comparerForPropertyType.InvokeMember("Default", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public, null, null, null);
            this.SetListSortDirection(direction);
        }

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            if (this.propertyDescriptor.PropertyType == typeof(System.Drawing.Bitmap))
            {
                object xVal = this.propertyDescriptor.GetValue(x);
                object yVal = this.propertyDescriptor.GetValue(y);

                int compare = 0;
                if (xVal != null)
                {
                    if (yVal == null) { compare = 1; }
                }
                else
                {
                    if (yVal != null) { compare = -1; }
                }

                return this.reverse * compare;
            }
            else
            {
                return this.reverse * this.comparer.Compare(this.propertyDescriptor.GetValue(x), this.propertyDescriptor.GetValue(y));
            }
        }

        #endregion

        private void SetPropertyDescriptor(PropertyDescriptor descriptor)
        {
            this.propertyDescriptor = descriptor;
        }

        private void SetListSortDirection(ListSortDirection direction)
        {
            this.reverse = direction == ListSortDirection.Ascending ? 1 : -1;
        }

        public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
        {
            this.SetPropertyDescriptor(descriptor);
            this.SetListSortDirection(direction);
        }
    }
}