using NUnit.Framework;

namespace WrangleTests.Dictionary
{
    [TestFixture]
    public class FromTests
    {
        [Test]
        public void ShouldAcceptNullArgs()
        {
            var args = Wrangle.Dictionary.From(null);

            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldAcceptEmptyArgs()
        {
            var raw = new string[0];
            var args = Wrangle.Dictionary.From(raw);

            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldEnsureArgsLengthIsEven()
        {
            var raw = new[] { "one", "two", "three" };

            Assert.That(() => Wrangle.Dictionary.From(raw), Throws.ArgumentException);
        }

        [Test]
        public void ShouldReturnCorrectDictionary()
        {
            var raw = new[] { "k1", "v1", "k2", "v2", "k3", "v3", "k4", "v4" };
            var args = Wrangle.Dictionary.From(raw);

            Assert.That(args.Count, Is.EqualTo(raw.Length / 2));

            for(var i = 0; i < raw.Length; i += 2)
            {
                var k = raw[i];
                var v = raw[i + 1];

                Assert.That(args, Contains.Key(k));
                Assert.That(args[k], Is.EqualTo(v));
            }
        }

        [Test]
        public void ShouldEnsureAllKeysHavePrefix()
        {
            var raw = new[] { "-k1", "v1", "k2", "-v2", "-k3", "v3" };

            Assert.That(() => Wrangle.Dictionary.From(raw, "-"), Throws.ArgumentException);
        }

        [Test]
        public void ShouldRemovePrefix()
        {
            var raw = new[] { "-k1", "v1", "-k2", "v2", "-k3", "v3" };
            var args = Wrangle.Dictionary.From(raw, "-");

            Assert.That(args, Contains.Key("k1"));
            Assert.That(args.Keys, Has.None.EqualTo("-k1"));
        }

        [Test]
        public void ShouldAcceptNullArgsWithPrefix()
        {
            var args = Wrangle.Dictionary.From(null, "-");

            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldAcceptEmptyArgsWithPrefix()
        {
            var raw = new string[0];
            var args = Wrangle.Dictionary.From(raw, "-");

            Assert.That(args, Is.Not.Null);
            Assert.That(args, Is.Empty);
        }

        [Test]
        public void ShouldEnsureArgsLengthIsEvenWithPrefix()
        {
            var raw = new[] { "-one", "two", "-three" };

            Assert.That(() => Wrangle.Dictionary.From(raw, "-"), Throws.ArgumentException);
        }

        [Test]
        public void ShouldReturnCorrectDictionaryWithPrefix()
        {
            var prefix = "/";
            var raw = new[] { "/k1", "v1", "/k2", "v2", "/k3", "v3", "/k4", "v4" };
            var args = Wrangle.Dictionary.From(raw, "/");

            Assert.That(args.Count, Is.EqualTo(raw.Length / 2));

            for (var i = 0; i < raw.Length; i += 2)
            {
                var k = raw[i].Replace(prefix, string.Empty);
                var v = raw[i + 1];

                Assert.That(args, Contains.Key(k));
                Assert.That(args[k], Is.EqualTo(v));
            }
        }

        [Test]
        public void ShouldAllowMultiCharacterPrefix()
        {
            var prefix = "hello:";
            var raw = new[] { "hello:k1", "v1", "hello:k2", "v2", "hello:k3", "v3", "hello:k4", "v4" };
            var args = Wrangle.Dictionary.From(raw, prefix);

            Assert.That(args.Count, Is.EqualTo(raw.Length / 2));

            for (var i = 0; i < raw.Length; i += 2)
            {
                var k = raw[i].Replace(prefix, string.Empty);
                var v = raw[i + 1];

                Assert.That(args, Contains.Key(k));
                Assert.That(args[k], Is.EqualTo(v));
            }
        }
    }
}
