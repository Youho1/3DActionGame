using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class PlayerVFXManager : MonoBehaviour
    {
        public VisualEffect footStep;
        public ParticleSystem Blade01;
        public ParticleSystem Blade02;
        public ParticleSystem Blade03;
        public VisualEffect Slash;
        public VisualEffect Heal;
        // 走る時のVFX
        public void Update_FootStep(bool state)
        {
            if (state)
            {
                footStep.Play();
            }
            else
            {
                footStep.Stop();
            }
        }
        // ブレード１のパーティクル
        public void PlayBlade01()
        {
            Blade01.Play();
        }
        // ブレード2のパーティクル
        public void PlayBlade02()
        {
            Blade02.Play();
        }
        // ブレード3のパーティクル
        public void PlayBlade03()
        {
            Blade03.Play();
        }
        // ブレードのパーティクルを停止
        public void StopBlade() {
            Blade01.Simulate(0);
            Blade01.Stop();
            Blade02.Simulate(0);
            Blade02.Stop();
            Blade03.Simulate(0);
            Blade03.Stop();
        }
        // 斬撃する時のVFX
        public void PlaySlash(Vector3 pos)
        {
            Slash.transform.position = pos;
            Slash.Play();
        }
        // 回復する時のVFX
        public void PlayHealVFX()
        {
            Heal.Play();
        }
    }
}
