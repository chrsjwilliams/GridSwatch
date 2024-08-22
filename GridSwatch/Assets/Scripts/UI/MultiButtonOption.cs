using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultiButtonOption : Option
{
    [System.Serializable]
    public struct MultiOption
    {
        public bool isOn;

        public MonoTweener onTweens;
        public MonoTweener offTweens;

        public Action onAction;
        public Action offAction;
    }

    // i don't think this will work because how will I pass in
    // the action to multiple buttons?

    // could the buttons themselves just send a signal that they've been pressed?
    // probaly create dyanight mode buttons and have color manager listen for those presses

    [SerializeField] private List<MultiOption> _options;
    [SerializeField] private MultiOption _selectedOption;
    public MultiOption SelectedOption { get { return _selectedOption; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
