// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using ActivityPub.Types.AS;
using ActivityPub.Types.AS.Collection;
using ActivityPub.Types.AS.Extended.Object;
using ActivityPub.Types.Attributes;
using ActivityPub.Types.Tests.Util.Fixtures;
using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Integration.Serialization;

public class ValueStripSerializationTests : SerializationTests
{
    public ValueStripSerializationTests(JsonLdSerializerFixture fixture) : base(fixture) {}

    [Fact]
    public void NullObjectsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASObject
        {
            Image = null
        };

        JsonUnderTest.Should().NotHaveProperty("image");
    }

    [Fact]
    public void NullObjectsShould_BePreserved_WhenIgnoreConditionIsNever()
    {
        ObjectUnderTest = new FakeObjectWithSpecialNullability();
        JsonUnderTest.Should().HaveProperty(nameof(FakeObjectWithSpecialNullability.NeverIgnoreObject));
    }

    [Fact]
    public void DefaultValuesShould_BeStrippedFromOutput_WhenIgnoreConditionIsWritingDefault()
    {
        ObjectUnderTest = new FakeObjectWithSpecialNullability();
        JsonUnderTest.Should().NotHaveProperty(nameof(FakeObjectWithSpecialNullability.IgnoreWhenDefaultInt));
    }

    [Fact]
    public void DefaultValuesShould_BePreserved_WhenIgnoreConditionIsNever()
    {
        ObjectUnderTest = new FakeObjectWithSpecialNullability();
        JsonUnderTest.Should().HaveProperty(nameof(FakeObjectWithSpecialNullability.NeverIgnoreInt));
    }

    [Fact]
    public void EmptyCollectionsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASObject
        {
            Attachment = new LinkableList<ASObject>()
        };

        JsonUnderTest.Should().NotHaveProperty("attachment");
    }

    [Fact]
    public void NullCollectionsShould_BeStrippedFromOutput()
    {
        ObjectUnderTest = new ASCollection
        {
            Items = null
        };

        JsonUnderTest.Should().NotHaveProperty("items");
    }

    [Fact]
    public void NullCollectionsShould_BeStripped_WhenIgnoreConditionIsNever()
    {
        ObjectUnderTest = new FakeObjectWithSpecialNullability();
        JsonUnderTest.Should().NotHaveProperty(nameof(FakeObjectWithSpecialNullability.NeverIgnoreList));
    }

    [Fact]
    public void NonNullObjectsShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            Image = new ImageObject()
        };

        JsonUnderTest.Should().HaveProperty("image");
    }

    [Fact]
    public void NonDefaultValuesShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            StartTime = DateTime.Now
        };

        JsonUnderTest.Should().HaveProperty("startTime");
    }

    [Fact]
    public void NonEmptyCollectionsShould_BePreserved()
    {
        ObjectUnderTest = new ASObject
        {
            Attachment = new LinkableList<ASObject>
            {
                new ASObject()
            }
        };

        JsonUnderTest.Should().HaveProperty("attachment");
    }
}

public class FakeObjectWithSpecialNullability : ASObject
{
    public FakeObjectWithSpecialNullability() => Entity = new FakeObjectWithSpecialNullabilityEntity { TypeMap = TypeMap };
    public FakeObjectWithSpecialNullability(TypeMap typeMap) : base(typeMap) => Entity = TypeMap.AsEntity<FakeObjectWithSpecialNullabilityEntity>();
    private FakeObjectWithSpecialNullabilityEntity Entity { get; }


    public ASObject? NeverIgnoreObject
    {
        get => Entity.NeverIgnoreObject;
        set => Entity.NeverIgnoreObject = value;
    }

    public int NeverIgnoreInt
    {
        get => Entity.NeverIgnoreInt;
        set => Entity.NeverIgnoreInt = value;
    }

    public List<string> NeverIgnoreList
    {
        get => Entity.NeverIgnoreList;
        set => Entity.NeverIgnoreList = value;
    }

    public int IgnoreWhenDefaultInt
    {
        get => Entity.IgnoreWhenDefaultInt;
        set => Entity.IgnoreWhenDefaultInt = value;
    }
}

[ImpliesOtherEntity(typeof(ASObjectEntity))]
public sealed class FakeObjectWithSpecialNullabilityEntity : ASEntity<FakeObjectWithSpecialNullability>
{
    public const string FakeObjectWithSpecialNullabilityEntityName = "FakeObjectWithSpecialNullability";
    public override string ASTypeName => FakeObjectWithSpecialNullabilityEntityName;

    public override IReadOnlySet<string>? ReplacesASTypes { get; } = new HashSet<string>
    {
        ASObjectEntity.ObjectType
    };


    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public ASObject? NeverIgnoreObject { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public int NeverIgnoreInt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public List<string> NeverIgnoreList { get; set; } = new();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int IgnoreWhenDefaultInt { get; set; }
}