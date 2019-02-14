﻿using BepInEx;
using UniRx;

namespace KKAPI.Maker.UI
{
    public abstract class BaseEditableGuiEntry<TValue> : BaseGuiEntry
    {
        private readonly BehaviorSubject<TValue> _incomingValue;
        private readonly Subject<TValue> _outgoingValue;

        protected BaseEditableGuiEntry(MakerCategory category, TValue initialValue, BaseUnityPlugin owner) : base(category, owner)
        {
            _incomingValue = new BehaviorSubject<TValue>(initialValue);
            _outgoingValue = new Subject<TValue>();
        }

        /// <summary>
        /// Buttons 1, 2, 3 are values 0, 1, 2
        /// </summary>
        public TValue Value
        {
            get => _incomingValue.Value;
            set
            {
                if (!Equals(value, _incomingValue.Value))
                    SetNewValue(value);
            }
        }

        /// <summary>
        /// Fired every time the value is changed, and once when the control is created.
        /// Buttons 1, 2, 3 are values 0, 1, 2
        /// </summary>
        public IObservable<TValue> ValueChanged => _outgoingValue;

        /// <summary>
        /// Use to get value changes for controls. Fired by external value set and by SetNewValue.
        /// </summary>
        protected IObservable<TValue> BufferedValueChanged => _incomingValue;

        /// <summary>
        /// Trigger value changed events and set the value
        /// </summary>
        protected void SetNewValue(TValue newValue)
        {
            _incomingValue.OnNext(newValue);
            _outgoingValue.OnNext(newValue);
        }

        public override void Dispose()
        {
            _incomingValue.Dispose();
            _outgoingValue.Dispose();
        }
    }
}