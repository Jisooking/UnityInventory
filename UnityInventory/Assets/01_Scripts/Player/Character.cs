public class Character
{        
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public StatusData Attack {get; private set;}
    public StatusData Defense {get; private set;}
    public StatusData Hp {get; private set;}
    public StatusData Critical {get; private set;}
    public int Gold {get; private set;}

    public Character()
    {
        
    }

    public void SetAttackStatus(StatusData status)
    {
        Attack = status;
    }
    public void SetDefenseStatus(StatusData status)
    {
        Defense = status;
    }

    public void SetHpStatus(StatusData status)
    {
        Hp = status;
    }

    public void SetCriticalStatus(StatusData status)
    {
        Critical = status;
    }

    public void SetGold(int gold)
    {
        Gold = gold;
    }
    public int GetAttackValue()
    {
        return Attack ? Attack.statusValue : 0;
    }

    public int GetDefenseValue()
    {
        return Defense ? Defense.statusValue : 0;
    }

    public int GetHpValue()
    {
        return Hp ? Hp.statusValue : 0;
    }

    public int GetCriticalValue()
    {
        return Critical? Critical.statusValue : 0;
    }
    public void LevelUp()
    {
        Level++;
    }
}
