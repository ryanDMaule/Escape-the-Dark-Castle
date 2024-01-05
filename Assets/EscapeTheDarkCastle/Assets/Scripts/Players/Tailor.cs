public class Tailor : PlayerBase
{
    private readonly int might = 1;
    private int cunning = 4;
    private int wisdom = 3;

    public override void getPlayerDieValue(string rollValue, EnemyBase enemy)
    {
        switch (rollValue)
        {
            case "0":
                if (this.inventoryContainsCard("the replication stones_0"))
                {
                    enemy.reduceEnemyCunning(2);
                }
                else
                {
                    enemy.reduceEnemyCunning(1);
                }
                break;

            case "1":
                if (this.inventoryContainsCard("the replication stones_0"))
                {
                    enemy.reduceEnemyWisdom(2);
                }
                else
                {
                    enemy.reduceEnemyWisdom(1);
                }
                break;

            case "2":
                if (this.inventoryContainsCard("the replication stones_0"))
                {
                    enemy.reduceEnemyCunning(2);
                }
                else
                {
                    enemy.reduceEnemyCunning(1);
                }
                break;

            case "3":
                enemy.reduceEnemyCunning(2);
                setShieldActiveState(true);
                break;

            case "4":
                if (this.inventoryContainsCard("the replication stones_0"))
                {
                    enemy.reduceEnemyMight(2);
                }
                else
                {
                    enemy.reduceEnemyMight(1);
                }
                break;

            case "5":
                enemy.reduceEnemyWisdom(2);
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
            "0" or "2" or "3" => ChapterDieOptions.CUNNING,
            "4" => ChapterDieOptions.MIGHT,
            "1" or "5" => ChapterDieOptions.WISDOM,
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
