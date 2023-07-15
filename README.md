# ActivityPubSharp - C# implementation of ActivityPub

---

**Please note - this project is incomplete and not ready for production use.
The information here describes the design and technical goals of ActivityPubSharp, not the currently implemented functionality.
Please see [the issues tracker](https://github.com/warriordog/ActivityPubSharp/issues) for detailed status.**

## About
ActivityPubSharp is a work-in-progress toolkit to support the use of ActivityPub in .NET applications.
Multiple packages are available to fit any use case or application.

### Focus
As the name suggests, ActivityPubSharp is focused on supporting ActivityPub rather than raw ActivityStreams or any other related standard.
If there is ever a conflict between standards, then the ActivityPub spec will be followed.

At present, ActivityPubSharp does not intend to offer full JSON-LD support.
The provided serialization code implements a minimum-viable approach that should support most use cases.
At future, it will be possible to inject a user-provided compaction/normalization layer through DI.

### Abstraction
Abstraction is a key goal of this project.
The intent is that it should be relatively simple to integrate this library at any appropriate level of abstraction.
A client-only app doesn't need tons of server-related code, and a server app shouldn't be bound to any particular web framework.
To support this, a variety of NuGet packages are produced at increasing levels of abstraction.
The main packages are as follows:


| Package                                               | Description                                                                                                                                                                    | Use Case                                                                                                                      |
|-------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------|
| [ActivityPubSharp.Types](ActivityPub.Types)           | Serializable type definitions for ActivityPub types. Includes utility classes to handle polymorphic properties.                                                                | You only need the ActivityPub object types, and nothing else.                                                                 |
| ActivityPubSharp.Types.Proposed                       | Extended type definitions for proposed extensions to ActivityStreams.                                                                                                          | You need to use the unreleased ActivityStreams extensions.                                                                    |
| ActivityPubSharp.Types.Mastodon                       | Extended type definitions for integration with Mastodon.                                                                                                                       | You are communicating with a Mastodon server.                                                                                 |
| ActivityPubSharp.Types.W3ID                           | Extended type definitions for W3ID Security. This only includes the types - actual security logic is handled by ActivityPubSharp.Auth.                                         | You need to secure communications using W3ID security.                                                                        |
| ActivityPubSharp.Types.Schema                         | Extended type definitions for schema.org.                                                                                                                                      | You integrate with an application that expects a schema, or you want to use a related technology like Microdata.              |
| ActivityPubSharp.Auth                                 | Authentication schemes used by common ActivityPub implementations.                                                                                                             | You will communicate with an external host which requires signatures or other authentication.                                 |
| [ActivityPubSharp.Client](ActivityPub.Client)         | Implementation of the Client conformance class. Supports the client side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Client conformance class.                                                                 |
| [ActivityPubSharp.Server](ActivityPub.Server)         | Implementation of the Server conformance class. Supports the server side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Server conformance class.                                                                 |
| [ActivityPubSharp.Federation](ActivityPub.Federation) | Implementation of the Federated Server conformance class. Supports [Server to Server interactions](https://www.w3.org/TR/activitypub/#server-to-server-interactions).          | You are implementing (at least) the Federated Server conformance class.                                                       |
| ActivityPubSharp.AspNetCore                           | Extends Server package with bindings and middleware for ASP.NET Core.                                                                                                          | You are implementing a server that will utilize ASP.NET.                                                                      |
| ActivityPubSharp                                      | Meta-package that includes all others.                                                                                                                                         | You want ActivityPub with minimal effort, and can accept the library authors' default implementations and 3rd-party bindings. |

It is possible to use multiple of the Client, Server, and Federation packages simultaneously.

### Modularity
ActivityPubSharp utilizes Dependency Injection to easily support unique or extended use cases.
For example, included services all utilize the [`IJsonLdSerializer` interface](ActivityPub.Types/Json/JsonLdSerializer.cs) which enables user code to replace or modify the default JSON support.
User-accessible DI services and and options are described in the README for each individual package.

## Technical Details

### Requirements
* .NET 7 (newer should work, but is untested)
* Any platform - no native code is used

### Design Goals
* Minimal dependencies - software supply chain risks are real and very underrated. ActivityPubSharp is developed using minimal external dependencies, but without reinventing all the wheels.
* Modern .NET - ActivityPubSharp has set .NET 7 as the minimum supported version. This unfortunately excludes some projects, but is critical for System.Text.Json support.