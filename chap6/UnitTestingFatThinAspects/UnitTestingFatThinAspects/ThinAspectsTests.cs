using System;
using System.Linq;
using System.Reflection;
using Moq;
using NUnit.Framework;
using PostSharp.Aspects;
using StructureMap;

namespace UnitTestingFatThinAspects
{
    public class MyNormalCode
    {
        [MyThinAspect]
        public string Reverse(string str)
        {
            return new string(str.Reverse().ToArray());
        }
    }

    [Serializable]
    public class MyThinAspect : OnMethodBoundaryAspect
    {
        IMyCrossCuttingConcern _concern;

        public override void RuntimeInitialize(MethodBase method)
        {
            if (!AspectSettings.On) return;
            _concern = ObjectFactory.GetInstance<IMyCrossCuttingConcern>();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (!AspectSettings.On) return;
            _concern.BeforeMethod("before");
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (!AspectSettings.On) return;
            _concern.AfterMethod("after");
        }
    }

    public interface IMyCrossCuttingConcern
    {
        void BeforeMethod(string logMessage);
        void AfterMethod(string logMessage);
    }

    [TestFixture]
    public class MyNormalCodeTest
    {
        [Test]
        public void TestReverse()
        {
            var stubAspect = new Mock<IMyCrossCuttingConcern>();
            ObjectFactory.Initialize(x =>
                x.For<IMyCrossCuttingConcern>().Use(stubAspect.Object)
            );

            var myCode = new MyNormalCode();
            var result = myCode.Reverse("hello");

            Assert.That(result, Is.EqualTo("olleh"));
        }

        [Test]
        public void TestReverseAlt()
        {
            AspectSettings.On = false;

            var myCode = new MyNormalCode();
            var result = myCode.Reverse("hello");

            Assert.That(result, Is.EqualTo("olleh"));
        }
    }

    public static class AspectSettings
    {
        public static bool On = true;
    }
}
