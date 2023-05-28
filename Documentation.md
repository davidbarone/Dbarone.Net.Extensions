# Assembly: Dbarone.Net.Extensions
## Contents
- [Dbarone.Net.Extensions.CollectionExtensions](#dbaronenetextensionscollectionextensions)
- [Dbarone.Net.Extensions.ObjectExtensions](#dbaronenetextensionsobjectextensions)
- [Dbarone.Net.Extensions.ReflectionExtensions](#dbaronenetextensionsreflectionextensions)
- [Dbarone.Net.Extensions.Justification](#dbaronenetextensionsjustification)
- [Dbarone.Net.Extensions.StringExtensions](#dbaronenetextensionsstringextensions)



---
## Dbarone.Net.Extensions.CollectionExtensions
Namespace: `Dbarone.Net.Extensions`

 Extension methods for collections and sequences. 

### method: CollectionExtensions.Sample``1
id: `M:Dbarone.Net.Extensions.CollectionExtensions.Sample``1(System.Collections.Generic.IEnumerable{``0},System.Int32)`

 Returns a statistically random subset of data. 

|Param | Description |
|-----|-----|
|T: |The type of the data element.|

|Name | Description |
|-----|------|
|data: |The data to provide a sample from.|
|sampleSize: |The sample size.|



#### Examples:


This shows how to increment an integer.
``` c#
    var index = 5;
    index++;
    
```


Another example.
``` c#
    var index = 6;
    index++;
    
```


### method: CollectionExtensions.CartesianProduct``1
id: `M:Dbarone.Net.Extensions.CollectionExtensions.CartesianProduct``1(System.Collections.Generic.IEnumerable{System.Collections.Generic.IEnumerable{``0}})`

 Creates a cartesian product of n sequences. The resulting sequence contains all combinations. Each element has n items in the sequence. Taken from: https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/ 

|Param | Description |
|-----|-----|
|T: |The type of the element|

|Name | Description |
|-----|------|
|sequences: |The data.|



### method: CollectionExtensions.Splice``1
id: `M:Dbarone.Net.Extensions.CollectionExtensions.Splice``1(``0[],System.Int64,System.Nullable{System.Int64})`

 Splices an array. 

|Param | Description |
|-----|-----|
|T: |The element type of the array.|

|Name | Description |
|-----|------|
|source: |The source array to splice.|
|start: |The start element to splice elements from.|
|number: |The number of elements to remove.|



### method: CollectionExtensions.Union
id: `M:Dbarone.Net.Extensions.CollectionExtensions.Union(System.Object,System.Object[])`

 Creates a collection out of a set of objects. Individual items can themselves be collections or not. 



|Name | Description |
|-----|------|
|obj1: |The current object.|
|obj2: |A set of additional objects.|




---
## Dbarone.Net.Extensions.ObjectExtensions
Namespace: `Dbarone.Net.Extensions`

 A collection of object extension methods. 

### method: ObjectExtensions.Extend
id: `M:Dbarone.Net.Extensions.ObjectExtensions.Extend(System.Object,System.Object[])`

 Merges properties from multiple objects. 



|Name | Description |
|-----|------|
|obj1: |The current object.|
|obj2: |A variable number of objects to merge into the current object.|



### method: ObjectExtensions.ToObject``1
id: `M:Dbarone.Net.Extensions.ObjectExtensions.ToObject``1(System.Collections.Generic.IDictionary{System.String,System.Object})`

 Converts a dictionary to a POCO object. 

|Param | Description |
|-----|-----|
|T: |The object type to convert the dictionary to.|

|Name | Description |
|-----|------|
|dict: |The dictionary containing the source values.|



### method: ObjectExtensions.ToDictionary
id: `M:Dbarone.Net.Extensions.ObjectExtensions.ToDictionary(System.Object,System.Func{System.String,System.String},System.Func{System.String,System.Object,System.Object})`

 Converts an object to a dictionary. 



|Name | Description |
|-----|------|
|obj: |The object to convert to a dictionary.|
|keyMapper: |Optional Func to map key names.|
|valueMapper: |Optional Fun to map values. The Func parameters are key (string) and object (value).|



### method: ObjectExtensions.CompareTo
id: `M:Dbarone.Net.Extensions.ObjectExtensions.CompareTo(System.Object,System.Object)`

 Compares the current object to another object. 



|Name | Description |
|-----|------|
|obj1: |The first object.|
|obj2: |The second object to compare.|



### method: ObjectExtensions.ValueEquals
id: `M:Dbarone.Net.Extensions.ObjectExtensions.ValueEquals(System.Object,System.Object)`

 Compares 2 objects and returns true if they are equivalent in value. Reference types are compared by doing a ValueEquals on all public properties and fields recursively, and collections are compared by element. For collections, order is important. 



|Name | Description |
|-----|------|
|obj1: |First object to compare.|
|obj2: |Second object to compare.|




---
## Dbarone.Net.Extensions.ReflectionExtensions
Namespace: `Dbarone.Net.Extensions`

 A collection of .NET reflection extension methods. 

### method: ReflectionExtensions.GetPropertiesDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetPropertiesDecoratedBy``1(System.Object,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of properties on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the properties defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.GetPropertiesDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetPropertiesDecoratedBy``1(System.Type,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of properties for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the properties defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.GetMembersDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetMembersDecoratedBy``1(System.Object,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of members on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the members defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.GetMembersDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetMembersDecoratedBy``1(System.Type,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of members for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the members defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.GetMethodsDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetMethodsDecoratedBy``1(System.Object,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of methods on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the methods defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.GetMethodsDecoratedBy``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetMethodsDecoratedBy``1(System.Type,System.Boolean,System.Reflection.BindingFlags)`

 Returns a collection of methods for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The object type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the methods defined for the current System.Type , using the specified binding constraints.|



### method: ReflectionExtensions.Value
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.Value(System.Object,System.String)`

 Gets the value of a object's property using reflection. 



|Name | Description |
|-----|------|
|obj: |The object to get the value from.|
|propertyName: |The name of the property|



### method: ReflectionExtensions.GetTypesAssignableFrom
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetTypesAssignableFrom(System.AppDomain,System.Type)`

 Returns a collection of types in an app domain that a specified base type is assignable from. 



|Name | Description |
|-----|------|
|domain: |The AppDomain to search for types.|
|baseType: |The base type.|



### method: ReflectionExtensions.GetSubclassTypesOf``1
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetSubclassTypesOf``1(System.AppDomain)`

 Returns a collection of types in an AppDomain that are a subclass of a specified base type. 

|Param | Description |
|-----|-----|
|T: |The base type.|

|Name | Description |
|-----|------|
|domain: |The AppDomain to search for types.|



### method: ReflectionExtensions.IsIndexerProperty
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.IsIndexerProperty(System.Reflection.PropertyInfo)`

 Returns true if a PropertyInfo object is an indexer property. 



|Name | Description |
|-----|------|
|prop: |The property to check.|



### method: ReflectionExtensions.IsNumeric
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.IsNumeric(System.Type)`

 Returns true if the .NET type is a numeric type. 



|Name | Description |
|-----|------|
|type: |The type to check.|



### method: ReflectionExtensions.Default
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.Default(System.Type)`

 Returns the default value for a type. For value types, returns default value. For reference types, returns null. 



|Name | Description |
|-----|------|
|type: |The type.|



### method: ReflectionExtensions.GetNullableType
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetNullableType(System.Type)`

 Gets the nullable type from a non-nullable type. 



|Name | Description |
|-----|------|
|type: |The non-nullable type. Can be value or reference type. Reference types are assumed to be nullable already.|



### method: ReflectionExtensions.GetNullableUnderlyingType
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.GetNullableUnderlyingType(System.Type)`

 Gets the underlying type for a nullable type. 



|Name | Description |
|-----|------|
|type: |The nullable type.|



### method: ReflectionExtensions.IsNullable
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.IsNullable(System.Type)`

 Returns whether a type is a nullable type. 



|Name | Description |
|-----|------|
|t: ||



### method: ReflectionExtensions.Parse
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.Parse(System.Type,System.String)`

 Parses a string to another type. The type must support the Parse method. 



|Name | Description |
|-----|------|
|str: |The string value to parse.|
|type: |The type of value to parse the string into.|



### method: ReflectionExtensions.ParseNullable
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.ParseNullable(System.Type,System.String)`

 Parses a string to another type. The type must support the Parse method. Null or empty strings are 



|Name | Description |
|-----|------|
|type: |The type convert the string value into. Must support a nullable version of the type.|
|str: |The string to parse.|



### method: ReflectionExtensions.CanConvertTo
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.CanConvertTo(System.Object,System.Type)`

 Determines whether an object can be converted to a specific type. Wrapper function for the the System.Convert class. For details of conversion rules, see: https://learn.microsoft.com/en-us/dotnet/api/system.convert?view=net-7.0 



|Name | Description |
|-----|------|
|obj: |The object to be converted.|
|conversionType: |The target type the object is to be converted to.|



### method: ReflectionExtensions.ConvertTo
id: `M:Dbarone.Net.Extensions.ReflectionExtensions.ConvertTo(System.Object,System.Type)`

 Attempts to convert an object to a specific type. Returns null if the object cannot be converted to the conversion Type. 



|Name | Description |
|-----|------|
|obj: |The object to be converted.|
|conversionType: |The target type the object is to be converted to.|




---
## Dbarone.Net.Extensions.Justification
Namespace: `Dbarone.Net.Extensions`

 The justification type. 

### F:Dbarone.Net.Extensions.Justification.LEFT
 Left-aligned 

---
### F:Dbarone.Net.Extensions.Justification.CENTRE
 Centre-aligned 

---
### F:Dbarone.Net.Extensions.Justification.RIGHT
 Right-aligned 

---

---
## Dbarone.Net.Extensions.StringExtensions
Namespace: `Dbarone.Net.Extensions`

 A collection of string extension methods. 

### method: StringExtensions.ToGuid
id: `M:Dbarone.Net.Extensions.StringExtensions.ToGuid(System.String)`

 Allows a short (TimeLow) guid or full guid to be converted to Guid 



|Name | Description |
|-----|------|
|str: |The input string value.|



### method: StringExtensions.Justify
id: `M:Dbarone.Net.Extensions.StringExtensions.Justify(System.String,System.Int32,Dbarone.Net.Extensions.Justification)`

 Justifies text. 



|Name | Description |
|-----|------|
|str: |The input string to justify.|
|length: |The length of text.|
|justification: |The justification style.|



### method: StringExtensions.ParseArgs
id: `M:Dbarone.Net.Extensions.StringExtensions.ParseArgs(System.String)`

 Parses a string for arguments. Arguments can be separated by whitespace. Single or double quotes can be used to delimit fields that contain space characters. 



|Name | Description |
|-----|------|
|str: |The input string to parse.|



### method: StringExtensions.IsNullOrWhiteSpace
id: `M:Dbarone.Net.Extensions.StringExtensions.IsNullOrWhiteSpace(System.String)`

 Wrapper for .NET IsNullOrWhiteSpace method. 



|Name | Description |
|-----|------|
|str: |Input value to test.|



### method: StringExtensions.IsNullOrEmpty
id: `M:Dbarone.Net.Extensions.StringExtensions.IsNullOrEmpty(System.String)`

 Wrapper for .NET IsNullOrEmpty method. 



|Name | Description |
|-----|------|
|str: |input value to test.|



### method: StringExtensions.RemoveRight
id: `M:Dbarone.Net.Extensions.StringExtensions.RemoveRight(System.String,System.Int32)`

 Removes characters from the right end of a string 



|Name | Description |
|-----|------|
|str: |The input string.|
|length: |The required length of the string.|



### method: StringExtensions.RemoveLeft
id: `M:Dbarone.Net.Extensions.StringExtensions.RemoveLeft(System.String,System.Int32)`

 Removes characters from the left end of a string 



|Name | Description |
|-----|------|
|str: |The input string.|
|length: |The required length of the string.|



### method: StringExtensions.ToStream
id: `M:Dbarone.Net.Extensions.StringExtensions.ToStream(System.String)`

 Converts a string value to a stream. 



|Name | Description |
|-----|------|
|str: |The input string.|



### method: StringExtensions.WordWrap
id: `M:Dbarone.Net.Extensions.StringExtensions.WordWrap(System.String,System.Int32)`

 Splits a string into chunks of [length] characters. Word breaks are avoided. 



|Name | Description |
|-----|------|
|str: ||
|length: ||



### method: StringExtensions.ToSnakeCase
id: `M:Dbarone.Net.Extensions.StringExtensions.ToSnakeCase(System.String)`

 Converts a string to snake case. 



|Name | Description |
|-----|------|
|str: |The input string value.|



