using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition_Function : MonoBehaviour {

	int state;
	public string sceneNameID;
	float delay_TIMER;
	[HideInInspector] public Transform myTransform;
	void Awake(){
		delay_TIMER = Time.time + 0.4f;
		DontDestroyOnLoad(gameObject);
	}
	public void LoadScene(string sceneNameID){
		this.sceneNameID = sceneNameID;
	}
	// Update is called once per frame
	void Update () {
		switch(state){
		case 0:
			if(Time.time < delay_TIMER){return;}
			if(myTransform.localPosition.x > 0){
				myTransform.localPosition = Vector3.Lerp(
					myTransform.localPosition,
					new Vector3(0 - 0.1f,myTransform.localPosition.y,myTransform.localPosition.z),
					0.24f);
				if(myTransform.localPosition.x <= 0){
					myTransform.localPosition = new Vector3(0,myTransform.localPosition.y,myTransform.localPosition.z);
				}
			}
			else{
				delay_TIMER = Time.time + 1f;
				state++;
			}
			break;
		case 1:
			if(Time.time < delay_TIMER){return;}
			SceneManager.LoadScene(sceneNameID);
			state++;
			break;
		case 2:
			if(_currentScene == sceneNameID){
				state++;
			}
			break;
		case 3:
			if(myTransform.localPosition.x > -25f){
				myTransform.localPosition = Vector3.Lerp(
					myTransform.localPosition,
					new Vector3(-25f - 0.1f,myTransform.localPosition.y,myTransform.localPosition.z),
					0.24f);
				if(myTransform.localPosition.x <= -25f){
					myTransform.localPosition = new Vector3(-25f,myTransform.localPosition.y,myTransform.localPosition.z);
				}
			}
			else{
				state++;
			}
			break;
		case 4:
			Destroy(gameObject);
			break;
		}
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	void OnDisable(){
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	string _currentScene;
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode){
		_currentScene = scene.name;
		System.GC.Collect();
	}
}
