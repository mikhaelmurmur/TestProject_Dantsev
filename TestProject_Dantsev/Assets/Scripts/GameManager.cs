using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool isActive = true;
    public int points = 0;
    public int level = 1;
    public int bombCharges = 0;
    public int totalPoints = 0;
    public int totalBombs = 0;
    [SerializeField]
    int pointsPerBomb = 500;
    [SerializeField]
    int pointsPerFirstLevel = 300;
    public int pointForThree = 100;
    [SerializeField]
    int levelAddition = 300;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int PointsLeft()
    {
        return pointsPerFirstLevel + (level - 1) * (levelAddition) - points;
    }

    public void Restart()
    {
        level = 1;
        points = 0;
        bombCharges = 0;
        totalPoints = 0;
        totalBombs = 0;
        isActive = true;
        Application.LoadLevel("LevelScene");
    }

    void Update()
    {
        // spaces between operators
        if (totalPoints - totalBombs * pointsPerBomb >= pointsPerBomb)
        {
            totalBombs++;
            bombCharges++;
            GameObject.Find("BoardManager").GetComponent<BoardManager>().SetBombCount(bombCharges);
        }
        if (points >= pointsPerFirstLevel+ (level-1)*(levelAddition))
        {
            level++;
            points = 0;
            Application.LoadLevel("LevelScene");
        }
    }
}
