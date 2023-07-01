# Collections implementation

```
ASCollection --+--> ASOrderedCollection
               +--> ASCollectionPage -----> ASOrderedCollectionPage
```

The differences between `Collection*` and `OrderedCollection*` are purely JSON-related.
The ordered version changes the property mapping of `Items` and uses a different ActivityStreams type.   
The types are fully compatible and the ordered variant can be ignored in most cases.
