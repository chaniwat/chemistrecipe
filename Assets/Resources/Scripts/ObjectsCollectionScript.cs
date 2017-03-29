using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsCollectionScript : MonoBehaviour {

	void Start () {
        GlobalObject globalObject = GameObject.Find("_Global").GetComponent<GlobalObject>();
        Text showMsgText = GameObject.Find("ShowMsgText").GetComponent<Text>();

        Debug.Log(globalObject.message);
        showMsgText.text = globalObject.message;
	}

}
