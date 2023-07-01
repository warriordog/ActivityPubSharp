// using System.Text.Json.Serialization;
// using ActivityPub.Types.Collection;
// using ActivityPub.Types.Json;
//
// namespace ActivityPub.Types.Tests.Unit.Json;
//
// public abstract class ASCollectionConverterTests
// {
//     private ASCollectionConverter ConverterFactory { get; set; } = new();
//
//     private ASCollection<ASObject>? Read(ref ReadOnlySpan<byte> jsonBytes, JsonSerializerOptions? options = null)
//     {
//         return Read<ASObject>(ref jsonBytes, options);
//     }
//     
//     private ASCollection<T>? Read<T>(ref ReadOnlySpan<byte> jsonBytes, JsonSerializerOptions? options = null)
//     where T : ASObject
//     {
//         var collectionType = typeof(ASCollection<T>);
//         options ??= JsonLdSerializerOptions.Default;
//         
//         // Quick check
//         ConverterFactory.CanConvert(collectionType).Should().BeTrue();
//         
//         // Read it
//         var reader = new Utf8JsonReader(jsonBytes);
//         reader.Read();
//         
//         // Convert it
//         var converter = (JsonConverter<ASCollection<T>>)ConverterFactory.CreateConverter(collectionType, options);
//         return converter.Read(ref reader, collectionType, options);
//     }
//
//     private JsonConverter<T> CreateConverter<T>(JsonSerializerOptions? options = null)
//     {
//         options ??= JsonLdSerializerOptions.Default;
//         ConverterFactory.CanConvert(typeof(T)).Should().BeTrue();
//         return (JsonConverter<T>)ConverterFactory.CreateConverter(typeof(T), options);
//     }
//
//     public class PagedCollectionShould : ASCollectionConverterTests
//     {
//         
//     }
//
//     public class OrderedPagedCollectionShould : ASCollectionConverterTests
//     {
//         
//     }
//     
//     public class UnpagedCollectionShould : ASCollectionConverterTests
//     {
//         [Fact]
//         public void InferFromCollectionTypeAndItemsProperty()
//         {
//             var json = """{"type":"Collection","items":[]}"""u8;
//             Read(ref json).Should().BeOfType<ASUnpagedCollection<ASObject>>();
//         }
//
//         [Fact]
//         public void InferFromASCollectionDeclarationAndItemsProperty()
//         {
//             var json = """{"items":[]}"""u8;
//             Read<ASCollection<ASObject>>(ref json).Should().BeOfType<ASUnpagedCollection<ASObject>>();
//         }
//
//         [Fact]
//         public void ParseContents()
//         {
//             var json = """{"type":"Collection","items":[{"type":"Like","id":"https://example.com/1","object":{"type":"Note"}}]}"""u8;
//             var obj = Read(ref json).As<ASUnpagedCollection<ASObject>>();
//             obj.Items.Should().HaveCount(1);
//             obj.Items[1].HasValue.Should().BeTrue();
//             obj.Items
//         }
//
//         [Fact]
//         public void PopulateTotalItemsBasedOnItemsCount()
//         {
//             
//         }
//     }
//
//     public class OrderedUnpagedCollectionShould : ASCollectionConverterTests
//     {
//         
//     }
//
//     public class CollectionPageShould : ASCollectionConverterTests
//     {
//         
//     }
//
//     public class OrderedCollectionPageShould : ASCollectionConverterTests
//     {
//         
//     }
// }