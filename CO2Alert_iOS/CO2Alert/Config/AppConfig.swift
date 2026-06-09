//
//  AppConfig.swift
//  CO2Alert
//
//  Created by Haruto Fukumoto on 2026/05/29.
//

import Foundation

// 本アプリの総設定
enum AppConfig {
    // データ元
    static let apiBaseURL = "https://carbon25.apps.kyusan-u.ac.jp/fukumoto/api/"
    
    // データ取得インターバル
    static let autoRefreshInterval: TimeInterval = 30
    
    // CO2危険度範囲設定
    static let safeThreshold = 0
    static let cautionThreshold = 800
    static let warningThreshold = 1000
    static let dangerThreshold = 1500
    static let emergencyThreshold = 2000
}
