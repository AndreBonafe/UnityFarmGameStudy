using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeMove;
    private float timeCount;
    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount < timeMove) {
            transform.Translate(speed * Time.deltaTime * Vector2.right);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerItens>().TotalWood++;
            Destroy(gameObject);
        }
    }
}
