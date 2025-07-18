﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UniversalAnalytics : MonoBehaviour
{
	#region Variables, Constants & Initializers
	public bool isTesting;
	public bool showDebugLogs;

	//public GoogleAnalyticsV4 GAComponent;
	//public static GoogleAnalyticsV4 googleAnalytics;

	// persistant singleton
	private static UniversalAnalytics _instance;

	#endregion

	#region Lifecycle methods

	private static UniversalAnalytics instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<UniversalAnalytics>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}

	void Awake() {
		LogDebug("Awake Called");
		
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

	void Start() {
		LogDebug("Start Called");

		//googleAnalytics = GAComponent;
	}
	
	void Destroy() {
		LogDebug("Destroy Called");
	}

	void OnEnable()
	{
		LogDebug("OnEnable Called");
		
	}
	
	void OnDisable()
	{
		LogDebug("OnDisable Called");
		
	}
	
	#endregion

	#region Callback Methods
    
   
    public static void LogEvent(string category, string action, string label, int value = 0)
    {
		UniversalAnalytics._instance.LogDebug ("Category: " + category + " | Action: " + action + " | Label: " + label);
		if (UniversalAnalytics._instance.isTesting) {
			return;
		} else {
#if UNITY_WP8
			if (Assets.Plugins.GoogleAnalyticsWP8.sharedManager().GetLogEventDelegate() != null)
			{
				Assets.Plugins.GoogleAnalyticsWP8.sharedManager().GetLogEventDelegate().Invoke(category, action, label, value);
			}
#else
			/*if (googleAnalytics != null) {
				googleAnalytics.LogEvent (category, action, label, value);
			}*/
#endif
		}
    }

	public void LogDebug(string message) {
		if (showDebugLogs)
			Debug.Log ("UniversalAnalyticsX >> " + message);
	}

	#endregion
}