using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserApp.CodeConventions;

namespace UserApp.Tests
{
    [TestClass]
    public class CSharpCodeConventionTests
    {
        public ICodeConvention Convention { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Convention = new CSharpCodeConvention();
        }

        [TestMethod]
        public void TestThatArgumentInOriginalCaseStaysIntact()
        {
            Assert.AreEqual("login", this.Convention.ConvertArgumentName("login"));
            Assert.AreEqual("provider_id", this.Convention.ConvertArgumentName("provider_id"));
        }

        [TestMethod]
        public void TestThatCSharpArgumentIsTranslatedCorrectly()
        {
            Assert.AreEqual("provider_id", this.Convention.ConvertArgumentName("providerId"));
        }

        [TestMethod]
        public void TestThatCSharpArgumentAlternativeIsTranslatedCorrectly()
        {
            Assert.AreEqual("login", this.Convention.ConvertArgumentName("Login"));
            Assert.AreEqual("provider_id", this.Convention.ConvertArgumentName("ProviderId"));
        }

        [TestMethod]
        public void TestThatMethodInOriginalCaseIsTranslatedCorrectly()
        {
            Assert.AreEqual("login", this.Convention.ConvertMethodName("login"));
            Assert.AreEqual("getSubscriptionDetails", this.Convention.ConvertMethodName("getSubscriptionDetails"));
        }

        [TestMethod]
        public void TestThatCSharpMethodIsTranslatedCorrectly()
        {
            Assert.AreEqual("login", this.Convention.ConvertMethodName("Login"));
            Assert.AreEqual("getSubscriptionDetails", this.Convention.ConvertMethodName("GetSubscriptionDetails"));
        }

        [TestMethod]
        public void TestThatPropertyInOriginalCaseIsTranslatedCorrectly()
        {
            Assert.AreEqual("login", this.Convention.ConvertPropertyName("login"));
            Assert.AreEqual("get_subscription_details", this.Convention.ConvertPropertyName("getSubscriptionDetails"));
        }

        [TestMethod]
        public void TestThatCSharpPropertyIsTranslatedCorrectly()
        {
            Assert.AreEqual("login", this.Convention.ConvertPropertyName("Login"));
            Assert.AreEqual("get_subscription_details", this.Convention.ConvertPropertyName("GetSubscriptionDetails"));
        }

        [TestMethod]
        public void TestThatCSharpServiceInOriginalCaseIsTranslatedCorrectly()
        {
            Assert.AreEqual("user", this.Convention.ConvertServiceName("user"));
            Assert.AreEqual("paymentMethod", this.Convention.ConvertServiceName("paymentMethod"));
        }

        [TestMethod]
        public void TestThatCSharpServiceIsTranslatedCorrectly()
        {
            Assert.AreEqual("user", this.Convention.ConvertServiceName("User"));
            Assert.AreEqual("paymentMethod", this.Convention.ConvertServiceName("PaymentMethod"));
        }
    }
}
