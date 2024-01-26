public class Cook : PlayerBase
{
    private readonly int might = 4;
    private int cunning = 3;
    private int wisdom = 1;
    public override void getPlayerDieValue(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "0":
                determineEnemyDamage(ChapterDieOptions.CUNNING, enemy, this);
                break;

            case "1":
                determineEnemyDamage(ChapterDieOptions.WISDOM, enemy, this);
                break;

            case "2" or "4":
                determineEnemyDamage(ChapterDieOptions.MIGHT, enemy, this);
                break;

            case "3":
                enemy.reduceEnemyMight(2);
                setShieldActiveState(true);
                break;

            case "5":
                enemy.reduceEnemyCunning(2);
                setShieldActiveState(true);
                break;

            default:
                break;
        }
    }

    public override ChapterDieOptions getCharacterRollResult(string rollValue)
    {
        return rollValue switch
        {
            "0" or "5" => ChapterDieOptions.CUNNING,
            "2" or "3" or "4" => ChapterDieOptions.MIGHT,
            "1" => ChapterDieOptions.WISDOM,
            _ => ChapterDieOptions.FAIL,
        };
    }

    public override int getPlayerMight()
    {
        return might;
    }

    public override int getPlayerCunning()
    {
        return cunning;
    }

    public override int getPlayerWisdom()
    {
        return wisdom;
    }
}
