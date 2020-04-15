namespace UnityEngine.UI
{
    using System;
    using System.Collections;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.Serialization;

    [AddComponentMenu("UI/Toggle Switcher", 0x23), RequireComponent(typeof(RectTransform))]
    public class bl_ToggleSwitcher : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement
    {
        public RectTransform Switcher;
        [SerializeField, Tooltip("Is the toggle currently on or off?"), FormerlySerializedAs("m_IsActive")]
        private bool m_IsOn;
        [SerializeField]
        private Vector2 IsOnPosition = Vector2.zero;
        [SerializeField]
        private Vector2 IsOffPosition = Vector2.zero;
        [SerializeField, Range(1, 10)]
        private float LerpSwitch = 3;

        public ToggleEvent onValueChanged = new ToggleEvent();

        private void InternalToggle()
        {
            if (this.IsActive() && this.IsInteractable())
            {
                this.isOn = !this.isOn;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.PlayEffect();
        }


        public void LayoutComplete()
        {

        }

        public void GraphicUpdateComplete()
        {

        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                this.InternalToggle();
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.InternalToggle();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.Set(this.m_IsOn, false);
            this.PlayEffect();
            if ((PrefabUtility.GetPrefabType(this) != PrefabType.Prefab) && !Application.isPlaying)
            {
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
        }
#endif
        private void PlayEffect()
        {
            if (this.Switcher != null)
            {
                if (!Application.isPlaying)
                {
                    Vector2 p = (this.m_IsOn) ? IsOnPosition : IsOffPosition; ;
                    Switcher.anchoredPosition = p;
                }
                else
                {
                    StopAllCoroutines();
                    StopCoroutine("Switch");
                    Vector2 p = (this.m_IsOn) ? IsOnPosition : IsOffPosition;
                    StartCoroutine(Switch(p));
                }
            }
        }

        IEnumerator Switch(Vector2 nextPos)
        {
            if (Switcher == null) { yield break; }
            while (Switcher.anchoredPosition != nextPos)
            {
                Switcher.anchoredPosition = Vector2.Lerp(Switcher.anchoredPosition, nextPos, Time.deltaTime * (LerpSwitch * 2));
                yield return null;
            }
        }

        public virtual void Rebuild(CanvasUpdate executing)
        {
            if (executing == CanvasUpdate.Prelayout)
            {
                this.onValueChanged.Invoke(this.m_IsOn);
            }
        }

        private void Set(bool value)
        {
            this.Set(value, true);
        }

        private void Set(bool value, bool sendCallback)
        {
            if (this.m_IsOn != value)
            {
                this.m_IsOn = value;
                this.PlayEffect();
                if (sendCallback)
                {
                    this.onValueChanged.Invoke(this.m_IsOn);
                }
            }
        }

        protected override void Start()
        {
            this.PlayEffect();
        }

        bool ICanvasElement.IsDestroyed()
        {
            return base.IsDestroyed();
        }

        public bool isOn
        {
            get
            {
                return this.m_IsOn;
            }
            set
            {
                this.Set(value);
            }
        }

        [ContextMenu("Get On Pos")]
        void GetOnPos()
        {
            if (Switcher != null)
            {
                IsOnPosition = Switcher.anchoredPosition;
            }
        }
        [ContextMenu("Get Off Pos")]
        void GetOffPos()
        {
            if (Switcher != null)
            {
                IsOffPosition = Switcher.anchoredPosition;
            }
        }

        [Serializable]
        public class ToggleEvent : UnityEvent<bool>
        {
        }
    }
}