using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SoundEntry
    {
        public string id;
        public AudioClip clip;
    }

    [Header("Add all your SFX here")]
    public SoundEntry[] sounds;

    public float cooldown = 0.05f;

    Dictionary<string, AudioClip> soundDict;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        soundDict = new Dictionary<string, AudioClip>();

        foreach (var s in sounds)
        {
            if (!soundDict.ContainsKey(s.id))
                soundDict.Add(s.id, s.clip);
        }
    }

    public void Play(string id, float volume = 1f, float pitch = 1f, float pitchVariance = 0.2f, bool ignoreCooldown = false)
    {

        // cooldown check
        if (cooldown > 0 && !ignoreCooldown)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        if (!soundDict.TryGetValue(id, out var clip)) return;

        var go = new GameObject("SFX_" + id);
        var src = go.AddComponent<AudioSource>();

        src.clip = clip;
        src.volume = volume;
        src.pitch = pitch + Random.Range(-pitchVariance, pitchVariance);
        src.spatialBlend = 0f; 

        cooldown = 0.05f;

        src.Play();
        Destroy(go, clip.length);
    }
}