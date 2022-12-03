# Progress of ActivityPubSharp

---

This document details the project progress.
Below are lists of the status and compatibility of various components.
There is no particular ordering.

TODO - fill this out

###Project Structure
- [ ] Match projects to exposed packages
  - [ ] Rename "Common" to "Types"
- [ ] Create NuGet package definitions
- [ ] OSS License

###Types
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
- [ ] Extensibility - custom properties

###Serialization
- [ ] Parser
  - [ ] Check for ActivityStreams context
- [ ] Serializer
  - [ ] Attach ActivityStreams context
- [ ] Allow other contexts to be provided
- [ ] Validate object graphs
- [ ] Map extra properties


Still unsure of the. The ideas are:
1. Use JSON attributes only (probably infeasible) (json-attributes branch)
2. Use JSON attributes + custom resolver (current approach) (json-attributes branch is moving this way)
3. Use custom resolver only (would allow no-dependencies for Types, might eliminate some of the ugly JSON converters) (serialization branch but that one is going away)
4. Use entirely custom code (probably need an intermediate type for JSON parsing)
5. Use JSON-LD (doesn't solve other issues, wildly complex) (json-ld branch)
6. 
###Client

###Server

###Federation

###Documentation

###Tests