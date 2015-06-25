using UnityEngine;
using System.Collections;

public class TileClick : MonoBehaviour 
{

	void Start () 
    {
	
	}



	void OnMouseOver() 
    {
        if (Input.GetMouseButton(0))
		{
            BoardManager bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            if (bm.DestroyTiles(this.gameObject))
            {
                Debug.Log("Y");
            }
            else
            {
                Debug.Log("N");
            }
            //if no destroy then some effect takes place
        }
    }
}
