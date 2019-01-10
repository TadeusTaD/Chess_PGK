using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ChessPiece : MonoBehaviour {

    private Text hpIndicator;
    private Text atIndicator;
    private int _currentX;
    private int _currentY;
    private int _hp;
    private int _attack;

    //[Header("Nazwa naglowka")]    - grupuje elementy
    //[HideInInspector]             - Ukrywa w inspektorze unity
    //[Tooltip("Poradnik")]         - dodaje opis po najechaniu myszka
    //[Space]                       - dodaje wolna linie
    //[Range(1, 10)]                - zamienia boxa na suwak

    [HideInInspector]
    public bool isWhite;
    public int currentX
    {
        get
        {
            return _currentX;
        }
        set
        {
            _currentX = value;
            //this.transform.position = new Vector2(_currentX, this.transform.position.y);
        }
    }
    public int currentY
    {
        get
        {
            return _currentY;
        }
        set
        {
            _currentY = value;
            //this.transform.position = new Vector2(this.transform.position.x, _currentY);
        }
    }
    public int hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            UpdateIndicators();
        }
    }
    public int attack
    {
        get
        {
            return _attack;
        }
        set
        {
            _attack = value;
            if (_attack < 0)
            {
                _attack = 0;
            }
            UpdateIndicators();
        }
    }
    
    public void Start()
    {
        // Get piece color basing on its start position
        isWhite = (transform.position.y < 3) ? true : false;

        // Get HP/AT indicators from Stats child
        hpIndicator = this.transform.Find("Stats").transform.Find("HP").GetComponentInChildren<Text>();
        atIndicator = this.transform.Find("Stats").transform.Find("AT").GetComponentInChildren<Text>();

        // Set start position
        currentX = (int)transform.position.x;
        currentY = (int)transform.position.y;

        // Set start stats
        int tmpHP = int.Parse(hpIndicator.text);
        int tmpAT = int.Parse(atIndicator.text);

        hp = tmpHP;
        attack = tmpAT;
    }
    public abstract List<Vector2> GetPossibleMoves(Field[,] board);
    public abstract Field GetPositionAfterAttack(Field[,] board, Field destination);
	public abstract float GetStrategicValue(Vector2Int position, int hp, int attack);

    public void UpdateIndicators()
    {
        hpIndicator.text = hp.ToString();
        atIndicator.text = attack.ToString();
    }
    
    public virtual bool DealDamageFromAttack(ChessPiece target)
    {
        return target.RecieveDamageFromAttack(this);
    }
    public virtual bool DealDamageFromEffect(ChessPiece target, int damage)
    {
        return target.RecieveDamageFromEffect(damage);
    }
    public virtual bool RecieveDamageFromAttack(ChessPiece damageDealer)
    {
        return RecieveDamage(damageDealer.attack);
    }
    public virtual bool RecieveDamageFromEffect(int damage)
    {
        return RecieveDamage(damage);
    }
    protected virtual bool RecieveDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            DestroyPiece();
            return true;
        }
        UpdateIndicators();
        return false;
    }
    public virtual void DestroyPiece()
    {
        Destroy(this.gameObject);
    }
	public virtual float GetCurrentStrategicValue()
	{
		return GetStrategicValue(new Vector2Int(currentX, currentY), hp, attack);
	}
}
