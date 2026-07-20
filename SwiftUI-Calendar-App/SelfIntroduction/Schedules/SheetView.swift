//
//  SheetView.swift
//  SelfIntroduction
//
//  Created by Haruto Fukumoto on 2026/04/28.
//

import SwiftUI

struct SheetView: View {
    @Binding var schedules: [ScheduleItem]
    @Binding var isScheduleSheet: Bool
    @Binding var selectedDay: Int
    @Binding var scheduleText: String
    @Binding var startTime: String
    @Binding var endTime: String
    @Binding var year: Int
    @Binding var month: Int
    
    var times = generateTimes()
    var body: some View {
        // 予定追加ポップ
            VStack(spacing: 20) {
                Text("\(year)年 \(month)月 \(selectedDay)日の予定")
                    .font(.title2)
                    .bold()
                    .padding(.top)
                Text("開始時間")
                Picker("開始時間", selection: $startTime) {
                    ForEach(times, id: \.self) { time in
                        Text(time)
                    }
                }.tint(.black)
                Text("終了時間")
                Picker("終了時間", selection: $endTime) {
                    ForEach(times, id: \.self) { time in
                        Text(time)
                            // .foregroundStyle(.black)
                    }
                }.tint(.black)
                
                TextField("予定を入力", text: $scheduleText)
                    .textFieldStyle(.roundedBorder)
                    .padding()
                
                Button {
                    let newSchedule = ScheduleItem(
                        year: year,
                        month: month,
                        day: selectedDay,
                        startTime: startTime,
                        endTime: endTime,
                        text: scheduleText
                    )
                    
                    // サーバー保存
                    saveSchedule(newSchedule)
                    // ローカル保存
                    schedules.append(newSchedule)
                    isScheduleSheet = false
                } label: {
                    Text("保存")
                        .frame(maxWidth: .infinity)
                        .padding()
                        .background(Color.blue)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                }
                .padding()
                
                Button("キャンセル") {
                    isScheduleSheet = false
                }.foregroundStyle(.black)
                
                Spacer()
            }
        
    }
}

func generateTimes() -> [String] {
    var times: [String] = []
    
    for hour in 0..<24 {
        times.append(String(format: "%02d:00", hour))
        times.append(String(format: "%02d:30", hour))
    }
    return times
}

func saveSchedule(_ schedule: ScheduleItem) {
    guard let url = URL(string: "http://133.17.158.63/server.php") else {
        return
    }
    // リクエスト作成
    var request = URLRequest(url: url)
    // POST送信
    request.httpMethod = "POST"
    // ヘッダー設定
    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
    // データ変換
    request.httpBody = try? JSONEncoder().encode(schedule)
    
    URLSession.shared.dataTask(with: request) { data, response, error in
        if let error = error {
            print("保存失敗", error)
            return
        }
        
        print("保存成功")
    }.resume()
    
//    guard let url = URL(string: "http://133.17.158.63:8000/server.php") else {
//        return
//    }
//    var request = URLRequest(url: url)
//    request.httpMethod = "POST"
//    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
//    
//    do {
//        request.httpBody = try JSONEncoder().encode(schedule)
//        let (_, response) = try await URLSession.shared.data(for: request)
//        print("保存成功")
//    } catch {
//        print("保存失敗", error)
//    }
}
