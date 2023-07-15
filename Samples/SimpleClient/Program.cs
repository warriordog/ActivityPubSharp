// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

// See https://aka.ms/new-console-template for more information

using ActivityPub.Client;
using ActivityPub.Types.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleClient;

var builder = Host.CreateApplicationBuilder(args);

// Fuck right off with those lifecycle messages
builder.Logging.ClearProviders();

// Register ActivityPubSharp modules
builder.TryAddClientModule();

// Add services
builder.Services.AddHostedService<ConsoleService>();

// Enable pretty-printing
builder.Services.Configure<IJsonLdSerializer>(serializer => serializer.SerializerOptions.WriteIndented = true);

// Start host
using var host = builder.Build();
await host.RunAsync();