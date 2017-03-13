using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelect : MonoBehaviour {
    
    private GlobalObject _Global;

    public void ChangeAvatar(Sprite selectedSprite) {
        // Get global object
        _Global = GameObject.Find("_Global").GetComponent<GlobalObject>();
        _Global.playerAvatar = selectedSprite;
    }
}
