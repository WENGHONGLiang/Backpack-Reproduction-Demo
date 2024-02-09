using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    public SceneFader fader;
    public Scenes FromScene;
    public Scenes ToScene;
    public void ChangeScene()
    {
        fader.FadeTo(FromScene, ToScene);
    }
}
