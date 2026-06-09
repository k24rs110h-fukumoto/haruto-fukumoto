//
//  CO2Models.swift
//  CO2Alert
//
//  Created by Haruto Fukumoto on 2026/05/29.
//

import Foundation
import SwiftUI

struct LocationResponse: Codable {
    let locations: [String]
}

struct PredictionResponse: Codable {
    let location: String
    let currentCO2: Int
    let predicted30min: Double?
    let predicted60min: Double?
    let predicted90min: Double?
    let alert: Bool

    enum CodingKeys: String, CodingKey {
        case location
        case currentCO2 = "current_co2"
        case predicted30min = "predicted_30min"
        case predicted60min = "predicted_60min"
        case predicted90min = "predicted_90min"
        case alert
    }
}

struct CO2DataItem: Codable, Identifiable {
    let id = UUID()
    let created: String
    let location: String?
    let co2: Double
    let temperature: Double?
    let humidity: Double?
    let pressure: Double?

    enum CodingKeys: String, CodingKey {
        case created
        case location
        case co2
        case temperature
        case humidity
        case pressure
    }

    var createdDate: Date {
        let formatter = DateFormatter()
        formatter.locale = Locale(identifier: "ja_JP")
        formatter.dateFormat = "yyyy-MM-dd HH:mm:ss"

        return formatter.date(from: created) ?? Date()
    }
}

enum CO2Level {
    case safe
    case caution
    case warning
    case danger
    case emergency

    static func from(_ ppm: Int) -> CO2Level {
        from(Double(ppm))
    }

    static func from(_ ppm: Double) -> CO2Level {
        switch ppm {
        case Double(AppConfig.safeThreshold)..<Double(AppConfig.cautionThreshold):
            return .safe
        case Double(AppConfig.cautionThreshold)..<Double(AppConfig.warningThreshold):
            return .caution
        case Double(AppConfig.warningThreshold)..<Double(AppConfig.dangerThreshold):
            return .warning
        case Double(AppConfig.dangerThreshold)..<Double(AppConfig.emergencyThreshold):
            return .danger
        default:
            return .emergency
        }
    }

    var title: String {
        switch self {
        case .safe:
            return "安全"
        case .caution:
            return "注意"
        case .warning:
            return "換気推奨"
        case .danger:
            return "危険"
        case .emergency:
            return "緊急"
        }
    }

    var message: String {
        switch self {
        case .safe:
            return "安全です"
        case .caution:
            return "注意が必要です"
        case .warning:
            return "換気が必要です"
        case .danger:
            return "危険です"
        case .emergency:
            return "緊急です"
        }
    }

    var color: Color {
        switch self {
        case .safe:
            return .green
        case .caution:
            return .yellow
        case .warning:
            return .orange
        case .danger, .emergency:
            return .red
        }
    }
}
