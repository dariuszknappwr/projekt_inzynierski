using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RPG.Combat;
using UnityEngine;
using UnityEngine.TestTools;

public class FighterEditTests
{
    /*
    Fighter fighter;
    Mock<Weapon> weaponMock;
    Transform myTransform;
    SerializedObject fighterSO;

    [SetUp]
    public void SetUp()
    {
        fighter = new GameObject().AddComponent<Fighter>();
        fighter.gameObject.AddComponent<Animator>();
        weaponMock = new Mock<Weapon>();
        fighterSO = new SerializedObject(fighter);
        myTransform = fighter.transform;
    }

    [Test]
    public void FighterShouldBeAbleToAttackAtTheStartOfTheGame()
    {
        fighter = new GameObject().AddComponent<Fighter>();
        Assert.AreEqual(Mathf.Infinity, fighter.TimeSinceLastAttack);
    }

    [Test]
    public void FighterShouldBeNotAbleToAttackimmediatelyTwiceInARow()
    {
        fighter.SetTestData(weaponMock.Object);
        fighterSO.FindProperty("timeBetweenAttacks").floatValue = 1;
        fighterSO.FindProperty("timeSinceLastAttack").floatValue = 0;
        fighterSO.ApplyModifiedProperties();
        fighter.Attack();
        Assert.AreEqual(0, fighter.TimeSinceLastAttack);
    }

    [Test]
    public void FighterShouldEquipWeapon()
    {
        fighterSO.FindProperty("leftHandTransform").objectReferenceValue = myTransform;
        fighterSO.FindProperty("rightHandTransform").objectReferenceValue = myTransform;
        fighterSO.ApplyModifiedProperties();
        weaponMock.Setup(x => (string)x.GetWeaponName()).Returns("Weapon");
        GameObject equippedPrefab = new GameObject();
        Weapon weapon = new GameObject().AddComponent<MeleeWeapon>();
        weapon.transform.SetParent(equippedPrefab.transform);
        weaponMock.Setup(x => x.EquippedPrefab).Returns(equippedPrefab);
        fighter.EquipWeapon(weaponMock.Object);
    }

    [Test]
    public void FighterShouldInvokeOnStomp()
    {
        fighter.SetTestData(weaponMock.Object);
        GameObject fighterGO = fighter.gameObject;
        GameObject stomped = new GameObject();
        fighter.InvokeOnStomp(fighterGO, stomped);
        weaponMock.Verify(x => x.ActionOnStomp(fighterGO, stomped), Times.Once());
    }

    [Test]
    [TestCase(Mathf.Infinity, 1f, true)]
    [TestCase(0.1f, 1f, false)]
    [TestCase(1f, 1f, true)]
    [TestCase(2f, 1f, true)]
    [TestCase(2f, 0f, true)]
    [TestCase(0f, 2f, false)]
    public void TimeBetweenAttacksPassedWorks(float timeSinceLastAttack, float timeBetweenAttacks, bool expected)
    {
        fighterSO.FindProperty("timeBetweenAttacks").floatValue = timeBetweenAttacks;
        fighterSO.FindProperty("timeSinceLastAttack").floatValue = timeSinceLastAttack;
        fighterSO.ApplyModifiedProperties();
        Assert.AreEqual(expected, fighter.TimeBetweenAttacksPassed());
    }

    [Test]
    [TestCase(1, 2, true)]
    [TestCase(2, 4, true)]
    [TestCase(3, 8, true)]
    [TestCase(4, 16, true)]
    [TestCase(1, 1, false)]
    [TestCase(1, 2, true)]
    [TestCase(1, 3, true)]
    [TestCase(1, 4, false)]
    [TestCase(4, 1, false)]
    public void FighterIsMatchingTargetLayers(int colliderLayer, int maskLayer, bool expected)
    {
        Collider collider = new GameObject().AddComponent<CapsuleCollider>();
        collider.gameObject.layer = colliderLayer;
        Assert.AreEqual(expected, Fighter.IsMatchingTargetLayers(collider, maskLayer));
    }
    */
}
