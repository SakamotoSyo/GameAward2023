using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//改善点
public class StateMachine<TOwner>
{
    public abstract class State 
    {
        /// <summary>ステートを管理しているステートマシン</summary>
        protected StateMachine<TOwner> StateMachine => stateMachine;
        internal StateMachine<TOwner> stateMachine;
        /// <summary>遷移の一覧</summary>
        internal Dictionary<int, State> transitions = new Dictionary<int, State>();

        /// <summary>このステートのオーナー</summary>
        protected TOwner Owner => stateMachine.Owner;

        internal void Enter(State currentState) 
        {
            OnEnter(currentState);
        }
        protected virtual void OnEnter(State currentState) { }

        internal void Update() 
        {
            OnUpdate();
        }

        protected virtual void OnUpdate() { }

        internal void Exit(State nextState) 
        {
            OnExit(nextState);
        }
        protected virtual void OnExit(State nextState){ }
    }

    /// <summary>現在のステート</summary>
    public State CurrentState { get; private set; }

    //ステートリスト
    private LinkedList<State> states = new LinkedList<State>();

    /// <summary>適当なStateに遷移するためのからのクラス</summary>
    public sealed class AnyState : State { }

    /// <summary>このステートマシンのオーナー</summary>
    public TOwner Owner { get; }

    public StateMachine(TOwner owner) 
    {
        Owner = owner;
    }

    /// <summary>
    /// ステートを追加する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Add<T>() where T : State, new() 
    {
        var state = new T();
        state.stateMachine = this;
        states.AddLast(state);
        return state;
    }

    /// <summary>
    /// 指定したStateを取得、なければ生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetOrAddState<T>() where T : State, new() 
    {
        foreach (var state in states) 
        {
            if (state is T result) 
            {
                return result;
            }
        }

        return Add<T>();
    }

    /// <summary>
    /// 遷移を定義するためのメソッド
    /// </summary>
    /// <typeparam name="Tdo">遷移元</typeparam>
    /// <typeparam name="Tto">遷移先</typeparam>
    /// <param name="eventID">イベントID</param>
    public void AddTransition<TFrom, Tto>(int eventId)
        where TFrom : State, new()
        where Tto : State, new()
    {
        var from = GetOrAddState<TFrom>();
        if (from.transitions.ContainsKey(eventId)) 
        {
            throw new System.ArgumentException($"ステート{nameof(TFrom)}に対してイベントID{eventId}の遷移は定義済みです");
        }

        var to = GetOrAddState<Tto>();
        from.transitions.Add(eventId, to);
    }

    /// <summary>
    /// どのステートからでも特定のステートに遷移できるイベントを追加する
    /// </summary>
    public void AddAnyTranstion<Tto>(int eventId) where Tto : State, new() 
    {
        AddTransition<AnyState, Tto>(eventId);
    }

    /// <summary>
    /// ステートマシンの実行を開始する（ジェネリック版）
    /// </summary>
    public void Start<TFirst>() where TFirst : State, new()
    {
        Start(GetOrAddState<TFirst>());
    }

    public void Start(State firstState) 
    {
       CurrentState = firstState;
        CurrentState.Enter(null);
    }

    public void Update() 
    {
        CurrentState.Update();
    }

    /// <summary>
    /// 遷移イベントを発行する
    /// </summary>
    /// <param name="eventId">イベントID</param>
    public void Dispatch(int eventId) 
    {
        State to;

        if (!CurrentState.transitions.TryGetValue(eventId, out to)) 
        {
            if (!GetOrAddState<AnyState>().transitions.TryGetValue(eventId, out to)) 
            {
                return;
            }
        }

        Change(to);
    }

    /// <summary>
    /// ステートを変更する
    /// </summary>
    /// <param name="nextState">遷移先のステート</param>
    public void Change(State nextState) 
    {
        CurrentState.Exit(nextState);
        nextState.Enter(null);
        CurrentState = nextState;
    }
}

