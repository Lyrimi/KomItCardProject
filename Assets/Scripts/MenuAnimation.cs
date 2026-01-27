using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] float WaitBeforeAnimation;
    [SerializeField] float WaitAfterAnimation;
    Animator animator;

    SceneLoader sceneLoader;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Main());
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator Main()
    {
        yield return new WaitForSeconds(WaitBeforeAnimation);
        animator.SetTrigger("Start");
        yield return new WaitForNextFrameUnit();
        float waitTime = animator.GetCurrentAnimatorStateInfo(0).length;
        print(waitTime);
        yield return new WaitForSeconds(waitTime + WaitAfterAnimation);
        print("ReadyForNextAction");
        sceneLoader.LoadScene(2);
    }
}
