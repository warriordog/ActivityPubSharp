// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
//
// using System.Reflection;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using ActivityPub.Types.Collection;
// using ActivityPub.Types.Internal;
// using static ActivityPub.Types.Collection.CollectionTypes;
//
// namespace ActivityPub.Types.Json;
//
// public class ASCollectionConverter : JsonConverterFactory
// {
//     // We only convert concrete types deriving from ASCollection<T>
//     public override bool CanConvert(Type type) => type.IsAssignableToGenericType(typeof(ASCollection<>));
//
//     // Pivot the type into correct converter
//     public override JsonConverter CreateConverter(Type collectionType, JsonSerializerOptions options)
//     {
//         // Create a copy of options that does not include ASCollectionConverter.
//         // Without this, we would enter an infinite loop because the converter would call itself.
//         var defaultConverterOptions = new JsonSerializerOptions(options).RemoveConvertersOfType<ASCollectionConverter>();
//
//         // Identify generic types
//         var objectType = collectionType.GetGenericArgumentsFor(typeof(ASCollection<>))[0];
//         var converterType = typeof(ASCollectionConverter<,>).MakeGenericType(objectType, collectionType);
//
//         // Create an instance
//         return (JsonConverter)Activator.CreateInstance(
//             converterType,
//             BindingFlags.Instance | BindingFlags.Public,
//             binder: null,
//             args: new object[] { defaultConverterOptions },
//             culture: null
//         )!;
//     }
// }
//
// internal class ASCollectionConverter<TObject, TCollection> : JsonConverter<TCollection>
//     where TObject : ASObject
//     where TCollection : ASCollection<TObject>
// {
//     private readonly JsonSerializerOptions _defaultOptions;
//     public ASCollectionConverter(JsonSerializerOptions defaultOptions) => _defaultOptions = defaultOptions;
//
//
//     public override TCollection? Read(ref Utf8JsonReader reader, Type declaredType, JsonSerializerOptions options)
//     {
//         // Parse into abstract form
//         var jsonInfo = JsonDocument.ParseValue(ref reader);
//
//         // Identify the correct subtype
//         var actualType = FindActualType(jsonInfo.RootElement, declaredType);
//         
//         // Deserialize it!
//         return (TCollection?)jsonInfo.Deserialize(actualType, _defaultOptions);
//     }
//
//     private static Type FindActualType(JsonElement jsonInfo, Type declaredType)
//     {
//         // Narrow to a more-specific collection type
//         var actualType = GetActualOpenType(jsonInfo, declaredType);
//
//         // Type may be an open generic.
//         // If so, we need to fill it
//         if (actualType.IsGenericType && !actualType.IsConstructedGenericType)
//             actualType = FillOpenGeneric(actualType, declaredType);
//
//         // Make sure the types are compatible.
//         if (!actualType.IsAssignableTo(declaredType))
//             throw new JsonException($"JSON collection of type {actualType} is not assignable to declared type {declaredType}");
//
//         return actualType;
//     }
//
//     private static Type GetActualOpenType(JsonElement jsonInfo, Type declaredType)
//     {
//         // If "type" property is included, then we should use that to figure out what was really sent
//         if (jsonInfo.TryGetProperty("type", out var asTypeElement) && asTypeElement.TryGetString(out var asType))
//             return ReadActualOpenType(jsonInfo, asType);
//         
//         // Otherwise, we can infer it based on declared type and included properties
//         return InferActualOpenType(jsonInfo, declaredType);
//     }
//     
//     private static Type ReadActualOpenType(JsonElement jsonInfo, string asType) => asType switch
//     {
//         CollectionType => jsonInfo.HasProperty("items")
//             ? typeof(ASUnpagedCollection<>)
//             : typeof(ASPagedCollection<>),
//
//         OrderedCollectionType => jsonInfo.HasProperty("orderedItems")
//             ? typeof(ASOrderedCollection<>)
//             : typeof(ASOrderedPagedCollection<>),
//
//         CollectionPageType => typeof(ASCollectionPage<>),
//
//         OrderedCollectionPageType => typeof(ASOrderedCollectionPage<>),
//
//         _ => throw new JsonException($"Could not match object of type {asType} to any known collection type")
//     };
//
//     private static Type InferActualOpenType(JsonElement jsonInfo, Type declaredType)
//     {
//         var isPage = declaredType.IsAssignableToGenericType(typeof(ASCollectionPage<>));
//         
//         // Ordered = no, paged = no
//         if (jsonInfo.HasProperty("items"))
//             return isPage
//                 ? typeof(ASCollectionPage<>)
//                 : typeof(ASUnpagedCollection<>);
//
//         // Ordered = yes, paged = no
//         if (jsonInfo.HasProperty("orderedItems"))
//             return isPage
//                 ? typeof(ASOrderedCollectionPage<>)
//                 : typeof(ASOrderedCollection<>);
//
//         // Ordered = yes, paged = yes
//         if (declaredType.IsAssignableToGenericType(typeof(ASOrderedPagedCollection<>)))
//             return typeof(ASOrderedPagedCollection<>);
//         
//         // Ordered = no, paged = no
//         if (declaredType.IsAssignableToGenericType(typeof(ASOrderedCollection<>)))
//             return typeof(ASOrderedCollection<>);
//         
//         // Oops
//         throw new JsonException($"Could not determine the collection type based on the provided JSON and base type {declaredType}");
//     }
//
//     private static Type FillOpenGeneric(Type actualType, Type declaredType)
//     {
//         // Figure out what to put in the generic slot (alternately - find the T in ASCollection<T>)
//         var itemType = GetGenericItemType(declaredType);
//         
//         // Close actualType by filling the slot
//         return actualType.MakeGenericType(itemType);
//     }
//     
//     private static Type GetGenericItemType(Type type)
//     {
//         // Happy path - its a constructed generic
//         if (type.IsConstructedGenericType)
//             return type.GetGenericArguments()[0];
//         
//         // Failure condition - its an open generic
//         if (type.IsGenericType)
//             throw new ArgumentException($"Type {type} is an open generic type", nameof(type));
//         
//         // Unhappy path - its not generic at all.
//         // Use ASType since that should trigger re-entry into the serializer.
//         return typeof(ASType);
//     }
//
//     // Re-enter the serializer 
//     public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, _defaultOptions);
// }