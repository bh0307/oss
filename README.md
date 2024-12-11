# 캠프파이어 보드게임 / Campfire Board Game




## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for more details.


## 1. 게임 소개 / Game Introduction
🛠️ Unity 기반 멀티플레이어 게임 프로젝트
이 프로젝트는 Unity와 Photon PUN을 활용하여 멀티플레이어 환경에서 아이템을 획득, 조합, 버리는 기능을 구현한 게임입니다.
  
이 게임은 **2~4인 보드게임**으로, 아이템을 모아서 맵을 가장 먼저 탈출하는 사람이 이기는 게임입니다.  
This is a **2-4 player board game**, where the player who collects items and escapes the map first wins the game.

## 2. 게임 룰 / Game Rules  
- 맵은 총 **4x4**로 되어 있으며, **가운데**에 캠프파이어가 위치해 있습니다.  
  The map is a **4x4** grid, with the **campfire** located in the center.
  
- 캠프파이어 근처에서는 **온기**를 얻을 수 있으며, 최대 4개까지 획득 가능합니다.  
  Near the campfire, you can obtain **warmth**, and up to 4 warmths can be collected.
  
- **온기**를 통해 캠프파이어 밖에서도 활동할 수 있습니다.  
  You can use **warmth** to perform actions outside the campfire area.
  
- 캠프파이어 밖에서는 **온기**가 턴마다 1개씩 소모됩니다.  
  Outside the campfire, **warmth** is consumed by 1 unit per turn.
  
- 각 턴마다 **이동**, **아이템 조합**, **아이템 묻기** 등의 행동을 할 수 있습니다.  
  Each turn, you can take actions such as **moving**, **combining items**, or **burying items**.
  
- **온기**를 이용하여 움직이면, 그 칸에 숨겨진 아이템을 얻을 수 있습니다.  
  If you move using **warmth**, you may discover hidden items in that tile.
  
- 아이템은 최대 2개까지 가질 수 있으며, 특정 **조합식**에 따라 아이템을 조합하여 새로운 아이템을 얻을 수 있습니다.  
  You can hold up to 2 items, and by following certain **combinations**, you can combine items to create new ones.
  
- 조합한 아이템을 가지고 각 모서리 블록에 알맞게 가져가면 탈출할 수 있습니다.  
  You can escape by bringing the combined items to the appropriate corner block.
  
- 여러 명의 사람들 중에 가장 빨리 탈출하는 사람이 승리합니다.  
  The player who escapes first among all players wins the game.

## 3. 게임 구현 / Game Implementation  
- 게임 구현은 유니티를 사용했습니다.  
  The game is implemented using Unity.
  
- 서버의 경우 포톤을 사용하여 EC 동아리 서버에 연결했습니다.  
  For the server, Photon was used to connect to the EC club server.
  
- 자세한 건 개발 일지 참고(readme파일 밑에 있습니다.).  
  Refer to the development log for more details.
  

## 4. 앞으로 더 개발하는 점 / Future Development  
- AI를 탐재하여 혼자 플레이해도 AI와 같이 게임이 가능하게 만들기  
  Implement AI so players can enjoy the game solo against AI opponents.
  
- 다른 모드를 만들어서 플레이어들 간의 시야를 차단하여 서로 어디서 뭘 했는지 모르게 만들기  
  Create different modes where players' views are restricted, preventing them from knowing others' actions.
  
- 맵의 크기나 제한 시간을 방장이 마음대로 조정할 수 있는 설정 칸 추가  
  Add a settings option allowing the host to adjust the map size and time limit freely.


