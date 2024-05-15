using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBase : MonoBehaviour, IFish
{


    #region Variables
    public string fishName;
    public string fishDescription;
    public int spawnChance;
    #endregion



    #region Interface

    public string FishName => fishName;

    public string FishDescription => fishDescription;

    public int SpawnChance => spawnChance;


    #endregion


}
