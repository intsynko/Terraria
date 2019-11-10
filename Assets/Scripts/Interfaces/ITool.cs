using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Интрефейс инструмент
/// </summary>
public interface ITool : IObject
{
    /// <summary>
    /// Нанести урон, возвращает урон, который может быть нанесен сущности
    /// </summary>
    /// <param name="entity">Сущность, по которой наносят урон</param>
    /// <returns></returns>
    float GetDamage(IEntity entity);
    /// <summary>
    /// Копать, возращает кол-во секунд, которое затратит на копание
    /// </summary>
    /// <param name="obj">Объект, который пытаемся копать</param>
    /// <returns></returns>
    float GetDig(IObject obj);

}
