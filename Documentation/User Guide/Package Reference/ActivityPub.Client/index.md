# ActivityPub.Client

This package implements logic and services necessary to implement an ActivityPub client.
[Everything is available through DI](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Client/ClientModule.cs) for simple, abstract usage.

## Available Services

| Service                                       | Implementation                                 | Description                                                                                   |
|-----------------------------------------------|------------------------------------------------|-----------------------------------------------------------------------------------------------|
| [`IActivityPubClient`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Client/IActivityPubClient.cs) | [`ActivityPubClient.cs`](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Client/ActivityPubClient.cs) | Implements an HTTP client that automatically parses JSON-LD and constructs ActivityPub models |
