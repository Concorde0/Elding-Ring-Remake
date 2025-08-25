using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    List<IEndGameObserve> endGameObservers = new List<IEndGameObserve>();

    public CharacterStats playerStats; 
    //依赖注入后可以通过GameManager访问playerStats
    //可以避免playerStats变为单例
    public void RegisterPlayer(CharacterStats playerStats)
    {
        this.playerStats = playerStats;
    }
    
    public void AddObserver(IEndGameObserve observer)
    {
        //Add方法可以将元素加入List尾部，这里写了一个Add方法，需要传入一个与List类型相对应的参数，调用时让此参数加到队列末端
        endGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserve observer)
    {
        endGameObservers.Remove(observer);
    }
    
    public void NotifyObservers()
    {
        foreach (var observer in endGameObservers)
        {
            //接口的常见用法，不关心如何实现，想要调用接口中的方法，只需要在对应的地方调用NotifyObservers()，就循环列表，通知实现接口的代码，实现函数方法
            observer.EndNotify();
        }
    }
}
 