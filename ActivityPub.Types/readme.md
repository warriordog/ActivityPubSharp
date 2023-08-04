# ActivityPub.Types

Models and serialization logic for ActivityPub types.
Out-of-the-box, supports the base and extended ActivityStreams types including collections.
Supports simple JSON-LD in the compacted form.
Provides a [pre-configured serializer](Json/JsonLdSerializer.cs) to convert between models and JSON.

## Usage

This package is loaded automatically by higher-level packages.
For standalone usage, call [`TryAddTypesModule`](TypesModule.cs) to register all services for DI.

## Synthetic Types

To improve developer experience, a few synthetic intermediate types have been introduced to the hierarchy.
These are as follows:

* [`ASType`](ASType.cs) - a common type between [`Link`](ASLink.cs) and [`Object`](ASObject.cs). This enables support for the `Range: Link | Object` construction that is common within the ActivityStreams specification.
* [`ASActor`](ASActor.cs) - base type for standard actor types. Technically, any object can be an actor if it supports the correct properties. To extend a non-actor type with actor functionality, implement `IActor` instead of `ASActor`.
* [`ASTransitiveActivity`](ASTransitiveActivity.cs) - a mirror to [`IntransitiveActivity`](ASIntransitiveActivity.cs) that implies just the opposite. Transitive activities support the [`object` property](https://www.w3.org/TR/activitystreams-vocabulary/#dfn-object).
* [`ASTargetedActivity`](ASTargetedActivity.cs) - extension of `TransitiveActivity` for activities which contain a [`target` property](https://www.w3.org/TR/activitystreams-vocabulary/#dfn-target).

## Utility Types

These types are implemented to support certain special-case properties.
Most exist for DX only, although a few are needed for type-model accuracy or serialization purposes.

* [`ASUri`](Util/ASUri.cs) - (will) implement the namespaced IRI concept used in JSON-LD. For now, just a wrapper around `Uri`
* [`JsonLDContext`](Util/JsonLDContext.cs) - encapsulates the `object | IRI` typing used in the JSON-LD context. Does not parse or enforce IRI correctness - its just a `string`.
* [`JsonLDTerm`](Util/JsonLDTerm.cs) - encapsulates the `object | IRI` typing used in JSON-LD term definitions. Does not parse or enforce IRI correctness - its just a `string`.
* [`Linkable<T>`](Util/Linkable.cs) - encapsulates the `Range: Link | T` construction used in ActivityStreams. *
  *Caution: this may be removed pending [#23](https://github.com/warriordog/ActivityPubSharp/issues/23).**
* [`LinkableList<T>`](Util/LinkableList.cs) - encapsulates the `Range: (Link | T)[]` construction used in ActivityStreams, as well as the extra cursed `Link | T | (Link | T)[]` variant.
  **Caution: this may be removed pending [#23](https://github.com/warriordog/ActivityPubSharp/issues/23).**
* [`NaturalLanguageString`](Util/NaturalLanguageString.cs) - supports language-tag mapped strings. The current implementation is subpar and will be replaced in [#12](https://github.com/warriordog/ActivityPubSharp/issues/12).

## Extensibility

Partial support for ActivityPub extensions is available.
At startup, the module scans all loaded assemblies and registers any type deriving from [ASType](ASType.cs) and containing an [ASTypeKey](Json/ASTypeKeyAttribute.cs) attribute.
Registered extension classes will be automatically constructed by the [polymorphic converter](Json/ASTypeConverter.cs), but all custom types are responsible for adding their own context to `JsonLDContexts` in the constructor.
Generic types ARE supported, but they must specify a generic type constraint to resolve ambiguous cases.
`ASType` is a safe default constraint.
See the [ActivityStreams documentation](https://www.w3.org/TR/activitystreams-core/#extensibility) for more information on extensibility.

## Limitations

* Currently, extensibility is limited to custom types. There is no support for new/additional properties and it is impossible to correctly convert an object extending multiple types.
* Several properties and edge cases are not correctly handled. See the [issue tracker](https://github.com/warriordog/ActivityPubSharp/issues) for details.