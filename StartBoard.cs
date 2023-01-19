using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Board board = FindObjectOfType<Board>();
        board.Init();
        board.boardUI.gameObject.SetActive(true);

        PlayerController player = FindObjectOfType<PlayerController>();
        player.Init();

        FadeOut fade = FindObjectOfType<FadeOut>();
        if (fade != null)
            fade.FadeI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
