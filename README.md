# VitaoTools 🛠️

[![Unity](https://img.shields.io/badge/Unity-2020.3+-black.svg?style=flat&logo=unity)](https://unity.com/)

Coleção de ferramentas essenciais para Unity que aceleram o desenvolvimento e simplificam tarefas comuns. VitaoTools oferece controle avançado de Animators, instanciação inteligente de prefabs com múltiplos modos de distribuição e sistema flexível de troca de câmeras.

## 📋 Índice

- [Funcionalidades](#-funcionalidades)
- [Instalação](#-instalação)
- [Ferramentas Disponíveis](#-ferramentas-disponíveis)
  - [Animator Controller Tool](#1-animator-controller-tool)
  - [Instance Prefab Tool](#2-instance-prefab-tool)
  - [CamSwitch Tool](#3-camswitch-tool)
- [Como Usar](#-como-usar)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Contribuindo](#-contribuindo)
- [Licença](#-licença)
- [Autor](#-autor)

## ✨ Funcionalidades

### Animator Controller Tool
- ✅ Controle de múltiplos Animators simultaneamente
- ✅ Suporte a todos os tipos de parâmetros (Trigger, Bool, Int, Float)
- ✅ Sistema de Auto Reset com delay configurável
- ✅ Validação automática de parâmetros
- ✅ Operações em batch e indexadas

### Instance Prefab Tool
- ✅ 5 modos de distribuição: Aleatório, Linha, Grade, Círculo e Espiral
- ✅ Projeção em superfície com raycast
- ✅ Posicionamento horizontal fixo
- ✅ Configurações avançadas de rotação e escala
- ✅ Editor customizado para Unity

### CamSwitch Tool
- ✅ Troca de câmeras por input do jogador
- ✅ Suporte a múltiplas câmeras
- ✅ Configuração flexível de teclas de atalho
- ✅ Integração com Cinemachine

## 📦 Instalação

### Método 1: Download Direto
1. Faça o download do repositório
2. Copie as pastas `Scripts` e `Editor` para dentro da pasta `Assets` do seu projeto Unity

### Método 2: Git Submodule
```bash
cd Assets
git submodule add https://github.com/VitorCEsar29/VitaoTools
```

### Método 3: Unity Package Manager
```bash
# Via Package Manager -> Add package from git URL
https://github.com/VitorCEsar29/VitaoTools.git
```

## 🛠️ Ferramentas Disponíveis

### 1. Animator Controller Tool

Controle poderoso e centralizado de múltiplos Animators no Unity.

#### Principais Recursos:
- **Controle Indexado**: Aplica parâmetros específicos a cada Animator
- **Controle em Batch**: Aplica o mesmo parâmetro a todos os Animators
- **Auto Reset**: Reseta automaticamente parâmetros após um delay
- **Validação**: Verifica existência de parâmetros antes de aplicar

#### Exemplo de Uso:
```csharp
// Adicionar script ao GameObject
AnimatorTeste animatorController = gameObject.AddComponent<AnimatorTeste>();

// Disparar animação em todos os Animators indexados
animatorController.PlayAllIndexed();

// Disparar trigger em todos os Animators
animatorController.PlayAllTrigger("Jump");

// Ativar bool em todos
animatorController.PlayAllBool("IsRunning", true);
```

[📖 Documentação Completa](Scripts/Animator/README.md)

---

### 2. Instance Prefab Tool

Sistema inteligente de instanciação de prefabs com múltiplos padrões de distribuição.

#### Modos de Distribuição:

| Modo | Descrição | Uso Ideal |
|------|-----------|-----------|
| **Aleatório** | Distribuição randômica na área circular | Vegetação, rochas, elementos naturais |
| **Linha** | Distribuição linear | Cercas, postes, árvores alinhadas |
| **Grade** | Padrão de matriz organizada | Plantações, pisos, estruturas repetitivas |
| **Círculo** | Distribuição circular uniforme | Formações circulares, decorações radiais |
| **Espiral** | Padrão em espiral crescente | Efeitos visuais, padrões orgânicos |

#### Recursos de Projeção:
- **Projeção em Superfície**: Adapta à topografia do terreno
- **Posicionamento Horizontal**: Altura fixa para objetos flutuantes

#### Exemplo de Uso:
```csharp
instancePrefab spawner = gameObject.AddComponent<instancePrefab>();
spawner.prefabsParaInstanciar = new GameObject[] { treePrefab };
spawner.modoDistribuicao = ModoDistribuicao.Grade;
```

[📖 Documentação Completa](Scripts/InstancePrefabs/README.md)

---

### 3. CamSwitch Tool

Sistema flexível para alternância entre múltiplas câmeras através de inputs do jogador.

#### Principais Recursos:
- **Múltiplas Câmeras**: Gerencia qualquer quantidade de câmeras na cena
- **Inputs Customizáveis**: Configure teclas de atalho personalizadas
- **Integração com Cinemachine**: Suporte nativo para Virtual Cameras
- **Editor Customizado**: Interface intuitiva no Inspector do Unity

#### Exemplo de Uso:
```csharp
// Adicionar script ao GameObject
CamSwitchPress camSwitch = gameObject.AddComponent<CamSwitchPress>();

// As câmeras são configuradas no Inspector
// Pressione as teclas configuradas para alternar entre câmeras
```

[📖 Documentação Completa](Scripts/CamSwitch/README.md)

---

## 💡 Como Usar

### Passo 1: Adicionar o Script
Adicione o script desejado a um GameObject na sua cena:
- `AnimatorTeste.cs` para controle de Animators
- `instancePrefab.cs` para instanciação de prefabs
- `CamSwitchPress.cs` para sistema de troca de câmeras

### Passo 2: Configurar no Inspector
Configure os parâmetros diretamente no Inspector do Unity com tooltips informativos.

### Passo 3: Executar
Execute via código ou utilize os editores customizados disponíveis.

## 📁 Estrutura do Projeto

```
VitaoTools/
├── Scripts/
│   ├── Animator/
│   │   ├── AnimatorTeste.cs          # Script principal do Animator Controller
│   │   └── README.md                 # Documentação detalhada
│   ├── InstancePrefabs/
│   │   ├── instancePrefab.cs         # Script principal de instanciação
│   │   └── README.md                 # Documentação detalhada
│   └── CamSwitch/
│       ├── CamSwitchPress.cs         # Script de troca de câmeras
│       └── README.md                 # Documentação detalhada
├── Editor/
│   ├── AnimatorEditor/
│   │   └── AnimatorEditor.cs         # Editor customizado para Animator
│   ├── EditorInstance/
│   │   └── instancePrefabEditor.cs   # Editor customizado para Instance
│   └── EditorCamSwitch/
│       └── CamSwitchPressEditor.cs   # Editor customizado para CamSwitch
└── README.md                          # Este arquivo
```

## 👤 Autor

**Vitor César**

- GitHub: [@VitorCEsar29](https://github.com/VitorCEsar29)
- Repository: [VitaoTools](https://github.com/VitorCEsar29/VitaoTools)

---

⭐ Se este projeto te ajudou, considere dar uma estrela no repositório!

💬 Encontrou algum bug ou tem sugestões? Abra uma [issue](https://github.com/VitorCEsar29/VitaoTools/issues)!
