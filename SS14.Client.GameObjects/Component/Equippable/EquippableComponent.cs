﻿using SS14.Shared;
using SS14.Shared.GameObjects;
using SS14.Shared.GO;
using SS14.Shared.GO.Component.Equippable;

namespace SS14.Client.GameObjects
{
    public class EquippableComponent : Component
    {
        /// <summary>
        /// Where is this equipment being worn
        /// </summary>
        public EquipmentSlot wearloc;

        /// <summary>
        /// What entity is wearing this equipment
        /// </summary>
        public Entity currentWearer { get; set; }
        
        public EquippableComponent()
        {
            Family = ComponentFamily.Equippable;
        }

        public override System.Type StateType
        {
            get { return typeof (EquippableComponentState); }
        }

        /// <summary>
        /// Handles equipped state change
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="wearloc"></param>
        private void EquippedBy(int uid, EquipmentSlot wearloc)
        {
            currentWearer = Owner.EntityManager.GetEntity(uid);
            this.wearloc = wearloc;
        }

        /// <summary>
        /// Handles unequipped state change
        /// </summary>
        private void UnEquipped()
        {
            currentWearer = null;
            wearloc = EquipmentSlot.None;
        }

        /// <summary>
        /// Handles incoming component state
        /// </summary>
        /// <param name="state"></param>
        public override void HandleComponentState(dynamic state)
        {
            int? holderUid = currentWearer != null ? currentWearer.Uid : (int?) null;
            if(state.Holder != holderUid)
            {
                if(state.Holder == null)
                {
                    UnEquipped();
                }
                else
                {
                    EquippedBy((int)state.Holder, state.WearLocation);
                }
            }
        }
    }
}