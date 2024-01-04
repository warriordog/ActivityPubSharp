// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class JsonLDContextTests
{
    public class ActivityStreamsShould : JsonLDContextTests
    {
        [Fact]
        public void IncludeASContext()
        {
            var context = IJsonLDContext.ActivityStreams;
            context.Should().Contain(JsonLDContextObject.ActivityStreams);
        }

        [Fact]
        public void IncludeNoOtherContexts()
        {
            var context = IJsonLDContext.ActivityStreams;
            context.Count.Should().Be(1);
        }
        
        [Fact]
        public void ReturnSharedInstance()
        {
            var first = IJsonLDContext.ActivityStreams;
            var second = IJsonLDContext.ActivityStreams;
            first.Should().BeSameAs(second);
        }
    }
    
    public class CreateASContextShould : JsonLDContextTests
    {
        [Fact]
        public void IncludeASContext()
        {
            var context = JsonLDContext.CreateASContext();
            context.Should().Contain(JsonLDContextObject.ActivityStreams);
        }

        [Fact]
        public void IncludeNoOtherContexts()
        {
            var context = JsonLDContext.CreateASContext();
            context.Count.Should().Be(1);
        }
        
        [Fact]
        public void ReturnNewInstance()
        {
            var first = JsonLDContext.CreateASContext();
            var second = JsonLDContext.CreateASContext();
            first.Should().NotBeSameAs(second);
        }
    }

    public class ContextsShould : JsonLDContextTests
    {
        private JsonLDContext ParentContext { get; }
        private JsonLDContext ContextUnderTest { get; }

        public ContextsShould()
        {
            ParentContext =
            [
                "https://example.com/context-1.jsonld",
                "https://example.com/context-1.jsonld",
                "https://example.com/context-2.jsonld"
            ];
            ContextUnderTest = new JsonLDContext(ParentContext)
            {
                "https://example.com/context-2.jsonld",
                "https://example.com/context-3.jsonld",
                "https://example.com/context-3.jsonld",
                "https://example.com/context-4.jsonld"
            };
        }
        
        [Fact]
        public void IncludeAllLocalContexts()
        {
            ContextUnderTest.Contexts.Should()
                .Contain("https://example.com/context-2.jsonld")
                .And.Contain("https://example.com/context-3.jsonld")
                .And.Contain("https://example.com/context-4.jsonld");
        }

        [Fact]
        public void IncludeAllInheritedContexts()
        {
            ContextUnderTest.Contexts.Should()
                .Contain("https://example.com/context-1.jsonld")
                .And.Contain("https://example.com/context-2.jsonld");
        }

        [Fact]
        public void NotIncludeDuplicates()
        {
            ContextUnderTest.Contexts.Should().HaveCount(4);
        }
    }

    public class LocalContextsShould : JsonLDContextTests
    {
        private JsonLDContext ParentContext { get; }
        private JsonLDContext ContextUnderTest { get; }
        
        public LocalContextsShould()
        {
            ParentContext =
            [
                "https://example.com/context-1.jsonld",
                "https://example.com/context-2.jsonld"
            ];
            ContextUnderTest = new JsonLDContext(ParentContext)
            {
                "https://example.com/context-2.jsonld",
                "https://example.com/context-3.jsonld",
                "https://example.com/context-4.jsonld"
            };
        }

        [Fact]
        public void ContainAllLocalContexts()
        {
            ContextUnderTest.LocalContexts.Should()
                .Contain("https://example.com/context-3.jsonld")
                .And.Contain("https://example.com/context-4.jsonld");
        }

        [Fact]
        public void NotContainInheritedContexts()
        {
            ContextUnderTest.LocalContexts.Should().NotContain("https://example.com/context-1.jsonld");
        }

        [Fact]
        public void NotContainSharedContexts()
        {
            ContextUnderTest.LocalContexts.Should().NotContain("https://example.com/context-2.jsonld");
        }
    }

    public class ContainsContextShould : JsonLDContextTests
    {
        private JsonLDContext ParentContext { get; }
        private JsonLDContext ContextUnderTest { get; }
        private JsonLDContextObject ContextObject { get; }

        public ContainsContextShould()
        {
            ParentContext = ["https://example.com/parent.jsonld"];
            ContextObject = new JsonLDContextObject(new Dictionary<string, JsonLDTerm>
            {
                ["key"] = new JsonLDExpandedTerm
                {
                    Id = "key",
                    Type = "value"
                }
            });
            ContextUnderTest = new JsonLDContext(ParentContext)
            {
                "https://example.com/context.jsonld",
                ContextObject
            };
        }
        
        [Fact]
        public void ReturnTrue_WhenContextIsEmpty()
        {
            var emptyContext = new JsonLDContext();
            ContextUnderTest.Contains(emptyContext).Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_WhenAllObjectsArePresent()
        {
            var otherContext = new JsonLDContext
            {
                "https://example.com/context.jsonld"
            };
            ContextUnderTest.Contains(otherContext).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenContextHasExtraObject()
        {
            var otherContext = new JsonLDContext
            {
                "https://example.com/context.jsonld",
                "https://example.com/wrong.jsonld"
            };
            ContextUnderTest.Contains(otherContext).Should().BeFalse();
        }

        [Fact]
        public void ReturnTrue_WhenParentContainsContexts()
        {
            var otherContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest.Contains(otherContext).Should().BeTrue();
        }
    }

    public class ContainsContextObjectShould : JsonLDContextTests
    {
        private JsonLDContext ParentContext { get; }
        private JsonLDContext ContextUnderTest { get; }

        public ContainsContextObjectShould()
        {
            ParentContext = ["https://example.com/parent.jsonld"];
            ContextUnderTest = new JsonLDContext(ParentContext)
            {
                "https://example.com/context.jsonld"
            };
        }
        
        [Fact]
        public void ReturnTrue_WhenObjectIsPresent()
        {
            ContextUnderTest.Contains("https://example.com/context.jsonld").Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_WhenObjectIsPresentInParent()
        {
            ContextUnderTest.Contains("https://example.com/parent.jsonld").Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenObjectIsNotPresent()
        {
            ContextUnderTest.Contains("https://example.com/wrong.jsonld").Should().BeFalse();
        }
    }

    public class ContainsTermShould : JsonLDContextTests
    {
        private JsonLDTerm ParentTerm { get; }
        private JsonLDTerm ChildTerm { get; }
        private JsonLDContext ContextUnderTest { get; }

        public ContainsTermShould()
        {
            ParentTerm = new JsonLDTerm { Id = "parent" };
            ChildTerm = new JsonLDExpandedTerm { Id = "child", Type = "value" };
            var parentContext = new JsonLDContext
            {
                new JsonLDContextObject(new Dictionary<string, JsonLDTerm>
                {
                    [ParentTerm.Id] = ParentTerm
                })
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                new JsonLDContextObject(new Dictionary<string, JsonLDTerm>
                {
                    [ChildTerm.Id] = ChildTerm
                })
            };
        }
        
        [Fact]
        public void ReturnTrue_WhenTermIsPresent()
        {
            ContextUnderTest.Contains(ChildTerm).Should().BeTrue();
        }

        [Fact]
        public void ReturnTrue_WhenTermIsPresentInParent()
        {
            ContextUnderTest.Contains(ParentTerm).Should().BeTrue();
        }

        [Fact]
        public void ReturnFalse_WhenTermIsNotPresent()
        {
            var wrongTerm = new JsonLDTerm { Id = "wrong" };
            ContextUnderTest.Contains(wrongTerm).Should().BeFalse();
        }
    }

    public class AddContextShould : JsonLDContextTests
    {
        private JsonLDContext ContextUnderTest { get; }

        public AddContextShould()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };
        }
        
        [Fact]
        public void DoNothingWhenContextIsEmpty()
        {
            ContextUnderTest.Add(new JsonLDContext());
            ContextUnderTest.Should().HaveCount(2);
        }

        [Fact]
        public void AddAllObjectsFromContext()
        {
            var context = new JsonLDContext
            {
                "https://example.com/new.jsonld"
            };
            ContextUnderTest.Add(context);
            ContextUnderTest.Should().Contain("https://example.com/new.jsonld");
        }
    }

    public class AddContextObjectShould : JsonLDContextTests
    {
        
        private JsonLDContext ContextUnderTest { get; }

        public AddContextObjectShould()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };
        }
        
        [Fact]
        public void DoNothingWhenContextIsAlreadyPresent()
        {
            ContextUnderTest.Add("https://example.com/child.jsonld");
            ContextUnderTest.Should().HaveCount(2);
        }
        
        [Fact]
        public void DoNothingWhenContextIsAlreadyPresentInParent()
        {
            ContextUnderTest.Add("https://example.com/parent.jsonld");
            ContextUnderTest.Should().HaveCount(2);
        }

        [Fact]
        public void AddObjectToContext()
        {
            ContextUnderTest.Add("https://example.com/new.jsonld");
            ContextUnderTest.Should().Contain("https://example.com/new.jsonld");   
        }
    }

    public class ClearShould : JsonLDContextTests
    {
        private JsonLDContext ContextUnderTest { get; }

        public ClearShould()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };
        }
        
        [Fact]
        public void RemoveAllLocalContexts()
        {
            ContextUnderTest.Clear();
            ContextUnderTest.Contains("https://example.com/child.jsonld").Should().BeFalse();
        }

        [Fact]
        public void IgnoreContextsInParent()
        {
            ContextUnderTest.Clear();
            ContextUnderTest.Contains("https://example.com/parent.jsonld").Should().BeTrue();               
        }
    }

    public class CopyToShould : JsonLDContextTests
    {
        private JsonLDContext ContextUnderTest { get; }

        public CopyToShould()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };
        }
        
        [Fact]
        public void ThrowIfThereIsNotEnoughSpace()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var array = new JsonLDContextObject[1];
                ContextUnderTest.CopyTo(array, 0);
            });
        }

        [Fact]
        public void CopyAllLocalContexts()
        {
            var array = new JsonLDContextObject[2];
            ContextUnderTest.CopyTo(array, 0);
            array.Should().Contain("https://example.com/child.jsonld");
        }

        [Fact]
        public void CopyAllParentContexts()
        {
            var array = new JsonLDContextObject[2];
            ContextUnderTest.CopyTo(array, 0);
            array.Should().Contain("https://example.com/parent.jsonld");
        }

        [Fact]
        public void CopyToCorrectOffset()
        {
            var array = new JsonLDContextObject[4];
            ContextUnderTest.CopyTo(array, 2);
            array[0].Should().BeNull();
            array[1].Should().BeNull();
            array[2].Should().NotBeNull();
            array[3].Should().NotBeNull();
        }
    }
    
    public class RemoveShould : JsonLDContextTests
    {
        private JsonLDContext ContextUnderTest { get; }

        public RemoveShould()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            ContextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };
        }
        
        [Fact]
        public void RemoveLocalContext()
        {
            ContextUnderTest.Remove("https://example.com/child.jsonld");
            ContextUnderTest.Should().NotContain("https://example.com/child.jsonld");
        }

        [Fact]
        public void IgnoreParentContext()
        {
            ContextUnderTest.Remove("https://example.com/parent.jsonld");
            ContextUnderTest.Should().Contain("https://example.com/parent.jsonld");   
        }

        [Fact]
        public void IgnoreMissingContext()
        {
            ContextUnderTest.Remove("https://example.com/wrong.jsonld");
            ContextUnderTest.Should().NotContain("https://example.com/wrong.jsonld");   
        }
    }

    public class CountShould : JsonLDContextTests
    {
        [Fact]
        public void AddParentCount_WhenParentIsPresent()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            var contextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };

            contextUnderTest.Count.Should().Be(2);
        }

        [Fact]
        public void UseLocalCount_WhenParentIsNull()
        {
            var contextUnderTest = new JsonLDContext()
            {
                "https://example.com/context.jsonld"
            };

            contextUnderTest.Count.Should().Be(1);   
        }
    }

    public class GetEnumeratorShould : JsonLDContextTests
    {
        [Fact]
        public void IncludeAllContexts()
        {
            var parentContext = new JsonLDContext
            {
                "https://example.com/parent.jsonld"
            };
            var contextUnderTest = new JsonLDContext(parentContext)
            {
                "https://example.com/child.jsonld"
            };

            var enumeratedCopy = contextUnderTest.ToList();

            enumeratedCopy.Should()
                .HaveCount(2)
                .And.Contain("https://example.com/parent.jsonld")
                .And.Contain("https://example.com/child.jsonld");
        }
    }
}