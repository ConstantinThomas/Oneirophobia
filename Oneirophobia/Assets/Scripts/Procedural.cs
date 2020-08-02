using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Procedural : MonoBehaviour
{
    [Header("Variables")]
    public int lvl;
    private int roomId;
    private int id;
    private int p;
    private int ennemyId;
    private int totalWeight;
    private bool isStarting;
    [Header("References")] 
    public Transform Camera;
    public Transform trP;
    public List<GameObject> room;
    public List<GameObject> prefabEnnemy;
    private GameObject CurrentRoom;
    private Vector3 NextRoomPos;
    
    void Start()
    {
        isStarting = true;
        RoomPlacer();
    }

    void Update()
    {
        //reset NextRoomPos quand le level change
        //quand le joueur passe une porte : appeler RoomPlacer et detruire la precedente
        
    }

    void ChangeRoom()
    {
        
    }

    void RoomPlacer()
    {
        if (lvl == 1)
        {
            if (isStarting)
            {
                Instantiate(room[0], Vector3.zero, Quaternion.identity);
                EnnemySpawner();
                isStarting = false;
            }
            else
            {
                roomId = Random.Range(0, 5);
                NextRoomPos = CurrentRoom.transform.position + NextPosition();
                CurrentRoom = Instantiate(room[roomId], NextRoomPos, Quaternion.identity);
                Camera.position = NextRoomPos;
                

                //random entre 0 et le nombre de salles du premier lvl
            }
        }
        else if (lvl == 2)
        {
            if (isStarting)
            {
                Instantiate(room[0], Vector3.zero, Quaternion.identity);
                isStarting = false;
            }
            else
            {
                roomId = Random.Range(5, 10);
                NextRoomPos = CurrentRoom.transform.position + NextPosition();
                Instantiate(room[roomId], NextRoomPos, Quaternion.identity);
                Camera.position = NextRoomPos;
            
                //random entre le nombre de salles du premier lvl et le nombre de salles du deuxieme lvl
            }
        }        
        else
        {
            if (isStarting)
            {
                Instantiate(room[0], Vector3.zero, Quaternion.identity);
                isStarting = false;
            }
            else
            {
                roomId = Random.Range(10, 15);
                NextRoomPos = CurrentRoom.transform.position + NextPosition();
                Instantiate(room[roomId], NextRoomPos, Quaternion.identity);
                Camera.position = NextRoomPos;
            
                //random entre le nombre de salles du deuxieme lvl et le nombre de salles du dernier lvl
            }
        }
    }

    void EnnemySpawner()
    {
        // ca boucle a l'infinie askip
        totalWeight = Random.Range(2, 9);
        for (int i = 0; i < 2; i++)
        {
            id = 0;
                //Random.Range(0, 1);
            p = prefabEnnemy[id].GetComponent<IaManager>().weight;
            Debug.Log("ca marche");
            Instantiate(prefabEnnemy[id], new Vector3(5, 0, 0), Quaternion.identity);
            totalWeight -= p - 1;
        }
    }

    Vector3 NextPosition()
    {
        Vector3 nextPosition = new Vector3(0,0,0);
        if ((int) trP.position.x > (int) Camera.position.x && (int) trP.position.y == (int) Camera.position.y)
        {
            nextPosition = new Vector3(18, 0);
            return nextPosition;
        }
        if ((int) trP.position.x < (int) Camera.position.x && (int) trP.position.y == (int) Camera.position.y)
        {
            nextPosition = new Vector3(-18, 0);
            return nextPosition;
        }
        if ((int) trP.position.x == (int) Camera.position.x && (int) trP.position.y > (int) Camera.position.y)
        {
            nextPosition = new Vector3(0, 10);
            return nextPosition;
        }
        if ((int) trP.position.x == (int) Camera.position.x && (int) trP.position.y < (int) Camera.position.y)
        {
            nextPosition = new Vector3(0, -10);
            return nextPosition;
        }
        return nextPosition;
    }
}
