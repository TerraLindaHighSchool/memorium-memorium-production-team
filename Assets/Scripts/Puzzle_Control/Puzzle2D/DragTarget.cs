using System.Collections;
using System.Collections.Generic;
using Puzzle_Control.Puzzle2D;
using UnityEngine;

public class DragTarget : PuzzleObject2D {
    public                      float          fuzz      = 0.1f;
    override protected readonly bool           clickable = false;
    override protected readonly bool           draggable = false;
    [SerializeField] private PuzzleObject2D target;

    public PuzzleObject2D Target => target;
    
    [SerializeField] private onComplete
}
