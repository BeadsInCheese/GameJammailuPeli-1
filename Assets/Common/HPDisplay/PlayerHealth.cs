using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    TextMeshProUGUI m_Object;
    // Start is called before the first frame update
    void Start()
    {
        m_Object = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.rotation = Quaternion.Euler( Vector3.zero);
        m_Object.text = CharacterControl.instance.stats.GetCurrentHP() + " / " + CharacterControl.instance.stats.GetMaxHP();
    }
}
