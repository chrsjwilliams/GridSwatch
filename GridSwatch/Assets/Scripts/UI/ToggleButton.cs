using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{

    [SerializeField] private MonoTweener _ActivateTweener;

    [SerializeField] private MonoTweener _DeactivateTweener;

    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;
    
    private bool m_IsActive;
    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_IsActive = false;
        m_Button.onClick.AddListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        if (m_IsActive)
        {
            _DeactivateTweener?.Play();
            OnDeactivate?.Invoke();
        }
        else
        {
            _ActivateTweener?.Play();
            OnActivate?.Invoke();
        }

        m_IsActive = !m_IsActive;
    }
}
