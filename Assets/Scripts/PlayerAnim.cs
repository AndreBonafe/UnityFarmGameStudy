using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;
    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       onMove();
       onRun();
       onUseTools();
    //    onRoll();
    }

    #region Movement

    void onMove()
    {
        if (player.direction.sqrMagnitude > 0 && !player.IsInBed)
        {
            if (player.isRolling)
            {
                anim.SetBool("activeRoll", true);
            }
            else
            {
                anim.SetBool("activeRoll", false);
                anim.SetInteger("transition", 1);
            }
        }
        else
        {
            anim.SetInteger("transition", 0);
        }
        if (player.direction.x > 0)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        else if (player.direction.x < 0)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        
    }

    void onUseTools()
    {
        if (player.IsCutting && player.direction.sqrMagnitude == 0)
        {
            anim.SetInteger("transition", 3);
        }
        if (player.IsDigging && player.direction.sqrMagnitude == 0)
        {
            anim.SetInteger("transition", 4);
        }
        if (player.IsWatering && player.direction.sqrMagnitude == 0)
        {
            anim.SetInteger("transition", 5);
        }
        if (player.IsFillingWateringCan && player.direction.sqrMagnitude == 0)
        {
            anim.SetBool("fillWaterCan", true);
        }
        if (!player.IsFillingWateringCan)
        {
            anim.SetBool("fillWaterCan", false);
        }
        if (player.IsEmptyCan && player.direction.sqrMagnitude == 0)
        {
            anim.SetBool("emptyCan", true);
        }
        if (!player.IsEmptyCan)
        {
            anim.SetBool("emptyCan", false);
        }
        if (player.IsPlanting)
        {
            anim.SetBool("planting", true);
        }
        if (!player.IsPlanting)
        {
            anim.SetBool("planting", false);
        }
        if (player.IsInteracting)
        {
            anim.SetBool("isInteracting", true);
        }
        if (!player.IsInteracting)
        {
            anim.SetBool("isInteracting", false);
        }
        if (player.StartedFishing && !player.IsNextToWaterFront)
        {
            anim.SetTrigger("startFishing");
        }
        if (!player.StartedFishing && !player.IsNextToWaterFront)
        {
            anim.ResetTrigger("startFishing");
        }
        if (player.StartedFishing && player.IsNextToWaterFront)
        {
            anim.SetTrigger("startFishingFront");
        }
        if (!player.StartedFishing && player.IsNextToWaterFront)
        {
            anim.ResetTrigger("startFishingFront");
        }
        if (player.IsFishing)
        {
            anim.SetInteger("transition", 6);
        }
        if (player.Caught)
        {
            anim.SetBool("pulling", true);
        }
        else
        {
            anim.SetBool("pulling", false);
        }
        if (player.IsPullingFish)
        {
            anim.SetBool("caught", true);
        }
        else
        {
            anim.SetBool("caught", false);
        }
    }
    void onRun()
    {

        if (player.isRunning && player.direction.sqrMagnitude > 0)
        {
            anim.SetInteger("transition", 2);
        }

    }

    // void onRoll()
    // {
    //     AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

    //     if(animState.IsName("Roll"))
    //     {
    //         Debug.Log("rolling");
    //     }
    // }

    #endregion
}
