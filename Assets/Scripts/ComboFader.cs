using UnityEngine;
using UnityEngine.UI;

public class ComboFader : MonoBehaviour
{
    public float speed;

    private float counter;
    private Text label;
    
    public void Init (int combo)
    {
        counter = 0.0f;

        label = GetComponent<Text> ();
        label.text = "x" + combo + "COMBO";
    }

    void Update ()
    {
        counter += Time.deltaTime;
        float alpha = 1 - counter;

        if (counter > 0.0f)
        {
            label.color = new Color(label.color.r, label.color.g, label.color.b, alpha);
            transform.position += Vector3.up * Time.deltaTime * speed;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}