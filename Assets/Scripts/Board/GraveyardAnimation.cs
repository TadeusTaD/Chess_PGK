using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardAnimation : MonoBehaviour
{

    public Animator anim;
    public Light light;

    private bool isClicked;
    public BoxCollider2D collider1;
    public BoxCollider2D collider2;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isClicked = false;
        collider1.enabled = true;
        collider2.enabled = false;
    }

    void OnMouseEnter()
    {
        light.GetComponent<Light>().intensity=2.4f;
    }

    void OnMouseExit()
    {
        light.GetComponent<Light>().intensity = 0.3f;
    }

    void OnMouseDown()
    {
        if (isClicked)
        {
            isClicked = false;
            collider1.enabled = true;
            collider2.enabled = false;
            anim.SetBool("checkGrave", false);
        }

        else
        {
            isClicked = true;
            collider1.enabled = false;
            collider2.enabled = true;
            anim.SetBool("checkGrave", true);

        }
    }
}
