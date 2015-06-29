using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;


public class BoardManager : MonoBehaviour
{
    [SerializeField]
    Text startLevelText = null;
    [SerializeField]
    int timePerStartScreen = 2;
    [SerializeField]
    GameObject levelPanel = null;
    [SerializeField]
    GameObject popupMenu = null;
    [SerializeField]
    Text scoreText = null;
    [SerializeField]
    Text totalScore = null;
    [SerializeField]
    Text bombCount = null;
    [SerializeField]
    Text levelText = null;
    [SerializeField]
    Text levelLeftText = null;
    [SerializeField]
    Button exitButton = null;
    [SerializeField]
    Button popupExit = null;
    [SerializeField]
    Button popupRestart = null;
    [SerializeField]
    Text timeLeftText = null;
    GameManager gameManager = null;
    bool isStarted = false;
    public float timePerLevel = 6f;
    public int columns = 4;
    public int rows = 4;
    public float size = 4f;
    int numberOfAnimals = 3;
    private Transform boardHolder;
    public List<GameObject> listOfTiles = new List<GameObject>();
    List<List<GameObject>> boardItems = new List<List<GameObject>>();
    [SerializeField]
    Sprite passiveBomb = null;
    [SerializeField]
    Sprite activeBomb = null;
    [SerializeField]
    GameObject bombButton = null;
    [SerializeField]
    int numberOfAnimalsToUse = 3;


    GameObject GetRandomTile()
    {
        numberOfAnimals = 3 + (gameManager.level - 1);
        int random = Random.Range(0, Math.Min(numberOfAnimals, numberOfAnimalsToUse));
        GameObject result = Instantiate(listOfTiles[random]);
        return result;
    }

