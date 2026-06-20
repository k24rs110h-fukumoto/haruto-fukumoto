//
//  ContentView.swift
//  SelfIntroduction
//
//  Created by Haruto Fukumoto on 2026/04/24.
//

import SwiftUI

struct ContentView: View {
    @State var schedules: [ScheduleItem] = []
    @State var year = Calendar.current.component(.year, from: Date())
    @State var month = Calendar.current.component(.month, from: Date())
    @State var day = Calendar.current.component(.day, from: Date())
    
    var body: some View{
        TabView {
            // カレンダーページ
            SelfIntroductionView()
                .tabItem {
                    Image(systemName: "person")
                    Text("自己紹介")
                }
            // カレンダーページ
            CalendarView(
                schedules: $schedules,
                year: $year,
                month: $month,
                day: $day
            )
                .tabItem {
                    Image(systemName: "calendar")
                    Text("カレンダー")
                }
            // リストページ
            ListView(schedules: $schedules)
                .tabItem {
                    Image(systemName: "list.bullet")
                    Text("リスト")
                }
            // 通知ページ
            NotificationView(
                            schedules: $schedules,
                            year: $year,
                            month: $month,
                            day: $day
                        )
                .tabItem {
                    Image(systemName: "bell")
                    Text("通知")
                }
        }
    }
}

#Preview {
    ContentView()
}
