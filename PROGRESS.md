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
- [ ] ActivityStreams Types
  - [ ] Core Types
  - [ ] Extended Types
    - [ ] Activity Types
    - [ ] Actor Types
    - [ ] Object Types
    - [ ] Link Types
- [ ] ActivityPub Types
- [ ] Synthetic Types
  - [x] ASType
  - [ ] TransitiveActivity

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
- [ ] Use real JSON-LD

Documentation

Tests