using System;
using UnityEngine;
[Serializable]
public class HealthSystem
{
    public int MaxHealth
    {
        private set;
        get;
    }

    private int _currentHealth;

    public int CurrentHealth
    {
        set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                OnHealthChange?.Invoke();
            }

        }

        get => _currentHealth;
    }


    public Action OnHealthChange;

    public void Initialize(int MaxHealth, int CurrentHealth)
    {
        this.MaxHealth = MaxHealth;
        this.CurrentHealth = CurrentHealth;
    }

    public void ChangeHealth(int changeNumber)
    {
        CurrentHealth += changeNumber;
    }

}
