using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.DynamoDBv2.Model;

namespace AWSLambda.StreamTest
{
   public class DynamoStreamMapper<T> where T:class, new()
    {
        

        public T Convert(Dictionary<string, AttributeValue> convert)
        {
            Type outType = typeof(T);

            var instance = new T();

            var props = outType.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(DynamoPropertyAttribute)));


            foreach (var prop in props)
            {
                if (prop.GetCustomAttributes(typeof(DynamoPropertyAttribute), true).Single() is DynamoPropertyAttribute attr)
                {
                    var value = attr.GetValue(convert[attr.Name]);

                    prop.SetValue(instance,value);
                }
            }

            return instance;
        }

    }


    [AttributeUsage(AttributeTargets.Property)]

    public abstract class DynamoPropertyAttribute : Attribute
    {
        public string Name { get; }

        public DynamoPropertyAttribute(string name)
        {
            Name = name;
        }


        public abstract object GetValue(AttributeValue value);

    }


    public class DynamoStringPropertyAttribute: DynamoPropertyAttribute
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
