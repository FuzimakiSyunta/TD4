using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMater : MonoBehaviour
{
    private PlayerMove playerMoveScript;
    public GameObject playerMove;

    private GameManager gameManager;
    public GameObject gamemanagerScript;

    public GameObject speedMaterImage;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMoveScript = GetComponent<PlayerMove>();
        gameManager = GetComponent<GameManager>();
        //�X�s�[�h���[�^�[UI������
        speedMaterImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.IsGameStarted())
        {
            speedMaterImage.SetActive(true);//�X�s�[�h���[�^�[�\��
            
        }else 
        {
            speedMaterImage.SetActive(false);//�X�s�[�h���[�^�[��\��
        }
    }
}
