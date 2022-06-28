using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BattleField playerBattleField;
    [SerializeField] private BattleField opponentBattleField;
    [SerializeField] private HUDManager hudManager;
    
    private Opponent opponent;
    private int numOfCells = 10;
    private GameSide side;
    
    private static GameManager _instance;

    public static GameManager Instance()
    {
        return _instance;
    }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance == this)
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        StartGame(numOfCells);
    }

    public void ChangeSides()
    {
        switch (side)
        {
            case GameSide.Player:
                side = GameSide.Opponent;
                playerBattleField.SetFieldCellsInteractable(true);
                opponentBattleField.SetFieldCellsInteractable(false);
                HitOpponent();
                break;
            case GameSide.Opponent:
                side = GameSide.Player;
                playerBattleField.SetFieldCellsInteractable(false);
                opponentBattleField.SetFieldCellsInteractable(true);
                break;
        }

        CheckForWin();
    }
    
    private void StartGame(int num = 10)
    {
        numOfCells = num;

        playerBattleField.GenerateBattleField(num ,autoGenerate: true);
        opponentBattleField.GenerateBattleField(num, autoGenerate: true);
        
        opponent = new Opponent(new Bot(), playerBattleField);
        opponent.ChangeStrategy(new RandomBot());
        
        playerBattleField.SetFieldCellsInteractable(false);
    }
    
    public void AddPlayerShip(int size, int x1, int y1, int x2, int y2)
    {
        playerBattleField.AddShip(size, x1, y1, x2, y2);
    }
    
    public void AddOpponentShip(int size, int x1, int y1, int x2, int y2)
    {
        opponentBattleField.AddShip(size, x1, y1, x2, y2);
    }
    
    public void HitPlayer(int y, int x)
    {
        playerBattleField.Hit(y, x);
    }
    
    public void HitOpponent()
    {
        opponent.MakeShot();
    }
    
    public bool CheckForWin()
    {
        if (opponentBattleField.CheckForEmpty() == true)
        {
            PrintWinnerName("Player");
            return true;
        }

        if (playerBattleField.CheckForEmpty() == true)
        {
            PrintWinnerName("Opponent");
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void PrintWinnerName(string name)
    {
        Console.WriteLine();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (i == 0 || i == 9)
                    Console.Write("&&&");
                else if (i == 5)
                {
                    Console.Write("name = " + name);
                    j = 10;
                }
            }

            Console.WriteLine();
        }
    }
    
    private enum GameSide
    {
        Player, Opponent
    }
}