using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Faz aparecer da estrutura no Inspector da Unity
[Serializable]

public struct Link
{
    //Enum para as direções do Player
    //UNI = Direção Única
    //BI = Ida e Volta
    public enum direction {UNI, BI}
    //Nó 1
    public GameObject node1;
    //Nó 2
    public GameObject node2;
    //Direção a se atribuir
    public direction dir;
}

public class WPManager : MonoBehaviour
{
    //Array de Waypoints
    public GameObject[] waypoints;
    //Lista de todos os links
    public Link[] links;
    //Variável do Graph
    public Graph graph = new Graph();

    void Start()
    {
        //Se houver mais de um waypoint na lista
        if(waypoints.Length > 0)
        {
            //Para cada wo no waypoints
            foreach(GameObject wp in waypoints)
            {
                //Cria um nó
                graph.AddNode(wp);
            }
            //Para cada link no links
            foreach(Link l in links)
            {
                //Cria o vértice de ida
                graph.AddEdge(l.node1, l.node2);
                if (l.dir == Link.direction.BI)
                    //Se for BI cria de ida e volta
                    graph.AddEdge(l.node2, l.node1);
            }
        }
    }

    void Update()
    {
        //Desenha na tela do jogo
        graph.debugDraw(); 
    }
}
