using System.Collections;
using System.Collections.Generic;
using NetworkLand;
using UnityEngine;

namespace NetworkLand
{
    public class GameManager : MonoBehaviour
    {
        public GameObject player;

        public bool IsStart
        {
            get { return isStart; }
        }
        private bool isStart = false;
        
        public void StartGame()
        {
            DoOnMainThread.ExecuteOnMainThread.Enqueue(
                () => { ResetGame(); }
            );
        }

        public bool Move(Vector3 dir)
        {
            if(dir != Vector3.zero)
            {
                DoOnMainThread.ExecuteOnMainThread.Enqueue(
                    () => { StartCoroutine(MovePlayer(dir)); }
                );

                return true;
            }

            return false;
        }

        private IEnumerator MovePlayer(Vector3 moveDir)
        {
            player.transform.Translate(moveDir);
            moveDir = Vector3.zero;
            yield return null;
        }

        public IEnumerator ResetGame()
        {
            player.transform.position = Vector3.zero;
            isStart = true;
            yield return null;
        }
    }
}
