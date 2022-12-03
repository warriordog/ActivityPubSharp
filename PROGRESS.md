# Progress of ActivityPubSharp

This document details the project progress.
Below are lists of the status and compatibility of various components.
There is no particular ordering.

TODO - fill this out

### Project Structure
- [ ] Match projects to exposed packages
  - [ ] Rename "Common" to "Types"
- [ ] Create NuGet package definitions
- [ ] OSS License

### Packages
#### Types
- [x] ActivityStreams Types
  - [x] Core Types
  - [x] Extended Types
    - [x] Activity Types
    - [x] Actor Types
    - [x] Object Types
    - [x] Link Types
- [ ] Synthetic / Utility Types
  - [x] ASType
  - [x] TransitiveActivity
  - [x] NaturalLanguageString
  - [x] ASActor
  - [x] ICollectionPage
  - [ ] IRI
- [x] Casts and operators
- [x] Allow other contexts to be provided
- [ ] Extensibility - custom properties

#### Types.Proposed

#### Types.Mastodon

#### Types.W3ID

#### Types.Schema

#### Json
- [ ] ActivityPubContractResolver (this will be the big one)
  - [ ] Check for / attach ActivityStreams context
  - [ ] Map names using contexts (MAYBE - this is getting into JSON-LD territory)
  - [ ] Validate object graphs
  - [ ] Promote all contexts to root
- [ ] Parser class
- [ ] Serializer class
- [ ] Extensibility points
  - [ ] Resolver logic
  - [ ] Parser overrides
  - [ ] Serializer overrides
- [ ] Unit tests
  - [ ] Resolver (oh god the sheer scope of this... its going to be thousands of tests)
  - [ ] Parser
  - [ ] Serializer


Still unsure of the. The ideas are:
1. Use JSON attributes only (probably infeasible) (json-attributes branch)
2. Use JSON attributes + custom resolver (json-attributes branch is moving this way)
3. Use custom resolver only (new approach, would allow no-dependencies for Types, might eliminate some of the ugly JSON converters) (json-custom-resolver branch)
   * This involves creating a single giant class that encapsulates all of the ActivityPub, ActivityStreams, and JSON-LD logic and interfaces with JSON.NET
4. Use entirely custom code (probably need an intermediate type for JSON parsing)
5. Use JSON-LD (doesn't solve other issues, wildly complex) (json-ld branch)

#### JsonLD
- [ ] Decide if we ever want to support this
- [ ] Build as extension of Json package using extensibility points

#### Auth
- [ ] Dependency on Types.W3ID

#### Client

#### Server

#### Federation

#### Documentation