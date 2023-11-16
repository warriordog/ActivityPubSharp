# InternalUtils

This package contains shared utility classes, internal to ActivityPubSharp.
They are accessible in any package but NOT by user code.
New packages will not have access by default.
To enable this, you must do two things:

1. Add a dependency on `InternalUtils`
2. Add the new package name in [`InternalUtilsModule.cs`](InternalUtilsModule.cs). You do NOT have to reference this file in code!