# ActivityPub.Types.Tests

Tests for the types package.

* Unit tests are in the [Unit](Unit) folder, organized to match the class hierarchy of the code being testing.
* Integration tests are in the [Integration](Integration) folder, and are organized based on functionality under test.
* Smoke tests, finally, are in the [Smoke](Smoke) folder. They are intended to execute complex synthetic or captured real-world data to "smoke out" any potential (de)serialization bugs.

Non-test utility classes, such as custom assertions and test fixtures, are located in the [Util](Util) folder.
Don't be afraid to add new utility types if needed.
The [`JsonElementAssertions` class](Util/Assertions/JsonElementAssertions.cs), in particular, can be extended to greatly simplify JSON tests.