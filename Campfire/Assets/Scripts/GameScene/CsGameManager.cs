using UnityEngine;
using System.Collections;

public class CsGameManager : MonoBehaviour
{
    public GameObject cube;  // 사용할 큐브 오브젝트를 저장하는 변수

    // 게임 시작 시 한 번 호출되는 함수 (초기화)
    void Start()
    {
        // 현재는 아무런 초기화 코드가 없음
    }

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        // 마우스 왼쪽 버튼을 클릭하면
        if (Input.GetMouseButtonDown(0))
        {
            // 큐브 오브젝트를 비활성화 (숨김)
            cube.SetActive(false);
        }

        // 마우스 오른쪽 버튼을 클릭하면
        if (Input.GetMouseButtonDown(1))
        {
            // 큐브 오브젝트를 활성화 (보이게 함)
            cube.SetActive(true);
        }
    }
}
