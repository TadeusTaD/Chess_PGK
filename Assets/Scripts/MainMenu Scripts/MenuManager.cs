using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public new Camera camera;
    public List<Transform> baseMovesImages = new List<Transform>();
    public List<Transform> specialSkillsImages = new List<Transform>();


    // Main menu functions
    public void NewBotGame()
    {
        SceneManager.LoadScene("Board");
		GameManager.gameType = Player.Type.Bot;
	}
	public void NewLocalGame()
	{
		SceneManager.LoadScene("Board");
		GameManager.gameType = Player.Type.Human;
	}
	public void EndGame()
    {
        Application.Quit();
    }

    // Tutorial functions

    // Base moves functions
    public void NextBaseMove()
    {
        int index = GetIndexOfImage(baseMovesImages);
        if (index < baseMovesImages.Count - 1)
        {
            StartCoroutine(SwapImages(index, index + 1, baseMovesImages));
        }
    } 
    public void PreviousBaseMove()
    {
        int index = GetIndexOfImage(baseMovesImages);
        if (index > 0)
        {
            StartCoroutine(SwapImages(index, index - 1, baseMovesImages));
        }
    }

    // Special skills functions
    public void NextSpecialSkill()
    {
        int index = GetIndexOfImage(specialSkillsImages);
        if (index < specialSkillsImages.Count - 1)
        {
            StartCoroutine(SwapImages(index, index + 1, specialSkillsImages));
        }
    }
    public void PreviousSpecialSkill()
    {
        int index = GetIndexOfImage(specialSkillsImages);
        if (index > 0)
        {
            StartCoroutine(SwapImages(index, index - 1, specialSkillsImages));
        }
    }


    private int GetIndexOfImage(List<Transform> list)
    {
        Transform currentImage = null;
        foreach (Transform img in list)
        {
            if (img.localPosition.x == 0)
            {
                currentImage = img;
                break;
            }
        }
        return list.IndexOf(currentImage);
    }
    private IEnumerator SwapImages(int i, int j, List<Transform> list)
    {
        Transform img1 = list[i];
        Transform img2 = list[j];
        float offset = (img1.position.x - img2.position.x) / 14;
        while (img2.localPosition.x != 0)
        {
            yield return new WaitForSeconds(0);
            img1.position += new Vector3(offset, 0, 0);
            img2.position += new Vector3(offset, 0, 0);
        }
    }

    // Change camera to Canvas functions
    public void ToMainMenu()
    {
        StartCoroutine(ChangeCameraPosition(0, 0));
    }
    public void ToTutorialMenu()
    {
        StartCoroutine(ChangeCameraPosition(-25, 0));
    }
    public void ToBaseRulesMenu()
    {
        StartCoroutine(ChangeCameraPosition(-25, 25));
    }
    public void ToSpecialSkillsMenu()
    {
        StartCoroutine(ChangeCameraPosition(-25, -25));
    }
    public void ToCardMenu()
    {
        StartCoroutine(ChangeCameraPosition(-50, 0));
    }

    private IEnumerator ChangeCameraPosition(float x, float y)
    {
        float xOffset = (x - camera.transform.position.x) / 25;
        float yOffset = (y - camera.transform.position.y) / 25;
        while (camera.transform.position.x != x || camera.transform.position.y != y)
        {
            yield return new WaitForSeconds(0);
            camera.transform.position += new Vector3(xOffset, yOffset, 0);
        }
        camera.transform.position = new Vector3(x, y, -10);
    }
}
