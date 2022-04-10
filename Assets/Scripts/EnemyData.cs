using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Standart Enemy", fileName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    [Tooltip("Спрайт врага")]
    [SerializeField] private Sprite mainSprite;
    public Sprite MainSprite
    {
        get { return mainSprite; }
        protected set {}
    }

    [Tooltip("Скорость врага")]
    [SerializeField] private float speed = 400;
    public float Speed
    {
        get { return speed; }
        protected set {}
    }

    [Tooltip("Время подготовки к атаке")]
    [SerializeField] private float atkPrep = 0.3f;
    public float AtkPrep
    {
        get { return atkPrep; }
        protected set {}
    }

    [Tooltip("Время атаки")]
    [SerializeField] private float atkHit = 0.5f;
    public float AtkHit
    {
        get { return atkHit; }
        protected set {}
    }

    [Tooltip("Кд атаки")]
    [SerializeField] private float atkCd = 2f;
    public float AtkCd
    {
        get { return atkCd; }
        protected set {}
    }

}
