import Foundation

final class CO2APIService {
    static let shared = CO2APIService()

    private init() {}

    func fetchLocations() async throws -> [String] {
        let url = try makeURL(path: "/locations.php")
        let response: LocationResponse = try await fetch(url: url)
        return response.locations
    }

    func fetchPrediction(location: String) async throws -> PredictionResponse {
        let encodedLocation = location.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? location
        let url = try makeURL(path: "/predict.php?location=\(encodedLocation)")
        return try await fetch(url: url)
    }

    func fetchCO2Data(location: String, period: Int = 24) async throws -> [CO2DataItem] {
        let encodedLocation = location.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? location
        let url = try makeURL(path: "/data.php?location=\(encodedLocation)&period=\(period)")
        return try await fetch(url: url)
    }

    private func makeURL(path: String) throws -> URL {
        guard let url = URL(string: AppConfig.apiBaseURL + path) else {
            throw CO2APIError.invalidURL
        }

        return url
    }

    private func fetch<T: Decodable>(url: URL) async throws -> T {
        print("API URL:", url.absoluteString)

        let (data, response) = try await URLSession.shared.data(from: url)

        guard let httpResponse = response as? HTTPURLResponse else {
            throw CO2APIError.invalidResponse
        }

        guard 200..<300 ~= httpResponse.statusCode else {
            throw CO2APIError.serverError(httpResponse.statusCode)
        }

        do {
            return try JSONDecoder().decode(T.self, from: data)
        } catch {
            let rawText = String(data: data, encoding: .utf8) ?? "文字列に変換できません"

            print("===== JSON Decode Error =====")
            print(error)
            print(rawText)
            print("=============================")

            throw CO2APIError.decodeError(error.localizedDescription)
        }
    }
}

enum CO2APIError: LocalizedError {
    case invalidURL
    case invalidResponse
    case serverError(Int)
    case decodeError(String)

    var errorDescription: String? {
        switch self {
        case .invalidURL:
            return "URLが正しくありません。"
        case .invalidResponse:
            return "サーバー応答が正しくありません。"
        case .serverError(let code):
            return "サーバーエラー: \(code)"
        case .decodeError(let message):
            return "データ変換エラー: \(message)"
        }
    }
}
