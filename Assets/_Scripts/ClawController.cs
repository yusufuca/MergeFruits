using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{

    public float minX = -2.5f;
    public float maxX = 2.5f;
    [SerializeField] LineRenderer lineRenderer;
    
    void Update()
    {
        if (!GameManager.gm.isGameOver)
        {
            TouchController();
        }
    }
    private void TouchController()
    {
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            lineRenderer.enabled = true;
            Vector3 inputPosition = Input.mousePosition;
            inputPosition = Input.mousePosition;
            if(Input.touchCount > 0)
            {
                inputPosition = Input.GetTouch(0).position;
            }
            inputPosition.z = -Camera.main.transform.position.z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Clamp(worldPosition.x, minX, maxX);
            transform.position = newPosition;
        }
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            lineRenderer.enabled = false;
            GameManager.gm.DropFruit();
        }
    }
}
