using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            if (!_Instance)
                _Instance = FindObjectOfType<AudioManager>();
            return _Instance;
        }
    }

    [SerializeField] AudioMixer Mixer;
    [SerializeField] int MaxSounds = 30;


    readonly Dictionary<string, TrackInfo> Tracks = new Dictionary<string, TrackInfo>();
    readonly List<AudioPoolItem> AudioPool = new List<AudioPoolItem>();
    readonly Dictionary<ulong, AudioPoolItem> ActivePool = new Dictionary<ulong, AudioPoolItem>();
    ulong IdGiver;
    Transform ListenerPos;
    
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (!Mixer) return;
        ResetPitch();
        AudioMixerGroup[] groups = Mixer.FindMatchingGroups(string.Empty);

        foreach (AudioMixerGroup g in groups)
        {
            TrackInfo trackInfo = new TrackInfo
            {
                name = g.name,
                group = g,
                trackFader = null
            };
            Tracks[g.name] = trackInfo;
        }

        for (int i = 0; i < MaxSounds; i++)
        {
            GameObject _newAudioObj = new GameObject("audio Pool Item");
            AudioSource _audioSource = _newAudioObj.AddComponent<AudioSource>();
            _newAudioObj.transform.parent = transform;

            AudioPoolItem _newPoolItem = new AudioPoolItem()
            {
                poolGameObj = _newAudioObj,
                poolAudioSource = _audioSource,
                poolTransform = _newAudioObj.transform,
                playing = false,
            };
            
            _newAudioObj.SetActive(false);
            AudioPool.Add(_newPoolItem);

        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public IEnumerator ChangePitch(float _value, float _duration)
    {
        Mixer.SetFloat("MasterPitch", _value);
        yield return new WaitForSeconds(_duration);
        ResetPitch();
    }

    public void ResetPitch()
    {
        Mixer.SetFloat("MasterPitch", 1);
    }

    void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        AudioListener _audioListener = FindObjectOfType<AudioListener>();
        if (_audioListener)
            ListenerPos = _audioListener.transform;
    }

    ulong ConfigurePoolObject(int _poolIndex, string _track, AudioClip _clip, Vector3 _position, float _vol,
        float _spatialBlend,float _pitch, float _unimportance)
    {
        if (_poolIndex < 0 || _poolIndex >= AudioPool.Count) return 0;

        AudioPoolItem _poolItem = AudioPool[_poolIndex];
        IdGiver++;

        AudioSource _source = _poolItem.poolAudioSource;
        _source.clip = _clip;
        _source.volume = _vol;
        _source.spatialBlend = _spatialBlend;
        _source.pitch = _pitch;

        _source.outputAudioMixerGroup = Tracks[_track].group;
        _source.transform.position = _position;
        _poolItem.playing = true;
        _poolItem.unimportance = _unimportance;
        _poolItem.ID = IdGiver;
        _poolItem.poolGameObj.SetActive(true);
        _source.Play();
        _poolItem.courotine = StopSoundDelayed(IdGiver, _source.clip.length);
        StartCoroutine(_poolItem.courotine);

        ActivePool[IdGiver] = _poolItem;
        return IdGiver;
    }

    IEnumerator StopSoundDelayed(ulong _id, float _delay)
    {
        yield return new WaitForSeconds(_delay);

        if (ActivePool.TryGetValue(_id, out var _activeSound))
        {
            _activeSound.poolAudioSource.Stop();
            _activeSound.poolAudioSource.clip = null;
            _activeSound.poolGameObj.SetActive(false);
            ActivePool.Remove(_id);

            _activeSound.playing = false;
        }
    }

    public ulong PlayOneShotSound(string _track, AudioClip _clip, Vector3 _position, float _vol, float _spatialBlend,
       float _pitch, int _priority = 128)
    {
        if (!Tracks.ContainsKey(_track) || _clip == null || _vol.Equals(0f)) return 0;

        float _unimportance = (ListenerPos.position - _position).sqrMagnitude / Mathf.Max(1, _priority);

        int _leastImportantIndex = -1;
        float _leastImportantValue = float.MaxValue;

        for (int i = 0; i < AudioPool.Count; i++)
        {
            AudioPoolItem _poolItem = AudioPool[i];

            if (!_poolItem.playing)
                return ConfigurePoolObject(i, _track, _clip, _position, _vol, _spatialBlend, _pitch,_unimportance);
            else if (_poolItem.unimportance > _leastImportantValue)
            {
                _leastImportantValue = _poolItem.unimportance;
                _leastImportantIndex = i;
            }
        }

        if (_leastImportantValue > _unimportance)
            return ConfigurePoolObject(_leastImportantIndex, _track, _clip, _position, _vol, _spatialBlend,
                _pitch,_unimportance);
        return 0;
    }

    public IEnumerator PlaySoundDelayed(float _delay, string _track, AudioClip _clip, Vector3 _position, float _vol,
        float _spatialBlend,
        int _priority = 128)
    {
        yield return new WaitForSeconds(_delay);
        PlayOneShotSound(_track, _clip, _position, _vol, _spatialBlend, _priority);
    }
}



public class TrackInfo
{
    public string name = string.Empty;
    public AudioMixerGroup group;
    public IEnumerator trackFader;
}

public class AudioPoolItem
{
    public GameObject poolGameObj;
    public Transform poolTransform;
    public AudioSource poolAudioSource;
    public bool playing;
    public IEnumerator courotine;
    public ulong ID;
    public float unimportance;
}