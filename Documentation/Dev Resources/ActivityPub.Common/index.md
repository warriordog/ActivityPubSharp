# ActivityPub.Common

Contains types and logic needed by multiple ActivityPubSharp packages.
Unlike [the internal utilities](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/InternalUtils), this module is available to user code.
It is loaded implicitly by most other packages, but you can initialize it manually through [CommonModule](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Common/CommonModule.cs).
All important services and configuration classes are bound through DI.

This module does *not* publish a package.
Instead, its included as a dependency by other packages that need it.

## Available Options

| Options Class                                    | Configuration Path    | Description                                       |
|--------------------------------------------------|-----------------------|---------------------------------------------------|
| [ActivityPubOptions](https://github.com/warriordog/ActivityPubSharp/blob/main/Source/ActivityPub.Common/Util/ActivityPubOptions.cs) | TBD - not implemented | Common, general-purpose ActivityPub configuration |