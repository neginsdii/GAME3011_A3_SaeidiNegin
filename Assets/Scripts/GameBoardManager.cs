using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameBoardManager : MonoBehaviour
{
	[Header("Tile Properties")]
	public Tile[,] tiles;
	public int NumberOfRows;
	public int NumberOfColumns;
	public Vector2 offset;


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
	public List<FlippedTiles> flippedTiles = new List<FlippedTiles>();
	public List<Tile> DisableTilesToRespawn = new List<Tile>();

	public AudioSource audioSource;
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		numberOfMatches = GameDataManager.NumofMatches;
		
	}
	private void Start()
	{
		tiles = new Tile[NumberOfRows, NumberOfColumns];
		GenerateBoard();
	}

	private void Update()
	{
		CheckFlippedTiles();
		UpdateCheckMatch();
	}
	public void GenerateBoard()//generate the board
	{
		Vector2 pos = Vector2.zero;
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				

				tiles[i, j] = Instantiate(tilePrefab, panel.transform);
				int max=0;
				if (numberOfMatches == 3)
				max = (int)(TileType.Count) - (numberOfMatches + 2);
				else if(numberOfMatches == 4)
				max = (int)(TileType.Count) - (numberOfMatches+4);
				else if (numberOfMatches == 5)
				max = (int)(TileType.Count) - (numberOfMatches + 5);
				tiles[i, j].FillTile(random.Next(0, max), new Vector2(pos.x + j * 64 +offset.x, pos.y +offset.y),new Vector2(i,j));
			}
			pos.y -= 64;
		}

		CheckMatchTile();
	}

	void CheckFlippedTiles()
	{
		List<int> CheckedFlippedTiles = new List<int>();
		for (int i = 0; i < flippedTiles.Count; i++)
		{
			if (!flippedTiles[i].firstTile.isMoving && !flippedTiles[i].secondTile.isMoving)
			{
				bool Matched = false;
				if (isMatched((int)flippedTiles[i].firstTile.type, flippedTiles[i].firstTile.indecies))
				{
					foreach (Vector2 indices in AllMatchedTiles)
					{
						tiles[(int)indices.x, (int)indices.y].DisableTile();
						GameDataManager.Score += 5;
					}
					Matched = true;
				}
				if (isMatched((int)flippedTiles[i].secondTile.type, flippedTiles[i].secondTile.indecies))
				{
					foreach (Vector2 indices in AllMatchedTiles)
					{
						tiles[(int)indices.x, (int)indices.y].DisableTile();
						GameDataManager.Score += 5;

					}
					Matched = true;
				}
				if (!Matched)
				{
					flippedTiles[i].firstTile.SetTarget(flippedTiles[i].secondTile.rect.anchoredPosition, flippedTiles[i].secondTile.indecies);
					flippedTiles[i].secondTile.SetTarget(flippedTiles[i].firstTile.rect.anchoredPosition, flippedTiles[i].firstTile.indecies);

				}
				else
				{
					if (!audioSource.isPlaying)
						audioSource.Play();
				}
				CheckedFlippedTiles.Add(i);
				
			}
		}
		if (CheckedFlippedTiles.Count > 0)
		{
			AddGravityToTiles();
			
			for (int i = 0; i < CheckedFlippedTiles.Count; i++)
			{
				flippedTiles.RemoveAt(CheckedFlippedTiles[i]);
			}
		}
	}
	public void CheckMatchTile()//check in the board if we have match tiles
	{
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				if(isMatched((int)tiles[i, j].type, tiles[i, j].indecies))
				{
					Debug.Log("Matched");
					List<int> types = new List<int>();

					createListOfTilesTypes(ref types, (int)tiles[i, j].type);
					int ind = random.Next(0, types.Count - 1);
					tiles[i, j].FillTile(types[ind], tiles[i, j].rect.anchoredPosition,new Vector2(i,j)) ;
				}
			}
		}
	}

	public void UpdateCheckMatch()
	{
		bool IsUpdating = false;
		for (int j = 0; j < NumberOfColumns; j++)
		{
			for (int k = 0; k < NumberOfRows; k++)
			{
				Tile nextTile = tiles[k, j];
				if (nextTile.isMoving)
				{
					IsUpdating = true;
					break;
				}
			}
			if (IsUpdating)
				break;
		}
		bool matching = false;
		if (!IsUpdating)
		{
			for (int i = 0; i < NumberOfRows; i++)
			{
				for (int j = 0; j < NumberOfColumns; j++)
				{
					if (isMatched((int)tiles[i, j].type, tiles[i, j].indecies))
					{
						foreach (Vector2 indices in AllMatchedTiles)
						{
							tiles[(int)indices.x, (int)indices.y].DisableTile();
							GameDataManager.Score += 4;

							matching = true;
						}
					}
				}
			}
			if(matching)
			{
				AddGravityToTiles();
				if (!audioSource.isPlaying)
					audioSource.Play();
			}
		}
	}

	private bool isMatched( int val, Vector2 ind)// if a tile has matching neighbuors, change the type of the tile
	{
		AllMatchedTiles.Clear();

		Vector2[] neighbours = { new Vector2(1,0), new Vector2(0,1), new Vector2(-1,0), new Vector2(0,-1) };

		for (int i = 0; i < neighbours.Length; i++)
		{
			List<Vector2> matchedTile = new List<Vector2>();

			for (int j = 1; j < NumberOfRows; j++)
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
			if(matchedTile.Count>=numberOfMatches-1)
			{
				AddMatchTiles(matchedTile);
			}
			
		}
		for (int i = 0; i < 2; i++)//is in the middle
		{
			List<Vector2> matchedTile = new List<Vector2>();
			Vector2[] neighbours1 = { ind + neighbours[i], ind + neighbours[i] * 2, ind + neighbours[i] * 3};
			Vector2[] neighbours2 = { ind + neighbours[i + 2], ind + neighbours[i + 2] * 2 , ind + neighbours[i + 2] * 3};
			for (int j = 0; j < neighbours1.Length; j++)
			{
				if (GetTileValue(neighbours1[j]) == val)
				{
					matchedTile.Add(neighbours1[j]);
				}
				else
					break;
			}
			for (int j = 0; j < neighbours2.Length; j++)
			{
				if (GetTileValue(neighbours2[j]) == val)
				{
					matchedTile.Add(neighbours2[j]);
				}
				else
					break;
			}
			if (matchedTile.Count>=numberOfMatches-1)
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

	public Vector2 GetPositionByIndex(Vector2 ind)
	{
		Vector2 pos = Vector2.zero;
		return new Vector2(pos.x + ind.y * 64 + offset.x, pos.y + ind.x * -64 + offset.y);
		
	}

	public void AddGravityToTiles()
	{
		for (int j = 0; j < NumberOfColumns; j++)
		{
			bool hasHole = false;
			for (int k = 0; k < NumberOfRows; k++)
			{
				Tile nextTile = tiles[k, j];
				if (nextTile.type == TileType.None)
				{
					hasHole = true;
					break;
				}
			}
			if (!hasHole) continue;
			DisableTilesToRespawn.Clear();
			int p = NumberOfRows - 1;
			Tile tmp = tiles[NumberOfRows - 1, j];

			for (int next = NumberOfRows - 1; next > -1; next--)
			{
				Tile nextTile = tiles[next, j];
				DisableTilesToRespawn.Add(nextTile);

			}
			List<Tile> ToBeAdded = new List<Tile>();
			for (int n = 0; n < DisableTilesToRespawn.Count; n++)
			{
				if (DisableTilesToRespawn[n].type == TileType.None)
				{
					Tile DTile = DisableTilesToRespawn[n];
					ToBeAdded.Add(DTile);
					DisableTilesToRespawn.RemoveAt(n);
					//DisableTilesToRespawn.Add(DTile);
					n = 0;
				}
			}
			for (int i = 0; i < ToBeAdded.Count; i++)
			{
				DisableTilesToRespawn.Add(ToBeAdded[i]);
			}
			int nxt = NumberOfRows - 1;
			int stp = -1;
			int sub = 1;
			for (int n = 0; n < DisableTilesToRespawn.Count; n++)
			{
				Tile DTile = DisableTilesToRespawn[n];
				if (DisableTilesToRespawn[n].type == TileType.None)
				{
					int max=0;
					if (numberOfMatches == 3)
						max = random.Next((int)(TileType.Count) - (numberOfMatches + 2));
					else if(numberOfMatches == 4)
						max = (random.Next((int)(TileType.Count)) % 4 == 0 ? (int)(TileType.Count)-2 : random.Next((int)(TileType.Count) - (numberOfMatches + 4)));
					else if (numberOfMatches == 5)
						max = (random.Next((int)(TileType.Count)) % 5 == 0 ? (int)(TileType.Count) - 2 : random.Next((int)(TileType.Count) - (numberOfMatches + 5)));
					DTile.FillTile( max, new Vector2(tmp.rect.anchoredPosition.x, stp * -64 + offset.y), new Vector2(nxt , j));
					
					//DTile.rect.anchoredPosition = new Vector2(tmp.rect.anchoredPosition.x, stp * -64 + offset.x);
					DTile.SetTarget(new Vector2(tmp.rect.anchoredPosition.x, (nxt ) * -64 + offset.y), new Vector2(nxt , j));
					stp--;
					
					//sub++;
				}
				else
				{
					DTile.SetTarget(new Vector2(tmp.rect.anchoredPosition.x, (nxt) * -64 + offset.y), new Vector2(nxt , j));
				}
				nxt = nxt - sub;

			}

		}
	}

	public void DestroyBoard()
	{
		for (int i = 0; i < NumberOfRows; i++)
		{
			for (int j = 0; j < NumberOfColumns; j++)
			{
				Destroy(tiles[i, j].gameObject);
			}
		}
	}
}

[System.Serializable]
public class FlippedTiles
{
	public Tile firstTile;
	public Tile secondTile;

	public FlippedTiles(Tile tileOne, Tile TileTwo)
	{
		firstTile = tileOne;
		secondTile = TileTwo;
	}
}