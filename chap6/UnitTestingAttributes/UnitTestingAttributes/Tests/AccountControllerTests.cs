using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using UnitTestingAttributes.Controllers;

namespace UnitTestingAttributes.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void ChangePassword_should_be_restricted_by_Authorize()
        {
            var classUnderTest = typeof (AccountController);
            var allMethods = classUnderTest.GetMethods();
            var changePwdMethods = allMethods.Where(m => m.Name == "ChangePassword");
            foreach (var changePwdMethod in changePwdMethods)
            {
                var attr = Attribute.GetCustomAttribute(changePwdMethod, typeof (AuthorizeAttribute));
                Assert.That(attr, Is.Not.Null);
            }
        }
    }
}