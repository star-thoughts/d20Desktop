using System;
using Fiction.GameScreen.Combat;
using NUnit.Framework;
using Moq;

namespace Fiction.GameScreen.Tests
{
    [TestFixture]
    public class CombatScenarioTests
    {
        [Test]
        public void CombatScenario_IsValid_IsFalseIfNoName()
        {
            CampaignSettings campaign = new CampaignSettings();
            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Combatants.Add(new Mock<ICombatantTemplate>().Object);

            Assert.IsFalse(scenario.IsValid);
        }

        [Test]
        public void CombatScenario_IsValid_IsFalseIfNoCombatants()
        {
            CampaignSettings campaign = new CampaignSettings();
            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Name = "Test";

            Assert.IsFalse(scenario.IsValid);
        }

        [Test]
        public void CombatScenario_IsValid_IsTrueIfHasNameAndCombatants()
        {
            CampaignSettings campaign = new CampaignSettings();
            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Name = "Test";
            scenario.Combatants.Add(new Mock<ICombatantTemplate>().Object);

            Assert.IsTrue(scenario.IsValid);
        }
    }
}
