using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraConfig
{
    [Tooltip("Câmera que será ativada")]
    public Camera camera;

    [Tooltip("Box Collider que ativa esta câmera (deixe vazio para usar o collider principal)")]
    public BoxCollider boxCollider;

    [Tooltip("Nome/descrição desta configuração")]
    public string nome = "Camera";
}

public class CamSwitchPress : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Lista de configurações de câmeras secundárias")]
    [SerializeField] private List<CameraConfig> configuracoesCameras = new List<CameraConfig>();

    [Header("Trigger Settings")]
    [Tooltip("Tag do objeto que irá ativar a troca de câmera")]
    [SerializeField] private string tagAlvo = "Player";

    [Tooltip("Box Collider padrão (usado se nenhuma câmera tiver collider específico)")]
    [SerializeField] private BoxCollider boxColliderPrincipal;

    [Header("Status")]
    [SerializeField] private bool mostrarStatus = true;

    private Camera cameraPrincipal;
    private Camera cameraAtualmenteAtiva;
    private int indiceCameraAtiva = -1;

    private void Start()
    {
        cameraPrincipal = Camera.main;

        foreach (var config in configuracoesCameras)
        {
            if (config != null && config.camera != null)
            {
                config.camera.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagAlvo))
        {
            AtivarCameraSecundaria();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagAlvo))
        {
            VoltarParaCameraPrincipal();
        }
    }

    private void AtivarCameraSecundaria()
    {
        if (configuracoesCameras == null || configuracoesCameras.Count == 0)
        {
            Debug.LogWarning("Nenhuma câmera secundária configurada!");
            return;
        }

        if (indiceCameraAtiva < 0 || indiceCameraAtiva >= configuracoesCameras.Count)
        {
            Debug.LogWarning($"Índice {indiceCameraAtiva} inválido! Usando câmera 0.");
            indiceCameraAtiva = 0;
        }

        if (cameraPrincipal != null)
        {
            cameraPrincipal.enabled = false;
        }

        cameraAtualmenteAtiva = configuracoesCameras[indiceCameraAtiva].camera;
        if (cameraAtualmenteAtiva != null)
        {
            cameraAtualmenteAtiva.enabled = true;
        }
    }

    private void VoltarParaCameraPrincipal()
    {
        if (cameraAtualmenteAtiva != null)
        {
            cameraAtualmenteAtiva.enabled = false;
        }

        if (cameraPrincipal != null)
        {
            cameraPrincipal.enabled = true;
        }

        cameraAtualmenteAtiva = null;
        indiceCameraAtiva = -1;
    }

    public int GetTotalCameras()
    {
        return configuracoesCameras != null ? configuracoesCameras.Count : 0;
    }

    public Camera GetCameraPrincipal()
    {
        return cameraPrincipal;
    }

    public Camera GetCameraAtiva()
    {
        return cameraAtualmenteAtiva;
    }

    public int GetIndiceCameraAtiva()
    {
        return indiceCameraAtiva;
    }

    public List<CameraConfig> GetConfiguracoesCameras()
    {
        return configuracoesCameras;
    }
}

