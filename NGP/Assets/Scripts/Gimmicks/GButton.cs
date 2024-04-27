using EnumDefines;
using UnityEngine;


public abstract class GButton : GimmickBase
{
    [SerializeField] protected ButtonType _type;
    //[SerializeField] protected bool isPairButton = false;

    private bool show;
    
    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //@TODO, 마우스커서로 옮겨서라도 테스트 해보기
        if (other.CompareTag("Player"))
        {
            Debug.Log("[GButton.OnTriggerEnter2D]");
            Activate();
        }
    }
}
