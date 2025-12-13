using UnityEngine;

public class SpellbookPageManager : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int currentPageIndex;

    private void OnEnable()
    {
        GoToPage(0);
    }

    public void GoLeft()
    {
        if(currentPageIndex > 0)
        {
            GoToPage(--currentPageIndex);
        }
        else
        {
            GoToPage(pages.Length - 1);
        }
    }

    public void GoRight()
    {
        if (currentPageIndex < pages.Length - 1) 
        {
            GoToPage(++currentPageIndex);
        }
        else
        {
            GoToPage(0);
        }
    }

    private void GoToPage(int pageIndex)
    {
        if (currentPageIndex >= pages.Length)
            return;
        currentPageIndex = pageIndex;
        for (int i = 0; i < pages.Length; i++)
        {
            if (i != currentPageIndex)
            {
                pages[i].gameObject.SetActive(false);
            }
            else
            {
                pages[i].gameObject.SetActive(true);
            }
        }
    }
}
