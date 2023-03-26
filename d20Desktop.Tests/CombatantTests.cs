using System;
using Fiction.GameScreen.Combat;
using NUnit.Framework;
using Moq;

namespace Fiction.GameScreen.Tests
{
    [TestFixture]
    public class CombatantTests
    {
        #region TryBeginTurn
        [Test]
        public void Combatant_TryBeginTurn_FastHeals()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template = new Mock<ICombatantTemplate>();
            template.SetupGet(p => p.FastHealing).Returns(5);

            CombatantPreparer preparer = new CombatantPreparer(template.Object);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.Health.MaxHealth = 100;
            combatant.Health.LethalDamage = 20;

            combatant.TryBeginTurn(new CombatSettings());

            Assert.That(combatant.Health.LethalDamage, Is.EqualTo(15));
        }
        [Test]
        public void Combatant_TryBeginTurn_ReturnsTrueIfNotSkippingDown()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template = new Mock<ICombatantTemplate>();

            CombatantPreparer preparer = new CombatantPreparer(template.Object);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.Health.MaxHealth = 10;
            combatant.Health.LethalDamage = 20;

            Assert.IsTrue(combatant.TryBeginTurn(new CombatSettings()));
        }
        [Test]
        public void Combatant_TryBeginTurn_ReturnsFalseIfSkippingDown()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template = new Mock<ICombatantTemplate>();

            CombatantPreparer preparer = new CombatantPreparer(template.Object);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.Health.MaxHealth = 10;
            combatant.Health.LethalDamage = 20;

            Assert.IsFalse(combatant.TryBeginTurn(new CombatSettings() { SkipDownedCombatants = true }));
        }
        [Test]
        public void Combatant_TryBeginTurn_ReturnsTrueIfSkippingDownButNotDown()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template = new Mock<ICombatantTemplate>();

            CombatantPreparer preparer = new CombatantPreparer(template.Object);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.Health.MaxHealth = 10;
            combatant.Health.LethalDamage = 5;

            Assert.IsTrue(combatant.TryBeginTurn(new CombatSettings() { SkipDownedCombatants = true }));
        }
        #endregion
        #region IsCurrent
        [Test]
        public void Combatant_IsCurrent_SetsHasGoneOnce()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template = new Mock<ICombatantTemplate>();

            CombatantPreparer preparer = new CombatantPreparer(template.Object);
            Combatant combatant = new Combatant(campaign, preparer);

            combatant.IsCurrent = true;
            Assert.IsTrue(combatant.HasGoneOnce);
        }
        #endregion
    }
}
