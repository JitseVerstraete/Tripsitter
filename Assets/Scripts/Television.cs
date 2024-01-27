using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : MonoBehaviour, IModifyInsanity
{
    private float _currentInsanityModifier = 0f;

    void Start()
    {
        TripFriend.Instance.RegisterInsanityModifier(this);
    }

    private void OnDestroy()
    {
        TripFriend.Instance.UnregisterInsanityModifier(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            _currentInsanityModifier += Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.O))
        {
            _currentInsanityModifier -= Time.deltaTime;
        }
    }

    public float GetInsanityModifier()
    {
        return _currentInsanityModifier;
    }
}
