using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelect : MonoBehaviour {

    public Sprite avatar1;
    public Sprite avatar2;
    public Sprite avatar3;

    private GlobalObject _Global;

    public void ChangeAvatar(Sprite selectedSprite) {
        // Get global object
        _Global = GameObject.Find("_Global").GetComponent<GlobalObject>();

        _Global.playerAvatar = selectedSprite;
    }
}
