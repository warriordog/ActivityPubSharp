// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Internal;

namespace ActivityPub.Types.Tests.Unit.Internal;

public abstract class CompositeASTypeTests
{
    /// <summary>
    ///     Contents:<br/>
    ///     <ul>
    ///         <li>Top-level types: 1.1, 1.2.1, 1.2.2, 1.3.1.1</li>
    ///         <li>Shadowed types: 1, 1.2, 1.3, 1.3.1</li>
    ///     </ul>
    /// </summary>
    private CompositeASType TypeUnderTest { get; } = new();

    protected CompositeASTypeTests()
    {
        TypeUnderTest.Add("1");
        TypeUnderTest.Add("1.1", "1");
        TypeUnderTest.Add("1.2", "1");
        TypeUnderTest.Add("1.2.1", "1.2");
        TypeUnderTest.Add("1.2.2", "1.2");
        TypeUnderTest.Add("1.3", "1");
        TypeUnderTest.Add("1.3.1", "1.3");
        TypeUnderTest.Add("1.3.1.1", "1.3.1");
    }

    public class TypesShould : CompositeASTypeTests
    {
        [Fact]
        public void ContainOnlyTopLevelTypes()
        {
            TypeUnderTest.Types.Should()
                .HaveCount(4)
                .And.Contain("1.1")
                .And.Contain("1.2.1")
                .And.Contain("1.2.2")
                .And.Contain("1.3.1.1");
        }
    }

    public class AllTypesShould : CompositeASTypeTests
    {
        [Fact]
        public void ContainAllTypes()
        {
            TypeUnderTest.AllTypes.Should()
                .HaveCount(8)
                .And.Contain("1")
                .And.Contain("1.1")
                .And.Contain("1.2")
                .And.Contain("1.2.1")
                .And.Contain("1.2.2")
                .And.Contain("1.3")
                .And.Contain("1.3.1")
                .And.Contain("1.3.1.1");
        }
    }

    public class AddShould : CompositeASTypeTests
    {
        [Fact]
        public void DoNothing_IfTypeIsDuplicate()
        {
            TypeUnderTest.Add("1");
            TypeUnderTest.Add("1.1", "1");

            TypeUnderTest.AllTypes.Should().HaveCount(8);
        }
        
        [Fact]
        public void UpdateAllTypes_IfNotDuplicate()
        {
            TypeUnderTest.Add("1.4", "1");

            TypeUnderTest.AllTypes.Should().Contain("1.4");
        }

        [Fact]
        public void UpdateTypes_IfNotShadowed()
        {
            TypeUnderTest.Add("1.4", "1");

            TypeUnderTest.Types.Should().Contain("1.4");
        }

        [Fact]
        public void NotUpdateTypes_IfShadowed()
        {
            TypeUnderTest.Add("1.4.1", "1.4");
            TypeUnderTest.Add("1.4", "1");

            TypeUnderTest.Types.Should().NotContain("1.4");
        }

        [Fact]
        public void RemoveShadowedTypes()
        {
            TypeUnderTest.Add("1.1.1", "1.1");
            
            TypeUnderTest.Types.Should().NotContain("1.1");
        }
    }
}