using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class OptionMenu : MonoBehaviour
{
    public enum OptionLabel { COLORBLIND, DAY_NIGHT}
    [System.Serializable]
    public struct GameOption
    {
        public OptionLabel label;
        public Option option;
    }

    [SerializeField, ReadOnly] bool _isOpen;
    public bool IsOpen { get { return _isOpen; } }

    [SerializeField] MonoTweener appearTweens;
    [SerializeField] MonoTweener disappeearTweens;

    [SerializeField] List<GameOption> gameOptions;

    //As the option menu I need to:
    //  send out info to the color manager when we are in colorblind mode
    //  send out info to color manager when we are day or night mode

    // Start is called before the first frame update
    void Start()
    {
        _isOpen = false;
        disappeearTweens?.Play();

    }




    public void ToggleOptionMenu()
    {
        _isOpen = !_isOpen;
        if(_isOpen)
        {
            appearTweens?.Play();
        }
        else
        {
            disappeearTweens?.Play();
        }

    }
}
