//
//  MonitorViewModel.swift
//  CO2Alert
//
//  Created by Haruto Fukumoto on 2026/05/28.
//

import Foundation
import Combine

@MainActor
final class MonitorViewModel: ObservableObject {
    @Published var locations: [String] = []
    @Published var selectedLocation: String = ""
    @Published var prediction: PredictionResponse?
    @Published var history: [CO2DataItem] = []

    @Published var allData: [CO2DataItem] = []

    @Published var isLoading = false
    @Published var errorMessage: String?
    @Published var lastUpdatedText = "未更新"

    private var timer: Timer?

    var currentCO2: Int {
        prediction?.currentCO2 ?? 0
    }

    var co2Level: CO2Level {
        CO2Level.from(currentCO2)
    }

    var hasPrediction: Bool {
        prediction != nil
    }

    var latestByLocation: [CO2DataItem] {
        let grouped = Dictionary(grouping: allData) { item in
            item.location ?? "不明"
        }

        return grouped.compactMap { _, items in
            items.sorted { $0.createdDate > $1.createdDate }.first
        }
    }

    func onAppear() {
        Task {
            await loadInitialData()
        }

        startAutoRefresh()
    }

    func onDisappear() {
        stopAutoRefresh()
    }

    func loadInitialData() async {
        isLoading = true
        errorMessage = nil

        do {
            let fetchedLocations = try await CO2APIService.shared.fetchLocations()
            locations = fetchedLocations

            if selectedLocation.isEmpty {
                selectedLocation = fetchedLocations.first ?? "EX00002"
            }

            await refreshAll()
        } catch {
            errorMessage = error.localizedDescription
        }

        isLoading = false
    }

    func refreshAll() async {
        guard !selectedLocation.isEmpty else {
            return
        }

        await fetchPrediction()
        await fetchHistory()
        await fetchDashboardData()
        updateLastUpdatedText()
    }

    func fetchPrediction() async {
        do {
            prediction = try await CO2APIService.shared.fetchPrediction(location: selectedLocation)
            errorMessage = nil
        } catch {
            errorMessage = error.localizedDescription
        }
    }

    func fetchHistory() async {
        do {
            history = try await CO2APIService.shared.fetchCO2Data(
                location: selectedLocation,
                period: 24
            )
            errorMessage = nil
        } catch {
            errorMessage = error.localizedDescription
        }
    }

    func fetchDashboardData() async {
        var fetchedData: [CO2DataItem] = []

        for location in locations {
            do {
                let data = try await CO2APIService.shared.fetchCO2Data(
                    location: location,
                    period: 24
                )

                fetchedData.append(contentsOf: data)
            } catch {
                errorMessage = error.localizedDescription
            }
        }

        allData = fetchedData
    }

    func locationChanged() {
        Task {
            await refreshAll()
        }
    }

    func displayName(for location: String) -> String {
        if location.isEmpty {
            return "不明"
        }

        return location
    }

    func wbgtText(for item: CO2DataItem) -> String {
        guard let temperature = item.temperature,
              let humidity = item.humidity else {
            return "--"
        }

        let wbgt = 0.7 * temperature + 0.2 * humidity / 10.0 + 0.1 * temperature
        return String(format: "%.1f℃", wbgt)
    }

    func startAutoRefresh() {
        stopAutoRefresh()

        timer = Timer.scheduledTimer(withTimeInterval: 30, repeats: true) { [weak self] _ in
            Task { @MainActor in
                await self?.refreshAll()
            }
        }
    }

    func stopAutoRefresh() {
        timer?.invalidate()
        timer = nil
    }

    private func updateLastUpdatedText() {
        let formatter = DateFormatter()
        formatter.locale = Locale(identifier: "ja_JP")
        formatter.dateFormat = "HH:mm:ss"
        lastUpdatedText = formatter.string(from: Date())
    }
}
