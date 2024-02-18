using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Fox")
        {
            Debug.Log("Fox coll");
            SceneManager.LoadScene("2 - Park");
        }
    }

    
}
