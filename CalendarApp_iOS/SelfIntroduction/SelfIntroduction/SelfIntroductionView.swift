//
//  SelfIntroductionView.swift
//  SelfIntroduction
//
//  Created by Haruto Fukumoto on 2026/04/27.
//


import SwiftUI

struct SelfIntroductionView: View {

    @State private var selectedPopup: PopupItem?

    var body: some View {
        ZStack {
            VStack() {

                VStack(alignment: .leading, spacing: 4) {
                    Text("自己紹介")
                        .font(.largeTitle)
                        .fontWeight(.bold)
                        .foregroundStyle(.accentfont)
                        .padding(.top)

                    Text("Self Introduction")
                        .font(.caption)
                        .foregroundColor(.blue)
                }
                .frame(maxWidth: .infinity, alignment: .leading)

                VStack {
                    HStack {
                        Image("Mrs.GreenApple")
                            .resizable()
                            .scaledToFit()
                            .frame(width: 110, height: 110)
                            .foregroundColor(.blue)
                            .padding()

                        VStack {
                            Text("福本 陽翔")
                                .font(.title)
                                .fontWeight(.bold)
                                .foregroundStyle(.black)

                            Text("徳山高校\n九州産業大学")
                                .font(.headline)
                                .foregroundStyle(.gray)
                                .lineLimit(nil)
                                .fixedSize(horizontal: false, vertical: true)
                                .multilineTextAlignment(.leading)
                        }
                    }

                    Text("よろしくお願いします!!!!!!")
                        .font(.body)
                        .padding()
                        .frame(maxWidth: .infinity)
                        .background(Color.accentColor.opacity(0.2))
                        .cornerRadius(18)
                }
                .padding(24)
                .background(Color.white)
                .cornerRadius(28)
                .shadow(color: .black.opacity(0.08), radius: 16, x: 0, y: 6)
                .onTapGesture {
                    selectedPopup = .name
                }

                HStack(spacing: 16) {

                    InfoCard(
                        imageName: "icon_game",
                        title: "趣味",
                        subtitle: "ゲーム\n音楽"
                    )
                    .onTapGesture {
                        selectedPopup = .hobby
                    }

                    InfoCard(
                        imageName: "icon_book",
                        title: "マイブーム",
                        subtitle: "温泉, サウナ"
                    )

                    InfoCard(
                        imageName: "icon_code",
                        title: "スキル",
                        subtitle: "Java,Python,HTML,Swift..."
                    )
                }

                VStack {
                    HStack {
                        Image("icon_target")
                            .resizable()
                            .scaledToFit()
                            .frame(width: 40, height: 40)

                        Text("今後の目標")
                            .font(.headline)
                            .fontWeight(.bold)
                            .foregroundStyle(.black)
                    }

                    Text("自分が満足できるまでアプリを作り続けれるエンジニアになる")
                        .font(.body)
                        .lineSpacing(6)
                        .foregroundStyle(.black)
                        .lineLimit(nil)
                        .fixedSize(horizontal: false, vertical: true)
                        .multilineTextAlignment(.leading)
                }
                .padding(22)
                .frame(maxWidth: .infinity, alignment: .leading)
                .background(Color.white)
                .cornerRadius(24)
                .shadow(color: .black.opacity(0.08), radius: 10, x: 0, y: 5)

                Spacer()
            }
            .padding(24)
        }
        .sheet(item: $selectedPopup) { item in
            DetailView(item: item)
        }
    }
}

struct InfoCard: View {
    let imageName: String
    let title: String
    let subtitle: String

    var body: some View {
        VStack(spacing: 12) {
            Image(imageName)
                .resizable()
                .scaledToFit()
                .frame(width: 40, height: 40)

            Text(title)
                .font(.headline)
                .fontWeight(.bold)
                .fixedSize(horizontal: true, vertical: false)

            Text(subtitle)
                .font(.subheadline)
                .foregroundStyle(.gray)
                .lineLimit(nil)
                .fixedSize(horizontal: false, vertical: true)
                .multilineTextAlignment(.center)
        }
        .padding()
        .frame(maxWidth: .infinity)
        .frame(height: 150)
        .background(Color.white)
        .cornerRadius(24)
        .shadow(color: .black.opacity(0.07), radius: 8, x: 0, y: 4)
    }
}

enum PopupItem: Identifiable {
    case name
    case hobby
    
    var id: String {
        switch self {
        case .name: return "name"
        case .hobby: return "hobby"
        }
    }

    var title: String {
        switch self {
        case .name: return "名前"
        case .hobby: return "趣味"
        }
    }

    var detail: String {
        switch self {
        case .name:
            return "福本 陽翔\n2005年9月15日 山口県周南市に3男1女の長男として生まれ\n櫛ヶ浜小学校で問題児として生活\n太華中学校卒業まで9年間野球部\n徳山高校進学\n九州産業大学理工学部情報科学科\n大企業の社畜"
        case .hobby:
            return "ゲーム\nVALORANT,スタレ,ポケモン,雀魂,Fortnite,広告ゲーム\n音楽\nMrs.GREENAPPLE,AI,米津玄師,King Gnu\n漫画\n呪術廻戦,DeathNote,東京喰種"
        }
    }
}

struct DetailView: View {
    let item: PopupItem

    var body: some View {
        VStack {
            Text(item.title)
                .font(.largeTitle)
                .fontWeight(.bold)
                .foregroundStyle(.black)

            Text(item.detail)
                .font(.body)
                .lineSpacing(8)
                .multilineTextAlignment(.leading)
                .frame(maxWidth: .infinity, alignment: .leading)

            Spacer()
        }
        .padding(30)
    }
}
