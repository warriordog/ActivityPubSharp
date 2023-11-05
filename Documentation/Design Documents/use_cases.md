# Use Cases

Business logic within ActivityPubSharp is based on a collection of small, atomic "use cases" that each implement a high-level function.
Two types of use cases exist: **high-level** and **low-level**.
High-level use cases look much like you would expect.
They are highly abstracted from the library internals and represent a distinct function that user may wish to perform.

Low-level use cases, on the other hand, deviate from the typical pattern.
They provide thin abstractions over simple routines and functions that would normally be implemented as services or mappers.
We take this unusual approach because ActivityPubSharp is first and foremost a library, with a distinct expectation of being wrapped by even higher level business logic or 3rd party integrations.
Other patterns are too rigid and would force all library users to adopt the same design, which may not even be possible.
To ease usage and integration, we adopt **library users** as an additional first-class user type alongside the *actual* end users.
High-level cases are for end users, low-level cases are for library users.

## Traits of a Use Case

All use cases, low or high, share the following traits:
* Stateless
* Have a public interface and separate implementation class
* Injected through DI as singletons
* Self-contained, but can compose over other use cases through DI
* Accept a dedicated options type, which exposes static configuration and extension hooks

This provides a good balance between flexibility and ease of use, while also promoting clean code and maintainability.
Additionally, this assists with unit testing and can be easily integrated into a variety of existing application architectures.


## See Also

* [Use Case List](<../Dev Resources/use_case_list.md>) - index of Use Cases available in the library