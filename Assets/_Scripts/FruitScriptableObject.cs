using UnityEngine;

public enum fruitTypes
{
    Strawberry,
    Banana,
    Grape,
    Orange,
    Apple,
    Watermelon
}

[CreateAssetMenu(fileName = "Fruits", menuName = "Scriptable Objects/Fruit")]
public class FruitDatas : ScriptableObject
{
    public fruitTypes fruitType;
    public FruitDatas toEvolve;
    public Vector3 intialScale;
    public Sprite sprite;
    public Sprite explosionSprite;
    public int maxMergeCount;
    public int scoreValue;
}