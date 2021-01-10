using GameManager;
using UnityEngine;

namespace NewBehav
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private void SetStateColor(States.State state)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (state == States.State.Dead)
            {
                renderer.material.color = Color.white;
            }
            else
            {
                renderer.material.color = Color.black;
            }
        }

        private void OnMouseDown()
        {
            //change state only if simulation is not running.
            if (!Map.MyReference.isSimulationRunning)
            {
                if (Map.MyReference.GetState(transform.position.x, transform.position.y) == States.State.Dead)
                {
                    Map.MyReference.SetState(transform.position.x, transform.position.y, States.State.Alive);
                    SetStateColor(States.State.Alive);
                }
                else
                {
                    Map.MyReference.SetState(transform.position.x, transform.position.y, States.State.Dead);
                    SetStateColor(States.State.Dead);
                }
            }
        }
    }
}

