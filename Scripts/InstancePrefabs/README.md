# Instance Prefab Tool - Documentação

## Novas Funcionalidades Adicionadas

### Modos de Distribuição

O sistema agora suporta 5 modos diferentes de distribuição de prefabs:

#### 1. **Aleatório** (Padrão)
- Distribui os prefabs aleatoriamente dentro da área circular definida
- Ideal para vegetação, rochas e elementos naturais

#### 2. **Linha**
- Distribui os prefabs em uma linha reta
- Útil para cercas, postes, árvores em linha

#### 3. **Grade**
- Distribui os prefabs em um padrão de grade/matriz
- Perfeito para plantações organizadas, pisos, estruturas repetitivas

#### 4. **Círculo**
- Distribui os prefabs uniformemente ao redor de um círculo
- Ideal para criar formações circulares, decorações radiais

#### 5. **Espiral**
- Distribui os prefabs em um padrão de espiral crescente
- Cria padrões interessantes e orgânicos

### Configurações de Projeção

#### Projeção em Superfície (Padrão)
- `Usar Projeção Superfície = true`
- Os prefabs serão projetados na superfície detectada usando raycast
- Respeita a normal da superfície e o LayerMask configurado

#### Posicionamento Horizontal
- `Usar Projeção Superfície = false`
- Os prefabs serão posicionados em um plano horizontal fixo
- Define a altura através do campo `Altura Horizontal`
- Útil para instanciar objetos flutuantes, nuvens, partículas aéreas

## Como Usar

1. **Selecione o Modo de Distribuição** no Inspector
2. **Configure a Projeção**:
   - Marque "Usar Projeção Superfície" para seguir o terreno
   - Desmarque para posicionamento horizontal fixo
3. **Ajuste os Parâmetros**:
   - Raio da Área de Efeito
   - Número de Prefabs
   - Altura Horizontal (se projeção desabilitada)
4. **Ative o Modo de Edição** no Inspector
5. **Clique na Scene View** onde deseja instanciar

## Dicas

- Use **Linha** com projeção horizontal para criar fileiras de objetos no ar
- Use **Grade** com projeção em superfície para preencher terrenos organizadamente
- Use **Espiral** para criar padrões decorativos únicos
- Combine diferentes modos com rotação e escala aleatória para resultados variados

## Atalhos e Dicas Visuais

- O círculo verde mostra a área de efeito
- Pequenas cruzes mostram onde cada prefab será instanciado (preview)
- O texto na Scene View mostra: Modo atual, Tipo de Projeção, Número de Prefabs e Raio
