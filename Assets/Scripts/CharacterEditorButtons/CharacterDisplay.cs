using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;
using UnityEngine.UI;

namespace Game.UI
{
    public class CharacterDisplay : MonoBehaviour
    {
        /// <summary>
        /// Animator for the Head
        /// </summary>
        public Animator Head;

        /// <summary>
        /// Animator for the Body
        /// </summary>
        public Animator Body;

        /// <summary>
        /// Animator for the Legs
        /// </summary>
        public Animator Legs;

        /// <summary>
        /// Animator for the ArmBehind
        /// </summary>
        public Animator ArmBehind;

        /// <summary>
        /// Character Outfit Button Prefab
        /// </summary>
        public GameObject CharacterOutfitButtonPrefab;

        /// <summary>
        /// Character Edit Panel
        /// </summary>
        public CharacterEditPanel CharacterEditPanel;

        /// <summary>
        /// Head Icon Holder
        /// </summary>
        public GameObject HeadIconHolder;

        /// <summary>
        /// Body Icon Holder
        /// </summary>
        public GameObject BodyIconHolder;

        /// <summary>
        /// Legs Icon Holder
        /// </summary>
        public GameObject LegsIconHolder;

        /// <summary>
        /// Character OutfitHolder
        /// </summary>
        public GameObject CharacterOutfitHolder;

        /// <summary>
        /// Selected Head Icon
        /// </summary>
        public Image SelectedHeadIcon;

        /// <summary>
        /// Selected Body Icon
        /// </summary>
        public Image SelectedBodyIcon;

        /// <summary>
        /// Selected Legs Icon
        /// </summary>
        public Image SelectedLegsIcon;

        /// <summary>
        /// Animation Type
        /// </summary>
        private AnimationType _AnimationType = AnimationType.Idle;

        /// <summary>
        /// Check to see if the created button
        /// </summary>
        private bool _CreatedButtons = false;

        /// <summary>
        /// List of Object Tab Displays
        /// </summary>
        private IList<GameObject> _CurrentObjectTabDisplays = new List<GameObject>();

        /// <summary>
        /// List of DisplayAnimationButtons
        /// </summary>
        public DisplayAnimationButton[] DisplayAnimationButtons;

        /// <summary>
        /// When this panel is enabled
        /// </summary>
        public void ShowPanel(bool toggle)
        {
            this.gameObject.SetActive(toggle);
            this.CharacterOutfitHolder.SetActive(toggle);
            if (!toggle)
                return;
            this._AnimationType = AnimationType.Idle;
            this.ChangeAnimatorLayerSet(CharacterBodyType.Head, this.CharacterEditPanel.CharacterConfig.HeadType, false, AssetFactory.Instance.HeadIcons[this.CharacterEditPanel.CharacterConfig.HeadType]);
            this.ChangeAnimatorLayerSet(CharacterBodyType.Body, this.CharacterEditPanel.CharacterConfig.BodyType, false, AssetFactory.Instance.BodyIcons[this.CharacterEditPanel.CharacterConfig.BodyType]);
            this.ChangeAnimatorLayerSet(CharacterBodyType.Legs, this.CharacterEditPanel.CharacterConfig.LegsType, false, AssetFactory.Instance.LegIcons[this.CharacterEditPanel.CharacterConfig.LegsType]);
            this.CharacterAnimationSelection(this._AnimationType);
            this.CreateButtons();
        }

