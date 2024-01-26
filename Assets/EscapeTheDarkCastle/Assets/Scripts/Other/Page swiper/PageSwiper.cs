using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    [SerializeField] int totalpages;
    private int currentPage = 1;

    [SerializeField] public Button back_button;
    [SerializeField] public Button forward_button;

    [SerializeField] ChapterLogicNew cl;


    void Start()
    {
        panelLocation = transform.position;
    }

    void Update()
    {
        if(cl.getState() == BattleState.COMBAT_OPTIONS)
        {
            if (currentPage == totalpages)
            {
                forward_button.gameObject.SetActive(false);
            }
            else
            {
                forward_button.gameObject.SetActive(true);
            }

            if (currentPage == 1)
            {
                back_button.gameObject.SetActive(false);
            }
            else
            {
                back_button.gameObject.SetActive(true);
            }
        }

    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        //Screen.width not working??
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / 70f;
        if(Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if(percentage > 0 && currentPage < totalpages)
            {
                currentPage++;
                newLocation += new Vector3(-70f, 0, 0);
            } else if(percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newLocation += new Vector3(70f, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        } else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }

    public void backButton()
    {
        Vector3 newLocation = panelLocation;
        if (currentPage >= totalpages)
        {
            currentPage--;
            newLocation += new Vector3(70f, 0, 0);
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
    }

    public void forwardButton()
    {
        Vector3 newLocation = panelLocation;
        if (currentPage < totalpages)
        {
            currentPage++;
            newLocation += new Vector3(-70f, 0, 0);
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while(t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

}
