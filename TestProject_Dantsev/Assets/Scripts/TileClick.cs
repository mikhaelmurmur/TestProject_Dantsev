using UnityEngine;
using System.Collections;

public class TileClick : MonoBehaviour 
{

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void Click()
    {
        if (Input.GetMouseButton(0))
        {
            BoardManager bm = GameObject.Find("BoardManager").GetComponent<BoardManager>();
            //if no destroy then some effect takes place
        }
    }
}
