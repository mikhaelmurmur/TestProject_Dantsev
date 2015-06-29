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
    Text start_level_text = null;
    [SerializeField]
    int time_per_start_screen = 2;
    [SerializeField]
    GameObject level_panel = null;
    [SerializeField]
    GameObject popup_menu = null;
    [SerializeField]
    Text score_text = null;
    [SerializeField]
    Text total_score = null;
    [SerializeField]
    Text bomb_count = null;
    [SerializeField]
    Text level_text = null;
    [SerializeField]
    Text level_left_text = null;
    [SerializeField]
    Button exit_button = null;
    [SerializeField]
    Button PopupExit = null;
    [SerializeField]
    Button PopupRestart = null;
    [SerializeField]
    Text TimeLeftText = null;
    GameManager gameManager = null;
    bool is_started = false;
    public float time_per_level = 6f;
    public int columns = 4;
    public int rows = 4;
    public float size = 4f;
    int number_of_animals = 3;
    private Transform board_holder;
    public List<GameObject> list_of_tiles = new List<GameObject>();
    List<List<GameObject>> board_items = new List<List<GameObject>>();
    [SerializeField]
    Sprite passive_bomb = null;
    [SerializeField]
    Sprite active_bomb = null;
    [SerializeField]
    GameObject bomb_button = null;
    [SerializeField]
    int number_of_animals_to_use = 3;


    GameObject GetRandomTile()
    {
        number_of_animals = 3 + (gameManager.level - 1);
        int random = Random.Range(0, Math.Min(number_of_animals, number_of_animals_to_use));
        GameObject result = Instantiate(list_of_tiles[random]);
        return result;
    }

    GameObject GetObjectByIndex(int col, int row)
    {
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows))
        {
            return board_items[col][row];
        }
        // you don't need else :)
        return null;
    }

    // why not recursion?
    public bool DestroyByBomb(int col, int row)//destroy nearby tiles
    {
        bool result = false;
        List<GameObject> list_to_destroy = new List<GameObject>();
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
                        list_to_destroy.Add(GetObjectByIndex(col - 1 + i, row - 1 + j));
                    }
                }
            }
            // It really looks horrible :)

            // do not commit commented code


            Destroy(list_to_destroy);
            Movement();
            FillUpWithRandom();
            AssignPoint(list_to_destroy.Count);
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
        time_per_level--;
    }

    void DecreaseStartScreen()
    {
        time_per_start_screen--;
    }

    void InItBoard()
    {
        for (int i = 0; i < columns; i++)
        {
            board_items.Add(new List<GameObject>());
            for (int j = 0; j < rows; j++)
            {
                GameObject tmp = GetRandomTile();
                tmp.transform.position = new Vector3(size * i, size * j, 0f);
                tmp.GetComponent<TileProperties>().row = j;
                tmp.GetComponent<TileProperties>().column = i;
                board_items[i].Add(tmp);
            }
        }
        start_level_text.text = "Level " + gameManager.level;

        score_text.text = "Score: 0";
        level_left_text.text = "Points left: " + gameManager.PointsLeft().ToString();
        total_score.text = "Total score: " + gameManager.total_points;
        SetBombCount(gameManager.bomb_charges);
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.is_active = true;
        InvokeRepeating("DecreaseStartScreen", 1.0f, 1.0f);
        PopupExit.onClick.AddListener(() => { ExitGame(); });
        PopupRestart.onClick.AddListener(() => { RestartGame(); });
        exit_button.onClick.AddListener(() => { ExitGame(); });
        time_per_level = Math.Max(gameManager.level - 3, 0) + time_per_level;
        level_text.text = "Level: " + gameManager.level;


        // why not a separate function ? 
        InItBoard();
    }



    void GetTilesToDestroy(int col, int row, List<GameObject> list_to_fill, GameObject tile_clicked)
    {
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows)
            && (tile_clicked.name == board_items[col][row].name) && (!list_to_fill.Contains(board_items[col][row])))
        {
            list_to_fill.Add(board_items[col][row]);
            GetTilesToDestroy(col + 1, row, list_to_fill, tile_clicked);
            GetTilesToDestroy(col - 1, row, list_to_fill, tile_clicked);
            GetTilesToDestroy(col, row - 1, list_to_fill, tile_clicked);
            GetTilesToDestroy(col, row + 1, list_to_fill, tile_clicked);
        }
    }

    List<GameObject> GetListToDestroy(GameObject tile_clicked, out bool toDestroy)//returns list of tiles to destroy and the
    {
        // use cammel case toDestroy
        toDestroy = false;
        int start_row = tile_clicked.GetComponent<TileProperties>().row;
        int start_col = tile_clicked.GetComponent<TileProperties>().column;
        List<GameObject> list_to_destroy = new List<GameObject>();
        List<GameObject> go_to_check = new List<GameObject>();
        go_to_check.Add(tile_clicked);
        GetTilesToDestroy(start_col, start_row, list_to_destroy, tile_clicked);

        // do not commit commented code

        toDestroy = list_to_destroy.Count > 2;
        return list_to_destroy;
    }

    public void SetBombCount(int count)
    {
        bomb_count.text = "Bombs: " + count.ToString();
    }

    void Destroy(List<GameObject> lst_to_destroy)
    {
        int col, row;
        foreach (GameObject tile in lst_to_destroy)
        {
            col = tile.GetComponent<TileProperties>().column;
            row = tile.GetComponent<TileProperties>().row;
            board_items[col][row] = null;
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
                if (board_items[columnIndex][rowIndex] == null)
                {
                    int result_row = rowIndex;
                    bool place_found = false;
                    // comment here
                    //for each empty space we search first element which is situated above our empty space
                    while ((result_row < rows) && (!place_found))
                    {
                        if (board_items[columnIndex][result_row] == null)
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
                        board_items[columnIndex][result_row].transform.DOMove(new Vector3(size * columnIndex, size * rowIndex, 0f), 0.5f);
                        board_items[columnIndex][result_row].GetComponent<TileProperties>().row = rowIndex;
                        board_items[columnIndex][rowIndex] = board_items[columnIndex][result_row];
                        board_items[columnIndex][result_row] = null;
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
                GameObject tile = board_items[columnIndex][rowIndex];
                if (tile == null)
                {
                    board_items[columnIndex][rowIndex] = tile = GetRandomTile();

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
        int sum = gameManager.point_for_three;
        count -= 3;
        while (count > 0)
        {
            sum = Convert.ToInt32(Math.Round(sum * 1.3));
            count--;
        }
        // cache! GameObject.Find("GameManager").GetComponent<GameManager>()
        // or event assing somewhere onstart, you are using it 18 times in this file
        gameManager.points += sum;
        gameManager.total_points += sum;
        score_text.text = "Score: " + gameManager.points.ToString();
        total_score.text = "Total score: " + gameManager.total_points;
        level_left_text.text = "Points left: " + gameManager.PointsLeft().ToString();
    }

    public bool DestroyTiles(GameObject clicked_tile) //returns true whether something is destroyed, or false if there is nothing to destroy
    {
        bool is_destroyed = false;
        List<GameObject> to_destroy = GetListToDestroy(clicked_tile, out is_destroyed);
        if (is_destroyed)
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySuccess();
            AssignPoint(to_destroy.Count);
            Destroy(to_destroy);
            Movement();
            FillUpWithRandom();
        }
        else
        {
            foreach (GameObject tile in to_destroy)
            {
                Sequence change_color = DOTween.Sequence();
                change_color.Append(tile.GetComponent<SpriteRenderer>().material.DOColor(Color.gray, 0.5f).SetLoops(1));
                change_color.Append(tile.GetComponent<SpriteRenderer>().material.DOColor(Color.white, 0.5f).SetLoops(1));
                change_color.Play();
            }
        }
        return is_destroyed;
    }

    IEnumerator ChangeColorWait()
    {
        yield return new WaitForSeconds(1);
    }



    void Update()
    {
        if (time_per_start_screen == 0)
        {
            InvokeRepeating("DecreaseTime", 1.0f, 1.0f);
            level_panel.SetActive(false);
            time_per_start_screen = -1;
            is_started = true;
        }
        // why not cache bomb_button.GetComponent<SpriteRenderer>().sprite ? into a variable
        Sprite bomb_button_sprite = bomb_button.GetComponent<SpriteRenderer>().sprite;
        // comment here
        // if there are bomb charges, but it has not been shown via active bomb sprite
        // then we change sprite to an active one
        if ((bomb_button_sprite == passive_bomb) && (gameManager.bomb_charges > 0))
        {
            bomb_button.GetComponent<SpriteRenderer>().sprite = active_bomb;
        }
        // if it's been shown that bomb is active, but there are no charges 
        // then deactivate bomb
        if ((bomb_button_sprite == active_bomb) && (gameManager.bomb_charges == 0))
        {
            bomb_button.GetComponent<SpriteRenderer>().sprite = passive_bomb;
        }
        if (time_per_level > 0)
        {
            TimeLeftText.text = "Time left: " + time_per_level.ToString();
        }
        else
        {
            gameManager.is_active = false;
            popup_menu.SetActive(true);
        }
    }
}