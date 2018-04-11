using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Manager : MonoBehaviour {
	public static Scene_Manager Manager;
	[HideInInspector] public GameObject prefab_SceneTransition;
	void Awake(){
		if(Scene_Manager.Manager != null){
			Destroy(gameObject);
		}
		else{
			Manager = GetComponent<Scene_Manager>();
			DontDestroyOnLoad(gameObject);
		}
	}
	public void Load(string sceneName){
		if(sceneName == string.Empty){return;}
		GameObject _Inst = Instantiate(prefab_SceneTransition) as GameObject;
		_Inst.GetComponent<SceneTransition_Function>().LoadScene(sceneName);
	}
}
