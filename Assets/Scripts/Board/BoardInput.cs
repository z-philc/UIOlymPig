﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Board))]
public class BoardInput : MonoBehaviour
{
    public Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    // set our clicked tile
    public void ClickTile(Tile tile)
    {
        if (board == null)
            return;

        if (board.clickedTile == null && board.boardQuery.IsUnblocked(tile.xIndex, tile.yIndex))
        {
            board.clickedTile = tile;
            board.ShowEffectAtTileClicked(tile);
        }

        if (board.clickedTile != null)
        {
            if (board.targetTile != null) board.targetTile = null;
            if (board.boardQuery.IsNextTo(tile, board.clickedTile))
            {
                board.targetTile = tile;
            }
            else
            {
                
                if (board.clickEffect != null) Destroy(board.clickEffect);
                board.clickedTile = null;
                board.clickedTile = tile;
                board.ShowEffectAtTileClicked(tile);
            }
        }  
    }

    public bool CheckTargetAndClickedIsCorrect(Tile tileA, Tile tileB)
    {

        return false;
    }
    // set our target tile
    public void DragToTile(Tile tile)
    {
        if (board == null)
            return;

        if (board.clickedTile != null && board.boardQuery.IsNextTo(tile, board.clickedTile)
            && board.boardQuery.IsUnblocked(tile.xIndex, tile.yIndex))
        {
            board.targetTile = tile;
        }
    }

    // Swap Tiles if we release the touch/mouse and have valid clicked and target Tiles
    public void ReleaseTile()
    {
        if (board == null)
            return;

        if (board.clickedTile != null && board.targetTile != null)
        {
            board.SwitchTiles(board.clickedTile, board.targetTile);

            board.clickedTile = null;
            board.targetTile = null;      
        }  
    }
}
