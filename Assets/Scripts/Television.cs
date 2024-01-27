using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Television : MonoBehaviour, IModifyInsanity
{
    private float _currentInsanityModifier = 0f;

    [SerializeField] private List<VideoClip> _goodChannels = new List<VideoClip>();
    [SerializeField] private List<VideoClip> _badChannels = new List<VideoClip>();


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
