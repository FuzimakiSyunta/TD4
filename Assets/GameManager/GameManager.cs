using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameStart = false; // �Q�[���J�n�t���O

    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���J�n�t���O��������
        isGameStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        isGameStart = true;
        // �Q�[���J�n���̏����������ɒǉ�
        Debug.Log("�Q�[�����J�n����܂����I");
    }

    public bool IsGameStarted()
    {
        return isGameStart;
    }
}
