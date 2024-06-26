using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI; // Ensure this is added to handle UI components directly

public class UIManager : MonoBehaviour
{
    //[SerializeField] private SimplePrefabSpawner spawner;
    [SerializeField] private EquipmentTransporter _transporter;
    [SerializeField] private Button _continueButton1; // For the first continue
    [SerializeField] private Button _continueButton2; // For the second continue
    [SerializeField] private Button _continueButton3; // For the third continue
    [SerializeField] private Button _skipButton;
    [SerializeField] private GameObject _helpCanvas; // UI canvas that contains help materials

    [SerializeField] private GameObject _onboard01;
    [SerializeField] private GameObject _onboard02;
    [SerializeField] private GameObject _onboard03;
    [SerializeField] private GameObject _onboardMain;
    [SerializeField] private GameObject _diagram;
    [SerializeField] private GameObject _xlrCanvas;

    [SerializeField] private GameObject _microphone;

    [SerializeField] private GameObject _cableGroup;

    [SerializeField] private AudioSource _canvasAudioSource;
    [SerializeField] private AudioClip _audioBoard01;
    [SerializeField] private AudioClip _audioBoard02;
    [SerializeField] private AudioClip _audioBoard03;
    [SerializeField] private AudioClip _audioBoard04;
    [SerializeField] private AudioClip _audioBoard05;
    [SerializeField] private AudioClip _audioBoard06;

    [SerializeField] private AudioSource _bGM;
    
    private bool[] _connections = new bool[6];  // Array to track the connection status of six connectors
   

    void Start()
    {
        StartCoroutine(DelayBeforeVoiceover(2));
        _bGM.Play(0);
        
        // Add listeners to buttons
        _continueButton1.onClick.AddListener(ShowMessageTwo);
        _continueButton2.onClick.AddListener(ShowMessage3);
        _continueButton3.onClick.AddListener(IntroToXLRCables);
        _skipButton.onClick.AddListener(HandleSkip);
        // startButton.onClick.AddListener(StartLesson);

        // Initially disable object placement and help canvas
        _transporter.DisableTransport();
        // helpCanvas.enabled = false; // Ensure the help canvas is initially disabled
        
        _onboardMain.SetActive(false);
        _onboard01.SetActive(false);
        _onboard02.SetActive(false);
        _onboard03.SetActive(false);

        ShowOnboarding();
        
        }

    public void ShowOnboarding()
    {
        StartCoroutine(DelayBeforeVoiceover(3));
        _onboardMain.SetActive(true);
        _onboard01.SetActive(true);
        // Display initial onboarding message
        Debug.Log("Welcome to Audio School: Learn your sound tech skills!");
        // Additional logic for playing voiceover or showing visuals can be added here
        
        StartCoroutine(DelayBeforeVoiceover(3));
        _canvasAudioSource.PlayOneShot(_audioBoard01);
    }

    public void ShowMessageTwo()
    {
        // Continue to the next part of onboarding
        _onboard02.SetActive(true);
        
        Debug.Log("Lesson 1: Mapping the Vibrations - Connect the system.");
        
       // Additional voiceover or visual effects can be triggered here
        StartCoroutine(DelayBeforeVoiceover(3));
        _canvasAudioSource.PlayOneShot(_audioBoard02);
    }

    public void ShowMessage3()
    {
        _onboard03.SetActive(true);
        // Show the message to prepare for the lesson
        Debug.Log("Let's get sound out of the system. Avoid feedback.");
        _transporter.EnableTransport(); // Enable placing objects if part of onboarding requires this
        
        // Additional voiceover or visual effects can be triggered here
        StartCoroutine(DelayBeforeVoiceover(3));
        _canvasAudioSource.PlayOneShot(_audioBoard03);
    }

    public void IntroToXLRCables()
    {
        _onboardMain.SetActive(false);
        
        // Show Audio Signal Flow Diagram
        _diagram.SetActive(true);
        
        
        StartCoroutine(DelayBeforeVoiceover(2));
        _canvasAudioSource.PlayOneShot(_audioBoard04);
        StartCoroutine(DelayBeforeStart());
        
    }

    public void StartLesson()
    {
        //Show XLR image. 
        _xlrCanvas.SetActive(true);
        
        // Actions to start the actual lesson
        Debug.Log("Starting the lesson...");
        _transporter.EnableTransport();
        _helpCanvas.SetActive(true); // Activate the help canvas
        _cableGroup.SetActive(true);
        
        
    }

    public void HandleSkip()
    {
        
        Debug.Log("Skipping to lesson.");
        StartLesson(); // Use StartLesson to unify the behavior
    }

    private IEnumerator DelayBeforeVoiceover(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
    }
    
    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSecondsRealtime(5);
        StartLesson();
        yield return new WaitForSecondsRealtime(15); // Here we leave time for the player to spawn gear.

    }

    public void FinishLesson()
    {
        _canvasAudioSource.PlayOneShot(_audioBoard06);
        StartCoroutine(DelayBeforeVoiceover(8));
        
        _canvasAudioSource.PlayOneShot(_audioBoard05);
    }
    
    public void UpdateConnectionStatus(int index, bool status)
    {
        _connections[index] = status;
        CheckAllConnections();
    }

    private void CheckAllConnections()
    {
        foreach (bool isConnected in _connections)
        {
            if (!isConnected) return;  // If any connection is not made, exit the method
        }
        FinishLesson();  // Call FinishLesson when all connections are made
    }

   
    
}