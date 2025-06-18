using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Q�[���J�n���o�̂��߂̃X�N���v�g
public class GameStartScript : MonoBehaviour
{
    // ���s�t���[���ɂ��^�C�~���O�̃Y����}���邽�߁A
    // �f���^�^�C����g�p����
    private float deltaTime = 0.0f;

    ///
    /// �Q�[���J�n���̉��o��s�����߂̕ϐ�
    ///

    // �J�E���g�_�E����s�����ǂ���
    public bool isStartCountDown = false;

    // �J�E���g�_�E���̉�
    // �J�E���g�� * ���L�̃J�E���g�������Ԃ����J�E���g�_�E�����s����
    [Header("�J�E���g�_�E����")]
    public int countDownTime = 3;
    
    // 1�J�E���g�̎��Ԃ��` 
   // [Header("1�J�E���g������̕\������")]
    public float oneSecond = 1.0f;
    // ���݂̃J�E���g�̌o�ߎ��Ԃ��`
    private float currenOneSecond = 0.0f;

    // �J�E���g�_�E���p�̃e�L�X�g�EUI�z��
    [Header("�J�E���g�_�E���e�L�X�g")]
    public Text countDownText = null;

    //[Header("�J�E���g�_�E���p��UI�z��")]
    //public GameObject[] countDownUIArray = new GameObject[3];

    // �J�E���g�_�E�����I��������
    private bool endCountDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // �o�ߎ��Ԃ̏�����
        // �ϐ��ɍ��킹��(�O���Őݒ�\)
        currenOneSecond = oneSecond;


    }

    // Update is called once per frame
    void Update()
    {
        // �f���^�^�C����X�V
        deltaTime = Time.deltaTime;

        // �J�E���g�_�E�����L���ȏꍇ
        if (isStartCountDown)
        {
            // �J�E���g�_�E���̎��s
            UpdateCountDown();
        }

        if (endCountDown)
        {

            // �e�L�X�g�̊g�嗦��㏸
            countDownText.transform.localScale = new Vector3(
                countDownText.transform.localScale.x + (deltaTime * 24.0f),
                countDownText.transform.localScale.y + (deltaTime * 24.0f),
                countDownText.transform.localScale.z
                );

            // �e�L�X�g��ړ�
            countDownText.transform.Translate(new Vector3(0.0f, deltaTime * 600.0f, 0.0f));


            // �J�E���g�_�E�����I�������ꍇ�̏���
            // �R���[�`����g�p���A0.5�b��ɃJ�E���g�_�E���̃e�L�X�g���\���ɂ���
            StartCoroutine(EndCountDoun());
        }

    }

    // �J�E���g�_�E������
    void UpdateCountDown()
    {
        // �J�E���g���ő�l�Ɠ��l�̏ꍇ(�J�E���g�J�n���⌸����)
        // �J�E���gUI(�܂��̓��O)�̕\����s��
        if (currenOneSecond == oneSecond)
        {
            // �J�E���g�_�E���̃e�L�X�g��L����
            countDownText.gameObject.SetActive(true);
            // �e�L�X�g�̊g�嗦����Z�b�g
            countDownText.transform.localScale = Vector3.one;
            // �e�L�X�g�̉�]����Z�b�g
            countDownText.transform.rotation = Quaternion.identity;

            // (ToDo)UI�̕\����s���A�X�V��L���ɂ���
            // �J�E���g�_�E���̎c��b���̒l�ɍ��킹�ĕ\����X�V
            countDownText.text = countDownTime.ToString();

            // �f�o�b�O�p�̃��O�o��
            Debug.Log("�J�E���g�_�E��: " + countDownTime + "�b");
        }

        // �e�L�X�g�̊g�嗦�����
        countDownText.transform.localScale = new Vector3(
            countDownText.transform.localScale.x - (deltaTime * 1.0f),
            countDownText.transform.localScale.y - (deltaTime * 1.0f),
            countDownText.transform.localScale.z
            );
        // �e�L�X�g���]
        countDownText.transform.Rotate(new Vector3(0.0f, 0.0f, deltaTime * 360.0f));

        // �o�ߎ��Ԃ�X�V
        currenOneSecond -= deltaTime;

        // 1�b�o�߂�����J�E���g�_�E����i�߂�
        if (currenOneSecond > 0.0f)
        {
            // �o�߂��Ă��Ȃ��ꍇ�͑������^�[��
            return;
        }

        // �J�E���g�_�E�����Ԃ���炷
        countDownTime--;
        // ����1�b�̎��Ԃ�ݒ�
        currenOneSecond = oneSecond;


        // �J�E���g�_�E�����I�������ꍇ
        if (countDownTime <= 0)
        {
            // �J�E���g�_�E���̃e�L�X�g��X�V
            countDownText.text = "GO!";
            // �e�L�X�g�̊g�嗦����Z�b�g
            countDownText.transform.localScale = Vector3.one;
            // �e�L�X�g�̉�]����Z�b�g
            countDownText.transform.rotation = Quaternion.identity;

            // �f�o�b�O�p�̃��O�o��
            Debug.Log("�s���܂��傤");

            // �J�E���g�_�E���𖳌��ɂ���
            isStartCountDown = false;
            // �J�E���g�_�E�����I�������t���O�𗧂Ă�
            endCountDown = true;
            // �Q�[���J�n�̏�����s��
            //StartGame();
        }


    }

    IEnumerator EndCountDoun()
    {

        // �J�E���g�I����̌㏈��
        // 0.5�b��ɃJ�E���g�_�E���̃e�L�X�g���\���ɂ���
        yield return new WaitForSeconds(0.5f);
        // �J�E���g�_�E���̃e�L�X�g�𖳌���
        countDownText.gameObject.SetActive(false);


    }

}
