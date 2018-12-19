using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using AWSLambda.StreamTest;
using Xunit;

namespace DynamoConverter
{
    public class DynamoConverterTests
    {
        [Fact]
        public void StringAttribute_Value_Mapped_Success()
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
            
           var results = DynamoStreamMapper<TestClass>.Convert(input);

            Assert.Equal("Apple", results.Test);

        }


        [Fact]
        public void MapAttribute_Value_Mapped_Success()
        {

            var input = new Dictionary<string, AttributeValue>
            {
                {
                    "Test",
                    new AttributeValue
                    {
                        S = "Apple"
                    }
                },
                {
                    "MapTest",
                    new AttributeValue
                    {
                       M = new Dictionary<string, AttributeValue>()
                       {
                           {"TestKey", new AttributeValue("Apple") }
                       }
                    }
                }
            };

            var results = DynamoStreamMapper<TestClass>.Convert(input);

            Assert.Equal("Apple", results.MapTestClass.MapTestProp);
        }

    }


    public class TestClass
    {
        public TestClass()
        {
            MapTestClass = new MapTestClass();
        }

        [DynamoStringProperty("Test")]
        public string Test { get; set; }

        [DyanmoMapPropertyAttribue("MapTest")]
        public MapTestClass MapTestClass { get; set; }
    }

    public class MapTestClass
    {
        [DynamoStringProperty("TestKey")]
        public string MapTestProp { get; set; }
    }
}
