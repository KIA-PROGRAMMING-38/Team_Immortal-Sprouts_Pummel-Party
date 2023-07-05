using UnityEngine;

[CreateAssetMenu]
public class FakeItemData : ScriptableObject
{
    private readonly static int numberOfItems = 3;

    public int[] itemIDs = new int[numberOfItems];  
    public string[] itemNames = new string[numberOfItems];
    public Sprite[] Icons = new Sprite[numberOfItems]; 
    public string[] descriptions = new string[numberOfItems];
    public bool[] isControllable = new bool[numberOfItems];
    public RealItem[] prefabs = new RealItem[numberOfItems];   

    public int GetItemCount() => numberOfItems;    
    public int GetItemID(int index) => itemIDs[index];
    public string GetItemName(int index) => itemNames[index];   
    public Sprite GetItemIcon(int index) => Icons[index];
    public string GetItemDescription(int index) => descriptions[index];

    public bool GetItemIsControllable(int index) => isControllable[index];  

    public RealItem GetItemPrefab(int index) => prefabs[index];

}
