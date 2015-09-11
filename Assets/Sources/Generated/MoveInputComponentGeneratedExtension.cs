using System.Collections.Generic;

namespace Entitas {
    public partial class Entity {
        public MoveInputComponent moveInput { get { return (MoveInputComponent)GetComponent(ComponentIds.MoveInput); } }

        public bool hasMoveInput { get { return HasComponent(ComponentIds.MoveInput); } }

        static readonly Stack<MoveInputComponent> _moveInputComponentPool = new Stack<MoveInputComponent>();

        public static void ClearMoveInputComponentPool() {
            _moveInputComponentPool.Clear();
        }

        public Entity AddMoveInput(Movement newMovement) {
            var component = _moveInputComponentPool.Count > 0 ? _moveInputComponentPool.Pop() : new MoveInputComponent();
            component.movement = newMovement;
            return AddComponent(ComponentIds.MoveInput, component);
        }

        public Entity ReplaceMoveInput(Movement newMovement) {
            var previousComponent = hasMoveInput ? moveInput : null;
            var component = _moveInputComponentPool.Count > 0 ? _moveInputComponentPool.Pop() : new MoveInputComponent();
            component.movement = newMovement;
            ReplaceComponent(ComponentIds.MoveInput, component);
            if (previousComponent != null) {
                _moveInputComponentPool.Push(previousComponent);
            }
            return this;
        }

        public Entity RemoveMoveInput() {
            var component = moveInput;
            RemoveComponent(ComponentIds.MoveInput);
            _moveInputComponentPool.Push(component);
            return this;
        }
    }

    public partial class Pool {
        public Entity moveInputEntity { get { return GetGroup(Matcher.MoveInput).GetSingleEntity(); } }

        public MoveInputComponent moveInput { get { return moveInputEntity.moveInput; } }

        public bool hasMoveInput { get { return moveInputEntity != null; } }

        public Entity SetMoveInput(Movement newMovement) {
            if (hasMoveInput) {
                throw new SingleEntityException(Matcher.MoveInput);
            }
            var entity = CreateEntity();
            entity.AddMoveInput(newMovement);
            return entity;
        }

        public Entity ReplaceMoveInput(Movement newMovement) {
            var entity = moveInputEntity;
            if (entity == null) {
                entity = SetMoveInput(newMovement);
            } else {
                entity.ReplaceMoveInput(newMovement);
            }

            return entity;
        }

        public void RemoveMoveInput() {
            DestroyEntity(moveInputEntity);
        }
    }

    public partial class Matcher {
        static AllOfMatcher _matcherMoveInput;

        public static AllOfMatcher MoveInput {
            get {
                if (_matcherMoveInput == null) {
                    _matcherMoveInput = new Matcher(ComponentIds.MoveInput);
                }

                return _matcherMoveInput;
            }
        }
    }
}
