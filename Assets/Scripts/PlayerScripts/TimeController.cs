using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class Keyframe
    {
        public Vector3 Position;
        public Vector3 LocalScale;

        public Keyframe(Vector3 position,Vector3 localScale)
        {
            this.Position = position;
            this.LocalScale = localScale;
        }
    }

    public class TimeController : MonoBehaviour
    {
        public List<Keyframe> keyframeList = new List<Keyframe>();

        public bool isReversing = false;
     
        public int keyframe = 5;
        private int _frameCounter = 0;
        private int _reverseCounter = 0;
     
        private Vector3 _currentPosition;
        private Vector3 _previousPosition;
        private Vector3 _currentScale;
        private SpriteRenderer _renderer;

        private int _decreaseIndexValue = 1;
        private bool _increaseForward;
        
        public bool firstRun;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                _decreaseIndexValue = keyframeList.Count;
            }
        }

        private void FixedUpdate()
        {
            if(!isReversing)
            {
                if(_frameCounter < keyframe)
                {
                    _frameCounter += 1;
                }
                else
                {
                    _frameCounter = 0;
                    var transform1 = transform;
                    var position = transform1.position;
                    keyframeList.Add(new Keyframe(position,transform1.localScale));
                }
            }
            else
            {
                if(_reverseCounter > 0)
                {
                    _reverseCounter -= 1;
                }
                else
                {
                    _reverseCounter = keyframe;
                    RestorePositions();
                }
     
                if(firstRun)
                {
                    firstRun = false;
                    RestorePositions();
                }
     
                var interpolation = (float) _reverseCounter / (float) keyframe;
                transform.position = Vector3.Lerp(_previousPosition, _currentPosition, interpolation);
                _renderer.flipX = !(_currentScale.x <= -1f);
            }
            
        }

        private void RestorePositions()
        {
            if (_decreaseIndexValue>= keyframeList.Count)
            {
                _decreaseIndexValue = keyframeList.Count-1;
                _increaseForward = true;
            }
            else if (_decreaseIndexValue <= 0)
            {
                _decreaseIndexValue = 1;
                _increaseForward = false;
            }

            var lastIndex = keyframeList.Count - _decreaseIndexValue;
            var secondToLastIndex = keyframeList.Count - (_decreaseIndexValue+1);

            if (_increaseForward)
            {
                _decreaseIndexValue -= 1;
                _currentPosition  = ((Keyframe) keyframeList[secondToLastIndex]).Position;
                _previousPosition = ((Keyframe) keyframeList[lastIndex]).Position;
                
                _currentScale  = ((Keyframe) keyframeList[secondToLastIndex]).LocalScale;

            }
            else
            {
                _decreaseIndexValue += 1;
                _currentPosition  = ((Keyframe) keyframeList[lastIndex]).Position;
                _previousPosition = ((Keyframe) keyframeList[secondToLastIndex]).Position;
                
                _currentScale  = ((Keyframe) keyframeList[lastIndex]).LocalScale;
            }
            
        }
    }
}