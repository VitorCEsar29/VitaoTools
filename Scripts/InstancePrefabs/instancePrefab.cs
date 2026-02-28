using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ModoDistribuicao
{
    Aleatorio,
    Linha,
    Grade,
    Circulo,
    Espiral
}

public class instancePrefab : MonoBehaviour
{
    [Header("Configurações de Prefab")]
    [Tooltip("Prefabs a serem instanciados")]
    public GameObject[] prefabsParaInstanciar;

    [Header("Modo de Distribuição")]
    [Tooltip("Modo de distribuição dos prefabs")]
    public ModoDistribuicao modoDistribuicao = ModoDistribuicao.Aleatorio;

    [Header("Configurações de Área")]
    [Tooltip("Raio do círculo de área de efeito")]
    [Range(0.5f, 100f)]
    public float raioAreaEfeito = 5f;

    [Tooltip("Número de prefabs a serem instanciados")]
    [Range(1, 500)]
    public int numeroDePrefabs = 10;

    [Header("Configurações de Projeção")]
    [Tooltip("Usar projeção em superfície (raycast) ou posicionamento horizontal plano")]
    public bool usarProjecaoSuperficie = true;

    [Tooltip("Quando desabilitado a projeção, usar este plano de altura")]
    public float alturaHorizontal = 0f;

    [Header("Configurações de Alinhamento")]
    [Tooltip("Alinhar os prefabs com a normal da superfície")]
    public bool alinharComNormal = true;
    
    [Tooltip("Offset vertical dos prefabs")]
    public float offsetVertical = 0f;
    
    [Header("Configurações de Rotação")]
    [Tooltip("Rotação aleatória no eixo Y")]
    public bool rotacaoAleatoriaY = true;
    
    [Tooltip("Escala aleatória")]
    public bool escalaAleatoria = false;
    
    [Range(0.5f, 2f)]
    public float escalaMinima = 0.8f;
    
    [Range(0.5f, 2f)]
    public float escalaMaxima = 1.2f;
    
    [Header("Configurações de Layer")]
    [Tooltip("Layer do chão para detectar")]
    public LayerMask layerMask = -1;
    
    [Header("Visualização")]
    [Tooltip("Cor do círculo de área de efeito")]
    public Color corCirculo = new Color(0f, 1f, 0f, 0.3f);

    [Header("Cor do Inspetor")]
    [Tooltip("Cor do Script no Inspetor")]
    public Color corInspetor = Color.white;

    [Header("Cor do Button")]
    [Tooltip("Cor do Botão ativo no Inspetor")]
    public Color corBotao;
    public Color buttonDisable;

    // Método chamado pelo editor para instanciar os prefabs
    public void InstanciarPrefabs(Vector3 pontoClique, Vector3 normalSuperficie)
    {
        if (prefabsParaInstanciar == null || prefabsParaInstanciar.Length == 0)
        {
            Debug.LogWarning("Nenhum prefab atribuído para instanciar!");
            return;
        }

        // Criar um grupo para organizar os prefabs instanciados
        GameObject grupo = new GameObject("GrupoPrefabs_" + System.DateTime.Now.ToString("HHmmss"));
        #if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(grupo, "Criar grupo de prefabs");
        #endif

        // Gerar posições baseado no modo de distribuição
        Vector3[] posicoes = GerarPosicoes(pontoClique, normalSuperficie);

        // Instanciar os prefabs nas posições geradas
        foreach (Vector3 posicao in posicoes)
        {
            InstanciarPrefabNaPosicao(posicao, normalSuperficie, grupo.transform);
        }
    }

