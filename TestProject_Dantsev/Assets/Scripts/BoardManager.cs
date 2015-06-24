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
    

    List<GameObject> GetListToDestroy(GameObject tile_clicked,out bool to_destroy)//returns list of tiles to destroy and the
    {
        bool flag_to_stop = false;
        to_destroy = false;
        int start_row = tile_clicked.GetComponent<TileProperties>().row;
        int start_col = tile_clicked.GetComponent<TileProperties>().column;
        List<GameObject> list_to_destroy = new List<GameObject>();
        List<GameObject> go_to_check = new List<GameObject>();
        go_to_check.Add(tile_clicked);
        int col, row;
        while(go_to_check.Count>0)
        {
            col = go_to_check[0].GetComponent<TileProperties>().column;
            row = go_to_check[0].GetComponent<TileProperties>().row;
            //check the nearest tiles and add them to the lists, if they are not there and types of tyles 
            //are equal
            //then delete 0th element from the go_to_check list
            
        }
        //after this check if count>2 then out true
        return list_to_destroy;
    }

    void Destroy()
    {
        //destroy tiles
        //move upper tiles down
        //create new random tiles
    }

    void AssignPoint()//also as a parametr list.count
    {
        //returns number of point to be assigned
    }

    public bool DestroyTiles(GameObject clicked_tile) //returns true whether something is destroyed, or false if there is nothing to destroy
    {
        bool is_destroyed = false;
        List<GameObject> to_destroy = GetListToDestroy(clicked_tile, out is_destroyed);
        Destroy();
        AssignPoint();
        return is_destroyed;
    }


    void Update()
    {

    }
}
