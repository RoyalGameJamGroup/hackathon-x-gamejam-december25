using UnityEngine;

public class TutorialBenis : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        PauseManager.Instance.SetGameRunningState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame(){
        PauseManager.Instance.SetGameRunningState(true);

        Destroy(this.gameObject);

    }
}
