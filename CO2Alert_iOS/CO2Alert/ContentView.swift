//
//  ContentView.swift
//  CO2Alert
//
//  Created by Haruto Fukumoto on 2026/05/28.
//

import SwiftUI

struct ContentView: View {
    @StateObject private var viewModel = MonitorViewModel()

    var body: some View {
        TabView {
            DashboardView(viewModel: viewModel)
                .tabItem {
                    Image(systemName: "gauge.with.dots.needle.bottom.50percent")
                    Text("ダッシュボード")
                }

            MonitorView(viewModel: viewModel)
                .tabItem {
                    Image(systemName: "mappin.and.ellipse")
                    Text("場所別一覧")
                }
        }
    }
}

#Preview {
    ContentView()
}
