//
//  MonitorView.swift
//  CO2Alert
//
//  Created by Haruto Fukumoto on 2026/05/28.
//

import SwiftUI

struct MonitorView: View {
    @ObservedObject var viewModel: MonitorViewModel

    var body: some View {
        NavigationStack {
            ScrollView {
                VStack {
                    // 監視場所選択
                    locationPicker
                    
                    // 現在のCO2
                    currentCO2Card
                    
                    // AI予測表示
                    predictionCard
                    
                    // 24時間履歴
                    historyCard

                    if let errorMessage = viewModel.errorMessage {
                        errorCard(message: errorMessage)
                    }
                }
                .padding()
            }
            .navigationTitle("CO2 Alert")
            .toolbar {
                ToolbarItem(placement: .topBarTrailing) {
                    Button {
                        Task {
                            await viewModel.refreshAll()
                        }
                    } label: {
                        Image(systemName: "arrow.clockwise")
                    }
                }
            }
            .onAppear {
                viewModel.onAppear()
            }
            .onDisappear {
                viewModel.onDisappear()
            }
        }
    }
    
    private var locationPicker: some View {
        VStack(alignment: .leading) {
            Text("監視場所")
                .font(.headline)

            Picker("監視場所", selection: $viewModel.selectedLocation) {
                ForEach(viewModel.locations, id: \.self) { location in
                    Text(location).tag(location)
                }
            }
            .pickerStyle(.menu)
            .padding()
            .background(Color(.secondarySystemBackground))
            .cornerRadius(12)
            .onChange(of: viewModel.selectedLocation) {
                viewModel.locationChanged()
            }
        }
    }

    private var currentCO2Card: some View {
        VStack {
            Text(viewModel.selectedLocation.isEmpty ? "場所未選択" : viewModel.selectedLocation)
                .font(.headline)

            Text("現在のCO2")
                .font(.subheadline)
                .foregroundStyle(.secondary)

            Text("\(viewModel.currentCO2)")
                .font(.system(size: 64, weight: .bold))
                .foregroundStyle(viewModel.co2Level.color)

            Text("ppm")
                .font(.title2)
                .foregroundStyle(.secondary)

            HStack {
                Circle()
                    .fill(viewModel.co2Level.color)
                    .frame(width: 18, height: 18)

                Text(viewModel.co2Level.title)
                    .font(.title2)
                    .bold()
            }

            Text(viewModel.co2Level.message)
                .font(.body)
                .multilineTextAlignment(.center)
                .foregroundStyle(.secondary)

            Text("最終更新: \(viewModel.lastUpdatedText)")
                .font(.caption)
                .foregroundStyle(.secondary)

            if viewModel.isLoading {
                ProgressView()
            }
        }
        .padding()
        .frame(maxWidth: .infinity)
        .background(cardBackgroundColor)
        .cornerRadius(20)
    }

    private var predictionCard: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text("AI予測")
                .font(.headline)

            HStack(spacing: 12) {
                predictionItem(title: "30分後", value: viewModel.prediction?.predicted30min)
                predictionItem(title: "60分後", value: viewModel.prediction?.predicted60min)
                predictionItem(title: "90分後", value: viewModel.prediction?.predicted90min)
            }

            if viewModel.prediction?.alert == true {
                Text("⚠️ 1000ppm超過の予測があります。換気を検討してください。")
                    .font(.subheadline)
                    .bold()
                    .foregroundStyle(.red)
                    .padding(.top, 8)
            }
        }
        .padding()
        .background(Color(.secondarySystemBackground))
        .cornerRadius(16)
    }

    private func predictionItem(title: String, value: Double?) -> some View {
        VStack {
            Text(title)
                .font(.caption)
                .foregroundStyle(.secondary)

            if let value {
                Text("\(Int(value))")
                    .font(.title2)
                    .bold()
                Text("ppm")
                    .font(.caption)
                    .foregroundStyle(.secondary)
            } else {
                Text("未計算")
                    .font(.subheadline)
                    .foregroundStyle(.secondary)
            }
        }
        .frame(maxWidth: .infinity)
        .padding()
        .background(Color(.systemBackground))
        .cornerRadius(12)
    }

    private var historyCard: some View {
        VStack(alignment: .leading) {
            HStack {
                Text("24時間履歴")
                    .font(.headline)

                Spacer()

                Text("\(viewModel.history.count)件")
                    .font(.caption)
                    .foregroundStyle(.secondary)
            }

            if viewModel.history.isEmpty {
                Text("履歴データがありません。")
                    .foregroundStyle(.secondary)
            } else {
                ForEach(viewModel.history.suffix(10)) { item in
                    HStack {
                        VStack(alignment: .leading) {
                            Text(formatDate(item.created))
                                .font(.caption)
                                .foregroundStyle(.secondary)

                            Text("\(Int(item.co2)) ppm")
                                .font(.body)
                                .bold()
                        }

                        Spacer()

                        if let temperature = item.temperature {
                            Text(String(format: "%.1f℃", temperature))
                                .font(.caption)
                                .foregroundStyle(.secondary)
                        }
                    }

                    Divider()
                }
            }
        }
        .padding()
        .background(Color(.secondarySystemBackground))
        .cornerRadius(16)
    }

    private func errorCard(message: String) -> some View {
        Text(message)
            .font(.subheadline)
            .foregroundStyle(.red)
            .padding()
            .frame(maxWidth: .infinity)
            .background(Color.red.opacity(0.1))
            .cornerRadius(12)
    }

    private var cardBackgroundColor: Color {
        viewModel.co2Level.color.opacity(0.15)
    }

    private func formatDate(_ text: String) -> String {
        let inputFormatter = DateFormatter()
        inputFormatter.locale = Locale(identifier: "ja_JP")
        inputFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"

        let outputFormatter = DateFormatter()
        outputFormatter.locale = Locale(identifier: "ja_JP")
        outputFormatter.dateFormat = "MM/dd HH:mm"

        if let date = inputFormatter.date(from: text) {
            return outputFormatter.string(from: date)
        }

        return text
    }
}
