using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Opponent
{
    private IBot strategy;

    public Opponent(IBot strategy, BattleField battleField)
    {
        this.strategy = strategy;
        this.strategy.BattleField = battleField;
    }

    public void ChangeStrategy(IBot strategy)
    {
        strategy.BattleField = this.strategy.BattleField;
        strategy.ShotsCount = this.strategy.ShotsCount;
        this.strategy = strategy;
    }

    public void MakeShot()
    {
        strategy.Shot();
    }
}

interface IBot
{
    BattleField BattleField { get; set; }
    int ShotsCount { get; set; }
    void Shot();
}

public class Bot : IBot
{
    public BattleField BattleField { get; set; }
    public int ShotsCount { get; set; }
    public int X { get; protected set; }
    public int Y { get; protected set; }

    public Bot()
    {
        X = Y = 0;
        ShotsCount = 0;
    }

    public Bot(BattleField battleField)
    {
        BattleField = battleField;
        X = Y = 0;
        ShotsCount = 0;
    }

    public virtual void Shot()
    {
        if (ShotsCount >= 100) return;
        ShotsCount++;
        bool find = false;
        for (int i = X; i < 10; i++)
        {
            for (int j = Y; j < 10; j++)
            {
                if (BattleField.FieldCells[i][j].fieldStatus == FieldCell.FieldStatus.Empty)
                {
                    find = true;
                    X = i;
                    Y = j;
                    break;
                }
            }

            if (find) break;
        }

        if (find) BattleField.Hit(X, Y);
    }
}

class AIBot : Bot, IBot
{
    public enum StateType
    {
        MISS = -1,
        HIT,
        KILL1,
        KILL2,
        KILL3,
        KILL4
    }

    public StateType State { get; private set; }
    public List<(int, int)> ShotsQueue { get; private set; }

    public AIBot()
    {
        ShotsQueue = new List<(int, int)>();
        State = StateType.MISS;

        X = Y = -1;
    }

    public AIBot(BattleField battleField) : base(battleField)
    {
        ShotsQueue = new List<(int, int)>();
        State = StateType.MISS;
        X = Y = -1;
    }

    public override void Shot()
    {
        if (ShotsCount >= 100) return;
        ShotsCount++;
        switch (State)
        {
            case StateType.MISS:
                if (ShotsQueue.Count == 0)
                {
                    RShot();
                }
                else
                {
                    State = (StateType) BattleField.Hit(ShotsQueue[0].Item1, ShotsQueue[0].Item2);
                    X = ShotsQueue[0].Item1;
                    Y = ShotsQueue[0].Item2;
                    ShotsQueue.RemoveAt(0);
                }

                break;
            case StateType.HIT:
                ShotsQueue.Clear();
                for (int i = 1; i < 4; i++)
                {
                    if (X - i > 0 && BattleField.FieldCells[X - i][Y].fieldStatus == FieldCell.FieldStatus.Empty)
                        ShotsQueue.Add((X - i, Y));
                    if (Y + i < BattleField.Width &&
                        BattleField.FieldCells[X][Y + i].fieldStatus == FieldCell.FieldStatus.Empty)
                        ShotsQueue.Add((X, Y + i));
                    if (X + i < BattleField.Height &&
                        BattleField.FieldCells[X + i][Y].fieldStatus == FieldCell.FieldStatus.Empty)
                        ShotsQueue.Add((X + i, Y));
                    if (Y - i > 0 && BattleField.FieldCells[X][Y - i].fieldStatus == FieldCell.FieldStatus.Empty)
                        ShotsQueue.Add((X, Y - i));
                }

                State = (StateType) BattleField.Hit(ShotsQueue[0].Item1, ShotsQueue[0].Item2);
                X = ShotsQueue[0].Item1;
                Y = ShotsQueue[0].Item2;
                ShotsQueue.RemoveAt(0);
                break;
            case StateType.KILL1:
            case StateType.KILL2:
            case StateType.KILL3:
            case StateType.KILL4:
                X = Y = -1;
                ShotsQueue.Clear();
                RShot();
                break;
            default:
                break;
        }
    }

    private void RShot()
    {
        Random random = new Random();
        int x = -1, y = -1;
        do
        {
            x = random.Next(0, BattleField.Height);
            y = random.Next(0, BattleField.Width);
        } while (BattleField.FieldCells[x][y].fieldStatus == FieldCell.FieldStatus.Empty);

        State = (StateType) BattleField.Hit(x, y);
        X = x;
        Y = y;
    }
}

class RandomBot : Bot, IBot
{
    public RandomBot()
    {
    }

    public RandomBot(BattleField battleField) : base(battleField)
    {
    }

    public override void Shot()
    {
        if (ShotsCount >= 100) return;
        ShotsCount++;
        Random random = new Random();
        int x, y;
        do
        {
            x = random.Next(0, BattleField.Height);
            y = random.Next(0, BattleField.Width);
        } while (BattleField.FieldCells[x][y].fieldStatus != FieldCell.FieldStatus.Empty);
        
        BattleField.Hit(x, y);
    }
}