    GameObject GetObjectByIndex(int col, int row)
    {
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows))
        {
            return boardItems[col][row];
        }
        // you don't need else :)
        return null;
    }

    // why not recursion?
    public bool DestroyByBomb(int col, int row)//destroy nearby tiles
    {
        bool result = false;
        List<GameObject> lisToDestroy = new List<GameObject>();
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows))
        {
            Debug.Log(col.ToString());
            Debug.Log(row.ToString());

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // maybe    if (GetObjectByIndex(col - 1 + i, row - 1 + j)) ?  without line above?
                    if (GetObjectByIndex(col - 1 + i, row - 1 + j))
                    {
                        lisToDestroy.Add(GetObjectByIndex(col - 1 + i, row - 1 + j));
                    }
                }
            }
            // It really looks horrible :)

            // do not commit commented code


            Destroy(lisToDestroy);
            Movement();
            FillUpWithRandom();
            AssignPoint(lisToDestroy.Count);
            result = true;
        }
        return result;
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void RestartGame()
    {
        gameManager.Restart();
    }

    void DecreaseTime()
    {
        timePerLevel--;
    }

    void DecreaseStartScreen()
    {
        timePerStartScreen--;
    }

    void InItBoard()
    {
        for (int columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            boardItems.Add(new List<GameObject>());
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                GameObject tmp = GetRandomTile();
                tmp.transform.position = new Vector3(size * columnIndex, size * rowIndex, 0f);
                tmp.GetComponent<TileProperties>().row = rowIndex;
                tmp.GetComponent<TileProperties>().column = columnIndex;
                boardItems[columnIndex].Add(tmp);
            }
        }
        startLevelText.text = "Level " + gameManager.level;

        scoreText.text = "Score: 0";
        levelLeftText.text = "Points left: " + gameManager.PointsLeft().ToString();
        totalScore.text = "Total score: " + gameManager.totalPoints;
        SetBombCount(gameManager.bombCharges);
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.isActive = true;
        InvokeRepeating("DecreaseStartScreen", 1.0f, 1.0f);
        popupExit.onClick.AddListener(() => { ExitGame(); });
        popupRestart.onClick.AddListener(() => { RestartGame(); });
        exitButton.onClick.AddListener(() => { ExitGame(); });
        timePerLevel = Math.Max(gameManager.level - 3, 0) + timePerLevel;
        levelText.text = "Level: " + gameManager.level;


        // why not a separate function ? 
        InItBoard();
    }



    void GetTilesToDestroy(int col, int row, List<GameObject> listToFill, GameObject tileClicked)
    {
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows)
            && (tileClicked.name == boardItems[col][row].name) && (!listToFill.Contains(boardItems[col][row])))
        {
            listToFill.Add(boardItems[col][row]);
            GetTilesToDestroy(col + 1, row, listToFill, tileClicked);
            GetTilesToDestroy(col - 1, row, listToFill, tileClicked);
            GetTilesToDestroy(col, row - 1, listToFill, tileClicked);
            GetTilesToDestroy(col, row + 1, listToFill, tileClicked);
        }
    }

    List<GameObject> GetListToDestroy(GameObject tileClicked, out bool toDestroy)//returns list of tiles to destroy and the
    {
        // use cammel case toDestroy
        toDestroy = false;
        int start_row = tileClicked.GetComponent<TileProperties>().row;
        int start_col = tileClicked.GetComponent<TileProperties>().column;
        List<GameObject> list_to_destroy = new List<GameObject>();
        List<GameObject> go_to_check = new List<GameObject>();
        go_to_check.Add(tileClicked);
        GetTilesToDestroy(start_col, start_row, list_to_destroy, tileClicked);

        // do not commit commented code

        toDestroy = list_to_destroy.Count > 2;
        return list_to_destroy;
    }

    public void SetBombCount(int count)
    {
        bombCount.text = "Bombs: " + count.ToString();
    }

    void Destroy(List<GameObject> lstToDestroy)
    {
        int col, row;
        foreach (GameObject tile in lstToDestroy)
        {
            col = tile.GetComponent<TileProperties>().column;
            row = tile.GetComponent<TileProperties>().row;
            boardItems[col][row] = null;
            tile.transform.position.Set(col, row, 1);
            tile.transform.DOMoveY(-40, 0.5f);

            //GameObject.Destroy(tile);
        }
    }

    void Movement()
    {
        for (int columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < rows - 1; rowIndex++)
            {
                if (boardItems[columnIndex][rowIndex] == null)
                {
                    int result_row = rowIndex;
                    bool place_found = false;
                    // comment here
                    //for each empty space we search first element which is situated above our empty space
                    while ((result_row < rows) && (!place_found))
                    {
                        if (boardItems[columnIndex][result_row] == null)
                        {
                            result_row++;
                        }
                        else
                        {
                            place_found = true;
                        }
                    }

                    if (place_found)//if we have found a tile, than move it down
                    {
                        boardItems[columnIndex][result_row].transform.DOMove(new Vector3(size * columnIndex, size * rowIndex, 0f), 0.5f);
                        boardItems[columnIndex][result_row].GetComponent<TileProperties>().row = rowIndex;
                        boardItems[columnIndex][rowIndex] = boardItems[columnIndex][result_row];
                        boardItems[columnIndex][result_row] = null;
                    }


                }
            }
        }
    }

    void FillUpWithRandom()
    {
        for (int columnIndex = 0; columnIndex < columns; columnIndex++)
        {
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                // why not cache a variable board_items[i][j]? 
                GameObject tile = boardItems[columnIndex][rowIndex];
                if (tile == null)
                {
                    boardItems[columnIndex][rowIndex] = tile = GetRandomTile();

                    tile.transform.position = new Vector3(size * columnIndex, size * columns, 0f);
                    tile.transform.DOMove(new Vector3(size * columnIndex, size * rowIndex, 0f), 0.5f);
                    tile.GetComponent<TileProperties>().row = rowIndex;
                    tile.GetComponent<TileProperties>().column = columnIndex;
                }
            }
        }
    }

    void AssignPoint(int count)
    {
        int sum = gameManager.pointForThree;
        count -= 3;
        while (count > 0)
        {
            sum = Convert.ToInt32(Math.Round(sum * 1.3));
            count--;
        }
        // cache! GameObject.Find("GameManager").GetComponent<GameManager>()
        // or event assing somewhere onstart, you are using it 18 times in this file
        gameManager.points += sum;
        gameManager.totalPoints += sum;
        scoreText.text = "Score: " + gameManager.points.ToString();
        totalScore.text = "Total score: " + gameManager.totalPoints;
        levelLeftText.text = "Points left: " + gameManager.PointsLeft().ToString();
    }

    public bool DestroyTiles(GameObject clickedTile) //returns true whether something is destroyed, or false if there is nothing to destroy
    {
        bool isDestroyed = false;
        List<GameObject> toDestroy = GetListToDestroy(clickedTile, out isDestroyed);
        if (isDestroyed)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySuccess();
            AssignPoint(toDestroy.Count);
            Destroy(toDestroy);
            Movement();
            FillUpWithRandom();
        }
        else
        {
            foreach (GameObject tile in toDestroy)
            {
                Sequence changeColor = DOTween.Sequence();
                changeColor.Append(tile.GetComponent<SpriteRenderer>().material.DOColor(Color.gray, 0.5f).SetLoops(1));
                changeColor.Append(tile.GetComponent<SpriteRenderer>().material.DOColor(Color.white, 0.5f).SetLoops(1));
                changeColor.Play();
            }
        }
        return isDestroyed;
    }

    IEnumerator ChangeColorWait()
    {
        yield return new WaitForSeconds(1);
    }



    void Update()
    {
        if (timePerStartScreen == 0)
        {
            InvokeRepeating("DecreaseTime", 1.0f, 1.0f);
            levelPanel.SetActive(false);
            timePerStartScreen = -1;
            isStarted = true;
        }
        // why not cache bomb_button.GetComponent<SpriteRenderer>().sprite ? into a variable
        Sprite bombButtonSprite = bombButton.GetComponent<SpriteRenderer>().sprite;
        // comment here
        // if there are bomb charges, but it has not been shown via active bomb sprite
        // then we change sprite to an active one
        if ((bombButtonSprite == passiveBomb) && (gameManager.bombCharges > 0))
        {
            bombButton.GetComponent<SpriteRenderer>().sprite = activeBomb;
        }
        // if it's been shown that bomb is active, but there are no charges 
        // then deactivate bomb
        if ((bombButtonSprite == activeBomb) && (gameManager.bombCharges == 0))
        {
            bombButton.GetComponent<SpriteRenderer>().sprite = passiveBomb;
        }
        if (timePerLevel > 0)
        {
            timeLeftText.text = "Time left: " + timePerLevel.ToString();
        }
        else
        {
            gameManager.isActive = false;
            popupMenu.SetActive(true);
        }
    }
}