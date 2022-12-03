# ActivityPubSharp - C# implementation of ActivityPub

---

*Please note - this project is incomplete and not ready for production use. The information here describes the design and technical goals of ActivityPubSharp, not the currently implemented functionality. Please see [PROGRESS.md](PROGRESS.md) for details about what is and is not currently supported.*

## About
ActivityPubSharp is a work-in-progress set of packages to support the use of ActivityPub in .NET applications.

### Focus
As the name suggests, ActivityPubSharp is focused on supporting ActivityPub rather than ActivityStreams or any other related standard.
If there is ever a conflict between standards, then ActivityPub will be followed.

Currently, ActivityPubSharp does not attempt to offer proper JSON-LD support.
The provided serialization code implements a minimum-viable approach that should most use cases.

### Abstraction
A key goal of this project is abstraction - that is, it should be relatively simple to integrate this library at any level of abstraction.
To support this, a variety of NuGet packages are produced at increasing levels of abstraction.
The main packages are as follows:


| Package                         | Description                                                                                                                                                                     | Use Case                                                                                                         |
|---------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| ActivityPubSharp.Types          | Serializable type definitions for ActivityPub types. Includes utility classes to handle polymorphic properties.                                                                 | You only need the ActivityPub object types, and nothing else.                                                    |
| ActivityPubSharp.Types.Proposed | Extended type definitions for proposed extensions to ActivityStreams.                                                                                                           | You need to use the unreleased ActivityStreams extensions.                                                       |
| ActivityPubSharp.Types.Mastodon | Extended type definitions for integration with Mastodon.                                                                                                                        | You are communicating with a Mastodon server.                                                                    |
| ActivityPubSharp.Types.W3ID     | Extended type definitions for W3ID Security. This only includes the types - actual security logic is handled by ActivityPubSharp.Auth.                                          | You need to secure communications using W3ID security.                                                           |
| ActivityPubSharp.Types.Schema   | Extended type definitions for schema.org.                                                                                                                                       | You integrate with an application that expects a schema, or you want to use a related technology like Microdata. |
| ActivityPubSharp.Json           | Serializer and parser for ActivityPub objects based on JSON.NET. Supports only the minimum of JSON-LD needed to implement ActivityPub.                                          | You need to read and/or write ActivityPub objects as JSON, but do not need further logic.                        |
| ActivityPubSharp.Auth           | Authentication schemes used by common ActivityPub implementations.                                                                                                              | You will communicate with an external host which requires signatures or other authentication.                    |
| ActivityPubSharp.Client         | Implementation of the Client conformance class. Supports the client side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions).  | You are implementing (at least) the Client conformance class.                                                    |
| ActivityPubSharp.Server         | Implementation of the Server conformance class. Supports the server side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions).  | You are implementing (at least) the Server conformance class.                                                    |
| ActivityPubSharp.Federation     | Implementation of the Federated Server conformance class. Supports [Server to Server interactions](https://www.w3.org/TR/activitypub/#server-to-server-interactions).           | You are implementing (at least) the Federated Server conformance class.                                          |

It is possible to use any combination of the Client, Server, and Federation packages simultaneously.

### Modularity
ActivityPubSharp is designed in a modular fashion intended to support unique or extended use cases.
For example, parsing code is abstracted to support use with any JSON parser, not just the included one based on Newtonsoft.JSON.
The packages within ActivityPubSharp are linked via interfaces and assembled through DI, allowing them to be easily extended by user code.

## Technical Details

### Requirements
* .NET 7 or newer runtime on any supported platform

### Design Goals
* Minimal dependencies - software supply chain risks are real and very underrated. ActivityPubSharp is developed using minimal external dependencies, but without reinventing all the wheels.
* Modern .NET - ActivityPubSharp has set .NET 7 as the minimum supported version. This does unfortunately exclude many projects, but it will be beneficial in the long-run. Net7.0 will be relevant for much longer than, say NetStandard2.0 or even 2.1.