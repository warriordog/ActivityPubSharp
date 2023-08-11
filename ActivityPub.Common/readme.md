# ActivityPub.Common

Contains types and logic needed by multiple ActivityPubSharp packages.
Unlike [internal projects](../Internal), this module is available to user code.
It is loaded implicitly by most other packages, but you can initialize it manually through [CommonModule](CommonModule.cs).
All important services and configuration classes are bound through DI.

This module does *not* publish a package.
Instead, its included as a dependency by other packages that need it.

## Available Options

| Options Class                                    | Configuration Path    | Description                                       |
|--------------------------------------------------|-----------------------|---------------------------------------------------|
| [ActivityPubOptions](Util/ActivityPubOptions.cs) | TBD - not implemented | Common, general-purpose ActivityPub configuration |
