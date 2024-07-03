using NUnit.Framework;
using System;

public class HealthTest
{
    [Test]
    public void InitializeWithValidValues()
    {
        Assert.DoesNotThrow(() =>
        {
            Health health = new(100, 100);
        });
    }

    [Test]
    public void InitializeWithNegativeHPThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Health health = new(-50, 100);
        });
    }

    [Test]
    public void InitializeWithMaxHPLessThanHPThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            Health health = new(100, 50);
        });
    }

    [Test]
    public void ApplyDamageReducesHP()
    {
        Health health = new(100, 100);
        health.ApplyDamage(50);
        Assert.AreEqual(health.HP.Value, 50);
    }

    [Test]
    public void ApplyDamageWhenDeadThrowsException()
    {
        Health health = new(0, 100);
        Assert.Throws<ArgumentException>(() =>
        {
            health.ApplyDamage(50);
        });
    }

    [Test]
    public void ApplyDamageWithNegativeAmountThrowsException()
    {
        Health health = new(100, 100);
        Assert.Throws<ArgumentException>(() =>
        {
            health.ApplyDamage(-50);
        });
    }

    [Test]
    public void HealIncreasesHP()
    {
        Health health = new(50, 100);
        health.Heal(30);
        Assert.AreEqual(health.HP.Value, 80);
    }

    [Test]
    public void HealWhenDeadThrowsException()
    {
        Health health = new(0, 100);
        Assert.Throws<ArgumentException>(() =>
        {
            health.Heal(50);
        });
    }

    [Test]
    public void HealWithNegativeAmountThrowsException()
    {
        Health health = new(100, 100);
        Assert.Throws<ArgumentException>(() =>
        {
            health.Heal(-30);
        });
    }

    [Test]
    public void SetMaxHPUpdatesMaxHP()
    {
        Health health = new(50, 100);
        health.SetMaxHP(150);
        Assert.AreEqual(health.MaxHP.Value, 150);
    }

    [Test]
    public void SetMaxHPBelowCurrentHPSetsCurrentHPToMaxHP()
    {
        Health health = new(100, 100);
        health.SetMaxHP(80);
        Assert.AreEqual(health.HP.Value, 80);
    }

    [Test]
    public void SetMaxHPWithNegativeAmountThrowsException()
    {
        Health health = new(100, 100);
        Assert.Throws<ArgumentException>(() =>
        {
            health.SetMaxHP(-50);
        });
    }

    [Test]
    public void ToStringReturnsCorrectString()
    {
        Health health = new(80, 100);
        string expected = "HP: 80/100";
        string actual = health.ToString();
        Assert.AreEqual(expected, actual);
    }
}
