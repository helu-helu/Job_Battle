using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyGem", menuName = "Scriptable Objects/EnergyGem")]
public class EnergyGemScriptableObject : ScriptableObject
{
    [SerializeField]public GameObject redGemPrefab;
    [SerializeField]public GameObject blueGemPrefab;
    [SerializeField]public GameObject greenGemPrefab;
}
