using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] SpriteRenderer spikeRenderer;
    [SerializeField] GameObject warningSign;
    public enum State { On, Off, Warning}
    State state = State.Off;
    float timer = 3;

    Cell standingCell;

    private void Start()
    {
        standingCell = TileManager.Instance.WorldToCell(transform.position);
        standingCell.cellObject |= CellObject.Spike;
        standingCell.spike = this;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Toggle();
        }
    }

    void Toggle()
    {
        switch(state)
        {
            case State.On:
                state = State.Off;
                timer = 3;
                spikeRenderer.enabled = false;
                break;
            case State.Off:
                state = State.Warning;
                timer = 0.4f;
                warningSign.SetActive(true);
                break;
            case State.Warning:
                state = State.On;
                timer = 3;
                warningSign.SetActive(false);
                spikeRenderer.enabled = true;
                break;
            default:
                break;
        }
    }

    public bool IsOn()
    {
        return state == State.On;
    }
}
