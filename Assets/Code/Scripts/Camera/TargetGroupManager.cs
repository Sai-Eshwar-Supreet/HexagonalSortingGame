using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class TargetGroupManager : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup targetGroup;
    public void AddTarget(Transform target)
    {
        if (targetGroup == null || target == null)
        {
            Debug.LogError("Target Group or Target is null.");
            return;
        }

        var newTarget = new CinemachineTargetGroup.Target
        {
            target = target,
            weight = 1,
            radius = 1
        };

        targetGroup.m_Targets = targetGroup.m_Targets.Concat(new[] { newTarget }).ToArray();
    }
    public void ClearTargets()
    {
        if (targetGroup == null)
        {
            Debug.LogError("Target Group is not assigned.");
            return;
        }

        targetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
    }
    public void RemoveTarget(Transform target)
    {
        if (targetGroup == null || target == null)
        {
            Debug.LogError("Target Group or Target is null.");
            return;
        }

        targetGroup.m_Targets = targetGroup.m_Targets.Where(t => t.target != target).ToArray();
    }

}
