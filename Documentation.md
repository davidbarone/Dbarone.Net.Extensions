# Assembly: Dbarone.Net.Extensions

>## type: CollectionExtensions
Namespace: Dbarone.Net.Extensions

 Extension methods for collections and sequences. 

>### Sample``1
Namespace: `Dbarone.Net.Extensions.CollectionExtensions`

 Returns a statistically random subset of data. 

|Param | Description |
|-----|-----|
|T: |The type of the data element.|

|Name | Description |
|-----|------|
|data: |The data to provide a sample from.|
|sampleSize: |The sample size.|


>### CartesianProduct``1
Namespace: `Dbarone.Net.Extensions.CollectionExtensions`

 Creates a cartesian product of n sequences. The resulting sequence contains all combinations. Each element has n items in the sequence. Taken from: https://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/ 

|Param | Description |
|-----|-----|
|T: |The type of the element|

|Name | Description |
|-----|------|
|sequences: |The data.|


>### Splice``1
Namespace: `Dbarone.Net.Extensions.CollectionExtensions`

 Splices an array. 

|Param | Description |
|-----|-----|
|T: |The element type of the array.|

|Name | Description |
|-----|------|
|source: |The source array to splice.|
|start: |The start element to splice elements from.|
|number: |The number of elements to remove.|


>### Union
Namespace: `Dbarone.Net.Extensions.CollectionExtensions`

 Creates a collection out of a set of objects. Individual items can themselves be collections or not. 



|Name | Description |
|-----|------|
|obj1: |The current object.|
|obj2: |A set of additional objects.|



>## type: ObjectExtensions
Namespace: Dbarone.Net.Extensions

 A collection of object extension methods. 

>### Extend
Namespace: `Dbarone.Net.Extensions.ObjectExtensions`

 Merges properties from multiple objects. 



|Name | Description |
|-----|------|
|obj1: |The current object.|
|obj2: |A variable number of objects to merge into the current object.|


>### ToObject``1
Namespace: `Dbarone.Net.Extensions.ObjectExtensions`

 Converts a dictionary to a POCO object. 

|Param | Description |
|-----|-----|
|T: |The object type to convert the dictionary to.|

|Name | Description |
|-----|------|
|dict: |The dictionary containing the source values.|


>### ToDictionary
Namespace: `Dbarone.Net.Extensions.ObjectExtensions`

 Converts an object to a dictionary. 



|Name | Description |
|-----|------|
|obj: |The object to convert to a dictionary.|
|keyMapper: |Optional Func to map key names.|
|valueMapper: |Optional Fun to map values. The Func parameters are key (string) and object (value).|


>### CompareTo
Namespace: `Dbarone.Net.Extensions.ObjectExtensions`

 Compares the current object to another object. 



|Name | Description |
|-----|------|
|obj1: |The first object.|
|obj2: |The second object to compare.|


>### ValueEquals
Namespace: `Dbarone.Net.Extensions.ObjectExtensions`

 Compares 2 objects and returns true if they are equivalent in value. Reference types are compared by doing a ValueEquals on all public properties and fields recursively, and collections are compared by element. For collections, order is important. 



|Name | Description |
|-----|------|
|obj1: |First object to compare.|
|obj2: |Second object to compare.|



>## type: ReflectionExtensions
Namespace: Dbarone.Net.Extensions

 A collection of .NET reflection extension methods. 

>### GetPropertiesDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of properties on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the properties defined for the current System.Type , using the specified binding constraints.|


>### GetPropertiesDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of properties for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the properties defined for the current System.Type , using the specified binding constraints.|


>### GetMembersDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of members on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the members defined for the current System.Type , using the specified binding constraints.|


>### GetMembersDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of members for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the members defined for the current System.Type , using the specified binding constraints.|


>### GetMethodsDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of methods on an object decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|obj: |The object instance to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the methods defined for the current System.Type , using the specified binding constraints.|


>### GetMethodsDecoratedBy``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of methods for a given type decorated by the specified attribute type. 

|Param | Description |
|-----|-----|
|T: |The specified attribute type.|

