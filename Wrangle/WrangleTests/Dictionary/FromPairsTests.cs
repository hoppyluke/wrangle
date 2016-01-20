using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WrangleTests.Dictionary
{
    [TestFixture]
    public class FromPairsTests
    {
        [Test]
        public void ShouldEnsureSeparatorIsNotNull()
        {
            var raw = new[] { "one", "two" };
            Assert.That(() => Wrangle.Dictionary.FromPairs(null, raw), Throws.ArgumentNullException);
        }

        [Test]
        public void ShouldEnsureSeparatorIsNotEmpty()
        {
            var raw = new[] { "one", "two" };
            Assert.That(() => Wrangle.Dictionary.FromPairs(string.Empty, raw), Throws.ArgumentNullException);
        }

        [Test]
        public void ShouldAcceptNullArgs()
        {
            var args = Wrangle.Dictionary.FromPairs(":", null);
            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldAcceptEmptyArgs()
        {
            var args = Wrangle.Dictionary.FromPairs(":", new string[0]);
            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldEnsureAllArgsContainSeparator()
        {
            var raw = new[] { "one:1", "two:2", "three3" };
            Assert.That(() => Wrangle.Dictionary.FromPairs(":", raw), Throws.ArgumentException);
        }

        [Test]
        public void ShouldEnsureAllArgsContainSeparatorOnlyOnce()
        {
            var raw = new[] { "one:1", "two:2:II" };
            Assert.That(() => Wrangle.Dictionary.FromPairs(":", raw), Throws.ArgumentException);
        }

        [Test]
        public void ShouldReturnCorrectDictionary()
        {
            var raw = new[] { "k1:v1", "k2:v2", "k3:v3" };
            var args = Wrangle.Dictionary.FromPairs(":", raw);

            Assert.That(args.Count, Is.EqualTo(raw.Length));

            for(var i = 0; i < raw.Length; i++)
            {
                var pair = raw[i].Split(new[] { ":" }, StringSplitOptions.None);
                Assert.That(args, Contains.Key(pair[0]));
                Assert.That(args[pair[0]], Is.EqualTo(pair[1]));
            }
        }
    }
}
