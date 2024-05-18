using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnOffButton : MonoBehaviour
{
    public bool IsOn = true;
    [SerializeField] Sprite onImage;
    [SerializeField] Sprite offImage;
    public UnityEvent OnSetOn;
    public UnityEvent OnSetOff;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.image.sprite = IsOn? onImage : offImage;
        button.onClick.AddListener(ChangeState);
    }

    private void ChangeState()
    {
        IsOn = !IsOn;
        if(IsOn)
        {
            button.image.sprite = onImage;
            OnSetOn?.Invoke();
        }
        else
        {
            button.image.sprite = offImage;
            OnSetOff?.Invoke();
        }
    }
}
