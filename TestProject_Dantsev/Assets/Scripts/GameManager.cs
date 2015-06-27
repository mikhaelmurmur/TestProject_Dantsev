using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool is_active = true;
    public int points = 0;
    public int level = 1;
    public int bomb_charges = 0;
    public int total_points = 0;
    public int total_bombs = 0;
    [SerializeField]
    int points_per_bomb=500;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int PointsLeft()
    {
        return level * 300 - points;
    }

    public void Restart()
    {
        level = 1;
        points = 0;
        bomb_charges = 0;
        total_points = 0;
        total_bombs = 0;
        is_active = true;
        Application.LoadLevel("LevelScene");
    }

    void Update()
    {
        if(total_points-total_bombs*points_per_bomb>=points_per_bomb)
        {
            total_bombs++;
            bomb_charges++;
            GameObject.Find("BoardManager").GetComponent<BoardManager>().SetBombCount(bomb_charges);
        }
		// Why 300?
        if (points >= level * 300)
        {
            level++;
            points = 0;
            Application.LoadLevel("LevelScene");
        }
    }
}
