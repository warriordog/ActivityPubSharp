# Progress of ActivityPubSharp

---

This document details the project progress.
Below are lists of the status and compatibility of various components.
There is no particular ordering.

TODO - fill this out

Project Structure
- [ ] Match projects to exposed packages
  - [ ] Rename "Common" to "Types"
- [ ] Create NuGet package definitions
- [ ] OSS License

Types
- [x] ActivityStreams Types
  - [x] Core Types
  - [x] Extended Types
    - [x] Activity Types
    - [x] Actor Types
    - [x] Object Types
    - [x] Link Types
- [ ] ActivityPub Types
- [ ] Synthetic / Utility Types
  - [x] ASType
  - [x] TransitiveActivity
  - [x] NaturalLanguageString
  - [ ] IRI
- [ ] Casts and operators
- [ ] Extensibility - custom properties
- [ ] Validators

JSON-LD
- [ ] Basic implementation
  - [ ] Context mapping
  - [ ] Parser
  - [ ] Serializer
- [ ] Full implementation

Serialization
- [ ] Simple implementation with basic JSON
- [ ] Parser
  - [ ] Check for ActivityPub context
- [ ] Serializer
  - [ ] Attach ActivityPub context
- [ ] Validate object graphs
- [ ] Use JSON-LD

Documentation

Tests