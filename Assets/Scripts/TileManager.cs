using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;
    public static TileManager Instance { get { return instance; } }

    public GameBoardManager gameBoard;
    public Tile SelectedTile;
    public Vector2 MouseStartPosition;
    public bool isSwapping = false;
    private void Awake()
	{
		if(instance==null)
		{
            instance = this;
		}
        else
		{
            Destroy(gameObject);
		}
	}
	void Start()
    {
        gameBoard = GetComponent<GameBoardManager>();
    }

    void Update()
    {
        if(SelectedTile && isSwapping)
        UpdateSelectedTile();
    }


    public void UpdateSelectedTile()
	{
        Vector2 mouseDirection =(Vector2) Input.mousePosition - MouseStartPosition;
        Vector2 mouseDirectionNormalized = mouseDirection.normalized;
        Vector2 AbsoluteValDirection = new Vector2(Mathf.Abs(mouseDirectionNormalized.x), Mathf.Abs(mouseDirectionNormalized.y));

        Vector2 MoveToDirection = Vector2.zero;
        if(mouseDirection.magnitude>gameBoard.offset.x)
		{
            if(AbsoluteValDirection.x>AbsoluteValDirection.y)
			{
                MoveToDirection = new Vector2(0,(mouseDirectionNormalized.x>0 )? 1:-1);
			}
            else if (AbsoluteValDirection.y > AbsoluteValDirection.x)
            {
                MoveToDirection = new Vector2(  (mouseDirectionNormalized.y > 0) ?-1 : 1,0);
            }
        }
        if(MoveToDirection.magnitude>0)
		{
            
            Vector2 ind = MoveToDirection+ SelectedTile.indecies;
         
            if (ind.x >= 0 && ind.x < gameBoard.NumberOfRows && ind.y >= 0 && ind.y < gameBoard.NumberOfColumns)
            {
                Vector2 destination = gameBoard.GetPositionByIndex(ind);
                SelectedTile.SetTarget(destination, ind);
            }
		}
	}

    public void SelectTile(Tile tile)
	{
        if (SelectedTile) return;
        SelectedTile = tile;
        MouseStartPosition = (Vector2)Input.mousePosition;
	}

    public void DropTile(Tile tile)
	{
        if (!SelectedTile || SelectedTile!=tile) return;
        isSwapping = true;
	}
}
