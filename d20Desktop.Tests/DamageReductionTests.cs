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
    public class DamageReductionTests
    {
        [Test]
        public void DamageReduction_Parse_ParsesStrings()
        {
            DamageReduction[] dr = DamageReduction.Parse("Something; DR 5/Type; DR 10/Type1 and Type2;Damage Reduction 5/Type1 or Type2;Something Else;10/Magic;DR 5/-")
                .ToArray();

            Assert.That(dr.Length, Is.EqualTo(5));

            //  Test the first one
            Assert.That(dr[0].Amount, Is.EqualTo(5));
            Assert.That(dr[0].Types.Count, Is.EqualTo(1));
            Assert.That(dr[0].Types[0], Is.EqualTo("Type"));

            //  Test the second one
            Assert.That(dr[1].Amount, Is.EqualTo(10));
            Assert.That(dr[1].Types.Count, Is.EqualTo(2));
            Assert.IsTrue(dr[1].RequiresAllTypes);
            Assert.That(dr[1].Types[0], Is.EqualTo("Type1"));
            Assert.That(dr[1].Types[1], Is.EqualTo("Type2"));

            //  Test the third one
            Assert.That(dr[2].Amount, Is.EqualTo(5));
            Assert.That(dr[2].Types.Count, Is.EqualTo(2));
            Assert.IsFalse(dr[2].RequiresAllTypes);
            Assert.That(dr[2].Types[0], Is.EqualTo("Type1"));
            Assert.That(dr[2].Types[1], Is.EqualTo("Type2"));

            //  Test the last one
            Assert.That(dr[3].Amount, Is.EqualTo(10));
            Assert.That(dr[3].Types.Count, Is.EqualTo(1));
            Assert.That(dr[3].Types[0], Is.EqualTo("Magic"));

            Assert.That(dr[4].Amount, Is.EqualTo(5));
            Assert.That(dr[4].Types.Count, Is.EqualTo(0));
        }

        [Test]
        public void DamageReduction_Apply_AppliesMultipleDamageReductions()
        {
            DamageReduction[] dr = DamageReduction.Parse("DR 10/Adamantine; DR 5/Bludgeoning")
                .ToArray();

            int amount = dr.Apply(15);
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Adamantine");
            Assert.That(amount, Is.EqualTo(10));

            amount = dr.Apply(15, "Bludgeoning");
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Adamantine", "Bludgeoning");
            Assert.That(amount, Is.EqualTo(15));
        }
        [Test]
        public void DamageReduction_Apply_AppliesCompoundAndDamageReduction()
        {
            DamageReduction[] dr = DamageReduction.Parse("DR 10/Adamantine and Bludgeoning")
                .ToArray();

            int amount = dr.Apply(15);
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Adamantine");
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Bludgeoning");
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Adamantine", "Bludgeoning");
            Assert.That(amount, Is.EqualTo(15));
        }
        [Test]
        public void DamageReduction_Apply_AppliesCompoundOrDamageReduction()
        {
            DamageReduction[] dr = DamageReduction.Parse("DR 10/Adamantine or Bludgeoning")
                .ToArray();

            int amount = dr.Apply(15);
            Assert.That(amount, Is.EqualTo(5));

            amount = dr.Apply(15, "Adamantine");
            Assert.That(amount, Is.EqualTo(15));

            amount = dr.Apply(15, "Bludgeoning");
            Assert.That(amount, Is.EqualTo(15));

            amount = dr.Apply(15, "Adamantine", "Bludgeoning");
            Assert.That(amount, Is.EqualTo(15));
        }
    }
}
