# ActivityPub.Types

Models and serialization logic for ActivityPub types.
Out-of-the-box, supports the base and extended ActivityStreams types including collections.
Supports simple JSON-LD in the compacted form.
Provides a [pre-configured serializer](Conversion/JsonLdSerializer.cs) to convert between models and JSON.

## Usage

This package is loaded automatically by higher-level packages.
For standalone usage, call [`TryAddTypesModule`](TypesModule.cs) to register all services for DI.

## Built-in Types

Standard ActivityStreams and ActivityPub types are modeled under the `ActivityPub.Types.AS` namespace.
Types are implemented as classes and grouped as follows:

* [`ActivityPub.Types.AS`](AS) - base types.
* [`ActivityPub.Types.AS.Collection`](AS/Collection) - Collection / page types.
* [`ActivityPub.Types.AS.Extended.Object`](AS/Extended/Object) - Extended Object types. These all derive from [`ASObject`](AS/ASObject.cs).
* [`ActivityPub.Types.AS.Extended.Link`](AS/Extended/Link) - Extended Link types. These all derive from [`ASLink`](AS/ASLink.cs).
* [`ActivityPub.Types.AS.Extended.Actor`](AS/Extended/Actor) - Extended Actor types. These all derive from [`ASActor`](AS/ASActor.cs).
* [`ActivityPub.Types.AS.Extended.Activity`](AS/Extended/Activity) - Extended Activity types. These all derive from [`ASActivity`](AS/ASActivity.cs) or one of the synthetic types described below.

## Synthetic Types

To improve developer experience, a few synthetic intermediate types have been introduced to the hierarchy.
These are as follows:

* [`ASType`](AS/ASType.cs) - a common type between [`Link`](AS/ASLink.cs) and [`Object`](AS/ASObject.cs). This enables support for the `Range: Link | Object` construction that is common within the ActivityStreams specification.
* [`ASActor`](AS/ASActor.cs) - base type for Actor objects. Defined object types may extend this directly, and any other compatible object (includes Inbox and Outbox) will be "promoted" to include this as a standalone.
* [`ASTransitiveActivity`](AS/ASTransitiveActivity.cs) - a mirror to [`IntransitiveActivity`](AS/ASIntransitiveActivity.cs) that implies just the opposite. Transitive activities support the [`object` property](https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object).
* [`ASTargetedActivity`](AS/ASTargetedActivity.cs) - extension of `TransitiveActivity` for activities which contain a [`target` property](https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target).

## Entities

### Overview

In addition to the types described above, ActivityPubSharp includes a second, lower-level type model referred to as the "Entity System".
This feature can be ignored in most cases as it exists to support custom extensions and polymorphic types.
It is, however, absolutely critical for those specific scenarios.

### Usage

The most common usage of the entity system can utilize a mid-level "easy path" - three special functions that are defined on all types.

* `Is<T>()` - returns true if the object includes a provided type, either as a base / super type or as an entirely separate type defined in the `type` property.
* `Is<T>(out T? instance)` - same as before, but also constructs and returns an instance of the target type. Follows the `TryGet` pattern.
* `As<T>()` - same as before, but returns the constructed type instead of a boolean. Throws an exception if the object doesn't contain the provided type.

These functions should be sufficient to comfortably work with most valid ActivityPub objects, even polymorphic ones.
All related instances are automatically linked - that is, changes to one instance will be reflected in all others associated with the same object.
This is true even for base types and instances that are constructed later.

### Implementation

Entities are implemented as an extra **sealed** and **non-inheriting** class associated with each type.
By convention, they are named the same as the type class with an `Entity` suffix.
For example, `ASObject` has an associated entities called `ASObjectEntity`.
Entities are typically placed in the same file as their associated type because the two are tightly coupled.
Typically, application code will not need to work with entity types directly.
However, it will be necessary if implementing a new extension or handling some rare edge cases.

## Utility Types

These types are implemented to support certain special-case properties.
Most exist for DX only, although a few are needed for type-model accuracy or serialization purposes.

* [`ASUri`](Util/ASUri.cs) - (will) implement the namespaced IRI concept used in JSON-LD. For now, just a wrapper around `Uri`.
* [`JsonLDContext`](Util/JsonLDContext.cs) - provides a consolidated view of the JSON-LD context associated with an object.
* [`JsonLDContextObject`](Util/JsonLDContextObject.cs) - encapsulates the `object | IRI` typing used in the JSON-LD context. Does not parse or enforce IRI correctness - its just a `string`.
* [`JsonLDTerm`](Util/JsonLDTerm.cs) - encapsulates the `object | IRI` typing used in JSON-LD term definitions. Does not parse or enforce IRI correctness - its just a `string`.
* [`Linkable<T>`](Util/Linkable.cs) - encapsulates the `Range: Link | T` construction used in ActivityStreams.
* [`LinkableList<T>`](Util/LinkableList.cs) - encapsulates the `Range: (Link | T)[]` construction used in ActivityStreams, as well as the extra cursed `Link | T | (Link | T)[]` variant.
* [`NaturalLanguageString`](Util/NaturalLanguageString.cs) - supports language-tag mapped strings. The current implementation is subpar and will be replaced in [#12](https://github.com/warriordog/ActivityPubSharp/issues/12).

## Collections

ActivityStreams Collection types are implemented in the [`ActivityPub.Types.AS.Collection` namespace](AS/Collection).
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

At startup, the module scans all loaded assemblies and registers any type deriving from [ASType](AS/ASType.cs) and containing an [ASTypeKey](Attributes/ASTypeKeyAttribute.cs) attribute.
Registered extension classes will be automatically constructed by the [polymorphic converter](Conversion/Converters/TypeMapConverter.cs).
Polymorphic extension types will be handled by the Entities system described above.

Extensions types should include a link to the context that defines them.
If the extension in question is not properly defined (has no context), then it can be excluded.
However, extension classes without a defined context will instead be placed into the ActivityStreams context, which will apply them to
**all** objects.

See the [ActivityStreams documentation](https://www.w3.org/TR/activitystreams-core/#extensibility) for more information on extensibility.

## Limitations

* Several properties and edge cases are not correctly handled. See the [issue tracker](https://github.com/warriordog/ActivityPubSharp/issues) for details.
* Only a minimal subset of JSON-LD is supported. For full compatibility, it is necessary to override the default [JsonLdSerializer](Conversion/JsonLdSerializer.cs) and pre-process the incoming JSON.

## Available Options

| Options Class                                                    | Configuration Path          | Description                                                                |
|------------------------------------------------------------------|-----------------------------|----------------------------------------------------------------------------|
| [JsonLdSerializerOptions](Conversion/JsonLdSerializerOptions.cs) | N/A - not mapped by default | Options for the default [JsonLdSerializer](Conversion/JsonLdSerializer.cs) |
