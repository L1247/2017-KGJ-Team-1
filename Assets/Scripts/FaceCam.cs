﻿using UnityEngine;

public class FaceCam : MonoBehaviour
{

        private Camera _mainCam = null;
        private Transform _mFace = null;


        private void Awake ()
        {
                _mainCam = Camera.main;
                _mFace = transform;
        }

        void FixedUpdate ()
        {
                if ( !_mFace )
                        return;
                if ( _mainCam != null )
                        UpdateFacingCamera();
        }

        private void UpdateFacingCamera ()
        {
                _mFace.LookAt( _mainCam.transform );
        }

}
