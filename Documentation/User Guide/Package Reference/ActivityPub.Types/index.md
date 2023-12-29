# ActivityPub.Types

Models and serialization logic for ActivityPub types.
Out-of-the-box, supports the base and extended ActivityStreams types including collections.
Supports simple JSON-LD in the compacted form.
Provides a [pre-configured serializer](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) to convert between models and JSON.

## Usage

This package is loaded automatically by higher-level packages.
For standalone usage, call [`TryAddTypesModule`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/TypesModule.cs) to register all services for DI.

## Built-in Types

Standard ActivityStreams and ActivityPub types are modeled under the `ActivityPub.Types.AS` namespace.
Types are implemented as classes and grouped as follows:

* [`ActivityPub.Types.AS`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS) - base types.
* [`ActivityPub.Types.AS.Collection`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Collection) - Collection / page types.
* [`ActivityPub.Types.AS.Extended.Object`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Extended/Object) - Extended Object types. These all derive from [`ASObject`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASObject.cs).
* [`ActivityPub.Types.AS.Extended.Link`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Extended/Link) - Extended Link types. These all derive from [`ASLink`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASLink.cs).
* [`ActivityPub.Types.AS.Extended.Actor`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Extended/Actor) - Extended Actor types. These all derive from [`APActor`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/APActor.cs).
* [`ActivityPub.Types.AS.Extended.Activity`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Extended/Activity) - Extended Activity types. These all derive from [`ASActivity`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASActivity.cs) or one of the synthetic types described below.

## Synthetic Types

To improve developer experience, a few synthetic intermediate types have been introduced to the hierarchy.
These are as follows:

* [`ASType`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASType.cs) - a common type between [`Link`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASLink.cs) and [`Object`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASObject.cs). This enables support for the `Range: Link | Object` construction that is common within the ActivityStreams specification.
* [`APActor`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/APActor.cs) - base type for Actor objects. Defined object types may extend this directly, and any other compatible object (includes Inbox and Outbox) will be "promoted" to include this as a standalone.

## Entities

### Overview

In addition to the "model" types described above, ActivityPubSharp includes a second, lower-level type model referred to as the "entity system".
This feature can be ignored in most cases, but it is absolutely critical to use or implement an ActivityStream extension.

### Usage

The most common usage of the entity system can utilize a mid-level "easy path" - three special functions that are defined on all types.

* `Is<TModel>()` - returns true if the object includes a provided model, either as a base / super type or as an entirely separate type defined in the `type` property.
* `Is<TModel>(out TModel? instance)` - same as before, but also constructs and returns an instance of the target model. Follows the `TryGet` pattern.
* `As<TModel>()` - same as before, but returns the constructed model instead of a boolean. Throws an exception if the object doesn't contain the provided type.

These functions should be sufficient to comfortably work with most valid ActivityPub objects, even polymorphic ones.
All related instances are automatically linked - that is, changes to one instance will be reflected in all others associated with the same object.
This is true even for base types and instances that are constructed later.


### Implementation

Entities are implemented as an extra **sealed** and **non-inheriting** class associated with each type.
By convention, they are named the same as the type class with an `Entity` suffix.
For example, `ASObject` has an associated entity called `ASObjectEntity`.
Entities are typically placed in the same file as their associated type because the two are tightly coupled.
Typically, application code will not need to work with entity types directly.
However, it will be necessary if implementing a new extension or handling some rare edge cases.

### Models vs Entities

* Models utilize **inheritance**, while entities use **composition**.
* Entities are automatically constructed during JSON conversion, while models are lazy-constructed on-use.
* Within a single type graph, entities are singletons while models may be duplicated.
* Models represent the "expected" shape of an AS object. Entities model the "actual" runtime shape.

### Entity Mapping

APSharp includes a 4-step process to determine which entities should be constructed from an incoming JSON message.
All entities detected by at least one test will be converted and parsed.
1. Map values of the `type` property to registered AS type names.
2. Try all entities that implement [`INamelessEntity`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/Overrides/INamelessEntity.cs).
3. Try all entities that implement [`IAnonymousEntity`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/Overrides/IAnonymousEntity.cs).
4. Try all configured [`IAnonymousEntitySelector`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/Overrides/IAnonymousEntitySelector.cs) instances.

