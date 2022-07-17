using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Action ShowSettings;
    public static Action HideSettings;

    public static Action PauseGame;
    public static Action ResumeGame;

    public static Action FadeIn;
    public static Action FadeOut;
    public static Action WhiteIn;
    public static Action WhiteOut;

    public SceneCollection[] SceneCollections;

    public GameStates state;
    private IEnumerator stateTransition;

    //This list contains all currently loaded scenes except Persistent.
    private List<string> loadedScenes;
    private List<string> keepLoaded;
    private IEnumerator scenesLoading;

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    private void Start() {
        loadedScenes = new List<string>();
        keepLoaded = new List<string>();
        //keepLoaded.Add("Persistent");
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        ShowSettings += OnShowSettings;
        HideSettings += OnHideSettings;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        ShowSettings -= OnShowSettings;
        HideSettings -= OnHideSettings;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        if (scene.buildIndex != 0 && loadedScenes != null)
            loadedScenes.Add(scene.name);
    }

    private void OnSceneUnloaded(Scene scene) {
        if (scene.buildIndex != 0)
            loadedScenes.Remove(scene.name);
    }

    private bool UnloadAllScenes() {
        if (scenesLoading == null) {
            scenesLoading = UnloadScenesCoroutine();
            StartCoroutine(scenesLoading);
            return true;
        }
        else return false;
    }

    private IEnumerator UnloadScenesCoroutine() {
        List<string> scenesToUnload = new List<string>();
        foreach (string sceneName in loadedScenes) {
            bool keepSceneLoaded = false;
            foreach (string s in keepLoaded) {
                if (s.Equals(sceneName)) keepSceneLoaded = true;
            }
            if (!keepSceneLoaded) {
                scenesToUnload.Add(sceneName);
            }
        }

        foreach (string sceneName in scenesToUnload) {
            AsyncOperation op = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            while (!op.isDone) yield return null;
        }

        scenesLoading = null;
    }

    private bool LoadScenes(params string[] sceneNames) {
        if (scenesLoading == null) {
            scenesLoading = LoadScenesCoroutine(sceneNames);
            StartCoroutine(scenesLoading);
            return true;
        }
        else return false;
    }

    private IEnumerator LoadScenesCoroutine(params string[] sceneNames) {
        yield return new WaitForSecondsRealtime(0.01f);

        List<AsyncOperation> ops = new List<AsyncOperation>();
        foreach (string sceneName in sceneNames) {
            bool alreadyLoaded = false;
            foreach (string s in loadedScenes) {
                if (s.Equals(sceneName)) alreadyLoaded = true;
            }
            if (!alreadyLoaded) {
                Debug.Log("LOADING: " + sceneName);
                AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                op.allowSceneActivation = false;
                ops.Add(op);
                //Wait for each scene to finish loading
                //while (op.progress < 0.9f) yield return null;
            }
        }

        Debug.Log("ACTIVATING...");
        //Activate every loading scene
        foreach (AsyncOperation op in ops) {
            op.allowSceneActivation = true;
        }

        //Wait for each scene to activate
        foreach (AsyncOperation op in ops) {
            while (!op.isDone) yield return null;
        }
        Debug.Log("ACTIVATION COMPLETE");

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneNames[0]));

        Debug.Log("ACTIVE SCENE SET: " + sceneNames[0]);

        scenesLoading = null;
    }

    private SceneCollection FindSceneCollectionByName(string name) {
        for (int i = 0; i < SceneCollections.Length; i++) {
            if (SceneCollections[i].CollectionName.Equals(name)) return SceneCollections[i];
        }
        return null;
    }

    private IEnumerator GoToSceneCollection(SceneCollection sceneCollection) {
        Debug.Log("GO TO COLLECTION " + sceneCollection.CollectionName);
        if (SceneManager.sceneCount > 1) {
            Debug.Log("UNLOADING...");
            if (FadeOut != null) FadeOut();
            yield return new WaitForSecondsRealtime(2.0f);
            UnloadAllScenes();
            while (scenesLoading != null) yield return null;
            Debug.Log("UNLOADING COMPLETE");
        }

        Debug.Log("LOADING SCENES...");
        LoadScenes(sceneCollection.SceneNames);
        while (scenesLoading != null) yield return null;
        Debug.Log("LOADING COMPLETE");

        if(sceneCollection.MusicType != MusicTrackType.NoMusic) AudioManager.instance.PlayMusic(sceneCollection.MusicType);
        state = sceneCollection.State;

        if (HideSettings != null) HideSettings();
        if (FadeIn != null) FadeIn();

        stateTransition = null;
    }

    private void Update() {
        switch (state) {
            case GameStates.Initialize:
                if (stateTransition == null) {
                    stateTransition = GoToSceneCollection(FindSceneCollectionByName("TitleScreen"));
                    StartCoroutine(stateTransition);
                }
                break;
            case GameStates.Game:
                if (Input.GetButtonDown("Pause"))
                    Pause();
                break;
            case GameStates.GamePaused:
                if (Input.GetButtonDown("Pause"))
                    Resume();
                break;
            default:
                break;
        }
    }

    public void LaunchGame() {
        if (stateTransition == null) {
            stateTransition = GoToSceneCollection(FindSceneCollectionByName("Game"));
            StartCoroutine(stateTransition);
        }
    }

    public void LaunchLevel1() {
        if (stateTransition == null) {
            stateTransition = GoToSceneCollection(FindSceneCollectionByName("Level1"));
            StartCoroutine(stateTransition);
        }
    }

    public void LaunchNewGame() {
        LaunchGame();
    }

    public void Pause() {
    }

    public void Resume() {

    }

    public void GoToMainMenu() {
        if (stateTransition == null) {
            stateTransition = GoToSceneCollection(FindSceneCollectionByName("TitleScreen"));
            StartCoroutine(stateTransition);
        }
    }

    private void OnShowSettings() {

    }

    private void OnHideSettings() {

    }
}

public enum GameStates
{
    Initialize,
    TitleScreen,
    GamePaused,
    GameSettings,
    Transition,
    Game
}

[Serializable]
public class SceneCollection
{
    public string CollectionName;
    public string[] SceneNames;
    public bool ShowCursor;
    public MusicTrackType MusicType;
    public GameStates State;
}