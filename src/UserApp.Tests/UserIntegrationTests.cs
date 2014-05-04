using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserApp.CodeConventions;
using UserApp.Exceptions;

namespace UserApp.Tests
{
    [TestClass]
    public class UserIntegrationTests
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
        public void TestThatUserCanLogIn()
        {
            this.Service.User.Login(login: Configuration.Default.TestUserLogin, password: Configuration.Default.TestUserPassword);
            var user = this.Service.User.Get()[0];
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestThatUserHasAdminPermission()
        {
            this.Service.User.Login(login: Configuration.Default.TestUserLogin, password: Configuration.Default.TestUserPassword);
            
            var user = this.Service.User.Get()[0];
            Assert.IsNotNull(user);

            Assert.IsTrue(user.Permissions.admin.value);
        }
    }
}
