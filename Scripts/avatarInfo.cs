using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avatarInfo : MonoBehaviour
{
    public UI2DSprite mainSprite;
    public int index;
    
    public UI2DSprite OnChoosen; 
    // Start is called before the first frame update
    void Start()
    {
        mainSprite.sprite2D = LoadPrefab.instance.SpriteAvatars[index];
    }
    void OnChooseAvatar()
    {
        RoomSelect.AvatarSelec = index;
        for (int i = 0; i < RoomSelect.instance.listAvatar.Length;i++)
        {
            RoomSelect.instance.listAvatar[i].OnChoosen.gameObject.SetActive(false);
        }
        this.OnChoosen.gameObject.SetActive(true);
    }
}
