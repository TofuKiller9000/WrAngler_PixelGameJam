using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFish
{
    //public string fishName;

    //public string fishDescription;

    //public float spawnChance;

    //[Space]

    //public GameObject fishPrefab;

    string FishName { get; }

    string FishDescription { get; }

    int SpawnChance { get; }
}