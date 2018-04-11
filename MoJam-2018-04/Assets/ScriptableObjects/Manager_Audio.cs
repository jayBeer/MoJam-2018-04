using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Manager_Audio : ScriptableObject  {
	[System.Serializable]
	public class _Music{
		public string MusicCodeID;
		public AudioClip Mucic_Clip;
		public float volume;
	}
	public _Music[] Music;

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
}
