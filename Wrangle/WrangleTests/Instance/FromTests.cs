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
            var args = Wrangle.Instance<MyIntArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            AssertIsDefault(args);
        }

        [Test]
        public void ShouldAcceptEmptyArgs()
        {
            var a = new string[0];
            var args = Wrangle.Instance<MyIntArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            AssertIsDefault(args);
        }

        [Test]
        public void ShouldSetStringProperties()
        {
            var a = new[] { "Id", "my-id", "Name", "my-name" };
            var args = Wrangle.Instance<MyStringArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Id, Is.EqualTo("my-id"));
            Assert.That(args.Name, Is.EqualTo("my-name"));
        }

        [Test]
        public void ShouldSetIntegerProperties()
        {
            var a = new[] { "Byte", "10", "SByte", "-11", "Short", "-2000", "UShort", "2001",
                "Int", "-300000", "UInt", "300001", "Long", "-4000000000", "ULong", "4000000001" };
            var args = Wrangle.Instance<MyIntegerArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Byte, Is.EqualTo(10));
            Assert.That(args.SByte, Is.EqualTo(-11));
            Assert.That(args.Short, Is.EqualTo(-2000));
            Assert.That(args.UShort, Is.EqualTo(2001));
            Assert.That(args.Int, Is.EqualTo(-300000));
            Assert.That(args.UInt, Is.EqualTo(300001u));
            Assert.That(args.Long, Is.EqualTo(-4000000000L));
            Assert.That(args.ULong, Is.EqualTo(4000000001UL));
        }

        [Test]
        public void ShouldSetRealProperties()
        {
            var a = new[] { "Float", "-123.45", "Double", "456.789", "Decimal", "0.000123456" };
            var args = Wrangle.Instance<MyRealArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Float, Is.EqualTo(-123.45f));
            Assert.That(args.Double, Is.EqualTo(456.789d));
            Assert.That(args.Decimal, Is.EqualTo(0.000123456m));
        }

        [Test]
        public void ShouldSetCharacterProperties()
        {
            var a = new[] { "Char", "x", "String", "y" };
            var args = Wrangle.Instance<MyCharacterArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Char, Is.EqualTo('x'));
            Assert.That(args.String, Is.EqualTo("y"));
        }

        [Test]
        public void ShouldSetBooleanProperties()
        {
            var a = new[] { "TrueValue", "true", "FalseValue", "false" };
            var args = Wrangle.Instance<MyBoolArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.TrueValue, Is.True);
            Assert.That(args.FalseValue, Is.False);
        }

        [Test]
        public void ShouldSetDateProperties()
        {
            var a = new[] { "TimeStamp", "2016-01-25 21:26:59", "Period", "00:05", "Offset", "2016-01-25 21:27:30 +01:00" };
            var args = Wrangle.Instance<MyDateArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.TimeStamp, Is.EqualTo(new DateTime(2016, 1, 25, 21, 26, 59)));
            Assert.That(args.Period, Is.EqualTo(TimeSpan.FromMinutes(5)));
            Assert.That(args.Offset, Is.EqualTo(new DateTimeOffset(new DateTime(2016, 1, 25, 21, 27, 30), TimeSpan.FromHours(1))));
        }

        [Test]
        public void ShouldSetGuidProperties()
        {
            var a = new[] { "Id", "b739ec98-41e1-49bf-8ff8-8b2c2035a475" };
            var args = Wrangle.Instance<MyGuidArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Id, Is.EqualTo(Guid.Parse("b739ec98-41e1-49bf-8ff8-8b2c2035a475")));
        }

        [Test]
        public void ShouldSetEnumByValue()
        {
            var a = new[] { "Value", "1" };
            var args = Wrangle.Instance<MyEnumArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Value, Is.EqualTo(MyEnumArgs.MyEnum.One));
        }

        [Test]
        public void SholdSetEnumByName()
        {
            var a = new[] { "Value", "Two" };
            var args = Wrangle.Instance<MyEnumArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Value, Is.EqualTo(MyEnumArgs.MyEnum.Two));
        }

        [Test]
        public void ShouldSetNonIntEnumByValue()
        {
            var a = new[] { "Value", "4000000003" };
            var args = Wrangle.Instance<MyLongEnumArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Value, Is.EqualTo(MyLongEnumArgs.MyULongEnum.Three));
        }

        [Test]
        public void ShouldSetNonIntEnumByName()
        {
            var a = new[] { "Value", "Two" };
            var args = Wrangle.Instance<MyLongEnumArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Value, Is.EqualTo(MyLongEnumArgs.MyULongEnum.Two));
        }

        [TestCase("/")]
        [TestCase("-")]
        public void ShouldDetectKnownPrefix(string prefix)
        {
            var a = new[] { prefix + "Name", "abc", prefix + "Value", "123" };
            var args = Wrangle.Instance<MyIntArgs>.From(a);

            Assert.That(args, Is.Not.Null);
            Assert.That(args.Name, Is.EqualTo("abc"));
            Assert.That(args.Value, Is.EqualTo(123));
        }

        [Test]
        public void ShouldValidateArgumentNames()
        {
            var a = new[] { "Name", "name", "Foo", "bar" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyIntArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForInvalidNumericArgument()
        {
            var a = new[] { "Name", "name", "Value", "foobar" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyIntArgs>.From(a));
        }
        
        [Test]
        public void ShouldThrowForOverflow()
        {
            var bigNumber = 1L + int.MaxValue;
            var a = new[] { "Name", "name", "Value", bigNumber.ToString() };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyIntArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForUnderflow()
        {
            var smallNumber = -1L + int.MinValue;
            var a = new[] { "Name", "name", "Value", smallNumber.ToString() };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyIntArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForSettingRealNumberToInteger()
        {
            var a = new[] { "Name", "name", "Value", "123.456" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyIntArgs>.From(a));
        }

        [TestCase("foo", "05:00", "2016-01-28 20:49:00 +00:00")]
        [TestCase("2016-01-28 20:49:00", "foo", "2016-01-28 20:49:00 +00:00")]
        [TestCase("2016-01-28 20:49:00", "05:00", "foo")]
        public void ShouldThrowForInvalidDateTimeArguments(string timestamp, string period, string offset)
        {
            var a = new[] { "TimeStamp", timestamp, "Period", period, "Offset", offset };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyDateArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForInvalidGuid()
        {
            var a = new[] { "Id", "zz39ec98-41e1-49bf-8ff8-8b2c2035a475" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyGuidArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForInvalidPropertyType()
        {
            var a = new[] { "Inner", "foo" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyImpossibleArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForOutOfRangeEnumValue()
        {
            var a = new[] { "Value", "1000" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyEnumArgs>.From(a));
        }

        [Test]
        public void ShouldThrowForInvalidEnumString()
        {
            var a = new[] { "Value", "Five" };

            Assert.Throws<ArgumentException>(() => Wrangle.Instance<MyEnumArgs>.From(a));
        }

        // TODO:
        // error handling e.g.:
        //    - attempt to assign string to int property
        //    - invalid enum name
        //    - invalid enum value
        //    - overflow integer properties
        //    - attempt to set real value to int property

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
        
        private class MyIntArgs
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private class MyStringArgs
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        private class MyBoolArgs
        {
            public bool TrueValue { get; set; }
            public bool FalseValue { get; set; }
        }

        private class MyCharacterArgs
        {
            public string String { get; set; }
            public char Char { get; set; }
        }

        private class MyIntegerArgs
        {
            public byte Byte { get; set; }
            public sbyte SByte { get; set; }
            public short Short { get; set; }
            public ushort UShort { get; set; }
            public int Int { get; set; }
            public uint UInt { get; set; }
            public long Long { get; set; }
            public ulong ULong { get; set; }
        }

        private class MyRealArgs
        {
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
        }

        private class MyDateArgs
        {
            public DateTime TimeStamp { get; set; }
            public TimeSpan Period { get; set; }
            public DateTimeOffset Offset { get; set; }
        }

        private class MyGuidArgs
        {
            public Guid Id { get; set; }
        }
        
        private class MyEnumArgs
        {
            public enum MyEnum
            {
                Zero = 0,
                One,
                Two
            }

            public MyEnum Value { get; set; }
        }

        private class MyLongEnumArgs
        {
            public enum MyULongEnum : ulong
            {
                One = 4000000001UL,
                Two = 4000000002UL,
                Three = 4000000003UL
            }

            public MyULongEnum Value { get; set; }
        }

        private class MyImpossibleArgs
        {
            public class InnerClass { }
            
            public InnerClass Inner { get; set; }
        }
    }
}
