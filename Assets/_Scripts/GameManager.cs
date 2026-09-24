using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public GameObject gameOverObject;
    public List<GameObject>toDropFruitsQueue = new List<GameObject>();
    public List<GameObject>failedFruits = new List<GameObject>();
    [SerializeField] private List<FruitDatas> queueFruits = new List<FruitDatas>();
    [SerializeField] private Transform clawArea;
    [SerializeField] private Transform queueArea;
    public TextMeshProUGUI scoreText;
    public GameObject scoreTextImage;
    public int score;
    public int toSpawnFruitCount;
    public GameObject emptyFruitPrefab;
    public Transform failLine;
    public int explosionRadius = 2;
    public int explosionForce = 2;
    public float explosionSpriteChangeTime = 0.3f;
    public float gameOverTimer;
    public LayerMask fruitLayer;
    public bool isGameOver = false;
    public Image vignette;
    Color vignetteColor;
    float currentOverTimer;
    private void Awake()
    {
        gm = this;
    }
    private void Start()
    {
        vignetteColor = GameManager.gm.vignette.color;
    }

    void Update()
    {
        if (!isGameOver)
        {
            //scoreTextImage.transform.localPosition = new Vector2(0, -150);
            scoreText.text = score.ToString();
            GenerateFruitQueue();
            if (Input.GetKeyUp(KeyCode.F) && !isGameOver) DropFruit();
            VignetteAlpha();
            vignette.color = vignetteColor;
        }
        else
        {
            scoreTextImage.transform.localPosition = new Vector2(0, 50);
            vignetteColor.a = 0;
            vignette.color = vignetteColor;
        }
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
                GameObject newFruit = Instantiate(emptyFruitPrefab,queueArea);
                newFruit.GetComponent<Fruit>().myData = queueFruits[rng];
                toDropFruitsQueue.Add(newFruit);
            }
        OrderFruits();
    }
    private void OrderFruits()
    {
        toDropFruitsQueue[0].transform.position = clawArea.transform.position;
        for(int i = 1; i < toDropFruitsQueue.Count; i++)
        {
            toDropFruitsQueue[i].transform.localPosition = new Vector2(i * -1, toDropFruitsQueue[i].transform.localPosition.y);
        }

    }
    public void RefreshButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void VignetteAlpha()
    {

        if (failedFruits.Count > 0)
        {
            currentOverTimer += Time.deltaTime;
            float alphaValue = Mathf.Clamp01(currentOverTimer / gameOverTimer);
            float velocity = 0;
            if (alphaValue > 0.3) vignetteColor.a = Mathf.SmoothDamp(vignette.color.a, alphaValue,ref velocity,0.1f);
        }
        else
        {
            currentOverTimer = 0;
            vignetteColor.a = 0;
        }
        vignette.color = vignetteColor;
    }
}
