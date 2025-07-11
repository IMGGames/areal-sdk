using System;

namespace Areal.SDK.IAP.Stores {
    [Serializable]
    internal class FakeTransaction : ITransaction {
        public string uuid;
        public string productId;

        public string GetProductId() => productId;
    }
}