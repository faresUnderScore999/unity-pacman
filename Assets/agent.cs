using UnityEngine;
using UnityEngine.AI;

public class AgentChase : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform playerTransform;
    public float catchDistance = 2.5f; 

    private NavMeshAgent agent;
    private bool isGameOver = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isGameOver || playerTransform == null) return;

        // Se déplace vers le joueur
        agent.SetDestination(playerTransform.position);

        // Vérifie si l'agent attrape le joueur
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= catchDistance)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        isGameOver = true;

        // Arrête l'agent
        agent.isStopped = true;

        // Met à jour le texte instantanément via le Singleton
        if (ScoreScript.Instance != null)
        {
            ScoreScript.Instance.ShowGameOver();
        }

        // Libère le curseur et fige le temps du jeu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        Debug.Log("Game Over déclenché !");
    }
}