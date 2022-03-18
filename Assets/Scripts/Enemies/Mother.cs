using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mother : MonoBehaviour
{
    [SerializeField] private List<GameObject> minions;
    [SerializeField] private GameObject[] shot_trans;
    private bool[] minionsStatus = new bool[3];
    private bool allAlive = true;
    [SerializeField] Gate gate;


    // Start is called before the first frame update
    // Update is called once per frame
    public IEnumerator StartPhase() 
    {
        yield return new WaitForSeconds(4f);
        Decision();
        yield return new WaitForSeconds(5f);
        StartCoroutine(Battle());
    }
    public IEnumerator Battle() 
    {
        while (minions[0].activeInHierarchy && minions[1].activeInHierarchy && minions[2].activeInHierarchy)
        {
            Decision();
            yield return new WaitForSeconds(5f);
        }
        yield return null;
    }
    public void Decision() 
    {
        //cannon
        int placement = Random.Range(0, shot_trans.Length);
        print(placement);
        if (shot_trans[placement].activeSelf)
        {
            Decision();
        }
        else
        {
            shot_trans[placement].SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int count = 0;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            foreach (GameObject m in minions) 
            {
                m.SetActive(true);
                minionsStatus[count++] = true;
            }
        StartCoroutine(StartPhase());
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Update()
    {
        //If all dead
        if(!minions[0].activeInHierarchy && !minions[1].activeInHierarchy && !minions[2].activeInHierarchy)
        {
            gate.gameObject.SetActive(true);
            gate.Usable = true;
        }
        else 
        {
            gate.gameObject.SetActive(false);
            gate.Usable = false;
        }

    }

}
