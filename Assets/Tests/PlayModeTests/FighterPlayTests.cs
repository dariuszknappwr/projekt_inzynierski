using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RPG.Combat;
using UnityEngine;
using UnityEngine.TestTools;

public class FighterPlayTests
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
        fighterSO = new SerializedObject(fighter);
        fighter.gameObject.AddComponent<Animator>();
        weaponMock = new Mock<Weapon>();
        myTransform = fighter.transform;

        fighterSO.FindProperty("leftHandTransform").objectReferenceValue = myTransform;
        fighterSO.FindProperty("rightHandTransform").objectReferenceValue = myTransform;
        Weapon defaultWeapon = new GameObject().AddComponent<MeleeWeapon>();
        fighterSO.FindProperty("defaultWeapon").objectReferenceValue = defaultWeapon;
        fighterSO.ApplyModifiedProperties();
        GameObject equippedPrefab = new GameObject();
        defaultWeapon.transform.SetParent(equippedPrefab.transform);
        defaultWeapon.SetTestData(equippedPrefab);
    }

    [UnityTest]
    public IEnumerator FighterShouldEquipDefaultWeapon()
    {
        yield return null;
    }

    [UnityTest]
    public IEnumerator FighterShouldUpdateTimeSinceLastAttack()
    {
        fighter.Attack();
        yield return null;
        Assert.Greater(fighter.TimeSinceLastAttack, 0);
    }
    */
}