        /// <summary>
        /// Change Animator Layer Set
        /// </summary>
        public void ChangeAnimatorLayerSet(CharacterBodyType type, int layerType, bool setValue, Sprite icon)
        {
            switch(type)
            {
                case CharacterBodyType.Head:
                    this.SelectedHeadIcon.sprite = icon;
                    for (int i = 0; i < this.Head.layerCount; i++)
                    {
                        this.Head.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }

                    if (setValue)
                    {
                        this.CharacterEditPanel.CharacterConfig.HeadType = layerType;
                    }
                    break;
                case CharacterBodyType.Body:
                    this.SelectedBodyIcon.sprite = icon;
                    for (int i = 0; i < this.Body.layerCount; i++)
                    {
                        this.Body.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    for (int i = 0; i < this.ArmBehind.layerCount; i++)
                    {
                        this.ArmBehind.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    if (setValue)
                    {
                        this.CharacterEditPanel.CharacterConfig.BodyType = layerType;
                        this.CharacterEditPanel.CharacterConfig.ArmBehindType = layerType;
                    }
                    break;
                case CharacterBodyType.Legs:
                    this.SelectedLegsIcon.sprite = icon;
                    for (int i = 0; i < this.Legs.layerCount; i++)
                    {
                        this.Legs.SetLayerWeight(i, i == layerType ? 1.0f : 0.0f);
                    }
                    if(setValue)
                        this.CharacterEditPanel.CharacterConfig.LegsType = layerType;
                    break;
                default:
                    break;
            }
            for(int i = 0; i < this._CurrentObjectTabDisplays.Count; i++)
            {
                CharacterOutfitButton outfitButton = this._CurrentObjectTabDisplays[i].GetComponent<CharacterOutfitButton>();
                if (outfitButton.CharacterBodyType != type)
                    continue;
                if (outfitButton.BodyType != layerType)
                    outfitButton.ButtonToggle(true);
            }
        }

        /// <summary>
        /// Create buttons on the slider
        /// </summary>
        public void CreateButtons()
        {
            if (this._CreatedButtons)
                return;
            this._CreatedButtons = true;
            for(int i = 0; i < AssetFactory.Instance.HeadIcons.Length; i++)
            {
                GameObject objIcon = (GameObject)Instantiate(this.CharacterOutfitButtonPrefab);
                objIcon.transform.SetParent(this.HeadIconHolder.transform);
                objIcon.transform.localPosition = Vector3.zero;
                objIcon.transform.localScale = Vector3.one;
                CharacterOutfitButton outfitButton = objIcon.GetComponent<CharacterOutfitButton>();
                outfitButton.DataBind(AssetFactory.Instance.HeadIcons[i], this, CharacterBodyType.Head, i);
                this._CurrentObjectTabDisplays.Add(objIcon);
            }
            for (int i = 0; i < AssetFactory.Instance.BodyIcons.Length; i++)
            {
                GameObject objIcon = (GameObject)Instantiate(this.CharacterOutfitButtonPrefab);
                objIcon.transform.SetParent(this.BodyIconHolder.transform);
                objIcon.transform.localPosition = Vector3.zero;
                objIcon.transform.localScale = Vector3.one;
                CharacterOutfitButton outfitButton = objIcon.GetComponent<CharacterOutfitButton>();
                outfitButton.DataBind(AssetFactory.Instance.BodyIcons[i], this, CharacterBodyType.Body, i);
                this._CurrentObjectTabDisplays.Add(objIcon);
            }
            for (int i = 0; i < AssetFactory.Instance.LegIcons.Length; i++)
            {
                GameObject objIcon = (GameObject)Instantiate(this.CharacterOutfitButtonPrefab);
                objIcon.transform.SetParent(this.LegsIconHolder.transform);
                objIcon.transform.localPosition = Vector3.zero;
                objIcon.transform.localScale = Vector3.one;
                CharacterOutfitButton outfitButton = objIcon.GetComponent<CharacterOutfitButton>();
                outfitButton.DataBind(AssetFactory.Instance.LegIcons[i], this, CharacterBodyType.Legs, i);
                this._CurrentObjectTabDisplays.Add(objIcon);
            }
        }

        /// <summary>
        /// Clear buttons from slider
        /// </summary>
        public void ClearButtons()
        {
            this._CreatedButtons = false;
            for(int i = 0; i < this._CurrentObjectTabDisplays.Count; i++ )
            {
                if (this._CurrentObjectTabDisplays[i] != null)
                    Destroy(this._CurrentObjectTabDisplays[i]);
                this._CurrentObjectTabDisplays[i] = null;
            }
            this._CurrentObjectTabDisplays = new List<GameObject>();
        }

        /// <summary>
        /// Character Animation Selection
        /// </summary>
        public void CharacterAnimationSelection(AnimationType animationType)
        {
            this._AnimationType = animationType;
            this.Head.SetBool("Idle", AnimationType.Idle == animationType);
            this.Head.SetBool("Duck", AnimationType.Duck == animationType);
            this.Head.SetBool("Run", AnimationType.Run == animationType);
            this.Head.SetBool("Jump", AnimationType.Jump == animationType);
            this.Head.SetBool("Faint", AnimationType.Faint == animationType);

            this.Body.SetBool("Idle", AnimationType.Idle == animationType);
            this.Body.SetBool("Duck", AnimationType.Duck == animationType);
            this.Body.SetBool("Run", AnimationType.Run == animationType);
            this.Body.SetBool("Jump", AnimationType.Jump == animationType);
            this.Body.SetBool("Faint", AnimationType.Faint == animationType);

            this.Legs.SetBool("Idle", AnimationType.Idle == animationType);
            this.Legs.SetBool("Duck", AnimationType.Duck == animationType);
            this.Legs.SetBool("Run", AnimationType.Run == animationType);
            this.Legs.SetBool("Jump", AnimationType.Jump == animationType);
            this.Legs.SetBool("Faint", AnimationType.Faint == animationType);

            this.ArmBehind.SetBool("Idle", AnimationType.Idle == animationType);
            this.ArmBehind.SetBool("Duck", AnimationType.Duck == animationType);
            this.ArmBehind.SetBool("Run", AnimationType.Run == animationType);
            this.ArmBehind.SetBool("Jump", AnimationType.Jump == animationType);
            this.ArmBehind.SetBool("Faint", AnimationType.Faint == animationType);
            
            for(int i = 0; i < this.DisplayAnimationButtons.Length; i++)
            {
                this.DisplayAnimationButtons[i].ButtonToggle(this.DisplayAnimationButtons[i].AnimationType != animationType);
                if (this.DisplayAnimationButtons[i].AnimationType != animationType && this._AnimationType != AnimationType.Idle)
                    this.DisplayAnimationButtons[i].SelectedButton = false;
            }
        }
    }
}