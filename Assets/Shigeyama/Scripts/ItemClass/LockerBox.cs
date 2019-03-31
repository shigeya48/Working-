using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerBox : MonoBehaviour,IGimmick
{
    [SerializeField]
    GameObject Brush;

    Animator anim;

    bool brushExistence = true;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public GameObject Item()
    {
        brushExistence = false;

        anim.SetBool("IsLocker", true);

        return Instantiate(Brush, transform.position, Quaternion.identity);
    }

    public bool BrushExistence
    {
        get { return brushExistence; }
        set { brushExistence = value; }
    }

    public void PlayGimmick(GameObject player)
    {
        if (player.GetComponent<PlayerSystem>().GetCatchItem != null
            && player.GetComponent<PlayerSystem>().GetCatchItem.GetComponent<BrushItem>())
        {
            player.GetComponent<PlayerSystem>().DestroyItem();
            brushExistence = true;
            anim.SetBool("IsLocker", false);
        }
    }

    public bool GimmickIsEvent()
    {
        return false;
    }
}
