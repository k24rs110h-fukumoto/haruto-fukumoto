//
//  NotificationView.swift
//  SelfIntroduction
//
//  Created by Haruto Fukumoto on 2026/04/28.
//

import SwiftUI

struct NotificationView: View {
    @Binding var schedules: [ScheduleItem]
    @Binding var year: Int
    @Binding var month: Int
    @Binding var day: Int
    
    var todaySchedules: [ScheduleItem] {
        schedules
            .filter {
                $0.year == year &&
                $0.month == month &&
                $0.day == day
            }
            .sorted {
                $0.startTime < $1.startTime
            }
    }
    
    var body: some View {
        List {
            ForEach(todaySchedules) { item in
                VStack(alignment: .leading, spacing: 6) {
                    
                    Text("\(item.year)年\(item.month)月\(item.day)日")
                        .font(.headline)
                    
                    Text("\(item.startTime)〜\(item.endTime)")
                        .font(.caption)
                    
                    Text(item.text)
                        .font(.body)
                }
                .padding(8)
            }
        }
    }
}
