import Foundation

@objc public class UdpRequests: NSObject {
    @objc public func echo(_ value: String) -> String {
        return value
    }
}
