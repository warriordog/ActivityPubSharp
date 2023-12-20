# ActivityPubSharp - C# implementation of ActivityPub

[![Build Status](https://github.com/warriordog/ActivityPubSharp/actions/workflows/build-dotnet.yml/badge.svg)](https://github.com/warriordog/ActivityPubSharp/actions/workflows/build-dotnet.yml)
[![NuGet Snapshot](https://github.com/warriordog/ActivityPubSharp/actions/workflows/publish-snapshot.yml/badge.svg)](https://github.com/warriordog/ActivityPubSharp/actions/workflows/publish-snapshot.yml)

❗❗ **Please note - this project is incomplete and not ready for production use.
The information here describes the design and technical goals of ActivityPubSharp, not the currently implemented functionality.
Please see [the issues tracker](https://github.com/warriordog/ActivityPubSharp/issues) for detailed status.**

## About
ActivityPubSharp is a toolkit of modular packages that support the use of ActivityPub in .NET applications.
Low-level APIs offer raw - but safe - access to strongly-typed models, while high-level interfaces support ergonomic and standards-compliant use of the protocol.
Special utility types and integrated JSON-LD converters further abstract ActivityPub's rougher edges, bringing the user experience up to par with more traditional APIs.
For more information, see the introduction post - [*What is ActivityPubSharp?*](https://github.com/warriordog/ActivityPubSharp/discussions/63)

### Modularity
Modularity and Abstraction are keys goals of this project.
It should be simple to integrate this library at any appropriate level of abstraction.
Low-level apps can utilize the [strongly-typed models](Source/ActivityPub.Types) and utility types, but bring their own parser, logic, and other components.
Mid-level apps may want to use the [HTTP client](Source/ActivityPub.Client) or [abstract server logic](Source/ActivityPub.Server).
On the other hand, high-level apps will likely desire framework integration and custom middleware.

To support these varying use cases, ActivityPubSharp is split into multiple separate NuGet packages.
While separate and self-contained, these packages are designed to work together and will seamlessly integrate through Dependency Injection.
A typical use case will include the highest-level "main" package needed for the application, and then as many "secondary" packages as desired for extra functionality.

| Package                        | Links                                                                                                                   | Description                                                                                                                                                                    | Use Case                                                                       |
|--------------------------------|-------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------|
| ActivityPub.Types              | [NuGet](https://www.nuget.org/packages/ActivityPub.Types), [Source](Source/ActivityPub.Types)                           | Serializable type definitions for ActivityPub types. Includes utility classes to handle polymorphic properties.                                                                | You only need the ActivityPub object types, and nothing else.                  |
| ActivityPub.Common             | [NuGet](https://www.nuget.org/packages/ActivityPub.Common), [Source](Source/ActivityPub.Common)                         | Core utilities, services, and configuration for all ActivityPub use cases.                                                                                                     | You are using any other part of this library.                                  |
| ActivityPub.Client             | [NuGet](https://www.nuget.org/packages/ActivityPub.Client), [Source](Source/ActivityPub.Client)                         | Implementation of the Client conformance class. Supports the client side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing the Client conformance class.                             |
| ActivityPub.Server             | [NuGet](https://www.nuget.org/packages/ActivityPub.Server), [Source](Source/ActivityPub.Server)                         | Implementation of the Server conformance class. Supports the server side of [Client to Server interactions](https://www.w3.org/TR/activitypub/#client-to-server-interactions). | You are implementing the Server conformance class, with or without Federation. |
| ActivityPub.Server.AspNetCore  | [NuGet](https://www.nuget.org/packages/ActivityPub.Server.AspNetCore), [Source](Source/ActivityPub.Server.AspNetCore)   | Extends Server package with bindings and middleware for ASP.NET Core.                                                                                                          | You are implementing a server that will utilize ASP.NET.                       |
| ActivityPub.Extension.Mastodon | [NuGet](https://www.nuget.org/packages/ActivityPub.Extension.Mastodon), [Source](Source/ActivityPub.Extension.Mastodon) | Extended type definitions and logic for integration with Mastodon.                                                                                                             | You are communicating with a Mastodon server.                                  |

### Non-goals
As the name suggests, ActivityPubSharp is focused specifically on ActivityPub rather than ActivityStreams, JSON-LD, or any other relational data standard.
While ActivityStreams *is* implemented as an underlying component of ActivityPub, it cannot be used standalone with this library.
The same is true of JSON-LD.
If there is ever a conflict between specifications, then the [ActivityPub spec](https://www.w3.org/TR/activitypub/) spec will be considered authoritative.

At present, ActivityPubSharp intends to offer only a subset of JSON-LD support.
The included serializers support all features that are necessary for ActivityPub and integration with mainstream fediverse software, but that is all.
Attempting to parse other forms of linked data may fail catastrophically.
In the future, it will be possible to inject a user-provided compaction/normalization layer through DI.
Advanced applications can use this feature to support JSON-LD while still using ActivityPubSharp's high-level APIs.

### Customization
ActivityPubSharp utilizes Dependency Injection to customize and extend the library.
For example, included services all utilize the [`IJsonLdSerializer` interface](Source/ActivityPub.Types/Conversion/JsonLdSerializer.cs) which enables user code to replace or modify the default JSON support.
Most default implementations expose configuration objects that are compatible with the Options Pattern.
User-accessible DI services and and options are described in the README for each individual package.

## Getting Started

ActivityPubSharp is not yet production-ready.
Most high-level functionality is not implemented.
There is, however, [an example package](Example) available with reference code.

## Technical Details

### Requirements
* .NET 7 or newer
* Any supported platform - no native code is used

### Design Goals
* Minimal dependencies - software supply chain risks are an underappreciated threat. ActivityPubSharp keeps minimal dependencies to reduce the supply chain footprint.
* Modern .NET - ActivityPubSharp has set .NET 7 as the minimum supported version. This unfortunately excludes some projects, but is critical for System.Text.Json support.
* Ergonomic and Conventional APIs - mid/high level APIs should feel and function like typical .NET APIs. An important goal (and difficult challenge) of this project is abstracting ActivityPub's dynamic nature to better fit with C#'s strongly-typed model.
