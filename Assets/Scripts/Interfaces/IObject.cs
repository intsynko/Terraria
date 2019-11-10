using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Состояние, может быть в виде физического блока или как дропнутый блок
/// </summary>
public enum Condition {Material, Dropped}

/// <summary>
/// Объект (блоки), который можно собрать
/// </summary>
public interface IObject
{
    /// <summary>
    /// ВОзможные состояния
    /// </summary>
    List<Condition> conditions { get; }
    /// <summary>
    /// Текущее состояние
    /// </summary>
    Condition CurrentCondition { get; }

    IEnumerator EnterPlayer(Transform playerTransform);

    void Instanse(int x, int y);
}
