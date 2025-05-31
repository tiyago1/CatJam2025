using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Pathfinding;

public class RandomPathAI : MonoBehaviour,IDisposable
{
    public int searchLength = 1000;
    public int spread = 1000;
    private IAstarAI ai;
    private bool isActive;

    private CancellationTokenSource cancellationTokenSource;
    
    public void Cancel()
    {
        isActive = false;
        cancellationTokenSource.Cancel();
        ai.canMove = false;
    }

    public Vector3 Activate()
    {
        isActive = true;
        ai = GetComponent<IAstarAI>();
        ai.canMove = true;
        cancellationTokenSource = new CancellationTokenSource();
        
        return MoveRandomPath();
    }

    private async UniTaskVoid RunAI()
    {
        while (isActive)
        {
            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
            {
                ai.canSearch = false;
                RandomPath path = RandomPath.Construct(transform.position, searchLength);
                path.spread = spread;
                ai.SetPath(path);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationTokenSource.Token);
        }
    }

    public Vector3 MoveRandomPath()
    {
        if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath))
        {
            ai.canSearch = false;
            RandomPath path = RandomPath.Construct(transform.position, searchLength);
            path.spread = spread;
            ai.SetPath(path);
            return path.endPoint;
        }

        return Vector3.zero;
    }
    
    public void Dispose()
    {
        Cancel();
        cancellationTokenSource.Cancel();
    }
}