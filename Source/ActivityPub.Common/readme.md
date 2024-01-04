# ActivityPub.Common

Contains types and logic needed by multiple ActivityPubSharp packages.
Unlike [the internal utilities](../InternalUtils), this module is available to user code.
It is loaded implicitly by most other packages, but you can initialize it manually through [CommonModule](CommonModule.cs).
All important services and configuration classes are bound through DI.

This module does *not* publish a package.
Instead, its included as a dependency by other packages that need it.

[Package Reference](https://warriordog.github.io/ActivityPubSharp/User%20Guide/Package%20Reference/ActivityPub.Common)
