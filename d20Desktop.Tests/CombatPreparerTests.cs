using System;
using Fiction.GameScreen.Combat;
using Fiction.GameScreen.Serialization;
using NUnit.Framework;
using Moq;

namespace Fiction.GameScreen.Tests
{
    [TestFixture]
    public class CombatPreparerTests
    {
        #region Combat Preparer
        [Test]
        public void CombatPreparer_Constructor_ThrowsIfNullScenario()
        {
            try
            {
                CombatPreparer prep = new CombatPreparer((CombatScenario)null);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException exc)
            {
                Assert.That(exc.ParamName, Is.EqualTo("scenario"));
            }
        }
        [Test]
        public void CombatPreparer_Constructor_ThrowsIfNullCombat()
        {
            try
            {
                CampaignSettings campaign = new CampaignSettings();
                CombatPreparer prep = new CombatPreparer(campaign, null);
                Assert.Fail("Expected ArgumentNullException was not thrown.");
            }
            catch (ArgumentNullException exc)
            {
                Assert.That(exc.ParamName, Is.EqualTo("combat"));
            }
        }
        [Test]
        public void CombatPreparer_Constructor_AddsCombatantsAndSetsOrdinals()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template1 = new Mock<ICombatantTemplate>();
            Mock<ICombatantTemplate> template2 = new Mock<ICombatantTemplate>();
            Mock<ICombatantTemplate> template3 = new Mock<ICombatantTemplate>();

            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Combatants.Add(template1.Object);
            scenario.Combatants.Add(template2.Object);
            scenario.Combatants.Add(template3.Object);

            template1.SetupGet(p => p.Name).Returns("Combatant");
            template2.SetupGet(p => p.Name).Returns("Combatant");
            template3.SetupGet(p => p.Name).Returns("Different");
            template1.SetupGet(p => p.HitDieString).Returns("1d8");
            template2.SetupGet(p => p.HitDieString).Returns("1d8");
            template3.SetupGet(p => p.HitDieString).Returns("1d8");

