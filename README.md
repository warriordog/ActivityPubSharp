# ActivityPubSharp - C# implementation of ActivityPub

---

*Please note - this project is incomplete and not ready for production use. The information here describes the design and technical goals of ActivityPubSharp, not the currently implemented functionality. Please see PROGRESS.md for details about what is and is not currently supported.*

## About
ActivityPubSharp is a work-in-progress set of packages to support the use of ActivityPub in .NET applications.

### Abstraction
A key goal of this project is abstraction - that is, it should be relatively simple to integrate this library at any level of abstraction.
To support this, a variety of NuGet packages are produced at increasing levels of abstraction.
The main packages are as follows:


| Package                        | Description                                                                                                                                                                    | Use Case                                                                                                                                                                   |
|--------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ActivityPubSharp.Types         | Type definitions for ActivityPub types. Includes utility classes to handle polymorphic properties.                                                                             | You only need the ActivityPub or ActivityStreams types, and nothing else.                                                                                                  |
| ActivityPubSharp.JsonLd        | Parser and Serializer for JSON-LD based on Newtonsoft.JSON. Can be used without ActivityPub.                                                                                   | You are using JSON-LD without ActivityPub or ActivityStreams semantics.                                                                                                    |
| ActivityPubSharp.Serialization | Extension of JsonLd package with support for ActivityPub context and semantics.                                                                                                | You have your own implementation of an [ActivityPub conformance class](https://www.w3.org/TR/activitypub/#conformance), but you don't want to write your own parsing code. |
| ActivityPubSharp.Client        | Implementation of the Client conformance class. Supports the client side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Client conformance class.                                                                                                              |
| ActivityPubSharp.Server        | Implementation of the Server conformance class. Supports the server side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing (at least) the Server conformance class.                                                                                                              |
| ActivityPubSharp.Federation    | Implementation of the Federated Server conformance class. Supports [Server to Server interactions](https://www.w3.org/TR/activitypub/#server-to-server-interactions).          | You are implementing (at least) the Federated Server conformance class.                                                                                                    |

It is possible to use any combination of the Client, Server, and Federation packages simultaneously.