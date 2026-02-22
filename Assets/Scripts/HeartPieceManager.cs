using UnityEngine;

public class HeartPieceManager : MonoBehaviour
{
    public HeartPiece[] pieces;

    private int currentActiveIndex = -1;
    private int snappedCount = 0;

    void Start()
    {
        // initialize pieces
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Initialize(this, i);
        }
    }

    public void ActivateNextPiece()
    {
        currentActiveIndex++;

        if (currentActiveIndex >= pieces.Length)
            return;

        pieces[currentActiveIndex].Activate();
    }

    public void OnPieceSnapped(HeartPiece piece)
    {
        snappedCount++;

        Debug.Log("Piece snapped: " + piece.pieceIndex);

        if (snappedCount >= pieces.Length)
        {
            OnHeartCompleted();
        }
    }

    void OnHeartCompleted()
    {
        Debug.Log("HEART COMPLETE");

        foreach (HeartPiece piece in pieces)
        {
            piece.PlayCompleteAnimation();
        }
    }
}