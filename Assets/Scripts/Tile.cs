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
    public Image tileImage;

    public Sprite[] sprites;
    public RectTransform rect;

    public Vector2 targetPosition =Vector2.one *-1;
    public Vector2 targetIndecies = Vector2.one*-1;

    public Vector2 resetIndecies = Vector2.one * -1;

    public bool isMoving = false;
    public bool isDisabled = false;
	private void Awake()
	{
        rect = GetComponent<RectTransform>();
        tileImage = GetComponent<Image>();
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
        tileImage.enabled = true;
        tileIcon.enabled = true;
        isDisabled = false;
	}
    public void MoveTile()
	{
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPosition, Time.deltaTime * 16.0f);
        if( (targetPosition- rect.anchoredPosition).magnitude<0.1)
		{
            rect.anchoredPosition = targetPosition;
            isMoving = false;
            indecies = targetIndecies;
            TileManager.Instance.gameBoard.tiles[(int)indecies.x, (int)indecies.y] = this;
            targetPosition = Vector2.one * -1;
            targetIndecies = Vector2.one * -1;
        }
    }

    public void ResetTarget()
	{
        //targetPosition = Vector2.zero;
       // targetIndecies = indecies;
	}
    public void SetTarget(Vector2 pos, Vector2 ind)
	{
        targetPosition = pos;
        targetIndecies = ind;
        isMoving = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
        resetIndecies = indecies;
            TileManager.Instance.DropTile(this);
    }

    public void OnPointerDown(PointerEventData eventData)
	{
		if(!isMoving && !isDisabled && type!=TileType.Fence && !GameDataManager.isGameFinished)
		{
            TileManager.Instance.SelectTile(this);
		}
	}

    public void DisableTile()
	{
        tileIcon.enabled = false;
        tileImage.enabled = true;
        type = TileType.None;
        isDisabled = true;
	}
}

[System.Serializable]
public enum TileType
{
    Chicken=0,
    Elephant,
    giraffe,
    Pig,
    Monkey,
    Penguin,
    Koala,
    Rabbit,
    Sheep,
    Squirrel,
    Tiger,
    Zebra,
    Fence,
    None,
    Count
}