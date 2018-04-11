using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour {

	public static Audio_Manager Manager;
	void Awake(){
		if(Audio_Manager.Manager != null){
			Destroy(gameObject);
		}
		else{
			Manager = GetComponent<Audio_Manager>();
			DontDestroyOnLoad(gameObject);
		}
	}

	[System.Serializable]
	public class _Music{
		public string MusicCodeID;
		public AudioClip Mucic_Clip;
		public float volume;
	}
	public _Music[] Music;
	AudioSource myMusic_AudioSource;

	[System.Serializable]
	public class _SoundEffect{
		public string SoundCodeID;
		public AudioClip[] Sound_Clip = new AudioClip[1];
		public float volumeMin = 1f;
		public float volumeMax = 1f;
		public float pitchMin = 1f;
		public float pitchMax = 1f;
	}
	public _SoundEffect[] SoundEffect;
	int mySFX_AudioSource_NUMBER;
	AudioSource[] mySFX_AudioSource = new AudioSource[10];
	// Use this for initialization
	void Start () {
		GameObject _Inst = new GameObject();
		_Inst.transform.parent = transform;
		_Inst.name = "AudioSources";
		myMusic_AudioSource = _Inst.AddComponent<AudioSource>() as AudioSource;
		myMusic_AudioSource.loop = true;
		for(int i = 0; i < mySFX_AudioSource.Length; i++){
			mySFX_AudioSource[i] = _Inst.AddComponent<AudioSource>() as AudioSource;
		}
		for(int i = 0; i < SoundEffect.Length; i++){
			if(SoundEffect[i].SoundCodeID != string.Empty){
				if(SoundEffect[i].volumeMin <= 0 && SoundEffect[i].volumeMax <= 0){
					SoundEffect[i].volumeMin = 1;
					SoundEffect[i].volumeMax = 1;
				}
				if(SoundEffect[i].pitchMin <= 0 && SoundEffect[i].pitchMax <= 0){
					SoundEffect[i].pitchMin = 1;
					SoundEffect[i].pitchMax = 1;
				}
			}
		}
	}
	public void Call_PlayMusic(string MusicCodeID) {
		int _id_i = -1;
		for(int i = 0; i < Music.Length; i++){
			if(Music[i].MusicCodeID != string.Empty && Music[i].MusicCodeID == MusicCodeID){
				_id_i = i;
				break;
			}
		}
		if(_id_i == -1){return;}
		myMusic_AudioSource.clip = Music[_id_i].Mucic_Clip;
		if(Music[_id_i].volume != 0){
			myMusic_AudioSource.volume = Music[_id_i].volume;
		}
		else{
			myMusic_AudioSource.volume = 1;
		}
		myMusic_AudioSource.Play();
	}
	public void Call_PlaySFX(string SoundCodeID){
		Call_PlaySFX(SoundCodeID,null);
	}
	public void Call_PlaySFX(string SoundCodeID, Transform SoundSource) {
		int _id_i = -1;
		float _distance_f = 1;
		if(SoundSource != null){
			_distance_f = Camera_SFX_Volume(SoundSource);
			if(_distance_f <= 0){return;}
		}
		for(int i = 0; i < SoundEffect.Length; i++){
			if(SoundEffect[i].SoundCodeID != string.Empty && SoundEffect[i].SoundCodeID == SoundCodeID){
				_id_i = i;
				break;
			}
		}
		if(_id_i == -1){return;}
		mySFX_AudioSource[mySFX_AudioSource_NUMBER].volume = Random.Range(SoundEffect[_id_i].volumeMin,SoundEffect[_id_i].volumeMax);
		mySFX_AudioSource[mySFX_AudioSource_NUMBER].volume *= _distance_f;
		mySFX_AudioSource[mySFX_AudioSource_NUMBER].pitch = Random.Range(SoundEffect[_id_i].pitchMin,SoundEffect[_id_i].pitchMax);
		mySFX_AudioSource[mySFX_AudioSource_NUMBER].PlayOneShot(SoundEffect[_id_i].Sound_Clip[Random.Range(0,SoundEffect[_id_i].Sound_Clip.Length)]);
		mySFX_AudioSource_NUMBER++;
		if(mySFX_AudioSource_NUMBER >= mySFX_AudioSource.Length){
			mySFX_AudioSource_NUMBER = 0;
		}
	}
	Transform myCamera;
	float Camera_SFX_Volume(Transform SoundSource){
		if(myCamera != Camera.main.transform){myCamera = Camera.main.transform;}
		float _distance_f = Vector3.Distance(myCamera.position,SoundSource.position);
		if(_distance_f <= 20){
			return 1.0f;
		}
		else if(_distance_f > 20 && _distance_f <= 30){
			return 0.6f;
		}
		else if(_distance_f > 30 && _distance_f <= 35){
			return 0.2f;
		}
		else if(_distance_f > 35 && _distance_f <= 40){
			return 0.1f;
		}
		else if(_distance_f > 40 && _distance_f <= 45){
			return 0.005f;
		}
		return 0.0f;
	}
		
//	public void Play(AudioClip clip){
//		AudioSource source = gameObject.AddComponent<AudioSource>();
//		source.clip = clip;
//		//source.outputAudioMixerGroup = output;
//		source.Play();
//		Destroy(source, clip.length);
//	}
}
