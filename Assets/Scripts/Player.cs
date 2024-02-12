using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Player : MonoBehaviour
{


    public static Player Instance { get; private set; }



    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
     

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void Update()
    {

        HandleMovement();
        HandleInteractions();
 
    }

    

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {

        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        RaycastHit raycastHit;


        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //has ClearCounter
                //clearCounter.Interact();
            }

        }
    }

    public void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        RaycastHit raycastHit;


        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //has ClearCounter
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }          
            } else
            {
                SetSelectedCounter(null);
            }
        } else
        {
            SetSelectedCounter(null);
        }

    }


    public void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);



        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            //не может двигаться на moveDir

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;

            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //не может двигаться на Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //не может двигаться ни в одном направлении
                }
            }



        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }



        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking() { 
        return isWalking; 
    }


    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }


}
