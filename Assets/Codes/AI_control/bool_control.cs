using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bool_control : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Ѫ������")]
    [SerializeField] [Tooltip("���Ѫ��")] private float maxbool = 100f;
    [SerializeField] [Tooltip("��ǰѪ��")] private float thisbool;
    [SerializeField] [Tooltip("Ѫ�����")] private RectTransform component;
    [SerializeField] [Tooltip("�������")] private float val;
    private float b;

    void Start()
    {
        thisbool = maxbool;
        val = component.sizeDelta[1];
        // Update is called once per frame
    }
    void Update()
    {
        b = thisbool / maxbool;
        Vector3 v = component.localScale;
        v[0] = 3.0f * b;
        component.localScale = v;
        Vector2 v2 = component.anchoredPosition;
        v2[0] = 0.001f + 1.5f * (1f - b);
        component.anchoredPosition = v2;
        //thisbool -= 10f * Time.deltaTime;
        if (thisbool < 0)
        {
            gameObject.GetComponent<AI>().enabled = false;
            Destroy(gameObject);
        }
    }
    public void hit(float hitbool)
    {
        thisbool -= hitbool;
    }
}