using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZone : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync(nextScene);
    }
}
