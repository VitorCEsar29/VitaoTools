# Animator Controller Tool - Documentação

## Descrição

O **AnimatorTeste** é uma ferramenta poderosa para controlar múltiplos Animators simultaneamente no Unity. Ele permite disparar triggers, alterar bools, integers e floats em diversos Animators de forma centralizada, com suporte a auto reset automático de parâmetros.

## Funcionalidades Principais

### 1. **Controle de Múltiplos Animators**
- Gerencia uma lista de Animators que podem ser controlados em conjunto
- Adiciona e remove Animators dinamicamente em tempo de execução
- Suporte para operações em batch (todos de uma vez)

### 2. **Tipos de Parâmetros Suportados**
- **Trigger**: Dispara um trigger no Animator
- **BoolTrue**: Define um bool como `true`
- **BoolFalse**: Define um bool como `false`
- **Int**: Define um valor inteiro
- **Float**: Define um valor float

### 3. **Sistema de Auto Reset**
- Reseta automaticamente os parâmetros após um delay configurável
- Opção para resetar bools para `false`
- Opção para resetar números (int/float) para `0`
- Delay configurável de 0.1 a 10 segundos
- Pode ser desabilitado se não for necessário

### 4. **Validação de Parâmetros**
- Verifica se o parâmetro existe no Animator antes de aplicar
- Logs de warning quando parâmetros não são encontrados
- Remove automaticamente Animators nulos da lista

## Métodos Públicos

### Métodos de Controle Indexado

#### `PlayAllIndexed()`
Aplica os parâmetros configurados no Inspector a cada Animator na lista, usando o mesmo índice.

**Exemplo de uso:**
- Animator[0] receberá Parameter[0]
- Animator[1] receberá Parameter[1]
- Se houver mais Animators que Parameters, usa o último Parameter

```csharp
animatorController.PlayAllIndexed();
```

### Métodos de Controle em Batch

#### `PlayTriggerAll(string triggerName)`
Dispara um trigger específico em todos os Animators da lista.

```csharp
animatorController.PlayTriggerAll("Attack");
```

#### `SetBoolAll(string boolName, bool value)`
Define um parâmetro bool em todos os Animators da lista.

```csharp
animatorController.SetBoolAll("IsRunning", true);
```

#### `SetIntegerAll(string intName, int value)`
Define um parâmetro integer em todos os Animators da lista.

```csharp
animatorController.SetIntegerAll("Speed", 5);
```

#### `SetFloatAll(string floatName, float value)`
Define um parâmetro float em todos os Animators da lista.

```csharp
animatorController.SetFloatAll("BlendSpeed", 0.75f);
```

### Métodos Dinâmicos

#### `PlayDynamicTriggers(IList<string> triggerList)`
Aplica triggers diferentes para cada Animator. Útil quando cada Animator precisa de um trigger específico.

```csharp
List<string> triggers = new List<string> { "Jump", "Attack", "Defend" };
animatorController.PlayDynamicTriggers(triggers);
```

#### `PlayDynamicParameters(IList<AnimationParameter> paramList)`
Aplica parâmetros complexos diferentes para cada Animator.

```csharp
List<AnimationParameter> params = new List<AnimationParameter>
{
    new AnimationParameter { name = "Speed", type = ParameterType.Int, intValue = 5 },
    new AnimationParameter { name = "Jump", type = ParameterType.Trigger }
};
animatorController.PlayDynamicParameters(params);
```

### Métodos de Gerenciamento

#### `AddAnimator(Animator animator)`
Adiciona um Animator à lista de controle.

```csharp
animatorController.AddAnimator(myAnimator);
```

#### `RemoveAnimator(Animator animator)`
Remove um Animator da lista de controle.

```csharp
animatorController.RemoveAnimator(myAnimator);
```

#### `ClearAllAnimators()`
Remove todos os Animators da lista.

```csharp
animatorController.ClearAllAnimators();
```

### Métodos de Reset

#### `ResetAllTriggers()`
Reseta manualmente todos os triggers e parâmetros configurados (de acordo com as opções de reset).

```csharp
animatorController.ResetAllTriggers();
```

#### `ForceAutoResetNow()`
Cancela o auto reset agendado e executa o reset imediatamente.

```csharp
animatorController.ForceAutoResetNow();
```

## Configuração no Inspector

### Configurações de Animators
- **Animators**: Lista de Animators a serem controlados

