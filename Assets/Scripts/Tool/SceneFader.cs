using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image image;
    public float speed = 0.5f;
    public AnimationCurve curve;

    private void Start()
    {
        image.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

        // Fixme: 换一个地方初始化
        PlayerState.instance.InitPlayerState();
        PlayerState.instance.InitPlayerAttribute();
    }

    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    public void FadeTo(Scenes from, Scenes to)
    {
        StartCoroutine(FadeOut(from, to));
    }

    IEnumerator FadeOut(Scenes from, Scenes to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float a = curve.Evaluate(t);
            image.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.UnloadSceneAsync((int)from);
        SceneManager.LoadSceneAsync((int)to, LoadSceneMode.Additive);
    }

}
