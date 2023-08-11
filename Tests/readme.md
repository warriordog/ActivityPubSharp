# Test Projects

These projects contain automated unit, integration, and smoke testing classes.

## Test Standards

* [xUnit](https://xunit.net/) is used as the test runner.
* [Fluent Assertions](https://fluentassertions.com) is available for testing support.
* Please do *not* add any mocking library - we prefer fakes over mocks for more explicit testing.
* To group tests, use abstract nested classes. See existing tests as an example.

## Test Projects

| ActivityPubSharp Package                  | Testing Project                                    |
|-------------------------------------------|----------------------------------------------------|
| [ActivityPub.Types](../ActivityPub.Types) | [ActivityPub.Types.Tests](ActivityPub.Types.Tests) |
