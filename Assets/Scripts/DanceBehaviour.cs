using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _followPlayerGO;
    [SerializeField] private Animator _animator = default;
    private string _tag = default;
    public bool isDancing = false;

    void FixedUpdate()
    {
        _tag = transform.parent.tag;

        if (!isDancing)
        {   
            if (DanceSystem.marker == "bat_dance" && _tag == "Bat")
            {
                MakeItDance();
            }
            else if(DanceSystem.marker == "skeleton_idle" && _tag == "Skeleton")
            {
                MakeItDance();
            }
        }
        
    }

    private void MakeItDance()
    {
        isDancing = true;
        _animator.SetTrigger("ToDancing");
        StartCoroutine(WaitToDanceAgain());
    }

    IEnumerator WaitToDanceAgain()
    {
        yield return new WaitForSeconds(2f);
        isDancing = false;
    }
}
