using System;
using System.Collections;
using System.Collections.Generic;
using CarrotHood.PlatformGateway;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Examples
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private Text _id;
        [SerializeField] private Text _name;
        [SerializeField] private Image _photo;
        [SerializeField] private Button _authorizeButton;
        [SerializeField] private GameObject _overlay;

        private void Start()
        {
            _overlay.SetActive(true);
            _authorizeButton.onClick.AddListener(OnAuthorizeButtonClicked);
            StartCoroutine(GetImage(x =>
            {
                UpdateVisuals(x);
                _overlay.SetActive(false);
            }));
        }

        private void OnAuthorizeButtonClicked()
        {
            StartCoroutine(Authorize());
        }

        private IEnumerator Authorize()
        {
            _overlay.SetActive(true);
            
            yield return PlatformGateway.Account.GetPlayerData();
            yield return GetImage(UpdateVisuals);
            
            _overlay.SetActive(false);
        }

        private IEnumerator GetImage(Action<Sprite> callback)
        {
            yield return Utils.DownloadSprite(PlatformGateway.Account.Player.profilePictureUrl, texture2D =>
            {
                callback?.Invoke(Utils.TextureToSprite(texture2D));
            });
        }

        private void UpdateVisuals(Sprite photoSprite)
        {
            _id.text = PlatformGateway.Account.Player.id;
            _name.text = PlatformGateway.Account.Player.name;
            _photo.sprite = photoSprite;
        }
    }
}