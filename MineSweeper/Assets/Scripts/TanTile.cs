using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TanTile : MonoBehaviour
{
    [SerializeField] Color baseColor, offsetColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject highlight;
    [SerializeField] TextMeshProUGUI numberText;
    [SerializeField] float bombCheckRadius;

    int numberOfBombs;
    int numberOfFlags;
    bool bombsAreNearby;
    bool mouseIsOnTile;
    bool foundGreenTiles = true;

    void Start()
    {
        CheckForBombs();
    }

    void CheckForBombs()
    {
        Collider2D[] bombCollisions = Physics2D.OverlapCircleAll(transform.position, bombCheckRadius); ;

        foreach (Collider2D bombsNearby in bombCollisions)
        {
            if (bombsNearby.gameObject.CompareTag("Bomb"))
            {
                numberOfBombs++;
                numberText.text = numberOfBombs.ToString();
                bombsAreNearby = true;
            }
        }
    }

    void Update()
    {
        DestroyGreenTiles();
        CheckForFlags();
    }

    void DestroyGreenTiles()
    {
        if (mouseIsOnTile && Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && numberOfBombs >= numberOfFlags)
        {
            Collider2D[] greenTileCollision = Physics2D.OverlapCircleAll(transform.position, bombCheckRadius); 

            foreach (Collider2D greenTile in greenTileCollision)
            {
                if (greenTile.gameObject.CompareTag("GreenTile"))
                {
                    Destroy(greenTile.gameObject);
                }
            }
        }

        if (!bombsAreNearby && !foundGreenTiles)
        {
            Collider2D[] greenTileCollision = Physics2D.OverlapCircleAll(transform.position, bombCheckRadius);

            foreach (Collider2D greenTile in greenTileCollision)
            {
                if (greenTile.gameObject.CompareTag("GreenTile"))
                {
                    Destroy(greenTile.gameObject);
                }
            }
        }
    }

    void CheckForFlags()
    {
        Collider2D[] flagCollisions = Physics2D.OverlapCircleAll(transform.position, bombCheckRadius); ;

        foreach (Collider2D flag in flagCollisions)
        {
            if (flag.gameObject.CompareTag("Flag"))
            {
                numberOfFlags++;
                Debug.Log("Found flags: " + numberOfFlags);
            }
        }
    }

    void OnMouseEnter()
    {
        if (bombsAreNearby)
        {
            highlight.SetActive(true);
            mouseIsOnTile = true;
        }
    }

    void OnMouseExit()
    {
        if (bombsAreNearby)
        {
            highlight.SetActive(false);
            mouseIsOnTile = false;
        }
    }

    public void Init(bool tanTileOffset)
    {
        spriteRenderer.color = tanTileOffset ? baseColor : offsetColor;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
