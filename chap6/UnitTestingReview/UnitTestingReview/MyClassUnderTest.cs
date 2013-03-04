using System.Linq;
using NUnit.Framework;

namespace UnitTestingReview
{
    public class MyStringClass
    {
        public string Reverse(string str)
        {
            if (str == null)
                return null;
            return new string(str.Reverse().ToArray());
        }
    }

    [TestFixture]
    public class MyStringClassTests
    {
        [Test]
        public void TestReverse()
        {
            var myStringObject = new MyStringClass();
            var reversedString = myStringObject.Reverse("hello");
            Assert.That(reversedString, Is.EqualTo("olleh"));
        }

        [Test]
        public void TestReverseWithNull()
        {
            var myStringObject = new MyStringClass();
            var reversedString = myStringObject.Reverse(null);
            Assert.That(reversedString, Is.Null);
        }
    }
}