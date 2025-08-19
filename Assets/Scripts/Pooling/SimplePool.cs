using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class SimplePool
{
    static Dictionary<PoolType, Pool> Pools = new Dictionary<PoolType, Pool>();

    public static void PreLoad(GameEntity prefab, Transform parent, int amount)
    {
        if (!prefab)
        {
            Debug.LogError("PREFAB IS EMPTY!!");
            return;
        }
        if (Pools.ContainsKey(prefab.poolType))
        {
            if (Pools[prefab.poolType] != null)
                return;
        }

        Pool p = new Pool();
        p.PreLoad(prefab, amount, parent);
        Pools[prefab.poolType] = p;
    }
    public static T PopFromPool<T>(PoolType poolType, Vector3 position, Quaternion rotation) where T : GameEntity
    {
        if (!Pools.ContainsKey(poolType))
        {
            Debug.LogError($"{poolType} IS NOT PRELOAD!!");
            return null;
        }
        return Pools[poolType].Pop(position, rotation) as T;
    }
    public static void PushToPool(GameEntity entity)
    {
        if (!Pools.ContainsKey(entity.poolType))
        {
            Debug.LogError($"{entity.poolType} IS NOT PRELOAD!!");
        }
        Pools[entity.poolType].Push(entity);
    }
    public static void Collect(PoolType poolType)
    {
        if (!Pools.ContainsKey(poolType))
        {
            Debug.LogError($"{poolType} IS NOT PRELOAD!!");
        }
        Pools[poolType].Collect();  
    }
    public static void CollectAll()
    {
        foreach (var pool in Pools.Values)
        {
            pool.Collect();
        }
    }
    public static void Release(PoolType poolType) 
    {
        if (!Pools.ContainsKey(poolType))
        {
            Debug.LogError($"{poolType} IS NOT PRELOAD!!");
        }
        Pools[poolType].Release();
    }
    public static void ReleaseAll()
    {
        foreach (var pool in Pools.Values)
        {
            pool.Release();
        }
    }
}
public class Pool
{
    Transform parent;
    GameEntity prefab;

    //Queue chứa các GameEntity đang ở trong pool
    Queue<GameEntity> inActives = new Queue<GameEntity>();
    //List chứa các GameEntity đang được sử dụng
    List<GameEntity> isActives = new List<GameEntity>();

    //Khởi tạo pool
    public void PreLoad(GameEntity prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < amount; i++)
        {
            Push(Pop(Vector3.zero, Quaternion.identity));
        }
    }
    //Lấy phần tử trong pool
    public GameEntity Pop(Vector3 position, Quaternion rotation)
    {
        GameEntity entity;

        if (inActives.Count <= 0)
            entity = GameObject.Instantiate(prefab, parent);
        else
            entity = inActives.Dequeue();
        entity.TF.SetPositionAndRotation(position, rotation);
        isActives.Add(entity);
        entity.gameObject.SetActive(true);

        return entity;
    }
    //Đẩy phần tử vào pool
    public void Push(GameEntity entity)
    {
        if (entity && entity.gameObject.activeSelf)
        {
            isActives.Remove(entity);
            inActives.Enqueue(entity);
            entity.gameObject.SetActive(false);
        }
    }
    //Đẩy tất cả phần tử vào pool
    public void Collect()
    {
        while (isActives.Count > 0)
        {
            Push(isActives[0]);
        }
    }
    //Giải phóng toàn bộ phần tử trong pool
    public void Release()
    {
        Collect();
        while (inActives.Count > 0)
            GameObject.Destroy(inActives.Dequeue().gameObject);
        inActives.Clear();
    }
}
