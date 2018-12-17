﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool isWhite;
    public Text manaIndicator;
    private int _maxMana = 1;
    public int maxMana
    {
        get
        {
            return _maxMana;
        }
        set
        {
            if (value >= 15)
                _maxMana = 15;
            else
                _maxMana = value;
        }
    }
    private int _manaPool;
    public int manaPool
    {
        get
        {
            return _manaPool;
        }
        set
        {
            _manaPool = value;
            UpdateIndicator();
        }
    }

    private List<GameObject> cardPrefabs = new List<GameObject>();
    public List<GameObject> deck = new List<GameObject>();
    public List<GameObject> hand = new List<GameObject>();
    public List<GameObject> graveyard = new List<GameObject>();

    public GameManager manager;
    public GameObject mulligan;

    public Player()
    {
        RefillManaPoints();
    }
    public void PrepareCards(List<GameObject> cardPrefabs)
    {
        this.cardPrefabs = cardPrefabs;
        DerenderHand();
        DeckDestroyer();
        deck.Clear();
        hand.Clear();
        graveyard.Clear();

        for (int i = 0; i < 100; i++) 
        {
            deck.Add(cardPrefabs[Random.Range(0, cardPrefabs.Count)].GetComponent<BaseCard>().Copy());
            //deck[i].transform.SetParent(GameObject.Find("Camera Rotation Point").transform, false);
        }

        for (int i = 0; i < 5; i++)
            DrawCard();
    }
    public void RenderHand()
    {
        DerenderHand();
        float offset = 4f / (hand.Count - 1);

        foreach (GameObject obj in hand)
        {
            if (isWhite == true)
                obj.transform.localPosition = new Vector3(8.5f + offset * hand.IndexOf(obj), 0.5f, -(float)hand.IndexOf(obj) / 100);
            else
            {
                obj.transform.localPosition = new Vector3(-1.5f - offset * hand.IndexOf(obj), 6.5f, -(float)hand.IndexOf(obj) / 100);
                obj.transform.rotation = new Quaternion(0, 0, 180, 0);
            }

            obj.gameObject.SetActive(true);
        }

    }
    public void DerenderHand()
    {
        foreach (GameObject obj in hand)
        {
            //obj.transform.localPosition += new Vector3(100, 100, 1);
            //obj.SetActive(false);
        }
    }
    private IEnumerator ShowMulliganWindow()
    {
        mulligan.GetComponentInChildren<Text>().text = "Zachowac karty czy dobrac nowe?";
        for (int i=0;i<10;i++)
        {
            mulligan.transform.localPosition += new Vector3(0, 0.5f, 0);
            yield return new WaitForSeconds(0);
        }
    }
    public void MulliganDrawButton()
    {
        DerenderHand();
        PrepareCards(cardPrefabs);
        RenderHand();
        StartCoroutine(HideMulliganWindow());
    }
    public void MulliganStayButton()
    {
        StartCoroutine(HideMulliganWindow());
    }
    private IEnumerator HideMulliganWindow()
    {
        manager.gameMode = Mode.idle;
        mulligan.GetComponentInChildren<Text>().text = "OK, I'm out!";
        for (int i = 0; i < 10; i++)
        {
            mulligan.transform.localPosition += new Vector3(0, -0.5f, 0);
            yield return new WaitForSeconds(0);
        }
    }
    public void UpdateIndicator()
    {
        try
        {
            manaIndicator.text = _manaPool.ToString();
        } catch (System.NullReferenceException)
        { }
    }

    public void RefillManaPoints()
    {
        manaPool = maxMana;
    }
    public void OnTurnChange()
    {
        manager.GetPlayer(manager.whiteTurn).DerenderHand();
        manager.GetPlayer(!manager.whiteTurn).DerenderHand();

        if (manager.turn == 1)
        {
            manager.gameMode = Mode.blocked;
            StartCoroutine(ShowMulliganWindow());
        } else
        {
            DrawCard();
        }

        RenderHand();
    }

    public void DrawCard()
    {
        
        hand.Add(deck[0]);
        deck.Remove(deck[0]);
    }
    public void DeckDestroyer()
    {
        foreach (GameObject obj in hand)
            Destroy(obj);
        foreach (GameObject obj in deck)
            Destroy(obj);
        foreach (GameObject obj in graveyard)
            Destroy(obj);
    }
}