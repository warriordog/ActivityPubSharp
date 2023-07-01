// See https://aka.ms/new-console-template for more information

using ActivityPub.Client;
using ActivityPub.Common.Util;
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
builder.Services.PostConfigure<ActivityPubOptions>(apo => apo.SerializerOptions.WriteIndented = true);

// Start host
using var host = builder.Build();
await host.RunAsync();