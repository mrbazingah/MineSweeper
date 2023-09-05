using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GreenTile : MonoBehaviour
{
    [SerializeField] Color baseColor, offsetColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject flag;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float randomFactor;

    bool hasBomb;
    bool hasFlag;
    bool mouseIsOnTile;
    bool canPlay;

    GameSession gameSession;

    void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();

        canPlay = true;

        int bombCanSpawn = (int)Random.Range(0, randomFactor);

        if (bombCanSpawn == 0)
        {
            Instantiate(bombPrefab, new Vector3(transform.position.x, transform.position.y, 0.5f), Quaternion.identity);
            hasBomb = true;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            canPlay = false;
        }

        if (mouseIsOnTile && Input.GetKeyDown(KeyCode.Mouse1) && canPlay)
        {
            if (!hasFlag)
            {
                flag.SetActive(true);
                hasFlag = true;

                if (hasBomb)
                {
                    gameSession.AddFlaggedBombs();
                }
            }

            else if (hasFlag)
            {
                flag.SetActive(false);
                hasFlag = false;
                
                if (hasBomb)
                {
                    gameSession.SubtractFlaggedBombs();
                }
            }
        }
    }

    public void Init(bool greenTileIsOffset)
    {
        spriteRenderer.color = greenTileIsOffset ? baseColor : offsetColor;
    }

  

    void OnMouseEnter()
    {
        if (canPlay)
        {
            highlight.SetActive(true);

            mouseIsOnTile = true;
        }
    }

    void OnMouseExit()
    {
        if (canPlay)
        {
            highlight.SetActive(false);

            mouseIsOnTile = false;
        }
    }

    void OnMouseDown()
    {
        if (!hasFlag && canPlay)
        {
            Destroy(gameObject);

            if (hasBomb)
            {
                gameSession.Lose();

                Debug.Log("You Lost");
            }
        }
    }
}
