/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated September 24, 2021. Replaces all prior versions.
 *
 * Copyright (c) 2013-2021, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using ScriptableEvents.Events;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spine.Unity.Examples {
	public class CombinedSkin : MonoBehaviour {
		[SpineSkin]
		public List<string> skinsToCombine;

        private Skin combinedSkin;
        private Skin defaultSkin;
        private SkinData skinData;
        public SkinDataScriptableEvent skinEvent;

        private void Start ()
        {
            SetSkin();
            skinData = new SkinData
            {
                skinsToCombine = skinsToCombine
            };
            defaultSkin = new Skin("Default");
            defaultSkin.CopySkin(combinedSkin);;
            skinEvent?.Raise(skinData);
        }

        private void SetSkin()
        {
            
            var skeletonComponent = GetComponent<ISkeletonComponent>();
            if (skeletonComponent == null) return;
            var skeleton = skeletonComponent.Skeleton;
            if (skeleton == null) return;

            combinedSkin = combinedSkin ?? new Skin("combined");
            combinedSkin.Clear();
            foreach (var skin in skinsToCombine.Select(skinName => skeleton.Data.FindSkin(skinName)).Where(skin => skin != null))
            {
                combinedSkin.AddSkin(skin);
            }

            skeleton.SetSkin(combinedSkin);
            skeleton.SetToSetupPose();
            var animationStateComponent = skeletonComponent as IAnimationStateComponent;
            if (animationStateComponent != null) animationStateComponent.AnimationState.Apply(skeleton);
        }
        public void SetSkin(List<string> skinsToAdd)
        {
            skinsToCombine = skinsToAdd;
            SetSkin();
        }
        public void SetSkin(SkinData skins)
        {
            skinsToCombine = skins.skinsToCombine;
            SetSkin();
        }

        public void ModifySkin(SkinData skinToModify)
        {
            var skeletonComponent = GetComponent<ISkeletonComponent>();
            var skeleton = skeletonComponent?.Skeleton;
            if (skeleton == null) return;

            combinedSkin.Clear();   
           
           
            combinedSkin = new Skin("combined");
            combinedSkin.CopySkin(defaultSkin);
           
            

            foreach (var skin in skinToModify.skinsToCombine.Select(skinName => skeleton.Data.FindSkin(skinName)).Where(skin => skin != null))
            {
                combinedSkin.AddSkin(skin);
            }

            skeleton.SetSkin(combinedSkin);
            skeleton.SetToSetupPose();
            var animationStateComponent = skeletonComponent as IAnimationStateComponent;
            if (animationStateComponent != null) animationStateComponent.AnimationState.Apply(skeleton);
            
            defaultSkin.Clear();
            defaultSkin.CopySkin(combinedSkin);
        }
        public void EquipSkin(SkinData skinToModify)
        {
            var skeletonComponent = GetComponent<ISkeletonComponent>();
            var skeleton = skeletonComponent?.Skeleton;
            if (skeleton == null) return;
            
            combinedSkin = new Skin("combined");
            combinedSkin.CopySkin(defaultSkin);
            
            foreach (var skin in skinToModify.skinsToCombine.Select(skinName => skeleton.Data.FindSkin(skinName)).Where(skin => skin != null))
            {
                combinedSkin.AddSkin(skin);
            }

            skeleton.SetSkin(combinedSkin);
            skeleton.SetToSetupPose();
            var animationStateComponent = skeletonComponent as IAnimationStateComponent;
            if (animationStateComponent != null) animationStateComponent.AnimationState.Apply(skeleton);
            
            defaultSkin.Clear();
            defaultSkin.CopySkin(combinedSkin);
        }
    }

}