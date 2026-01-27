using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Animator anim;
    string[] Scenes = {"Menu","Tutorial","Easy","FINISH"};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(int id)
    {
        print("Tried to loadScene");
        StartCoroutine(LoadSceneWithEffect(id));
    }

    IEnumerator LoadSceneWithEffect(int id)
    {
        anim.SetTrigger("NewScene");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(id);
    }
}
