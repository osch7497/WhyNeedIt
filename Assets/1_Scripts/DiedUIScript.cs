using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DiedUIScript : MonoBehaviour
{
    [SerializeField] private Image RedScreen;
    void OnEnable(){
        RedScreen.color = new Color(1,0,0,1);
        IEnumerator co = FadeOutRedScreen();
        StartCoroutine(co);
    }
    IEnumerator FadeOutRedScreen()
    {
        for(int i = 0; i <= 100; i++){
            RedScreen.color = new Color(1,0,0,1-(i*0.01f));
            yield return new WaitForSeconds(0.02f);
        }
        Time.timeScale = 0f;
    }
    void Restart(){
        SceneManager.LoadScene("Game");
    }
}
