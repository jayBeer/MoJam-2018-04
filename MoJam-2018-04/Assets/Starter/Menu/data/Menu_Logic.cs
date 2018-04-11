using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Logic : MonoBehaviour {

	[HideInInspector] public int Panel; int Panel_TOGGLE;
	[HideInInspector] public Transform[] Panel_Transform;

	[HideInInspector] public int Panel_Instruction;
	[HideInInspector] public Transform Panel_Instruction_Page_Transform;
	[HideInInspector] public Transform Panel_Instruction_Nav_Transform;
	[HideInInspector] public GameObject[] Panel_Instruction_NavButton_GameObject = new GameObject[2];

	[HideInInspector] public string Name_Game;
	[HideInInspector] public Text Name_Game_TEXT;
	[HideInInspector] public string Name_Team;
	[HideInInspector] public Text[] Name_Team_TEXT;
	[HideInInspector] public string[] Name_TeamMember;
	[HideInInspector] public Text Name_TeamMember_TEXT;
	public string Scene_PlayNameID = "game";

	public GameInfo myGameInfo;

	// Use this for initialization
	void Start () {
		Setup_GameInfo();
		Setup_Text();
		Reset_Instruction_Page();
	}
	void Setup_GameInfo(){
		Name_Game = myGameInfo.Name_Game;
		Name_Team = myGameInfo.Name_Team;
		for(int i = 0; i < myGameInfo.Name_TeamMember.Length; i++){
			Name_TeamMember[i] = myGameInfo.Name_TeamMember[i];
		}
	}
	void Setup_Text(){
		Name_Game_TEXT.text = Name_Game;
		for(int i = 0; i < Name_Team_TEXT.Length; i++){
			Name_Team_TEXT[i].text = Name_Team;
		}
		Name_Team_TEXT[0].text += " Presents";
		Name_TeamMember_TEXT.text = "";
		for(int i = 0; i < Name_TeamMember.Length; i++){
			Name_TeamMember_TEXT.text += Name_TeamMember[i]+"\n";
		}
	}
	bool isLoading = false;
	public void Call_Play(){
		if(Scene_PlayNameID == string.Empty){return;}
		if(isLoading == true){return;}
		Scene_Manager.Manager.Load(Scene_PlayNameID);
		isLoading = true;
	}

	public void Call_PanelID(int ID){
		if(isLoading == true){return;}
		Audio_Manager.Manager.Call_PlaySFX("button");
		Panel_TOGGLE = -1;
		Panel = ID;
	}
	public void Call_Quit(){
		if(isLoading == true){return;}
		Application.Quit();
	}
	public void Call_InstructionPageID(int toggle){
		if(isLoading == true){return;}
		if(toggle > 0){
			Panel_Instruction ++;
		}
		else if(toggle <= 0){
			Panel_Instruction --;
		}
		if(Panel_Instruction > 2){Panel_Instruction = 2;}
		if(Panel_Instruction < 0){Panel_Instruction = 0;}
	}

	// Update is called once per frame
	void Update () {
		if(Panel_TOGGLE != Panel){
			if(Panel < 0){Panel = 0;}
			int _check_i = 0;
			for(int i = 0; i < Panel_Transform.Length; i++){
				if(Panel_Transform[i] != null && i != 0){
					if(i == Panel){
						if(Function_Panel_Transition_Effect_Bot(Panel_Transform[i], true) == false){
							_check_i++;
						}
					}
					else{
						if(Function_Panel_Transition_Effect_Bot(Panel_Transform[i], false) == false){
							_check_i++;
						}
					}
				}
			}
			if(_check_i == 0){
				Panel_TOGGLE = Panel;
			}
		}
		Function_Instruction_Page(Panel_Instruction);
	}
	public void Reset_Instruction_Page(){
		Panel_Instruction_Page_Transform.localPosition = new Vector3(
			0,
			Panel_Instruction_Page_Transform.localPosition.y,
			Panel_Instruction_Page_Transform.localPosition.z);
		Panel_Instruction = 0;
		Panel_Instruction_Nav_Transform.localPosition = new Vector3(
			-60,
			Panel_Instruction_Nav_Transform.localPosition.y,
			Panel_Instruction_Nav_Transform.localPosition.z);
		Panel_Instruction_NavButton_GameObject[0].SetActive(false);
		Panel_Instruction_NavButton_GameObject[1].SetActive(true);
	}
	void Function_Instruction_Page(int page){
		if(page > 2){page = 2;}
		if(page < 0){page = 0;}
		float _pagepos_f = -2000 * page;
		if(Panel_Instruction_Page_Transform.localPosition.x == _pagepos_f){return;}

		float _speed_f = 0.2f;
		float _navpos_f = -60 + (page * 60);
		if(page <= 0){
			Panel_Instruction_NavButton_GameObject[0].SetActive(false);
			Panel_Instruction_NavButton_GameObject[1].SetActive(true);
		}
		else if(page >= 2){
			Panel_Instruction_NavButton_GameObject[0].SetActive(true);
			Panel_Instruction_NavButton_GameObject[1].SetActive(false);
		}
		else{
			Panel_Instruction_NavButton_GameObject[0].SetActive(true);
			Panel_Instruction_NavButton_GameObject[1].SetActive(true);
		}
		if(Panel_Instruction_Nav_Transform.localPosition.x != _navpos_f){
			Panel_Instruction_Nav_Transform.localPosition = new Vector3(
				_navpos_f,
				Panel_Instruction_Nav_Transform.localPosition.y,
				Panel_Instruction_Nav_Transform.localPosition.z);
		}
		if(Panel_Instruction_Page_Transform.localPosition.x != _pagepos_f){
			if(Panel_Instruction_Page_Transform.localPosition.x < _pagepos_f){
				Panel_Instruction_Page_Transform.localPosition = Vector3.Lerp(
					Panel_Instruction_Page_Transform.localPosition,
					new Vector3(_pagepos_f + 0.1f,Panel_Instruction_Page_Transform.localPosition.y,Panel_Instruction_Page_Transform.localPosition.z),
					_speed_f);
				if(Panel_Instruction_Page_Transform.localPosition.x >= _pagepos_f){
					Panel_Instruction_Page_Transform.localPosition = new Vector3(
						_pagepos_f,
						Panel_Instruction_Page_Transform.localPosition.y,
						Panel_Instruction_Page_Transform.localPosition.z);
				}
			}
			else if(Panel_Instruction_Page_Transform.localPosition.x > _pagepos_f){
				Panel_Instruction_Page_Transform.localPosition = Vector3.Lerp(
					Panel_Instruction_Page_Transform.localPosition,
					new Vector3(_pagepos_f - 0.1f,Panel_Instruction_Page_Transform.localPosition.y,Panel_Instruction_Page_Transform.localPosition.z),
					_speed_f);
				if(Panel_Instruction_Page_Transform.localPosition.x <= _pagepos_f){
					Panel_Instruction_Page_Transform.localPosition = new Vector3(
						_pagepos_f,
						Panel_Instruction_Page_Transform.localPosition.y,
						Panel_Instruction_Page_Transform.localPosition.z);
				}
			}
		}
	}

	bool Function_Panel_Transition_Effect_Bot(Transform thisTransform, bool toggle){
		float _speed_f = 0.2f;
		float _offpos_f = 2000;
		switch(toggle){
		case true:
			if(thisTransform.localPosition.y != 0){
				thisTransform.localPosition = Vector3.Lerp(
					thisTransform.localPosition,
					new Vector3(thisTransform.localPosition.x,0 + 0.1f,thisTransform.localPosition.z),
					_speed_f);
				if(thisTransform.localPosition.y >= 0){
					thisTransform.localPosition = new Vector3(thisTransform.localPosition.x,0,thisTransform.localPosition.z);
				}
				return false;
			}
			return true;
		default:
			if(thisTransform.localPosition.y != -_offpos_f){
				thisTransform.localPosition = Vector3.Lerp(
					thisTransform.localPosition,
					new Vector3(thisTransform.localPosition.x,-_offpos_f + -0.1f,thisTransform.localPosition.z),
					_speed_f);
				if(thisTransform.localPosition.y <= -_offpos_f){
					thisTransform.localPosition = new Vector3(thisTransform.localPosition.x,-_offpos_f,thisTransform.localPosition.z);
				}
				return false;
			}
			return true;
		}
	}
}
