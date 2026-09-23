using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int myIndex;
    public FruitDatas myData;
    public bool isDropped = false;
    public int mergeCount;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private fruitTypes myType;

    private void Start()
    {
        if(myData != null) SetupFruit();
    }

    void Update()
    {
        
    }
    private void SetupFruit()
    {
        spriteRenderer.sprite = myData.sprite;
        myType = myData.fruitType;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fruit otherScript = collision.gameObject.GetComponent<Fruit>();
        if (otherScript != null)
        {
            if (otherScript.isDropped && isDropped)
            {
                int otherIndex = otherScript.myIndex;
                if (myIndex > otherIndex && myData.fruitType == otherScript.myData.fruitType)
                {
                    mergeCount += otherScript.mergeCount + 1;
                    if (mergeCount >= myData.maxMergeCount)
                    {
                        Destroy(gameObject);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        transform.localScale *= 1.2f;
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }
    


}
