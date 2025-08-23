using UnityEngine;

namespace RPG.UI
{
    public class PlayerStatsModel
    {
        /// <summary>
        /// 当前装备的数据来源（ScriptableObject）
        /// </summary>
        public EquipmentData Equipment { get; private set; }

        public void SetEquipment(EquipmentData equipment)
        {
            Equipment = equipment;
        }
    }
}