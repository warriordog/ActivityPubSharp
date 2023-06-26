# Collections implementation

```
Collection -+-> PagedCollection ------> OrderedPagedCollection
            |
            +-> UnpagedCollection --+-> OrderedUnpagedCollection
                                    |
                                    +-> CollectionPage -----------> OrderedCollectionPage
```

The differences between `*Collection` and `Ordered*Collection` are purely JSON-related.
The ordered version changes the property mapping of `Items` and uses a different ActivityStreams type.   
The types are fully compatible and the ordered variant can be ignored in most cases.

| Type                       | Abstract | Ordered | Paged | Type Name             | Type Keys                     |
|----------------------------|----------|---------|-------|-----------------------|-------------------------------|
| ASCollection               | Yes      | ----    | ----  | Collection            | Collection, OrderedCollection |
| ASPagedCollection          | no       | no      | Yes   | Collection            |                               |
| ASUnpagedCollection        | no       | no      | no    | Collection            |                               |
| ASOrderedPagedCollection   | no       | Yes     | Yes   | OrderedCollection     |                               |
| ASOrderedUnpagedCollection | no       | Yes     | no    | OrderedCollection     |                               |
| ASOrderedCollectionPage    | no       | Yes     | no    | OrderedCollectionPage | OrderedCollectionPage         |
| ASCollectionPage           | no       | no      | no    | CollectionPage        | CollectionPage                |