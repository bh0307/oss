using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    GameObject myTurnStartPanel;
    [SerializeField]
    GameObject myTurnMovePanel;
    [SerializeField]
    GameObject myTurnActionPanel;
    [SerializeField]
    GameObject othersTurnPanel;

    public static UiManager UM;
    public float LImitTime;
    public Text text_Timer;
    public GameObject notice_obj;
    public Text notice;
    PhotonView PV;

    void Awake()
    {
        UM=this;
        PV = GetComponent<PhotonView>();
    }
     void Update()
    {
        LImitTime -= Time.deltaTime;
        text_Timer.text = "���ѽð� : "  + (int)LImitTime;
        if(LImitTime <= 0)
        {
            GameManager.GM.RPC_NewTurnStart();
            LImitTime = 10f;
        }
    }

    public void MyTurnStart()
    {
        myTurnStartPanel.SetActive(true);
        myTurnMovePanel.SetActive(false);
        myTurnActionPanel.SetActive(false);
        othersTurnPanel.SetActive(false);
        notice_obj.SetActive(false);
    }

    public void MyTurnMove()
    {
        myTurnStartPanel.SetActive(false);
        myTurnMovePanel.SetActive(true);
        myTurnActionPanel.SetActive(false);
        othersTurnPanel.SetActive(false);
        GameManager.GM.myController.Bulddeok();
        notice_obj.SetActive(false);
    }

    public void MyTurnAction()
    {
        myTurnStartPanel.SetActive(false);
        myTurnMovePanel.SetActive(false);
        myTurnActionPanel.SetActive(true);
        othersTurnPanel.SetActive(false);
        notice_obj.SetActive(false);
    }

    public void OthersTurn()
    {
        myTurnStartPanel.SetActive(false);
        myTurnMovePanel.SetActive(false);
        myTurnActionPanel.SetActive(false);
        //othersTurnPanel.SetActive(true);
        notice_obj.SetActive(false);
        notice_obj.SetActive(true);
        SetNotice("�ٸ� �÷��̾��� �����Դϴ�.");
    }

    IEnumerator Notice()
    {
        notice_obj.SetActive(false);
        notice_obj.SetActive(true);
        yield return new WaitForSeconds(3f);
        notice_obj.SetActive(false);
    }
    
    public void SetNotice(string txt)
    {   
        notice_obj.SetActive(false);
        notice_obj.SetActive(true);
        notice.text = txt;
    }
}
