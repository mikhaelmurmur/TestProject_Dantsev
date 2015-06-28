using UnityEngine;
using System.Collections;

public class TileClick : MonoBehaviour 
{

    void OnMouseOver()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().is_active)
        {
            if (Input.GetMouseButton(0))
            {
                if (!GameObject.Find("bomb").GetComponent<BombScript>().ActiveBomb())
                {
                    BoardManager bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayClick();
                    if (!bm.DestroyTiles(this.gameObject))
                    {

                    }

                }
                else
                {
                    GameObject.Find("BoardManager").GetComponent<BoardManager>().
                        DestroyByBomb(gameObject.GetComponent<TileProperties>().column,
                        gameObject.GetComponent<TileProperties>().row);
                    GameObject.Find("bomb").GetComponent<BombScript>().DestroyBomb();
                }
                //if no destroy then some effect takes place
            }
        }
    }
}
