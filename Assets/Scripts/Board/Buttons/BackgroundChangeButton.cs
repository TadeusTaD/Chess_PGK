using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChangeButton : MonoBehaviour
{
    public GameObject img1;
    public GameObject img2;
    public Canvas menu;

    



    private bool menuShown = false;
    public void OnClick()
    {
        if (menuShown)
            ShowMenu();
        else
            HideMenu();

        menuShown = !menuShown;
    }
    private void ShowMenu()
    {
        menu.transform.position += new Vector3(1, 0, 0);
    }
    private void HideMenu()
    {
        menu.transform.position -= new Vector3(1, 0, 0);
    }
    public void BG1()
    {
        Vector3 pos1 = img1.transform.position;
        Vector3 pos2 = img1.transform.position;
        img1.transform.position = new Vector3(pos1.x, pos1.y, 10);
        img2.transform.position = new Vector3(pos2.x, pos2.y, 12);
    }
    public void BG2()
    {
        Vector3 pos1 = img1.transform.position;
        Vector3 pos2 = img1.transform.position;
        img1.transform.position = new Vector3(pos1.x, pos1.y, 12);
        img2.transform.position = new Vector3(pos2.x, pos2.y, 10);
    }


}
