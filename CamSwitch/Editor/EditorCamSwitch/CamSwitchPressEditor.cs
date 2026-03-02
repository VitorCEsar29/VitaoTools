using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CamSwitchPress))]
public class CamSwitchPressEditor : Editor
{
    private SerializedProperty configuracoesCameras;
    private SerializedProperty indiceCameraAtiva;
    private SerializedProperty tagAlvo;
    private SerializedProperty mostrarStatus;

    private void OnEnable()
    {
        configuracoesCameras = serializedObject.FindProperty("configuracoesCameras");
        indiceCameraAtiva = serializedObject.FindProperty("indiceCameraAtiva");
        tagAlvo = serializedObject.FindProperty("tagAlvo");
        mostrarStatus = serializedObject.FindProperty("mostrarStatus");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CamSwitchPress script = (CamSwitchPress)target;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Camera Switch System", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Sistema de troca de câmeras por trigger. Configure as câmeras secundárias e o índice da câmera que será ativada.", MessageType.Info);

        if (GUILayout.Button("📖 Ver Documentação Completa"))
        {
            Application.OpenURL("https://github.com/VitorCEsar29/VitaoTools/blob/main/CamSwitch/Script/CamSwitch/README.md");
        }

        EditorGUILayout.Space(10);

        DrawCameraSettings(script);

        EditorGUILayout.Space(10);

        DrawTriggerSettings();

        EditorGUILayout.Space(10);

        DrawStatusInfo(script);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawCameraSettings(CamSwitchPress script)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("📷 Configurações de Câmeras", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        EditorGUILayout.PropertyField(configuracoesCameras, new GUIContent("Câmeras Secundárias"), true);

        int totalCameras = script.GetTotalCameras();

        if (totalCameras > 0)
        {
            EditorGUILayout.Space(5);
            var configs = script.GetConfiguracoesCameras();

            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                string status = config.boxCollider != null ? "✓ Collider específico" : "⊙ Usa collider padrão";
                string cameraName = config.camera != null ? config.camera.name : "Não definida";
                EditorGUILayout.LabelField($"  [{i}] {config.nome}: {cameraName} - {status}", EditorStyles.miniLabel);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("⚠️ Adicione pelo menos uma configuração de câmera!", MessageType.Warning);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawTriggerSettings()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("🎯 Trigger Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        EditorGUILayout.PropertyField(tagAlvo, new GUIContent("Tag do Objeto"));

        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox("💡 Não esqueça:\n• Adicionar um Collider com 'Is Trigger' ativado\n• Configurar a tag correta no objeto que vai ativar", MessageType.Info);

        EditorGUILayout.EndVertical();
    }

    private void DrawStatusInfo(CamSwitchPress script)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.PropertyField(mostrarStatus, new GUIContent("Mostrar Status em Runtime"));

        if (mostrarStatus.boolValue && Application.isPlaying)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("📊 Status Runtime", EditorStyles.boldLabel);

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Câmera Principal", script.GetCameraPrincipal(), typeof(Camera), true);
            EditorGUILayout.ObjectField("Câmera Ativa", script.GetCameraAtiva(), typeof(Camera), true);

            int indiceAtivo = script.GetIndiceCameraAtiva();
            if (indiceAtivo >= 0)
            {
                EditorGUILayout.LabelField("Índice da Câmera Ativa", indiceAtivo.ToString());
            }
            else
            {
                EditorGUILayout.LabelField("Índice da Câmera Ativa", "Nenhuma (usando principal)");
            }
            GUI.enabled = true;
        }

        EditorGUILayout.EndVertical();
    }
}
