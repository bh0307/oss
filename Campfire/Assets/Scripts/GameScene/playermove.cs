using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    // 네 방향으로 이동할 벡터 배열 (우, 좌, 상, 하)
    Vector3[] arr = {
        new Vector3(1.1f, 0, 0),  // 오른쪽
        new Vector3(-1.1f, 0, 0), // 왼쪽
        new Vector3(0, 0, 1.1f),  // 위
        new Vector3(0, 0, -1.1f)  // 아래
    };

    Animator animator;  // 애니메이터 컴포넌트
    public bool LeftMove = false;  // 왼쪽 이동 상태
    public bool RightMove = false; // 오른쪽 이동 상태
    public bool UpMove = false;    // 위쪽 이동 상태
    public bool DownMove = false;  // 아래쪽 이동 상태
    public bool isMoving = false;  // 이동 중 여부
    Vector3 moveVelocity = Vector3.zero;  // 이동 속도
    float moveSpeed = 0.5f;  // 이동 속도 (기본값)
    public Vector3 countMove;  // 이동 횟수를 저장하는 변수
    public Vector3 target_x;  // 목표 x 좌표
    public Vector3 target_z;  // 목표 z 좌표

    public GameObject planePrefab;  // 플레이어와 함께 이동할 plane 오브젝트 프리팹
    public GameObject plane;  // 인스턴스화된 plane 오브젝트

    // 초기화 함수 (게임 시작 시 한 번 실행)
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();  // Animator 컴포넌트를 가져옴
        countMove = new Vector3(0f, 0f, 0f);  // countMove 초기화
        target_x = transform.position;  // 현재 위치를 목표 위치로 설정
        target_z = transform.position;  // 현재 위치를 목표 위치로 설정

        // plane 오브젝트 인스턴스화 후 위치 설정
        plane = Instantiate(planePrefab, transform);
        plane.transform.position += new Vector3(-3.5f, 1f, 0);
        plane.SetActive(false);  // 처음에는 비활성화
    }

    // 매 프레임마다 호출되는 함수
    void Update()
    {
        if(isMoving)  // 이동 중이라면
        {
            // 현재 위치에서 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, (target_x + target_z)/2, 1f * Time.deltaTime);
        }
    }

    // countMove의 x와 z 값의 절대값 합을 구하는 함수
    float CountMove(Vector3 countMove)
    {
        return Mathf.Abs(countMove.x) + Mathf.Abs(countMove.z);
    }

    // plane 오브젝트의 위치를 이동시키는 함수
    public void planePos(int i)
    {
        // countMove에 i에 해당하는 방향을 더해 다음 위치 계산
        Vector3 nextCountMove = countMove + arr[i];
        if (CountMove(nextCountMove) < 5)  // 이동 횟수가 5 이하일 경우
        {
            countMove = nextCountMove;  // countMove 갱신
            if(i > 1)  // 위/아래 방향이면
            {
                target_z += 2 * arr[i];  // target_z 갱신
            }
            else  // 왼쪽/오른쪽 방향이면
            {
                target_x += 2 * arr[i];  // target_x 갱신
            }
            
            plane.transform.position += arr[i];  // plane의 위치 갱신
        }
        Debug.Log(countMove);  // 현재 위치 출력
    }

    // Bulddeok() 함수: plane 오브젝트를 활성화
    public void Bulddeok()
    {
        plane.SetActive(true);
    }

    // Move() 함수: plane 비활성화 후 이동 시작
    public void Move()
    {
        plane.SetActive(false);  // plane 비활성화
        isMoving = true;  // 이동 상태로 변경
    }

    // 오른쪽으로 이동하는 함수
    public void MovingRight()
    {
        Vector3 nextCountMove = countMove + new Vector3(1.1f, 0, 0);  // 오른쪽 방향으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 횟수가 5 이하일 경우
        {
            countMove = nextCountMove;  // countMove 갱신
            target_x += new Vector3(1.1f, 0, 0);  // target_x 갱신
        }
        Debug.Log(countMove);  // 현재 위치 출력
    }

    // 왼쪽으로 이동하는 함수
    public void movingLeft()
    {
        Vector3 nextCountMove = countMove + new Vector3(-1.1f, 0, 0);  // 왼쪽 방향으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 횟수가 5 이하일 경우
        {
            countMove = nextCountMove;  // countMove 갱신
            target_x += new Vector3(-1.1f, 0, 0);  // target_x 갱신
        }
        Debug.Log(countMove);  // 현재 위치 출력
    }

    // 위로 이동하는 함수
    public void MovingUp(int i)
    {
        Vector3 nextCountMove = countMove + new Vector3(0, 0, 1.1f);  // 위쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 횟수가 5 이하일 경우
        {
            countMove = nextCountMove;  // countMove 갱신
            target_z += new Vector3(0, 0, 1.1f);  // target_z 갱신
        }
        Debug.Log(countMove);  // 현재 위치 출력
    }

    // 아래로 이동하는 함수
    public void MovingDown()
    {
        Vector3 nextCountMove = countMove + new Vector3(0, 0, -1.1f);  // 아래쪽으로 이동
        if (CountMove(nextCountMove) < 5)  // 이동 횟수가 5 이하일 경우
        {
            countMove = nextCountMove;  // countMove 갱신
            target_z += new Vector3(0, 0, -1.1f);  // target_z 갱신
        }
        Debug.Log(countMove);  // 현재 위치 출력
    }
}
