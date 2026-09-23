using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<GameObject>toDropFruitsQueue = new List<GameObject>();
    [SerializeField] private List<FruitDatas> queueFruits = new List<FruitDatas>();
    public int toSpawnFruitCount;
    public GameObject emptyFruitPrefab;

    private void Awake()
    {
        gm = this;
    }

    void Update()
    {
        GenerateFruitQueue();
        if (Input.GetKeyUp(KeyCode.F)) DropFruit();
    }

    public void DropFruit()
    {
        GameObject dropingFruit = toDropFruitsQueue[0];
        dropingFruit.GetComponent<Rigidbody2D>().simulated = true;
        toDropFruitsQueue.RemoveAt(0);
        Fruit script = dropingFruit.GetComponent<Fruit>();
        script.myIndex = dropingFruit.GetInstanceID();
        script.isDropped = true;
    }
    public void GenerateFruitQueue()
    {

            for (int i = toDropFruitsQueue.Count; i < toSpawnFruitCount; i++)
            {
                int rng = Random.Range(0, queueFruits.Count);
                GameObject newFruit = Instantiate(emptyFruitPrefab);
                newFruit.GetComponent<Fruit>().myData = queueFruits[rng];
                toDropFruitsQueue.Add(newFruit);
            }
    }
}
