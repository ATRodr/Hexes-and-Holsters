using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.SkillTreeSystem
{
    public class PlayerSkillManager : MonoBehaviour
    {
        // Start is called before the first frame update

        // unlockable abilities
        private int chainLightningLevel, destructiveWaveLevel, dynamiteDashLevel, goldenGunLevel;
        private int skillPoints;

        public int ChainLightingLevel => chainLightningLevel;
        public int DestructiveWaveLevel => destructiveWaveLevel;
        public int DynamiteDashLevel => dynamiteDashLevel;
        public int GoldenGunLevel => goldenGunLevel;
        
        public bool ChainLightning => chainLightningLevel > 0;
        public bool DestructiveWave => destructiveWaveLevel > 0;
        public bool DynamiteDash => dynamiteDashLevel > 0;
        public bool GoldenGun => goldenGunLevel > 0;
        
        public int SkillPoints => skillPoints;

        public UnityAction OnSkillPointsChanged;

        private void Awake()
        {
            skillPoints = 10;
        }
        
        public void GainSkillPoint()
        {
            skillPoints++;
            OnSkillPointsChanged?.Invoke();
        }
    }
}
