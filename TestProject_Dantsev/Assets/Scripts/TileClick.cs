using UnityEngine;
using System.Collections;

public class TileClick : MonoBehaviour
{

    void OnMouseOver()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().isActive)
        {
            if (Input.GetMouseButton(0))
            {
                // assing BoardManager here, you are using it in any case

                BoardManager boardManager = GameObject.Find("BoardManager").GetComponent<BoardManager>();
                if (!GameObject.Find("bomb").GetComponent<BombScript>().ActiveBomb())
                {
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayClick();
                    // why? :)
                    boardManager.DestroyTiles(this.gameObject);


                }
                else
                {
                    boardManager.DestroyByBomb
                        (gameObject.GetComponent<TileProperties>().column,
                        gameObject.GetComponent<TileProperties>().row);
                    GameObject.Find("bomb").GetComponent<BombScript>().DestroyBomb();
                }

            }
        }
    }
}
