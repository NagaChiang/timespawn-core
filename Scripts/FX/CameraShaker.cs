﻿using System.Collections;
using System.Collections.Generic;
using Timespawn.Core.Utils;
using UnityEngine;

namespace Timespawn.Core.FX
{
    public class CameraShaker : Singleton<CameraShaker>
    {
        private const float SEED_MAX = 10000.0f;

        private Camera MainCamera;
        private CameraShakeParams CurrentCameraShakeParams;

        private float ElapsedDuration;
        private float EaseScale;

        private Vector3 PositionSeeds;
        private Vector3 RotationSeeds;

        private void Awake()
        {
            MainCamera = Camera.main;
        }

        private void Update()
        {
            if (ElapsedDuration < CurrentCameraShakeParams.Duration)
            {
                ElapsedDuration += Time.deltaTime;

                UpdateEaseInOut();
                UpdateTransform();
            }
        }

        public void PlayShake(CameraShakeParams shakeParams)
        {
            CurrentCameraShakeParams = shakeParams;
            ElapsedDuration = 0.0f;
            EaseScale = 0.0f;

            // Perlin noise seeds (x-axis)
            PositionSeeds.x = Random.Range(0.0f, SEED_MAX);
            PositionSeeds.y = Random.Range(0.0f, SEED_MAX);
            PositionSeeds.z = Random.Range(0.0f, SEED_MAX);
            RotationSeeds.x = Random.Range(0.0f, SEED_MAX);
            RotationSeeds.y = Random.Range(0.0f, SEED_MAX);
            RotationSeeds.z = Random.Range(0.0f, SEED_MAX);
        }

        public void StopShake()
        {
            ElapsedDuration = CurrentCameraShakeParams.Duration;
        }

        private void UpdateEaseInOut()
        {
            float easeInDuration = CurrentCameraShakeParams.EaseInDuration;
            float easeOutDuration = CurrentCameraShakeParams.EaseOutDuration;
            if (ElapsedDuration < easeInDuration)
            {
                // Ease in
                if (easeInDuration > 0)
                {
                    EaseScale += Time.deltaTime / easeInDuration;
                }
                else
                {
                    EaseScale = 1.0f;
                }
            }
            else if ((CurrentCameraShakeParams.Duration - ElapsedDuration) < easeOutDuration)
            {
                // Ease out
                if (easeOutDuration > 0)
                {
                    EaseScale -= Time.deltaTime / easeOutDuration;
                }
                else
                {
                    EaseScale = 0.0f;
                }
            }

            EaseScale = Mathf.Clamp01(EaseScale);
        }

        private void UpdateTransform()
        {
            Vector3 newPosition;
            ShakeParamsVector3 posParams = CurrentCameraShakeParams.PositionShakeParams;
            newPosition.x = CalculateOffset(posParams.x.Amplitude, posParams.x.Frequency, PositionSeeds.x);
            newPosition.y = CalculateOffset(posParams.y.Amplitude, posParams.y.Frequency, PositionSeeds.y);
            newPosition.z = CalculateOffset(posParams.z.Amplitude, posParams.z.Frequency, PositionSeeds.z);

            Vector3 newRotation;
            ShakeParamsVector3 rotParams = CurrentCameraShakeParams.RotationShakeParams;
            newRotation.x = CalculateOffset(rotParams.x.Amplitude, rotParams.x.Frequency, RotationSeeds.x);
            newRotation.y = CalculateOffset(rotParams.y.Amplitude, rotParams.y.Frequency, RotationSeeds.y);
            newRotation.z = CalculateOffset(rotParams.z.Amplitude, rotParams.z.Frequency, RotationSeeds.z);

            MainCamera.transform.localPosition = newPosition;
            MainCamera.transform.localRotation = Quaternion.Euler(newRotation);
        }

        private float CalculateOffset(float amplitude, float frequency, float seed)
        {
            return amplitude * EaseScale * (Mathf.PerlinNoise(seed, ElapsedDuration * frequency) - 0.5f);
        }
    }
}