![image](https://github.com/user-attachments/assets/2834190f-68a7-436e-9e4f-437adc3edb80)


## 5. 📋 주요 기능
-아이템 획득: 맵에서 아이템을 찾아 인벤토리에 추가할 수 있습니다.

-아이템 조합: 특정 아이템들을 조합하여 새로운 아이템을 생성할 수 있습니다.

-아이템 버리기: 필요하지 않은 아이템을 인벤토리에서 제거할 수 있습니다.

-멀티플레이어 지원: Photon PUN을 통해 여러 플레이어와 함께 게임을 즐길 수 있습니다.

## 6.🗂️ 코드 구성
-Inventory.cs: 아이템의 획득, 조합, 버리기 등의 인벤토리 관련 기능을 관리합니다.
![image](https://github.com/user-attachments/assets/b12c0af3-1c18-4dff-9529-419200a607e0)


-MapManager.cs: 맵의 아이템 배치와 관련된 기능을 담당합니다.

-Ending.cs: 게임 종료 조건을 확인하고 처리합니다.

-UiManager.cs: 사용자 인터페이스의 알림 및 업데이트를 관리합니다.

-ItemContainor.cs: 아이템 이미지와 관련된 데이터를 보관합니다.

## 7.🔧 사용 기술
-Unity: 게임 개발 엔진으로, 2D 및 3D 게임 개발에 사용되었습니다.

-Photon PUN: 실시간 멀티플레이어 기능 구현을 위해 사용되었습니다.

-C#: 게임 로직과 시스템 구현에 사용되었습니다.

## 8. 실행 법!!



## 9 .🚀 시작하기

게임 시작을 누르면 로비씬으로 이동합니다.
When you press "Start Game," you will move to the lobby scene.
![image](https://github.com/user-attachments/assets/6253b407-b774-4a1c-8f8f-856f7ce80605)

로비씬에서는 방 찾기를 누르면 방을 찾을 수 있습니다.
In the lobby scene, you can press "Find Room" to search for rooms.
![image](https://github.com/user-attachments/assets/61003dd0-e5db-4389-87b9-c18d8f1650e2)

방 찾기를 누르면 이제 만들어진 방들을 볼 수 있습니다.
When you press "Find Room," you can see the rooms that have been created.
![image](https://github.com/user-attachments/assets/72f7ccde-7acf-4227-9639-af7f8e600f03)

누군가가 방을 만들면 그 방이 보이고 몇 명이 있는지도 볼 수 있습니다.
If someone creates a room, you can see that room and the number of participants.
![image](https://github.com/user-attachments/assets/e8b3783b-b987-4ee9-be09-dbbf46302717)

create room을 누르면 방을 만들 수 있습니다.
You can press "Create Room" to create a new room.
![image](https://github.com/user-attachments/assets/c32aa391-cdd0-47ce-af06-d69c3ddc8c7e)

사람이 들어오고 start를 누르면 게임이 시작되고 leave를 누르면 방에서 나갈 수 있습니다.
When someone joins, pressing "Start" begins the game, and pressing "Leave" exits the room.
![image](https://github.com/user-attachments/assets/1d1d2431-dcc0-4227-8d11-5ee19d84e87e)

게임이 시작되면 플레이어는 초록색 존에서 랜덤한 위치에 리스폰 됩니다.
When the game starts, players respawn at random locations in the green zone.
![image](https://github.com/user-attachments/assets/2eec3d46-1428-4aa8-8e17-755ce34206f7)

제한시간 내에 무엇을 할지 정하고 검은색 지역으로 가서 아이템을 찾습니다.
Decide what to do within the time limit and move to the black zone to find items.
![image](https://github.com/user-attachments/assets/bfac0ad3-42f5-4e2d-9de8-e61610178e06)

원하는 곳으로 가서 아이템을 찾습니다. 만약 그 구역에 없으면 없다는 알림이 뜹니다. 
검은색 지역으로 가면 하트가 하나씩 떨어집니다. 하트는 최대 4개까지 가질 수 있습니다.
Go to the desired location to find items. If the item is not there, a notification will appear. 
In the black zone, hearts decrease one by one, and you can have up to four hearts.
![image](https://github.com/user-attachments/assets/c653c262-601b-4863-8fc1-8de833f42520)

다른 사람의 차례에는 다른 사람의 차례라고 공지가 뜨고 이때는 아무것도 할 수 없습니다.
During another player’s turn, a notification appears saying it’s their turn, and you cannot take any actions.
![image](https://github.com/user-attachments/assets/20d09673-531c-418a-96ba-e90beff08b6c)

화살표를 눌러 자기가 이동하고자 하는 곳을 타일을 통해 볼 수 있습니다.
Press the arrow keys to see the tiles of your desired movement path.
![image](https://github.com/user-attachments/assets/58415de2-8b0e-4eb3-813c-23c0eb3d3061)

이동 후 아이템을 얻으면 아이템을 얻었다는 공지가 뜨고 오른쪽 인벤토리 칸에 어떤 아이템을 얻었는지 공지가 뜹니다.
After moving, if you obtain an item, a notification will display that you’ve acquired it, and it will appear in the right inventory slot.
![image](https://github.com/user-attachments/assets/10012472-1955-421a-a105-ec0fa12cfa90)

각 모서리 부분에 가면 어떤 아이템을 가져와야 게임이 끝나는지 알려줍니다.
At each corner, you’ll find information about which items are needed to finish the game.
![image](https://github.com/user-attachments/assets/75357961-2653-43f0-9061-288227742689)

이동하다 보면 또 아이템을 얻을 수 있습니다.
While moving, you may acquire additional items.
![image](https://github.com/user-attachments/assets/4e2426a1-6ab7-4b40-8fb3-7d6685290471)

조합이 불가능한 아이템의 경우 조합할 수 없다고 뜹니다.
If an item cannot be combined, a message will indicate that it’s not possible.
![image](https://github.com/user-attachments/assets/0465f517-2e1f-4561-8718-b730d301aa71)

이런 경우에는 다른 한 아이템을 버려야 하는데 이때 묻기 기능을 씁니다.
In this case, you must discard one of the items using the "bury" feature.
![image](https://github.com/user-attachments/assets/8095b085-199d-4bb3-8fa9-5a59df4b6a56)

아이템을 버리면 인벤토리 칸에서 아이템이 사라집니다. 버린 아이템은 묻은 위치에 그대로 존재합니다.
Discarded items disappear from the inventory but remain in the spot where they were buried.
![image](https://github.com/user-attachments/assets/a841194e-2793-4a5b-a1db-f2ee4d552d5c)

다시 움직이면서 아이템을 찾습니다.
Move again to search for more items.
![image](https://github.com/user-attachments/assets/64a48d84-91f5-490b-8cf3-a461e82d6625)

만약 하트가 없으면 초록 존으로 다시 돌아와서 하트를 채워야 합니다. 하트는 1턴당 1개씩 찹니다.
If you run out of hearts, return to the green zone to replenish them. Hearts recover one per turn.
![image](https://github.com/user-attachments/assets/af81e5dd-af29-4a68-a1e7-e1477a117d3c)

계속해서 게임을 플레이하다 보면 다른 아이템을 얻고 조합을 해서 조합 아이템을 만들 수 있습니다.
As the game progresses, you can acquire and combine items to create new ones.
![image](https://github.com/user-attachments/assets/86e6a3ef-f3ad-4a17-83ef-9260329f4ae5)

이제 가지고 있는 아이템을 가지고 특정 지역으로 가면 게임이 끝났다는 공지가 올라오고 게임이 끝납니다.
Finally, take the required items to a specific location to receive a notification that the game has ended.
![image](https://github.com/user-attachments/assets/e6dd22e7-d083-4c8b-9481-437736836f01)

















