// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ActivityPub.Types.Util;

namespace ActivityPub.Types.Tests.Unit.Util;

public abstract class ASUriTests
{
    private const string TestUriString = "https://example.com/some.uri";
    
    public class ConstructorShould : ASUriTests
    {
        [Fact]
        public void CreateUriFromString()
        {
            var uri = new ASUri(TestUriString);
            uri.Uri.ToString().Should().Be(TestUriString);
        }

        [Fact]
        public void WrapExistingUri()
        {
            var obj = new Uri(TestUriString);
            var uri = new ASUri(obj);
            uri.Uri.Should().Be(obj);
        }
    }

    public class ToStringShould : ASUriTests
    {
        private ASUri UriUnderTest { get; } = new(TestUriString);

        [Fact]
        public void ReturnUriAsAString() => UriUnderTest.ToString().Should().Be(TestUriString);
    }

    public class OperatorTests : ASUriTests
    {
        [Fact]
        public void CastToStringShould_ReturnStringValue()
        {
            string str = new ASUri(TestUriString);
            str.Should().Be(TestUriString);
        }

        [Fact]
        public void CastFromStringShould_CreateFromString()
        {
            ASUri asUri = TestUriString;
            asUri.ToString().Should().Be(TestUriString);
        }

        [Fact]
        public void CastToUriShould_ReturnUriValue()
        {
            Uri asUri = new ASUri(TestUriString);
            asUri.ToString().Should().Be(TestUriString);
        }

        [Fact]
        public void CastFromUriShould_WrapUri()
        {
            var uri = new Uri(TestUriString);
            ASUri asUri = uri;
            asUri.Uri.Should().Be(uri);
        }
    }

    public abstract class EqualityTests : ASUriTests
    {
        private ASUri ASUriUnderTest { get; } = new(SameString);
        
        private const string SameString = "https://example.com/first";
        private Uri SameUri { get; } = new(SameString);
        private ASUri SameASUri { get; } = new(SameString);
        
        private const string OtherString = "https://example.com/second";
        private Uri OtherUri { get; } = new(OtherString);
        private ASUri OtherASUri { get; } = new(OtherString);
        
        public class EqualsMethodShould : EqualityTests
        {
            [Fact]
            public void ReturnTrue_WhenASUriMatches() => ASUriUnderTest.Equals(SameASUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenUriMatches() => ASUriUnderTest.Equals(SameUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenStringMatches() => ASUriUnderTest.Equals(SameString).Should().BeTrue();
            
            [Fact]
            public void ReturnFalse_WhenASUriDoesNotMatch() => ASUriUnderTest.Equals(OtherASUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriDoesNotMatch() => ASUriUnderTest.Equals(OtherUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringDoesNotMatch() => ASUriUnderTest.Equals(OtherString).Should().BeFalse();

            [Fact]
            public void ReturnFalse_WhenASUriIsNull() => ASUriUnderTest.Equals((ASUri?)null).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriIsNull() => ASUriUnderTest.Equals((Uri?)null).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringIsNull() => ASUriUnderTest.Equals((string?)null).Should().BeFalse();
        }

        public class EqualsOperatorShould : EqualityTests
        {
            [Fact]
            public void ReturnTrue_WhenASUriMatches() => (ASUriUnderTest == SameASUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenUriMatches() => (ASUriUnderTest == SameUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenStringMatches() => (ASUriUnderTest == SameString).Should().BeTrue();
            
            [Fact]
            public void ReturnFalse_WhenASUriDoesNotMatch() => (ASUriUnderTest == OtherASUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriDoesNotMatch() => (ASUriUnderTest == OtherUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringDoesNotMatch() => (ASUriUnderTest == OtherString).Should().BeFalse();

            [Fact]
            public void ReturnFalse_WhenASUriIsNull() => (ASUriUnderTest == (ASUri?)null).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriIsNull() => (ASUriUnderTest == (Uri?)null).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringIsNull() => (ASUriUnderTest == (string?)null).Should().BeFalse();
        }

        public class NotEqualsOperatorShould : EqualityTests
        {
            [Fact]
            public void ReturnFalse_WhenASUriMatches() => (ASUriUnderTest != SameASUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriMatches() => (ASUriUnderTest != SameUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringMatches() => (ASUriUnderTest != SameString).Should().BeFalse();
            
            [Fact]
            public void ReturnTrue_WhenASUriDoesNotMatch() => (ASUriUnderTest != OtherASUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenUriDoesNotMatch() => (ASUriUnderTest != OtherUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenStringDoesNotMatch() => (ASUriUnderTest != OtherString).Should().BeTrue();

            [Fact]
            public void ReturnTrue_WhenASUriIsNull() => (ASUriUnderTest != (ASUri?)null).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenUriIsNull() => (ASUriUnderTest != (Uri?)null).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenStringIsNull() => (ASUriUnderTest != (string?)null).Should().BeTrue();
        }

        public class AreEqualShould : EqualityTests
        {
            [Fact]
            public void ReturnTrue_WhenASUriMatches() => ASUri.AreEqual(ASUriUnderTest, SameASUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenUriMatches() => ASUri.AreEqual(ASUriUnderTest, SameUri).Should().BeTrue();
            [Fact]
            public void ReturnTrue_WhenStringMatches() => ASUri.AreEqual(ASUriUnderTest, SameString).Should().BeTrue();
            
            [Fact]
            public void ReturnFalse_WhenASUriDoesNotMatch() => ASUri.AreEqual(ASUriUnderTest, OtherASUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenUriDoesNotMatch() => ASUri.AreEqual(ASUriUnderTest, OtherUri).Should().BeFalse();
            [Fact]
            public void ReturnFalse_WhenStringDoesNotMatch() => ASUri.AreEqual(ASUriUnderTest, OtherString).Should().BeFalse();


            [Fact]
            public void ReturnFalse_WhenOnlyLeftIsNull() => ASUri.AreEqual(null, ASUriUnderTest).Should().BeFalse();
            [Fact]
            public void Return_False_WhenOnlyRightIsNull() => ASUri.AreEqual(ASUriUnderTest, (ASUri?)null).Should().BeFalse();
            [Fact]
            public void ReturnTrue_WhenBothAreNull() => ASUri.AreEqual(null, (ASUri?)null).Should().BeTrue();
        }
    }
}