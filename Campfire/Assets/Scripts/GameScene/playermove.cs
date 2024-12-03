using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    // 방향에 대한 벡터 배열 (우, 좌, 상, 하 이동을 위한 벡터들)
    Vector3[] arr = {
        new Vector3(1.1f, 0, 0),  // 오른쪽 이동
        new Vector3(-1.1f, 0, 0), // 왼쪽 이동
        new Vector3(0, 0, 1.1f),  // 위쪽 이동
        new Vector3(0, 0, -1.1f)  // 아래쪽 이동
    };

    Animator animator;  // 캐릭터 애니메이션 제어
    public bool LeftMove = false;  // 왼쪽 이동 상태
    public bool RightMove = false; // 오른쪽 이동 상태
    public bool UpMove = false;    // 위쪽 이동 상태
    public bool DownMove = false;  // 아래쪽 이동 상태
    public bool isMoving = false;  // 이동 중 상태 여부
    Vector3 moveVelocity = Vector3.zero;  // 이동 속도 (현재는 사용되지 않음)
    float moveSpeed = 0.5f;  // 이동 속도
    public Vector3 countMove;  // 이동된 거리 추적
    public Vector3 target_x;  // 목표 x 위치
    public Vector3 target_z;  // 목표 z 위치

    public GameObject planePrefab;  // 평면 객체의 프리팹
    public GameObject plane;  // 생성된 평면 객체

    // 게임 시작 시 호출되는 함수
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();  // Animator 컴포넌트 가져오기
        countMove = new Vector3(0f, 0f, 0f);  // 이동 거리 초기화
        target_x = transform.position;  // 목표 x 좌표 초기화
        target_z = transform.position;  // 목표 z 좌표 초기화

        plane = Instantiate(planePrefab, transform);  // 평면 객체 생성
        plane.transform.position += new Vector3(-3.5f, 1f, 0);  // 평면의 위치 설정
        plane.SetActive(false);  // 평면 객체 비활성화
    }

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        if(isMoving)  // 이동 중이라면
        {
            // 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, (target_x + target_z) / 2, 1f * Time.deltaTime);
        }
    }

    // 이동한 거리를 계산하는 함수
    float CountMove(Vector3 countMove)
    {
        return Mathf.Abs(countMove.x) + Mathf.Abs(countMove.z);  // x와 z 방향으로 이동한 거리의 절댓값 합을 반환
    }

    // 평면 객체의 위치를 갱신하는 함수 (이동 방향에 따라)
    public void planePos(int i)
    {
        Vector3 nextCountMove = countMove + arr[i];  // 다음 위치 계산
        if (CountMove(nextCountMove) < 5)  // 이동 거리가 5 미만이면
        {
            countMove = nextCountMove;  // 이동 거리 갱신
            if(i > 1)
            {
                target_z += 2 * arr[i];  // z 방향으로 이동
            }
            else
            {
                target_x += 2 * arr[i];  // x 방향으로 이동
            }

            plane.transform.position += arr[i];  // 평면 위치 갱신
        }
        Debug.Log(countMove);  // 이동 거리 로그 출력
    }

    // 평면 객체를 활성화하는 함수
    public void Bulddeok()
    {
        plane.SetActive(true);  // 평면 객체를 활성화
    }

    // 캐릭터를 이동시키는 함수
    public void Move()
    {
        plane.SetActive(false);  // 평면 객체 비활성화
        isMoving = true;  // 이동 시작
    }

    // 오른쪽으로 이동하는 함수
    public void MovingRight()
    {
        Vector3 nextCountMove = countMove + new Vector3(1.1f, 0, 0);  // 오른쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 거리가 5 미만이면
        {
            countMove = nextCountMove;  // 이동 거리 갱신
            target_x += new Vector3(1.1f, 0, 0);  // x 방향 목표 위치 갱신
        }
        Debug.Log(countMove);  // 이동 거리 로그 출력
    }

    // 왼쪽으로 이동하는 함수
    public void movingLeft()
    {
        Vector3 nextCountMove = countMove + new Vector3(-1.1f, 0, 0);  // 왼쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 거리가 5 미만이면
        {
            countMove = nextCountMove;  // 이동 거리 갱신
            target_x += new Vector3(-1.1f, 0, 0);  // x 방향 목표 위치 갱신
        }
        Debug.Log(countMove);  // 이동 거리 로그 출력
    }

    // 위쪽으로 이동하는 함수
    public void MovingUp(int i)
    {
        Vector3 nextCountMove = countMove + new Vector3(0, 0, 1.1f);  // 위쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 거리가 5 미만이면
        {
            countMove = nextCountMove;  // 이동 거리 갱신
            target_z += new Vector3(0, 0, 1.1f);  // z 방향 목표 위치 갱신
        }
        Debug.Log(countMove);  // 이동 거리 로그 출력
    }

    // 아래쪽으로 이동하는 함수
    public void MovingDown()
    {
        Vector3 nextCountMove = countMove + new Vector3(0, 0, -1.1f);  // 아래쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 거리가 5 미만이면
        {
            countMove = nextCountMove;  // 이동 거리 갱신
            target_z += new Vector3(0, 0, -1.1f);  // z 방향 목표 위치 갱신
        }
        Debug.Log(countMove);  // 이동 거리 로그 출력
    }
}
