using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RandomEnum : MonoBehaviour {

    public List<Action> Types;

}
public enum Action
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}
