﻿using SFML.Graphics;
using SFML.System;
using SS14.Shared.GameObjects;
using SS14.Shared.GO;
using SS14.Shared.GO.Component.Hitbox;
using System;

namespace SS14.Client.GameObjects
{
    public class HitboxComponent : Component {

        public FloatRect AABB { get; set; }
        public Vector2f Size {
            get {
                return new Vector2f(AABB.Width, AABB.Height);
            }
            set {
                AABB = new FloatRect(
                    AABB.Left + (AABB.Width - value.X),
                    AABB.Top + (AABB.Height - value.Y),
                    value.X,
                    value.Y
                    );
            }
        }
        public Vector2f Offset {
            get {
                return new Vector2f(AABB.Left + AABB.Width / 2f, AABB.Top + AABB.Height / 2f);
            }
            set {
                AABB = new FloatRect(
                    value.X - AABB.Width / 2f,
                    value.Y - AABB.Height / 2f,
                    AABB.Width,
                    AABB.Height
                    );
            }
        }


        public HitboxComponent() {
            Family = ComponentFamily.Hitbox;
            Size = new Vector2f();
            Offset = new Vector2f();
        }

        public override Type StateType {
            get {
                return typeof(HitboxComponentState);
            }
        }

        public override void HandleComponentState(dynamic state) {
            AABB = state.AABB;
        }

    }
}
