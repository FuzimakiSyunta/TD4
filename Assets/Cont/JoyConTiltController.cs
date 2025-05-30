using UnityEngine;

public class JoyConTiltController : MonoBehaviour
{
    public bool isLeftJoyCon;  // ��Joy-Con�Ȃ�true�A�EJoy-Con�Ȃ�false
    private Joycon joycon;

    void Start()
    {
        // JoyconManager�̃��X�g���獶���E��Joy-Con���擾
        foreach (var jc in JoyconManager.Instance.j)
        {
            if (jc.isLeft == isLeftJoyCon)
            {
                joycon = jc;
                break;
            }
        }

        if (joycon == null)
        {
            Debug.LogError("�w�肵��Joy-Con��������܂���ł����B");
        }
    }

    void Update()
    {
        if (joycon == null) return;

        // Joy-Con�̊p�x�i�N�H�[�^�j�I���j���擾
        Quaternion targetRotation = Quaternion.Euler(-roll, 0, pitch);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // �I�u�W�F�N�g�̉�]�ɔ��f�i�K�v�ɉ����Ē����j
        transform.rotation = joyconRotation;
    }
}
