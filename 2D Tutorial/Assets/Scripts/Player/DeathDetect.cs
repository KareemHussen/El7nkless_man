using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathDetect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        if(gameObject.transform.position.y <= -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
