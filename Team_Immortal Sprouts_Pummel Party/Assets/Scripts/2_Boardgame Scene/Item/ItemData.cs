using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int Id;
    public string Name;
    public Sprite Icon;
    public string Description;
    public bool isControllable;
    public int Weight;
    public GameObject Prefab;
}
