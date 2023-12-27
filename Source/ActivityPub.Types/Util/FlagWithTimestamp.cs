// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.Conversion.Converters;

namespace ActivityPub.Types.Util;

/// <summary>
///     A value that can be a <see langword="boolean"/> flag or a <see cref="DateTime"/> timestamp.
///     For example, <see cref="ActivityPub.Types.AS.Extended.Activity.QuestionActivity.Closed"/>. 
/// </summary>
[JsonConverter(typeof(FlagWithTimestampConverter))]
public class FlagWithTimestamp
{
    /// <summary>
    ///     The <see langword="boolean"/> value of this flag.
    ///     Will be true if <see cref="Timestamp"/> has a value.
    ///     If set to false, then <see cref="Timestamp"/> will also be set to <see langword="null"/>. 
    /// </summary>
    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            if (!value)
                _timestamp = null;
        }
    }

    /// <summary>
    ///     The timestamp of this flag, is present.
    ///     If set to a value, then <see cref="Timestamp"/> will automatically change to <see langword="true"/>.
    ///     If set to <see langword="null"/>, then <see cref="Timestamp"/> will automatically change to <see langword="false"/>.
    /// </summary>
    public DateTime? Timestamp
    {
        get => _timestamp;
        set
        {
            _value = value != null;
            _timestamp = value;
        }
    }

    private bool _value;
    private DateTime? _timestamp;

    /// <summary>
    ///     Returns the value of <see cref="Value"/>.
    /// </summary>
    public static implicit operator bool(FlagWithTimestamp flag) => flag.Value;
    
    /// <summary>
    ///     Returns the value of <see cref="Timestamp"/>;
    /// </summary>
    public static implicit operator DateTime?(FlagWithTimestamp flag) => flag.Timestamp;

    /// <summary>
    ///     Creates a flag with the specified value for <see cref="Value"/>.
    /// </summary>
    public static implicit operator FlagWithTimestamp(bool value) => new() { Value = value };
    
    /// <summary>
    ///     Creates a flag with the specified value for <see cref="Timestamp"/>.
    /// </summary>
    public static implicit operator FlagWithTimestamp(DateTime timestamp) => new() { Timestamp = timestamp };
}