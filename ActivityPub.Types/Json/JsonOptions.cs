// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace ActivityPub.Types.Json;

// TODO see if we can remove this
public class JsonOptions
{
    public JsonSerializerOptions SerializerOptions { get; }
    public JsonReaderOptions ReaderOptions { get; }
    public JsonWriterOptions WriterOptions { get; }
    public ASTypeRegistry TypeRegistry { get; }
    public JsonNodeOptions NodeOptions { get; }

    public JsonOptions(JsonSerializerOptions options, ASTypeRegistry typeRegistry)
    {
        TypeRegistry = typeRegistry;
        SerializerOptions = options;
        ReaderOptions = new JsonReaderOptions
        {
            MaxDepth = options.MaxDepth,
            AllowTrailingCommas = options.AllowTrailingCommas,
            CommentHandling = options.ReadCommentHandling
        };
        WriterOptions = new JsonWriterOptions
        {
            MaxDepth = options.MaxDepth,
            Encoder = options.Encoder,
            Indented = options.WriteIndented
        };
        NodeOptions = new JsonNodeOptions
        {
            PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive
        };
    }
}