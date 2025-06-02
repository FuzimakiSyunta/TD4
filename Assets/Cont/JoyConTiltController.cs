using UnityEngine;

public class JoyConTiltController : MonoBehaviour
{
    public bool isLeftJoyCon;  // ��Joy-Con�Ȃ�true�A�EJoy-Con�Ȃ�false
    private Joycon joycon;

    void Start()
    {
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

        // Joy-Con�̉����x�f�[�^���擾
        Vector3 accel = joycon.GetAccel();

        // �s�b�`�ƃ��[���̌v�Z�i�����x����Z�o�j
        float pitch = Mathf.Atan2(accel.y, accel.z) * Mathf.Rad2Deg;
        float roll = Mathf.Atan2(accel.x, accel.z) * Mathf.Rad2Deg;

        // �N�H�[�^�j�I����]�Ƃ��ĕϊ�
        Quaternion joyconRotation = Quaternion.Euler(-pitch, 0f, roll);

        // ��]�����炩�ɔ��f
        transform.rotation = Quaternion.Slerp(transform.rotation, joyconRotation, Time.deltaTime * 5f);
    }
}
