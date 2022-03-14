using UnityEngine;

public abstract class State: ScriptableObject
{
    protected GameKnife _gameKnife;

    public virtual void Init()
    {

    }

    public void SetGameKnife(GameKnife gameKnife)
    {
        _gameKnife = gameKnife;
    }

    public abstract void Update();
}
