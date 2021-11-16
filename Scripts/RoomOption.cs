using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class RoomOption : MonoBehaviour
    {
        public GameObject BG;
        public GameObject Main;
        TweenPosition TweenPos;
        bool Opened = false;
        //Setting
        public GameObject MainCaiDat;
        public UISlider Music, Sound;
        public GameObject MainHuongDan;
        public GameObject MainLuatChoi;
        public GameObject MainLichSu;
        private void Start()
        {
            Music.value = UserConfig.GAMEMUSIC;
            Sound.value = UserConfig.GAMESOUND;
        }
        public void OnChangeGameMusic()
        {
            RoomSelect.instance.Music.volume = Music.value;
        }
        public void OnChangeGameSound()
        {
            AudioListener.volume = Sound.value;
        }
        //
        public IEnumerator OnTween()
        {
            if (!Opened)
            {
                if (!Main.GetComponent<TweenPosition>())
                {
                    Main.AddComponent<TweenPosition>();
                }
                BG.SetActive(true);
                TweenPos = Main.GetComponent<TweenPosition>();
                TweenPos.from = Vector3.zero;
                TweenPos.to = new Vector3(0, -275, 0);
                TweenPos.duration = RoomSelect.timeTweenPanel;
                Main.transform.localPosition = Vector3.zero;
                TweenPos.PlayForward();
                yield return new WaitForSeconds(RoomSelect.timeTweenPanel);
                Opened = true;
            }
            else
            {
                BG.SetActive(false);
                TweenPos.PlayReverse();
                yield return new WaitForSeconds(RoomSelect.timeTweenPanel);
                Opened = false;
            }
        }

        void OnLichSu()
        {
            StartCoroutine(OnTween());
            StartCoroutine(UltilityGame.instance.TweenPanel(true, MainLichSu));
        }
        void OnCloseLichSu() {
            StartCoroutine(UltilityGame.instance.TweenPanel(false, MainLichSu));
        }
        void OnHuongDan()
        {
            StartCoroutine(OnTween());
            StartCoroutine(UltilityGame.instance.TweenPanel(true, MainHuongDan));
        }
        void OnCloseHuongDan()
        {
            StartCoroutine(UltilityGame.instance.TweenPanel(false, MainHuongDan));
        }
        void OnLuatChoi()
        {
            StartCoroutine(OnTween());
            StartCoroutine(UltilityGame.instance.TweenPanel(true, MainLuatChoi));
        }
        void OnCloseLuatChoi()
        {
            StartCoroutine(UltilityGame.instance.TweenPanel(false, MainLuatChoi));
        }
        void OnCaiDat()
        {
            StartCoroutine(OnTween());
            StartCoroutine(UltilityGame.instance.TweenPanel(true,MainCaiDat));
        }
        void OnCloseCaiDat()
        {
            PlayerPrefs.SetFloat("CoUpMusic",Music.value);
            PlayerPrefs.SetFloat("CoUpSound",Sound.value);
            StartCoroutine(UltilityGame.instance.TweenPanel(false, MainCaiDat));
        }
    }

