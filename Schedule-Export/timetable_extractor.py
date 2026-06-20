import re
import pdfplumber


DAYS = ["月", "火", "水", "木", "金"]
PERIODS = [1, 2, 3, 4, 5, 6]


def extract_timetable(pdf_path):
    timetable = []

    with pdfplumber.open(pdf_path) as pdf:
        for page_index, page in enumerate(pdf.pages):
            semester = "前期" if page_index == 0 else "後期"

            tables = page.extract_tables()
            if not tables:
                continue

            table = tables[0]

            day_row = table[0]
            period_row = table[1]
            time_row = table[2]

            current_grade = ""

            for row in table[3:]:
                if row[0]:
                    current_grade = row[0].replace("\n", "")

                for col_index in range(1, len(row)):
                    cell = row[col_index]

                    if not cell:
                        continue

                    day = get_day(day_row, col_index)
                    period = get_period(period_row, col_index)
                    time_range = get_time(time_row, col_index)

                    if day is None or period is None:
                        continue

                    parsed = parse_cell(cell)

                    if parsed is None:
                        continue

                    parsed["semester"] = semester
                    parsed["grade"] = current_grade
                    parsed["day"] = day
                    parsed["period"] = period
                    parsed["time"] = time_range

                    timetable.append(parsed)

    return timetable


def get_day(day_row, col_index):
    day = None

    for i in range(1, col_index + 1):
        if day_row[i] in DAYS:
            day = day_row[i]

    return day


def get_period(period_row, col_index):
    value = period_row[col_index]

    if value is None:
        return None

    try:
        return int(value)
    except ValueError:
        return None


def get_time(time_row, col_index):
    value = time_row[col_index]

    if value is None:
        return ""

    return value.replace("：", ":")


def parse_cell(cell):
    lines = [line.strip() for line in cell.split("\n") if line.strip()]

    room = find_room(lines)

    if room is None:
        return None

    course_lines = []
    class_code = ""
    teacher = ""
    target = ""

    for line in lines:
        code_match = re.search(r"\[\d{3}\]", line)

        if code_match:
            class_code = code_match.group()
            teacher = line.replace(class_code, "").strip()
            continue

        if line == room:
            continue

        if is_room_line(line):
            continue

        if is_target_line(line):
            target = line
            continue

        if is_category_line(line):
            continue

        course_lines.append(line)

    course_name = "\n".join(course_lines)

    return {
        "course_name": course_name,
        "class_code": class_code,
        "teacher": teacher,
        "room": room,
        "target": target
    }


def find_room(lines):
    for line in lines:
        if is_room_line(line):
            return line

    return None


def is_room_line(line):
    if re.fullmatch(r"\d{5}", line):
        return True

    if re.fullmatch(r"\d{5}(・\d{5})+", line):
        return True

    if "教室" in line:
        return True

    if re.fullmatch(r"[A-Z]\d{3,4}", line):
        return True

    if re.fullmatch(r"\d[A-Z]\d{3}", line):
        return True

    return False


def is_target_line(line):
    keywords = ["RS", "以降", "のみ", "学生", "高校生"]

    return any(keyword in line for keyword in keywords)


def is_category_line(line):
    categories = [
        "全", "①", "②", "③", "④", "⑤", "⑥", "⑦",
        "a", "b", "c", "ア", "イ", "ウ"
    ]

    return line in categories


def build_room_timetable(timetable):
    rooms = {}

    for item in timetable:
        semester = item["semester"]
        room = item["room"]
        day = item["day"]
        period = item["period"]

        if room not in rooms:
            rooms[room] = {}

        if semester not in rooms[room]:
            rooms[room][semester] = {}

        if day not in rooms[room][semester]:
            rooms[room][semester][day] = {}

        rooms[room][semester][day][period] = item

    return rooms


def print_room_timetable(room_timetable):
    for room, semesters in room_timetable.items():
        print("\n")
        print("=" * 70)
        print(f"教室: {room}")
        print("=" * 70)

        for semester in ["前期", "後期"]:
            if semester not in semesters:
                continue

            print(f"\n--- {semester} ---")

            for day in DAYS:
                print(f"\n{day}曜日")

                for period in PERIODS:
                    item = semesters[semester].get(day, {}).get(period)

                    if item is None:
                        print(f"{period}限: -")
                    else:
                        print(
                            f"{period}限: "
                            f"{item['course_name']} / "
                            f"{item['teacher']} / "
                            f"{item['grade']} / "
                            f"{item['time']}"
                        )