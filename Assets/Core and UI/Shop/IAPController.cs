using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


namespace IAP_Dev
{
    [System.Serializable]
    public class ItemIap
    {
        public string key;
        public ProductType productType;
    }

    public class IAPController : Singleton<IAPController>
    {
        [Header("key infomation : ")]
        public string primaryKEY = "";
        public int amount;

        StoreController m_StoreController;
        [SerializeField] private List<ItemIap> lstKeyCode;

        [SerializeField]  string GameID;
        [SerializeField]  string GameName;
        // [SerializeField] private bool isInitialized = false;
        public Action<bool> OnPurchaseSuccess;

        private void Start()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("No internet connection.");
                GameDataLoader.instance.ShowPopupNetworkError();
                GameDataLoader.instance.disabledStatus = true;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Debug.Log("Connected via Wi-Fi or LAN.");
                GameDataLoader.instance.CheckNetwork();
                if (GameDataLoader.instance.disabledStatus) return;
                StartCoroutine(InitializeIAPPack());

                // InitializeIAP();
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                Debug.Log("Connected via mobile data.");
                GameDataLoader.instance.CheckNetwork();
                StartCoroutine(InitializeIAPPack());
                // InitializeIAP();
            }
        }

        IEnumerator InitializeIAPPack()
        {
            yield return new WaitForSeconds(.5f);
            if (GameDataLoader.instance.disabledStatus == false)
            {
                InitializeIAP();
                yield return new WaitForSeconds(.5f);

            }
        }

        async void InitializeIAP()
        {
            m_StoreController = UnityIAPServices.StoreController();
            m_StoreController.OnPurchasePending += OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed += OnPurchaseFailed;

            m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
            Debug.Log("Connecting to store.");
            await m_StoreController.Connect();

            m_StoreController.OnProductsFetchFailed += OnProductsFetchedFailed;
            m_StoreController.OnProductsFetched += OnProductsFetched;

            m_StoreController.OnPurchaseDeferred += OnPurchaseDeferred;
            FetchProducts();
        }

        void FetchProducts()
        {
            var initialProductsToFetch = new List<ProductDefinition>();
            for (int i = 0; i < lstKeyCode.Count; i++)
            {
                initialProductsToFetch.Add(new(lstKeyCode[i].key, lstKeyCode[i].productType));
            }

            m_StoreController.FetchProducts(initialProductsToFetch);
        }

        void OnPurchaseFailed(FailedOrder order)
        {
            var product = GetFirstProductInOrder(order);
            if (product == null)
            {
                Debug.Log("Could not find product in failed order.");
            }

            OnPurchaseSuccess?.Invoke(false);
            Debug.Log($"Purchase failed - Product: '{product?.definition.id}'," +
                      $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                      + $"Purchase Failure Details: {order.Details}");
        }


        
        private readonly HashSet<string> verifyingTransactions = new();
        private void OnPurchasePending(PendingOrder order)
        {
            Debug.Log("[IAP] OnPurchasePending called.");

            try
            {
                Product product = GetFirstProductInOrder(order);

                if (product == null)
                {
                    Debug.LogError("[IAP] Product not found.");
                    FinishPurchase(false);
                    return;
                }

                if (order.Info == null)
                {
                    Debug.LogError("[IAP] Order info is null.");
                    FinishPurchase(false);
                    return;
                }

                string productId = product.definition.id;
                string transactionId = order.Info.TransactionID;
                string receipt = order.Info.Receipt;

                if (string.IsNullOrWhiteSpace(transactionId))
                {
                    Debug.LogError("[IAP] TransactionId is empty.");
                    FinishPurchase(false);
                    return;
                }

                // Store mua thành công, không phụ thuộc API.
                m_StoreController.ConfirmPurchase(order);

                FinishPurchase(true);

                // API chỉ ghi nhận độc lập.
                if (string.IsNullOrWhiteSpace(receipt))
                {
                    Debug.LogWarning(
                        $"[IAP API] Receipt is empty. API request skipped. " +
                        $"TransactionId={transactionId}"
                    );
                    return;
                }

                SendPurchaseToApi(
                    product,
                    transactionId,
                    receipt
                );
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                FinishPurchase(false);
            }
        }

        private void SendPurchaseToApi(
    Product product,
    string transactionId,
    string receipt)
        {
            if (purchaseApiService == null)
            {
                Debug.LogWarning("[IAP API] PurchaseApiService is null.");
                return;
            }

            if (!verifyingTransactions.Add(transactionId))
            {
                Debug.LogWarning(
                    $"[IAP API] Transaction is already being sent: {transactionId}"
                );
                return;
            }

            double price;

            try
            {
                price = Convert.ToDouble(product.metadata.localizedPrice);
            }
            catch (Exception exception)
            {
                verifyingTransactions.Remove(transactionId);

                Debug.LogWarning(
                    $"[IAP API] Cannot convert localized price. " +
                    $"TransactionId={transactionId}, Error={exception.Message}"
                );
                return;
            }

            StartCoroutine(
                purchaseApiService.VerifyPurchase(
                    "Sweet Sort",
                    product.definition.id,
                    transactionId,
                    price,
                    product.metadata.isoCurrencyCode,
                    GetCurrentPlatform(),
                    receipt,
                    response =>
                    {
                        verifyingTransactions.Remove(transactionId);

                        if (response == null)
                        {
                            Debug.LogWarning(
                                $"[IAP API] Backend response is null. " +
                                $"TransactionId={transactionId}"
                            );
                            return;
                        }

                        Debug.Log(
                            $"[IAP API] Backend recorded transaction: {transactionId}"
                        );
                    },
                    error =>
                    {
                        verifyingTransactions.Remove(transactionId);

                        Debug.LogWarning(
                            $"[IAP API] Request failed. " +
                            $"TransactionId={transactionId}, Error={error}"
                        );

                        // Không gọi FinishPurchase(false).
                        // Không ảnh hưởng kết quả Store.
                    }
                )
            );
        }
        void OnPurchaseConfirmed(Order order)
        {
            switch (order)
            {
                case ConfirmedOrder confirmedOrder:
                    OnPurchaseConfirmed(confirmedOrder);
                    break;
                case FailedOrder failedOrder:
                    OnPurchaseConfirmationFailed(failedOrder);
                    break;
                default:
                    Debug.Log("Unknown OnPurchaseConfirmed result.");
                    break;
            }
        }
        private void OnPurchaseConfirmed(ConfirmedOrder order)
        {
            var product = GetFirstProductInOrder(order);

            if (product == null)
            {
                Debug.LogError("[IAP] Product not found in confirmed order.");
                FinishPurchase(false);
                return;
            }

            Debug.Log($"[IAP] Purchase confirmed: {product.definition.id}");

            FinishPurchase(true);
        }
        private void FinishPurchase(bool success)
        {
            var callback = OnPurchaseSuccess;
            OnPurchaseSuccess = null;
            callback?.Invoke(success);
        }

        void OnPurchaseConfirmationFailed(FailedOrder order)
        {
            var product = GetFirstProductInOrder(order);
            if (product == null)
            {
                Debug.Log("Could not find product in failed confirmation.");
            }

            OnPurchaseSuccess?.Invoke(false);
            Debug.Log($"Confirmation failed - Product: '{product?.definition.id}'," +
                      $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                      + $"Confirmation Failure Details: {order.Details}");
        }

        private Product GetFirstProductInOrder(Order order)
        {
            if (order?.CartOrdered == null)
            {
                return null;
            }

            return order.CartOrdered
                .Items()
                .FirstOrDefault()
                ?.Product;
        }

        // Calling StoreController.Connect without a listener on the StoreController.OnStoreDisconnected event will result in warnings.
        void OnStoreDisconnected(StoreConnectionFailureDescription description)
        {
            Debug.Log($"Store disconnected details: {description.message}");
        }

        // Calling StoreController.Connect without listeners on StoreController.OnProductsFetched and StoreController.OnProductsFetchedFailed will result in warnings.
        void OnProductsFetched(List<Product> products)
        {
            Debug.Log($"Products fetched successfully for {products.Count} products.");
        }

        void OnProductsFetchedFailed(ProductFetchFailed failure)
        {
            Debug.Log($"Products fetch failed for {failure.FailedFetchProducts.Count} products: {failure.FailureReason}");
        }

        public void BuyProduct(string productId, Action<bool> success)
        {
            OnPurchaseSuccess = success;
            m_StoreController?.PurchaseProduct(productId);
        }

        public string GetPriceValue(string productId)
        {
            var product = m_StoreController?.GetProducts().FirstOrDefault(p => p.definition.id == productId);
            if (product != null)
            {
                return product.metadata.localizedPriceString;
            }
            else
            {
                Debug.Log("Product not found");
                return "Loading...";
            }
        }
        void OnPurchaseDeferred(DeferredOrder order)
        {
            var product = GetFirstProductInOrder(order);
            Debug.Log($"Purchase deferred - Product: {product?.definition.id}");
        }

     

        public void CreateKeyCode()
        {
            lstKeyCode.Clear();
            if (lstKeyCode.Any(k => k.key == primaryKEY))
            {
                Debug.Log("Key already exists.");
                return;
            }
            for (int i = 0; i < amount; i++)
            {
                lstKeyCode.Add(new ItemIap { key = primaryKEY + "_pack_" + (i+1), productType = ProductType.Consumable });

            }
        }

        public void GetGameInfor()
        {
             GameID = Application.identifier;
             GameName = Application.productName;
        }

        [SerializeField]
        private PurchaseApiService purchaseApiService;

        public void SubmitPurchaseResult(
            string gameCode,
            string productId,
            string transactionId,
            double price,
            string currency,
            PurchasePlatform platform,
            string receiptData)
        {
            StartCoroutine(
                purchaseApiService.VerifyPurchase(
                    gameCode,
                    productId,
                    transactionId,
                    price,
                    currency,
                    platform,
                    receiptData,
                    OnVerifyPurchaseSuccess,
                    OnVerifyPurchaseFailed
                )
            );
        }

        private void OnVerifyPurchaseSuccess(
    VerifyPurchaseResponse response)
        {
            Debug.Log("Xác minh giao dịch thành công.");

            if (response.data != null)
            {
                Debug.Log(
                    $"Transaction ID: {response.data.transactionId}"
                );

                Debug.Log(
                    $"Product ID: {response.data.productId}"
                );
            }

            // Chỉ cộng vật phẩm hoặc cập nhật UI
            // sau khi backend xác minh thành công.
            //RefreshPlayerInventory();
        }

        private void OnVerifyPurchaseFailed(string error)
        {
            Debug.LogError(
                $"Xác minh giao dịch thất bại: {error}"
            );
        }

        private PurchasePlatform GetCurrentPlatform()
        {
#if UNITY_IOS
    return PurchasePlatform.IOS;
#elif UNITY_ANDROID
            return PurchasePlatform.ANDROID;
#else
    return PurchasePlatform.WEB;
#endif
        }

        private void OnBackendVerifySuccess(
           PendingOrder order,
           VerifyPurchaseResponse response)
        {
            Debug.Log("Backend xác minh giao dịch thành công.");

            // Backend đã xử lý và cộng tiền.
            // Không cộng tiền local tại đây.

            m_StoreController.ConfirmPurchase(order);
        }

        private void OnBackendVerifyFailed(string error)
        {
            Debug.LogError(
                $"Backend purchase verification failed: {error}"
            );

            OnPurchaseSuccess?.Invoke(false);

            // Không ConfirmPurchase ở đây.
            // PendingOrder có thể được trả lại khi khởi động IAP lần sau
            // để bạn thử xác minh lại.
        }
    }
}
