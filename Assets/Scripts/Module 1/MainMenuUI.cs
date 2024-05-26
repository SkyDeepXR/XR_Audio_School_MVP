using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private List<CanvasGroup> uiPages = new(); // put them in order
    [SerializeField] private CanvasGroup splashScreen;

    [Header("Lesson")]
    [SerializeField] private ModuleManager moduleManager;
    [SerializeField] private List<GameObject> moduleGroupList = new(); 
    
    [Header("DEBUG")]
    [SerializeField] private CanvasGroup currentPage;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowPage(splashScreen);
        
        Invoke(nameof(GoToNextPage), 3f);
        
        
    }
    

    [Button]
    public void GoToPreviousPage()
    {
        int currentIndex = uiPages.IndexOf(currentPage);
        int previousIndex = (currentIndex - 1);
        if (previousIndex < 0)
            return;
        
        ShowPage(uiPages[previousIndex]);
    }

    [Button]
    public void GoToNextPage()
    {
        int currentIndex = uiPages.IndexOf(currentPage);
        int nextIndex = (currentIndex + 1) % uiPages.Count;
        if (nextIndex >= uiPages.Count)
            return;
        
        ShowPage(uiPages[nextIndex]);
    }
    
    public void ShowPage(CanvasGroup canvasGroupToShow)
    {
        currentPage = canvasGroupToShow;
        
        foreach (var uiPage in uiPages)
        {
            bool isCurrentPage = uiPage == canvasGroupToShow;
            uiPage.alpha = isCurrentPage ? 1 : 0;
            uiPage.interactable = isCurrentPage;
            uiPage.blocksRaycasts = isCurrentPage;
        }
    }

    public void GoToModuleAndCloseMainMenu(int moduleIndex)
    {
        gameObject.SetActive(false);

        moduleManager.StartModule(moduleIndex);
    }
    
}
