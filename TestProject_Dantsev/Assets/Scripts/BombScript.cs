using UnityEngine;
using System.Collections;
using System;

public class BombScript : MonoBehaviour
{

    [SerializeField]
    GameObject bomb = null;
    GameObject bombObj = null;
    

    public bool ActiveBomb()
    {
        return bombObj != null;
    }

    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().isActive)
        {
            if (bombObj != null)
            {
                bombObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bombObj.transform.position = new Vector3(bombObj.transform.position.x + 1, bombObj.transform.position.y + 1, -1);
                if (Input.GetMouseButton(0))
                {
                    Debug.Log(bombObj.transform.position.x);
                    Debug.Log(bombObj.transform.position.y);
                    float size = GameObject.Find("BoardManager").GetComponent<BoardManager>().size;
                    int col = Convert.ToInt32((bombObj.transform.position.x) / size);
                    int row = Convert.ToInt32((bombObj.transform.position.y) / size);
                    if (GameObject.Find("BoardManager").GetComponent<BoardManager>().DestroyByBomb(col, row))
                    {
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayBomb();
                        GameObject.Destroy(bombObj);
                        bombObj = null;
                    }
                }
            }
        }
    }

    public void DestroyBomb()
    {
        if(bombObj!=null)
        {
            GameObject.Destroy(bombObj);
            bombObj = null;
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && (bombObj == null)&& GameObject.Find("GameManager").GetComponent<GameManager>().bombCharges>0)
        {
            bombObj = GameObject.Instantiate(bomb);
            GameObject.Find("GameManager").GetComponent<GameManager>().bombCharges--;
            GameObject.Find("BoardManager").GetComponent<BoardManager>().SetBombCount(GameObject.Find("GameManager").GetComponent<GameManager>().bombCharges);
        }
		
    }
}
