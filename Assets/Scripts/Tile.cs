using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Tile : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public TileType type;
    public Vector2 indecies;
    public Image tileIcon;

    public Sprite[] sprites;
    public RectTransform rect;

    public Vector2 targetPosition;
    public Vector2 targetIndecies;

    public bool isMoving = false;
	private void Awake()
	{
        rect = GetComponent<RectTransform>();

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            MoveTile();
    }

    public void FillTile(int value, Vector2 pos, Vector2 ind)
	{
        type = (TileType)value;
        tileIcon.sprite = sprites[value];
        rect.anchoredPosition = pos;
        indecies = ind;
	}
    public void MoveTile()
	{
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPosition, Time.deltaTime * 16.0f);
        if( (targetPosition- rect.anchoredPosition).magnitude<0.1)
		{
            rect.anchoredPosition = targetPosition;
            isMoving = false;
            indecies = targetIndecies;
            TileManager.Instance.isSwapping=false;
            TileManager.Instance.SelectedTile = null;
        }
	}
    public void ResetTarget()
	{
        targetPosition = Vector2.zero;
        targetIndecies = indecies;
	}
    public void SetTarget(Vector2 pos, Vector2 ind)
	{
        targetPosition = pos;
        targetIndecies = ind;
        isMoving = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        
            TileManager.Instance.DropTile(this);
    }

    public void OnPointerDown(PointerEventData eventData)
	{
		if(!isMoving)
		{
            TileManager.Instance.SelectTile(this);
		}
	}
}

[System.Serializable]
public enum TileType
{
    Chicken=0,
    Elephant,
    giraffe,
    Koala,
    Monkey,
    Penguin,
    Pig,
    Rabbit,
    Sheep,
    Squirrel,
    Tiger,
    Zebra,
    None,
    Count
}