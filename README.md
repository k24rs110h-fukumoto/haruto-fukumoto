# Haruto Fukumoto

九州産業大学理工学部情報科学科

主にUnityを用いたゲーム開発を中心に、Webアプリケーション、iOSアプリケーション、AIを活用したシステム開発に取り組んでいます。

ゲーム開発では、システム設計やプログラミングだけでなく、AI技術やWeb技術を組み合わせた新しい体験づくりにも興味を持っています。

---

## Skills

### Programming Languages

* C#
* Python
* Swift
* JavaScript
* TypeScript
* PHP
* SQL
* C
* HTML / CSS

### Frameworks & Technologies

* Unity
* SwiftUI
* Django
* Django REST Framework
* React
* Next.js
* FastAPI
* Firebase

### Tools

* Git / GitHub
* SQLite
* MySQL
* Xcode
* Visual Studio Code

---

# Projects

## Unity 3D Horror Game

現在制作中の3D心理ホラーゲームです。
プレイヤーは廃病院を探索しながら、散らばった記録やメモを集め、答えのない探索を行います。突然驚かせる演出ではなく、不安感や緊張感を徐々に高める「心理的な恐怖」を重視して設計しています。
また、今後は探索による情報収集とストーリー理解を中心に据え、プレイヤー自身が考察しながら進められる体験を目指しています。

### 主な実装

* 一人称視点操作
* インベントリシステム
* アイテム取得・使用
* ライトシステム
* イベントシステム
* サウンド演出

### 使用技術

Unity / C# / Git

https://github.com/user-attachments/assets/35d4c180-5921-4f70-879c-22c332c49ae2

https://github.com/user-attachments/assets/abded16a-fc51-4640-b012-25371947fb4d

---

## Unity 2D RPG

オリジナルの2DピクセルRPGを制作しています。
プレイヤーは魔法を学びながら世界を旅し、戦闘を通して成長していきます。単純なレベル上げだけでなく、魔法や装備の組み合わせによる戦略性を重視したゲームを目指しています。
現在は戦闘システムや敵AI、キャラクター育成機能を中心に開発を進めています。

### 主な実装

* ターン制バトル
* キャラクター育成
* 魔法システム
* インベントリ
* 敵AI
* 全データAssets管理（Scriptsの共通化）

### 使用技術

Unity / C# / Unity Sentis


https://github.com/user-attachments/assets/fc53e757-bab7-45c6-bd4b-070e32a9862b

https://github.com/user-attachments/assets/aed68bb3-ffae-4986-9a58-4121a16033f1


---

## Whiteout Survival Tools

ゲーム「Whiteout Survival」の非公式攻略支援Webアプリケーションです。
ゲーム内では建築や研究、装備強化に大量の素材計算が必要になります。実際にプレイする中で計算が煩雑だと感じたため、必要素材を自動計算できるツールとして開発しました。
プレイヤー目線で「本当に欲しい機能」を意識しながら制作しました。

### 主な機能

* 資源計算
* 英雄育成支援
* 強化素材計算
* 各種攻略ツール

### 使用技術

Next.js
Django REST Framework
Firebase

### Website

https://whiteout-survival-tools.vercel.app/

<p align="center">
<img src="https://github.com/user-attachments/assets/f0f12a08-8344-4d26-bfc6-6a4d796ce761" width="700">
</p>

---

## CO2 Alert System

教室や室内空間におけるCO2濃度を監視し、換気のタイミングを支援するiPhoneアプリケーションです。
修士の学生と共同で行なっているCO2濃度のAI予測研究のアプリケーションバージョンです。

### 主な機能

* CO2濃度の可視化
* 将来予測
* 換気支援
* 熱中症対策

### 使用技術

SwiftUI
Python
FastAPI
Machine Learning

<p align="center">
  <img src="https://github.com/user-attachments/assets/ee9137e6-2401-40a9-8e0e-197ca8d7c1d0" width="250">
  <img src="https://github.com/user-attachments/assets/52bfde85-b742-423d-b34a-120146f4e20b" width="250">
</p>

---

## Calendar Application

日々の予定管理を目的として開発したスケジュール管理アプリケーションです。
カレンダー表示だけでなく、予定の登録・編集・削除機能やデータベースへの保存機能を実装し、実際に継続利用できるアプリケーションを目指しました。
研究室での成果発表に使ったため自己紹介のTabも作成しました。

### 主な機能

* カレンダー表示
* 予定管理
* 通知機能
* データベース保存

### 使用技術

SwiftUI
MySQL
Apache

<p align="center">
  <img src="https://github.com/user-attachments/assets/588ce43a-de5e-464b-8e95-82ae9d150f5b" width="220">
  <img src="https://github.com/user-attachments/assets/30975a37-ee0d-482a-94ff-f4b5791fad84" width="220">
  <img src="https://github.com/user-attachments/assets/09900f57-6186-4c0c-a7bc-7ea06adb3881" width="220">
</p>

---

## Arduino Projects

授業を通して取り組んだ組込みシステム開発です。
LEDやセンサーを用いた制御プログラムの作成だけでなく、シリアル通信を利用したデータの送受信や、状態遷移を用いた制御システムの実装などを行いました。
ソフトウェアだけでなくハードウェアも含めて動作を設計することで、システム全体を考える力を身につけました。

### 主な内容

* LED制御
* センサー制御
* シリアル通信
* 各種制御システム
* Arduino実機操作

### 使用技術

Arduino
C

---

# Research

現在、「Unityゲーム開発支援システム」に関する研究を進めています。

ゲーム開発では、プロジェクトの大規模化に伴いスクリプト同士の依存関係が複雑になり、保守や機能追加が難しくなるという課題があります。

そこで、Unityプロジェクト内のスクリプトやコンポーネントの関係を解析し、開発者が構造を把握しやすくする支援システムの研究に取り組んでいます。


---

# GitHub

https://github.com/k24rs110h-fukumoto

# Contact

Email

[k24rs110@st.kyusan-u.ac.jp](mailto:k24rs110@st.kyusan-u.ac.jp)

[runoaima98@gmail.com](mailto:runoaima98@gmail.com)
