using UnityEngine;
using UnityEngine.UI;

public class MicroscopeSlideCell : MonoBehaviour
{
    [SerializeField] private int ChaetocerosAmount;
    [SerializeField] private int PseudoNitzschiaAmount;
    [SerializeField] private int SkeletonemaAmount;

    public int GetTotalPlanktonCount()
    {
        return ChaetocerosAmount + PseudoNitzschiaAmount + SkeletonemaAmount;
    }

    public int GetChaetocerosCount()
    {
        return ChaetocerosAmount; 
    }

    public int GetPseudoNitzschiaCount()
{
        return PseudoNitzschiaAmount;
    }

    public int GetSkeletonemaCount()
    {
        return SkeletonemaAmount;
    }




}
