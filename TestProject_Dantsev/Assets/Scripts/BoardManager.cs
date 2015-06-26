using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
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

    GameObject GetRandomTile()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        number_of_animals = 3 + (gm.level - 1);
        int random = Random.Range(0, Math.Min(number_of_animals, 5));
        GameObject result = Instantiate(list_of_tiles[random]);
        return result;
    }

    public bool DestroyByBomb(int col, int row)//destroy neraby tiles
    {
        bool result = false;
        int count = 0;
        List<GameObject> list_to_destroy = new List<GameObject>();
        if ((col > -1) && (col < columns) && (row > -1) && (row < rows))
        {
            Debug.Log(col.ToString());
            Debug.Log(row.ToString());
            list_to_destroy.Add(board_items[col][row]);
            count++;
            if ((col > 0) && (col < columns - 1))
            {
                list_to_destroy.Add(board_items[col + 1][row]);
                list_to_destroy.Add(board_items[col - 1][row]);
                count += 2;
                if ((row > 0) && (row < rows - 1))
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col][row - 1]);
                   
                    list_to_destroy.Add(board_items[col + 1][row + 1]);
                    list_to_destroy.Add(board_items[col - 1][row + 1]);
                    list_to_destroy.Add(board_items[col + 1][row - 1]);
                    list_to_destroy.Add(board_items[col - 1][row - 1]);
                    count += 6;
                }
                if (row == 0)
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col + 1][row + 1]);
                    list_to_destroy.Add(board_items[col - 1][row + 1]);
                    count += 3;
                }
                if (row == rows - 1)
                {
                    list_to_destroy.Add(board_items[col][rows - 1]);
                    list_to_destroy.Add(board_items[col + 1][row - 1]);
                    list_to_destroy.Add(board_items[col - 1][row - 1]);
                    count += 3;
                }
            }
            if (col == 0)
            {
                list_to_destroy.Add(board_items[col + 1][row]);
                if ((row > 0) && (row < rows - 1))
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col][row - 1]);

                    list_to_destroy.Add(board_items[col + 1][row + 1]);
                    list_to_destroy.Add(board_items[col + 1][row - 1]);
                    count += 4;
                }
                if (row == 0)
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col + 1][row + 1]);
                    count += 2;
                }
                if (row == rows - 1)
                {
                    list_to_destroy.Add(board_items[col][rows - 1]);
                    list_to_destroy.Add(board_items[col + 1][row - 1]);
                    count += 2;
                }
            }
            if (col == columns - 1)
            {
                list_to_destroy.Add(board_items[col - 1][row]);
                count++;
                if ((row > 0) && (row < rows - 1))
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col][row - 1]);

                    list_to_destroy.Add(board_items[col - 1][row + 1]);
                    list_to_destroy.Add(board_items[col - 1][row - 1]);
                    count += 4;
                }
                if (row == 0)
                {
                    list_to_destroy.Add(board_items[col][row + 1]);
                    list_to_destroy.Add(board_items[col - 1][row + 1]);
                    count += 2;
                }
                if (row == rows - 1)
                {
                    list_to_destroy.Add(board_items[col][rows - 1]);
                    list_to_destroy.Add(board_items[col - 1][row - 1]);
                    count += 2;
                }
            }
            Destroy(list_to_destroy);
            Movement();
            FillUpWithRandom();
            AssignPoint(count);
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
        GameObject.Find("GameManager").GetComponent<GameManager>().Restart();
    }

    void DecreaseTime()
    {
        time_per_level--;
    }

    void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().is_active = true;
        InvokeRepeating("DecreaseTime", 1.0f, 1.0f);
        PopupExit.onClick.AddListener(() => { ExitGame(); });
        PopupRestart.onClick.AddListener(() => { RestartGame(); });
        exit_button.onClick.AddListener(() => { ExitGame(); });
        time_per_level = Math.Max(GameObject.Find("GameManager").GetComponent<GameManager>().level - 3, 0) + 6;
        level_text.text = "Level: " + GameObject.Find("GameManager").GetComponent<GameManager>().level;
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
            score_text.text = "Score: 0";
            level_left_text.text = "Points left: " + GameObject.Find("GameManager").GetComponent<GameManager>().PointsLeft().ToString();
            total_score.text = "Total score: "+ GameObject.Find("GameManager").GetComponent<GameManager>().total_points;
            SetBombCount(GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges);
        }
    }




    List<GameObject> GetListToDestroy(GameObject tile_clicked, out bool to_destroy)//returns list of tiles to destroy and the
    {
        to_destroy = false;
        int start_row = tile_clicked.GetComponent<TileProperties>().row;
        int start_col = tile_clicked.GetComponent<TileProperties>().column;
        List<GameObject> list_to_destroy = new List<GameObject>();
        List<GameObject> go_to_check = new List<GameObject>();
        go_to_check.Add(tile_clicked);
        int col, row;
        while (go_to_check.Count > 0)
        {
            col = go_to_check[0].GetComponent<TileProperties>().column;
            row = go_to_check[0].GetComponent<TileProperties>().row;
            //check the nearest tiles and add them to the lists, if they are not there and types of tyles 
            //are equal
            //then delete 0th element from the go_to_check list

            //ugly if here. do not look up there. GODS MUST BURN ME FOR IT.
            #region
            if ((col > 0) && (col < columns - 1))
            {
                if ((row > 0) && (row < rows - 1))
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }
                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
                if (row == 0)
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }
                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                }
                if (row == rows - 1)
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }
                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
            }
            if (col == 0)
            {
                if ((row > 0) && (row < rows - 1))
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }

                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
                if (row == 0)
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }

                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                }
                if (row == rows - 1)
                {
                    if ((board_items[col + 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }

                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
            }
            if (col == columns - 1)
            {
                if ((row > 0) && (row < rows - 1))
                {

                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
                if (row == 0)
                {

                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row + 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row + 1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                }
                if (row == rows - 1)
                {
                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row - 1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row - 1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
            }
            #endregion
            go_to_check.RemoveAt(0);
        }
        if (list_to_destroy.Count > 2)
        {
            to_destroy = true;
        }
        else
        {
            to_destroy = false;
        }
        //after this check if count>2 then out true
        return list_to_destroy;
    }

    public void SetBombCount(int count)
    {
        bomb_count.text = "Bombs: " + count.ToString();
    }

    void Destroy(List<GameObject> lst_to_destroy)
    {
        //destroy tiles
        //move upper tiles down
        //create new random tiles
        int col, row;
        foreach (GameObject tile in lst_to_destroy)
        {
            col = tile.GetComponent<TileProperties>().column;
            row = tile.GetComponent<TileProperties>().row;
            board_items[col][row] = null;
            GameObject.Destroy(tile);
        }
    }

    void Movement()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows - 1; j++)
            {
                if (board_items[i][j] == null)
                {
                    int result_row = j;
                    bool place_found = false;

                    while ((result_row < rows) && (!place_found))
                    {
                        if (board_items[i][result_row] == null)
                        {
                            result_row++;
                        }
                        else
                        {
                            place_found = true;
                        }
                    }

                    if (place_found)
                    {
                        board_items[i][result_row].transform.position = new Vector3(size * i, size * j, 0f);
                        board_items[i][result_row].GetComponent<TileProperties>().row = j;
                        board_items[i][j] = board_items[i][result_row];
                        board_items[i][result_row] = null;
                    }


                }
            }
        }
    }

    void FillUpWithRandom()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board_items[i][j] == null)
                {
                    board_items[i][j] = GetRandomTile();
                    board_items[i][j].transform.position = new Vector3(size * i, size * j, 0f);
                    board_items[i][j].GetComponent<TileProperties>().row = j;
                    board_items[i][j].GetComponent<TileProperties>().column = i;
                }
            }
        }
    }

    void AssignPoint(int count)//also as a parametr list.count
    {
        int sum = 100;
        count -= 3;
        while (count > 0)
        {
            sum = Convert.ToInt32(Math.Round(sum * 1.3));
            count--;
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().points += sum;
        GameObject.Find("GameManager").GetComponent<GameManager>().total_points += sum;
        score_text.text = "Score: " + GameObject.Find("GameManager").GetComponent<GameManager>().points.ToString();
        total_score.text = "Total score: " + GameObject.Find("GameManager").GetComponent<GameManager>().total_points;
        level_left_text.text = "Points left: " + GameObject.Find("GameManager").GetComponent<GameManager>().PointsLeft().ToString();
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
        return is_destroyed;
    }

    void Update()
    {
        if ((bomb_button.GetComponent<SpriteRenderer>().sprite == passive_bomb) && (GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges > 0))
        {
            bomb_button.GetComponent<SpriteRenderer>().sprite = active_bomb;
        }
        if ((bomb_button.GetComponent<SpriteRenderer>().sprite == active_bomb) && (GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges == 0))
        {
            bomb_button.GetComponent<SpriteRenderer>().sprite = passive_bomb;
        }
        if (time_per_level > 0)
        {
            TimeLeftText.text = "Time left: " + time_per_level.ToString();
        }
        else
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().is_active = false;
            popup_menu.SetActive(true);
        }
    }
}