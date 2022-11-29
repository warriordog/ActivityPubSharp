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
- [ ] Synthetic / Utility Types
  - [x] ASType
  - [x] TransitiveActivity
  - [x] NaturalLanguageString
  - [x] ASActor
  - [x] ICollectionPage
  - [ ] IRI
- [x] Casts and operators
- [ ] Extensibility - custom properties

Serialization
- [ ] Parser
  - [ ] Check for ActivityStreams context
- [ ] Serializer
  - [ ] Attach ActivityStreams context
- [ ] Allow other contexts to be provided
- [ ] Validate object graphs
- [ ] Map extra properties

Client

Server

Federation

Documentation

Tests