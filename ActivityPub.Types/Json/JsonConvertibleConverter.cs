// // This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// // If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
//
// using System.Reflection;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using ActivityPub.Types.Internal;
//
// namespace ActivityPub.Types.Json;
//
// public class JsonConvertibleConverter : JsonConverterFactory
// {
//     private readonly ASTypeRegistry _typeRegistry;
//     public JsonConvertibleConverter(ASTypeRegistry typeRegistry) => _typeRegistry = typeRegistry;
//
//     public override bool CanConvert(Type type) => type.IsConstructedGenericType && type.IsAssignableToGenericType(typeof(IJsonConvertible<>));
//
//     public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
//     {
//         // Create JsonOptions
//         var jsonOptions = new JsonOptions(options, _typeRegistry);
//
//         // Construct converter
//         var converterType = typeof(JsonConvertibleConverter<>).MakeGenericType(typeToConvert);
//         return (JsonConverter)Activator.CreateInstance(
//             converterType,
//             BindingFlags.Instance | BindingFlags.Public,
//             binder: null,
//             args: new object[] { jsonOptions },
//             culture: null
//         )!;
//     }
// }
//
// internal class JsonConvertibleConverter<T> : JsonConverter<T>
//     where T : IJsonConvertible<T>
// {
//     private readonly JsonOptions _jsonOptions;
//     public JsonConvertibleConverter(JsonOptions jsonOptions) => _jsonOptions = jsonOptions;
//
//     public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions _) => T.Deserialize(ref reader, _jsonOptions);
//     public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions _) => T.Serialize(value, writer, _jsonOptions);
// }

