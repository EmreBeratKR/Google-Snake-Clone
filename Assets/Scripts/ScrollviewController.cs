using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollviewController : MonoBehaviour
{
    [SerializeField] RectTransform scrollview;
    [SerializeField] GameObject[] items;
    public int selectedItem = 0;
    float targetX;
    Vector3 targetPos;
    Vector3 snapVector;
    bool isDragging = false;
    bool isClicking = false;

    void Start()
    {
        targetPos = items[selectedItem].GetComponent<RectTransform>().position;
        targetX = targetPos.x;
    }

    void Update()
    {
        if (!isClicking)
        {
            checkItemsPos();
        }
        highlightSelectedItem();
    }

    void checkItemsPos() // calculates which item is closest to mid
    {
        float minDistance = Mathf.Abs(items[selectedItem].GetComponent<RectTransform>().position.x - targetX);
        for (int i = 0; i < items.Length; i++)
        {
            if (Mathf.Abs(items[i].GetComponent<RectTransform>().position.x - targetX) < minDistance)
            {
                minDistance = Mathf.Abs(items[i].GetComponent<RectTransform>().position.x - targetX);
                selectedItem = i;
            }
        }
        snapVector = targetPos - items[selectedItem].GetComponent<RectTransform>().position;
    }

    void highlightSelectedItem() // highlights the selected item
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (i == selectedItem)
            {
                items[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                items[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
            }
        }
    }

    public void selectClickedItem() // selects the clicked item
    {
        if (!isDragging && !isClicking)
        {
            isClicking = true;
            Vector2 clickedPos = Input.mousePosition;
            RectTransform itemTransform;
            for (int i = 0; i < items.Length; i++)
            {
                itemTransform = items[i].GetComponent<RectTransform>();
                if ((itemTransform.position.x - itemTransform.sizeDelta.x/2f) < clickedPos.x && (itemTransform.position.x + itemTransform.sizeDelta.x/2f) > clickedPos.x)
                {
                    selectedItem = i;
                    break;
                }
            }
            snapVector = targetPos - items[selectedItem].GetComponent<RectTransform>().position;
            StartCoroutine(snapToMid());
        }
    }

    public void startDrag()
    {
        isDragging = true;
    }

    public void endDrag() // executes 1 time when mouse dragging ends
    {
        StartCoroutine(snapToMid());
    }

    IEnumerator snapToMid() // snaps the selected item to middle
    {
        for (int i = 0; i < 60f; i++)
        {
            yield return new WaitForSeconds(0.01f/60f);
            scrollview.position += snapVector / 60;
        }
        isDragging = false;
        isClicking = false;
    }

    IEnumerator itemBreathIn() // scales up the item size
    {
        for (int i = 0; i < 60; i++)
        {
            //transform.GetChild(0).localScale += new Vector3(0.005f, 0.005f, 0);
            yield return new WaitForSeconds(1f/120f);
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(itemBreathOut());
    }

    IEnumerator itemBreathOut() // scales down the item size
    {
        for (int i = 0; i < 60; i++)
        {
            //transform.GetChild(0).localScale -= new Vector3(0.005f, 0.005f, 0);
            yield return new WaitForSeconds(1f/120f);
        }
        StartCoroutine(itemBreathIn());
    }
}