using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimatorTeste : MonoBehaviour
{
    #region Variables

    [Header("Configurações de Animators")]
    [Tooltip("Lista de Animators para controlar")]
    [SerializeField] private List<Animator> animators = new List<Animator>();

    [Header("Configurações de Parâmetros")]
    [Tooltip("Parâmetros de animação (mesmo índice que Animator). Usado no método PlayAllIndexed.")]
    [SerializeField] private List<AnimationParameter> parameters = new List<AnimationParameter>();

    [Header("Configurações de Debug")]
    [Tooltip("Ativa logs detalhados no console para debug")]
    [SerializeField] private bool logDebug = false;

    [Header("Configurações de Auto Reset")]
    [Tooltip("Ativa reset automático de parâmetros após disparar animações")]
    [SerializeField] private bool autoResetEnabled = false;

    [Tooltip("Tempo em segundos antes de resetar os parâmetros disparados")]
    [Range(0.1f, 10f)]
    [SerializeField] private float autoResetDelay = 2f;

    [Tooltip("Reverte parâmetros Bool para false após o delay")]
    [SerializeField] private bool resetBoolsToFalse = true;

    [Tooltip("Redefine parâmetros Int e Float para 0 após o delay")]
    [SerializeField] private bool resetNumbersToZero = false;

    [Header("Cores do Inspetor")]
    [Tooltip("Cor do Script no Inspetor")]
    public Color corInspetor = Color.white;

    private Coroutine autoResetRoutine;

    #endregion

    #region Estruturas

    public enum ParameterType { Trigger, BoolTrue, BoolFalse, Int, Float }

    [System.Serializable]
    public class AnimationParameter
    {
        [Tooltip("Nome do parâmetro no Animator")]
        public string name;

        [Tooltip("Tipo de parâmetro a ser aplicado")]
        public ParameterType type = ParameterType.Trigger;

        [Tooltip("Valor inteiro (usado apenas quando Type = Int)")]
        public int intValue;

        [Tooltip("Valor float (usado apenas quando Type = Float)")]
        public float floatValue;

        public void Apply(Animator animator)
        {
            if (animator == null || string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("AnimationParameter: Animator nulo ou nome de parâmetro vazio.");
                return;
            }

            if (!HasParameter(animator, name))
            {
                Debug.LogWarning($"AnimationParameter: Parâmetro '{name}' não encontrado no Animator '{animator.name}'.");
                return;
            }

            switch (type)
            {
                case ParameterType.Trigger:
                    animator.SetTrigger(name);
                    break;
                case ParameterType.BoolTrue:
                    animator.SetBool(name, true);
                    break;
                case ParameterType.BoolFalse:
                    animator.SetBool(name, false);
                    break;
                case ParameterType.Int:
                    animator.SetInteger(name, intValue);
                    break;
                case ParameterType.Float:
                    animator.SetFloat(name, floatValue);
                    break;
            }
        }

        private bool HasParameter(Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName) return true;
            }
            return false;
        }
    }

    #endregion

    #region Métodos Públicos

    public void PlayAllIndexed()
    {
        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        if (parameters.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum parâmetro de animação configurado!");
            return;
        }

        for (int i = 0; i < animators.Count; i++)
        {
            var animator = animators[i];
            if (animator == null)
            {
                Debug.LogWarning($"[AnimatorTeste] Animator no índice {i} é nulo!");
                continue;
            }

            var param = i < parameters.Count ? parameters[i] : parameters[parameters.Count - 1];
            param.Apply(animator);
            DebugLog($"[AnimatorTeste] Animator {i} - parâmetro '{param.name}' aplicado.");
        }
        ScheduleAutoReset();
    }

    public void PlayTriggerAll(string triggerName)
    {
        if (string.IsNullOrEmpty(triggerName))
        {
            Debug.LogWarning("[AnimatorTeste] Nome do trigger está vazio!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        foreach (var animator in animators)
        {
            if (animator == null) continue;
            animator.SetTrigger(triggerName);
            DebugLog($"[AnimatorTeste] Trigger '{triggerName}' enviado para {animator.name}.");
            appliedCount++;
        }

        DebugLog($"[AnimatorTeste] Trigger '{triggerName}' aplicado em {appliedCount} animator(s).");
        ScheduleAutoReset(triggerName);
    }

    public void SetBoolAll(string boolName, bool value)
    {
        if (string.IsNullOrEmpty(boolName))
        {
            Debug.LogWarning("[AnimatorTeste] Nome do bool está vazio!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        foreach (var animator in animators)
        {
            if (animator == null) continue;
            animator.SetBool(boolName, value);
            DebugLog($"[AnimatorTeste] Bool '{boolName}'={value} em {animator.name}.");
            appliedCount++;
        }

        DebugLog($"[AnimatorTeste] Bool '{boolName}' aplicado em {appliedCount} animator(s).");
        ScheduleAutoReset(boolName);
    }

    public void PlayDynamicTriggers(IList<string> triggerList)
    {
        if (triggerList == null || triggerList.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Lista de triggers dinâmicos está vazia ou nula!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        for (int i = 0; i < animators.Count; i++)
        {
            var animator = animators[i];
            if (animator == null) continue;

            string trig = i < triggerList.Count ? triggerList[i] : triggerList[triggerList.Count - 1];
            if (!string.IsNullOrEmpty(trig))
            {
                animator.SetTrigger(trig);
                DebugLog($"[AnimatorTeste] Dynamic trigger '{trig}' aplicado em {animator.name}.");
                appliedCount++;
            }
        }

        DebugLog($"[AnimatorTeste] {appliedCount} trigger(s) dinâmico(s) aplicado(s).");
        ScheduleAutoReset();
    }

    public void PlayDynamicParameters(IList<AnimationParameter> paramList)
    {
        if (paramList == null || paramList.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Lista de parâmetros dinâmicos está vazia ou nula!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        for (int i = 0; i < animators.Count; i++)
        {
            var animator = animators[i];
            if (animator == null) continue;

            AnimationParameter p = i < paramList.Count ? paramList[i] : paramList[paramList.Count - 1];
            p.Apply(animator);
            DebugLog($"[AnimatorTeste] Dynamic parâmetro '{p.name}' aplicado em {animator.name}.");
            appliedCount++;
        }

        DebugLog($"[AnimatorTeste] {appliedCount} parâmetro(s) dinâmico(s) aplicado(s).");
        ScheduleAutoReset();
    }

    public void ResetAllTriggers()
    {
        if (animators.Count == 0 || parameters.Count == 0)
        {
            return;
        }

        int resetCount = 0;
        foreach (var animator in animators)
        {
            if (animator == null) continue;

            foreach (var p in parameters)
            {
                if (string.IsNullOrEmpty(p.name)) continue;

                if (p.type == ParameterType.Trigger)
                {
                    animator.ResetTrigger(p.name);
                    resetCount++;
                }

                if (resetBoolsToFalse && (p.type == ParameterType.BoolTrue || p.type == ParameterType.BoolFalse))
                {
                    animator.SetBool(p.name, false);
                    resetCount++;
                }

                if (resetNumbersToZero)
                {
                    if (p.type == ParameterType.Int)
                    {
                        animator.SetInteger(p.name, 0);
                        resetCount++;
                    }
                    if (p.type == ParameterType.Float)
                    {
                        animator.SetFloat(p.name, 0f);
                        resetCount++;
                    }
                }
            }
        }

        DebugLog($"[AnimatorTeste] Reset automático executado. {resetCount} parâmetro(s) resetado(s).");
    }

    public void AddAnimator(Animator animator)
    {
        if (animator != null && !animators.Contains(animator))
        {
            animators.Add(animator);
            DebugLog($"[VfxController] Animator '{animator.name}' adicionado.");
        }
    }

    public void RemoveAnimator(Animator animator)
    {
        if (animator != null && animators.Remove(animator))
        {
            DebugLog($"[VfxController] Animator '{animator.name}' removido.");
        }
    }

    public void ForceAutoResetNow()
    {
        if (autoResetRoutine != null)
        {
            StopCoroutine(autoResetRoutine);
            autoResetRoutine = null;
        }
        ResetAllTriggers();
        DebugLog("[AnimatorTeste] Reset forçado manualmente.");
    }

    public void ClearAllAnimators()
    {
        animators.Clear();
        DebugLog("[AnimatorTeste] Todos os Animators foram removidos da lista.");
    }

    public void SetIntegerAll(string intName, int value)
    {
        if (string.IsNullOrEmpty(intName))
        {
            Debug.LogWarning("[AnimatorTeste] Nome do parâmetro int está vazio!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        foreach (var animator in animators)
        {
            if (animator == null) continue;
            animator.SetInteger(intName, value);
            DebugLog($"[AnimatorTeste] Integer '{intName}'={value} em {animator.name}.");
            appliedCount++;
        }

        DebugLog($"[AnimatorTeste] Integer '{intName}' aplicado em {appliedCount} animator(s).");
        ScheduleAutoReset(intName);
    }

    public void SetFloatAll(string floatName, float value)
    {
        if (string.IsNullOrEmpty(floatName))
        {
            Debug.LogWarning("[AnimatorTeste] Nome do parâmetro float está vazio!");
            return;
        }

        if (animators.Count == 0)
        {
            Debug.LogWarning("[AnimatorTeste] Nenhum Animator atribuído na lista!");
            return;
        }

        int appliedCount = 0;
        foreach (var animator in animators)
        {
            if (animator == null) continue;
            animator.SetFloat(floatName, value);
            DebugLog($"[AnimatorTeste] Float '{floatName}'={value} em {animator.name}.");
            appliedCount++;
        }

        DebugLog($"[AnimatorTeste] Float '{floatName}' aplicado em {appliedCount} animator(s).");
        ScheduleAutoReset(floatName);
    }
    #endregion

    #region Auto Reset

    private void ScheduleAutoReset(string singleParamName = null)
    {
        if (!autoResetEnabled) return;

        if (autoResetRoutine != null)
        {
            StopCoroutine(autoResetRoutine);
        }

        autoResetRoutine = StartCoroutine(AutoResetCoroutine());
        string paramInfo = singleParamName != null ? $"para '{singleParamName}'" : "para parâmetros disparados";
        DebugLog($"[AnimatorTeste] Auto reset agendado ({autoResetDelay}s) {paramInfo}");
    }

    private IEnumerator AutoResetCoroutine()
    {
        yield return new WaitForSeconds(autoResetDelay);
        ResetAllTriggers();
        autoResetRoutine = null;
    }

    #endregion

    #region Utilities

    private void DebugLog(string msg)
    {
        if (logDebug)
        {
            Debug.Log(msg, this);
        }
    }

    private void OnValidate()
    {
        if (autoResetDelay < 0.1f)
        {
            autoResetDelay = 0.1f;
        }

        if (animators.Contains(null))
        {
            animators.RemoveAll(a => a == null);
        }
    }

    #endregion
}
