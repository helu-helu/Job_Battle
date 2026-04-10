using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyGem", menuName = "Scriptable Objects/EnergyGem")]
public class EnergyGemScriptableObject : ScriptableObject
{
    public GameObject blueGemPrefab;
    public GameObject redGemPrefab;
    public GameObject greenGemPrefab;
}
