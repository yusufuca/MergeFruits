using UnityEngine;

public enum fruitTypes
{
    Strawberry,
    Banana
}

[CreateAssetMenu(fileName = "Fruits", menuName = "Scriptable Objects/Fruit")]
public class FruitDatas : ScriptableObject
{
    public fruitTypes fruitType;
    public fruitTypes toEvolve;
    public Sprite sprite;
    public int maxMergeCount;
    public int scoreValue;
}