using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] ParticleSystem dieParticles;
    [SerializeField, Range(0.01f,1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f,1f)] float jumpHeight = 0.5f;

    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;

    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos -1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update(){
        var moveDir = Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow))
            moveDir += new Vector3(0, 0, 1);

        else if(Input.GetKey(KeyCode.DownArrow))
            moveDir += new Vector3(0, 0, -1);

        else if(Input.GetKey(KeyCode.RightArrow))
            moveDir += new Vector3(1, 0, 0);

        else if(Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector3(-1, 0, 0);
        
        if(moveDir != Vector3.zero && IsJumping() == false)
            Jump(moveDir);
    }

    private void Jump(Vector3 targetDirection)
    {
        // Rotasi
        Vector3 TargetPosition = transform.position + targetDirection;
        transform.LookAt(TargetPosition);

        // Animasi Loncat 
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration/2));

        if(TargetPosition.z <= backBoundary ||
            TargetPosition.x <= leftBoundary ||
            TargetPosition.x >= rightBoundary)
            return;

        if(Tree.AllPositions.Contains(TargetPosition))
            return;

        // Maju Mundur / Move
        transform.DOMoveX(TargetPosition.x, moveDuration);
        transform.DOMoveZ(TargetPosition.z, moveDuration);
    }

    private bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.enabled == false)
            return;

        var car = other.GetComponent<Car>();
        if (car != null)
        {
            AnimateDie(car);
        }

        if (other.tag == "Car")
        {
            // AnimateDie();
        }
    }

    private void AnimateDie(Car car)
    {
        // var isRight = car.trasnform.rotation.y == 90;
        // transform.DOMoveX(isRight ? 8 : -8, 0.2f);
        // transform
        //     .DORotate(Vector3.forward*360, 0.2f)
        //     .SetLoops(-1, LoopType.Restart);

        // Gepeng
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(3, 0.2f);
        transform.DOScaleZ(2, 0.2f);

        // Matiin pergerakan
        this.enabled = false;
        dieParticles.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("Stay");
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("Exit");
    }
}
