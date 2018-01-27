using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayHUD : MonoBehaviour
{
	public static OverlayHUD instance;
    public AnimationCurve overlay_fadein, overlay_fadeout;

    public string overlay_test;

    public Overlay[] overlays;

    private Canvas _canvas;

	void Awake(){instance = this;}
    // Use this for initialization
    void Start()
    {
        _canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) LoadOverlay(overlay_test);
    }


    public void LoadOverlay(string name)
    {
        for (int i = 0; i < overlays.Length; i++)
        {
            if (name == overlays[i].name)
            {
                StartCoroutine(LoadOverlayRoutine(overlays[i]));
                return;
            }
        }
        Debug.Log("Could not load overlay for " + name);
    }

    IEnumerator LoadOverlayRoutine(Overlay o)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(o.name));
        obj.transform.SetParent(_canvas.transform, true);
        (obj.transform as RectTransform).sizeDelta = Vector3.zero;

        CanvasGroup group = obj.GetComponent<CanvasGroup>();

        for (float i = 0; i < 0.45F; i += Time.deltaTime)
        {
            group.alpha = overlay_fadein.Evaluate(i / 0.45F);
            yield return null;
        }
        yield return new WaitForSeconds(o.lifetime);
        for (float i = 0; i < 0.45F; i += Time.deltaTime)
        {
            group.alpha = overlay_fadeout.Evaluate(i / 0.45F);
            yield return null;
        }

        Destroy(obj);

    }
}
