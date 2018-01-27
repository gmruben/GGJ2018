using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;
	void Awake() {instance = this;}
	public AudioSource [] sfxsource_pool;
	public AudioSource music;
	public float VolumeMain = 1.0F;
	public AudioClipContainer [] sfx_clips;
	public AudioClipContainer [] music_clips;
	

	public static void PlaySFX(string name, Transform t = null)
	{
		AudioClipContainer target = null;
		foreach(AudioClipContainer c in instance.sfx_clips)
		{
			if(c.name == name)
			{
				target = c;
				break;
			}
		}
		if(target != null) 
		{
			AudioSource target_source = GetNextSFXSource();
			if(t != null) target_source.transform.position = t.position;
			instance.StartCoroutine(PlayOnAudioSource(target, target_source));
		}
	}

	static IEnumerator PlayOnAudioSource(AudioClipContainer a, AudioSource source)
	{
		Debug.Log("queued playing audio clip " + a.name + " on source " + source);
		//while(source.isPlaying && a.wait_for_playtime) 
		//{
		//	yield return null;
		//}

		source.PlayOneShot(a.clip, a.volume * instance.VolumeMain);

		while(source.isPlaying) yield return null;
		source.transform.position = instance.transform.position;
		Debug.Log("Finished playing audio clip " + a.name + " on source " + source);
	}

	public static AudioSource GetNextSFXSource()
	{
		AudioSource target = null;
		for(int i = 0; i < instance.sfxsource_pool.Length; i++)
		{
			if(!instance.sfxsource_pool[i].isPlaying) return instance.sfxsource_pool[i];
			if(target == null)
			{
				target = instance.sfxsource_pool[i];
			}
		}
		if(target == null) return instance.sfxsource_pool[0];
		return target;
	}
}
[System.Serializable]
public class AudioClipContainer
{
	public string name;
	public AudioClip clip;
	public float volume = 1.0F;
	public bool looping;
	public bool wait_for_playtime;
}
