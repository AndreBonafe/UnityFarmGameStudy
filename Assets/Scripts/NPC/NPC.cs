using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float speed;
    private int index;
    private Animator anim;
    public List<Transform> paths = new List<Transform>();
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(!DialogueControl.instance.IsShowing)
        {
            transform.position = Vector2.MoveTowards(transform.position, paths[index].position, speed * Time.deltaTime);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        

        if(Vector2.Distance(transform.position, paths[index].position) < 0.1f)
        {
            if(index < paths.Count -1)
            {
                index ++;
            }
            else
            {
                index = 0;
            }
        }

        Vector2 direction = paths[index].position - transform.position;

        if(direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
    }
}
