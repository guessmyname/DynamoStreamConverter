namespace DynamoStreamConverter.FSharp

open System.Collections.Generic

module DynamoStreamConverter =
    open Amazon.DynamoDBv2.Model
    open System
    open System.Linq


    let mapprops = 
        fun (t : Type) (v :  Dictionary<string,AttributeValue>) ->
            let instance = Activator.CreateInstance(t)

            let props =             
                query {

                    for prop in  t.GetProperties() do
                    where ( Attribute.IsDefined( prop, typeof<Attribute>, true))
                }

            for prop in props do
                let propAttr = 
                    prop.GetCustomAttributes(typeof<Attribute>, true).Single()

                    match propAttr with
                    | :? Attribute as m -> m

                    return propAttr

            ref instance



    let convert<'T> (z) (x : Dictionary<string,AttributeValue>)  =

        fun (z) (x : Dictionary<string,AttributeValue>) ->
             mapprops z x