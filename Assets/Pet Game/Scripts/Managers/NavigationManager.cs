using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameScene { MAINMENU = 0, 	RECEPTIONView = 1, CATVIEW = 2, DOGVIEW = 3, BUNNYVIEW = 4, TIGERVIEW = 5, PANDAVIEW = 6, CAREERCATVIEW = 7, UNICORNVIEW = 8 }

public class NavigationManager : MonoBehaviour {

	#region Variables, Constants & Initializers

	public bool ShowDebugLogs;

	private Dictionary<string, Stack> navigationStacks = new Dictionary<string, Stack>();
	public Stack navigationStack;
	public GameScene launchScene;

	public GameObject mainMenu;
	public GameObject receptionView;
	public GameObject catView;
	public GameObject dogView;
	public GameObject bunnyView;
	public GameObject tigerView;
    public GameObject pandaView;
    public GameObject careerCatView;
    public GameObject unicornView;

    private GameObject runningScene;

	// persistant singleton
    private static NavigationManager _instance;

	#endregion
	
	#region Lifecycle methods

    public static NavigationManager instance
	{
		get
		{
			if(_instance == null)
			{
                _instance = GameObject.FindObjectOfType<NavigationManager>();

				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}
	
	void Awake() 
	{
		Debug.Log("Awake Called");

		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		OnBackKeyPressed ();
	}

	void Start ()
	{
        //Debug.Log("Start Called");

        // for any init behavior setup
		runningScene = null;
        navigationStack = new Stack();
		SetGameScene (launchScene);
    }
	
	void OnEnable()
	{
		//Debug.Log("OnEnable Called");

	}
	
	void OnDisable()
	{
		//Debug.Log("OnDisable Called");

	}

	#endregion

	#region Utility Methods 

	private void SetGameScene(GameScene scene) {
		if(runningScene != null) {
			Destroy (runningScene);
		}

		switch (scene) {
			case GameScene.MAINMENU:
				runningScene = GetGameSceneInstance (mainMenu);
				break;
			case GameScene.RECEPTIONView:
				runningScene = GetGameSceneInstance (receptionView);
				break;
			case GameScene.CATVIEW:
				runningScene = GetGameSceneInstance (catView);
				break;
			case GameScene.DOGVIEW:
				runningScene = GetGameSceneInstance (dogView);
				break;
			case GameScene.BUNNYVIEW:
				runningScene = GetGameSceneInstance (bunnyView);
				break;
			case GameScene.TIGERVIEW:
				runningScene = GetGameSceneInstance (tigerView);
				break;
			case GameScene.PANDAVIEW:
			    runningScene = GetGameSceneInstance(pandaView);
			    break;
            case GameScene.CAREERCATVIEW:
                runningScene = GetGameSceneInstance(careerCatView);
                break;
            case GameScene.UNICORNVIEW:
                runningScene = GetGameSceneInstance(unicornView);
                break;

        }

		navigationStack.Push (scene);

		runningScene.SetActive (true);
	}

	private GameObject GetGameSceneInstance(GameObject prefab) {
		GameObject gameScene = GameObject.Instantiate(prefab) as GameObject;
		gameScene.name = prefab.name;
		gameScene.GetComponent<Canvas>().worldCamera = Camera.main;

		return gameScene;
	}

	public void ReplaceScene(GameScene scene) {
		SetGameScene (scene);
	}

	public void ReplaceSceneWithClear(GameScene scene) {
		navigationStack.Clear();
		SetGameScene (scene);
	}

	#endregion

	#region Callback Methods 

	private void OnBackKeyPressed() {
#if UNITY_ANDROID || UNITY_WP8
		if (Input.GetKeyDown(KeyCode.Escape) && (!GameManager.instance.isGamePaused)) 
		{ 
			switch ((GameScene) navigationStack.Peek()) {
			case GameScene.MAINMENU:
				//Application.Quit();
				break;
			case GameScene.RECEPTIONView:
				navigationStack.Clear();
				ReplaceScene(GameScene.MAINMENU);
				break;
			case GameScene.CATVIEW:
                PlayerPrefs.SetInt("StoryMode", 0);
				ReplaceScene(GameScene.MAINMENU);
				break;
			case GameScene.DOGVIEW:
                PlayerPrefs.SetInt("StoryMode", 0);
				ReplaceScene(GameScene.MAINMENU);
				break;
			case GameScene.BUNNYVIEW:
                PlayerPrefs.SetInt("StoryMode", 0);
				ReplaceScene(GameScene.MAINMENU);
				break;
			case GameScene.TIGERVIEW:
                PlayerPrefs.SetInt("StoryMode", 0);
				ReplaceScene(GameScene.MAINMENU);
				break;
			case GameScene.PANDAVIEW:
                PlayerPrefs.SetInt("StoryMode", 0);
				ReplaceScene(GameScene.MAINMENU);
				break;
			}
		}
#endif
    }

    #endregion
}
