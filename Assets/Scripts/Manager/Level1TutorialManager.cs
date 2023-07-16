using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject _moveShootGuide;
    [SerializeField] private GameObject _debuffGuide;
    [SerializeField] private GameObject _reviveGuide;  
    [SerializeField] private GameObject _researchSystemGuide;

    private bool _finishMoveShootGuide;
    private bool _finishDebuffGuide;
    private bool _finishReviveGuide;
    private bool _finishResearchSystemGuide;

    [SerializeField] private GameObject gameManage;

    void Start()
    {
        
    }

    
    void Update()
    {
        //StartGuidence.
    }

    private IEnumerator MoveAndShootGuide()
    {
        yield return null;
    }

    private IEnumerator DebuffGuide()
    {
        yield return null;
    }

    private IEnumerator ReviveGuide()
    {
        yield return null;
    }


    private IEnumerator ReSearchSystemGuide()
    {
        yield return null;
    }

    private IEnumerator StartGuidence() {
        gameManage.SetActive(false);

        while (!_finishMoveShootGuide)
        {
            yield return null;
        }
    }
}
