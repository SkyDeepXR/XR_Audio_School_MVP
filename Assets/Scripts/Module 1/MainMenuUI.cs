using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private List<CanvasGroup> uiPages = new(); // put them in order

    [Header("Lesson")]
    [SerializeField] private ModuleManager moduleManager;
    [SerializeField] private List<GameObject> moduleGroupList = new(); 
    
    [Header("DEBUG")]
    [SerializeField] private CanvasGroup currentPage;
    [SerializeField, ReadOnly] private int currentPageIndex;

    [Header("AUDIO MANAGERS")] 
    [SerializeField] private OnboardAudioManager _onboardAudio;
    
    [SerializeField] private float bgmDelay = 3f; // Default delay of 3 seconds
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        ShowPage(0);
        
        Invoke(nameof(PlayBGMWithDelay), bgmDelay);
        
        //Invoke(nameof(GoToNextPage), 3f);
    }
    
    private void PlayBGMWithDelay()
    {
        _onboardAudio.PlayBGM();
        // Add any additional logic here
    }

    void Update()
    {
        // button click for splash screen
        if (currentPageIndex <= 2 && OVRInput.GetDown(OVRInput.Button.One))
        {
            GoToNextPage();
        }
    }
    

    [Button]
    public void GoToPreviousPage()
    {
        int previousIndex = currentPageIndex;

        do
        {
            previousIndex--;
        } while (uiPages[previousIndex - 1] == null && previousIndex >= 0); // skip empty pages
        
        if (previousIndex < 0)
            return;
        
        ShowPage(previousIndex);
    }

    [Button]
    public void GoToNextPage()
    {
        int nextIndex = currentPageIndex + 1;
        if (nextIndex >= uiPages.Count)
            return;
        
        ShowPage(nextIndex);
    }
    
    public void ShowPage(int newPageIndex)
    {
        currentPageIndex = newPageIndex;

        for (int i = 0; i < uiPages.Count; i++)
        {
            var uiPage = uiPages[i];
            bool isCurrentPage = i == currentPageIndex;
            if (uiPage== null)
            {
                continue;
            }
            uiPage.alpha = isCurrentPage ? 1 : 0;
            uiPage.interactable = isCurrentPage;
            uiPage.blocksRaycasts = isCurrentPage;
        }
        
        // index must match narration event index in OnboardAudioManager.cs
        _onboardAudio.PlayNarrationClip(currentPageIndex, uiPages[currentPageIndex] == null ? GoToNextPage : null);
    }
    
    public void ShowPage(CanvasGroup canvasGroupToShow)
    {
        currentPageIndex = uiPages.IndexOf(canvasGroupToShow);

        for (int i = 0; i < uiPages.Count; i++)
        {
            var uiPage = uiPages[i];
            bool isCurrentPage = i == currentPageIndex;
            if (uiPage== null)
            {
                continue;
            }
            uiPage.alpha = isCurrentPage ? 1 : 0;
            uiPage.interactable = isCurrentPage;
            uiPage.blocksRaycasts = isCurrentPage;
        }
        
        // index must match narration event index in OnboardAudioManager.cs
        _onboardAudio.PlayNarrationClip(currentPageIndex, uiPages[currentPageIndex] == null ? GoToNextPage : null);
    }

    public void GoToModuleAndCloseMainMenu(int moduleIndex)
    {
        gameObject.SetActive(false);
        _onboardAudio.StopBGM(); //stop backround music
        moduleManager.StartModule(moduleIndex);
    }
    
}
