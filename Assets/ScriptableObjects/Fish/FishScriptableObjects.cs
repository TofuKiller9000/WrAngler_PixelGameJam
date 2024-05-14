using UnityEngine;


[CreateAssetMenu(fileName = "FishName", menuName = "ScriptableObjects/Fish", order = 1)]

public class FishScriptableObjects : ScriptableObject
{

    public string fishName;

    public string fishDescription;

    public float spawnChance; 

    [Space]

    public GameObject fishPrefab;



}
