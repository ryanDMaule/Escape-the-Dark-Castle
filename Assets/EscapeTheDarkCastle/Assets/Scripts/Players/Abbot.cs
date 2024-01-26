public class Abbot : PlayerBase
{
    private int might = 3;
    private int cunning = 1;
    private int wisdom = 4;

    public override void getPlayerDieValue(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "0":
                determineEnemyDamage(ChapterDieOptions.CUNNING, enemy, this);
                break;

            case "1":
                determineEnemyDamage(ChapterDieOptions.MIGHT, enemy, this);
                break;

            case "2" or "4":
                determineEnemyDamage(ChapterDieOptions.WISDOM, enemy, this);
                break;

            case "3":
                enemy.reduceEnemyWisdom(2);
                setShieldActiveState(true);
                break;

            case "5":
                enemy.reduceEnemyMight(2);
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
            "0" => ChapterDieOptions.CUNNING,
            "1" or "5" => ChapterDieOptions.MIGHT,
            "2" or "3" or "4" => ChapterDieOptions.WISDOM,
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
