using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnCollision2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Fox")
        {
            Debug.Log("Fox coll");
            SceneManager.LoadScene("3 - City 1");
        }
    }

    
}
