// using System.Text.Json;
// using System.Text.Json.Serialization;
//
// namespace ActivityPub.Types.Json;
//
// /// <summary>
// /// Serializes any type as normal, but no-op on deserialization.
// /// This allows get-only properties to be used.
// /// </summary>
// /// <remarks>
// /// As of 2023-06-28, this is not supported by any built-in method.
// /// https://github.com/dotnet/runtime/issues/30688
// /// https://github.com/dotnet/runtime/issues/34404
// /// </remarks>
// public class WriteOnlyConverter : JsonConverterFactory
// {
//     public override bool CanConvert(Type type) => true;
//
//     // Pivot the type into correct instance
//     public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
//     {
//         var converterType = typeof(WriteOnlyConverter<>).MakeGenericType(type);
//         return (JsonConverter)Activator.CreateInstance(converterType)!;
//     }
// }
//
// internal class WriteOnlyConverter<T> : JsonConverter<T>
// {
//     // No-op
//     public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => default;
//     
//     // Default behavior
//     public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, options);
// }