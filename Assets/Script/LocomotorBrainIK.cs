using System;
using UnityEngine;

public class LocomotorBrainIK : MonoBehaviour
{
    [SerializeField] private int _maxLegMove;
    [SerializeField] private LegLocomotorIK[] _legLocomotorIks;

    private int _currentLegMove;
    private void Start() {
        foreach (var leg in _legLocomotorIks) {
            if( leg==null) continue;
            leg.OnLegStartMouvement += LegOnOnLegStartMouvement;
            leg.OnLegEndMouvement += LegOnOnLegEndMouvement;
        }
    }

    private void OnDestroy() {
        foreach (var leg in _legLocomotorIks) {
            if( leg==null) continue;
            leg.OnLegStartMouvement -= LegOnOnLegStartMouvement;
            leg.OnLegEndMouvement -= LegOnOnLegEndMouvement;
        }
    }

    private void LegOnOnLegEndMouvement(object sender, EventArgs e) {
        _currentLegMove--;
        CheckLegLock();
    }

    private void LegOnOnLegStartMouvement(object sender, EventArgs e) {
        _currentLegMove++;
        CheckLegLock();
    }

    private void CheckLegLock() {
        foreach (var leg in _legLocomotorIks) { 
            if (leg == null)continue;
            leg.LegMoveLock = _currentLegMove >= _maxLegMove;
        }
    }
}