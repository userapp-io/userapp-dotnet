using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserApp.CodeConventions;

namespace UserApp.Tests
{
    [TestClass]
    public class ObjectAccessDecoratorTests
    {
        public dynamic Underlying { get; set; }
        public dynamic Subject { get; set; }

        [TestInitialize]
        public void Setup()
        {
            this.Underlying = new ExpandoObject();
            this.Subject = new ObjectAccessDecorator(new CSharpCodeConvention(), this.Underlying);
        }

        [TestMethod]
        public void TestThatPropertyIsReadable()
        {
            this.Underlying.test = "first value";
            Assert.AreEqual("first value", this.Subject.test);
        }

        [TestMethod]
        public void TestThatPropertyIsWriteable()
        {
            this.Subject.test = "different value";
            Assert.AreEqual("different value", this.Subject.test);
        }

        [TestMethod]
        public void TestThatPropertyIsWriteableUsingIndex()
        {
            this.Subject["test"] = "different value";
            Assert.AreEqual("different value", this.Subject.test);
        }

        [TestMethod]
        public void TestThatObjectWrittenIsDecorated()
        {
            this.Subject.test = new {sub_test="other value"};
            this.Subject.test.bob = "different value";
            Assert.AreEqual("different value", this.Subject.test.bob);
            Assert.AreEqual("other value", this.Subject.test.sub_test);
        }
    }
}
