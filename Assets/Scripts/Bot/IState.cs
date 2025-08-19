using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> 
{
    void OnEnter(T t);
    void OnExecute(T t);
    void OnExit(T t);
}
public interface IDamage
{
    void Damage(Character owner, Vector3 position);
}