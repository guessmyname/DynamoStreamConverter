using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using AWSLambda.StreamTest;
using Xunit;

namespace DynamoConverter
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            var input = new Dictionary<string, AttributeValue>
            {
                {
                    "Test",
                    new AttributeValue
                    {
                        S = "Apple"
                    }
                }
            };

            var converter = new DynamoStreamMapper<TestClass>();

           var results = converter.Convert(input);

            Assert.Equal("Apple", results.Test);

        }
    }


    public class TestClass
    {

        [DynamoStringProperty("Test")]
        public string Test { get; set; }
    }
}