    private Vector3[] GerarPosicoes(Vector3 pontoClique, Vector3 normalSuperficie)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];
        Quaternion orientacaoCirculo = usarProjecaoSuperficie ? 
            Quaternion.FromToRotation(Vector3.up, normalSuperficie) : Quaternion.identity;

        switch (modoDistribuicao)
        {
            case ModoDistribuicao.Aleatorio:
                posicoes = GerarPosicoesAleatorias(pontoClique, orientacaoCirculo);
                break;

            case ModoDistribuicao.Linha:
                posicoes = GerarPosicoesLinha(pontoClique, orientacaoCirculo);
                break;

            case ModoDistribuicao.Grade:
                posicoes = GerarPosicoesGrade(pontoClique, orientacaoCirculo);
                break;

            case ModoDistribuicao.Circulo:
                posicoes = GerarPosicoesCirculo(pontoClique, orientacaoCirculo);
                break;

            case ModoDistribuicao.Espiral:
                posicoes = GerarPosicoesEspiral(pontoClique, orientacaoCirculo);
                break;
        }

        return posicoes;
    }

    private Vector3[] GerarPosicoesAleatorias(Vector3 centro, Quaternion orientacao)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];

        for (int i = 0; i < numeroDePrefabs; i++)
        {
            Vector2 pontoAleatorio2D = Random.insideUnitCircle * raioAreaEfeito;
            Vector3 offsetLocal = new Vector3(pontoAleatorio2D.x, 0f, pontoAleatorio2D.y);
            posicoes[i] = centro + orientacao * offsetLocal;
        }

        return posicoes;
    }

    private Vector3[] GerarPosicoesLinha(Vector3 centro, Quaternion orientacao)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];
        float espacamento = (raioAreaEfeito * 2f) / Mathf.Max(numeroDePrefabs - 1, 1);

        for (int i = 0; i < numeroDePrefabs; i++)
        {
            float offset = -raioAreaEfeito + (i * espacamento);
            Vector3 offsetLocal = new Vector3(offset, 0f, 0f);
            posicoes[i] = centro + orientacao * offsetLocal;
        }

        return posicoes;
    }

    private Vector3[] GerarPosicoesGrade(Vector3 centro, Quaternion orientacao)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];
        int lado = Mathf.CeilToInt(Mathf.Sqrt(numeroDePrefabs));
        float espacamento = (raioAreaEfeito * 2f) / Mathf.Max(lado - 1, 1);

        int index = 0;
        for (int x = 0; x < lado && index < numeroDePrefabs; x++)
        {
            for (int z = 0; z < lado && index < numeroDePrefabs; z++)
            {
                float offsetX = -raioAreaEfeito + (x * espacamento);
                float offsetZ = -raioAreaEfeito + (z * espacamento);
                Vector3 offsetLocal = new Vector3(offsetX, 0f, offsetZ);
                posicoes[index] = centro + orientacao * offsetLocal;
                index++;
            }
        }

        return posicoes;
    }

    private Vector3[] GerarPosicoesCirculo(Vector3 centro, Quaternion orientacao)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];
        float anguloIncremento = 360f / numeroDePrefabs;

        for (int i = 0; i < numeroDePrefabs; i++)
        {
            float angulo = i * anguloIncremento * Mathf.Deg2Rad;
            Vector3 offsetLocal = new Vector3(
                Mathf.Cos(angulo) * raioAreaEfeito,
                0f,
                Mathf.Sin(angulo) * raioAreaEfeito
            );
            posicoes[i] = centro + orientacao * offsetLocal;
        }

        return posicoes;
    }

    private Vector3[] GerarPosicoesEspiral(Vector3 centro, Quaternion orientacao)
    {
        Vector3[] posicoes = new Vector3[numeroDePrefabs];

        for (int i = 0; i < numeroDePrefabs; i++)
        {
            float t = (float)i / numeroDePrefabs;
            float angulo = t * Mathf.PI * 4f; // 2 voltas
            float raio = t * raioAreaEfeito;

            Vector3 offsetLocal = new Vector3(
                Mathf.Cos(angulo) * raio,
                0f,
                Mathf.Sin(angulo) * raio
            );
            posicoes[i] = centro + orientacao * offsetLocal;
        }

        return posicoes;
    }

    private void InstanciarPrefabNaPosicao(Vector3 posicao, Vector3 normalSuperficie, Transform parent)
    {
        Vector3 posicaoFinal;
        Vector3 normalFinal;

        if (usarProjecaoSuperficie)
        {
            // Raycast para baixo para encontrar o chão
            Vector3 origemRay = posicao + normalSuperficie * 10f;
            RaycastHit hit;

            if (Physics.Raycast(origemRay, -normalSuperficie, out hit, 20f, layerMask))
            {
                posicaoFinal = hit.point + hit.normal * offsetVertical;
                normalFinal = hit.normal;
            }
            else
            {
                return; // Não encontrou superfície, não instancia
            }
        }
        else
        {
            // Modo horizontal - usar altura fixa
            posicaoFinal = new Vector3(posicao.x, alturaHorizontal + offsetVertical, posicao.z);
            normalFinal = Vector3.up;
        }

        // Calcular rotação baseada na normal
        Quaternion rotacao = Quaternion.identity;

        if (alinharComNormal)
        {
            rotacao = Quaternion.FromToRotation(Vector3.up, normalFinal);
        }

        // Adicionar rotação aleatória no eixo Y
        if (rotacaoAleatoriaY)
        {
            float anguloAleatorio = Random.Range(0f, 360f);
            rotacao *= Quaternion.Euler(0f, anguloAleatorio, 0f);
        }

        // Escolher um prefab aleatório
        GameObject prefabEscolhido = prefabsParaInstanciar[Random.Range(0, prefabsParaInstanciar.Length)];

        // Instanciar o prefab
        GameObject instancia = Instantiate(prefabEscolhido, posicaoFinal, rotacao, parent);

        // Aplicar escala aleatória se habilitado
        if (escalaAleatoria)
        {
            float escala = Random.Range(escalaMinima, escalaMaxima);
            instancia.transform.localScale = Vector3.one * escala;
        }

        // Registrar no sistema de Undo
        #if UNITY_EDITOR
        Undo.RegisterCreatedObjectUndo(instancia, "Instanciar prefab");
        #endif
    }
    
    // Método para desenhar o círculo de área de efeito no editor
    public void DesenharCirculoAreaEfeito(Vector3 centro, Vector3 normal)
    {
        Quaternion orientacao = usarProjecaoSuperficie ? 
            Quaternion.FromToRotation(Vector3.up, normal) : Quaternion.identity;

        // Desenhar preview baseado no modo de distribuição
        Vector3[] posicoes = GerarPosicoes(centro, normal);

        // Desenhar círculo de área
        int segmentos = 64;
        Vector3 pontoAnterior = Vector3.zero;

        for (int i = 0; i <= segmentos; i++)
        {
            float angulo = (i / (float)segmentos) * Mathf.PI * 2f;
            Vector3 pontoLocal = new Vector3(Mathf.Cos(angulo) * raioAreaEfeito, 0f, Mathf.Sin(angulo) * raioAreaEfeito);
            Vector3 pontoMundo = centro + orientacao * pontoLocal;

            if (i > 0)
            {
                Debug.DrawLine(pontoAnterior, pontoMundo, corCirculo, 0.1f);
            }

            pontoAnterior = pontoMundo;
        }

        // Desenhar linhas cruzadas
        Vector3 direita = orientacao * Vector3.right * raioAreaEfeito;
        Vector3 frente = orientacao * Vector3.forward * raioAreaEfeito;

        Debug.DrawLine(centro - direita, centro + direita, corCirculo, 0.1f);
        Debug.DrawLine(centro - frente, centro + frente, corCirculo, 0.1f);

        // Desenhar preview das posições
        Color corPreview = new Color(corCirculo.r, corCirculo.g, corCirculo.b, 1f);
        foreach (Vector3 pos in posicoes)
        {
            DesenharCruz(pos, orientacao, 0.3f, corPreview);
        }
    }

    private void DesenharCruz(Vector3 centro, Quaternion orientacao, float tamanho, Color cor)
    {
        Vector3 direita = orientacao * Vector3.right * tamanho;
        Vector3 frente = orientacao * Vector3.forward * tamanho;
        Vector3 cima = orientacao * Vector3.up * tamanho;

        Debug.DrawLine(centro - direita, centro + direita, cor, 0.1f);
        Debug.DrawLine(centro - frente, centro + frente, cor, 0.1f);
        Debug.DrawLine(centro, centro + cima, cor, 0.1f);
    }
}
