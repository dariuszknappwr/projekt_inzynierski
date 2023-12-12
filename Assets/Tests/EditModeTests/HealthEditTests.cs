using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RPG.Core;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthEditTests
{
    /*
    Health health;
    float maxHealthPoints = 100;

    [SetUp]
    public void SetUp()
    {
        health = new Health(maxHealthPoints);
    }

    [Test]
    [TestCase(60, 40)]
    [TestCase(100, 0)]
    [TestCase(150, 0)]
    public void HealthShouldTakePositiveDamage(float damage, float expectedHealth)
    {
        health.TakeDamage(damage);
        Assert.AreEqual(expectedHealth, health.HealthPoints);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(-1000)]
    public void HealthShouldThrowExceptionOnNegativeDamage(float damage)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(damage));
    }

    [Test]
    [TestCase(0, 10, 100)]
    [TestCase(60, 10, 50)]
    [TestCase(10, 50, 100)]
    [TestCase(100, 100, 0)]
    public void HealthShouldHealPositiveAmount(float damage, float healAmount, float expectedHealth)
    {
        health.TakeDamage(damage);
        health.Heal(healAmount);
        Assert.AreEqual(expectedHealth, health.HealthPoints);
    }

    [Test]
    [TestCase(-1)]
    [TestCase(-10)]
    [TestCase(-1000)]
    public void HealthShouldThrowExceptionOnNegativeHealAmount(float healAmount)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => health.Heal(healAmount));
    }

    [Test]
    [TestCase(10, 0, false)]
    [TestCase(60, 10, false)]
    [TestCase(60, 100, false)]
    [TestCase(100, 0, true)]
    [TestCase(100, 10, true)]
    [TestCase(100, 100, true)]
    public void CharacterDiesAndStaysDead(float damage, float healAmount, bool expectedIsDead)
    {
        health.TakeDamage(damage);
        health.Heal(healAmount);
        Assert.AreEqual(expectedIsDead, health.IsDead);
    }
    */
}