### Configurações de Parâmetros
- **Parameters**: Lista de parâmetros indexados (usado pelo método `PlayAllIndexed`)
  - **Name**: Nome do parâmetro no Animator
  - **Type**: Tipo do parâmetro (Trigger, BoolTrue, BoolFalse, Int, Float)
  - **Int Value**: Valor inteiro (quando Type = Int)
  - **Float Value**: Valor float (quando Type = Float)

### Configurações de Debug
- **Log Debug**: Ativa logs detalhados no console

### Configurações de Auto Reset
- **Auto Reset Enabled**: Ativa o sistema de auto reset
- **Auto Reset Delay**: Tempo em segundos antes do reset (0.1 a 10s)
- **Reset Bools To False**: Reseta bools para false
- **Reset Numbers To Zero**: Reseta ints e floats para 0

### Cores do Inspetor
- **Cor Inspetor**: Personaliza a cor do script no inspetor (requer custom editor)

## Casos de Uso

### 1. Sistema de Múltiplos Personagens
Controle as animações de vários NPCs simultaneamente quando um evento acontece:
```csharp
public class EventManager : MonoBehaviour
{
    public AnimatorTeste npcAnimators;
    
    void OnBossDefeated()
    {
        npcAnimators.PlayTriggerAll("Celebrate");
    }
}
```

### 2. Efeitos Visuais Sincronizados
Ative múltiplos efeitos VFX controlados por Animators:
```csharp
public class VFXManager : MonoBehaviour
{
    public AnimatorTeste vfxAnimators;
    
    void PlayExplosionEffect()
    {
        vfxAnimators.SetBoolAll("IsActive", true);
        vfxAnimators.PlayTriggerAll("Explode");
    }
}
```

### 3. UI Animada em Conjunto
Anime múltiplos elementos de UI ao mesmo tempo:
```csharp
public class UIAnimationController : MonoBehaviour
{
    public AnimatorTeste uiAnimators;
    
    void ShowMenu()
    {
        uiAnimators.PlayTriggerAll("FadeIn");
    }
    
    void HideMenu()
    {
        uiAnimators.PlayTriggerAll("FadeOut");
    }
}
```

### 4. Sistema de Combo com Reset
Execute ações em sequência com reset automático:
```csharp
public class ComboSystem : MonoBehaviour
{
    public AnimatorTeste characterAnimators;
    
    void PerformCombo()
    {
        // Auto reset irá limpar os triggers após 2 segundos
        characterAnimators.PlayTriggerAll("Combo1");
        Invoke(nameof(NextCombo), 0.5f);
    }
    
    void NextCombo()
    {
        characterAnimators.PlayTriggerAll("Combo2");
    }
}
```

## Dicas e Boas Práticas

### ✅ Recomendado
- Use **Auto Reset** para animações temporárias como ataques ou pulos
- Ative **Log Debug** durante desenvolvimento para acompanhar as operações
- Use `PlayAllIndexed()` quando cada Animator precisa de parâmetros específicos
- Use `PlayTriggerAll()` quando todos devem receber o mesmo comando
- Configure **Reset Bools To False** para evitar estados inconsistentes

### ⚠️ Atenção
- Certifique-se de que os nomes dos parâmetros correspondem aos do Animator Controller
- Animators nulos são automaticamente filtrados, mas geram warnings
- O Auto Reset afeta TODOS os parâmetros na lista, não apenas os disparados
- Múltiplas chamadas antes do Auto Reset completar irão reiniciar o timer

### ❌ Evite
- Adicionar o mesmo Animator múltiplas vezes (verificação automática previne isso)
- Usar delays muito curtos no Auto Reset (mínimo 0.1s)
- Desabilitar o GameObject antes do Auto Reset completar (coroutine será interrompida)

## Validações Automáticas

O sistema inclui validações que:
- Verificam se o parâmetro existe no Animator antes de aplicar
- Removem Animators nulos automaticamente
- Garantem que Auto Reset Delay não seja menor que 0.1s
- Exibem warnings quando operações falham
- Contam e reportam quantos Animators foram afetados

## Integração com Unity Events

O AnimatorTeste pode ser facilmente integrado com Unity Events:

1. No Inspector, adicione um **Button** ou **Event Trigger**
2. Configure o evento para chamar métodos públicos do AnimatorTeste
3. Exemplo: Button.OnClick() → AnimatorTeste.PlayTriggerAll("ButtonClick")

## Performance

- ✅ Operações otimizadas para múltiplos Animators
- ✅ Coroutine única para Auto Reset (não cria múltiplas)
- ✅ Validações feitas apenas quando necessário
- ✅ Logs podem ser desabilitados em produção

---

**Desenvolvido Por Vitor para VitaoTools** 🎮
