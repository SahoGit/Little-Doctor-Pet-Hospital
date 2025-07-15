using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using BeeAdsSDK;

public class GameManager : MonoBehaviour {

	#region Variables, Constants & Initializers

	public bool ShowDebugLogs;

	public ArrayList charactersDataList;
	public ArrayList hatsDataList;
	public ArrayList beardStylesDataList;
	public ArrayList glassesDataList;

	[HideInInspector]
	public bool isGameFirstLoop;
	[HideInInspector]
	public string currentSalonMode;
	[HideInInspector]
	public string currentScene;
	[HideInInspector]
	public string currentItem;
	[HideInInspector]
	public string woundTag;
	[HideInInspector] 
	public ContactPoint2D contact;
	public BaseItem selectedCharacter;

	[HideInInspector]
	public bool isGamePaused;
	
	// persistant singleton
    private static GameManager _instance;

	#endregion
	
	#region Lifecycle methods

    public static GameManager instance
	{
		get
		{
			if(_instance == null)
			{
                _instance = GameObject.FindObjectOfType<GameManager>();

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
		
	}

	void Start ()
	{
		Debug.Log("Start Called");

		// for any init behavior setup
		this.isGamePaused = false;
		this.isGameFirstLoop = true;

		this.SetData();
	}
	
	void OnEnable()
	{
		Debug.Log("OnEnable Called");

	}
	
	void OnDisable()
	{
		Debug.Log("OnDisable Called");

	}

	#endregion

	#region Utility Methods 

	private void SetData() {
		this.charactersDataList = DataProvider.GetCharactersDataList ();
		this.hatsDataList = DataProvider.GetHatsDataList ();
		this.glassesDataList = DataProvider.GetGlassesDataList ();
		this.beardStylesDataList = DataProvider.GetBeardStylesDataList ();
	}

	public void LogDebug(string message) {
		if (ShowDebugLogs)
			Debug.Log ("GameManager >> " + message);
	}
	
	private void LogErrorDebug(string message) {
		if (ShowDebugLogs)
			Debug.LogError ("GameManager >> " + message);
	}

	#endregion

	#region Callback Methods 
	public IEnumerator FillLoading(Image img)
	{
		if (img.fillAmount < 1)
		{
			img.fillAmount = img.fillAmount + 0.009f;
			yield return new WaitForSeconds(0.02f);
			StartCoroutine(FillLoading(img));
		}
		else if (img.color.a >= 1f)
		{
			StopCoroutine(FillLoading(img));
		}
	}

	#endregion
}
