using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RPG.Core;
using UnityEngine;
using UnityEngine.TestTools;

public class RandomLootEditTests
{
    /*
    RandomLoot spawner;
    Transform myTransform;

    [SetUp]
    public void SetUp()
    {
        spawner = new GameObject().AddComponent<RandomLoot>();
        myTransform = spawner.transform;
    }

    [Test]
    public void RandomLootShouldThrowExceptionOnSpawningWithoutAssigningItem()
    {
        Assert.Throws<NullReferenceException>( () => spawner.Spawn(myTransform));
    }

    [Test]
    public void RandomLootShouldSpawn()
    {
        spawner.loots = new RandomLoot.LootItem[1];
        RandomLoot.LootItem item = new RandomLoot.LootItem();
        GameObject go = new GameObject();
        item.lootObject = go;
        item.chance = 100;
        spawner.loots[0] = item;
        spawner.Spawn(myTransform);
    }

    [Test]
    public void RandomLootShouldSpawnOneOfAssignedItems()
    {
        spawner.loots = new RandomLoot.LootItem[2];
        RandomLoot.LootItem item1 = new RandomLoot.LootItem();
        RandomLoot.LootItem item2 = new RandomLoot.LootItem();
        GameObject go1 = new GameObject();
        GameObject go2 = new GameObject();
        item1.lootObject = go1;
        item2.lootObject = go2;
        item1.chance = 40;
        item2.chance = 60;
        spawner.loots[0] = item1;
        spawner.loots[1] = item2;
        spawner.Spawn(myTransform);
    }
    */
}
