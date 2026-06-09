//
//  CalendarView.swift
//  SelfIntroduction
//
//  Created by Haruto Fukumoto on 2026/04/26.
//

import Foundation
import SwiftUI

// Identifiableデータを識別できるように
// Codeable 保存読み込みができるように
struct ScheduleItem: Identifiable, Codable {
    var id = UUID()
    let year: Int
    let month: Int
    let day: Int
    let startTime: String
    let endTime: String
    let text: String
    
    // 保存するときに使うキーの指定
    enum CodingKeys: String, CodingKey {
        case year
        case month
        case day
        case startTime
        case endTime
        case text
    }
}

// カレンダーページを表示
struct CalendarView: View {
    
    @State var currentDate = Date()
    
    @State private var isScheduleSheet = false
    @State private var selectedDay = 1
    @State private var scheduleText = ""
    @State private var startTime = "00:00"
    @State private var endTime = "00:30"
    
    @Binding var schedules: [ScheduleItem]
    @Binding var year: Int
    @Binding var month: Int
    @Binding var day: Int
    
    let days: [String] = ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"]
    let calendar = Calendar.current
    
    var body: some View {
        GeometryReader { geometry in
            
            if let newdate = calendar.date(from: DateComponents(year: year, month: month)),
               let firstDay = calendar.date(from: DateComponents(year: year, month: month, day: 1)),
               let range = calendar.range(of: .day, in: .month, for: newdate) {
                
                let newdates = range.count
                let newday = calendar.component(.weekday, from: firstDay)
                let dates = Array(1...newdates)
                let screenWidth = geometry.size.width
                      
                VStack {
                    HStack {
                        Button {
                            if month == 1 {
                                year = year - 1
                                month = 12
                            } else {
                                month = month - 1
                            }
                            changeMonth(-1)
                        } label: {
                            Text("<")
                                .padding()
                                .font(.title)
                        }
                        
                        Spacer()
                        
                        Button {
                            if month == 12 {
                                year = year + 1
                                month = 1
                            } else {
                                month = month + 1
                            }
                            changeMonth(1)
                        } label: {
                            Text(">")
                                .padding()
                                .font(.title)
                        }
                    }
                    MonthView(currentDate: $currentDate)
                    Text("Self Introduction")
                    Text(String(year))
                        .padding(.bottom)
                    // 曜日表示
                    HStack {
                        ForEach(days, id: \.self){ day in
                            Text(day).frame(maxWidth: .infinity, alignment: .center)
                        }
                    }
                    // 日付表示
                    VStack(spacing: 0) {
                        ForEach(0...5, id: \.self) { i in
                            HStack(spacing: 0) {
                                ForEach(1...7, id: \.self) { j in
                                    let index = i * 7 + j - newday
                                    
                                    if index >= 0 && index < dates.count {
                                        Button {
                                            selectedDay = dates[index]
                                            scheduleText = ""
                                            isScheduleSheet = true
                                        } label: {
                                            Text("\(dates[index])")
                                                .frame(width: screenWidth/7, height: screenWidth/7)
                                                .foregroundStyle(.black)
                                                .background(Color.accentColor)
                                                .clipShape(Circle())
                                            
                                        }
                                    } else {
                                        Text("")
                                            .frame(width: screenWidth/7, height: screenWidth/7)
                                    }
                                }
                            }
                        }
                    }
                }
                
                // 予定追加ポップ
                .sheet(isPresented: $isScheduleSheet) {
                    SheetView(
                        schedules: $schedules,
                        isScheduleSheet: $isScheduleSheet,
                        selectedDay: $selectedDay,
                        scheduleText: $scheduleText,
                        startTime: $startTime,
                        endTime: $endTime,
                        year: $year,
                        month: $month
                    )
                }.frame(width: geometry.size.width, height: geometry.size.height)
            }
            
        }
    }
    
    func changeMonth(_ value: Int) {
        if let newDate = calendar.date(byAdding: .month, value: value, to: currentDate) {
            currentDate = newDate
        }
    }
}
