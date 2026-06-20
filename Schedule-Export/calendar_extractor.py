import re
import pdfplumber
from datetime import date


WEEKDAYS = ["月", "火", "水", "木", "金", "土", "日"]


def extract_calendar_days(pdf_path):
    text = read_calendar_text(pdf_path)

    calendar_days = []

    # 今回は学年暦PDFの構造が複雑なので、
    # まずは授業期間をコード側で定義して授業実施日を作る。
    # あとでPDFから完全自動抽出に変える予定。
    calendar_days += make_weekday_lessons(
        start_date=date(2026, 4, 7),
        end_date=date(2026, 5, 28),
        quarter="第1Q"
    )

    calendar_days += make_weekday_lessons(
        start_date=date(2026, 6, 4),
        end_date=date(2026, 7, 22),
        quarter="第2Q"
    )

    calendar_days += make_weekday_lessons(
        start_date=date(2026, 9, 14),
        end_date=date(2026, 11, 6),
        quarter="第3Q"
    )

    calendar_days += make_weekday_lessons(
        start_date=date(2026, 11, 13),
        end_date=date(2027, 1, 18),
        quarter="第4Q"
    )

    calendar_days = apply_special_days(calendar_days)

    return calendar_days


def read_calendar_text(pdf_path):
    text = ""

    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            page_text = page.extract_text()

            if page_text:
                text += page_text + "\n"

    return text


def make_weekday_lessons(start_date, end_date, quarter):
    result = []

    lesson_counter = {
        "月": 0,
        "火": 0,
        "水": 0,
        "木": 0,
        "金": 0
    }

    current = start_date

    while current <= end_date:
        day_name = get_japanese_weekday(current)

        if day_name in lesson_counter:
            lesson_counter[day_name] += 1

            result.append({
                "date": current.isoformat(),
                "day": day_name,
                "lesson_count": lesson_counter[day_name],
                "quarter": quarter,
                "is_class_day": True,
                "note": ""
            })

        current = date.fromordinal(current.toordinal() + 1)

    return result


def apply_special_days(calendar_days):
    # 学年暦にある祝日授業実施日・振替休業日を反映する
    remove_dates = [
        "2026-07-29",
        "2026-07-30",
        "2026-12-25",
        "2027-01-06",
        "2027-01-15"
    ]

    special_class_days = [
        {
            "date": "2026-04-29",
            "day": "水",
            "lesson_day": "水",
            "note": "水曜授業実施日"
        },
        {
            "date": "2026-07-20",
            "day": "月",
            "lesson_day": "月",
            "note": "月曜授業実施日"
        },
        {
            "date": "2026-10-12",
            "day": "月",
            "lesson_day": "月",
            "note": "月曜授業実施日"
        },
        {
            "date": "2026-11-03",
            "day": "火",
            "lesson_day": "火",
            "note": "火曜授業実施日"
        },
        {
            "date": "2026-11-23",
            "day": "月",
            "lesson_day": "月",
            "note": "月曜授業実施日"
        },
        {
            "date": "2026-05-28",
            "day": "木",
            "lesson_day": "月",
            "note": "月曜授業実施日"
        },
        {
            "date": "2026-11-05",
            "day": "木",
            "lesson_day": "月",
            "note": "月曜授業実施日"
        }
    ]

    calendar_days = [
        item for item in calendar_days
        if item["date"] not in remove_dates
    ]

    for special in special_class_days:
        existing = find_by_date(calendar_days, special["date"])

        if existing:
            existing["day"] = special["lesson_day"]
            existing["actual_weekday"] = special["day"]
            existing["note"] = special["note"]
        else:
            calendar_days.append({
                "date": special["date"],
                "day": special["lesson_day"],
                "actual_weekday": special["day"],
                "lesson_count": None,
                "quarter": detect_quarter(special["date"]),
                "is_class_day": True,
                "note": special["note"]
            })

    calendar_days.sort(key=lambda x: x["date"])

    calendar_days = recount_lesson_numbers(calendar_days)

    return calendar_days


def recount_lesson_numbers(calendar_days):
    counters = {}

    for item in calendar_days:
        quarter = item["quarter"]
        day = item["day"]

        key = f"{quarter}_{day}"

        if key not in counters:
            counters[key] = 0

        counters[key] += 1
        item["lesson_count"] = counters[key]

    return calendar_days


def find_by_date(calendar_days, target_date):
    for item in calendar_days:
        if item["date"] == target_date:
            return item

    return None


def detect_quarter(date_text):
    if "2026-04-01" <= date_text <= "2026-05-28":
        return "第1Q"

    if "2026-06-04" <= date_text <= "2026-07-22":
        return "第2Q"

    if "2026-09-14" <= date_text <= "2026-11-06":
        return "第3Q"

    if "2026-11-13" <= date_text <= "2027-01-18":
        return "第4Q"

    return ""


def get_japanese_weekday(target_date):
    # Python: 月曜=0, 日曜=6
    index = target_date.weekday()
    return WEEKDAYS[index]


def print_calendar_days(calendar_days):
    print("\n")
    print("=" * 70)
    print("授業実施日")
    print("=" * 70)

    for item in calendar_days:
        actual = item.get("actual_weekday", item["day"])

        print(
            f"{item['date']} "
            f"({actual}) "
            f"{item['quarter']} "
            f"{item['day']}曜授業 "
            f"{item['lesson_count']}回目 "
            f"{item['note']}"
        )