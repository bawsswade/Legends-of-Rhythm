using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Beatz;

[Serializable]
public class NoteChain : MonoBehaviour {
    [SerializeField]
    public List<NoteBehavior> ChainList = new List<NoteBehavior>();
    private int curBeatIndex = 0;
    private int behaviorIndex = -1;
    private GameObject curBehaviorObj;
    private Action curBehavior;

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("Increment",0, 60f /SongManager.bpm);
        ChangeBehaviors();
	}

    private void Update()
    {
        // limited to one behavior per beat
        curBehavior();
    }

    public void AddNoteBehavior(List<NoteBehavior> _behaviors)
    {
        ChainList = _behaviors;
    }

    private void Increment()
    {
        // check if current behaviros lifetime is up
        if (curBeatIndex < ChainList[behaviorIndex].beatLifetime)
        {
            curBeatIndex++;
        }
        else
        {
            ChangeBehaviors();
        }
    }

    private void ChangeBehaviors()
    {
        curBeatIndex = 0;
        if (behaviorIndex < ChainList.Count-1)
        {
            behaviorIndex++;
        }

        NoteBehavior n = ChainList[behaviorIndex];
        if (n.type == BehaviorType.EXPAND)
        {
            curBehavior = () => Expand((GameObject)n.affectedObj, n.rate);
        }
        else if (n.type == BehaviorType.ROTATE)
        {
            curBehavior = () => Rotate((GameObject)n.affectedObj, n.rotVec, n.rate);
        }
        else if (n.type == BehaviorType.TRANSLATE)
        {
            curBehavior = () => Translate((GameObject)n.affectedObj, n.dirVec, n.rate);
        }
        else if (n.type == BehaviorType.CREATE)
        {
            curBehavior = () => Create(n.prefab, n.startPos);
        }
        else if (n.type == BehaviorType.IDLE)
        {
            curBehavior = Idle;
        }
    }

    public void Translate(GameObject _g, Vector3 _dirVec, float rate)
    {
        _g.transform.position += _dirVec * rate;
    }

    public void Rotate(GameObject _g, Vector3 _rotVec, float rate)
    {
        _g.transform.Rotate(_rotVec * rate);
    }

    public void Expand(GameObject _g, float rate)
    {
        _g.transform.localScale += new Vector3(rate, rate, rate);
    }

    public void Create(GameObject _prefab, Vector3 _startPos)
    {
        Instantiate(_prefab, _startPos, Quaternion.identity);
    }
    
    public void Idle()
    {

    }
}

[Serializable]
public class NoteBehavior 
{
    [Header("Global")]
    [SerializeField]
    public BehaviorType type;
    [SerializeField]
    public int beatLifetime;
    [SerializeField]
    public UnityEngine.Object affectedObj;
    [SerializeField]
    public float rate;

    public NoteBehavior()
    {
        type = BehaviorType.EXPAND;
        beatLifetime = 0;
        affectedObj = null;
        rate = 0;
        dirVec = Vector3.zero;
        rotVec = Vector3.zero;
        prefab = null;
        startPos = Vector3.zero;
    }
    [Header("Translate Variables")]
    public Vector3 dirVec = Vector3.zero;
    [Header("Rotate Variables")]
    public Vector3 rotVec = Vector3.zero;
    [Header("Create Variables")]
    public GameObject prefab = null;
    public Vector3 startPos;
}

[Serializable]
public enum BehaviorType
{
    EXPAND,
    TRANSLATE,
    ROTATE,
    CREATE,
    IDLE
}