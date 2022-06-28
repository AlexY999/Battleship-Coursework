using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BattleField : MonoBehaviour
{
    [SerializeField] private GameObject fieldPrefab;
    [SerializeField] private bool opponentPanel = false;
    
    public static int Width { get; private set; }
    public static int Height { get; private set; }
    public List<List<FieldCell>> FieldCells;

    private List<int> ships;

    public void OnDisable()
    {
        foreach (var fieldCell in FieldCells.SelectMany(list => list))
        {
            fieldCell.OnShipHit -= OnShipKilled;
        }
    }

    public void GenerateBattleField(int numOfCells ,bool autoGenerate = false)
    {
        Width = Height = numOfCells;
        ships = new List<int>(new int[4]);

        FieldCells = new List<List<FieldCell>>();
        for (int i = 0; i < numOfCells; i++)
        {
            FieldCells.Add(new List<FieldCell>());
            for (int k = 0; k < numOfCells; k++)
            {
                var obj = Instantiate(fieldPrefab, transform);
                var cell = obj.AddComponent<FieldCell>();
                cell.OnShipHit += OnShipKilled;
                cell.opponentPanel = opponentPanel;
                FieldCells[i].Add(cell);
            }
        }

        if (autoGenerate)
        {
            AutoGenerate();
        }
    }

    public void SetFieldCellsInteractable(bool interactable)
    {
        foreach (var fieldCell in FieldCells.SelectMany(list => list))
        {
            fieldCell.SetButton(interactable);
        }
    }
    
    private void AutoGenerate()
    {
        var random = new Random();
        int x1, y1, x2, y2;
        for (int i = 4; i > 0; i--)
        {
            for (int j = 4 - i; j >= 0; j--)
            {
                do
                {
                    if (Convert.ToBoolean(random.Next(2)))
                    {
                        x1 = random.Next(Width - i);
                        x2 = x1 + i - 1;
                        y1 = y2 = random.Next(Height);
                    }
                    else
                    {
                        x1 = x2 = random.Next(Height);
                        y1 = random.Next(Width - i);
                        y2 = y1 + i - 1;
                    }
                } while (!AddShip(i, x1, y1, x2, y2));
            }
        }
    }

    public int Hit(int y, int x)
    {
        int res = FieldCells[y][x].Hit();
        return res;
    }

    private void OnShipKilled(int res)
    {
        switch (res)
        {
            case -1:
                Debug.Log("Miss...");
                GameManager.Instance().ChangeSides();
                break;  
            case 0:
                Debug.Log("Hit!");
                break;
            case 1:
                ships[0]--;
                Debug.Log("Killed ship with size one!");
                break;
            case 2:
                ships[1]--;
                Debug.Log("Killed ship with size two!");
                break;
            case 3:
                ships[2]--;
                Debug.Log("Killed ship with size three!");
                break;
            case 4:
                ships[3]--;
                Debug.Log("Killed ship with size four!");
                break;
        }
    }

    private bool CheckPlace(int size, int x1, int y1, int x2, int y2)
    {
        if (x1 >= Width || x2 >= Width || y1 >= Height || y2 >= Height || x1 < 0 || x2 < 0 || y1 < 0 || y2 < 0 ||
            (x1 != x2 && y1 != y2) || (size - 1 != Math.Abs(x1 - x2) && size - 1 != Math.Abs(y1 - y2)))
        {
            Debug.Log("Wrong coordinates or ship size!");
            return false;
        }

        if (ships[size - 1] == 4 - size + 1)
        {
            Debug.Log("All ships of this size have already been added!");
            return false;
        }

        int xStart = Math.Max(Math.Min(x1 - 1, x2 - 1), 0);
        int xEnd = Math.Min(Math.Max(x1 + 1, x2 + 1), Height - 1);
        int yStart = Math.Max(Math.Min(y1 - 1, y2 - 1), 0);
        int yEnd = Math.Min(Math.Max(y1 + 1, y2 + 1), Width - 1);

        for (int i = xStart; i <= xEnd; i++)
        {
            for (int j = yStart; j <= yEnd; j++)
            {
                if (FieldCells[j][i].fieldStatus == FieldCell.FieldStatus.Ship)
                {
                    Debug.Log("In this place we have a ship!");
                    return false;
                }
            }
        }

        return true;
    }

    public bool AddShip(int size, int x1, int y1, int x2, int y2)
    {
        if (!CheckPlace(size, x1, y1, x2, y2)) return false;
        
        Ship ship = new Ship(size);
        for (int i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
        {
            for (int j = Math.Min(y1, y2); j <= Math.Max(y1, y2); j++)
            {
                FieldCells[j][i].Add(ship);
            }
        }
        
        ships[size - 1]++;
        
        Debug.Log("Ship added to the battlefield");
        return true;
    }

    public bool CheckForEmpty()
    {
        foreach (var ship in ships)
        {
            if (ship != 0)
                return false;
        }

        return true;
    }
}