using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TUTORIAL : https://www.youtube.com/watch?v=rjFgThTjLso

public class PageSwiperNew : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    public float percentThreshold = 0.2f;
    public float easing = 0.5f;

    [SerializeField] int totalpages;
    private int currentPage = 1;

    [SerializeField] public Button back_button;
    [SerializeField] public Button forward_button;

    [SerializeField] ChapterLogicBase clBase;

    //Screen.width seems to think the width is larger than a phone
    private float screenWidth = 55f;

    void Start()
    {
        panelLocation = transform.position;
        arrowFormatting();
    }

    public void arrowFormatting()
    {
        if (currentPage == 1)
        {
            back_button.gameObject.SetActive(false);
            forward_button.gameObject.SetActive(true);
        }
        else if (currentPage > 1 && currentPage < totalpages)
        {
            back_button.gameObject.SetActive(true);
            forward_button.gameObject.SetActive(true);
        }
        else if (currentPage == totalpages)
        {
            back_button.gameObject.SetActive(true);
            forward_button.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / screenWidth;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && currentPage < totalpages)
            {
                currentPage++;
                newLocation += new Vector3(-screenWidth, 0, 0);

            }
            else if (percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newLocation += new Vector3(screenWidth, 0, 0);

            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
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
            newLocation += new Vector3(screenWidth, 0, 0);

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
            newLocation += new Vector3(-screenWidth, 0, 0);

            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        arrowFormatting();

        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
