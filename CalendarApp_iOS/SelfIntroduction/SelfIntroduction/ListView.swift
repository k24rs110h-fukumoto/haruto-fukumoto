import Foundation
import SwiftUI

// サーバーからのJSONを受け取る型
struct ScheduleResponse: Codable {
    let status: String
    let schedules: [ScheduleItem]
}

struct ListView: View {
    @Binding var schedules: [ScheduleItem]
    
    var body: some View {
        List {
            ForEach(schedules) { item in
                VStack(alignment: .leading) {
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
        .onAppear {
            loadSchedules()
        }
    }
    
    func loadSchedules() {
        guard let url = URL(string: "http://133.17.158.63/server.php") else {
            return
        }
        
        URLSession.shared.dataTask(with: url) { data, response, error in
            if let error = error {
                print("取得失敗", error)
                return
            }
            
            guard let data = data else {
                print("データなし")
                return
            }
            
            DispatchQueue.main.async {
                do {
                    let decoded = try JSONDecoder().decode(ScheduleResponse.self, from: data)
                    self.schedules = decoded.schedules
                    print("取得成功")
                } catch {
                    print("JSON変換失敗", error)
                    if let text = String(data: data, encoding: .utf8) {
                        print("PHPからの返答:", text)
                    }
                }
            }
        }.resume()
    }
}
