using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tile : MonoBehaviour
{
    public TileType type;
    public Vector2 indecies;
    public Image tileIcon;

    public Sprite[] sprites;
    public RectTransform rect;

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
        
    }

    public void FillTile(int value, Vector2 pos, Vector2 ind)
	{
        type = (TileType)value;
        tileIcon.sprite = sprites[value];
        rect.anchoredPosition = pos;
        indecies = ind;
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