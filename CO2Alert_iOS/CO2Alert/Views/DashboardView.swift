import SwiftUI
import Charts

struct DashboardView: View {
    @ObservedObject var viewModel: MonitorViewModel
    @State private var selectedChartLocation: String = ""

    private var highCO2Items: [CO2DataItem] {
        viewModel.latestByLocation
            .sorted { $0.co2 > $1.co2 }
    }

    private var chartData: [CO2DataItem] {
        viewModel.allData
            .filter { item in
                guard !selectedChartLocation.isEmpty else {
                    return true
                }

                return item.location == selectedChartLocation
            }
            .sorted { $0.createdDate < $1.createdDate }
            .suffix(200)
    }

    var body: some View {
        NavigationStack {
            ScrollView {
                VStack(spacing: 16) {
                    co2ChartCard
                    highCO2Card

                    if let errorMessage = viewModel.errorMessage {
                        errorCard(message: errorMessage)
                    }
                }
                .padding()
            }
            .navigationTitle("ダッシュボード")
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
            .refreshable {
                await viewModel.refreshAll()
            }
            .task {
                await viewModel.refreshAll()

                if selectedChartLocation.isEmpty {
                    selectedChartLocation = viewModel.locations.first ?? ""
                }
            }
        }
    }

    private var co2ChartCard: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text("場所別CO2グラフ")
                .font(.headline)

            Picker("場所", selection: $selectedChartLocation) {
                ForEach(viewModel.locations, id: \.self) { location in
                    Text(viewModel.displayName(for: location))
                        .tag(location)
                }
            }
            .pickerStyle(.menu)

            if chartData.isEmpty {
                Text("グラフ表示できるデータがありません。")
                    .foregroundStyle(.secondary)
                    .frame(maxWidth: .infinity, minHeight: 180)
            } else {
                Chart {
                    ForEach(chartData) { item in
                        LineMark(
                            x: .value("時刻", item.createdDate),
                            y: .value("CO2", item.co2)
                        )
                    }
                }
                .frame(height: 260)
            }

            Text("選択した場所の最新200件を表示")
                .font(.caption)
                .foregroundStyle(.secondary)
        }
        .padding()
        .background(Color(.secondarySystemBackground))
        .cornerRadius(16)
    }

    private var highCO2Card: some View {
        VStack(alignment: .leading, spacing: 12) {
            Text("現在CO2が高い場所")
                .font(.headline)

            if highCO2Items.isEmpty {
                Text("データがありません。")
                    .foregroundStyle(.secondary)
            } else {
                ForEach(highCO2Items) { item in
                    VStack(alignment: .leading, spacing: 8) {
                        HStack {
                            Text(viewModel.displayName(for: item.location ?? ""))
                                .font(.headline)

                            Spacer()

                            VStack(alignment: .trailing, spacing: 4) {
                                Text("\(Int(item.co2)) ppm")
                                    .font(.title3)
                                    .bold()
                                    .foregroundStyle(CO2Level.from(item.co2).color)

                                Text("WBGT \(viewModel.wbgtText(for: item))")
                                    .font(.caption)
                                    .bold()
                            }
                        }

                        HStack {
                            if let temperature = item.temperature {
                                Text(String(format: "温度 %.1f℃", temperature))
                            }

                            if let humidity = item.humidity {
                                Text(String(format: "湿度 %.1f%%", humidity))
                            }
                        }
                        .font(.caption)
                        .foregroundStyle(.secondary)

                        Text(item.created)
                            .font(.caption2)
                            .foregroundStyle(.secondary)
                    }
                    .padding()
                    .background(Color(.systemBackground))
                    .cornerRadius(12)
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
}
