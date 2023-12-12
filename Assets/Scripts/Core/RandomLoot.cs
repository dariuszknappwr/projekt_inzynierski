using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class RandomLoot : MonoBehaviour
    {
        [Serializable]
        public struct LootItem
        {
            public GameObject lootObject;
            [Range(0, 100)] public int chance;
        }
        public LootItem[] loots;

        public void Spawn(Transform location)
        {
            GameObject randomLoot = GetRandomLoot();
            if(randomLoot == null)
            {
                throw new NullReferenceException("You are trying to Instantiate object that is null. Assign items to array");
            }
            Instantiate(randomLoot, location.position, Quaternion.identity);
        }

        private GameObject GetRandomLoot()
        {
            int cumulativeChance = 0;
            foreach (LootItem loot in loots)
            {
                cumulativeChance += loot.chance;
            }
            int random = UnityEngine.Random.Range(0, cumulativeChance + 1);
            foreach (LootItem loot in loots)
            {
                random -= loot.chance;
                if (random <= 0)
                    return loot.lootObject;
            }
            return null;
        }
    }

}