using UnityEngine;
using TMPro;
using System.Collections;

public class StonkScroller : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Vector2 speed = Vector2.one;
    [SerializeField] private string texture_name;
    private Material material;
    private Vector2 current;

    [SerializeField] private TMP_Text TMPtext;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = renderer.material;
        current = material.GetTextureOffset(texture_name);
    }

    private void OnEnable()
    {
        StartCoroutine(EverySecondRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {

        current += speed * Time.deltaTime;
        material.SetTextureOffset(texture_name, current);
    }

    IEnumerator EverySecondRoutine()
    {
        while (true)
        {
            float value = Random.Range(0.0000f, 0.00001f);
            TMPtext.text = value.ToString("0.0000000000");
            yield return new WaitForSeconds(1);
        }
    }
}
