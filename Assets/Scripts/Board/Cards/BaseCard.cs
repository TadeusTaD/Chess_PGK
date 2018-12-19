using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardMode { small, enlarging, big, downscaling, onEnterBlur, selectedBlur};
public abstract class BaseCard : MonoBehaviour {

    private CardMode mode;

    [Range(1, 20)]
    public int manaCost;
    protected GameManager manager;
    public Text manaCostIndicator;
    public Text description;
    protected List<GameObject> indicators = new List<GameObject>();
    public RawImage blur;

    private float enlargeScale = 0;
    private float time = 0;
    private bool chooseCardMoment=true;
    private bool isSelected = false;

    private Color redColor = new Color(255f / 255, 0f / 255, 0f / 255);
    private Color greenColor = new Color(0f / 255, 255f / 255, 0f / 255);
    private Color yellowColor = new Color(255f / 255, 255f / 255, 0f / 255);
    private Color whiteColor = new Color(255f / 255, 255f / 255, 255f / 255);
    private Color positiveColor;
    private Color onMouseColor;
    private Color selectedColor;
    private Color negativeColor;

    public void Awake()
    {
        positiveColor = greenColor;
        negativeColor = redColor;
        onMouseColor = whiteColor;
        selectedColor = yellowColor;
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        if (blur != null && mode!=CardMode.onEnterBlur && mode != CardMode.selectedBlur)
        {
            if (manaCost <= manager.GetPlayer().manaPool)
            {
                //blur.color = new Color(0f / 255, 255f / 255, 0f / 255, (0.75f + (Mathf.Sin(time) / 4)));
                blur.color = new Color(positiveColor.r, positiveColor.g, positiveColor.b, (0.75f + (Mathf.Sin(time) / 4)));
            }
            else
            {
                //blur.color = new Color(255f / 255, 0f / 255, 0f / 255, (0.75f + (Mathf.Sin(time) / 4)));
                blur.color = new Color(negativeColor.r, negativeColor.g, negativeColor.b, (0.75f + (Mathf.Sin(time) / 4)));
            }
        }
        else if(blur != null && mode == CardMode.onEnterBlur)
        {
                 blur.color = new Color(onMouseColor.r, onMouseColor.g, onMouseColor.b, (0.75f + (Mathf.Sin(time) / 4)));
        }
        else
        {
                blur.color = new Color(selectedColor.r, selectedColor.g, selectedColor.b, (0.75f + (Mathf.Sin(time) / 4)));
        }

        if (time <= 314f)
            time += 0.05f;
        else
            time -= 0.05f;
    }

    public void OnMouseEnter()
    {
        if (manager.gameMode == Mode.blocked)
        {
            mode = CardMode.onEnterBlur;
            Debug.Log("ZMIEN KOLOR NA BIAŁY!");
        }
        else
        {
            print("Weszlo");
            mode = CardMode.enlarging;
            StartCoroutine(Enlarge());
        }

    }

    public void OnMouseDown()
    {
        if (!isSelected && manager.gameMode == Mode.blocked)
        {
            mode = CardMode.selectedBlur;
            manager.GetComponent<GameManager>().selectedCards.Add(this.gameObject);
            Debug.Log("ZMIEN KOLOR NA ZOLTY!");
            isSelected = true;
        }
        else
        {
            mode = CardMode.small;
            manager.GetComponent<GameManager>().selectedCards.Remove(this.gameObject);
            Debug.Log("ZMIEN KOLOR ZNOWU NA ZIELONY!");
            isSelected = false;
        }
    }


    public void OnMouseExit()
    {
        if (!isSelected)
        {
            print("Opuszczono");
            mode = CardMode.downscaling;
            StartCoroutine(Downscale());
        }
        else if (isSelected)
        {
            mode = CardMode.selectedBlur;
            isSelected = true;
        }
    }
    public void ActivateColor()
    {
        positiveColor = yellowColor;
    }
    public void DeactivateColor()
    {
        positiveColor = greenColor;
    }

    public IEnumerator Enlarge()
    {
        while (mode == CardMode.enlarging && enlargeScale <= 1f)
        {
            enlargeScale += 0.1f;
            this.transform.Find("Canvas").transform.localScale += new Vector3(0.2f, 0.2f, 0);
            this.transform.Find("Canvas").transform.localPosition += new Vector3(0, 0.1f, -1f);
            yield return new WaitForSeconds(0);
        }
    }
    public IEnumerator Downscale()
    {
        while (mode == CardMode.downscaling && enlargeScale > 0.1f)
        {
            enlargeScale -= 0.1f;
            this.transform.Find("Canvas").transform.localScale -= new Vector3(0.2f, 0.2f, 0);
            this.transform.Find("Canvas").transform.localPosition -= new Vector3(0, 0.1f, -1f);
            yield return new WaitForSeconds(0);
        }


    }



    public abstract void ActivateEffect(GameManager manager);
    public void MoveToGraveyard()
    {
        manager.GetPlayer().hand.Remove(this.gameObject);
        this.gameObject.SetActive(false);
        manager.GetPlayer().RenderHand();
    }
    public GameObject Copy()
    {
        System.Type type = this.GetType();
        BaseCard card = Instantiate(this);
        card.gameObject.SetActive(false);

        return card.gameObject;   
    }
    public virtual void ClearIndicator()
    {
        try
        {
            foreach (GameObject obj in indicators)
                Destroy(obj);
        }
        catch (System.NullReferenceException) { }
    }

    public abstract void CancelCardUse();
    public void CancelCardUse(RaycastHit hit)
    {
        CancelCardUse();
    }


    // Ustawianie delegat
    public void DelegateCancelAdd()
    {
        ActivateColor();

        manager.onVoidHit += CancelCardUse;
        manager.onCardHit += CancelCardUse;
    }
    public void DelegateCancelClear()
    {
        DeactivateColor();

        manager.onVoidHit -= CancelCardUse;
        manager.onCardHit -= CancelCardUse;
    }
}
