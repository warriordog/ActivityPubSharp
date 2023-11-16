# ActivityPub.Client

This package implements logic and services necessary to implement an ActivityPub client.
[Everything is available through DI](ClientModule.cs) for simple, abstract usage.

## Available Services

| Service                                       | Implementation                                 | Description                                                                                   |
|-----------------------------------------------|------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [`IActivityPubClient`](IActivityPubClient.cs) | [`ActivityPubClient.cs`](ActivityPubClient.cs) | Implements an HTTP client that automatically parses JSON-LD and constructs ActivityPub models |
