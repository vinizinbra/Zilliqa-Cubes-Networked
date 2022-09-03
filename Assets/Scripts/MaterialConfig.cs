using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObject/MaterialConfig")]
public class MaterialConfig : ScriptableObject
{
    public Material[] materials;
}