            template1.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template1.Object), new CombatantPreparer(template1.Object) });
            template2.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template2.Object) });
            template3.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template3.Object) });

            CombatPreparer preparer = new CombatPreparer(scenario);

            Assert.That(preparer.Combatants.Count, Is.EqualTo(4));
            Assert.That(preparer.Combatants[0].Ordinal, Is.EqualTo(1));
            Assert.That(preparer.Combatants[1].Ordinal, Is.EqualTo(2));
            Assert.That(preparer.Combatants[2].Ordinal, Is.EqualTo(3));
            Assert.That(preparer.Combatants[3].Ordinal, Is.EqualTo(0), "The only combatant with this name should have an ordinal of 0.");
        }
        [Test]
        public void CombatPreparer_AddCombatants_AddsCombatantsAndSetsOrdinals()
        {
            CampaignSettings campaign = new CampaignSettings();
            Mock<ICombatantTemplate> template1 = new Mock<ICombatantTemplate>();
            Mock<ICombatantTemplate> template2 = new Mock<ICombatantTemplate>();
            Mock<ICombatantTemplate> template3 = new Mock<ICombatantTemplate>();

            CombatScenario scenario = new CombatScenario(campaign);
            scenario.Combatants.Add(template1.Object);
            scenario.Combatants.Add(template2.Object);
            scenario.Combatants.Add(template3.Object);

            template1.SetupGet(p => p.Name).Returns("Combatant");
            template2.SetupGet(p => p.Name).Returns("Combatant");
            template3.SetupGet(p => p.Name).Returns("Different");
            template1.SetupGet(p => p.HitDieString).Returns("1d8");
            template2.SetupGet(p => p.HitDieString).Returns("1d8");
            template3.SetupGet(p => p.HitDieString).Returns("1d8");

            template1.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template1.Object), new CombatantPreparer(template1.Object) });
            template2.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template2.Object) });
            template3.Setup(p => p.Prepare(It.IsAny<CombatPreparer>()))
                .Returns((CombatPreparer c) => new CombatantPreparer[] { new CombatantPreparer(template3.Object) });

            CombatPreparer preparer = new CombatPreparer(scenario);
            preparer.AddCombatants(scenario);

            Assert.That(preparer.Combatants.Count, Is.EqualTo(8));
            Assert.That(preparer.Combatants[0].Ordinal, Is.EqualTo(1));
            Assert.That(preparer.Combatants[1].Ordinal, Is.EqualTo(2));
            Assert.That(preparer.Combatants[2].Ordinal, Is.EqualTo(3));
            Assert.That(preparer.Combatants[3].Ordinal, Is.EqualTo(1));
            Assert.That(preparer.Combatants[4].Ordinal, Is.EqualTo(4));
            Assert.That(preparer.Combatants[5].Ordinal, Is.EqualTo(5));
            Assert.That(preparer.Combatants[6].Ordinal, Is.EqualTo(6));
            Assert.That(preparer.Combatants[7].Ordinal, Is.EqualTo(2));
        }
        [Test]
        public void CombatPreparer_ResolveInitiatives_ResolvesInitiativesAndGroupsTies()
        {
            CombatPreparer preparer = new CombatPreparer(new CampaignSettings());

            Mock<ICombatantTemplate> template1 = new Mock<ICombatantTemplate>();

            CombatantPreparer combatant1 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 10, };
            CombatantPreparer combatant2 = new CombatantPreparer(template1.Object) { InitiativeModifier = 0, InitiativeTotal = 1, };
            CombatantPreparer combatant3 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 10, };
            CombatantPreparer combatant4 = new CombatantPreparer(template1.Object) { InitiativeModifier = 2, InitiativeTotal = 10, };
            CombatantPreparer combatant5 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 3, };
            CombatantPreparer combatant6 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 5, };
            CombatantPreparer combatant7 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 6, };

            preparer.Combatants.Add(combatant1);
            preparer.Combatants.Add(combatant2);
            preparer.Combatants.Add(combatant3);
            preparer.Combatants.Add(combatant4);
            preparer.Combatants.Add(combatant5);
            preparer.Combatants.Add(combatant6);
            preparer.Combatants.Add(combatant7);

            preparer.ResolveInitiatives();

            Assert.That(combatant4.InitiativeOrder, Is.EqualTo(1));
            Assert.That(combatant4.InitiativeGroup, Is.EqualTo(1));

            Assert.That(combatant1.InitiativeOrder, Is.EqualTo(2));
            Assert.That(combatant1.InitiativeGroup, Is.EqualTo(2));

            Assert.That(combatant3.InitiativeOrder, Is.EqualTo(3));
            Assert.That(combatant3.InitiativeGroup, Is.EqualTo(2));

            Assert.That(combatant7.InitiativeOrder, Is.EqualTo(4));
            Assert.That(combatant7.InitiativeGroup, Is.EqualTo(3));

            Assert.That(combatant6.InitiativeOrder, Is.EqualTo(5));
            Assert.That(combatant6.InitiativeGroup, Is.EqualTo(4));

            Assert.That(combatant5.InitiativeOrder, Is.EqualTo(6));
            Assert.That(combatant5.InitiativeGroup, Is.EqualTo(5));

            Assert.That(combatant2.InitiativeOrder, Is.EqualTo(7));
            Assert.That(combatant2.InitiativeGroup, Is.EqualTo(6));
        }

        [Test]
        public void CombatPreparer_ResolveInitiatives_ResolvesInitiativeAndStartsOrderAfterOriginalCombat()
        {
            Mock<ICombatant> c1 = new Mock<ICombatant>();
            Mock<ICombatant> c2 = new Mock<ICombatant>();

            CampaignSettings campaign = new CampaignSettings();
            ActiveCombat combat = new ActiveCombat("Test", new Mock<IXmlActiveCombatSerializer>().Object, new ICombatant[] { c1.Object, c2.Object });
            c1.SetupGet(p => p.InitiativeOrder).Returns(1);
            c2.SetupGet(p => p.InitiativeOrder).Returns(2);

            CombatPreparer preparer = new CombatPreparer(campaign, combat);

            Mock<ICombatantTemplate> template1 = new Mock<ICombatantTemplate>();

            CombatantPreparer combatant1 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 10, };
            CombatantPreparer combatant2 = new CombatantPreparer(template1.Object) { InitiativeModifier = 0, InitiativeTotal = 1, };
            CombatantPreparer combatant3 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 10, };
            CombatantPreparer combatant4 = new CombatantPreparer(template1.Object) { InitiativeModifier = 2, InitiativeTotal = 10, };
            CombatantPreparer combatant5 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 3, };
            CombatantPreparer combatant6 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 5, };
            CombatantPreparer combatant7 = new CombatantPreparer(template1.Object) { InitiativeModifier = 1, InitiativeTotal = 6, };

            preparer.Combatants.Add(combatant1);
            preparer.Combatants.Add(combatant2);
            preparer.Combatants.Add(combatant3);
            preparer.Combatants.Add(combatant4);
            preparer.Combatants.Add(combatant5);
            preparer.Combatants.Add(combatant6);
            preparer.Combatants.Add(combatant7);

            preparer.ResolveInitiatives();

            Assert.That(combatant4.InitiativeOrder, Is.EqualTo(3));
            Assert.That(combatant4.InitiativeGroup, Is.EqualTo(1));

            Assert.That(combatant1.InitiativeOrder, Is.EqualTo(4));
            Assert.That(combatant1.InitiativeGroup, Is.EqualTo(2));

            Assert.That(combatant3.InitiativeOrder, Is.EqualTo(5));
            Assert.That(combatant3.InitiativeGroup, Is.EqualTo(2));

            Assert.That(combatant7.InitiativeOrder, Is.EqualTo(6));
            Assert.That(combatant7.InitiativeGroup, Is.EqualTo(3));

            Assert.That(combatant6.InitiativeOrder, Is.EqualTo(7));
            Assert.That(combatant6.InitiativeGroup, Is.EqualTo(4));

            Assert.That(combatant5.InitiativeOrder, Is.EqualTo(8));
            Assert.That(combatant5.InitiativeGroup, Is.EqualTo(5));

            Assert.That(combatant2.InitiativeOrder, Is.EqualTo(9));
            Assert.That(combatant2.InitiativeGroup, Is.EqualTo(6));
        }
        #endregion
        #region Combatant Preparer
        [Test]
        public void CombatantPreparer_Initiative_UpdatesInitiativeWhenRollAndModifierChanged()
        {
            CombatantPreparer prep = new CombatantPreparer(new Mock<ICombatantTemplate>().Object);

            Assert.That(prep.InitiativeTotal, Is.EqualTo(0), "Default should be a 0 initiative total.");

            prep.InitiativeModifier = 5;
            Assert.That(prep.InitiativeTotal, Is.EqualTo(5), "Updating modifier didn't update initiative total.");

            prep.InitiativeRoll = 3;
            Assert.That(prep.InitiativeTotal, Is.EqualTo(8), "Updating roll didn't update initiative total.");

            prep.InitiativeTotal = 6;
            Assert.That(prep.InitiativeRoll, Is.EqualTo(1), "Updating total didn't update initiative roll.");
        }
        #endregion
    }
}
