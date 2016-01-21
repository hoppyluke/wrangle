using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WrangleTests.Instance
{
    [TestFixture]
    public class FromTests
    {
        [Test]
        public void ShouldAcceptNullArgs()
        {
            string[] a = null;
            var args = Wrangle.Instance<MyArgs>.From(a);
            
            Assert.That(args, Is.Not.Null);
            AssertIsDefault(args);
        }

        [Test]
        public void ShouldAcceptEmptyArgs()
        {
            var a = new string[0];
            var args = Wrangle.Instance<MyArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            AssertIsDefault(args);
        }

        [Test]
        public void ShouldSetStringValues()
        {
            var a = new[] { "Id", "my-id", "Name", "my-name" };
            var args = Wrangle.Instance<MyStringArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Id, Is.EqualTo("my-id"));
            Assert.That(args.Name, Is.EqualTo("my-name"));
        }

        private static void AssertIsDefault(object o)
        {
            var properties = from p in o.GetType().GetProperties()
                             where p.GetSetMethod() != null
                             && p.GetSetMethod() != null
                             select p;

            foreach(var p in properties)
            {
                var v = p.GetValue(o);
                if (p.PropertyType.IsValueType)
                    Assert.That(v, Is.EqualTo(Activator.CreateInstance(p.PropertyType)));
                else
                    Assert.That(v, Is.Null);
            }
        }
        
        private class MyArgs
        {
            public string Name { get; set; }

            public int Value { get; set; }
        }

        private class MyStringArgs
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
