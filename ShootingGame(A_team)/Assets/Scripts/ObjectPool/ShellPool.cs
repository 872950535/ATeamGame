using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShellPool", menuName = "ObjectPool/ShellPool")]
public class ShellPool : ScriptableObject
{
    public Shell prefab;

    private Queue<Shell> poolInstanceQueue = new Queue<Shell>();
    public Shell GetInstance()
    {
        if (poolInstanceQueue.Count > 0)
        {
            Shell instanceToUse = poolInstanceQueue.Dequeue();
            Debug.Log(poolInstanceQueue.Count);
            instanceToUse.gameObject.SetActive(true);
            return instanceToUse;
        }
        return Instantiate(prefab);
    }
    public void ReturnInstance(Shell gamejectToPool, Transform poolTransform)
    {
        poolInstanceQueue.Enqueue(gamejectToPool);
        gamejectToPool.gameObject.SetActive(false);
        gamejectToPool.transform.SetParent(poolTransform);
    }
}
