using Fiction.GameScreen.Combat;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiction.GameScreen.Tests
{
    [TestFixture]
    public class CombatantHealthTests
    {
        [Test]
        public void CombatantHealth_IsDead_IsTrueIfDead()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.UnconsciousAt = 0;
            health.DeadAt = -10;

            Assert.IsFalse(health.IsDead);

            health.LethalDamage = 50;
            Assert.IsFalse(health.IsDead);

            health.NonlethalDamage = 50;
            Assert.IsFalse(health.IsDead);

            health.LethalDamage = 100;
            Assert.IsFalse(health.IsDead);

            health.LethalDamage = 110;
            Assert.IsTrue(health.IsDead);

            health.TemporaryHitPoints = 20;
            Assert.IsFalse(health.IsDead);

            health.LethalDamage = 130;
            Assert.IsTrue(health.IsDead);
        }

        [Test]
        public void ComtatantHealth_IsUnconscious_IsTrueIfUnconscious()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.UnconsciousAt = 0;
            health.DeadAt = -10;

            Assert.IsFalse(health.IsUnconscious);

            health.LethalDamage = 50;
            Assert.IsFalse(health.IsUnconscious);

            health.NonlethalDamage = 50;
            Assert.IsFalse(health.IsUnconscious);

            health.NonlethalDamage = 51;
            Assert.IsTrue(health.IsUnconscious);

            health.LethalDamage = 101;
            Assert.IsTrue(health.IsUnconscious);

            health.NonlethalDamage = 0;
            Assert.IsTrue(health.IsUnconscious);

            health.TemporaryHitPoints = 20;
            Assert.IsFalse(health.IsUnconscious);

            health.LethalDamage = 121;
            Assert.IsTrue(health.IsUnconscious);

            health.LethalDamage = 131;
            Assert.IsFalse(health.IsUnconscious, "Can't be unconscious if dead.");
        }

        [Test]
        public void CombatantHealth_ApplyNonlethalDamage_AppliesNonlethalNormally()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;

            health.ApplyNonlethalDamage(10);
            Assert.That(health.LethalDamage, Is.EqualTo(0));
            Assert.That(health.NonlethalDamage, Is.EqualTo(10));

            health.ApplyNonlethalDamage(100);
            Assert.That(health.LethalDamage, Is.EqualTo(10));
            Assert.That(health.NonlethalDamage, Is.EqualTo(100));
        }
        [Test]
        public void CombatantHealth_ApplyHealing_AppliesHealingToLethalAndNonLethal()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.LethalDamage = 30;
            health.NonlethalDamage = 30;

            health.ApplyHealing(20, false);
            Assert.That(health.LethalDamage, Is.EqualTo(10));
            Assert.That(health.NonlethalDamage, Is.EqualTo(10));
        }
        [Test]
        public void CombatantHealth_ApplyHealing_OverhealCreatesTemporaryHitPoints()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.LethalDamage = 30;

            health.ApplyHealing(40, true);
            Assert.That(health.LethalDamage, Is.EqualTo(0));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(10));
        }
        [Test]
        public void CombatantHealth_ApplyDamage_LethalDamageTakesTemporaryHitPointsFirst()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.LethalDamage = 0;
            health.TemporaryHitPoints = 30;

            health.ApplyLethalDamage(10);
            Assert.That(health.LethalDamage, Is.EqualTo(0));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(20));

            health.ApplyLethalDamage(20);
            Assert.That(health.LethalDamage, Is.EqualTo(0));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(0));

            health.TemporaryHitPoints = 10;
            health.ApplyLethalDamage(15);
            Assert.That(health.LethalDamage, Is.EqualTo(5));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(0));
        }
        [Test]
        public void CombatantHealth_ApplyDamage_NonlethalDamageTakesTemporaryHitPointsFirst()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.LethalDamage = 0;
            health.TemporaryHitPoints = 30;

            health.ApplyNonlethalDamage(10);
            Assert.That(health.NonlethalDamage, Is.EqualTo(0));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(20));

            health.ApplyNonlethalDamage(20);
            Assert.That(health.NonlethalDamage, Is.EqualTo(0));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(0));

            health.TemporaryHitPoints = 10;
            health.ApplyNonlethalDamage(15);
            Assert.That(health.NonlethalDamage, Is.EqualTo(5));
            Assert.That(health.TemporaryHitPoints, Is.EqualTo(0));
        }
        [Test]
        public void CombatantHealth_ApplyDamage_ReducedByDamageReduction()
        {
            CombatantHealth health = new CombatantHealth();
            health.MaxHealth = 100;
            health.LethalDamage = 0;

            health.ApplyLethalDamage(10);
        }
    }
}
