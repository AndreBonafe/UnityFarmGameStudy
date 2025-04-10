using System.Collections;
using System.Collections.Generic;
using Assembly_CSharp.Assets.Scripts.InventorySystem;
using Assets.Scripts.InventorySystem;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float treeHealth;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem leafs;
    [SerializeField] private ParticleSystem fallingLeafsLeft;
    [SerializeField] private ParticleSystem fallingLeafsRight;
    [SerializeField] private GameItem _woodPrefab;
    private int totalDrops;
    private double directionX;
    private bool isCutted;
    public double DirectionX { get => directionX; set => directionX = value; }
    public void Start()
    {
        totalDrops = (int)Random.Range(3f, 5f);
        isCutted = false;
    }

    public void onHit()
    {
        if (treeHealth <= 0 && !isCutted)
        {   
            isCutted = true;
            if (DirectionX > 0)
            {
                anim.SetTrigger("isCutted");
                fallingLeafsLeft.Play();
            }
            else
            {
                anim.SetTrigger("isCuttedRight");
                fallingLeafsRight.Play();
            }
            StartCoroutine(InstantiateWood());
        }
        else if (!isCutted)
        {
            treeHealth--;
            leafs.Play();
            if (DirectionX > 0)
            {
                anim.SetTrigger("isCutting");
            }
            else
            {
                anim.SetTrigger("isCuttingRight");
            }
        }
    }

    private IEnumerator InstantiateWood()
    {
        yield return new WaitForSeconds(1.1f);
        if (TryGetComponent<GameItemSpawner>(out var itemSpawner))
        {
            for (int i = 0; i < totalDrops; i++)
            {
                itemSpawner.SpawnItem(_woodPrefab.Stack, (float)DirectionX * -1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Axe"))
        {
            GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
            DirectionX = player.transform.position.x - transform.position.x;
            onHit();
        }
    }
}
