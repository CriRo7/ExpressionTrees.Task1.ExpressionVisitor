using System;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        // todo: add as many test methods as you wish, but they should be enough to cover basic scenarios of the mapping generator

        [TestMethod]
        public void MappingGenerator_Assigns_Int_Properties()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>(f => f.IntFoo, b => b.IntBar);

            var foo = new Foo() {IntFoo = 5};
            var res = mapper.Map(foo);
            Assert.AreEqual(res.IntBar, foo.IntFoo);
        }

        [TestMethod]
        public void MappingGenerator_Assigns_String_Properties()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>(f => f.StringFoo, b => b.StringBar);

            var foo = new Foo() {StringFoo = "stringFOO"};
            var res = mapper.Map(foo);
            Assert.AreEqual(res.StringBar, foo.StringFoo);
        }

        [TestMethod]
        public void MappingGenerator_Assign_Different_Types_Throws_Error()
        {
            var mapGenerator = new MappingGenerator();
            
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var mapper = mapGenerator.Generate<Foo, Bar>(f => f.StringFoo, b => b.IntBar);

                var foo = new Foo() {StringFoo = "stringFOO"};
                var res = mapper.Map(foo);
            });
            
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var mapper = mapGenerator.Generate<Foo, Bar>(f => f.IntFoo, b => b.StringBar);

                var foo = new Foo() {StringFoo = "stringFOO"};
                var res = mapper.Map(foo);
            });
        }
    }
}
