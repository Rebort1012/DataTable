
public class Hero
{

    public int Id
    {
        get;
    }

    public str Name
    {
        get;
    }

    public string PerfabName
    {
        get;
    }

    public enum EnumJob
    {
        ,
        Sword,
        Magic,
        Bow,
    }

    public enum Job
    {
        get;
    }

    public int Icon
    {
        get;
    }

    public int MP
    {
        get;
    }

    public int Exp
    {
        get;
    }

    public int Rank
    {
        get;
    }

    public int AttackID
    {
        get;
    }

    public int SkillID
    {
        get;
    }

    public bool IsHero
    {
        get;
    }

    public int TalentID
    {
        get;
    }

    public List<int> Params;

    public void Parse(string data)
    {
        
    }
}