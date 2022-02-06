using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    private Animator playerAnimator;
    
    // start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // set any animation parameter initialized in playerAnimator
    public void SetAnimationParam<T>(string paramName, T paramValue) {
        if (typeof(T) == typeof(float)) 
        {
            playerAnimator.SetFloat(paramName, (float)(object)paramValue);
        } else if (typeof(T) == typeof(int)) 
        {
            playerAnimator.SetInteger(paramName, (int)(object)paramValue);
        } else if (typeof(T) == typeof(bool))
        {
            playerAnimator.SetBool(paramName, (bool)(object)paramValue);
        }
        else
        {
            throw new ArgumentException(String.Format("{0} must be of float, bool or integer type.", paramValue), "paramValue");
        }
    }

    // get any animation parameter initialized in playerAnimator
    public T GetAnimationParam<T>(string paramName) {
        if (typeof(T) == typeof(float)) 
        {
            return (T)Convert.ChangeType(playerAnimator.GetFloat(paramName), typeof(T));
        } else if (typeof(T) == typeof(int)) 
        {
            return (T)Convert.ChangeType(playerAnimator.GetInteger(paramName), typeof(T));
        } else if (typeof(T) == typeof(bool))
        {
            return (T)Convert.ChangeType(playerAnimator.GetBool(paramName), typeof(T));
        }
        else
        {
            throw new NotSupportedException(String.Format("GetAnimationParam<{0}> type must be of float, bool or integer.", typeof(T).ToString()));
        }
    }
}
