using UnityEngine;
using System.Collections;
using System;

public class BombScript : MonoBehaviour
{

    [SerializeField]
    GameObject bomb = null;
    GameObject bomb_obj = null;
    

    public bool ActiveBomb()
    {
        
        return bomb_obj != null;
    }

    void Update()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().is_active)
        {
            if (bomb_obj != null)
            {
                bomb_obj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                bomb_obj.transform.position = new Vector3(bomb_obj.transform.position.x + 1, bomb_obj.transform.position.y + 1, -1);
                if (Input.GetMouseButton(0))
                {
                    Debug.Log(bomb_obj.transform.position.x);
                    Debug.Log(bomb_obj.transform.position.y);
                    int col = Convert.ToInt32((bomb_obj.transform.position.x) / 3.5);
                    int row = Convert.ToInt32((bomb_obj.transform.position.y) / 3.5);
                    if (GameObject.Find("BoardManager").GetComponent<BoardManager>().DestroyByBomb(col, row))
                    {
                        GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayBomb();
                        GameObject.Destroy(bomb_obj);
                        bomb_obj = null;
                    }
                }
            }
        }
    }

    public void DestroyBomb()
    {
        if(bomb_obj!=null)
        {
            GameObject.Destroy(bomb_obj);
            bomb_obj = null;
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && (bomb_obj == null)&& GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges>0)
        {
            bomb_obj = GameObject.Instantiate(bomb);
            GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges--;
            GameObject.Find("BoardManager").GetComponent<BoardManager>().SetBombCount(GameObject.Find("GameManager").GetComponent<GameManager>().bomb_charges);
        }
        if (bomb_obj != null)
        {

        }
    }
}
