using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHud : MonoBehaviour
{

    //TODO: Make enemy generic and move these into Skelton class

    //TODO: look to make these prefabs which get generated instead of having the exact value passed thropugh for everything
    //DESTROY them when done.

    [SerializeField] Image background;

    [SerializeField] Image backButton;
    [SerializeField] Image forwardButton;

    [SerializeField] Image Frame_1;
    [SerializeField] Text title_1;
    [SerializeField] Text description_1;
    [SerializeField] Image mightImage_1;
    [SerializeField] Image cunningImage_1;
    [SerializeField] Image WisdomImage_1;
    [SerializeField] Image randomImage_1;
    [SerializeField] Text mightValue_1;
    [SerializeField] Text cunningValue_1;
    [SerializeField] Text wisdomValue_1;
    [SerializeField] Text randomValue_1;
    [SerializeField] Image enemyDamage_1;
    [SerializeField] Button continue_1;

    [SerializeField] Image Frame_2;
    [SerializeField] Text title_2;
    [SerializeField] Text description_2;
    [SerializeField] Image mightImage_2;
    [SerializeField] Image cunningImage_2;
    [SerializeField] Image WisdomImage_2;
    [SerializeField] Image RandomImage_2;
    [SerializeField] Text mightValue_2;
    [SerializeField] Text cunningValue_2;
    [SerializeField] Text wisdomValue_2;
    [SerializeField] Text randomValue_2;
    [SerializeField] Image enemyDamage_2;
    [SerializeField] Button continue_2;


    public void showOptionsHUD()
    {
        background.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        forwardButton.gameObject.SetActive(true);

        Frame_1.gameObject.SetActive(true);
        title_1.gameObject.SetActive(true);
        description_1.gameObject.SetActive(true);
        mightImage_1.gameObject.SetActive(true);
        cunningImage_1.gameObject.SetActive(true);
        WisdomImage_1.gameObject.SetActive(true);
        randomImage_1.gameObject.SetActive(true);
        mightValue_1.gameObject.SetActive(true);
        cunningValue_1.gameObject.SetActive(true);
        wisdomValue_1.gameObject.SetActive(true);
        randomValue_1.gameObject.SetActive(true);
        enemyDamage_1.gameObject.SetActive(true);
        continue_1.gameObject.SetActive(true);

        Frame_2.gameObject.SetActive(true);
        title_2.gameObject.SetActive(true);
        description_2.gameObject.SetActive(true);
        mightImage_2.gameObject.SetActive(true);
        cunningImage_2.gameObject.SetActive(true);
        WisdomImage_2.gameObject.SetActive(true);
        RandomImage_2.gameObject.SetActive(true);
        mightValue_2.gameObject.SetActive(true);
        cunningValue_2.gameObject.SetActive(true);
        wisdomValue_2.gameObject.SetActive(true);
        randomValue_2.gameObject.SetActive(true);
        enemyDamage_2.gameObject.SetActive(true);
        continue_2.gameObject.SetActive(true);
    }

    public void hideOptionsHUD()
    {
        background.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        forwardButton.gameObject.SetActive(false);

        Frame_1.gameObject.SetActive(false);
        title_1.gameObject.SetActive(false);
        description_1.gameObject.SetActive(false);
        mightImage_1.gameObject.SetActive(false);
        cunningImage_1.gameObject.SetActive(false);
        WisdomImage_1.gameObject.SetActive(false);
        randomImage_1.gameObject.SetActive(false);
        mightValue_1.gameObject.SetActive(false);
        cunningValue_1.gameObject.SetActive(false);
        wisdomValue_1.gameObject.SetActive(false);
        randomValue_1.gameObject.SetActive(false);
        enemyDamage_1.gameObject.SetActive(false);
        continue_1.gameObject.SetActive(false);

        Frame_2.gameObject.SetActive(false);
        title_2.gameObject.SetActive(false);
        description_2.gameObject.SetActive(false);
        mightImage_2.gameObject.SetActive(false);
        cunningImage_2.gameObject.SetActive(false);
        WisdomImage_2.gameObject.SetActive(false);
        RandomImage_2.gameObject.SetActive(false);
        mightValue_2.gameObject.SetActive(false);
        cunningValue_2.gameObject.SetActive(false);
        wisdomValue_2.gameObject.SetActive(false);
        randomValue_2.gameObject.SetActive(false);
        enemyDamage_2.gameObject.SetActive(false);
        continue_2.gameObject.SetActive(false);
    }
}
