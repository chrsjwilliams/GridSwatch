using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIProduct : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI nameText;
  [SerializeField] private TextMeshProUGUI desctiptionText;
  [SerializeField] private Image icon;
  [SerializeField] private TextMeshProUGUI priceText;
  [SerializeField] private Button purchaseButton;

  public delegate void PurchaseEvent(Product data, Action OnComplete);

  public event PurchaseEvent OnPurchase;
  
  private Product product;


  public void Setup(Product product)
  {
      this.product = product;
      nameText.text = product.metadata.localizedTitle;
      desctiptionText.text = product.metadata.localizedDescription;
      priceText.SetText($"{product.metadata.localizedPriceString} " + $"{product.metadata.isoCurrencyCode}");

      Texture2D texture = StoreIconProvider.GetIcon(product.definition.id);

      if (texture != null)
      {
          Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);

          icon.sprite = sprite;
      }
      else
      {
          Debug.LogError($"No Sprite found for {product.definition.id}!");
      }
  }

  public void Purchase()
  {
      purchaseButton.enabled = false;
      OnPurchase?.Invoke(product, HandlePurchaseComplete);
  }

  private void HandlePurchaseComplete()
  {
      purchaseButton.enabled = true;
  }
}
