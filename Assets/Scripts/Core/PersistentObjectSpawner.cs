using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{


    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistenObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistenObjects();
            hasSpawned = true;
        }

        private void SpawnPersistenObjects()
        {
            GameObject persistentObject = Instantiate(persistenObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}