|Name | Description |
|-----|------|
|t: |The object type to check.|
|inherit: |If true, specifies to also search the ancestors of element for custom attributes.|
|bindingFlags: |When overridden in a derived class, searches for the methods defined for the current System.Type , using the specified binding constraints.|


>### Value
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Gets the value of a object's property using reflection. 



|Name | Description |
|-----|------|
|obj: |The object to get the value from.|
|propertyName: |The name of the property|


>### GetTypesAssignableFrom
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of types in an app domain that a specified base type is assignable from. 



|Name | Description |
|-----|------|
|domain: |The AppDomain to search for types.|
|baseType: |The base type.|


>### GetSubclassTypesOf``1
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns a collection of types in an AppDomain that are a subclass of a specified base type. 

|Param | Description |
|-----|-----|
|T: |The base type.|

|Name | Description |
|-----|------|
|domain: |The AppDomain to search for types.|


>### IsIndexerProperty
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns true if a PropertyInfo object is an indexer property. 



|Name | Description |
|-----|------|
|prop: |The property to check.|


>### IsNumeric
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns true if the .NET type is a numeric type. 



|Name | Description |
|-----|------|
|type: |The type to check.|


>### Default
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns the default value for a type. For value types, returns default value. For reference types, returns null. 



|Name | Description |
|-----|------|
|type: |The type.|


>### GetNullableType
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Gets the nullable type from a non-nullable type. 



|Name | Description |
|-----|------|
|type: |The non-nullable type. Can be value or reference type. Reference types are assumed to be nullable already.|


>### GetNullableUnderlyingType
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Gets the underlying type for a nullable type. 



|Name | Description |
|-----|------|
|type: |The nullable type.|


>### IsNullable
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Returns whether a type is a nullable type. 



|Name | Description |
|-----|------|
|t: ||


>### Parse
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Parses a string to another type. The type must support the Parse method. 



|Name | Description |
|-----|------|
|str: |The string value to parse.|
|type: |The type of value to parse the string into.|


>### ParseNullable
Namespace: `Dbarone.Net.Extensions.ReflectionExtensions`

 Parses a string to another type. The type must support the Parse method. Null or empty strings are 



|Name | Description |
|-----|------|
|type: |The type convert the string value into. Must support a nullable version of the type.|
|str: |The string to parse.|



>## type: Justification
Namespace: Dbarone.Net.Extensions

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

>## type: StringExtensions
Namespace: Dbarone.Net.Extensions

 A collection of string extension methods. 

>### ToGuid
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Allows a short (TimeLow) guid or full guid to be converted to Guid 



|Name | Description |
|-----|------|
|str: |The input string value.|


>### Justify
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Justifies text. 



|Name | Description |
|-----|------|
|str: |The input string to justify.|
|length: |The length of text.|
|justification: |The justification style.|


>### ParseArgs
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Parses a string for arguments. Arguments can be separated by whitespace. Single or double quotes can be used to delimit fields that contain space characters. 



|Name | Description |
|-----|------|
|str: |The input string to parse.|


>### IsNullOrWhiteSpace
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Wrapper for .NET IsNullOrWhiteSpace method. 



|Name | Description |
|-----|------|
|str: |Input value to test.|


>### IsNullOrEmpty
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Wrapper for .NET IsNullOrEmpty method. 



|Name | Description |
|-----|------|
|str: |input value to test.|


>### RemoveRight
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Removes characters from the right end of a string 



|Name | Description |
|-----|------|
|str: |The input string.|
|length: |The required length of the string.|


>### RemoveLeft
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Removes characters from the left end of a string 



|Name | Description |
|-----|------|
|str: |The input string.|
|length: |The required length of the string.|


>### ToStream
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Converts a string value to a stream. 



|Name | Description |
|-----|------|
|str: |The input string.|


>### WordWrap
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Splits a string into chunks of [length] characters. Word breaks are avoided. 



|Name | Description |
|-----|------|
|str: ||
|length: ||


>### ToSnakeCase
Namespace: `Dbarone.Net.Extensions.StringExtensions`

 Converts a string to snake case. 



|Name | Description |
|-----|------|
|str: |The input string value.|


