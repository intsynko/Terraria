using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Это интерфейс врага
/// </summary>
public interface IEntity 
{
    /// <summary>
    /// Нанести урон
    /// </summary>
    void Damage(float damage);
}

