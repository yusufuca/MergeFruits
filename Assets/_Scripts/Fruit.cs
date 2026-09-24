using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{

    public int myIndex;
    public FruitDatas myData;
    public bool isDropped = false;
    public int mergeCount;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private fruitTypes myType;
    private float currentOverTimer = 0;

   
    private void Start()
    {
        if(myData != null) SetupFruit();
        
    }

    void Update()
    {
        GameOver();
    }
    private void SetupFruit()
    {
        spriteRenderer.sprite = myData.sprite;
        myType = myData.fruitType;
        transform.localScale = myData.intialScale;
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
                    GameManager.gm.score += myData.scoreValue;
                    if (mergeCount >= myData.maxMergeCount)
                    {
                        if (myData.toEvolve != null)
                        {
                            GameObject newFruit = Instantiate(GameManager.gm.emptyFruitPrefab,transform.position,Quaternion.identity);
                            Fruit newFruitScript = newFruit.GetComponent<Fruit>();
                            newFruitScript.myData = myData.toEvolve;
                            newFruitScript.isDropped = true;
                            newFruitScript.myIndex = newFruit.GetInstanceID();
                            newFruit.GetComponent<Rigidbody2D>().simulated = true;
                        }
                        if (GameManager.gm.failedFruits.Contains(gameObject)) GameManager.gm.failedFruits.Remove(gameObject);
                        if (GameManager.gm.failedFruits.Contains(collision.gameObject)) GameManager.gm.failedFruits.Remove(collision.gameObject);
                        Destroy(gameObject);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        transform.localScale *= 1.2f;
                        Destroy(collision.gameObject);
                        if (GameManager.gm.failedFruits.Contains(collision.gameObject)) GameManager.gm.failedFruits.Remove(collision.gameObject);
                    }
                    MergeExplosion();
                }
            }
        }
    }
    
    private void MergeExplosion()
    {
       Collider2D[] collidedFruits = Physics2D.OverlapCircleAll(transform.position, GameManager.gm.explosionRadius, GameManager.gm.fruitLayer);
        foreach(Collider2D hit in collidedFruits)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                Vector2 direction = hit.transform.position - transform.position;
                float distance = direction.magnitude;

                if (distance <= 0) distance = 0.01f;

                float forceMultiplier = 1 - (distance / GameManager.gm.explosionRadius);

                rb.AddForce(direction.normalized * GameManager.gm.explosionForce * forceMultiplier, ForceMode2D.Impulse);
                StartCoroutine(ExplosionSrpiteChangeRouitne());
            }
        }
    }
    private IEnumerator ExplosionSrpiteChangeRouitne()
    {
        spriteRenderer.sprite = myData.explosionSprite;
        yield return new WaitForSeconds(GameManager.gm.explosionSpriteChangeTime);
        spriteRenderer.sprite = myData.sprite;
    }
    private void GameOver()
    {
        if(isDropped && transform.position.y > GameManager.gm.failLine.transform.position.y)
        {
            currentOverTimer += Time.deltaTime;
            if(!GameManager.gm.failedFruits.Contains(gameObject)) GameManager.gm.failedFruits.Add(gameObject);
            if(currentOverTimer >= GameManager.gm.gameOverTimer)
            {
                Debug.LogWarning("GAMEOVER");
                GameManager.gm.gameOverObject.SetActive(true);
                GameManager.gm.isGameOver = true;
            }
        }
        else
        {
            currentOverTimer = 0;
            GameManager.gm.failedFruits.Remove(gameObject);
        }
    }

}
