using System;

namespace Areal.SDK.IAP {
    [Serializable]
    internal class FakeTransaction : ITransaction {
        public string uuid;
        public string productId;

        public string GetProductId() => productId;
    }
}