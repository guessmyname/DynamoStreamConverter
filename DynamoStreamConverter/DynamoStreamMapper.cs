using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Amazon.DynamoDBv2.Model;

namespace AWSLambda.StreamTest
{
    public static class DynamoStreamMapper<T> where T : class, new()
    {
        public static T Convert(Dictionary<string, AttributeValue> convert)
        {
            var instance = MapPropertiesToTypeInsance(typeof(T), convert);

            return instance as T;
        }

        private static object MapPropertiesToTypeInsance(Type instanceType, Dictionary<string, AttributeValue> convert)
        {
            var instance = Activator.CreateInstance(instanceType);
            
            var props = instance.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamoPropertyAttribute), true));
            
            foreach (var prop in props)

            {
                var propAttr = prop.GetCustomAttributes(typeof(DynamoPropertyAttribute), true).Single();

                Debug.Assert(propAttr != null);

                switch (propAttr)
                {
                    case DyanmoMapPropertyAttribue m:

                        if (!convert.ContainsKey(m.Name)) continue;

                        var map = m.GetValue(convert[m.Name]) as Dictionary<string, AttributeValue>;

                        var childInstance = MapPropertiesToTypeInsance(prop.PropertyType, map);

                        prop.SetValue(instance, childInstance);

                        break;
                    case DynamoPropertyAttribute a:
                        if (!convert.ContainsKey(a.Name)) continue;

                        var value = a.GetValue(convert[a.Name]);

                        prop.SetValue(instance, value);
                        break;
                }
            }

            return instance;
        }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public abstract class DynamoPropertyAttribute : Attribute
    {
        protected DynamoPropertyAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }


        public abstract object GetValue(AttributeValue value);
    }


    public class DyanmoMapPropertyAttribue : DynamoPropertyAttribute
    {
        public DyanmoMapPropertyAttribue(string name) : base(name)
        {
        }

        public override object GetValue(AttributeValue value)
        {
            return value.M;
        }
    }


    public class DynamoStringPropertyAttribute : DynamoPropertyAttribute
    {
        public DynamoStringPropertyAttribute(string name) : base(name)
        {
        }

        public override object GetValue(AttributeValue value)
        {
            return value.S;
        }
    }
}