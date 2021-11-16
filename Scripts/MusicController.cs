using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    // Start is called before the first frame update
    float Cliplengh = 0;
    int rDomMusic = 0;
    bool isPlayMusic = true;
    void Start()
    {
        StartToRandomMusic();
    }

    void StartToRandomMusic()
    {
        int rDom = UnityEngine.Random.Range(0,LoadPrefab.instance.GameMusic.Length); 
        while(rDomMusic == rDom)
            rDom = UnityEngine.Random.Range(0, LoadPrefab.instance.GameMusic.Length);
        rDom = rDomMusic;
        for (int i = 0; i < LoadPrefab.instance.GameMusic.Length; i++)
            LoadPrefab.instance.GameMusic[i].SetActive(false);

        LoadPrefab.instance.GameMusic[rDom].SetActive(true);
        Cliplengh = LoadPrefab.instance.GameMusic[rDom].GetComponent<AudioSource>().clip.length;
        LoadPrefab.instance.GameMusic[rDom].GetComponent<AudioSource>().volume = UserConfig.GAMEMUSIC;
        isPlayMusic = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(Cliplengh > 0)
        {
            Cliplengh -= Time.deltaTime;
        }
        if(Cliplengh <= 0 && isPlayMusic == true)
        {
            isPlayMusic = false;
            StartToRandomMusic();
        }
    }
}
