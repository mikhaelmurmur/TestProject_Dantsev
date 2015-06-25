using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    public int columns = 16;
    public int rows = 16;
    private Transform board_holder;
    public List<GameObject> list_of_tiles = new List<GameObject>();
    List<List<GameObject>> board_items = new List<List<GameObject>>();

    GameObject GetRandomTile()
    {
        int random = Random.Range(0, list_of_tiles.Count);
        GameObject result = Instantiate(list_of_tiles[random]);
        return result;
    }

    void Start()
    {
        for (int i = 0; i < columns; i++)
        {
            board_items.Add(new List<GameObject>());
            for (int j = 0; j < rows; j++)
            {
                GameObject tmp = GetRandomTile();
                tmp.transform.position = new Vector3(i, j, 0f);
                tmp.GetComponent<TileProperties>().row = j;
                tmp.GetComponent<TileProperties>().column = i;
                board_items[i].Add(tmp);
            }
        }
    }

    GameObject GetTileByIndex(int row, int column)//returns tile by its position on the board
    {
        return board_items[row][column];
    }


    List<GameObject> GetListToDestroy(GameObject tile_clicked, out bool to_destroy)//returns list of tiles to destroy and the
    {
        int count = 0;
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
            if ((col > 0) && (col < columns))
            {
                if ((row > 0) && (row < rows))
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
                if (row == rows)
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
                if ((row > 0) && (row < rows))
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
                if (row == rows)
                {
                    if ((board_items[col +1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col + 1][row])))
                    {
                        go_to_check.Add(board_items[col + 1][row]);
                        list_to_destroy.Add(board_items[col + 1][row]);
                    }

                    if ((board_items[col][row-1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row-1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
            }
            if (col == columns)
            {
                if ((row > 0) && (row < rows))
                {

                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col ][row+1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row+1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                    if ((board_items[col ][row-1].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col][row-1])))
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
                    if ((board_items[col][row+1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row+1])))
                    {
                        go_to_check.Add(board_items[col][row + 1]);
                        list_to_destroy.Add(board_items[col][row + 1]);
                    }
                }
                if (row == rows)
                {
                    if ((board_items[col - 1][row].name == board_items[col][row].name)
                         && (!list_to_destroy.Contains(board_items[col - 1][row])))
                    {
                        go_to_check.Add(board_items[col - 1][row]);
                        list_to_destroy.Add(board_items[col - 1][row]);
                    }
                    if ((board_items[col][row-1].name == board_items[col][row].name)
                        && (!list_to_destroy.Contains(board_items[col][row-1])))
                    {
                        go_to_check.Add(board_items[col][row - 1]);
                        list_to_destroy.Add(board_items[col][row - 1]);
                    }
                }
            }
            #endregion
            count++;
            go_to_check.RemoveAt(0);
        }
        if (count > 2)
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
            for (int j = 0; j < rows; j++)
            {
                if (board_items[i][j] != null)
                {
                    int result_row = j;
                    bool place_found = false;
                    while ((result_row > 0) && (!place_found))
                    {
                        if (board_items[i][result_row] == null)
                        {
                            result_row--;
                        }
                        else
                        {
                            place_found = true;
                        }
                    }
                    if (place_found)
                    {
                        board_items[i][j].transform.position = new Vector3(i, result_row, 0f);
                        board_items[i][j].GetComponent<TileProperties>().row = result_row;
                        board_items[i][j].GetComponent<TileProperties>().column = i;
                        board_items[i][result_row] = board_items[i][j];
                        board_items[i][j] = null;
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
                    GameObject tmp = GetRandomTile();
                    board_items[i][j].transform.position = new Vector3(i, j, 0f);
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
    }

    public bool DestroyTiles(GameObject clicked_tile) //returns true whether something is destroyed, or false if there is nothing to destroy
    {
        bool is_destroyed = false;
        List<GameObject> to_destroy = GetListToDestroy(clicked_tile, out is_destroyed);
        if (is_destroyed)
        {
            AssignPoint(to_destroy.Count);
            Destroy(to_destroy);
            Movement();
            FillUpWithRandom();
        }
        return is_destroyed;
    }


    void Update()
    {

    }
}
