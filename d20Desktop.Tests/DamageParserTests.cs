using Fiction.GameScreen;

namespace d20Desktop.Tests
{
    [TestFixture]
    public class DamageParserTests
    {
        [Test]
        public void DamageParser_TryParse_ParsesDamage()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(10));

            Assert.IsTrue(DamageParser.TryParse("10+10", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(20));

            Assert.IsTrue(DamageParser.TryParse("10+10+30", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(50));
        }

        [Test]
        public void DamageParser_TryParse_AppliesDamageReductionToEach()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(5));

            Assert.IsTrue(DamageParser.TryParse("10+10", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(10));

            Assert.IsTrue(DamageParser.TryParse("10+10+30", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(35));
        }

        [Test]
        public void DamageParser_TryParse_AppliesDamageReductionToTotal()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10", 5, false, out amount));
            Assert.That(amount, Is.EqualTo(5));

            Assert.IsTrue(DamageParser.TryParse("10+10", 5, false, out amount));
            Assert.That(amount, Is.EqualTo(15));

            Assert.IsTrue(DamageParser.TryParse("10+10+30", 5, false, out amount));
            Assert.That(amount, Is.EqualTo(45));
        }

        [Test]
        public void DamageParser_TryParse_AppliesDamageReductionToEachHandlingNegatives()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10", 15, true, out amount));
            Assert.That(amount, Is.EqualTo(0));

            Assert.IsTrue(DamageParser.TryParse("10+10", 15, true, out amount));
            Assert.That(amount, Is.EqualTo(0));

            Assert.IsTrue(DamageParser.TryParse("10+10+30", 15, true, out amount));
            Assert.That(amount, Is.EqualTo(15));
        }

        [Test]
        public void DamageParser_TryParse_AppliesDamageReductionToTotalHandlingNegatives()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10", 15, false, out amount));
            Assert.That(amount, Is.EqualTo(0));

            Assert.IsTrue(DamageParser.TryParse("10+10", 25, false, out amount));
            Assert.That(amount, Is.EqualTo(0));

            Assert.IsTrue(DamageParser.TryParse("10+10+30", 55, false, out amount));
            Assert.That(amount, Is.EqualTo(0));
        }

        [Test]
        public void DamageParser_TryParse_HandlesInvalidString()
        {

            int amount = 0;
            Assert.IsFalse(DamageParser.TryParse("abcd", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(0));
            Assert.IsFalse(DamageParser.TryParse("10+15+a", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(0));
            Assert.IsFalse(DamageParser.TryParse("10-5", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(0));
            Assert.IsFalse(DamageParser.TryParse("10+{5", 0, true, out amount));
            Assert.That(amount, Is.EqualTo(0));
        }

        [Test]
        public void DamageParser_TryParse_HandlesBraces()
        {
            int amount = 0;
            Assert.IsTrue(DamageParser.TryParse("10+{10}", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(15));
            Assert.IsTrue(DamageParser.TryParse("10+{10+15}", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(30));
            Assert.IsTrue(DamageParser.TryParse("10+{10}+5", 5, true, out amount));
            Assert.That(amount, Is.EqualTo(15));
        }
    }
}
