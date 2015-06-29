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
    int points_per_bomb = 500;
    [SerializeField]
    int points_per_first_level = 300;
    public int point_for_three = 100;
    [SerializeField]
    int level_addition = 300;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int PointsLeft()
    {
        return points_per_first_level + (level - 1) * (level_addition) - points;
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
        // spaces between operators
        if (total_points - total_bombs * points_per_bomb >= points_per_bomb)
        {
            total_bombs++;
            bomb_charges++;
            GameObject.Find("BoardManager").GetComponent<BoardManager>().SetBombCount(bomb_charges);
        }
        if (points >= points_per_first_level+ (level-1)*(level_addition))
        {
            level++;
            points = 0;
            Application.LoadLevel("LevelScene");
        }
    }
}
