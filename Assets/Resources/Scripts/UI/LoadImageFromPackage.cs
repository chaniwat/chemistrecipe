using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImageFromPackage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Get a reference to the storage service, using the default Firebase App
        Firebase.Storage.FirebaseStorage storage = Firebase.Storage.FirebaseStorage.DefaultInstance;

        // Create a storage reference from our storage service
        Firebase.Storage.StorageReference images_ref = storage.GetReferenceFromUrl("gs://chemresipe.appspot.com/Medal.unitypackage");

        // Create local filesystem URL
        string local_url = "file:///local/images/island.jpg";

        // Download to the local filesystem
        images_ref.GetFileAsync(local_url).ContinueWith(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                Debug.Log("File downloaded.");
            }
        });
    }
	
}
