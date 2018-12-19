using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startAnimation()
    {
        anim.SetBool("animParam", true);
    }

    public void setIdleAnimation(bool state)
    {
        anim.SetBool("mulligan", state);
    }
    public void stopIdleAnimation()
    {
        anim.Play("ChooseCardIdle", 0, 0);
        anim.speed = 0;
        
    }
}