## Utility Types

These types are implemented to support certain special-case properties.
Most exist for DX only, although a few are needed for type-model accuracy or serialization purposes.

* [`ASUri`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/ASUri.cs) - (will) implement the namespaced IRI concept used in JSON-LD. For now, just a wrapper around `Uri`.
* [`JsonLDContext`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/JsonLDContext.cs) - provides a consolidated view of the JSON-LD context associated with an object.
* [`JsonLDContextObject`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/JsonLDContextObject.cs) - encapsulates the `object | IRI` typing used in the JSON-LD context. Does not parse or enforce IRI correctness - its just a `string`.
* [`JsonLDTerm`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/JsonLDTerm.cs) - encapsulates the `object | IRI` typing used in JSON-LD term definitions. Does not parse or enforce IRI correctness - its just a `string`.
* [`Linkable<T>`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/Linkable.cs) - encapsulates the `Range: Link | T` construction used in ActivityStreams.
* [`LinkableList<T>`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/LinkableList.cs) - encapsulates the `Range: (Link | T)[]` construction used in ActivityStreams, as well as the extra cursed `Link | T | (Link | T)[]` variant.
* [`NaturalLanguageString`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Util/NaturalLanguageString.cs) - models BCP47 language-tag mapped strings.

## Collections

ActivityStreams Collection types are implemented in the [`ActivityPub.Types.AS.Collection` namespace](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/Collection).
All four basic types are modeled, and the implementations support extensions like any other type.

Collections are, currently, a weak point in the library.
They are fully implemented and functional, but the API is not particularly ergonomic.
One notable limitation is that ordered and unordered collections do not have a common base type, which makes it difficult to implement code that handles either form.
Another limitation is `TotalItems` - if unset, it returns the size of `Items`.
But once set to any value, it cannot be reset to null and therefore the original logic is unavailable.
These limitations will be resolved at a later date.

## Extensibility

### Overview

ActivityPubSharp includes support for most valid ActivityStreams extensions.
Custom types are supported, and extensions of built-in types can be emulated by inheriting from the original type.
JSON-LD contexts are followed while constructing extension types, although property names are not currently remapped.

### Usage

At startup, the module scans all loaded assemblies and registers any type deriving from [ASType](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/AS/ASType.cs) and implementing [IASModel](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/IASModel.cs).
Registered extension classes will be automatically constructed by the [polymorphic converter](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/Converters/TypeMapConverter.cs).
Polymorphic extension types will be handled by the Entities system described above.

Extensions types should include a link to the context that defines them.
If the extension in question is not properly defined (has no context), then it can be excluded.
However, extension classes without a defined context will instead be placed into the ActivityStreams context, which will apply them to
**all** objects.

See the [ActivityStreams documentation](https://www.w3.org/TR/activitystreams-core/#extensibility) for more information on extensibility.

## Limitations

* Several properties and edge cases are not correctly handled. See the [issue tracker](https://github.com/warriordog/ActivityPubSharp/issues) for details.
* Only a minimal subset of JSON-LD is supported. For full compatibility, it is necessary to override the default [JsonLdSerializer](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) and pre-process the incoming JSON.

## Available Options

| Options Class                                                    | Configuration Path          | Description                                                                |
|------------------------------------------------------------------|-----------------------------|----------------------------------------------------------------------------|
| [JsonLdSerializerOptions](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializerOptions.cs) | N/A - not mapped by default | Options for the default [JsonLdSerializer](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) |
| [ConversionOptions](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/Overrides/ConversionOptions.cs)   | N/A - not mapped by default | General JSON / JSON-LD conversion options                                  |

## Available Services

| Service                                               | Implementation                                          | Description                                                           |
|-------------------------------------------------------|---------------------------------------------------------|-----------------------------------------------------------------------|
| [`IJsonLdSerializer`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) | [`JsonLdSerializer.cs`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) | Parses and serializes ActivityStreams messages from JSON or JSON-LD.  |
| [`IASTypeInfoCache`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/ASTypeInfoCache.cs)   | [`ASTypeInfoCache`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Types/Conversion/ASTypeInfoCache.cs)      | Indexes metadata related to AS / AP types defined in the application. |
