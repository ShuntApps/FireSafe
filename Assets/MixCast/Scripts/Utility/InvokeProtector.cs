/**********************************************************************************
* Blueprint Reality Inc. CONFIDENTIAL
* 2019 Blueprint Reality Inc.
* All Rights Reserved.
*
* NOTICE:  All information contained herein is, and remains, the property of
* Blueprint Reality Inc. and its suppliers, if any.  The intellectual and
* technical concepts contained herein are proprietary to Blueprint Reality Inc.
* and its suppliers and may be covered by Patents, pending patents, and are
* protected by trade secret or copyright law.
*
* Dissemination of this information or reproduction of this material is strictly
* forbidden unless prior written permission is obtained from Blueprint Reality Inc.
***********************************************************************************/

#if UNITY_STANDALONE_WIN
using System.Collections.Generic;
using UnityEngine;

namespace BlueprintReality.MixCast
{
    public class InvokeProtector : MonoBehaviour
    {
        private const float MAX_ALLOWED_RATE = 1f / 360f;

        private static InvokeProtector m_Instance = null;

        private static void CreateInstance()
        {
            if (m_Instance == null)
            {
                GameObject go = new GameObject("_INVOKE_PROTECTOR_");
                DontDestroyOnLoad(go);
                go.hideFlags = HideFlags.HideAndDontSave;
                m_Instance = go.AddComponent<InvokeProtector>();
            }
        }

        public delegate void invoke_delegate();
        private List<invoke_delegate> _protected_invoke_func_table_ = new List<invoke_delegate>();
        private List<long> _protected_invoke_timestamp_table_ = new List<long>();
        private List<long> _protected_repeat_rate_table_ = new List<long>();

        public static void InvokeRepeating(invoke_delegate method, float start_time = 0f, float fastest_allowed_rate = 1f / 90f)
        {
            if (m_Instance == null)
            {
                CreateInstance();
            }
            if (m_Instance != null)
            {
                m_Instance._invoke_repeating_(method, start_time, fastest_allowed_rate);
            }
        }

        private void _invoke_repeating_(invoke_delegate method, float start_time, float fastest_allowed_rate)
        {
            if (!_protected_invoke_func_table_.Contains(method))
            {
                _protected_invoke_func_table_.Add(method);

                long _maximum_allowed_rate_ = Mathf.FloorToInt(Mathf.Max(fastest_allowed_rate, MAX_ALLOWED_RATE) * 1000);

                _protected_repeat_rate_table_.Add(_maximum_allowed_rate_);

                long _current_timestamp_ = System.Diagnostics.Stopwatch.GetTimestamp();
                if (start_time < float.Epsilon)
                {
                    method();
                    _protected_invoke_timestamp_table_.Add(_current_timestamp_);
                }
                else
                {
                    var _valid_timestamp_ = _current_timestamp_ + Mathf.FloorToInt(start_time * 1000) - _maximum_allowed_rate_;
                    _protected_invoke_timestamp_table_.Add(_valid_timestamp_);
                }
            }
            else
            {
                throw new System.InvalidOperationException("Same delegate already invoking!");
            }
            if (!IsInvoking("_protected_invoke_repeating_"))
            {
                InvokeRepeating("_protected_invoke_repeating_", 0, MAX_ALLOWED_RATE);
            }
        }

        public static void CancelInvokeRepeating(invoke_delegate method)
        {
            if (m_Instance != null)
            {
                m_Instance._cancel_invoke_repeating_(method);
            }
        }

        private void _cancel_invoke_repeating_(invoke_delegate method)
        {
            if (_protected_invoke_func_table_.Contains(method))
            {
                int index = _protected_invoke_func_table_.IndexOf(method);
                _protected_invoke_func_table_.RemoveAt(index);
                _protected_invoke_timestamp_table_.RemoveAt(index);
                _protected_repeat_rate_table_.RemoveAt(index);

                if (_protected_invoke_func_table_.Count <= 0)
                {
                    if (IsInvoking("_protected_invoke_repeating_"))
                    {
                        CancelInvoke("_protected_invoke_repeating_");
                    }
                }
            }
            else
            {
                throw new System.InvalidOperationException("Required delegate is not invoking!");
            }
        }

        private void _protected_invoke_repeating_()
        {
            long current_tick = System.Diagnostics.Stopwatch.GetTimestamp();
            for (int i = 0; i < _protected_invoke_func_table_.Count; i++)
            {
                var old_tick = _protected_invoke_timestamp_table_[i];
                long gap = (current_tick - old_tick) * 1000 / System.Diagnostics.Stopwatch.Frequency;
                if (gap > _protected_repeat_rate_table_[i])
                {
                    _protected_invoke_func_table_[i]();
                    _protected_invoke_timestamp_table_[i] = current_tick;
                }
            }
        }

        private void OnDestroy()
        {
            _protected_invoke_func_table_.Clear();
            _protected_invoke_timestamp_table_.Clear();
            _protected_repeat_rate_table_.Clear();

            if (IsInvoking("_protected_invoke_repeating_"))
            {
                CancelInvoke("_protected_invoke_repeating_");
            }

            m_Instance = null;
        }
    }
}
#endif
