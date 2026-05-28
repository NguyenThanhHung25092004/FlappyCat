using System;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    public Sound[] sounds;

    [Header("BGM Playlist Settings")]
    [SerializeField] private string[] bgmPlaylist;
    private string currentBGMName;

    private Coroutine bgmCoroutine;

    protected override void Awake()
    {
        base.Awake();
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        PlayRandomBGM();
    }

    public void PlayRandomBGM()
    {
        if (bgmPlaylist == null || bgmPlaylist.Length == 0)
        {
            Debug.LogWarning("NO MUSIC ADDED!!");
            return;
        }

        // bỏ đồng hồ đếm ngược cũ nếu có
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
        }

        // Dừng bài cũ
        if (!string.IsNullOrEmpty(currentBGMName))
        {
            StopSound(currentBGMName);
        }

        // Chọn bài mới ngẫu nhiên
        int randomIndex = UnityEngine.Random.Range(0, bgmPlaylist.Length);
        string selectedBGM = bgmPlaylist[randomIndex];

        // Tìm thông tin bài hát để lấy độ dài (length)
        Sound s = Array.Find(sounds, sound => sound.name == selectedBGM);
        if (s == null) return;

        s.source.Play();
        currentBGMName = selectedBGM;

        float waitTime = Mathf.Max(0f, s.clip.length - 0.05f);
        bgmCoroutine = StartCoroutine(WaitAndPlayNextBGM(waitTime));

        Debug.Log($"<color=cyan>SoundManager:</color> CurrentBGM: {selectedBGM}");
    }

    private IEnumerator WaitAndPlayNextBGM(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);

        // Sau khi hết số giây của bài hát, tự động gọi lại hàm chọn bài ngẫu nhiên
        PlayRandomBGM();
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    #region STATIC METHODS FOR EASY ACCESS
    public static void playMusic()
    {
        if (Instance != null)
            Instance.PlayRandomBGM();
    }

    public static void stopMusic()
    {
        if (Instance != null)
        {
            if (Instance.bgmCoroutine != null) Instance.StopCoroutine(Instance.bgmCoroutine);
            if (!string.IsNullOrEmpty(Instance.currentBGMName)) Instance.StopSound(Instance.currentBGMName);
        }
    }
    #endregion
}