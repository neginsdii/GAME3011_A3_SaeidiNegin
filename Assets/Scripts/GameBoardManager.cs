using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameBoardManager : MonoBehaviour
{
	[Header("Tile Properties")]
	public Tile[,] tiles;
	[SerializeField]
	private int NumberOfRows;
	[SerializeField]
	private int NumberOfColumns;
	[SerializeField]
	private Vector2 offset;


	[Header("References to Game Objects")]
	[SerializeField]
	private Tile tilePrefab;
	[SerializeField]
	private GameObject panel;

	[Header("Difficulty properties")]
	[SerializeField]
	private int numberOfMatches;

	System.Random random = new System.Random();
	List<Vector2> AllMatchedTiles = new List<Vector2>();

	private void Start()
	{
		tiles = new Tile[NumberOfRows, NumberOfColumns];
		GenerateBoard();
	}

	private void Update()
	{
		
	}
	private void GenerateBoard()
	{
		Vector2 pos = Vector2.zero;
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				tiles[i, j] = Instantiate(tilePrefab, panel.transform);
				tiles[i, j].FillTile(random.Next(0, (int)(TileType.Count) - 2), new Vector2(pos.x + j * 64 +offset.x, pos.y +offset.y),new Vector2(i,j));
			}
			pos.y -= 64;
		}

		CheckMatchTile();
	}
	public void CheckMatchTile()
	{
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				if(isMatched((int)tiles[i, j].type, tiles[i, j].indecies))
				{
					List<int> types = new List<int>();

					createListOfTilesTypes(ref types, (int)tiles[i, j].type);
					int ind = random.Next(0, types.Count - 1);
					tiles[i, j].FillTile(types[ind], tiles[i, j].rect.anchoredPosition,new Vector2(i,j)) ;
				}
			}
		}
	}


	private bool isMatched( int val, Vector2 ind)
	{
		AllMatchedTiles.Clear();

		Vector2[] neighbours = { new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0,-1) };

		for (int i = 0; i < neighbours.Length; i++)
		{
			List<Vector2> matchedTile = new List<Vector2>();

			for (int j = 1; j < numberOfMatches; j++)
			{
				Vector2 indecies = ind + neighbours[i] * j;
				if ( GetTileValue(indecies)== val)
				{
					matchedTile.Add(indecies);
					
				}
				else
				{
					break;
				}
			}
			if(matchedTile.Count>1)
			{
				AddMatchTiles(matchedTile);
			}
			
		}
		for (int i = 0; i < 2; i++)//is in the middle
		{
			List<Vector2> matchedTile = new List<Vector2>();
			Vector2[] neighbours1 = { ind + neighbours[i], ind + neighbours[i + 2] };
			for (int j = 0; j < neighbours1.Length; j++)
			{
				if(GetTileValue(neighbours1[j])==val)
				{
					matchedTile.Add(neighbours[j]);
				}
			}
			if(matchedTile.Count>1)
			{
				AddMatchTiles(matchedTile);
			}
		}

		if (AllMatchedTiles.Count > 0)
		{
			AllMatchedTiles.Add(ind);
			return true;
		}
		else
		return false;
	}

	int GetTileValue(Vector2 ind)
	{
		if((int)ind.x>=0 && (int)ind.x < NumberOfRows && (int)ind.y >= 0 && (int)ind.y < NumberOfColumns)
		return (int)tiles[(int)ind.x, (int)ind.y].type;
		else
		return -1;
	}

	
	public void AddMatchTiles(List<Vector2> matchedNeighbours)
	{
		for (int i = 0; i < matchedNeighbours.Count; i++)
		{
			bool existed = false;
			for (int j = 0; j < AllMatchedTiles.Count; j++)
			{
				if(AllMatchedTiles[j].Equals(matchedNeighbours[i]))
				{
					existed = true;
					break;
				}
			}
			if (!existed)
				AllMatchedTiles.Add(matchedNeighbours[i]);
		}
	}

	void createListOfTilesTypes(ref List<int> types, int val)
	{
		for (int i = 0; i <(int) TileType.Count-2; i++)
		{
			if(i!=val)
			types.Add(i);
		}
	}
}