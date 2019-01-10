using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraveyardButton : MonoBehaviour
{
    public GameManager manager;
    private bool graveyardShowed = false;
    private RawImage arrow;

    public void Start()
    {
        arrow = transform.Find("Button").transform.Find("Arrow").GetComponent<RawImage>();
    }

    public void OnClick()
    {
        if (graveyardShowed)
            StartCoroutine(HideGraveyard());
        else
            StartCoroutine(ShowGraveyard());
    }
    public IEnumerator ShowGraveyard()
    {
        graveyardShowed = true;
        for (int i=0;i<10;i++)
        {
            foreach (GameObject obj in manager.GetPlayer().graveyard)
            {
                obj.transform.position += new Vector3(-0.3f, 0, 0);
            }
            arrow.transform.Rotate(new Vector3(0, 0, 18), Space.Self);
            yield return new WaitForSeconds(0);
        }
    }
    public IEnumerator HideGraveyard()
    {
        graveyardShowed = false;
        for (int i = 0; i < 10; i++)
        {
            foreach (GameObject obj in manager.GetPlayer().graveyard)
            {
                obj.transform.position -= new Vector3(-0.3f, 0, 0);
            }
            arrow.transform.Rotate(new Vector3(0, 0, 18), Space.Self);
            yield return new WaitForSeconds(0);
        }
    }
}
