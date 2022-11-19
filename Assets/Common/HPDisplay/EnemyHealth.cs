using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyHealth : MonoBehaviour
{
    public EnemyStats stats;
     TextMeshProUGUI m_Object;
    // Start is called before the first frame update
    void Start()
    {
        m_Object=GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        m_Object.text = stats.stats.GetCurrentHP() + " / " + stats.stats.GetMaxHP();
    }
}
