﻿/*==============================================================================
Copyright 2017 Maxst, Inc. All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.IO;
using JsonFx.Json;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine.Rendering;

namespace maxstAR
{
    public class QrCodeTrackableBehaviour : AbstractQrCodeTrackableBehaviour
    {
        [SerializeField]
        private string qrCodeSearchingWords = "";

        public string QrCodeSearchingWords
        {
            get
            {
                return this.qrCodeSearchingWords;
            }

            set
            {
                this.qrCodeSearchingWords = value;
            }
        }

        [SerializeField]
        private float qrCodeRealSize = -1.0f;

        public float QrCodeRealSize
        {
            get
            {
                return this.qrCodeRealSize;
            }

            set
            {
                this.qrCodeRealSize = value;
            }
        }

        public override void OnTrackSuccess(string id, string name, Matrix4x4 poseMatrix)
        {
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable renderers
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			transform.position = MatrixUtils.PositionFromMatrix(poseMatrix);
			transform.rotation = MatrixUtils.QuaternionFromMatrix(poseMatrix);
			
			/**#region EDITING_SITE(1)
			PlayerPrefs.SetString("Choose", name);
			Debug.Log("Trackable detected : " + name);
			#endregion**/
        }

        public override void OnTrackFail()
        {
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable renderer
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable collider
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			/**#region EDITING_SITE(2)
			PlayerPrefs.SetString("Choose", "");
			Debug.Log("QR Code Lost...");
			#endregion**/
        }
    }
}