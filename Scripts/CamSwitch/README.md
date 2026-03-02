# Camera Switch Press 📷

Sistema inteligente de troca de câmeras ativado por triggers para Unity, perfeito para criar pontos de vista cinematográficos, câmeras de segurança, ou qualquer situação que exija mudança dinâmica de perspectiva.

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Instalação](#-instalação)
- [Como Usar](#-como-usar)
- [Configurações](#-configurações)
- [Exemplos Práticos](#-exemplos-práticos)
- [API Pública](#-api-pública)
- [Dicas e Boas Práticas](#-dicas-e-boas-práticas)
- [Solução de Problemas](#-solução-de-problemas)

## 🎯 Visão Geral

O **CamSwitchPress** permite criar zonas de trigger que automaticamente trocam a câmera ativa quando o jogador (ou outro objeto) entra na área. Quando o objeto sai da área, a câmera principal é restaurada automaticamente.

### Características Principais:

- ✅ **Ativação Automática**: Troca de câmera ao entrar no trigger
- ✅ **Retorno Automático**: Volta para a câmera principal ao sair
- ✅ **Múltiplas Câmeras**: Suporte a várias configurações de câmera
- ✅ **Colliders Personalizados**: Use colliders diferentes para cada câmera
- ✅ **Fácil Configuração**: Interface intuitiva no Inspector
- ✅ **Validação Automática**: Sistema de avisos para configurações incorretas

## 📦 Instalação

### 1. Adicionar o Script

Adicione o componente `CamSwitchPress` a um GameObject na sua cena:

```csharp
// Via código
GameObject triggerZone = new GameObject("Camera Trigger Zone");
CamSwitchPress camSwitch = triggerZone.AddComponent<CamSwitchPress>();

// Ou adicione manualmente pelo Inspector
```

### 2. Configurar Collider

O GameObject precisa ter um **BoxCollider** marcado como **Trigger**:

```csharp
BoxCollider boxCollider = triggerZone.AddComponent<BoxCollider>();
boxCollider.isTrigger = true;
boxCollider.size = new Vector3(10, 5, 10); // Ajuste o tamanho da zona
```

### 3. Adicionar Câmeras Secundárias

Configure as câmeras que serão ativadas pelo trigger no Inspector ou por código.

## 🎮 Como Usar

### Configuração Básica no Inspector

1. **Adicione o Script** ao GameObject que terá a zona de trigger
2. **Configure o BoxCollider** e marque como "Is Trigger"
3. **Defina a Tag Alvo** (padrão: "Player")
4. **Adicione Configurações de Câmera**:
   - Clique em "+" na lista "Configuracoes Cameras"
   - Arraste a câmera secundária para o campo "Camera"
   - Opcionalmente, configure um collider específico
   - Dê um nome descritivo à configuração

### Exemplo Completo

```csharp
using UnityEngine;

public class SetupCameraSwitch : MonoBehaviour
{
    void Start()
    {
        // 1. Criar zona de trigger
        GameObject triggerZone = new GameObject("Security Camera Zone");
        triggerZone.transform.position = new Vector3(0, 0, 10);
        
        // 2. Adicionar e configurar BoxCollider
        BoxCollider boxCollider = triggerZone.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(10, 5, 10);
        
        // 3. Adicionar CamSwitchPress
        CamSwitchPress camSwitch = triggerZone.AddComponent<CamSwitchPress>();
        
        // 4. Criar e configurar câmera secundária
        GameObject securityCamObj = new GameObject("Security Camera");
        Camera securityCam = securityCamObj.AddComponent<Camera>();
        securityCam.transform.position = new Vector3(0, 10, 5);
        securityCam.transform.LookAt(triggerZone.transform);
        securityCam.enabled = false;
        
        // 5. Configurar no script
        var configs = camSwitch.GetConfiguracoesCameras();
        configs.Add(new CameraConfig 
        { 
            camera = securityCam,
            nome = "Câmera de Segurança"
        });
    }
}
```

## ⚙️ Configurações

### CameraConfig (Configuração de Câmera)

| Propriedade | Tipo | Descrição |
|------------|------|-----------|
| **camera** | Camera | Câmera que será ativada |
| **boxCollider** | BoxCollider | Collider específico para esta câmera (opcional) |
| **nome** | string | Nome/descrição da configuração |

### CamSwitchPress (Script Principal)

#### Camera Settings

| Propriedade | Tipo | Padrão | Descrição |
|------------|------|--------|-----------|
| **configuracoesCameras** | List\<CameraConfig\> | Empty | Lista de configurações de câmeras secundárias |

#### Trigger Settings

| Propriedade | Tipo | Padrão | Descrição |
|------------|------|--------|-----------|
| **tagAlvo** | string | "Player" | Tag do objeto que ativará a troca |
| **boxColliderPrincipal** | BoxCollider | null | Collider padrão (se nenhuma câmera tiver collider específico) |

#### Status

| Propriedade | Tipo | Padrão | Descrição |
|------------|------|--------|-----------|
| **mostrarStatus** | bool | true | Mostrar informações de status no Inspector |

## 💡 Exemplos Práticos

### 1. Câmera de Segurança

Perfeito para simular câmeras de vigilância em jogos de stealth:

```csharp
// Câmera que mostra a visão de uma câmera de segurança
GameObject securityZone = new GameObject("Security Zone");
BoxCollider col = securityZone.AddComponent<BoxCollider>();
col.isTrigger = true;
col.size = new Vector3(15, 5, 15);

CamSwitchPress switcher = securityZone.AddComponent<CamSwitchPress>();
// Configure a câmera de segurança no Inspector
```

### 2. Cutscene Trigger

Ative uma câmera cinematográfica quando o jogador chegar em um ponto específico:

```csharp
// Zona que ativa uma câmera cinematográfica
GameObject cutsceneZone = new GameObject("Cutscene Trigger");
cutsceneZone.transform.position = new Vector3(50, 0, 20);

BoxCollider trigger = cutsceneZone.AddComponent<BoxCollider>();
trigger.isTrigger = true;
trigger.size = new Vector3(5, 10, 5);

CamSwitchPress camSwitch = cutsceneZone.AddComponent<CamSwitchPress>();
// A câmera será ativada quando o player entrar na zona
```

### 3. Ponto de Observação

Permita que o jogador veja uma vista panorâmica ao entrar em uma área:

```csharp
// Mirante que mostra uma vista panorâmica
GameObject viewpoint = new GameObject("Scenic Viewpoint");
viewpoint.transform.position = new Vector3(100, 50, 100);

BoxCollider viewTrigger = viewpoint.AddComponent<BoxCollider>();
viewTrigger.isTrigger = true;
viewTrigger.size = new Vector3(8, 6, 8);

CamSwitchPress viewSwitch = viewpoint.AddComponent<CamSwitchPress>();

// Criar câmera panorâmica
GameObject panoramicCamObj = new GameObject("Panoramic Camera");
Camera panoramicCam = panoramicCamObj.AddComponent<Camera>();
panoramicCam.fieldOfView = 90;
panoramicCam.transform.position = new Vector3(100, 60, 100);
panoramicCam.enabled = false;

var configs = viewSwitch.GetConfiguracoesCameras();
configs.Add(new CameraConfig 
{ 
    camera = panoramicCam,
    nome = "Vista Panorâmica"
});
```

### 4. Múltiplas Zonas de Câmera

Configure várias zonas com câmeras diferentes:

```csharp
public class MultiCameraSetup : MonoBehaviour
{
    public Camera[] securityCameras;
    public Transform[] triggerPositions;
    
    void Start()
    {
        for (int i = 0; i < triggerPositions.Length; i++)
        {
            GameObject zone = new GameObject($"Camera Zone {i}");
            zone.transform.position = triggerPositions[i].position;
            
            BoxCollider col = zone.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(10, 5, 10);
            
            CamSwitchPress switcher = zone.AddComponent<CamSwitchPress>();
            var configs = switcher.GetConfiguracoesCameras();
            configs.Add(new CameraConfig 
            { 
                camera = securityCameras[i],
                nome = $"Câmera {i + 1}"
            });
        }
    }
}
```

## 📚 API Pública

### Métodos Públicos

#### GetTotalCameras()
Retorna o número total de câmeras configuradas.

```csharp
int totalCameras = camSwitch.GetTotalCameras();
Debug.Log($"Total de câmeras: {totalCameras}");
```

#### GetCameraPrincipal()
Retorna a referência para a câmera principal (Camera.main).

```csharp
Camera mainCam = camSwitch.GetCameraPrincipal();
if (mainCam != null)
{
    Debug.Log($"Câmera principal: {mainCam.name}");
}
```

#### GetCameraAtiva()
Retorna a câmera atualmente ativa (ou null se estiver usando a principal).

```csharp
Camera activeCam = camSwitch.GetCameraAtiva();
if (activeCam != null)
{
    Debug.Log($"Câmera ativa: {activeCam.name}");
}
else
{
    Debug.Log("Usando câmera principal");
}
```

#### GetIndiceCameraAtiva()
Retorna o índice da câmera atualmente ativa (-1 se estiver usando a principal).

```csharp
int index = camSwitch.GetIndiceCameraAtiva();
Debug.Log($"Índice da câmera ativa: {index}");
```

#### GetConfiguracoesCameras()
Retorna a lista de configurações de câmera.

```csharp
List<CameraConfig> configs = camSwitch.GetConfiguracoesCameras();
foreach (var config in configs)
{
    Debug.Log($"Câmera configurada: {config.nome}");
}
```

### Exemplo de Uso da API

```csharp
public class CameraMonitor : MonoBehaviour
{
    public CamSwitchPress camSwitch;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowCameraInfo();
        }
    }
    
    void ShowCameraInfo()
    {
        Debug.Log("=== Camera Info ===");
        Debug.Log($"Total de câmeras: {camSwitch.GetTotalCameras()}");
        
        Camera activeCam = camSwitch.GetCameraAtiva();
        if (activeCam != null)
        {
            Debug.Log($"Câmera ativa: {activeCam.name} (Índice: {camSwitch.GetIndiceCameraAtiva()})");
        }
        else
        {
            Camera mainCam = camSwitch.GetCameraPrincipal();
            Debug.Log($"Usando câmera principal: {mainCam?.name ?? "None"}");
        }
    }
}
```

## 🎯 Dicas e Boas Práticas

### 1. Tag do Jogador
Certifique-se de que seu objeto do jogador tem a tag correta:
```csharp
// No Inspector ou via código:
player.tag = "Player";
```

### 2. Collider como Trigger
Sempre marque o BoxCollider como Trigger:
```csharp
boxCollider.isTrigger = true;
```

### 3. Desabilitar Câmeras Secundárias
As câmeras secundárias devem começar desabilitadas (o script faz isso automaticamente no Start):
```csharp
secondaryCamera.enabled = false;
```

### 4. Posicionamento de Câmeras
Use `LookAt` para apontar câmeras para áreas específicas:
```csharp
securityCam.transform.LookAt(targetPosition);
```

### 5. Visualização no Editor
Use Gizmos para visualizar as zonas de trigger:
```csharp
void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
}
```

### 6. Múltiplas Câmeras
Para gerenciar várias câmeras, use nomes descritivos:
```csharp
new CameraConfig 
{ 
    camera = cam,
    nome = "Câmera do Corredor Principal"
};
```

## 🔧 Solução de Problemas

### Problema: A câmera não troca

**Solução 1**: Verifique a tag do objeto
```csharp
// Certifique-se de que o objeto tem a tag correta
Debug.Log($"Tag do objeto: {gameObject.tag}");
```

**Solução 2**: Verifique se o collider é trigger
```csharp
BoxCollider col = GetComponent<BoxCollider>();
Debug.Log($"Is Trigger: {col.isTrigger}");
```

**Solução 3**: Verifique se há câmeras configuradas
```csharp
if (camSwitch.GetTotalCameras() == 0)
{
    Debug.LogWarning("Nenhuma câmera configurada!");
}
```

### Problema: A câmera não volta para a principal

**Causa Comum**: O OnTriggerExit não está sendo chamado

**Solução**: Certifique-se de que:
- O Rigidbody está presente no objeto que entra (se necessário)
- O collider não está sendo desativado dentro do trigger
- O objeto não está sendo destruído dentro do trigger

### Problema: Múltiplos triggers interferindo

**Solução**: Use tags diferentes ou adicione lógica customizada:
```csharp
// Modifique o script para verificar camadas também
if (other.CompareTag(tagAlvo) && other.gameObject.layer == playerLayer)
{
    AtivarCameraSecundaria();
}
```

### Problema: Warnings sobre índice inválido

**Causa**: A lista de câmeras está vazia ou o índice está incorreto

**Solução**: Sempre verifique antes de usar:
```csharp
if (camSwitch.GetTotalCameras() > 0)
{
    // Seguro para usar
}
```

## 📝 Notas Técnicas

### Ordem de Execução
1. `Start()`: Inicializa câmera principal e desabilita câmeras secundárias
2. `OnTriggerEnter()`: Ativa câmera secundária quando objeto entra
3. `OnTriggerExit()`: Retorna para câmera principal quando objeto sai

### Dependências
- **UnityEngine**: Namespace padrão do Unity
- **Camera.main**: Requer uma câmera com tag "MainCamera" na cena

## 🤝 Contribuindo

Encontrou um bug ou tem uma sugestão? 

1. Abra uma [issue](https://github.com/VitorCEsar29/VitaoTools/issues)

---
**VitaoTools** - Simplificando o desenvolvimento em Unity 🚀

[← Voltar para README Principal](../../README.md)
