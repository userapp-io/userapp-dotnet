using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserApp.CodeConventions;
using UserApp.Exceptions;


namespace UserApp.Tests
{
    public class ServiceIntegrationTests
    {
        public dynamic Service { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Service = new UserApp.API(Configuration.Default.TestAppId, new
            {
                BaseAddress = Configuration.Default.TestBaseAddress
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidServiceException))]
        public void TestThatInvalidServiceThrowsException()
        {
            this.Service.FakeService.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidMethodException))]
        public void TestThatInvalidServiceMethodThrowsException()
        {
            this.Service.User.FakeMethod();
        }

        [TestMethod]
        public void TestThatUnauthenticatedCallThrowsException()
        {
            try
            {
                this.Service.User.Get();
                Assert.Fail("Should not be able to pass this point.");
            }
            catch (ServiceException exception)
            {
                Assert.AreEqual("INVALID_CREDENTIALS", exception.ErrorCode);
            }
        }
    }
}
