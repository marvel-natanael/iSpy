using System;
using UnityEngine;

public class TurretAnimation : MonoBehaviour
{
    public enum State
    {
        LEFT = 0,
        DOWN = 1,
        RIGHT = 2,
        UP = 3
    }

    private State _state = State.LEFT;
    private Animator _animator;
    private Transform _transform;

    private Turret _turret;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _turret = GetComponent<Turret>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log(_state.ToString());
        Debug.Log(transform.localEulerAngles.z);
        SetState();*/
       // _state = _turret.GetState;
        SetStateAnimation();
    }

    private void SetState()
    {
        _state = transform.localEulerAngles.z switch
        {
            0 => State.LEFT,
            90 => State.DOWN,
            -180 => State.RIGHT,
            -90 => State.UP,
            _ => _state
        };
    }

    private void SetStateAnimation()
    {
        switch (_state)
        {
            case State.LEFT:
                SetAnimation(_state, true);
                SetAnimation(State.UP, false);
                SetAnimation(State.RIGHT, false);
                SetAnimation(State.DOWN, false);
                break;
            case State.RIGHT:
                SetAnimation(_state, true);
                SetAnimation(State.UP, false);
                SetAnimation(State.LEFT, false);
                SetAnimation(State.DOWN, false);
                break;

            case State.DOWN:
                SetAnimation(_state, true);
                SetAnimation(State.UP, false);
                SetAnimation(State.LEFT, false);
                SetAnimation(State.RIGHT, false);
                break;
            case State.UP:
                SetAnimation(_state, true);
                SetAnimation(State.RIGHT, false);
                SetAnimation(State.LEFT, false);
                SetAnimation(State.DOWN, false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetAnimation(State state, bool isActive)
    {
        _animator.SetBool(state.ToString(), isActive);
    }
    
    
}
