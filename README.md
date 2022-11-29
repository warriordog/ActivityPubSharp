# ActivityPubSharp - C# implementation of ActivityPub

---

*Please note - this project is incomplete and not ready for production use. The information here describes the design and technical goals of ActivityPubSharp, not the currently implemented functionality. Please see [PROGRESS.md](PROGRESS.md) for details about what is and is not currently supported.*

## About
ActivityPubSharp is a work-in-progress set of packages to support the use of ActivityPub in .NET applications.

### Focus
As the name suggests, ActivityPubSharp is focused on supporting ActivityPub rather than ActivityStreams or any other related standard.
If there is ever a conflict between standards, then ActivityPub will be followed.

Currently, ActivityPubSharp does not attempt to support JSON-LD.
Additional contexts and other JSON-LD operators will be ignored.
This may change in the future.

### Abstraction
A key goal of this project is abstraction - that is, it should be relatively simple to integrate this library at any level of abstraction.
To support this, a variety of NuGet packages are produced at increasing levels of abstraction.
The main packages are as follows:


| Package                        | Description                                                                                                                                                                    | Use Case                                                                                                                                                                   |
|--------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ActivityPubSharp.Types         | POCO type definitions for ActivityPub types. Includes utility classes to handle polymorphic properties.                                                                        | You only need the ActivityPub object types, and nothing else.                                                                                                              |
| ActivityPubSharp.Serialization | Parser and Serializer based on Newtonsoft.JSON. Supports any ActivityPub type, but JSON-LD is NOT supported.                                                                   | You have your own implementation of an [ActivityPub conformance class](https://www.w3.org/TR/activitypub/#conformance), but you don't want to write your own parsing code. |
| ActivityPubSharp.Auth          | Authentication schemes used by common ActivityPub implementations.                                                                                                             | You will communicate with an external host which requires signatures or other authentication.                                                                              |
| ActivityPubSharp.Client        | Implementation of the Client conformance class. Supports the client side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Client conformance class.                                                                                                              |
| ActivityPubSharp.Server        | Implementation of the Server conformance class. Supports the server side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Server conformance class.                                                                                                              |
| ActivityPubSharp.Federation    | Implementation of the Federated Server conformance class. Supports [Server to Server interactions](https://www.w3.org/TR/activitypub/#server-to-server-interactions).          | You are implementing (at least) the Federated Server conformance class.                                                                                                    |

It is possible to use any combination of the Client, Server, and Federation packages simultaneously.

### Modularity
ActivityPubSharp is designed in a modular fashion intended to support unique or extended use cases.
For example, parsing code is abstracted to support use with any JSON parser, not just the included one based on Newtonsoft.JSON.
The packages within ActivityPubSharp are linked via interfaces and assembled through DI, allowing them to be easily extended by user code.