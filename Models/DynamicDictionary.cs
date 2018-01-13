using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace PRMS.Models
{
    public class DynamicDictionary : DynamicObject
    {
        // The inner dictionary.
    public    Dictionary<string, object> dictionary
            = new Dictionary<string, object>();


        
        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }
        public void Add(string name, object val = null)
        {
            if (!dictionary.ContainsKey(name))
            {
                dictionary.Add(name, val);
            }
            else
            {
                dictionary[name] = val;
            }
        }
        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name;

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }


        public static object GetPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }



        // Get the property value by index.
        public override bool TryGetIndex(
            GetIndexBinder binder, object[] indexes, out object result)
        {

            int index = (int)indexes[0];
            return dictionary.TryGetValue("Property" + index, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }



        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (dictionary.ContainsKey(binder.Name) &&
                dictionary[binder.Name] is Delegate)
            {
                Delegate del = dictionary[binder.Name] as Delegate;
                result = del.DynamicInvoke(args);
                return true;
            }
            return base.TryInvokeMember(binder, args, out result);
        }
    }

}