using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpener : MonoBehaviour
{

    public GameObject panel; 

    public void openInventory()
    {
        Animator animator = panel.GetComponent<Animator>();

        bool isOpen = animator.GetBool("OpenBool");
        animator.SetBool("OpenBool", !isOpen);
    }

}
