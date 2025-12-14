using UnityEngine;

public class SpeedUpMalus : MonoBehaviour
{

    public float speedMultiplier = 1.2f;

    public GameObject gameManager;

    public void ApplySpeedUp()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in gameObjects)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.speed *= speedMultiplier;
            }
        }

        gameManager.GetComponent<GameManager>().zombieSpeed *= speedMultiplier;
        gameManager.GetComponent<GameManager>().sceletonSpeed *= speedMultiplier;
        gameManager.GetComponent<GameManager>().seekerSpeed *= speedMultiplier;
    }
}
