using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    //Local a se mover
    Transform goal;
    //Velocidade do Tank (Player)
    public float speed = 5.0f;
    //Distância minima entre o Player e o local alvo até que procure outro local
    public float accuracy = 1.0f;
    //Velocidade de Rotação
    public float rotSpeed = 2.0f;

    //GameObject do WPManager
    public GameObject wpManager;
    //Onde vão ser guardados os waypoints
    GameObject[] wps;
    //O "nó" atual no qual o Player está
    GameObject currentNode;
    //Valor em Int da posição do waypoint que o Player está
    int currentWP = 0;
    //Váriavel do Graph
    Graph g;

    void Start()
    {
        //Atribui os waypoints
        wps = wpManager.GetComponent<WPManager>().waypoints;
        //Atribui o Graph
        g = wpManager.GetComponent<WPManager>().graph;
        //Nó atual é igual a posição 0 do Array de Waypoints
        currentNode = wps[0];
    }

    //Ir até o Heliporto
    public void GoToHeli()
    {
        //Calcula o caminho do nó atual até o da posição 1 do Array
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }

    //Ir até as Ruínas
    public void GoToRuin()
    {
        //Calcula o caminho do nó atual até o da posição 6 do Array
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        //Se o tamanho da Lista dos nós for igual a zero não faça nada, pois não haverá para onde o Player se mover
        //Ou
        //Se o currentWP for igual ao tamanho máximo da Lista de nós não faça nada, pois estará fora do alcance da List
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;

        //Define nó atual
        currentNode = g.getPathPoint(currentWP);

        //Se a distância entre Player e o Ponto for menor que a accuracy então incremente o currentWP e faça o ir até o próximo ponto
        if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
        {
            currentWP++;
        }

        //Se o currentWP for menor que o tamanho da Lista
        if (currentWP < g.getPathLength())
        {
            //Seta a variável goal como a posição do Waypoint que o Player está indo
            goal = g.getPathPoint(currentWP).transform;
            //Vector3 para a posição do Destino
            Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
            //Vector3 para o vetor direção entre o Destino e o Player;
            Vector3 direction = lookAtGoal - this.transform.position;
            //Rotaciona o Player suavemente na direção do Vetor
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
        }

        //Move o Player
        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
