using UnityEngine;

public class HeartPieceManager : MonoBehaviour
{
    public static HeartPieceManager Instance;

    public HeartPiece[] pieces;

    public Transform heartSnapTargetGroupTransform;

    private int currentActiveIndex = -1;
    private int snappedCount = 0;
    public int darkCount = 0;
    private GameManager manager;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        // initialize pieces
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Initialize(this, i);
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateNextPiece()
    {
        if (snappedCount <= currentActiveIndex)
        {
            return;
        }
        currentActiveIndex++;

        if (currentActiveIndex >= pieces.Length)
            return;

        Vector2 randomInsideUnitCircle = Random.insideUnitCircle;
        randomInsideUnitCircle *= GameManager.Instance.player.movementBox.radius;
        randomInsideUnitCircle += new Vector2(GameManager.Instance.player.movementBox.gameObject.transform.position.x, GameManager.Instance.player.movementBox.gameObject.transform.position.y);

        pieces[currentActiveIndex].Activate(randomInsideUnitCircle);
    }

    public void ActivateNextPiece(Vector2 position)
    {
        if (snappedCount <= currentActiveIndex)
        {
            return;
        }
        currentActiveIndex++;

        if (currentActiveIndex >= pieces.Length)
            return;

        pieces[currentActiveIndex].Activate(position);
    }

    public void OnPieceSnapped(HeartPiece piece)
    {
        snappedCount++;

        if (piece.isDark)
        {
            darkCount++;
        }

        Debug.Log("Piece snapped: " + piece.pieceIndex);

        if (snappedCount >= 1)//pieces.Length)
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

        GameManager.Instance.OnCompleteHeart();
    }

    public void ResetPieces()
    {
        currentActiveIndex = -1;
        snappedCount = 0;
        darkCount = 0;

        foreach (HeartPiece piece in pieces)
        {
            piece.gameObject.SetActive(false);
            piece.isSnapped = false;

            SnapToTarget snap = piece.GetComponent<SnapToTarget>();
            if (snap != null)
            {
                snap.Reactivate();
            }
        }
    }

}