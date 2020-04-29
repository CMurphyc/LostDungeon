using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioName
{
    Gunshot1,
    Gunshot2,
    PickItem,
    BuildFort,
    SommonRobot,
    Buff1,
    Buff2,
    SwitchClip,
    Sword,
    TapClick,
    Teleport,
    Beast,
    HumanHurt1,
    HumanHurt2,
    HumanDie1,
    HumanDie2,
    BombExplode,
    WizardIce,
    WizardFire,
    BossRabbitShot,
    EngineerShot,
    MonsterBeAttacked,
    ItemUpgrade,
    MainSceneBGM,
    BattleBGM,
    TeamBGM
}


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0f, 1f)]
    public float pitch = 1f;

    private AudioSource theAS;

    public void SetUpSound(AudioSource _theAS)
    {
        theAS = _theAS;
        theAS.clip = clip;
    }

    public void Play()
    {
        theAS.pitch = pitch;
        theAS.volume = volume;
        theAS.Play();
    }
    public bool isPlaying()
    {
        return theAS.isPlaying;

    }
    public void Mute()
    {
        theAS.Stop();
    }

    public void Loop(bool isLoop)
    {
        theAS.loop = isLoop;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private List<Sound> sounds;
    private Dictionary<AudioName, string> AudioNameToAudioClip;


    GameObject main;
    private void Awake()
    {
        main = GameObject.FindWithTag("GameEntry");
        if (instance == null) instance = this;
        else if (instance != this) Object.Destroy(gameObject);

        DictionaryInit();
        SoundsInit();

    }

    private void SoundsInit()
    {
        sounds = new List<Sound>();

        foreach (var it in AudioNameToAudioClip)
        {
            string prefix = "Audio/";
            prefix += it.Value;
            AudioClip ac = Resources.Load(prefix) as AudioClip;
            Sound sd = new Sound();
            sd.clip = ac;
            sd.name = it.Value;
            sounds.Add(sd);
        }

        foreach (var sound in sounds)
        {
            //Debug.Log("add sound");
            GameObject temp = new GameObject();
            temp.AddComponent<AudioSource>();
            temp.transform.parent = main.transform;
            sound.SetUpSound(temp.GetComponent<AudioSource>());
        }
    }
    private void DictionaryInit()
    {
        AudioNameToAudioClip = new Dictionary<AudioName, string>();

        AudioNameToAudioClip.Add(AudioName.Gunshot1, "fx_gun_1");
        AudioNameToAudioClip.Add(AudioName.Gunshot2, "fx_shoot_e2");
        AudioNameToAudioClip.Add(AudioName.PickItem, "fx_item");
        AudioNameToAudioClip.Add(AudioName.BuildFort, "fx_skill_c4");
        AudioNameToAudioClip.Add(AudioName.SommonRobot, "fx_skill_c6");
        AudioNameToAudioClip.Add(AudioName.Buff1, "fx_skill_ploy");
        AudioNameToAudioClip.Add(AudioName.Buff2, "fx_vine_show_s5");
        AudioNameToAudioClip.Add(AudioName.SwitchClip, "fx_switch #180557");
        AudioNameToAudioClip.Add(AudioName.Sword, "fx_sword1");
        AudioNameToAudioClip.Add(AudioName.TapClick, "fx_tapClick");
        AudioNameToAudioClip.Add(AudioName.Teleport, "fx_transform");
        AudioNameToAudioClip.Add(AudioName.Beast, "fx_wolf_atk");
        AudioNameToAudioClip.Add(AudioName.HumanHurt1, "human1 hurt2");
        AudioNameToAudioClip.Add(AudioName.HumanHurt2, "human2 hurt2");
        AudioNameToAudioClip.Add(AudioName.HumanDie1, "human1 die");
        AudioNameToAudioClip.Add(AudioName.HumanDie2, "human2 die");
        AudioNameToAudioClip.Add(AudioName.BombExplode, "fx_explode_small");
        AudioNameToAudioClip.Add(AudioName.WizardIce, "fx_ice");
        AudioNameToAudioClip.Add(AudioName.WizardFire, "fx_fireworks_play_long");
        AudioNameToAudioClip.Add(AudioName.BossRabbitShot, "fx_gun_4");
        AudioNameToAudioClip.Add(AudioName.EngineerShot, "fx_gun_cannon");
        AudioNameToAudioClip.Add(AudioName.MonsterBeAttacked, "fx_boss13_dead");
        AudioNameToAudioClip.Add(AudioName.ItemUpgrade, "fx_switch #180557");
        AudioNameToAudioClip.Add(AudioName.MainSceneBGM, "bgm_5Low");
        AudioNameToAudioClip.Add(AudioName.BattleBGM, "bgm_7Low");
        AudioNameToAudioClip.Add(AudioName.TeamBGM, "bgm_room");
    }
    public bool isPlaying(AudioName _name)
    {
        bool ret = false;
        foreach (var sound in sounds)
        {
            //Debug.Log(sound.name);
            if (sound.name == AudioNameToAudioClip[_name])
            {
                ret = sound.isPlaying();
            }
        }
        return ret;

    }

    public void PlayAudio(AudioName _name, bool isLoop)
    {
        foreach (var sound in sounds)
        {
            //Debug.Log(sound.name);
            if (sound.name == AudioNameToAudioClip[_name])
            {
                sound.Loop(isLoop);
                sound.Play();
            }
        }
    }
    public void MuteAll()
    {
        for (int i = 0; i < sounds.Count; ++i)
        {
            sounds[i].Mute();
        }
    }
}