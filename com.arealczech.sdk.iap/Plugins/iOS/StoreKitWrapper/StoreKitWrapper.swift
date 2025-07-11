import Foundation
import StoreKit

// Structs

public struct PurchaseResult: Codable {
    let ok: Bool
    let productId: String
    let message: String?

    enum CodingKeys: String, CodingKey {
        case ok, productId, message
    }

    public func encode(to encoder: Encoder) throws {
        var container = encoder.container(keyedBy: CodingKeys.self)
        try container.encode(ok, forKey: .ok)
        try container.encode(productId, forKey: .productId)

        if !ok {
            try container.encode(message ?? "unknown error", forKey: .message)
        }
    }
}

public struct InitializeResult: Codable {
    let ok: Bool
    let products: [ProductInfo]?
    let message: String?
}

public struct ProductInfo: Codable {
    let id: String
    let name: String

    let localPrice: Decimal
    let localCurrencyIso: String
}

// Unity bridge

@_silgen_name("UnitySendMessage")
func UnitySendMessage(
    _ obj: UnsafePointer<CChar>, _ method: UnsafePointer<CChar>, _ message: UnsafePointer<CChar>)

func sendUnityMessage(method: String, message: String) {
    "__Areal.SDK.IAP.MessageReceiver".withCString { objPtr in
        method.withCString { methodPtr in
            message.withCString { msgPtr in
                UnitySendMessage(objPtr, methodPtr, msgPtr)
            }
        }
    }
}

func toJson(_ data: Codable) throws -> String {
    guard let json = String(data: try JSONEncoder().encode(data), encoding: .utf8) else {
        throw NSError(
            domain: "EncodingError", code: 0,
            userInfo: [NSLocalizedDescriptionKey: "Failed to convert JSON data to String"])
    }

    return json
}

func sendPurchaseResult(_ result: PurchaseResult) throws {
    sendUnityMessage(method: "OnPurchaseResultReceived", message: try toJson(result))
}

// StoreKitManager

@objc public class StoreKitManager: NSObject {
    static let shared = StoreKitManager()

    public func purchase(productId: String) throws {
        try sendPurchaseResult(PurchaseResult(ok: true, productId: productId, message: nil))
    }
}

// @_cdecl("StoreKitWrapper_")
// public func test() -> UnsafePointer<CChar> {
//     return UnsafePointer(strdup("test"))
// }

// @_cdecl("StoreKitWrapper_free")
// public func freeMemory(_ ptr: UnsafeMutableRawPointer) {
//     free(ptr)
// }
